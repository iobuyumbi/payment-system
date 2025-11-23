using AutoMapper;
using FluentValidation;
using FluentValidation.TestHelper;
using Microsoft.AspNetCore.Http;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Solidaridad.Application.Helpers;
using Solidaridad.Application.Models;
using Solidaridad.Application.Models.Farmer;
using Solidaridad.Application.Models.PaymentBatch;
using Solidaridad.Application.Models.Validators.Farmer;
using Solidaridad.Core.Entities;
using Solidaridad.Core.Entities.Base;
using Solidaridad.Core.Entities.Excel.Import;
using Solidaridad.DataAccess.Migrations;
using Solidaridad.DataAccess.Repositories;
using Solidaridad.DataAccess.Repositories.Impl;
using Solidaridad.Shared.Services;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using Solidaridad.Application.Models.ExcelImport;
using Solidaridad.Application.Models.ExcelExport;
using Solidaridad.Core.Enums;
using System.Text.RegularExpressions;
namespace Solidaridad.Application.Services.Impl;

public class FarmerService : IFarmerService
{
    #region DI
    private readonly HttpClient _httpClient;
    private readonly IMapper _mapper;
    private readonly IAddressRepository _addressRepository;
    private readonly IFarmerRepository _farmerRepository;
    private readonly ICountryRepository _countryRepository;
    private readonly IAdminLevel1Repository _countyRepository;
    private readonly IAdminLevel2Repository _subCountyRepository;
    private readonly IAdminLevel3Repository _wardRepository;
    private readonly IAdminLevel4Repository _villageRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly ICooperativeRepository _cooperativeRepository;
    private readonly IFarmerCooperativeRepository _farmerCooperativeRepository;
    private readonly IClaimService _claimService;
    private readonly ImportFarmerValidator _importFarmerValidator;
    private readonly IExcelImportDetailRepository _excelImportDetailRepository;
    private readonly CreateFarmerValidator _createFarmerValidator;
    private readonly UpdateFarmerValidator _updateFarmerValidator;
    private readonly IDocumentTypeRepository _documentTypeRepository;
    private IPaymentRequestDeductibleRepository _paymentRequestDeductibleRepository;
    private readonly IApiRequestLogRepository _apiRequestLogRepository;
    private readonly IPaymentBatchRepository _paymentBatchRepository;
    private readonly ApiService _apiService;


    public FarmerService(HttpClient httpClient, IMapper mapper, IFarmerRepository farmerRepository,
        IPaymentBatchRepository paymentBatchRepository,
        IDocumentTypeRepository documentTypeRepository, IApiRequestLogRepository apiRequestLogRepository,
        IAddressRepository addressRepository, IPaymentRequestDeductibleRepository paymentRequestDeductibleRepository,
        ICountryRepository countryRepository, IAdminLevel1Repository countyRepository, IExcelImportDetailRepository excelImportDetailRepository, IAdminLevel2Repository subCountyRepository, IAdminLevel3Repository wardRepository, IAdminLevel4Repository villageRepository, IProjectRepository projectRepository, ICooperativeRepository cooperativeRepository,
        IFarmerCooperativeRepository farmerCooperativeRepository,
        IClaimService claimService, ApiService apiService)
    {
        _mapper = mapper;
        _farmerRepository = farmerRepository;
        _addressRepository = addressRepository;
        _countryRepository = countryRepository;
        _projectRepository = projectRepository;
        _cooperativeRepository = cooperativeRepository;
        _farmerCooperativeRepository = farmerCooperativeRepository;
        _documentTypeRepository = documentTypeRepository;
        _claimService = claimService;
        _countyRepository = countyRepository;
        _wardRepository = wardRepository;
        _subCountyRepository = subCountyRepository;
        _villageRepository = villageRepository;
        _importFarmerValidator = new ImportFarmerValidator();
        _createFarmerValidator = new CreateFarmerValidator();
        _updateFarmerValidator = new UpdateFarmerValidator();
        _excelImportDetailRepository = excelImportDetailRepository;
        _paymentRequestDeductibleRepository = paymentRequestDeductibleRepository;
        _httpClient = httpClient;
        _apiRequestLogRepository = apiRequestLogRepository;
        _paymentBatchRepository = paymentBatchRepository;
        _apiService = apiService;
    }
    #endregion

    #region Methods
    public async Task<ImportFarmerResponseModel> ImportAsync(ImportFarmerModel importFarmerModel)
    {
        try
        {
            var farmer = _mapper.Map<Farmer>(importFarmerModel);
            farmer.EnumerationDate = ParseUtility.ParseDateTimeValue(farmer.EnumerationDate);
            //if (string.IsNullOrEmpty(importFarmerModel.CountryName))
            //    throw new Exception("Unable to create farmer. Country does not exist");

            //if (string.IsNullOrEmpty(importFarmerModel.ProjectName))
            //    throw new Exception("Unable to create farmer. Project does not exist");

            //if (string.IsNullOrEmpty(importFarmerModel.CooperativeName))
            //    throw new Exception("Unable to create farmer. Cooperative does not exist");

            // 1. Save country get country Id
            var countryExist = await _countryRepository.GetAllAsync(c => c.CountryName.ToLower() == importFarmerModel.CountryName.ToLower());
            if (countryExist != null && countryExist.Any())
            {
                farmer.CountryId = countryExist.FirstOrDefault().Id;
            }
            else
            {
                var country = new Country
                {
                    CountryName = importFarmerModel.CountryName
                };
                var countrySaved = await _countryRepository.AddAsync(country);
                if (countrySaved != null)
                {
                    farmer.CountryId = countrySaved.Id;
                }
            }

            // 3. Save cooperative get cooperative Id
            var copExist = await _cooperativeRepository.GetAllAsync(c => c.Name.ToLower() == importFarmerModel.CooperativeName.ToLower());
            //if (copExist != null && copExist.Any())
            //{
            //    farmer.CooperativeId = copExist.FirstOrDefault().Id;
            //}
            //else
            //{
            //    var cooperative = new Cooperative
            //    {
            //        Name = importFarmerModel.CooperativeName,
            //        CountryId = farmer.CountryId
            //    };
            //    var copSaved = await _cooperativeRepository.AddAsync(cooperative);
            //    if (copSaved != null)
            //    {
            //        farmer.CooperativeId = copSaved.Id;
            //    }
            //}


            var farmerExist = await _farmerRepository.GetAllAsync(ti => ti.BeneficiaryId.ToLower() == importFarmerModel.BeneficiaryId.ToLower());
            if (farmerExist == null || !farmerExist.Any())
            {
                farmer.EnumerationDate = DateTime.UtcNow;
                farmer.DateOfBirth = DateTime.UtcNow;
                farmer.CreatedOn = DateTime.UtcNow;

                var addedFarmer = await _farmerRepository.AddAsync(farmer);
                return new ImportFarmerResponseModel
                {
                    Id = addedFarmer.Id
                };
            }
            else
            {
                return new ImportFarmerResponseModel
                {
                    Id = (await _farmerRepository.UpdateAsync(farmer)).Id
                };
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public async Task<ImportFarmerResponseModel> ImportByApiAsync(ImportFarmerModel importFarmerModel)
    {
        try
        {
            // var farmer = _mapper.Map<Farmer>(importFarmerModel);
            // farmer.EnumerationDate = ParseUtility.ParseDateTimeValue(farmer.EnumerationDate);

            var farmer = new Farmer
            {
                FirstName = importFarmerModel.FirstName,
                OtherNames = importFarmerModel.OtherNames,
                DateOfBirth = importFarmerModel.DateOfBirth.HasValue ? Convert.ToDateTime(importFarmerModel.DateOfBirth.Value.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")) : null,
                Mobile = importFarmerModel.Mobile,
                AlternateContactNumber = importFarmerModel.AlternateContactNumber,
                SystemId = importFarmerModel.SystemId,
                ParticipantId = importFarmerModel.ParticipantId,
                EnumerationDate = importFarmerModel.EnumerationDate.HasValue ? Convert.ToDateTime(importFarmerModel.EnumerationDate.Value.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")) : null,
                HasDisability = importFarmerModel.HasDisability,
                AccessToMobile = importFarmerModel.AccessToMobile,
                PaymentPhoneNumber = importFarmerModel.PaymentPhoneNumber,
                IsFarmerPhoneOwner = importFarmerModel.IsFarmerPhoneOwner,
                PhoneOwnerName = importFarmerModel?.PhoneOwnerName,
                PhoneOwnerAddress = importFarmerModel?.PhoneOwnerAddress,
                PhoneOwnerNationalId = importFarmerModel?.PhoneOwnerNationalId,
                PhoneOwnerRelationWithFarmer = importFarmerModel.PhoneOwnerRelationWithFarmer,
                Gender = importFarmerModel.Gender,
                BeneficiaryId = importFarmerModel.BeneficiaryId,
                Source = RecordSource.API.ToString()
                //CreatedOn = DateTime.UtcNow
            };

            // 1. Save country get country Id
            var countryExist = await _countryRepository.GetAllAsync(c => c.CountryName.ToLower() == importFarmerModel.CountryName.ToLower());
            if (countryExist != null && countryExist.Any())
            {
                farmer.CountryId = countryExist.FirstOrDefault().Id;
            }
            else
            {
                throw new Exception("Country not found");
            }

            // 2. Save or retrieve county ID
            var countyExist = await _countyRepository.GetAllAsync(c => c.CountyName.ToLower() == importFarmerModel.AdminLevel1.ToLower());
            if (countyExist != null && countyExist.Any())
            {
                farmer.AdminLevel1Id = countyExist.FirstOrDefault().Id;
            }
            else
            {
                throw new Exception("Admin Level 1 not found");
            }

            // 3. Save or retrieve subcounty ID
            var subCountyExist = await _subCountyRepository.GetAllAsync(c => c.SubCountyName.ToLower() == importFarmerModel.AdminLevel2.ToLower());
            if (subCountyExist != null && subCountyExist.Any())
            {
                farmer.AdminLevel2Id = subCountyExist.FirstOrDefault().Id;
            }
            else
            {
                throw new Exception("Admin Level 2 not found");
            }

            // 4. Save or retrieve ward ID
            var wardExist = await _wardRepository.GetAllAsync(c => c.WardName.ToLower() == importFarmerModel.AdminLevel3.ToLower());
            if (wardExist != null && wardExist.Any())
            {
                farmer.AdminLevel3Id = wardExist.FirstOrDefault().Id;
            }
            else
            {
                throw new Exception("Admin Level 3 not found");
            }

            // 5. Save or retrieve village ID
            var villageExist = await _villageRepository.GetAllAsync(c => c.VillageName.ToLower() == importFarmerModel.AdminLevel4.ToLower());
            //if (villageExist != null && villageExist.Any())
            //{
            //    farmer.AdminLevel4Id = villageExist.FirstOrDefault().Id;
            //}
            //else
            //{
            //    throw new Exception("Admin Level 4 not found");
            //}

            // 6. Save cooperative get cooperative Id
            var copExist = await _cooperativeRepository.GetAllAsync(c => c.Name.ToLower() == importFarmerModel.CooperativeName.ToLower());
            //if (copExist != null && copExist.Any())
            //{
            //    farmer.CooperativeId = copExist.FirstOrDefault().Id;
            //}
            //else
            //{
            //    throw new Exception("Cooperative not found");
            //}


            var farmerExist = await _farmerRepository.GetAllAsync(ti => ti.BeneficiaryId.ToLower() == importFarmerModel.BeneficiaryId.ToLower());
            if (farmerExist == null || !farmerExist.Any())
            {


                farmer.EnumerationDate = farmer.EnumerationDate.HasValue ? farmer.EnumerationDate.Value.ToUniversalTime() : null;
                farmer.DateOfBirth = farmer.DateOfBirth.HasValue ? farmer.EnumerationDate.Value.ToUniversalTime() : null;

                var addedFarmer = await _farmerRepository.AddAsync(farmer);
                return new ImportFarmerResponseModel
                {
                    Id = addedFarmer.Id
                };
            }
            else
            {
                return new ImportFarmerResponseModel
                {
                    Id = (await _farmerRepository.UpdateAsync(farmer)).Id
                };
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public async Task<CreateFarmerResponseModel> CreateAsync(CreateFarmerModel createFarmerModel)
    {
        try
        {
            var validationResult = _createFarmerValidator.Validate(createFarmerModel);
            if (!validationResult.IsValid)
            {
                throw new ValidationException("Validation failed", validationResult.Errors);
            }

            var phoneCheck = PhoneNumberHelper.NormalizeAndValidatePhoneNumber(createFarmerModel.PaymentPhoneNumber, createFarmerModel.CountryId);

            if (phoneCheck.IsValid)
            {
                createFarmerModel.PaymentPhoneNumber = phoneCheck.NormalizedNumber;
            }
            else
            {
                throw new ValidationException("Invalid payment phone number");
            }


            var farmer = _mapper.Map<Farmer>(createFarmerModel);
            farmer.CreatedOn = DateTime.UtcNow;
            farmer.CreatedBy = new Guid(_claimService.GetUserId());
            farmer.DocumentTypeId = string.IsNullOrEmpty(createFarmerModel.DocumentTypeId) ? null : Guid.Parse(createFarmerModel.DocumentTypeId);

            var addedFarmer = await _farmerRepository.AddAsync(farmer);

            if (addedFarmer != null)
            {
                // 1. add cooperatives
                var farmerCooperative = new List<FarmerCooperative>();
                await _apiService.CreateContactAsync(createFarmerModel);
                foreach (var item in createFarmerModel.Cooperative)
                {
                    farmerCooperative.Add(new FarmerCooperative
                    {
                        FarmerId = addedFarmer.Id,
                        CooperativeId = Guid.Parse(item.Value)
                    });
                }

                await _farmerCooperativeRepository.AddRange(farmerCooperative);

                // 2. add projects
                await _farmerRepository.AddFarmerProjectAsync(addedFarmer.Id, createFarmerModel.ProjectIds);
            }

            return new CreateFarmerResponseModel
            {
                Id = addedFarmer.Id
            };
        }
        catch (ValidationException ex)
        {
            throw ex;
        }
    }

    public async Task<BaseResponseModel> DeleteAsync(Guid id)
    {
        var farmer = await _farmerRepository.GetFirstAsync(tl => tl.Id == id);
        farmer.IsDeleted = true;

        return new BaseResponseModel
        {
            Id = (await _farmerRepository.UpdateAsync(farmer)).Id
        };
    }

    public async Task<FarmerSearchResponseModel> GetAllAsync(FarmerSearchParams farmerSearchParams)
    {
        var _farmers = await _farmerRepository.GetAllAsync(c =>
           (string.IsNullOrEmpty(farmerSearchParams.Filter) ||
            c.FirstName.Contains(farmerSearchParams.Filter) ||
            c.OtherNames.Contains(farmerSearchParams.Filter) ||
            c.Mobile.Contains(farmerSearchParams.Filter))
            && (farmerSearchParams.CountryId == null || c.CountryId == farmerSearchParams.CountryId)
            && (farmerSearchParams.AdminLevel1Id == null || c.AdminLevel1Id == farmerSearchParams.AdminLevel1Id)
            && (farmerSearchParams.AdminLevel2Id == null || c.AdminLevel2Id == farmerSearchParams.AdminLevel2Id)
            && (farmerSearchParams.AdminLevel3Id == null || c.AdminLevel3Id == farmerSearchParams.AdminLevel3Id)
            && c.IsDeleted == false
        );

        if (_farmers != null && _farmers.Any())
        {
            var _payments = await _paymentRequestDeductibleRepository.GetAllAsync(c => 1 == 1);

            var _countries = await _countryRepository.GetAllAsync(c => c.IsActive == true);
            var _level1 = await _countyRepository.GetAllAsync(c => c.IsActive == true);
            var _level2 = await _subCountyRepository.GetAllAsync(c => c.IsActive == true);
            var _level3 = await _wardRepository.GetAllAsync(c => c.IsActive == true);


            int numberOfObjectsPerPage = farmerSearchParams.PageSize;

            var queryResultPage = _farmers
                .Skip(numberOfObjectsPerPage * (farmerSearchParams.PageNumber))
                .Take(numberOfObjectsPerPage);

            queryResultPage.ToList().ForEach(f => f.Country = _countries.FirstOrDefault(c => c.Id == f.CountryId));
            queryResultPage.ToList().ForEach(f => f.AdminLevel1 = _level1.FirstOrDefault(c => c.Id == f.AdminLevel1Id));
            queryResultPage.ToList().ForEach(f => f.AdminLevel2 = _level2.FirstOrDefault(c => c.Id == f.AdminLevel2Id));
            queryResultPage.ToList().ForEach(f => f.AdminLevel3 = _level3.FirstOrDefault(c => c.Id == f.AdminLevel3Id));


            var list = _mapper.Map<IEnumerable<FarmerResponseModel>>(queryResultPage).ToList();

            foreach (var farmer in list)
            {
                var cooperative = await GetCooperatives(farmer.Id);

                farmer.Cooperative = cooperative;
            }

            List<FarmerResponseModel> finalList = new List<FarmerResponseModel>();

            // Get projectid bt paymebtbatch
            var paymentBatch = await _paymentBatchRepository.GetFirstAsync(c => c.Id == farmerSearchParams.PaymentBatchId);
            if (paymentBatch != null)
            {
                farmerSearchParams.ProjectId = paymentBatch.ProjectId;
            }

            if (farmerSearchParams.ProjectId != null)
            {
                foreach (var farmer in list)
                {
                    var projects = await _farmerRepository.GetFarmerProjects((Guid)farmer.Id);

                    if (projects.Any(c => c.Id == farmerSearchParams.ProjectId))
                    {
                        finalList.Add(farmer);
                    }
                }
            }


            //foreach (var farmer in list)
            //{
            //    var projects = await GetCooperatives(farmer.Id);
            //    farmer.Cooperative = cooperative;
            //}

            // list.ForEach(async c => c.Cooperative = await GetCooperatives(c.Id));
            //list.ForEach(c =>
            //c.WalletBalance = _payments
            //    .Where(p => p.SystemId == c.SystemId)
            //    .Sum(p => p.FarmerEarningsShareLc));

            //string _contacts = await _apiService.GetAllContactAsync();
            //var contacts = JsonDocument.Parse(_contacts);

            //var contactPhoneNumbers = new HashSet<string>();
            //if (contacts.RootElement.ValueKind == JsonValueKind.Array) // Directly an array
            //{
            //    foreach (var contact in contacts.RootElement.EnumerateArray())
            //    {
            //        if (contact.TryGetProperty("phone_number", out var phoneProperty) && phoneProperty.ValueKind == JsonValueKind.String)
            //        {
            //            contactPhoneNumbers.Add(phoneProperty.GetString());
            //        }
            //    }
            //}
            //var documentType = await _documentTypeRepository.GetAllAsync(c=> true);
            //var documentTypeList = await _documentTypeRepository.GetAllAsync(c => true);

            //list.ForEach(c =>
            //{
            //    var matchingDocumentType = documentTypeList.FirstOrDefault(dt => dt.Id == c.DocumentTypeId);
            //    if (matchingDocumentType != null)
            //    {
            //        c.DocumentType = new SelectItemModel
            //        {
            //            Value = matchingDocumentType.Id.ToString(),
            //            Label = matchingDocumentType.TypeName
            //        };
            //    }
            //});

            //list.ForEach(c => {

            //    c.IsFarmerVerified = contactPhoneNumbers.Contains(c.Mobile);
            //    c.ValidationSource = "onafriq";
            //});

            var stats = new FarmerStatsModel { TotalFarmers = _farmers.Count, VerifiedFarmers = _farmers.Count(x => x.IsFarmerVerified), UnverifiedFarmers = _farmers.Count(x => !x.IsFarmerVerified) };
            FarmerSearchResponseModel result = new FarmerSearchResponseModel
            {
                Farmers = farmerSearchParams.ProjectId != null ? finalList : list,
                FarmerStats = stats,
                TotalCount = _farmers.Count(),
            };

            return result;
        }

        return null;
    }

    public async Task<IEnumerable<PaymentRequestDeductibleModel>> GetAllByBatchAsync(DeductibleExportModel model)
    {
        var paymentList = await _paymentRequestDeductibleRepository.GetAllAsync(
             ti =>
             ti.StatusId >= 0 &&
             ti.PaymentBatchId == model.BatchId && ti.IsDeleted == false
         );

        var list = _mapper.Map<IEnumerable<PaymentRequestDeductibleModel>>(paymentList);
        var farmers = await _farmerRepository.GetAllAsync(c => c.IsDeleted == false);

        //if (model.StatusId == 1)
        //{
        foreach (var payment in list)
        {
            // Fetch a single farmer or null if no match
            var _farmer = farmers.FirstOrDefault(c => c.SystemId == payment.SystemId);

            if (model.IsFarmerValid.HasValue)
            {
                _farmer = farmers.FirstOrDefault(c => c.SystemId == payment.SystemId);
            }
            payment.Farmer = _farmer != null
                ? _mapper.Map<FarmerResponseModel>(_farmer)
                : null; // Assign an empty object if no match found
        }
        //}
        var finalList = list.Where(p => p.Farmer != null).ToList();

        return finalList;
    }

    public async Task<IEnumerable<FarmerResponseModel>> GetAllByListIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var farmer = await _farmerRepository.GetAllAsync(ti => ti.Id == id && ti.IsDeleted == false);

        return _mapper.Map<IEnumerable<FarmerResponseModel>>(farmer);
    }

    public async Task<UpdateFarmerResponseModel> UpdateAsync(Guid id, UpdateFarmerModel updatefarmerModel)
    {
        try
        {
            var validationResult = _updateFarmerValidator.Validate(updatefarmerModel);
            if (!validationResult.IsValid)
            {

                throw new ValidationException("Validation failed", validationResult.Errors);
            }
            var farmer = await _farmerRepository.GetFirstAsync(ti => ti.Id == id);

            _mapper.Map(updatefarmerModel, farmer);

            farmer.UpdatedOn = DateTime.UtcNow;
            farmer.UpdatedBy = new Guid(_claimService.GetUserId());
            farmer.DocumentTypeId = string.IsNullOrEmpty(updatefarmerModel.DocumentTypeId) ? null : Guid.Parse(updatefarmerModel.DocumentTypeId);
            //farmer.DocumentTypeId = updatefarmerModel.DocumentType.Value != null ? new Guid(updatefarmerModel.DocumentType.Value) : null;

            var updatedfarmer = await _farmerRepository.UpdateAsync(farmer);
            var existingMaps = await _farmerCooperativeRepository.GetAllAsync(c => c.FarmerId == id);
            foreach (var map in existingMaps)
            {
                await _farmerCooperativeRepository.DeleteAsync(map);
            }

            if (updatedfarmer != null)
            {
                // 1. upsert cooperatives
                var farmerCooperative = new List<FarmerCooperative>();

                foreach (var item in updatefarmerModel.Cooperative)
                {
                    var cooperativeId = Guid.Parse(item.Value);
                    farmerCooperative.Add(new FarmerCooperative
                    {
                        FarmerId = id,
                        CooperativeId = cooperativeId
                    });
                }

                if (farmerCooperative.Any())
                {
                    await _farmerCooperativeRepository.AddRange(farmerCooperative);
                }

                // 2. upsert projects
                // 2. add projects
                await _farmerRepository.UpdateFarmerProjectAsync(updatedfarmer.Id, updatefarmerModel.ProjectIds);
            }

            return new UpdateFarmerResponseModel
            {
                Id = id
            };
        }
        catch (ValidationException ex)
        {
            throw ex;
        }
        catch (Exception ex)
        { throw ex; }
    }

    public async Task ImportFarmer(IFormFile file, Guid? id)
    {
        try
        {
            XSSFWorkbook hssfwb;
            using (var stream = file.OpenReadStream())
            {
                hssfwb = new XSSFWorkbook(stream);
            }

            ISheet sheet = hssfwb.GetSheet(Convert.ToString(ExelImportConstants.SHEET_FARMER));
            var _listFarmersExcel = new List<ImportFarmerModel>();
            var _listFarmers = new List<Farmer>();
            var _errorDetails = new List<ExcelImportDetail>();

            // check if sheet exists
            if (sheet == null)
            {
                _errorDetails.Add(new ExcelImportDetail
                {
                    Id = Guid.NewGuid(),
                    ExcelImportId = id ?? Guid.Empty,
                    TabName = Convert.ToString(ExelImportConstants.SHEET_FARMER),
                    RowNumber = 0,
                    IsSuccess = false,
                    Remarks = "Sheet 'Farmers List' is missing in the uploaded file"
                });
            }
            if (sheet != null)
            {
                DataFormatter formatter = new DataFormatter();
                for (int row = 1; row <= sheet.LastRowNum; row++)
                {
                    if (sheet.GetRow(row) != null) //null is when the row only contains empty cells 
                    {
                        //int col = 0;
                        _listFarmersExcel.Add(new ImportFarmerModel
                        {
                            FirstName = ParseUtility.ParseAnyValueToString(formatter.FormatCellValue(sheet.GetRow(row).GetCell(2))),
                            OtherNames = ParseUtility.ParseAnyValueToString(formatter.FormatCellValue(sheet.GetRow(row).GetCell(3))),
                            Mobile = ParseUtility.ParseAnyValueToString(formatter.FormatCellValue(sheet.GetRow(row).GetCell(19))),
                            SystemId = ParseUtility.ParseAnyValueToString(formatter.FormatCellValue(sheet.GetRow(row).GetCell(0))),
                            CountryName = ParseUtility.ParseAnyValueToString(formatter.FormatCellValue(sheet.GetRow(row).GetCell(5))),
                            AdminLevel1 = ParseUtility.ParseAnyValueToString(formatter.FormatCellValue(sheet.GetRow(row).GetCell(6))),
                            AdminLevel2 = ParseUtility.ParseAnyValueToString(formatter.FormatCellValue(sheet.GetRow(row).GetCell(7))),
                            AdminLevel3 = ParseUtility.ParseAnyValueToString(formatter.FormatCellValue(sheet.GetRow(row).GetCell(8))),
                            AdminLevel4 = ParseUtility.ParseAnyValueToString(formatter.FormatCellValue(sheet.GetRow(row).GetCell(9))),
                            CooperativeName = ParseUtility.ParseAnyValueToString(formatter.FormatCellValue(sheet.GetRow(row).GetCell(24))),
                            ParticipantId = DateTime.Now.Ticks.ToString(),
                            HasDisability = false,
                            AccessToMobile = true,
                            PaymentPhoneNumber = ParseUtility.ParseAnyValueToString(formatter.FormatCellValue(sheet.GetRow(row).GetCell(19))),
                            IsFarmerPhoneOwner = true,
                            Gender = 1,
                            BeneficiaryId = ParseUtility.ParseAnyValueToString(formatter.FormatCellValue(sheet.GetRow(row).GetCell(1))),
                        });
                    }
                }
                try
                {

                    var farmers = await _farmerRepository.GetAllAsync(c => 1 == 1);
                    int r = 1;
                    foreach (var importFarmerModel in _listFarmersExcel)
                    {
                        var errorMessages = new List<string>();

                        var farmer = new Farmer
                        {
                            FirstName = importFarmerModel.FirstName,
                            OtherNames = importFarmerModel.OtherNames,
                            Mobile = importFarmerModel.Mobile,
                            SystemId = importFarmerModel.SystemId,
                            ParticipantId = importFarmerModel.ParticipantId,
                            HasDisability = importFarmerModel.HasDisability,
                            AccessToMobile = importFarmerModel.AccessToMobile,
                            PaymentPhoneNumber = importFarmerModel.PaymentPhoneNumber,
                            IsFarmerPhoneOwner = importFarmerModel.IsFarmerPhoneOwner,
                            Gender = importFarmerModel.Gender,
                            BeneficiaryId = importFarmerModel.BeneficiaryId,
                            Source = RecordSource.Excel.ToString(),
                            CreatedBy = Guid.Parse(_claimService.GetUserId()),
                            CreatedOn = DateTime.UtcNow
                        };



                        // 1. Save or retrieve country ID
                        var countryExist = await _countryRepository.GetAllAsync(c => c.CountryName.ToLower() == importFarmerModel.CountryName.ToLower());
                        if (countryExist != null && countryExist.Any())
                        {
                            farmer.CountryId = countryExist.FirstOrDefault().Id;
                        }
                        else
                        {
                            errorMessages.Add("The country specified is invalid or empty");
                            var _message = string.Join(", ", errorMessages);
                            var errorDetails = new ExcelImportDetail
                            {
                                Id = Guid.NewGuid(),
                                ExcelImportId = (Guid)id,
                                TabName = "",
                                RowNumber = r,
                                IsSuccess = false,
                                Remarks = _message,
                            };
                            _errorDetails.Add(errorDetails);
                            continue;
                            //var country = new Country
                            //{
                            //    CountryName = importFarmerModel.CountryName
                            //};
                            //var countrySaved = await _countryRepository.AddAsync(country);
                            //if (countrySaved != null)
                            //{
                            //    farmer.CountryId = countrySaved.Id;
                            //}
                        }

                        // 2. Save or retrieve county ID
                        if (string.IsNullOrWhiteSpace(importFarmerModel.AdminLevel1))
                        {
                            errorMessages.Add("Admin level 1 not defined");
                        }
                        else
                        {
                            var countyExist = await _countyRepository.GetAllAsync(c => c.CountyName.ToLower() == importFarmerModel.AdminLevel1.ToLower());
                            if (countyExist != null && countyExist.Any())
                            {
                                farmer.AdminLevel1Id = countyExist.FirstOrDefault().Id;
                            }
                            else
                            {
                                var county = new AdminLevel1
                                {
                                    CountyName = importFarmerModel.AdminLevel1,
                                    CountryId = farmer.CountryId
                                };
                                var countySaved = await _countyRepository.AddAsync(county);
                                if (countySaved != null)
                                {
                                    farmer.AdminLevel1Id = countySaved.Id;
                                }
                            }
                        }

                        // 3. Save or retrieve subcounty ID
                        if (string.IsNullOrWhiteSpace(importFarmerModel.AdminLevel2))
                        {
                            errorMessages.Add("Admin level 2 not defined");
                        }
                        else
                        {
                            var subCountyExist = await _subCountyRepository.GetAllAsync(c => c.SubCountyName.ToLower() == importFarmerModel.AdminLevel2.ToLower());
                            if (subCountyExist != null && subCountyExist.Any())
                            {
                                farmer.AdminLevel2Id = subCountyExist.FirstOrDefault().Id;
                            }
                            else
                            {
                                var subCounty = new AdminLevel2
                                {
                                    SubCountyName = importFarmerModel.AdminLevel2,
                                    CountyId = farmer.AdminLevel1Id
                                };
                                var subCountySaved = await _subCountyRepository.AddAsync(subCounty);
                                if (subCountySaved != null)
                                {
                                    farmer.AdminLevel2Id = subCountySaved.Id;
                                }
                            }
                        }

                        // 4. Save or retrieve ward ID
                        if (string.IsNullOrWhiteSpace(importFarmerModel.AdminLevel3))
                        {
                            errorMessages.Add("Admin level 3 not defined");
                        }
                        else
                        {
                            var wardExist = await _wardRepository.GetAllAsync(c => c.WardName.ToLower() == importFarmerModel.AdminLevel3.ToLower());
                            if (wardExist != null && wardExist.Any())
                            {
                                farmer.AdminLevel3Id = wardExist.FirstOrDefault().Id;
                            }
                            else
                            {
                                var ward = new AdminLevel3
                                {
                                    WardName = importFarmerModel.AdminLevel3,
                                    SubCountyId = farmer.AdminLevel2Id
                                };
                                var wardSaved = await _wardRepository.AddAsync(ward);
                                if (wardSaved != null)
                                {
                                    farmer.AdminLevel3Id = wardSaved.Id;
                                }
                            }
                        }

                        // 5. Save or retrieve village ID
                        //var villageExist = await _villageRepository.GetAllAsync(c => c.VillageName.ToLower() == importFarmerModel.AdminLevel4.ToLower());


                        // 6. Save or retrieve cooperative ID
                        //var copExist = await _cooperativeRepository.GetAllAsync(c => c.Name.ToLower() == importFarmerModel.CooperativeName.ToLower());

                        var message = "";
                        if (farmer != null)
                        {
                            if (farmer.CountryId == Guid.Empty || farmer.CountryId == null)
                            {
                                errorMessages.Add("The country specified is invalid or empty");
                            }
                            else if (farmers.Any(f => f.SystemId == farmer.SystemId))
                            {
                                errorMessages.Add("Farmer with current system id already exists");
                            }
                            else if (farmers.Any(f => f.Mobile == farmer.Mobile))
                            {
                                errorMessages.Add("Farmer with current Mobile already exists");
                            }
                            else if (farmer.FirstName == null || farmer.FirstName == "")
                            {
                                errorMessages.Add("First name is not provided");
                            }
                            else if (farmer.OtherNames == null || farmer.OtherNames == "")
                            {
                                errorMessages.Add("Other names is not provided");
                            }
                            else if (farmer.Mobile == null || farmer.Mobile == "")
                            {
                                errorMessages.Add("Mobile number is not provided");
                            }
                            else if (farmer.SystemId == null || farmer.SystemId == "")
                            {
                                errorMessages.Add("System id  is not provided");
                            }
                            else if (farmer.ParticipantId == null || farmer.ParticipantId == "")
                            {
                                errorMessages.Add("Participant id is not provided");
                            }
                            else if (farmer.HasDisability == null)
                            {
                                errorMessages.Add("Disability coloumn is not provided");
                            }
                            else if (farmer.AccessToMobile == null)
                            {
                                errorMessages.Add("Access to mobile coloumn is not provided");
                            }
                            else if (farmer.PaymentPhoneNumber == null || farmer.PaymentPhoneNumber == "")
                            {
                                errorMessages.Add("Payment phone number is not provided");
                            }
                            else if (farmer.IsFarmerPhoneOwner == null)
                            {
                                errorMessages.Add("Is farmer phone owner coloumn is not provided");
                            }
                            else if (farmer.Gender == null || farmer.Gender == 0)
                            {
                                errorMessages.Add("Gender is not provided");
                            }
                            else if (farmer.ParticipantId == null || farmer.ParticipantId == "")
                            {
                                errorMessages.Add("Participant id is not provided");
                            }

                            // validate phone number
                            if (farmer.CountryId != Guid.Empty)
                            {
                                var phoneCheck = PhoneNumberHelper.NormalizeAndValidatePhoneNumber(farmer.Mobile, farmer.CountryId);

                                if (phoneCheck.IsValid)
                                {
                                    farmer.Mobile = phoneCheck.NormalizedNumber;
                                }
                                else
                                {
                                    errorMessages.Add("Invalid mobile number");
                                }
                            }
                            else
                            {
                                errorMessages.Add("Country is invalid or not provided");
                            }

                            // validate payment phone number
                            if (farmer.CountryId != Guid.Empty)
                            {
                               
                            }
                            else
                            {
                                errorMessages.Add("Country is invalid or not provided");
                            }

                            if (errorMessages.Any())
                            {
                                message = string.Join(", ", errorMessages);
                                var errorDetails = new ExcelImportDetail
                                {
                                    Id = Guid.NewGuid(),
                                    ExcelImportId = (Guid)id,
                                    TabName = "",
                                    RowNumber = r,
                                    IsSuccess = false,
                                    Remarks = message,
                                };
                                _errorDetails.Add(errorDetails);
                            }
                            else
                            {
                                _listFarmers.Add(farmer);
                            }
                        }
                    }

                }
                catch (Exception ex) { }

                var newList = _listFarmers.Where(c => !string.IsNullOrEmpty(c.Mobile));

                // 1. get existing farmers
                var existingList = await _farmerRepository.GetAllAsync(c => 1 == 1);

                // 2. remove elements with duplicate systemId and save new records
                var listToAdd = newList.Except(existingList, new SystemIdComparer()).ToList();
                await _farmerRepository.AddRange(listToAdd);

                // 3. get elements with duplicate systemId and update them
                var listToUpdate = newList.Except(listToAdd);
                foreach (var item in listToUpdate)
                {
                    var farmer = existingList.FirstOrDefault(ti => ti.SystemId == item.SystemId);
                    if (farmer != null)
                    {
                        farmer.FirstName = item.FirstName;
                        farmer.UpdatedBy = Guid.Parse(_claimService.GetUserId());
                        farmer.UpdatedOn = DateTime.UtcNow;
                    }

                    await _farmerRepository.UpdateAsync(farmer);
                }
                //await _farmerRepository.UpdateRange(listToUpdate);
                foreach (var item in listToAdd)
                {
                    await _apiService.CreateImportContactAsync(item);
                }
            }

            await _excelImportDetailRepository.AddRange(_errorDetails);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public async Task<IEnumerable<SelectItemModel>> GetCooperatives(Guid? farmerId)
    {
        var _coop = await _cooperativeRepository.GetAllAsync(c => c.IsDeleted == false);
        var _farmerCoops = await _farmerCooperativeRepository.GetAllAsync(c => c.FarmerId == farmerId);

        var list = from c in _coop
                   join fc in _farmerCoops on c.Id equals fc.CooperativeId
                   select new SelectItemModel
                   {
                       Label = c.Name,
                       Value = Convert.ToString(c.Id)
                   };

        return list.OrderBy(c => c.Value);
    }

    public async Task<IEnumerable<SelectItemModel>> GetProjects(Guid farmerId)
    {
        var _farmerCoops = await _farmerRepository.GetFarmerProjects(farmerId);

        var result = from c in _farmerCoops
                     select new SelectItemModel
                     {
                         Label = c.ProjectName,
                         Value = Convert.ToString(c.Id)
                     };

        return result.OrderBy(c => c.Label);
    }

    public async Task<IEnumerable<SelectItemModel>> GetMasterDocumentTypes()
    {
        var _docTypes = await _documentTypeRepository.GetAllAsync(c => 1 == 1);

        var list = from c in _docTypes
                   select new SelectItemModel
                   {
                       Label = c.TypeName,
                       Value = Convert.ToString(c.Id)
                   };

        return list.OrderBy(c => c.Value);
    }







    #endregion
}


public class PhoneNumberResult
{
    public bool IsValid { get; set; }
    public string? NormalizedNumber { get; set; }
    public string? ErrorMessage { get; set; }
}

public static class PhoneNumberHelper
{
    private static readonly Dictionary<Guid, string> CountryCodeMap = new()
    {
        { Guid.Parse("a82beb82-2d92-11ef-ad6b-46477cdd49d1"), "254" }, // Kenya
        { Guid.Parse("a82c10f6-2d92-11ef-ad6b-46477cdd49d1"), "256" }, // Uganda
        { Guid.Parse("e31fb4b1-1916-43ef-a86a-c4c32d20fec4"), "255" }  // Tanzania
    };

    public static PhoneNumberResult NormalizeAndValidatePhoneNumber(string rawNumber, Guid countryId)
    {
        if (!CountryCodeMap.TryGetValue(countryId, out var countryCode))
        {
            return new PhoneNumberResult
            {
                IsValid = false,
                ErrorMessage = "Unsupported country ID."
            };
        }

        if (string.IsNullOrWhiteSpace(rawNumber))
        {
            return new PhoneNumberResult
            {
                IsValid = false,
                ErrorMessage = "Phone number is required."
            };
        }

        // Clean up input
        string phone = rawNumber.Trim()
            .Replace(" ", "")
            .Replace("-", "")
            .Replace("+", "");

        // Normalize based on input length and starting digits
        if (phone.StartsWith("0") && phone.Length == 10)
        {
            // local format, replace leading 0 with country code
            phone = countryCode + phone.Substring(1);
        }
        else if ((phone.Length == 9 || phone.Length == 10) && (phone.StartsWith("6") || phone.StartsWith("7")))
        {
            // assume missing code, prepend it
            phone = countryCode + phone;
        }
        else if (phone.StartsWith(countryCode) && phone.Length == (countryCode.Length + 9))
        {
            // already correct
        }
        else
        {
            return new PhoneNumberResult
            {
                IsValid = false,
                ErrorMessage = "Unrecognized or invalid phone number format."
            };
        }

        string fullWithPlus = "+" + phone;

        // Validate using regex: +{countryCode}[6|7]XXXXXXXX
        string pattern = countryCode switch
        {
            "254" => @"^\+2547\d{8}$",     // Kenya
            "256" => @"^\+2567\d{8}$",     // Uganda
            "255" => @"^\+255[6-7]\d{8}$", // Tanzania
            _ => ""
        };

        if (!Regex.IsMatch(fullWithPlus, pattern))
        {
            return new PhoneNumberResult
            {
                IsValid = false,
                ErrorMessage = "Phone number format is not valid for selected country."
            };
        }

        return new PhoneNumberResult
        {
            IsValid = true,
            NormalizedNumber = fullWithPlus
        };
    }
}
