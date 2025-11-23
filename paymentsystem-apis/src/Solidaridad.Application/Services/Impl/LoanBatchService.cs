using AutoMapper;
using Dapper;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Solidaridad.Application.Models;
using Solidaridad.Application.Models.AttachmentUpload;
using Solidaridad.Application.Models.LoanApplication;
using Solidaridad.Application.Models.LoanBatch;
using Solidaridad.Application.Models.PaymentBatch;
using Solidaridad.Application.Models.Project;
using Solidaridad.Application.Models.Validators.LoanBatch;
using Solidaridad.Core.Entities.Base;
using Solidaridad.Core.Entities.Loans;
using Solidaridad.DataAccess.Repositories;
using Solidaridad.DataAccess.Repositories.Impl;
using Solidaridad.Shared.Services;
using System.Collections.Generic;
using System.Linq;

namespace Solidaridad.Application.Services.Impl;

public class LoanBatchService : ILoanBatchService
{
    #region DI
    private readonly IMapper _mapper;
    private readonly string _connectionString;
    private readonly ILoanBatchRepository _loanBatchRepository;
    private readonly ILoanBatchItemRepository _loanBatchItemRepository;
    private readonly IMasterLoanItemRepository _loanItemRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly ILoanApplicationRepository _loanApplicationRepository;
    private readonly ILoanBatchProcessingFeeRepository _loanBatchProcessingFeeRepository;
    private readonly IClaimService _claimService;
    private readonly IConfiguration _configuration;
    private readonly CreateLoanBatchValidator _createLoanBatchValidator;
    private readonly UpdateLoanBatchValidator _updateLoanBatchValidator;

    private readonly IAttachmentUploadRepository _attachmentUploadRepository;

    private readonly IBatchAttachmentMappingRepository _batchAttachmentMappingRepository;


    public LoanBatchService(IMapper mapper, ILoanBatchRepository loanBatchRepository, IProjectRepository projectRepository,
        ILoanBatchItemRepository loanBatchItemRepository, IConfiguration configuration,
        IMasterLoanItemRepository loanItemRepository, ILoanApplicationRepository loanApplicationRepository,
        IClaimService claimService, ILoanBatchProcessingFeeRepository loanBatchProcessingFeeRepository,
        IAttachmentUploadRepository attachmentUploadRepository, IBatchAttachmentMappingRepository batchAttachmentMappingRepository)
    {
        _mapper = mapper;
        _loanBatchRepository = loanBatchRepository;
        _loanBatchItemRepository = loanBatchItemRepository;
        _loanItemRepository = loanItemRepository;
        _projectRepository = projectRepository;
        _claimService = claimService;
        _configuration = configuration;

        _connectionString = _configuration.GetSection("ConnectionStrings").Get<DatabaseConfiguration>().DefaultConnection;
        _createLoanBatchValidator = new CreateLoanBatchValidator();
        _updateLoanBatchValidator = new UpdateLoanBatchValidator();
        _loanBatchProcessingFeeRepository = loanBatchProcessingFeeRepository;
        _loanApplicationRepository = loanApplicationRepository;
        _attachmentUploadRepository = attachmentUploadRepository;
        _batchAttachmentMappingRepository = batchAttachmentMappingRepository;
    }
    #endregion

    #region Loan Batch
    public async Task<CreateLoanBatchResponseModel> CreateAsync(CreateLoanBatchModel createLoanBatchModel)
    {
        try
        {
            var validationResult = _createLoanBatchValidator.Validate(createLoanBatchModel);
            if (!validationResult.IsValid)
            {

                throw new ValidationException("Validation failed", validationResult.Errors);
            }

            var loanBatch = _mapper.Map<LoanBatch>(createLoanBatchModel);
            loanBatch.InitiatedBy = new Guid(_claimService.GetUserId());
            loanBatch.CreatedBy = Guid.Parse(_claimService.GetUserId());
            loanBatch.CreatedOn = DateTime.UtcNow;
            loanBatch.StageText = "Initiated";
            loanBatch.EffectiveDate = DateTime.UtcNow;
            loanBatch.InitiationDate = DateTime.UtcNow;

            var addedBatch = await _loanBatchRepository.AddAsync(loanBatch);

            // add multi processing fee
            if (addedBatch.Id != Guid.Empty)
            {
                var list = new List<LoanBatchProcessingFee>();
                foreach (var item in createLoanBatchModel.ProcessingFees)
                {
                    list.Add(new LoanBatchProcessingFee
                    {
                        Value = item.Value,
                        FeeName = item.FeeName,
                        FeeType = item.FeeType,
                        LoanBatchId = addedBatch.Id
                    });
                }
                await _loanBatchRepository.AddRangeLoanBatchProcessingFeeAsync(list);
            }

            return new CreateLoanBatchResponseModel
            {
                Id = addedBatch.Id
            };
        }
        catch (ValidationException ex)
        {
            throw ex;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public async Task<BaseResponseModel> DeleteAsync(Guid id)
    {
        var loanBatch = await _loanBatchRepository.GetFirstAsync(tl => tl.Id == id);
        if (loanBatch != null)
        {
            loanBatch.IsDeleted = true;
        }

        return new BaseResponseModel
        {
            Id = (await _loanBatchRepository.UpdateAsync(loanBatch)).Id
        };
    }

    public async Task<LoanBatchResponseModel> GetSingle(Guid id)
    {
        var loanBatch = await _loanBatchRepository.GetSingle(id);
        var applications = await _loanApplicationRepository.GetAllAsync(c => c.IsDeleted == false);
        var _loanBatch = _mapper.Map<LoanBatchResponseModel>(loanBatch);

        _loanBatch.TotalApplications = applications.Count(c => c.LoanBatchId == loanBatch.Id && c.Status != new Guid("6f103a88-8443-45ad-9c37-afe07f6b48e1") && c.IsDeleted == false);
        
        _loanBatch.TotalBatchAmount = (float)applications.Where(c => c.LoanBatchId == _loanBatch.Id && (c.Status == new Guid("e24d24a8-fc69-4527-a92a-97f6648a43c5")
        || c.Status == new Guid("f49faffa-b113-4546-ac7f-485164e5a822")) && c.IsDeleted == false)
.Sum(c => c.PrincipalAmount);
        ;
        _loanBatch.TotalDraft = applications.Count(c => c.Status == new Guid("6f103a88-8443-45ad-9c37-afe07f6b48e1") && c.LoanBatchId == _loanBatch.Id && c.IsDeleted == false);
        _loanBatch.TotalAccepted = applications.Count(c => c.Status == new Guid("3118a07e-013a-4b3a-a2c1-74c921feeba1") && c.LoanBatchId == _loanBatch.Id && c.IsDeleted == false );
        _loanBatch.TotalRejected = applications.Count(c => c.Status == new Guid("0dddbbcb-ac18-421a-942d-05ca579abb0c") && c.LoanBatchId == _loanBatch.Id && c.IsDeleted == false);
        _loanBatch.TotalClosed = applications.Count(c => c.Status == new Guid("f49faffa-b113-4546-ac7f-485164e5a822") && c.LoanBatchId == _loanBatch.Id && c.IsDeleted == false);
        _loanBatch.TotalDisbursed = applications.Count(c => c.Status == new Guid("e24d24a8-fc69-4527-a92a-97f6648a43c5") && c.LoanBatchId == _loanBatch.Id && c.IsDeleted == false);


        if (_loanBatch != null)
        {
            _loanBatch.Project = _mapper.Map<ProjectResponseModel>(await _projectRepository.GetFirstAsync(c => c.Id == loanBatch.ProjectId));
        }

        return _loanBatch;
    }

    public async Task<LoanBatchesResponseModel> GetAllAsync(LoanBatchSearchParams loanBatchSearchParams)
    {
        var _projects = await _projectRepository.GetAllAsync(c => c.CountryId == loanBatchSearchParams.CountryId);
        var projectIds = _projects.Select(p => p.Id).ToHashSet();
        var applications = await _loanApplicationRepository.GetAllAsync(c => c.IsDeleted == false);

        var _loanBatches = await _loanBatchRepository.GetAllAsync(c =>
            (string.IsNullOrEmpty(loanBatchSearchParams.Filter) || c.Name.Contains(loanBatchSearchParams.Filter)) &&
            (loanBatchSearchParams.ProjectId == null || c.ProjectId == loanBatchSearchParams.ProjectId) &&
            projectIds.Contains(c.ProjectId) && c.IsDeleted == false
        );

        int numberOfObjectsPerPage = loanBatchSearchParams.PageSize;

        var queryResultPage = _loanBatches
            .Skip(numberOfObjectsPerPage * (loanBatchSearchParams.PageNumber ))
            .Take(numberOfObjectsPerPage);

        queryResultPage.ToList().ForEach(f => f.Project = _projects.FirstOrDefault(c => c.Id == f.ProjectId));

        var list = _mapper.Map<IEnumerable<LoanBatchResponseModel>>(queryResultPage);

            int total = _loanBatches.Count();

        foreach (var item in list)
        {
            
            item.TotalApplications = applications.Count(c => c.LoanBatchId == item.Id && c.Status != new Guid("6f103a88-8443-45ad-9c37-afe07f6b48e1"));
            item.TotalBatchAmount = (float)applications
            .Where(c => c.LoanBatchId == item.Id && c.Status != new Guid("e24d24a8-fc69-4527-a92a-97f6648a43c5"))
            .Sum(c => c.PrincipalAmount);
            
            item.TotalDraft = applications.Count(c => c.Status == new Guid("6f103a88-8443-45ad-9c37-afe07f6b48e1") || c.Status == new Guid() && c.LoanBatchId == item.Id);
            item.TotalAccepted = applications.Count(c => c.Status == new Guid("3118a07e-013a-4b3a-a2c1-74c921feeba1") && c.LoanBatchId == item.Id);
            item.TotalRejected = applications.Count(c => c.Status == new Guid("0dddbbcb-ac18-421a-942d-05ca579abb0c") && c.LoanBatchId == item.Id);
            item.TotalClosed = applications.Count(c => c.Status == new Guid("f49faffa-b113-4546-ac7f-485164e5a822") && c.LoanBatchId == item.Id);
            item.TotalDisbursed = applications.Count(c => c.Status == new Guid("e24d24a8-fc69-4527-a92a-97f6648a43c5") && c.LoanBatchId == item.Id);

        }
        var counts = new LoanBatchCountsResponseModel
        {
            TotalBatches = total,

            TotalApplications = applications.Count(c => _loanBatches.Select(b => b.Id).Contains(c.LoanBatchId) && 
            c.Status != Guid.Parse("6f103a88-8443-45ad-9c37-afe07f6b48e1")),

            TotalDraft = applications.Count(c => c.Status == new Guid("6f103a88-8443-45ad-9c37-afe07f6b48e1") || c.Status == new Guid()
            && _loanBatches.Select(b => b.Id).Contains(c.LoanBatchId)),

            TotalAccepted = applications.Count(c => c.Status == new Guid("3118a07e-013a-4b3a-a2c1-74c921feeba1") &&
             _loanBatches.Select(b => b.Id).Contains(c.LoanBatchId)),

            TotalRejected = applications.Count(c => c.Status == new Guid("0dddbbcb-ac18-421a-942d-05ca579abb0c") && 
            _loanBatches.Select(b => b.Id).Contains(c.LoanBatchId)),

            TotalClosed = applications.Count(c => c.Status == new Guid("f49faffa-b113-4546-ac7f-485164e5a822") &&
            _loanBatches.Select(b => b.Id).Contains(c.LoanBatchId)),

            TotalDisbursed = applications.Count(c => c.Status == new Guid("e24d24a8-fc69-4527-a92a-97f6648a43c5") &&
             _loanBatches.Select(b => b.Id).Contains(c.LoanBatchId)),

        };

        return new LoanBatchesResponseModel { LoanBatches= list.OrderByDescending(x => x.CreatedOn),
        LoanCounts = counts
        };

    }

    public async Task<IEnumerable<LoanBatchResponseModel>> GetAllByListIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var loanBatch = await _loanBatchRepository.GetAllAsync(ti => ti.Id == id);

        return _mapper.Map<IEnumerable<LoanBatchResponseModel>>(loanBatch);
    }

    public object GetByProjectIds(List<string> projectIds, CancellationToken cancellationToken = default)
    {
        var loanBatch = _loanBatchRepository.GetByProjectIds(projectIds);

        return loanBatch;
    }

    public async Task<UpdateLoanBatchResponseModel> UpdateAsync(Guid id, UpdateLoanBatchModel updateLoanBatchModel)
    {
        try
        {
            var validationResult = _updateLoanBatchValidator.Validate(updateLoanBatchModel);
            if (!validationResult.IsValid)
            {
                throw new ValidationException("Validation failed", validationResult.Errors);
            }

            var loanBatch = await _loanBatchRepository.GetFirstAsync(ti => ti.Id == id);
            if (loanBatch == null)
            {
                throw new KeyNotFoundException("Loan batch not found");
            }
            _mapper.Map(updateLoanBatchModel, loanBatch);


            var existingFeeList = await _loanBatchProcessingFeeRepository.GetAllAsync(c => c.LoanBatchId == id);
            var ids = existingFeeList.Select(i => i.Id);

            foreach (var itemId in ids)
            {
                var entity = await _loanBatchProcessingFeeRepository.GetFirstAsync(c => c.Id == itemId);

                var delete_response = await _loanBatchProcessingFeeRepository.DeleteAsync(entity);
            }


            //if (id != Guid.Empty)
            //{
            //    var updatedFees = updateLoanBatchModel.ProcessingFees.Select(item => new LoanBatchProcessingFee
            //    {
            //        Value = item.Value,
            //        FeeName = item.FeeName,
            //        FeeType = item.FeeType,
            //        LoanBatchId = id
            //    }).ToList();

            //    await _loanBatchRepository.AddRangeLoanBatchProcessingFeeAsync(updatedFees);
            //}

            loanBatch.UpdatedBy = Guid.Parse(_claimService.GetUserId());

            loanBatch.UpdatedOn = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToUniversalTime();

            loanBatch.EffectiveDate = new DateTime(loanBatch.EffectiveDate.Year, loanBatch.EffectiveDate.Month, 1).ToUniversalTime();
            loanBatch.InitiationDate = new DateTime(loanBatch.InitiationDate.Year, loanBatch.InitiationDate.Month, 1).ToUniversalTime();

            var updatedLoanBatch = await _loanBatchRepository.UpdateAsync(loanBatch);
            return new UpdateLoanBatchResponseModel { Id = updatedLoanBatch.Id };
        }
        catch (ValidationException)
        {
            throw;  // Rethrow without losing stack trace
        }
        catch (KeyNotFoundException)
        {
            throw;  // Handle missing loan batch gracefully
        }
        catch (Exception ex)
        {
            // Log the exception here if logging is available
            throw new Exception("An error occurred while updating the loan batch", ex);
        }
    }

    public async Task<List<LoanBatchResponseModel>> GetValidLoanBatches(Guid countryId)
    {
        // Step 1: Get all loan batches that are not deleted and not status 4
        var loanBatches = await _loanBatchRepository.GetAllAsync(c => c.CountryId == countryId && !c.IsDeleted && c.StatusId != 4);

        // Step 2: Get all loan batch items that are not deleted
        var loanBatchItems = await _loanBatchItemRepository.GetAllAsync(ti => !ti.IsDeleted);

        // Step 3: Filter loan batches to only those with at least one corresponding item
        var validLoanBatches = loanBatches
            .Where(lb => loanBatchItems.Any(ti => ti.LoanBatchId == lb.Id))
            .ToList();

        return _mapper.Map<List<LoanBatchResponseModel>>(validLoanBatches);
    }


    #endregion

    #region Loan Batch Item
    public async Task<CreateLoanBatchItemResponseModel> CreateLoanBatchItem(CreateLoanBatchItemModel createLoanBatchItemModel)
    {
        try
        {
            var mapping = _mapper.Map<LoanBatchItem>(createLoanBatchItemModel);

            var addedMap = await _loanBatchItemRepository.AddAsync(mapping);

            return new CreateLoanBatchItemResponseModel
            {
                Id = mapping.Id
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public async Task<IEnumerable<SelectItemModel>> GetItemUnitsAsync()
    {
        try
        {
            using (var connection = GetConnection())
            {
                string sql = string.Format("select \"Id\" as Value, \"Name\" as Label, \"Abbreviation\" as Attrib1  from public.\"ItemUnit\"");
                return await connection.QueryAsync<SelectItemModel>(sql);
            }
        }
        catch (Exception)
        {
            return new SelectItemModel[] { };
        }

    }

    public async Task<IEnumerable<LoanBatchItemResponseModel>> GetBatchItems(Guid loanBatchId, CancellationToken cancellationToken = default)
    {
        var loanBatch = await _loanBatchItemRepository.GetAllAsync(ti => ti.LoanBatchId == loanBatchId && ti.IsDeleted == false);
        var loanItems = await _loanItemRepository.GetAllAsync(c => 1 == 1);
        var loanItemSelect = loanItems.Select(li => new SelectItemModel { Value = Convert.ToString(li.Id), Label = li.ItemName });
        var list = _mapper.Map<IEnumerable<LoanBatchItemResponseModel>>(loanBatch);
        var units = await GetItemUnitsAsync();

        list.ToList().ForEach(item => item.LoanItem = loanItemSelect.Where(c => c.Value == Convert.ToString(item.LoanItemId)).FirstOrDefault());
        list.ToList().ForEach(item => item.Unit = units.Where(c => c.Value == Convert.ToString(item.UnitId)).FirstOrDefault());

        return list;
    }

    public async Task<BaseResponseModel> DeleteBatchItemAsync(Guid id)
    {
        var loanBatchItem = await _loanBatchItemRepository.GetFirstAsync(tl => tl.Id == id);
        if (loanBatchItem != null)
        {
            loanBatchItem.IsDeleted = true;
        }

        return new BaseResponseModel
        {
            Id = (await _loanBatchItemRepository.UpdateAsync(loanBatchItem)).Id
        };
    }

    public async Task<UpdateLoanBatchItemResponseModel> UpdateLoanBatchItemAsync(Guid id, UpdateLoanBatchItemModel updateLoanBatchItemModel)
    {
        var loanBatch = await _loanBatchItemRepository.GetFirstAsync(ti => ti.Id == id);

        _mapper.Map(updateLoanBatchItemModel, loanBatch);

        return new UpdateLoanBatchItemResponseModel
        {
            Id = (await _loanBatchItemRepository.UpdateAsync(loanBatch)).Id
        };
    }
    #endregion

    public async Task<UpdateLoanBatchResponseModel> UpdateStage(Guid id, Guid excelImportId, UpdateStageModel model)
    {
        // get payment stages
        //var stages = await _paymentBatchRepository.GetPaymentApprovalStages();

        string stageName = model.Action switch
        {
            "review_rejected" => "Review Rejected",
            "review_next" => "Pending Approval",
            "approve" => "Approved",
            _ => "Under Review" // Default case
        };

        //var stageId = stages.Any() ? stages.Where(c => c.StageText == stageName).FirstOrDefault().Id : Guid.Parse("fc4497a2-d9ee-49e1-a9df-99d48731d321");

        // add to history
        //await _loanBatchItemRepository.AddPaymentHistory(new LoanBatchHistory
        //{
        //    Action = stageName,
        //    Comments = model.Remarks,
        //    PaymentBatchId = id,
        //    StageId = stageId,
        //    CreatedBy = Guid.Parse(_claimService.GetUserId()),
        //    CreatedOn = DateTime.UtcNow,
        //});

        // update status id in payment batch
        var batch = await _loanBatchRepository.GetFirstAsync(ti => ti.Id == id);
        batch.StageText = stageName;
        batch.UpdatedOn = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);

        batch.UpdatedBy = Guid.Parse(_claimService.GetUserId());

        var updatedbatch = await _loanBatchRepository.UpdateAsync(batch);

        if (stageName == "Approved")
        {
            await _loanApplicationRepository.TransferFromStaging(excelImportId);
        }

        //Get initiator name
        //var users = await _userService.GetAllAsync("");
        //var initiator = users.FirstOrDefault(user => user.Id == new Guid(_claimService.GetUserId()));
        //if (initiator == null)
        //{
        //    throw new Exception("User not found."); // Handle the case where the user is not found
        //}

        //await SaveActivityLog(new CreateActivityLogModel
        //{
        //    Title = $"{batch.BatchName} - status updated",
        //    Description = $"{batch.Status} by {initiator.Username}",
        //    Link = $"/payment-batch/details/{batch.Id}"
        //});

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

        return new UpdateLoanBatchResponseModel
        {
            Id = id
        };
    }

    public async Task<IEnumerable<AttachmentResponseModel>> GetBatchDocuments(Guid id, CancellationToken cancellationToken = default)
    {
        if (id != Guid.Empty) // Fix: Replace null check with Guid.Empty check
        {
            var attachmentMap = await _batchAttachmentMappingRepository.GetAllAsync(c => c.LoanBatchId == id);
            var attachmentFiles = await _attachmentUploadRepository.GetAllAsync(c => true);

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

            return data.ToList();
        }

        return Enumerable.Empty<AttachmentResponseModel>();
    }

    protected NpgsqlConnection GetConnection()
    {
        return new NpgsqlConnection(_connectionString);
    }

    protected class DatabaseConfiguration
    {
        public string DefaultConnection { get; set; }
    }
}


