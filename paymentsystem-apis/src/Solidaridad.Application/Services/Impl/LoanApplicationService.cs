using AutoMapper;
using Dapper;
using LinqToDB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Npgsql;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Quartz.Util;
using Solidaridad.Application.Helpers;
using Solidaridad.Application.Models;
using Solidaridad.Application.Models.ActivityLog;
using Solidaridad.Application.Models.ApplicationStatusLog;
using Solidaridad.Application.Models.AttachmentUpload;
using Solidaridad.Application.Models.EmiSchedule;
using Solidaridad.Application.Models.Farmer;
using Solidaridad.Application.Models.LoanApplication;
using Solidaridad.Application.Models.PaymentBatch;
using Solidaridad.Core.Entities;
using Solidaridad.Core.Entities.Base;
using Solidaridad.Core.Entities.Excel.Import;
using Solidaridad.Core.Entities.Loans;
using Solidaridad.DataAccess.Identity;
using Solidaridad.DataAccess.Migrations;
using Solidaridad.DataAccess.Repositories;
using Solidaridad.Shared.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;


namespace Solidaridad.Application.Services.Impl;

public class LoanApplicationService : BaseService, ILoanApplicationService
{
    #region DI
    private readonly IMapper _mapper;
    private readonly ILoanApplicationRepository _loanApplicationRepository;
    private readonly ILoanRepaymentService _loanRepaymentService;
    private readonly ILoanApplicationImportStagingRepository _loanApplicationImportStagingRepository;
    private readonly ILoanBatchRepository _loanBatchRepository;
    private readonly IClaimService _claimService;
    private readonly ILoanBatchService _loanBatchService;
    private readonly IFarmerRepository _farmerRepository;
    private readonly ICountryRepository _countryRepository;
    private readonly IMasterLoanItemRepository _masterLoanItemRepository;
    private readonly ILoanBatchItemRepository _loanBatchItemRepository;
    private readonly IExcelImportDetailRepository _excelImportDetailRepository;
    private readonly ILoanItemRepository _loanItemRepository;
    private readonly ILoanItemImportStagingRepository _loanItemImportStagingRepository;
    private readonly IAttachmentMappingRepository _attachmentMappingRepository;
    private readonly IAttachmentUploadRepository _attachmentUploadRepository;
    private readonly IApplicationStatusLogRepository _applicationStatusLogRepository;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly string _connectionString;
    private readonly ILoanRepaymentRepository _loanRepaymentRepository;
    private readonly IConfiguration _configuration;
    private readonly IEMIScheduleRepository _eMIScheduleRepository;
    private readonly IItemUnitRepository _itemUnitRepository;
    private readonly IUserService _userService;
    private readonly IExcelImportRepository _excelImportRepository;
    private readonly IApplicationStatusRepository _applicationStatusRepository;

    const string DISBURSED = "e24d24a8-fc69-4527-a92a-97f6648a43c5";

    public LoanApplicationService(IMapper mapper, ICountryRepository countryRepository, ILoanRepaymentService loanRepaymentService,
        IApplicationStatusRepository applicationStatusRepository,
        IExcelImportRepository excelImportRepository,
        ILoanApplicationRepository loanApplicationRepository,
        IMasterLoanItemRepository masterLoanItemRepository,
        ILoanItemRepository loanItemRepository,
        ILoanBatchItemRepository loanBatchItemRepository,
        ILoanRepaymentRepository loanRepaymentRepository,
        ILoanBatchRepository loanBatchRepository,
        ILoanApplicationImportStagingRepository loanApplicationImportStagingRepository,
        IClaimService claimService,
        ILoanBatchService loanBatchService,
        IFarmerRepository farmerRepository, IConfiguration configuration,
        IAttachmentUploadRepository attachmentUploadRepository,
        IAttachmentMappingRepository attachmentMappingRepository,
        IExcelImportDetailRepository excelImportDetailRepository,
        IApplicationStatusLogRepository applicationStatusLogRepository,
        ILoanItemImportStagingRepository loanItemImportStagingRepository,
        IEMIScheduleRepository eMIScheduleRepository,
        IItemUnitRepository itemUnitRepository,
        IActivityLogService activityLogService,
        IUserService userService,
         UserManager<ApplicationUser> userManager) : base(activityLogService)
    {
        _claimService = claimService;
        _loanBatchService = loanBatchService;
        _mapper = mapper;
        _loanApplicationRepository = loanApplicationRepository;
        _loanBatchRepository = loanBatchRepository;
        _farmerRepository = farmerRepository;
        _configuration = configuration;
        _connectionString = _configuration.GetSection("ConnectionStrings").Get<DatabaseConfiguration>().DefaultConnection;
        _countryRepository = countryRepository;
        _masterLoanItemRepository = masterLoanItemRepository;
        _loanItemRepository = loanItemRepository;
        _attachmentMappingRepository = attachmentMappingRepository;
        _attachmentUploadRepository = attachmentUploadRepository;
        _applicationStatusLogRepository = applicationStatusLogRepository;
        _userManager = userManager;
        _excelImportDetailRepository = excelImportDetailRepository;
        _loanRepaymentRepository = loanRepaymentRepository;
        _eMIScheduleRepository = eMIScheduleRepository;
        _loanBatchItemRepository = loanBatchItemRepository;
        _itemUnitRepository = itemUnitRepository;
        _excelImportRepository = excelImportRepository;
        _loanItemImportStagingRepository = loanItemImportStagingRepository;
        _loanApplicationImportStagingRepository = loanApplicationImportStagingRepository;
        _applicationStatusRepository = applicationStatusRepository;
        _userService = userService;
        _loanRepaymentService = loanRepaymentService;
    }
    #endregion

    public async Task<CreateLoanApplicationResponseModel> CreateAsync(
        CreateLoanApplicationModel loanApplicationModel)
    {

        var applicationStatusMaster = await _applicationStatusRepository.GetFirstAsync(c => c.Name == "Draft");

        var loanApplication = _mapper.Map<LoanApplication>(loanApplicationModel);

        loanApplication.Id = Guid.NewGuid();
        loanApplication.LoanNumber = LoanNumberGenerator.GenerateLoanNumber("KE");
        loanApplication.CreatedBy = Guid.Parse(_claimService.GetUserId());
        loanApplication.CreatedOn = DateTime.UtcNow;
        loanApplication.Status = applicationStatusMaster != null ? applicationStatusMaster.Id : Guid.Parse("6f103a88-8443-45ad-9c37-afe07f6b48e1");
        loanApplication.OfficerId = !string.IsNullOrWhiteSpace(loanApplicationModel.OfficerId) && Guid.TryParse(loanApplicationModel.OfficerId, out var officerId)
     ? officerId
     : Guid.Empty;

        var loanBatch = await _loanBatchService.GetSingle(loanApplication.LoanBatchId);
        var existingLoanApplications = await _loanApplicationRepository.GetAllAsync(
            c => c.LoanBatchId == loanApplication.LoanBatchId
            && loanApplication.FarmerId == c.FarmerId
            && c.IsDeleted == false
        );
        if (existingLoanApplications != null && existingLoanApplications.Any())
        {
            return new CreateLoanApplicationResponseModel
            {
                Id = Guid.Empty,
                Message = "existing application"
            };

        }

        if (loanBatch != null)
        {
            decimal feeApplied = 0;
            foreach (var fee in loanBatch.ProcessingFees)
            {
                if (fee.FeeType.Trim().ToLower() == "flat")
                {
                    feeApplied += fee.Value;
                }
                else
                {
                    feeApplied += loanApplication.PrincipalAmount * (fee.Value / 100);
                }
            }
            loanApplication.FeeApplied = feeApplied;
            loanApplication.PrincipalAmount = loanApplication.PrincipalAmount + feeApplied;
            loanApplication.EffectivePrincipal = loanApplication.PrincipalAmount;

            var annualInteresrRate = loanBatch.CalculationTimeframe == "Monthly" ? loanBatch.InterestRate * loanBatch.Tenure :
                loanBatch.InterestRate;

            loanApplication.InterestRate = annualInteresrRate;
            loanApplication.InterestAmount = loanApplication.PrincipalAmount * (annualInteresrRate / 100);
            loanApplication.RemainingBalance = loanApplication.PrincipalAmount + loanApplication.InterestAmount;
        }

        var addedLoanApplication = await _loanApplicationRepository.AddAsync(loanApplication);
        var units = await _loanBatchService.GetItemUnitsAsync();
        var masterItems = await _masterLoanItemRepository.GetAllAsync(c => 1 == 1);

        if (addedLoanApplication != null)
        {
            var loanItems = new List<LoanItem>();

            foreach (var item in loanApplicationModel.LoanItems)
            {
                int unitId = 0;
                var unit = units.FirstOrDefault(u => u.Label.ToLower().Equals(item.Unit.ToLower()));
                if (unit != null)
                {
                    unitId = Convert.ToInt32(unit.Value);
                }
                loanItems.Add(new LoanItem
                {
                    ItemName = item.ItemName,
                    MasterLoanItemId = masterItems.FirstOrDefault(c => c.ItemName.ToLower() == item.ItemName.ToLower()).Id,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    CategoryId = masterItems.FirstOrDefault(c => c.ItemName.ToLower() == item.ItemName.ToLower()).CategoryId,
                    UnitId = unitId,
                    LoanApplicationId = addedLoanApplication.Id,
                });
            }
            await _loanItemRepository.AddRange(loanItems);
            if (loanApplicationModel.AttachmentIds != null)
            {
                foreach (var id in loanApplicationModel.AttachmentIds)
                {
                    var map = new AttachmentMapping
                    {
                        LoanApplicationId = addedLoanApplication.Id,
                        AttachmentId = id
                    };
                    await _attachmentMappingRepository.AddAsync(map);
                }
            }
            var applicationStatusLog = new ApplicationStatusLog
            {
                Id = new Guid(),
                ApplicationId = addedLoanApplication.Id,
                StatusId = addedLoanApplication.Status,
                Comments = "Drafted application",
                CreatedBy = new Guid(_claimService.GetUserId()),
                CreatedOn = DateTime.UtcNow
            };
            var log = await _applicationStatusLogRepository.AddAsync(applicationStatusLog);
            // generate EMI schedule
            await GenerateEMISchedule(loanApplicationId: addedLoanApplication.Id);
        }

        return new CreateLoanApplicationResponseModel
        {
            Id = addedLoanApplication.Id
        };
    }

    //public async Task<ImportLoanApplicationResponseModel> ImportAsync(ImportLoanApplicationModel loanApplicationModel)
    //{
    //    var loanApplication = _mapper.Map<LoanApplication>(loanApplicationModel);
    //    loanApplication.Id = Guid.NewGuid();

    //    var farmers = await _farmerRepository.GetAllAsync(c => c.SystemId == loanApplicationModel.SystemId);
    //    if (farmers != null && farmers.Any())
    //    {
    //        loanApplication.FarmerId = farmers[0].Id;
    //    }
    //    var addedLoanApplication = await _loanApplicationRepository.AddAsync(loanApplication);

    //    // add items
    //    var units = await _loanBatchService.GetItemUnitsAsync();
    //    var masterItems = await _masterLoanItemRepository.GetAllAsync(c => 1 == 1);

    //    if (addedLoanApplication != null)
    //    {
    //        var loanItems = new List<LoanItem>();
    //        foreach (var item in loanApplicationModel.LoanItems)
    //        {
    //            int unitId = 0;
    //            var unit = units.FirstOrDefault(u => u.Label.ToLower().Equals(item.Unit.ToLower()));
    //            if (unit != null)
    //            {
    //                unitId = Convert.ToInt32(unit.Value);
    //            }
    //            loanItems.Add(new LoanItem
    //            {
    //                ItemName = item.ItemName,
    //                MasterLoanItemId = masterItems.FirstOrDefault(c => c.ItemName.ToLower() == item.ItemName.ToLower()).Id,
    //                Quantity = item.Quantity,
    //                UnitPrice = item.UnitPrice,
    //                CategoryId = masterItems.FirstOrDefault(c => c.ItemName.ToLower() == item.ItemName.ToLower()).CategoryId,
    //                UnitId = unitId,
    //                LoanApplicationId = addedLoanApplication.Id,
    //            });
    //        }
    //        await _loanItemRepository.AddRange(loanItems);
    //    }

    //    return new ImportLoanApplicationResponseModel
    //    {
    //        Id = addedLoanApplication.Id
    //    };
    //}

    public async Task<ImportLoanApplicationResponseModel> ImportAsync(ImportLoanApplicationModel loanApplicationModel)
    {
        var response = new ImportLoanApplicationResponseModel();

        // Preload reference data
        var units = await _loanBatchService.GetItemUnitsAsync();
        var loanBatch = await _loanBatchRepository.GetFirstAsync(c => c.Id == loanApplicationModel.LoanBatchId);
        var masterItems = await _masterLoanItemRepository.GetAllAsync(c => 1 == 1);
        var loanBatchItems = await _loanBatchItemRepository.GetAllAsync(c => c.LoanBatchId == loanApplicationModel.LoanBatchId);

        // Enrich loanBatchItems with actual ItemName via LoanItem
        var enrichedBatchItems = loanBatchItems
            .Join(masterItems,
                  batch => batch.LoanItemId,
                  loanItem => loanItem.Id,
                  (batch, loanItem) => new
                  {
                      BatchItem = batch,
                      ItemName = loanItem.ItemName,
                      UnitId = batch.UnitId,
                      UnitPrice = batch.UnitPrice,
                      AvailableQuantity = batch.Quantity,
                      MasterLoanItemId = loanItem.Id
                  })
            .ToList();

        var loanItems = new List<LoanItem>();
        var errors = new List<string>();

        if (loanBatch.StatusId != 3)
        {
            errors.Add("This loan batch cannot accept applications");
        }

        // validate if loan batch has items
        if (!loanBatchItems.Any() || loanBatchItems.Count == 0)
        {
            errors.Add("Please add items to loan batch first");
        }

        // Validate loan items
        foreach (var item in loanApplicationModel.LoanItems)
        {
            var matchedMasterItem = masterItems.FirstOrDefault(c => c.ItemName.ToLower() == item.ItemName.ToLower());
            var matchedUnit = units.FirstOrDefault(u => u.Attrib1.ToLower() == item.Unit.ToLower()); //Attrib1 to compare Abbreviation instead of Name
            var batchItem = enrichedBatchItems.FirstOrDefault(b => b.ItemName == item.ItemName);

            if (string.IsNullOrWhiteSpace(item.ItemName))
            {
                errors.Add("Item name is not provided");
            }

            if (batchItem == null)
            {
                errors.Add("Item name not found in Loan Batch Items");
            }

            if (matchedMasterItem == null)
            {
                errors.Add($"Master item not found: {item.ItemName}");
            }

            if (matchedUnit == null)
            {
                errors.Add($"Unit not found: {item.Unit} for item {item.ItemName}");
            }

            if (batchItem.UnitPrice != item.UnitPrice)
            {
                errors.Add($"Unit Price mismatch. Expected {batchItem.UnitPrice}, got {item.UnitPrice}");
            }

            if (item.Quantity > batchItem.AvailableQuantity)
            {
                errors.Add($"Quantity exceeds batch limit. Allowed: {batchItem.AvailableQuantity}, Provided: {item.Quantity}");
            }

            if (item.UnitPrice < 0)
            {
                errors.Add("Price Per Unit must be a valid number");
            }

            if (item.Quantity < 0)
            {
                errors.Add("Quantity must be a valid number");
            }

            if (string.IsNullOrWhiteSpace(item.Unit))
            {
                errors.Add("Unit not provided");
            }

            if (matchedMasterItem != null && matchedUnit != null)
            {
                loanItems.Add(new LoanItem
                {
                    ItemName = item.ItemName,
                    MasterLoanItemId = matchedMasterItem.Id,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    CategoryId = matchedMasterItem.CategoryId,
                    UnitId = Convert.ToInt32(matchedUnit.Value),
                    // LoanApplicationId will be set after app save
                });
            }
        }

        // Validate loan application
        if (loanApplicationModel.SystemId == string.Empty)
        {
            errors.Add("Farmer with current System Id does not exist");
        }
        if (loanApplicationModel.WitnessFullName == null || loanApplicationModel.WitnessFullName == "")
        {
            errors.Add("Witness name not provided");
        }
        else if (loanApplicationModel.WitnessRelation == null || loanApplicationModel.WitnessRelation == "")
        {
            errors.Add("Witness relation not provided");
        }
        else if (loanApplicationModel.WitnessPhoneNo == null || loanApplicationModel.WitnessPhoneNo == "")
        {
            errors.Add("Witness phone number not provided");
        }
        else if (loanApplicationModel.WitnessNationalId == null || loanApplicationModel.WitnessNationalId == "")
        {
            errors.Add("Witness national id not provided");
        }

        if (errors.Any())
        {
            response.Success = false;
            response.Errors = errors;
            return response;
        }

        // All items are valid - proceed to save
        var loanApplication = _mapper.Map<LoanApplication>(loanApplicationModel);
        loanApplication.Id = Guid.NewGuid();

        var farmers = await _farmerRepository.GetAllAsync(c => c.SystemId == loanApplicationModel.SystemId);
        if (farmers != null && farmers.Any())
        {
            loanApplication.FarmerId = farmers[0].Id;
        }

        // update PrincipalAmount from items
        foreach (var loanItem in loanItems)
        {
            decimal principalAmount = 0;
            principalAmount += (decimal)(loanItem.UnitPrice * loanItem.Quantity);
            loanApplication.PrincipalAmount = principalAmount;
        }

        // add loan application
        loanApplication.LoanNumber = LoanNumberGenerator.GenerateLoanNumber("KE");
        var addedLoanApplication = await _loanApplicationRepository.AddAsync(loanApplication);

        // add loan items
        foreach (var loanItem in loanItems)
        {
            loanItem.LoanApplicationId = addedLoanApplication.Id;
        }

        await _loanItemRepository.AddRange(loanItems);

        response.Id = addedLoanApplication.Id;
        response.Success = true;

        return response;
    }

    public async Task<BaseResponseModel> DeleteAsync(Guid id)
    {
        var project = await _loanApplicationRepository.GetFirstAsync(tl => tl.Id == id);
        project.IsDeleted = true;
        return new BaseResponseModel
        {
            Id = (await _loanApplicationRepository.UpdateAsync(project)).Id
        };
    }

    public async Task<IEnumerable<LoanApplicationResponseModel>> GetAllAsync(LoanApplicationSearchParams searchParams)
    {
        IEnumerable<LoanApplication> _loanApplications;

        if (searchParams.BatchId != null)
        {
            _loanApplications = await _loanApplicationRepository.GetAllAsync(
                c => c.LoanBatchId == searchParams.BatchId && c.IsDeleted == false
            );
        }
        else
        {
            _loanApplications = await _loanApplicationRepository.GetAllAsync(
                c => string.IsNullOrEmpty(searchParams.Filter)
            );
        }
        int numberOfObjectsPerPage = searchParams.PageSize;
        var queryResult = _loanApplications
          .Skip(numberOfObjectsPerPage * (searchParams.PageNumber - 1)).Take(numberOfObjectsPerPage);

        var loanApplications = _mapper.Map<ReadOnlyCollection<LoanApplicationResponseModel>>(queryResult);
        var farmerList = await _farmerRepository.GetAllAsync(c => c.Id == c.Id);
        var applicationStatusList = await _applicationStatusRepository.GetAllAsync(c => true);
        var loanBatchesList = await _loanBatchService.GetAllAsync(new LoanBatchSearchParams { PageNumber = 1, PageSize = 100000, CountryId = searchParams.CountryId });

        foreach (var loanApplication in loanApplications)
        {
            // farmer
            var farmers = farmerList.ToList().Where(c => c.Id == loanApplication.FarmerId);
            if (farmers != null && farmers.Any())
            {
                loanApplication.Farmer = _mapper.Map<FarmerResponseModel>(farmers.FirstOrDefault());
            }

            //loan batches
            var loanBatches = loanBatchesList.LoanBatches.ToList().Where(c => c.Id == loanApplication.LoanBatchId);
            if (loanBatches != null && loanBatches.Any())
            {
                loanApplication.LoanBatch = _mapper.Map<LoanBatchResponseModel>(loanBatches.FirstOrDefault());
            }

            // loan items
            var applItems = _mapper.Map<List<LoanAppItemImportModel>>(await _loanItemRepository.GetAllAsync(c => c.LoanApplicationId == loanApplication.Id));

            // Retrieve units
            var units = await GetItemUnitsAsync();

            // Set units for each loan item
            applItems.ForEach(item =>
            {
                var unit = units.FirstOrDefault(u => u.Value == Convert.ToString(item.UnitId));
                item.Unit = unit.Label;
            });

            // Assign updated items to the loan application
            loanApplication.LoanItems = applItems;

            foreach (var item in applItems)
            {
                loanApplication.TotalValue = loanApplication.TotalValue + (float)(item.Quantity * item.UnitPrice);
            }

            var applicationStatusLog = (await _applicationStatusLogRepository.GetAllAsync(c => c.ApplicationId == loanApplication.Id)).FirstOrDefault();

            if (applicationStatusLog != null)
            {
                var users = await _userManager.Users.ToListAsync();
                var creatorUser = users.FirstOrDefault(user => user.Id == applicationStatusLog.CreatedBy.ToString());
                IList<string> userRoles = new List<string>();

                if (creatorUser?.Id != null && Guid.TryParse(creatorUser.Id, out var creatorUserId) && creatorUserId != Guid.Empty)
                {
                    var user = await _userManager.FindByIdAsync(creatorUser.Id.ToString());
                    if (user != null)
                    {
                        userRoles = await _userManager.GetRolesAsync(user);
                    }
                }


                loanApplication.ModeratorRole = string.Join(" ", userRoles);
                if (creatorUser != null)
                {
                    loanApplication.Moderator = creatorUser.UserName;
                }
                else
                {
                    loanApplication.Moderator = "Unknown";
                }

            }

            var paymentHistory = _loanRepaymentRepository.GetRepaymentHistory((Guid)loanApplication.Id);
            if (paymentHistory != null && paymentHistory.Count > 0)
            {
                loanApplication.InUse = true;
            }

            var _appStatus = applicationStatusList.FirstOrDefault(c => c.Id == loanApplication.Status);
            if (_appStatus != null)
            {
                loanApplication.StatusText = _appStatus.Name;
            }
            else
            {
                loanApplication.StatusText = "Draft";
            }
        }

        return _mapper.Map<IEnumerable<LoanApplicationResponseModel>>(loanApplications);
    }

    public async Task<LoanApplicationResponseModel> GetSingleAsync(Guid id)
    {
        var _loanApplication = await _loanApplicationRepository.GetFirstAsync(c => c.Id == id);

        return _mapper.Map<LoanApplicationResponseModel>(_loanApplication);
    }

    public async Task<IEnumerable<SelectItemModel>> GetItemUnitsAsync()
    {
        try
        {
            using (var connection = GetConnection())
            {
                string sql = string.Format("select \"Id\" as Value, \"Name\" as Label from public.\"ItemUnit\"");
                return await connection.QueryAsync<SelectItemModel>(sql);
            }
        }
        catch (Exception)
        {
            return new SelectItemModel[] { };
        }

    }

    public async Task<LoanApplicationResponseModel> GetApplicationDocuments(Guid id, CancellationToken cancellationToken = default)
    {
        var loanApplication = await _loanApplicationRepository.GetFirstAsync(ti => ti.Id == id);

        var single = _mapper.Map<LoanApplicationResponseModel>(loanApplication);
        if (single != null)
        {
            var attachmentMap = await _attachmentMappingRepository.GetAllAsync(c => c.LoanApplicationId == single.Id);
            var attachmentFiles = await _attachmentUploadRepository.GetAllAsync(c => 1 == 1);

            var data = from attachment in attachmentMap
                       join attFile in attachmentFiles
                       on attachment.AttachmentId equals attFile.Id
                       select new AttachmentResponseModel
                       {
                           Id = attachment.AttachmentId,
                           ImagePath = attFile.ImagePath,
                           ThumbPath = attFile.ThumbPath,
                           FileName = attFile.FileName,
                           ContentType = attFile.ContentType,
                           FileSize = attFile.FileSize
                       };
            single.AttachmentFiles = data.ToList();
        }

        return single;
    }

    public async Task<UpdateLoanApplicationResponseModel> UpdateAsync(Guid id, UpdateLoanApplicationModel loanApplicationModel)
    {
        var loanApplication = await _loanApplicationRepository.GetFirstAsync(ti => ti.Id == id);

        _mapper.Map(loanApplicationModel, loanApplication);
        loanApplication.OfficerId = !string.IsNullOrWhiteSpace(loanApplicationModel.OfficerId) && Guid.TryParse(loanApplicationModel.OfficerId, out var officerId)
    ? officerId
    : Guid.Empty;
        var updateLoanApplication = await _loanApplicationRepository.UpdateAsync(loanApplication);

        var units = await _loanBatchService.GetItemUnitsAsync();
        var masterItems = await _masterLoanItemRepository.GetAllAsync(c => 1 == 1);
        if (updateLoanApplication != null)
        {
            // Step 1: Delete existing loan items
            var existingLoanItems = await _loanItemRepository.GetAllAsync(li => li.LoanApplicationId == updateLoanApplication.Id);
            if (existingLoanItems.Any())
            {
                foreach (var item in existingLoanItems)
                {
                    await _loanItemRepository.DeleteAsync(item);
                }
            }

            // Step 2: Add new loan items
            var loanItems = new List<LoanItem>();
            foreach (var item in loanApplicationModel.LoanItems)
            {
                var masterItem = masterItems.FirstOrDefault(c => c.ItemName.ToLower() == item.ItemName.ToLower());
                if (masterItem != null)
                {
                    loanItems.Add(new LoanItem
                    {
                        Id = item.Id ?? Guid.NewGuid(),
                        ItemName = item.ItemName,
                        MasterLoanItemId = masterItem.Id,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice,
                        CategoryId = masterItem.CategoryId,
                        UnitId = item.UnitId,
                        LoanApplicationId = updateLoanApplication.Id,
                    });
                }
            }
            await _loanItemRepository.AddRange(loanItems);

            // Uncomment and update if attachment mapping logic is needed
            // if (loanApplicationModel.AttachmentIds != null)
            // {
            //     foreach (var aid in loanApplicationModel.AttachmentIds)
            //     {
            //         var map = new AttachmentMapping
            //         {
            //             LoanApplicationId = id,
            //             AttachmentId = aid
            //         };
            //         await _attachmentMappingRepository.AddAsync(map);
            //     }
            // }
        }
        return new UpdateLoanApplicationResponseModel
        {

        };
    }

    public async Task ImportLoanApplication(IFormFile file, Guid? id, Guid? batchId)
    {
        try
        {
            XSSFWorkbook hssfwb;
            using (var stream = file.OpenReadStream())
            {
                hssfwb = new XSSFWorkbook(stream);
            }

            ISheet sheet = hssfwb.GetSheet(Convert.ToString(ExelImportConstants.SHEET_KENYA_APPLICATION));
            var _listapplicationsexcel = new List<ImportLoanApplicationModel>();
            var _listapplications = new List<LoanApplicationImportStaging>();
            var _errorDetails = new List<ExcelImportDetail>();
            var existingLoanApplications = await _loanApplicationRepository.GetFull(c => c.LoanBatchId == batchId && c.IsDeleted == false);

            var existingLoanBatch = await _loanBatchRepository.GetSingle((Guid)batchId);
            if (existingLoanBatch != null)
            {
                existingLoanBatch.StageText = "Initiated";

                await _loanBatchRepository.UpdateAsync(existingLoanBatch);
            }

            // Check if sheet exists
            if (sheet != null)
            {
                DataFormatter formatter = new DataFormatter();
                for (int rowIndex = 1; rowIndex <= sheet.LastRowNum; rowIndex++)
                {
                    var irow = sheet.GetRow(rowIndex);
                    if (irow == null || irow.Cells.All(c => c == null || c.CellType == CellType.Blank))
                        continue;

                    var row = irow.RowNum;

                    //if (sheet.GetRow(row) != null) // null is when the row only contains empty cells
                    //{
                    _listapplicationsexcel.Add(new ImportLoanApplicationModel
                    {
                        SystemId = ParseUtility.ParseAnyValueToString(formatter.FormatCellValue(sheet.GetRow(row).GetCell(2))),
                        WitnessFullName = ParseUtility.ParseAnyValueToString(formatter.FormatCellValue(sheet.GetRow(row).GetCell(7))),
                        WitnessRelation = ParseUtility.ParseAnyValueToString(formatter.FormatCellValue(sheet.GetRow(row).GetCell(14))),
                        WitnessNationalId = ParseUtility.ParseAnyValueToString(formatter.FormatCellValue(sheet.GetRow(row).GetCell(8))),
                        WitnessPhoneNo = ParseUtility.ParseAnyValueToString(formatter.FormatCellValue(sheet.GetRow(row).GetCell(9))),
                        DateOfWitness = ParseUtility.ParseAnyValueToString(formatter.FormatCellValue(sheet.GetRow(row).GetCell(11))),
                        //DateTime.TryParse(formatter.FormatCellValue(sheet.GetRow(row).GetCell(11)), out DateTime dateOfWitnessResult) ? dateOfWitnessResult.ToUniversalTime() : DateTime.MinValue,
                        GrandTotal = ParseUtility.ParseNullableDecimalValue(formatter.FormatCellValue(sheet.GetRow(row).GetCell(6))),
                        Country = ParseUtility.ParseAnyValueToString(formatter.FormatCellValue(sheet.GetRow(row).GetCell(12))),
                        OfficerId = ParseUtility.ParseAnyValueToString(formatter.FormatCellValue(sheet.GetRow(row).GetCell(16))),
                    });
                    //}
                }

                int r = 1;

                var _farmers = await _farmerRepository.GetAllAsync(c => true);
                var _countries = await _countryRepository.GetAllAsync(c => true);

                foreach (var importApplicationModel in _listapplicationsexcel)
                {
                    ++r;
                    var importEntity = new LoanApplicationImportStaging
                    {
                        ExcelImportId = (Guid)id,
                        RowNumber = r,
                        LoanNumber = LoanNumberGenerator.GenerateLoanNumber("KE"),
                        CreatedBy = Guid.Parse(_claimService.GetUserId()),
                        CreatedOn = DateTime.UtcNow,
                        FarmerId = _farmers.FirstOrDefault(f => f.SystemId == importApplicationModel.SystemId)?.Id ?? Guid.Empty,
                        WitnessFullName = importApplicationModel.WitnessFullName,
                        WitnessRelation = importApplicationModel.WitnessRelation ?? "",
                        WitnessPhoneNo = importApplicationModel.WitnessPhoneNo ?? "",
                        WitnessNationalId = importApplicationModel.WitnessNationalId ?? "",
                        DateOfWitness = string.IsNullOrEmpty(importApplicationModel.DateOfWitness) ? DateTime.UtcNow :
                                        Convert.ToDateTime(importApplicationModel.DateOfWitness),
                        LoanBatchId = batchId ?? Guid.Empty,
                        PrincipalAmount = importApplicationModel.GrandTotal.HasValue ? importApplicationModel.GrandTotal.Value : 0,
                        CountryId = string.IsNullOrEmpty(importApplicationModel.Country) ? null :
                                    _countries.FirstOrDefault(c => c.CountryName.ToLower().Trim() == importApplicationModel.Country.ToLower().Trim())?.Id ?? Guid.Empty,
                        OfficerId = string.IsNullOrEmpty(importApplicationModel.OfficerId) ? null :
                                    Guid.Parse(importApplicationModel.OfficerId)

                    };

                    var message = "";
                    if (importApplicationModel != null)
                    {
                        var errorMessages = new List<string>();

                        var farmer = _farmers.FirstOrDefault(f => f.SystemId == importApplicationModel.SystemId);
                        if (farmer != null)
                        {
                            if (farmer.CountryId != importEntity.CountryId)
                            {
                                errorMessages.Add("Farmer country does not match with the application country");
                            }
                        }

                        if (importEntity.FarmerId == Guid.Empty)
                        {
                            errorMessages.Add("Farmer with current System Id does not exist");
                        }
                        if (importApplicationModel.WitnessFullName == null || importApplicationModel.WitnessFullName == "")
                        {
                            errorMessages.Add("Witness name not provided");
                        }
                        else if (importApplicationModel.WitnessRelation == null || importApplicationModel.WitnessRelation == "")
                        {
                            errorMessages.Add("Witness relation not provided");
                        }
                        else if (importApplicationModel.WitnessPhoneNo == null || importApplicationModel.WitnessPhoneNo == "")
                        {
                            errorMessages.Add("Witness phone number not provided");
                        }
                        else if (importApplicationModel.WitnessNationalId == null || importApplicationModel.WitnessNationalId == "")
                        {
                            errorMessages.Add("Witness national id not provided");
                        }
                        //else if (importApplicationModel.DateOfWitness == DateTime.MinValue)
                        //{
                        //    importApplicationModel.DateOfWitness = DateTime.UtcNow;
                        //}

                        if (existingLoanApplications.Any(c => c.FarmerId == importEntity.FarmerId))
                        {
                            errorMessages.Add("Farmer already has an existing loan application in current product.");
                        }

                        // validate phone number
                        if (importEntity.CountryId.HasValue)
                        {
                            var phoneCheck = PhoneNumberHelper.NormalizeAndValidatePhoneNumber(importEntity.WitnessPhoneNo, (Guid)importEntity.CountryId);

                            if (phoneCheck.IsValid)
                            {
                                importEntity.WitnessPhoneNo = phoneCheck.NormalizedNumber;
                            }
                            else
                            {
                                errorMessages.Add("Invalid witness phone number");
                            }
                        }
                        else
                        {
                            errorMessages.Add("Country is invalid or not provided");
                        }

                        if (errorMessages.Any())
                        {
                            message = string.Join(", ", errorMessages);
                            importEntity.ValidationErrors = message;
                            importEntity.StatusId = 0;

                            var errorDetails = new ExcelImportDetail
                            {
                                Id = Guid.NewGuid(),
                                ExcelImportId = (Guid)id,
                                TabName = ExelImportConstants.SHEET_KENYA_APPLICATION,
                                RowNumber = r,
                                IsSuccess = false,
                                Remarks = message,
                            };
                            _errorDetails.Add(errorDetails);
                        }
                        else
                        {
                            importEntity.StatusId = 1;
                        }

                        _listapplications.Add(importEntity);
                    }
                    else
                    {
                        var errorDetails = new ExcelImportDetail
                        {
                            Id = Guid.NewGuid(),
                            ExcelImportId = Guid.NewGuid(),
                            TabName = ExelImportConstants.SHEET_KENYA_APPLICATION,
                            RowNumber = r,
                            IsSuccess = false,
                            Remarks = "Row is empty",
                        };
                        _errorDetails.Add(errorDetails);
                    }
                }

                //// INSERT ONLY IF ALL ROWS PASS
                // if (_listapplications.Count() > 0 && _errorDetails.Count == 0)
                //{
                // var newList = _listapplications.Where(c => true).ToList();

                //// 1. Get existing applications
                //var existingList = await _loanApplicationRepository.GetAllAsync(c => true);

                //// 2. Remove elements with duplicate SystemId and save new records
                //var listToAdd = newList
                //     .Where(newItem => !existingList.Any(existingItem =>
                //    existingItem.FarmerId == newItem.FarmerId && existingItem.LoanBatchId == newItem.LoanBatchId))
                //    .ToList();

                await _loanApplicationImportStagingRepository.AddRange(_listapplications);

                // 3. Get elements with duplicate SystemId and update them
                //var listToUpdate = newList.Except(listToAdd);
                //foreach (var item in listToUpdate)
                //{
                //    var application = existingList.FirstOrDefault(existingItem =>
                //            existingItem.FarmerId == item.FarmerId && existingItem.LoanBatchId == item.LoanBatchId);
                //    if (application != null)
                //    {
                //        application.WitnessFullName = item.WitnessFullName;
                //        application.WitnessRelation = item.WitnessRelation;
                //        application.WitnessPhoneNo = item.WitnessPhoneNo;
                //        application.DateOfWitness = item.DateOfWitness;
                //        application.WitnessNationalId = item.WitnessNationalId;
                //        application.UpdatedBy = Guid.Parse(_claimService.GetUserId());
                //        application.UpdatedOn = DateTime.UtcNow;
                //    }

                //    await _loanApplicationRepository.UpdateAsync(application);
                //}
                //}

                if (_errorDetails.Count() > 0)
                    await _excelImportDetailRepository.AddRange(_errorDetails);

                // Save loan items
                await ProcessLoanItems(id.Value, batchId.Value, hssfwb, _farmers, existingLoanBatch);
            }

            // Process loan items

        }
        catch (Exception ex)
        {
            throw;
        }
    }

    private async Task ProcessLoanItems(Guid id, Guid batchId, XSSFWorkbook hssfwb, List<Farmer> _farmers, LoanBatch existingLoanBatch)
    {
        ISheet sheetItems = hssfwb.GetSheet(Convert.ToString(ExelImportConstants.SHEET_LOAN_ITEMS));
        var _importedloanItems = new List<ImportLoanItemsModel>();
        var _listLoanItems = new List<LoanItemImportStaging>();

        List<ExcelImportDetail> _errorDetails = new List<ExcelImportDetail>();

        if (sheetItems != null)
        {
            //var _farmers = await _farmerRepository.GetAllAsync(c => 1 == 1);
            var loanBatchItems = await _loanBatchItemRepository.GetAllAsync(c => c.LoanBatchId == batchId);
            var itemUnits = _itemUnitRepository.GetItemUnits();
            // Load all LoanItems to cross-reference with loanBatchItems
            var masterLoanItems = await _masterLoanItemRepository.GetAllAsync(x => true);

            if (!loanBatchItems.Any() || loanBatchItems.Count == 0)
            {
                throw new Exception("Please add items to loan batch first");
            }

            DataFormatter formatter = new DataFormatter();
            for (int rowIndex = 1; rowIndex <= sheetItems.LastRowNum; rowIndex++)
            {
                var irow = sheetItems.GetRow(rowIndex);
                if (irow == null || irow.Cells.All(c => c == null || c.CellType == CellType.Blank))
                    continue;

                var row = irow.RowNum;

                _importedloanItems.Add(new ImportLoanItemsModel
                {
                    SystemId = ParseUtility.ParseAnyValueToString(formatter.FormatCellValue(sheetItems.GetRow(row).GetCell(0))),
                    ItemName = ParseUtility.ParseAnyValueToString(formatter.FormatCellValue(sheetItems.GetRow(row).GetCell(1))),
                    PricePerUnit = ParseUtility.ParseDecimalValue(formatter.FormatCellValue(sheetItems.GetRow(row).GetCell(2))),
                    Quantity = ParseUtility.ParseDecimalValue(formatter.FormatCellValue(sheetItems.GetRow(row).GetCell(3))),
                    Unit = ParseUtility.ParseAnyValueToString(formatter.FormatCellValue(sheetItems.GetRow(row).GetCell(4))),
                });
            }

            int r = 1;

            // Enrich loanBatchItems with actual ItemName via LoanItem
            var enrichedBatchItems = loanBatchItems
                .Join(masterLoanItems,
                      batch => batch.LoanItemId,
                      loanItem => loanItem.Id,
                      (batch, loanItem) => new
                      {
                          BatchItem = batch,
                          ItemName = loanItem.ItemName,
                          UnitId = batch.UnitId,
                          UnitPrice = batch.UnitPrice,
                          AvailableQuantity = batch.Quantity,
                          MasterLoanItemId = loanItem.Id
                      })
                .ToList();

            foreach (var importItemModel in _importedloanItems)
            {
                ++r;
                var errorMessages = new List<string>();
                var importEntity = new LoanItemImportStaging();

                var selectedfarmer = _farmers.FirstOrDefault(f => f.SystemId == importItemModel.SystemId);
                if (selectedfarmer == null)
                {
                    errorMessages.Add("Farmer does not exist");
                }
                else
                {
                    //var loanApplication = await _loanApplicationRepository.GetFirstAsync(c => c.LoanBatchId == batchId && c.FarmerId == selectedfarmer.Id);
                    var loanApplicationStaging = await _loanApplicationRepository.GetLoanAppImportStaging(batchId);
                    var loanApplication = loanApplicationStaging.FirstOrDefault(c => c.FarmerId == selectedfarmer.Id && c.ExcelImportId == id);

                    if (loanApplication == null)
                    {
                        errorMessages.Add("Loan application does not exist");
                        // continue;
                    }

                    var batchItem = enrichedBatchItems.FirstOrDefault(b => b.ItemName == importItemModel.ItemName);

                    importEntity = new LoanItemImportStaging
                    {
                        ItemName = importItemModel.ItemName,
                        UnitPrice = importItemModel.PricePerUnit,
                        Quantity = importItemModel.Quantity,
                        UnitId = itemUnits.FirstOrDefault(c => c.Abbreviation == importItemModel.Unit).Id,
                        MasterLoanItemId = batchItem.MasterLoanItemId,
                        LoanApplicationId = loanApplication.Id,
                        ExcelImportId = id,
                        RowNumber = r
                    };

                    var itemUnit = itemUnits.FirstOrDefault(u => u.Id == batchItem?.UnitId);

                    if (string.IsNullOrWhiteSpace(importItemModel.ItemName))
                    {
                        errorMessages.Add("Item does not exist");
                    }
                    else if (batchItem == null)
                    {
                        errorMessages.Add("ItemName not found in Loan Batch Items");
                    }
                    else
                    {
                        if (!string.Equals(importItemModel.Unit, itemUnit?.Abbreviation, StringComparison.OrdinalIgnoreCase))
                        {
                            errorMessages.Add($"Unit '{importItemModel.Unit}' does not match batch unit '{itemUnit?.Abbreviation}'");
                        }

                        if (batchItem.UnitPrice != importItemModel.PricePerUnit)
                        {
                            errorMessages.Add($"Unit Price mismatch. Expected {batchItem.UnitPrice}, got {importItemModel.PricePerUnit}");
                        }

                        if (importItemModel.Quantity > batchItem.AvailableQuantity)
                        {
                            errorMessages.Add($"Quantity exceeds batch limit. Allowed: {batchItem.AvailableQuantity}, Provided: {importItemModel.Quantity}");
                        }
                    }

                    if (importItemModel.PricePerUnit < 0)
                    {
                        errorMessages.Add("Price Per Unit must be a valid number");
                    }

                    if (importItemModel.Quantity < 0)
                    {
                        errorMessages.Add("Quantity must be a valid number");
                    }

                    if (string.IsNullOrWhiteSpace(importItemModel.Unit))
                    {
                        errorMessages.Add("Unit not provided");
                    }

                }

                if (errorMessages.Any())
                {
                    string message = string.Join(", ", errorMessages);
                    _errorDetails.Add(new ExcelImportDetail
                    {
                        Id = Guid.NewGuid(),
                        ExcelImportId = id,
                        TabName = ExelImportConstants.SHEET_LOAN_ITEMS,
                        RowNumber = r,
                        IsSuccess = false,
                        Remarks = message
                    });

                    importEntity.ExcelImportId = id;
                    importEntity.RowNumber = r;
                    importEntity.StatusId = 0;
                    importEntity.ValidationErrors = message;
                    _listLoanItems.Add(importEntity);
                }
                else
                {
                    importEntity.StatusId = 1;
                    _listLoanItems.Add(importEntity);
                }
            }

            if (_listLoanItems.Count > 0)
            {
                //var newList = _listLoanItems.Where(c => !string.IsNullOrWhiteSpace(c.ItemName)).ToList();
                //var existingList = await _loanItemImportStagingRepository.GetAllAsync(c => c.LoanApplicationId == id);

                //var listToAdd = newList
                //    .Where(newItem => !existingList.Any(existingItem =>
                //        existingItem.ItemName == newItem.ItemName))
                //    .ToList();

                await _loanItemImportStagingRepository.AddRange(_listLoanItems);

                var newList = _listLoanItems.Where(c => !string.IsNullOrWhiteSpace(c.ItemName)).ToList();
                // update PrincipalAmount from items
                foreach (var loanItem in newList)
                {
                    var importedLoanAppList = await _loanApplicationRepository.GetLoanAppImportStaging(batchId);
                    var importedLoanApp = importedLoanAppList.FirstOrDefault(c => c.Id == loanItem.LoanApplicationId);

                    decimal principalAmount = 0;
                    principalAmount += (decimal)(loanItem.UnitPrice * loanItem.Quantity);
                    importedLoanApp.PrincipalAmount = principalAmount;

                    decimal feeApplied = 0;
                    foreach (var fee in existingLoanBatch.ProcessingFees)
                    {
                        if (fee.FeeType.Trim().ToLower() == "flat")
                        {
                            feeApplied += fee.Value;
                        }
                        else
                        {
                            feeApplied += importedLoanApp.PrincipalAmount * (fee.Value / 100);
                        }
                    }
                    importedLoanApp.FeeApplied = feeApplied;
                    importedLoanApp.PrincipalAmount = importedLoanApp.PrincipalAmount + feeApplied;
                    importedLoanApp.EffectivePrincipal = importedLoanApp.PrincipalAmount;

                    _loanApplicationRepository.UpdateLoanAppImportStaging(importedLoanApp);
                }

                //var listToUpdate = newList.Except(newList);
                //foreach (var item in listToUpdate)
                //{
                //    var loanItem = existingList.FirstOrDefault(existingItem =>
                //        existingItem.ItemName == item.ItemName);

                //    if (loanItem != null)
                //    {
                //        loanItem.ItemName = item.ItemName;
                //        loanItem.Quantity = item.Quantity;
                //        loanItem.UnitPrice = item.UnitPrice;
                //        loanItem.UnitId = item.UnitId ?? 0;

                //        await _loanItemImportStagingRepository.UpdateAsync(loanItem);
                //    }
                //}
            }

            if (_errorDetails.Count > 0)
            {
                try
                {
                    await _excelImportDetailRepository.AddRange(_errorDetails);
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }
    }

    public async Task<IEnumerable<LoanApplicationResponseModel>> GetFarmerLoanApps(Guid farmerId, Guid countryId)
    {
        try
        {
            IEnumerable<LoanApplication> _loanApplications;

            _loanApplications = await _loanApplicationRepository.GetAllAsync(
                c => c.FarmerId == farmerId && c.IsDeleted == false);


            int numberOfObjectsPerPage = 1000;
            var queryResult = _loanApplications;


            var loanApplications = _mapper.Map<ReadOnlyCollection<LoanApplicationResponseModel>>(queryResult);
            var farmerList = await _farmerRepository.GetAllAsync(c => c.Id == c.Id);
            var applicationStatusList = await _applicationStatusRepository.GetAllAsync(c => true);
            var loanBatchesList = await _loanBatchService.GetAllAsync(new LoanBatchSearchParams { PageNumber = 1, PageSize = 100000, CountryId = countryId });

            foreach (var loanApplication in loanApplications)
            {
                // farmer
                var farmers = farmerList.ToList().Where(c => c.Id == loanApplication.FarmerId);
                if (farmers != null && farmers.Any())
                {
                    loanApplication.Farmer = _mapper.Map<FarmerResponseModel>(farmers.FirstOrDefault());
                }

                //loan batches
                var loanBatches = loanBatchesList.LoanBatches.ToList().Where(c => c.Id == loanApplication.LoanBatchId);
                if (loanBatches != null && loanBatches.Any())
                {
                    loanApplication.LoanBatch = _mapper.Map<LoanBatchResponseModel>(loanBatches.FirstOrDefault());
                }

                // loan items
                var applItems = _mapper.Map<List<LoanAppItemImportModel>>(await _loanItemRepository.GetAllAsync(c => c.LoanApplicationId == loanApplication.Id));

                // Retrieve units
                var units = await GetItemUnitsAsync();

                // Set units for each loan item
                applItems.ForEach(item =>
                {
                    var unit = units.FirstOrDefault(u => u.Value == Convert.ToString(item.UnitId));
                    item.Unit = unit.Label;
                });

                // Assign updated items to the loan application
                loanApplication.LoanItems = applItems;

                foreach (var item in applItems)
                {
                    loanApplication.TotalValue = loanApplication.TotalValue + (float)(item.Quantity * item.UnitPrice);
                }

                var applicationStatusLog = (await _applicationStatusLogRepository.GetAllAsync(c => c.ApplicationId == loanApplication.Id)).FirstOrDefault();

                if (applicationStatusLog != null)
                {
                    var users = await _userManager.Users.ToListAsync();
                    var creatorUser = users.FirstOrDefault(user => user.Id == applicationStatusLog.CreatedBy.ToString());
                    IList<string> userRoles = new List<string>();

                    if (creatorUser?.Id != null && Guid.TryParse(creatorUser.Id, out var creatorUserId) && creatorUserId != Guid.Empty)
                    {
                        var user = await _userManager.FindByIdAsync(creatorUser.Id.ToString());
                        if (user != null)
                        {
                            userRoles = await _userManager.GetRolesAsync(user);
                        }
                    }


                    loanApplication.ModeratorRole = string.Join(" ", userRoles);
                    if (creatorUser != null)
                    {
                        loanApplication.Moderator = creatorUser.UserName;
                    }
                    else
                    {
                        loanApplication.Moderator = "Unknown";
                    }

                }

                var paymentHistory = _loanRepaymentRepository.GetRepaymentHistory((Guid)loanApplication.Id);
                if (paymentHistory != null && paymentHistory.Count > 0)
                {
                    loanApplication.InUse = true;
                }

                var _appStatus = applicationStatusList.FirstOrDefault(c => c.Id == loanApplication.Status);
                if (_appStatus != null)
                {
                    loanApplication.StatusText = _appStatus.Name;
                }
                else
                {
                    loanApplication.StatusText = "Draft";
                }
            }

            return _mapper.Map<IEnumerable<LoanApplicationResponseModel>>(loanApplications);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public async Task<ApplicationStatusEditResponseModel> UpdateStatusAsync(Guid statusId, IEnumerable<ApplicationStatusEditModel> statusModel)
    {
        try
        {
            foreach (var item in statusModel)
            {
                var loanApplication = await _loanApplicationRepository.GetFirstAsync(ti => ti.Id == item.Id);

                loanApplication.Status = statusId;
                if (statusId == new Guid("e24d24a8-fc69-4527-a92a-97f6648a43c5"))
                {
                    loanApplication.DisbursementDate = DateTime.UtcNow;
                }

                var updateLoanApplication = await _loanApplicationRepository.UpdateAsync(loanApplication);
                var applicationStatusLog = new ApplicationStatusLog
                {
                    Id = new Guid(),
                    ApplicationId = item.Id,
                    StatusId = statusId,
                    Comments = item.Comments,
                    CreatedBy = new Guid(_claimService.GetUserId()),
                    CreatedOn = DateTime.UtcNow
                };
                var log = await _applicationStatusLogRepository.AddAsync(applicationStatusLog);

                if (statusId == new Guid("e24d24a8-fc69-4527-a92a-97f6648a43c5"))
                {

                    await _loanRepaymentService.GeneratePaymentSchedule(item.Id);
                }


            }

            return new ApplicationStatusEditResponseModel();
        }
        catch (Exception ex)
        {
            return new ApplicationStatusEditResponseModel();
        }
    }

    public async Task<IEnumerable<ApplicationStatusLogResponseModel>> GetStatusHistory(Guid id)
    {
        try
        {
            var _applicationStatusLog = await _applicationStatusLogRepository.GetFull(c => c.ApplicationId == id);

            if (_applicationStatusLog != null)
            {
                var applicationStatusLog = _mapper.Map<List<ApplicationStatusLogResponseModel>>
                    (_applicationStatusLog.OrderByDescending(c => c.CreatedOn));

                var users = await _userManager.Users.ToListAsync();

                foreach (var applicationStatus in applicationStatusLog)
                {
                    var creatorUser = users.FirstOrDefault(user => user.Id == applicationStatus.CreatedBy.ToString());
                    IList<string> userRoles = new List<string>();

                    if (creatorUser?.Id != null && Guid.TryParse(creatorUser.Id, out var creatorUserId) && creatorUserId != Guid.Empty)
                    {
                        var user = await _userManager.FindByIdAsync(creatorUser.Id.ToString());
                        if (user != null)
                        {
                            userRoles = await _userManager.GetRolesAsync(user);
                        }
                    }

                    applicationStatus.ModeratorRole = string.Join(" ", userRoles);
                    if (creatorUser != null)
                    {
                        applicationStatus.Moderator = creatorUser.UserName;
                    }
                    else
                    {
                        applicationStatus.Moderator = "Deleted User";
                    }
                }
                return new List<ApplicationStatusLogResponseModel>(applicationStatusLog);
            }

            return new List<ApplicationStatusLogResponseModel>();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public async Task<BaseResponseModel> GenerateEMISchedule(Guid loanApplicationId)
    {
        //decimal principal, decimal annualInterestRate, int tenureMonths, DateTime startDate
        var loanApplication = await _loanApplicationRepository.GetFirstAsync(c => c.Id == loanApplicationId);
        var loanBatch = await _loanBatchService.GetSingle(loanApplication.LoanBatchId);
        var annualInterestRate = loanBatch.CalculationTimeframe == "Monthly" ? loanBatch.InterestRate * 12 : loanBatch.InterestRate;
        var tenureMonths = loanBatch.Tenure;
        var principal = loanApplication.PrincipalAmount;

        List<EMISchedule> emiSchedule = new List<EMISchedule>();

        // Step 1: Calculate Simple Interest
        decimal simpleInterest = (principal * annualInterestRate * (tenureMonths / 12.0m)) / 100;
        decimal totalPayable = principal + simpleInterest;

        // Step 2: Compute EMI
        decimal monthlyEMI = totalPayable / tenureMonths;
        decimal monthlyPrincipal = principal / tenureMonths;
        decimal monthlyInterest = simpleInterest / tenureMonths;

        decimal remainingBalance = totalPayable;

        var startDate = new DateTime(loanBatch.InitiationDate.Year, loanBatch.InitiationDate.Month, 1)
                                .AddMonths(loanBatch.GracePeriod + 1); // Ensure start date is always the 1st

        for (int i = 0; i < tenureMonths; i++)
        {
            DateTime emiStartDate = startDate.AddMonths(i);
            DateTime emiEndDate = emiStartDate.AddMonths(1).AddDays(-1);

            remainingBalance -= monthlyEMI;

            emiSchedule.Add(new EMISchedule
            {
                LoanApplicationId = loanApplicationId,
                Amount = monthlyEMI,
                PrincipalAmount = monthlyPrincipal,
                InterestAmount = monthlyInterest,
                StartDate = emiStartDate.ToUniversalTime().AddHours(5).AddMinutes(30),
                EndDate = emiEndDate.ToUniversalTime().AddHours(5).AddMinutes(30),
                Balance = Math.Max(remainingBalance, 0), // Ensure it doesn't go negative
                PaymentStatus = "Pending"
            });
        }

        var entity = _mapper.Map<IEnumerable<EMISchedule>>(emiSchedule);
        await _eMIScheduleRepository.AddRange(entity);

        return new BaseResponseModel
        {
            Id = entity.FirstOrDefault().Id
        };
    }

    //public async Task<BaseResponseModel> GenerateEmiSchedule(Guid loanbatchId)
    //{
    //    var loanApplications = await _loanApplicationRepository.GetAllAsync(c => c.LoanBatchId == loanbatchId);
    //    var loanBatches = await _loanBatchService.GetSingle(loanbatchId);

    //    var emiSchedules = loanApplications.SelectMany(loan =>
    //    {
    //        var loanBatch = loanBatches;
    //        var monthlyRate = loanBatch.CalculationTimeframe == "Monthly" ? loanBatch.InterestRate / 100 : loanBatch.InterestRate / 12 / 100;
    //        var annualInterestRate = loanBatch.CalculationTimeframe == "Monthly" ? loanBatch.InterestRate * 12 : loanBatch.InterestRate;

    //        /*
    //            As per client's email, to ensure that the loans are affordable for the farmers,
    //            the interest is not added to the principal amount
    //         */
    //        //var emi = loan.PrincipalAmount * monthlyRate * (decimal)Math.Pow((1 + (double)monthlyRate), loanBatch.Tenure) /
    //        //          (decimal)(Math.Pow((1 + (double)monthlyRate), loanBatch.Tenure) - 1);

    //        var balance = loan.PrincipalAmount;
    //        var startDate = new DateTime(loanBatch.InitiationDate.Year, loanBatch.InitiationDate.Month, 1)
    //                            .AddMonths(loanBatch.GracePeriod); // Ensure start date is always the 1st

    //        var schedule = new List<EMISchedule>();

    //        decimal totalInterest = (loan.PrincipalAmount * annualInterestRate * loanBatch.Tenure) / (100 * 12); // Simple Interest Calculation
    //        decimal totalAmount = loan.PrincipalAmount + totalInterest; // Total amount to be repaid
    //        decimal emi = totalAmount / loanBatch.Tenure; // Fixed EMI amount

    //        decimal remainingPrincipal = loan.PrincipalAmount;

    //        for (int i = 0; i < loanBatch.Tenure; i++)
    //        {
    //            //var interest = balance * monthlyRate;
    //            //var principal = emi - interest;
    //            //var endDate = startDate.AddMonths(1).AddDays(-1);

    //            decimal interest = (loan.PrincipalAmount * annualInterestRate) / (100 * 12); // Fixed interest per month
    //            decimal principal = emi - interest; // Principal portion of EMI
    //            var endDate = startDate.AddMonths(1).AddDays(-1); // Last day of the month

    //            //schedule.Add(new EMISchedule
    //            //{
    //            //    Amount = Math.Round(emi, 2),
    //            //    InterestAmount = Math.Round(interest, 2),
    //            //    StartDate = startDate.ToUniversalTime(), // Convert to UTC,
    //            //    EndDate = endDate.ToUniversalTime(), // Convert to UTC,
    //            //    Balance = Math.Round(balance - principal, 2),
    //            //    LoanApplicationId = loan.Id,
    //            //    PaymentStatus = "Pending"
    //            //});

    //            //balance -= principal;
    //            //startDate = new DateTime(startDate.Year, startDate.Month, 1, 0, 0, 0, DateTimeKind.Utc).AddMonths(1); // Move to next month's 1st in UTC

    //            schedule.Add(new EMISchedule
    //            {
    //                Amount = Math.Round(emi, 2),
    //                InterestAmount = Math.Round(interest, 2),
    //                PrincipalAmount = Math.Round(principal, 2),
    //                StartDate = startDate.ToUniversalTime(),
    //                EndDate = endDate.ToUniversalTime(),
    //                Balance = Math.Round(remainingPrincipal - principal, 2),
    //                LoanApplicationId = loan.Id,
    //                PaymentStatus = "Pending"
    //            });

    //            remainingPrincipal -= principal;
    //            startDate = new DateTime(startDate.Year, startDate.Month, 1, 0, 0, 0, DateTimeKind.Utc).AddMonths(1); // Move to next month's 1st
    //        }

    //        return schedule;
    //    }).ToList();

    //    var entity = _mapper.Map<IEnumerable<EMISchedule>>(emiSchedules);

    //    await _eMIScheduleRepository.AddRange(entity);

    //    return new BaseResponseModel
    //    {
    //        Id = entity.FirstOrDefault().Id
    //    };
    //}

    public async Task<IEnumerable<EMIScheduleResponseModel>> GetEmiSchedule(Guid loanApplicationId)
    {
        var schedules = await _eMIScheduleRepository.GetAllAsync(c => c.LoanApplicationId == loanApplicationId);

        return _mapper.Map<IEnumerable<EMIScheduleResponseModel>>(schedules.OrderBy(c => c.StartDate));
    }

    public async Task<List<EMISchedule>> UpdateEMISchedule_NA(decimal paymentReceived, DateTime paymentDate, Guid loanApplicationId)
    {
        List<EMISchedule> existingSchedule = new List<EMISchedule>();
        existingSchedule = await _eMIScheduleRepository.GetAllAsync(c => c.LoanApplicationId == loanApplicationId);

        var loanApplication = await _loanApplicationRepository.GetFirstAsync(c => c.Id == loanApplicationId);
        var loanBatch = await _loanBatchService.GetSingle(loanApplication.LoanBatchId);

        decimal monthlyRate;
        int remainingTenure = 0;

        monthlyRate = loanBatch.CalculationTimeframe == "Monthly" ? loanBatch.InterestRate / 100 : loanBatch.InterestRate / 12 / 100;

        List<EMISchedule> updatedSchedule = new List<EMISchedule>();
        decimal remainingBalance = existingSchedule[0].Balance; // Start with the latest balance
        decimal remainingPayment = paymentReceived;

        // Apply the payment to principal
        for (int i = 0; i < existingSchedule.Count; i++)
        {
            var emi = existingSchedule[i];

            // Deduct payment first from principal
            if (remainingPayment > 0)
            {
                decimal principalToDeduct = Math.Min(remainingPayment, emi.Balance);
                remainingBalance -= principalToDeduct;
                remainingPayment -= principalToDeduct;
            }

            // If payment fully covers the balance, end the loop
            if (remainingBalance <= 0)
            {
                break;
            }

            updatedSchedule.Add(new EMISchedule
            {
                Amount = emi.Amount,
                InterestAmount = emi.InterestAmount,
                StartDate = emi.StartDate,
                EndDate = emi.EndDate,
                Balance = Math.Round(remainingBalance, 2),
                LoanApplicationId = emi.LoanApplicationId
            });
        }

        // If the loan is still active, recalculate EMI with the new balance
        if (remainingBalance > 0)
        {
            // Calculate new EMI for the remaining tenure
            decimal newEMI = (remainingBalance * monthlyRate * (decimal)Math.Pow((1 + (double)monthlyRate), remainingTenure)) /
                             (decimal)(Math.Pow((1 + (double)monthlyRate), remainingTenure) - 1);

            DateTime newStartDate = existingSchedule.Last().EndDate.AddDays(1);

            for (int i = 0; i < remainingTenure; i++)
            {
                var interest = remainingBalance * monthlyRate;
                var principal = newEMI - interest;
                var endDate = newStartDate.AddMonths(1).AddDays(-1);

                updatedSchedule.Add(new EMISchedule
                {
                    Amount = Math.Round(newEMI, 2),
                    InterestAmount = Math.Round(interest, 2),
                    StartDate = newStartDate,
                    EndDate = endDate,
                    Balance = Math.Round(remainingBalance - principal, 2),
                    LoanApplicationId = existingSchedule[0].LoanApplicationId
                });

                remainingBalance -= principal;
                if (remainingBalance <= 0) break;

                newStartDate = newStartDate.AddMonths(1);
            }
        }

        return updatedSchedule;
    }

    public async Task<List<EMISchedule>> UpdateEMISchedule(decimal paymentReceived, DateTime paymentDate, Guid loanApplicationId)
    {
        List<EMISchedule> existingSchedule = new List<EMISchedule>();
        existingSchedule = await _eMIScheduleRepository.GetAllAsync(c =>
                                                                    c.LoanApplicationId == loanApplicationId &&
                                                                    c.PaymentStatus != "Paid");

        existingSchedule = existingSchedule.OrderBy(c => c.StartDate).ToList();

        var loanApplication = await _loanApplicationRepository.GetFirstAsync(c => c.Id == loanApplicationId);
        var loanBatch = await _loanBatchService.GetSingle(loanApplication.LoanBatchId);

        decimal monthlyRate;

        monthlyRate = loanBatch.CalculationTimeframe == "Monthly" ? loanBatch.InterestRate / 100 : loanBatch.InterestRate / 12 / 100;
        List<EMISchedule> updatedSchedule = new List<EMISchedule>();

        decimal remainingBalance = existingSchedule.First().Balance; // Get latest balance
        decimal remainingPayment = paymentReceived;
        int remainingTenure = existingSchedule.Count;

        foreach (var emi in existingSchedule)
        {
            if (remainingPayment <= 0)
            {
                updatedSchedule.Add(emi); // No more payments to apply
                continue;
            }

            decimal principalPaid = Math.Min(remainingPayment, emi.Amount - emi.InterestAmount); // Pay principal first
            remainingPayment -= principalPaid;
            remainingBalance -= principalPaid;

            decimal interestPaid = Math.Min(remainingPayment, emi.InterestAmount); // Pay interest next
            remainingPayment -= interestPaid;

            string paymentStatus;
            if (principalPaid + interestPaid >= emi.Amount)
            {
                paymentStatus = "Paid";
                remainingTenure--; // Reduce remaining tenure
            }
            else if (principalPaid > 0)
            {
                paymentStatus = "Partially Paid";
            }
            else
            {
                paymentStatus = "Pending";
            }

            updatedSchedule.Add(new EMISchedule
            {
                Id = emi.Id,
                Amount = emi.Amount,
                InterestAmount = emi.InterestAmount - interestPaid,
                StartDate = emi.StartDate,
                EndDate = emi.EndDate,
                Balance = Math.Round(remainingBalance, 2),
                LoanApplicationId = emi.LoanApplicationId,
                PaymentStatus = paymentStatus,

            });
        }

        // Step 2: If the loan is not fully repaid, recalculate EMI
        if (remainingBalance > 0 && remainingTenure > 0)
        {
            decimal newEMI = CalculateNewEMI(remainingBalance, monthlyRate, remainingTenure);
            DateTime newStartDate = existingSchedule.Last().EndDate.AddDays(1);

            for (int i = 0; i < remainingTenure; i++)
            {
                var interest = remainingBalance * monthlyRate;
                var principal = newEMI - interest;
                var endDate = newStartDate.AddMonths(1).AddDays(-1);

                updatedSchedule.Add(new EMISchedule
                {
                    Amount = Math.Round(newEMI, 2),
                    InterestAmount = Math.Round(interest, 2),
                    StartDate = newStartDate,
                    EndDate = endDate,
                    Balance = Math.Round(remainingBalance - principal, 2),
                    LoanApplicationId = existingSchedule[0].LoanApplicationId,
                    PaymentStatus = "Pending"
                });

                remainingBalance -= principal;
                if (remainingBalance <= 0) break;

                newStartDate = newStartDate.AddMonths(1);
            }
        }

        _loanApplicationRepository.SaveUpdatedSchedule(loanApplicationId, updatedSchedule);

        return updatedSchedule;
    }

    public decimal CalculateNewPayable(decimal principal, decimal interest, decimal paymentAmount)
    {
        // Step 1: Deduct payment from principal first
        decimal remainingPrincipal = principal - Math.Min(paymentAmount, principal);
        decimal remainingInterest = interest; // Since it's simple interest, it remains fixed.

        // Step 2: Calculate new payable amount
        decimal newPayable = remainingPrincipal + remainingInterest;

        return newPayable;
    }

    public int CalculateRemainingTenure(decimal remainingBalance, decimal emi, decimal monthlyRate)
    {
        if (remainingBalance <= 0) return 0; // Loan fully repaid

        double numerator = Math.Log((double)(emi / (emi - (remainingBalance * monthlyRate))));
        double denominator = Math.Log(1 + (double)monthlyRate);

        int remainingTenure = (int)Math.Ceiling(numerator / denominator); // Round up to next full month
        return remainingTenure;
    }

    public decimal CalculateNewEMI(decimal principal, decimal annualRate, int tenureMonths)
    {
        if (tenureMonths <= 0) throw new ArgumentException("Tenure must be greater than zero.");

        decimal monthlyRate = annualRate / 12 / 100; // Convert annual rate to monthly rate (percentage to decimal)

        // Simple Interest Calculation
        decimal totalInterest = principal * monthlyRate * tenureMonths;
        decimal totalAmount = principal + totalInterest;
        decimal emi = totalAmount / tenureMonths; // Divide equally over tenure

        return Math.Round(emi, 2);
    }

    public decimal CalculateNewEMI_old(decimal remainingBalance, decimal monthlyRate, int remainingTenure)
    {
        if (remainingTenure <= 0) return 0; // No EMI needed if loan is paid

        return (remainingBalance * monthlyRate * (decimal)Math.Pow((1 + (double)monthlyRate), remainingTenure)) /
               (decimal)(Math.Pow((1 + (double)monthlyRate), remainingTenure) - 1);
    }

    public async Task<UpdateLoanApplicationResponseModel> UpdateStage(Guid id, UpdateStageModel model)
    {
        // get payment stages
        var stages = await _loanApplicationRepository.GetLoanAppApprovalStages();

        string stageName = model.Action switch
        {
            "review_rejected" => "Review Rejected",
            "review_next" => "Pending Approval",
            "approved" => "Approved",
            _ => "Under Review" // Default case
        };

        var stageId = stages.Any() ? stages.Where(c => c.StageText == stageName).FirstOrDefault().Id : Guid.Parse("fc4497a2-d9ee-49e1-a9df-99d48731d321");

        // add to history
        await _loanApplicationRepository.AddLoanAppHistory(new LoanApplicationHistory
        {
            Action = stageName,
            Comments = model.Remarks,
            LoanApplicationId = id,
            StageId = stageId,
            CreatedBy = Guid.Parse(_claimService.GetUserId()),
            CreatedOn = DateTime.UtcNow,
        });

        // update status id in loan application
        var loanApp = await _loanApplicationRepository.GetFirstAsync(ti => ti.Id == id);
        loanApp.Status = stageId;
        loanApp.UpdatedOn = DateTime.UtcNow;
        loanApp.UpdatedBy = Guid.Parse(_claimService.GetUserId());

        var updatedbatch = await _loanApplicationRepository.UpdateAsync(loanApp);

        //Get initiator name
        var users = await _userService.GetAllAsync("");
        var initiator = users.FirstOrDefault(user => user.Id == new Guid(_claimService.GetUserId()));
        if (initiator == null)
        {
            throw new Exception("User not found."); // Handle the case where the user is not found
        }

        await SaveActivityLog(new CreateActivityLogModel
        {
            Title = $"{loanApp.LoanNumber} - status updated",
            Description = $"{loanApp.Status} by {initiator.Username}",
            Link = $"/loan-application/track/{loanApp.Id}"
        });

        //construct email using template
        //var emailTemplates = await _emailTemplateRepository.GetAllAsync(tl => tl.Id == new Guid("35b845b6-37be-4705-8e14-b2d009f73b16"));
        //var emailTemplate = emailTemplates.FirstOrDefault();
        //emailTemplate.Variables = await _emailTemplateVariableRepository.GetAllAsync(tl => tl.EmailTemplateId == emailTemplate.Id);
        //var emailBody = _emailTemplateService.RenderTemplate(
        //                 emailTemplate,
        //                 new Dictionary<string, string>
        //                {
        //                     { "Initiator_Name", initiator.Username },
        //                     { "Link", $"{_portalSettings.PortalUrl}/payment-batch/details/{batch.Id}" }

        //                });

        //Send email
        //var recipients = new[] { "karanvir0153@icloud.com", "munish@enet.co.ke" };

        //foreach (var recipient in recipients)
        //{
        //    await _emailService.SendEmailAsync(
        //        Common.Email.EmailMessage.Create(
        //            recipient, // One recipient at a time
        //            emailBody,
        //            "Payment status change"
        //        )
        //    );
        //}

        return new UpdateLoanApplicationResponseModel
        {
            Id = id
        };
    }

    public async Task<LoanApplicationTrackModel> GetImportSummary(Guid loanBatchId)
    {
        var loanBatch = _mapper.Map<LoanBatchResponseModel>
            (await _loanBatchRepository.GetFirstAsync(lb => loanBatchId.Equals(lb.Id)));

        var _loanApplicationsImported = await _loanApplicationRepository.GetLoanAppImportStaging(loanBatchId);

        var excelImportIds = _loanApplicationsImported
        .Select(x => x.ExcelImportId)
        .Distinct()
        .ToList();

        var _excelImport = await _loanApplicationRepository.GetAllAsync(c => c.Id == _loanApplicationsImported.LastOrDefault().ExcelImportId);

        return new LoanApplicationTrackModel
        {
            LoanBatch = loanBatch,
            ImportedLoanApplications = _loanApplicationsImported.ToList(),
            ImportedRowCount = _loanApplicationsImported.Count(),
            FailedRowCount = _loanApplicationsImported.Where(c => c.StatusId == 0).Count(),
            SuccessRowCount = _loanApplicationsImported.Where(c => c.StatusId == 1).Count(),
        };
    }

    public async Task<List<LoanAppImportModel>> GetImportHistory(Guid loanBatchId)
    {
        var loanBatch = _mapper.Map<LoanBatchResponseModel>(await _loanBatchRepository.GetSingle(loanBatchId));
        var _loanApplicationsImported = await _loanApplicationRepository.GetLoanAppImportStaging(loanBatchId);

        //var excelImportIds = _loanApplicationsImported
        //    .Select(x => x.ExcelImportId)
        //    .Distinct()
        //    .ToList();

        // Get the latest ExcelImportId based on CreatedOn or another indicator
        var latestImport = _loanApplicationsImported
            .OrderByDescending(x => x.CreatedOn) // 
            .FirstOrDefault();

        if (latestImport == null)
        {
            return new List<LoanAppImportModel>();
        }

        var _excelImports = await _excelImportRepository
            .GetAllAsync(e => latestImport.ExcelImportId == e.Id);

        var _itemDetails = await _loanItemImportStagingRepository.GetAllAsync(c => c.ExcelImportId == latestImport.ExcelImportId);

        var result = _excelImports
         .Select(ei =>
          {
              var relatedApplications = _loanApplicationsImported
                  .Where(app => app.ExcelImportId == ei.Id)
                  .Select(app => new LoanApplicationStagingModel
                  {
                      Id = app.Id,
                      RowNumber = app.RowNumber,
                      StatusId = app.StatusId,
                      ValidationErrors = app.ValidationErrors,
                      ExcelImportId = app.ExcelImportId,
                      FarmerId = app.FarmerId,
                      WitnessFullName = app.WitnessFullName,
                      WitnessNationalId = app.WitnessNationalId,
                      WitnessPhoneNo = app.WitnessPhoneNo,
                      WitnessRelation = app.WitnessRelation,
                      DateOfWitness = app.DateOfWitness,
                      EnumeratorFullName = app.EnumeratorFullName,
                      PrincipalAmount = app.PrincipalAmount,
                      ValidationStatus = app.ValidationStatus
                  })
                  .ToList();

              var failedCount = relatedApplications.Count(x => x.StatusId == 0);
              var failedItemCount = _itemDetails.Count(x => x.StatusId == 0);

              return new LoanAppImportModel
              {
                  ExcelImportId = ei.Id,
                  FileName = ei.Filename,
                  ImportedDateTime = ei.ImportedDateTime,
                  Applications = relatedApplications,
                  IsFailedBatch = (failedCount > 0 || failedItemCount > 0 || _itemDetails.Count == 0),
                  LoanBatch = loanBatch,
                  LoanItems = _itemDetails
              };
          })
            .ToList();

        return result;
    }

    /// <summary>
    /// Get only applicable loan applications
    /// discard with future dates
    /// </summary>
    /// <param name="loanBatchId"></param>
    /// <returns></returns>
    public async Task<List<LoanApplication>> GetEffectiveLoanApplications(Guid loanBatchId)
    {
        var loanBatch = await _loanBatchRepository.GetFirstAsync(x => x.Id == loanBatchId);
        if (loanBatch == null)
            return null;

        // Use grace period if available, otherwise use effective date directly
        var effectiveBaseDate = loanBatch.GracePeriod.HasValue
            ? loanBatch.EffectiveDate.AddMonths(loanBatch.GracePeriod.Value)
            : loanBatch.EffectiveDate;

        var startDate = new DateTime(effectiveBaseDate.Year, effectiveBaseDate.Month, 1);

        if (startDate <= DateTime.Today)
        {
            var loanApplications = await _loanApplicationRepository.GetAllAsync(x =>
                x.LoanBatchId == loanBatchId &&
                x.IsDeleted == false &&
                x.Status == Guid.Parse(DISBURSED)
            );

            return loanApplications;
        }

        return null;
    }

    protected NpgsqlConnection GetConnection()
    {
        return new NpgsqlConnection(_connectionString);
    }

    protected class DatabaseConfiguration
    {
        public string DefaultConnection { get; set; }
    }





    public async Task<IEnumerable<EMISchedule>> GetLatestEMISchedule(Guid id)
    {
        var logs = await _applicationStatusLogRepository.GetAllAsync(c => c.ApplicationId == id && c.IsDeleted == false);

        var latestLog = logs
            .OrderByDescending(log => log.CreatedOn)
            .FirstOrDefault();

        if (latestLog != null && latestLog.StatusId == new Guid("e24d24a8-fc69-4527-a92a-97f6648a43c5"))
        {
            var schedule = await _eMIScheduleRepository.GetAllAsync(c => c.LoanApplicationId == id && c.IsDeleted == false);
            return schedule.OrderBy(c => c.EndDate);
        }

        return Enumerable.Empty<EMISchedule>();
    }







}
