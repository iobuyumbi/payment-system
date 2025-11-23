using AutoMapper;
using LoanManagementSystem;
using Solidaridad.Application.Common.PortalSettings;
using Solidaridad.Application.Models;
using Solidaridad.Application.Models.ActivityLog;
using Solidaridad.Application.Models.ExcelExport;
using Solidaridad.Application.Models.LoanApplication;
using Solidaridad.Application.Models.PaymentBatch;
using Solidaridad.Application.Models.PaymentBatch.Transitions;
using Solidaridad.Application.Models.Project;
using Solidaridad.Core.Entities;
using Solidaridad.Core.Entities.Base;
using Solidaridad.Core.Entities.Loans;
using Solidaridad.Core.Entities.Payments;
using Solidaridad.Core.Enums;
using Solidaridad.DataAccess.Migrations;
using Solidaridad.DataAccess.Repositories;
using Solidaridad.DataAccess.Repositories.Impl;
using Solidaridad.Shared.Services;
using System.Linq;
using System.Security.Cryptography;
using System.Text.Json;

namespace Solidaridad.Application.Services.Impl;

public class PaymentBatchService : BaseService, IPaymentBatchService
{
    #region DI
    private readonly IMapper _mapper;
    private readonly IPaymentBatchRepository _paymentBatchRepository;
    private readonly IPaymentRequestDeductibleRepository _paymentRequestDeductibleRepository;
    private readonly IFacilitationRepository _facilitationRepository;
    private readonly IPaymenBatchLoanBatchMappingRepository _paymentBatchLoanBatchMappingRepository;
    private readonly IPaymentBatchProjectMappingRepository _paymentBatchProjectMappingRepository;
    private readonly ICountryRepository _countryRepository;
    private readonly ILoanBatchRepository _loanBatchRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly IClaimService _claimService;
    private readonly IUserService _userService;
    private readonly IEmailTemplateService _emailTemplateService;
    private readonly IEmailService _emailService;
    private readonly IEmailTemplateRepository _emailTemplateRepository;
    private readonly IEmailTemplateVariableRepository _emailTemplateVariableRepository;
    private readonly PortalSettings _portalSettings;
    private readonly IPaymentDeductibleService _paymentDeductibleService;
    private readonly ILoanRepaymentRepository _loanRepaymentRepository;
    private readonly ILoanApplicationService _loanApplicationService;
    private readonly ILoanRepaymentService _loanRepaymentService;
    private readonly IFarmerRepository _farmerRepository;



    public PaymentBatchService(IMapper mapper, IPaymentBatchRepository paymentBatchRepository,
        IPaymentBatchProjectMappingRepository paymentBatchProjectMappingRepository,
        ILoanRepaymentRepository loanRepaymentRepository,
        ILoanApplicationService loanApplicationService,
        ILoanRepaymentService loanRepaymentService,

        IPaymenBatchLoanBatchMappingRepository paymentBatchLoanBatchMappingRepository,
        ILoanBatchRepository loanBatchRepository, IEmailTemplateRepository emailTemplateRepository,
        IProjectRepository projectRepository,
        ICountryRepository countryRepository,
        IFacilitationRepository facilitationRepository,
        IPaymentRequestDeductibleRepository paymentRequestDeductibleRepository,
        IUserService userService,
        IClaimService claimService,
        IEmailTemplateService emailTemplateService,
        IEmailTemplateVariableRepository emailTemplateVariableRepository, PortalSettings portalSettings,
        IActivityLogService activityLogService,
        IPaymentDeductibleService paymentDeductibleService,
        IFarmerRepository farmerRepository,
        IEmailService emailService) : base(activityLogService)
    {
        _mapper = mapper;
        _paymentBatchRepository = paymentBatchRepository;
        _paymentBatchLoanBatchMappingRepository = paymentBatchLoanBatchMappingRepository;
        _countryRepository = countryRepository;
        _loanBatchRepository = loanBatchRepository;
        _userService = userService;
        _claimService = claimService;
        _projectRepository = projectRepository;
        _emailService = emailService;
        _emailTemplateService = emailTemplateService;
        _emailTemplateRepository = emailTemplateRepository;
        _emailTemplateVariableRepository = emailTemplateVariableRepository;
        _facilitationRepository = facilitationRepository;
        _paymentRequestDeductibleRepository = paymentRequestDeductibleRepository;
        _portalSettings = portalSettings;
        _paymentDeductibleService = paymentDeductibleService;
        _loanRepaymentRepository = loanRepaymentRepository;
        _loanApplicationService = loanApplicationService;
        _loanRepaymentService = loanRepaymentService;
        _farmerRepository = farmerRepository;
        _paymentBatchProjectMappingRepository = paymentBatchProjectMappingRepository;
    }
    #endregion

    #region Methods
    public async Task<CreatePaymentBatchResponseModel> CreateAsync(CreatePaymentBatchModel model)
    {
        try
        {
            // get payment stages
            var stages = await _paymentBatchRepository.GetPaymentApprovalStages();

            var batch = _mapper.Map<PaymentBatch>(model);
            batch.ProjectId = Guid.Parse(model.ProjectIds[0].Value);
            batch.StatusId = stages.Any() ? stages.Where(c => c.StageText == "Initiated").FirstOrDefault().Id : Guid.Parse("fc4497a2-d9ee-49e1-a9df-99d48731d321");
            batch.CreatedBy = Guid.Parse(_claimService.GetUserId());
            batch.CreatedOn = DateTime.UtcNow;
            batch.UpdatedBy = Guid.Parse(_claimService.GetUserId());
            batch.UpdatedOn = DateTime.UtcNow;

            var addedBatch = await _paymentBatchRepository.AddAsync(batch);

            // add loan batch - payment batch mapping
            if (addedBatch != null)
            {
                var mapping = new List<PaymenBatchLoanBatchMapping>();
                if (model.LoanBatchIds != null)
                {
                    foreach (var item in model.LoanBatchIds)
                    {
                        mapping.Add(new PaymenBatchLoanBatchMapping
                        {
                            PaymentBatchId = addedBatch.Id,
                            LoanBatchId = Guid.Parse(item.Value)
                        });
                    }
                    await _paymentBatchLoanBatchMappingRepository.AddRange(mapping);
                }
                var projectmapping = new List<PaymentBatchProjectMapping>();
                if (model.ProjectIds != null)
                {
                    foreach (var item in model.ProjectIds)
                    {
                        projectmapping.Add(new PaymentBatchProjectMapping
                        {
                            PaymentBatchId = addedBatch.Id,
                            ProjectId = Guid.Parse(item.Value)
                        });
                    }
                    await _paymentBatchProjectMappingRepository.AddRange(projectmapping);
                }
                await SaveActivityLog(new CreateActivityLogModel
                {
                    Title = $"{addedBatch.BatchName} - A new payment batch added",
                    Description = $"{addedBatch.Status}",
                    Link = $"/payment-batch/details/{addedBatch.Id}"
                });
            }

            // add to history
            await _paymentBatchRepository.AddPaymentHistory(new PaymentBatchHistory
            {
                Action = "Created",
                Comments = "Payment batch created",
                PaymentBatchId = addedBatch.Id,
                StageId = stages.Any() ? stages.Where(c => c.StageText == "Initiated").FirstOrDefault().Id : Guid.Parse("fc4497a2-d9ee-49e1-a9df-99d48731d321"),
                CreatedBy = Guid.Parse(_claimService.GetUserId()),
                CreatedOn = DateTime.UtcNow,
            });

            return new CreatePaymentBatchResponseModel
            {
                Id = addedBatch.Id,

            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public async Task<BaseResponseModel> DeleteAsync(Guid id)
    {
        var batch = await _paymentBatchRepository.GetFirstAsync(tl => tl.Id == id);
        batch.IsDeleted = true;
        return new BaseResponseModel
        {
            Id = (await _paymentBatchRepository.UpdateAsync(batch)).Id
        };
    }

    public async Task<PaymentBatchesResponseModel> GetAllAsync(PaymentSearchParams searchParams)
    {
        var _farmers = await _paymentBatchRepository.GetFullAsync(c =>
           (string.IsNullOrEmpty(searchParams.Filter) ||
            c.BatchName.Contains(searchParams.Filter)
          ) && searchParams.CountryId == c.CountryId
            && c.IsDeleted == false
            && c.IsExcelUploaded == true);

        int numberOfObjectsPerPage = searchParams.PageSize;
        var _countries = await _countryRepository.GetAllAsync(c => c.IsActive == true);

        var queryResultPage = _farmers
            .Skip(numberOfObjectsPerPage * (searchParams.PageNumber))
            .Take(numberOfObjectsPerPage);

        queryResultPage.ToList().ForEach(f => f.Country = _countries.FirstOrDefault(c => c.Id == f.CountryId));

        var list = _mapper.Map<IEnumerable<PaymentBatchResponseModel>>(queryResultPage).ToList();

        // 1. get payment and loan batch mapping
        var selectedPaymentBatchIds = list.Select(f => f.Id);

        var paymentBatchLoanBatchMapping = await _paymentBatchLoanBatchMappingRepository.GetAllAsync(c => selectedPaymentBatchIds.Contains(c.PaymentBatchId));
        var selectedMappingIds = paymentBatchLoanBatchMapping.Select(f => new { f.LoanBatchId, f.PaymentBatchId });

        // 2. get loan batches
        var _loanBatches = await _loanBatchRepository.GetAllAsync(c => c.IsDeleted == false);

        // 3. get projects
        var _projects = await _projectRepository.GetAllAsync(c => c.IsDeleted == false);
        // 4. bind loan batches
        //var _result = from pb in list
        //              join mapping in paymentBatchLoanBatchMapping on pb.Id equals mapping.PaymentBatchId
        //              join lb in _loanBatches on mapping.LoanBatchId equals lb.Id
        //              group lb by new { pb.Id, pb.BatchName, pb.Country, pb.Status, pb.DateCreated } into grouped
        //              select new PaymentBatchResponseModel
        //              {
        //                  Id = grouped.Key.Id,
        //                  BatchName = grouped.Key.BatchName,
        //                  Country = grouped.Key.Country,
        //                  Status = grouped.Key.Status,
        //                  DateCreated = grouped.Key.DateCreated,
        //                  LoanBatches = grouped.Select(lb => new LoanBatchResponseModel
        //                  {
        //                      Id = lb.Id,
        //                      Name = lb.Name,
        //                      Project = _mapper.Map<ProjectResponseModel>(_projects.FirstOrDefault(p => p.Id == lb.ProjectId))
        //                  }).ToList()
        //              };

        var queryResult = from pb in list
                          join mapping in paymentBatchLoanBatchMapping on pb.Id equals mapping.PaymentBatchId into pbMappings
                          from mapping in pbMappings.DefaultIfEmpty() // LEFT JOIN
                          join lb in _loanBatches on mapping != null ? mapping.LoanBatchId : (Guid?)null equals lb.Id into lbGroup
                          from lb in lbGroup.DefaultIfEmpty() // LEFT JOIN with LoanBatch
                          group lb by new { pb.Id, pb.BatchName, pb.Country, pb.Status, pb.DateCreated, pb.CreatedOn, pb.CreatedBy, pb.PaymentModule, pb.UpdatedOn } into grouped
                          select new PaymentBatchResponseModel
                          {
                              Id = grouped.Key.Id,
                              BatchName = grouped.Key.BatchName,
                              Country = grouped.Key.Country,
                              Status = grouped.Key.Status,
                              DateCreated = grouped.Key.DateCreated,
                              PaymentModule = grouped.Key.PaymentModule,
                              UpdatedOn = grouped.Key.UpdatedOn,
                              CreatedOn = grouped.Key.CreatedOn,
                              CreatedBy = grouped.Key.CreatedBy,
                              LoanBatches = grouped
                                  .Where(lb => lb != null) // Only map if lb is not null
                                  .Select(lb => new LoanBatchResponseModel
                                  {
                                      Id = lb.Id,
                                      Name = lb.Name,
                                      Project = _mapper.Map<ProjectResponseModel>(_projects.FirstOrDefault(p => p.Id == lb.ProjectId))
                                  })
                                  .ToList()
                          };

        var finalResult = new List<PaymentBatchResponseModel>();
        foreach (var item in queryResult)
        {
            var paymentStats = new Models.PaymentDeductible.PaymentStats();
            if (item.PaymentModule == 3) // deductible payment
            {
                paymentStats = await _paymentDeductibleService.GetPaymentBatchStats((Guid)item.Id);
            }
            else
            {
                paymentStats = await _paymentDeductibleService.GetFacilitationPaymentBatchStats((Guid)item.Id);
            }

            var loanBatchModels = item.LoanBatches
                .Select(lb => new LoanBatchResponseModel
                {
                    Id = lb.Id,
                    Name = lb.Name,
                    Project = _mapper.Map<ProjectResponseModel>(_projects.FirstOrDefault(p => p.Id == lb.ProjectId))
                })
                .ToList();

            finalResult.Add(new PaymentBatchResponseModel
            {
                Id = item.Id,
                BatchName = item.BatchName,
                Country = item.Country,
                Status = item.Status,
                DateCreated = item.DateCreated,
                PaymentModule = item.PaymentModule,
                UpdatedOn = item.UpdatedOn,
                CreatedOn = item.CreatedOn,
                CreatedBy = item.CreatedBy,
                PaymentStats = paymentStats,
                LoanBatches = loanBatchModels
            });
        }
        var stats = new PaymentBatchStatsModel {
            TotalBatches = _farmers.Count()
        };

        return new PaymentBatchesResponseModel
        {
            PaymentBatchResponseModel = finalResult.OrderByDescending(c => c.DateCreated),
            PaymentBatchStats = stats
        };
        }

    public async Task<PaymentBatchResponseModel> GetSingle(Guid id, Guid? countryId)
    {
        var paymentBatch = new PaymentBatch();
        // 1. Get the payment batch
        if (countryId != null)
        {
            paymentBatch = await _paymentBatchRepository.GetFirstAsync(ti => ti.Id == id
        && ti.CountryId == countryId
        && ti.IsDeleted == false);
        }
        else
        {
            paymentBatch = await _paymentBatchRepository.GetFirstAsync(ti => ti.Id == id

       && ti.IsDeleted == false);
        }

        if (paymentBatch == null)
            return null;

        // 2. Get country details
        var country = await _countryRepository.GetFirstAsync(c => c.Id == paymentBatch.CountryId && c.IsActive == true);
        paymentBatch.Country = country;

        // 3. Get payment and loan batch mappings for this payment batch
        var paymentBatchLoanBatchMappings = await _paymentBatchLoanBatchMappingRepository.GetAllAsync(c => c.PaymentBatchId == id);
        var loanBatchIds = paymentBatchLoanBatchMappings.Select(mapping => mapping.LoanBatchId);

        // 4. Get loan batches
        var loanBatches = await _loanBatchRepository.GetAllAsync(lb => loanBatchIds.Contains(lb.Id));

        // 5. Get all projects
        var projects = await _projectRepository.GetAllAsync(p => 1 == 1);

        // 6. Map data to response model
        var responseModel = _mapper.Map<PaymentBatchResponseModel>(paymentBatch);
        // 8. Get status
        var stages = await _paymentBatchRepository.GetPaymentApprovalStages();
        responseModel.Status = stages.Where(c => c.Id == responseModel.StatusId).FirstOrDefault();

        if (responseModel.PaymentModule == 4)
        {
            responseModel.Project = _mapper.Map<ProjectResponseModel>(projects.FirstOrDefault(p => p.Id == responseModel.ProjectId));
            var rowsImported = await _facilitationRepository.GetAllAsync(c => c.PaymentBatchId == id);
            responseModel.SuccessRowCount = rowsImported.Count(c => c.StatusId > 0);
            responseModel.FailedRowCount = rowsImported.Count(c => c.StatusId == 0);
        }
        else
        {
            var rowsImported = await _paymentRequestDeductibleRepository.GetAllAsync(c => c.PaymentBatchId == id);
            responseModel.SuccessRowCount = rowsImported.Count(c => c.StatusId > 0);
            responseModel.FailedRowCount = rowsImported.Count(c => c.StatusId == 0);
        }

        responseModel.LoanBatches = loanBatches.Select(lb => new LoanBatchResponseModel
        {
            Id = lb.Id,
            Name = lb.Name,
            Project = _mapper.Map<ProjectResponseModel>(projects.FirstOrDefault(p => p.Id == lb.ProjectId))

        }).ToList();

        return responseModel;
    }

    public async Task<UpdatePaymentBatchResponseModel> UpdateAsync(Guid id, UpdatePaymentBatchModel model)
    {
        try
        {
            var _batch = await _paymentBatchRepository.GetFirstAsync(ti => ti.Id == id);
            Guid statusId = _batch.StatusId;
            var batch = _mapper.Map(model, _batch);

            batch.StatusId = statusId;
            var updatedbatch = await _paymentBatchRepository.UpdateAsync(batch);
            return new UpdatePaymentBatchResponseModel
            {
                Id = id
            };
        }
        catch (Exception ex)
        {
            return new UpdatePaymentBatchResponseModel
            {
                Id = id
            };
        }
    }

    public async Task<object> GetPaymentBatchHistory(Guid id)
    {
        var history = await _paymentBatchRepository.GetPaymentHistory(id);
        var users = await _userService.GetAllAsync("");

        var result = from hist in history
                     join usr in users on hist.CreatedBy equals usr.Id
                     select new
                     {
                         Id = hist.Id,
                         CreatedBy = usr.Username,
                         CreatedOn = hist.CreatedOn,
                         Action = hist.Action,
                         Comments = hist.Comments
                     };

        return result.ToList();
    }

    public async Task<UpdatePaymentBatchResponseModel> UpdateStage(Guid id, UpdateStageModel model)
    {
        // get payment stages
        var stages = await _paymentBatchRepository.GetPaymentApprovalStages();

        string stageName = model.Action switch
        {
            "review_rejected" => "Review Rejected",
            "review_next" => "Pending Approval",
            "approved" => "Approved",
            "rejected" => "Rejected",
            _ => "Under Review" // Default case
        };

        var stageId = stages.Any() ? stages.Where(c => c.StageText == stageName).FirstOrDefault().Id : Guid.Parse("fc4497a2-d9ee-49e1-a9df-99d48731d321");

        // add to history
        await _paymentBatchRepository.AddPaymentHistory(new PaymentBatchHistory
        {
            Action = stageName,
            Comments = model.Remarks,
            PaymentBatchId = id,
            StageId = stageId,
            CreatedBy = Guid.Parse(_claimService.GetUserId()),
            CreatedOn = DateTime.UtcNow,
        });

        // update status id in payment batch
        var batch = await _paymentBatchRepository.GetFirstAsync(ti => ti.Id == id);
        batch.StatusId = stageId;
        batch.UpdatedOn = DateTime.UtcNow;
        batch.UpdatedBy = Guid.Parse(_claimService.GetUserId());

        var nextStatus = MapActionToNextStatus(model.Action, batch.BatchStatus);
        if (nextStatus == null)
            throw new Exception("Invalid transition");

        // Maker-Checker Check
        bool isMakerCheckerViolation =
            (model.Action == "approve" || model.Action == "reject" || model.Action == "start-review")
            && model.PerformedBy == batch.CreatedBy;

        if (isMakerCheckerViolation)
            throw new Exception("You cannot review or approve your own batch (maker-checker rule).");

        batch.BatchStatus = nextStatus.Value; // enum gets stored as int in DB

        var updatedbatch = await _paymentBatchRepository.UpdateAsync(batch);

        //Get initiator name
        var users = await _userService.GetAllAsync("");
        var initiator = users.FirstOrDefault(user => user.Id == new Guid(_claimService.GetUserId()));
        if (initiator == null)
        {
            throw new Exception("User not found."); // Handle the case where the user is not found
        }

        await SaveActivityLog(new CreateActivityLogModel
        {
            Title = $"{batch.BatchName} - status updated",
            Description = $"{batch.Status} by {initiator.Username}",
            Link = $"/payment-batch/details/{batch.Id}"
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

        return new UpdatePaymentBatchResponseModel
        {
            Id = id
        };
    }

    public async Task<bool> SendEmail(Guid batchId)
    {
        try
        {
            //Get initiator name
            var users = await _userService.GetAllAsync("");
            var initiator = users.FirstOrDefault(user => user.Id == new Guid(_claimService.GetUserId()));
            if (initiator == null)
            {
                throw new Exception("User not found."); // Handle the case where the user is not found
            }

            //construct email using template
            var emailTemplates = await _emailTemplateRepository.GetAllAsync(tl => tl.Id == new Guid("35b845b6-37be-4705-8e14-b2d009f73b16"));
            var emailTemplate = emailTemplates.FirstOrDefault();
            emailTemplate.Variables = await _emailTemplateVariableRepository.GetAllAsync(tl => tl.EmailTemplateId == emailTemplate.Id);
            var emailBody = _emailTemplateService.RenderTemplate(
                             emailTemplate,
                             new Dictionary<string, string>
                            {
                             { "Initiator_Name", initiator.Username },
                             { "Link", $"{_portalSettings.PortalUrl}/payment-batch/details/{batchId}" }

                            });

            //Send email
            var recipients = new[] { "karanvir0153@icloud.com", "munish@enet.co.ke" };

            foreach (var recipient in recipients)
            {
                await _emailService.SendEmailAsync(
                    Common.Email.EmailMessage.Create(
                        recipient, // One recipient at a time
                        emailBody,
                        "Payment status change"
                    )
                );
            }
            return true;
        }
        catch (Exception ex) { return false; }
    }

    public async Task<PaymentStatsResponseModel> GetStats(Guid? CountryId)
    {

        var paymentBatches = await _paymentBatchRepository.GetAllAsync(ti => ti.CountryId == CountryId);


        var stages = await _paymentBatchRepository.GetPaymentApprovalStages();


        int totalPaymentBatches = paymentBatches.Count;


        var stageCounts = stages.Select(stage => new
        {
            StageName = stage.StageText,
            Count = paymentBatches.Count(pb => pb.StatusId == stage.Id)
        }).ToList();


        var response = new PaymentStatsResponseModel
        {
            TotalPaymentBatches = totalPaymentBatches,
            StageCounts = stageCounts.ToDictionary(x => x.StageName, x => x.Count)
        };

        return response;
    }

    public async Task<UpdatePaymentBatchResponseModel> ProcessManually(Guid id)
    {
        // get payment stages
        var stages = await _paymentBatchRepository.GetPaymentApprovalStages();
        string stageName = "Approved";
        var stageId = stages.Where(c => c.StageText == stageName).FirstOrDefault().Id;

        // add to history
        await _paymentBatchRepository.AddPaymentHistory(new PaymentBatchHistory
        {
            Action = stageName,
            Comments = "Processed manually",
            PaymentBatchId = id,
            StageId = stageId,
            CreatedBy = Guid.Parse(_claimService.GetUserId()),
            CreatedOn = DateTime.UtcNow,
        });

        // update status id in payment batch
        var batch = await _paymentBatchRepository.GetFirstAsync(ti => ti.Id == id);

        var nextStatus = MapActionToNextStatus("approved", batch.BatchStatus);
        if (nextStatus == null)
            throw new Exception("Invalid transition");

        batch.BatchStatus = nextStatus.Value; // enum gets stored as int in DB

        batch.StatusId = stageId;
        batch.UpdatedOn = DateTime.UtcNow;
        batch.UpdatedBy = Guid.Parse(_claimService.GetUserId());

        var updatedbatch = await _paymentBatchRepository.UpdateAsync(batch);

        // update payment status
        if (batch.PaymentModule == 3)
        {
            var deductibes = await _paymentRequestDeductibleRepository.GetAllAsync(ti => ti.PaymentBatchId == id
                                    && ti.IsDeleted == false);

            deductibes.ForEach(c =>
             {
                 c.IsPaymentComplete = true;
                 c.Remarks = "Processed manually";
                 c.IsManual = true;
                 c.UpdatedBy = Guid.Parse(_claimService.GetUserId());
                 c.UpdatedOn = DateTime.UtcNow;
                 c.PaymentStatus = Guid.Parse("271d9c1a-2c4f-4ee2-ad0f-d7dc36bd255f"); // "Completed";
             });

            await _paymentRequestDeductibleRepository.UpdateRange(deductibes);


            var paymenBatchLoanBatchMapping = await _paymentBatchLoanBatchMappingRepository.GetAllAsync(x => x.PaymentBatchId == id);
            var paymentBatch = new PaymentBatch();
            if (paymenBatchLoanBatchMapping != null)
            {
                var loanBatch = await _loanBatchRepository.GetAllAsync(x => x.Id == paymenBatchLoanBatchMapping.FirstOrDefault().LoanBatchId);
                paymentBatch = await _paymentBatchRepository.GetFirstAsync(ti => ti.Id == id && ti.IsDeleted == false);

                var loanApplications = await _loanApplicationService.GetEffectiveLoanApplications(loanBatch.FirstOrDefault().Id);

                var farmerIds = loanApplications.Select(x => x.FarmerId).Distinct().ToList();

                var farmers = await _farmerRepository.GetAllAsync(f => farmerIds.Contains(f.Id));
                loanApplications.ForEach(x =>
                {
                    x.Farmer = farmers.FirstOrDefault(f => f.Id == x.FarmerId);
                });


                foreach (var item in deductibes)
                {
                    if (item.IsPaymentComplete == true)
                    {
                        try
                        {
                            Guid loanApplicationId = Guid.NewGuid();
                            var matchingLoanApplication = new LoanApplication();

                            if (loanApplications != null)
                            {
                                matchingLoanApplication = loanApplications?.FirstOrDefault(app => app.Farmer != null && app.Farmer.SystemId == item.SystemId);
                                if (matchingLoanApplication != null)
                                {
                                    loanApplicationId = matchingLoanApplication.Id;
                                }
                                else
                                {
                                    continue;
                                    //throw new InvalidOperationException($"No matching LoanApplication found for item with SystemId: {item.SystemId}");
                                }
                            }
                            else
                            {
                                continue;
                            }

                           

                          


                            /* newer method*/
                            _loanRepaymentRepository.ApplyPayment(
                               loanApplicationId,
                               item.FarmerLoansDeductionsLc,
                               paymentBatch != null ? paymentBatch.BatchName : "N/A",
                              "N/A"
                               );

                            if (paymentBatch != null)
                            {
                                paymentBatch.ReferenceNumber = "N/A";
                            }
                            await _paymentBatchRepository.UpdateAsync(paymentBatch);
                            // To generate the latest payment based loan statement
                            await _loanRepaymentService.GenerateLatestPaymentBasedLoanStatement(loanApplicationId, item.FarmerEarningsShareLc, "N/A");
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                }
            }


            //_loanRepaymentRepository.ApplyPayment(
            //   loanApplicationId,
            //     item.FarmerLoansDeductionsLc,
            //     paymentBatch != null ? paymentBatch.BatchName : "NA",
            //    refNumber
            //     );

            // TO DO
            // UPDATE LOAN : done
            //loan Repayment Repo update : done


        }
        else if (batch.PaymentModule == 4)
        {
            var facilitations = await _facilitationRepository.GetAllAsync(ti => ti.PaymentBatchId == id
                                && ti.IsDeleted == false);

            facilitations.ForEach(c =>
            {
                c.IsPaymentComplete = true;
                c.Remarks = "Processed manually";
                c.PaymentStatus = Guid.Parse("271d9c1a-2c4f-4ee2-ad0f-d7dc36bd255f"); // "Completed";
            });

            await _facilitationRepository.UpdateRange(facilitations);
        }

        //Get initiator name
        var users = await _userService.GetAllAsync("");
        var initiator = users.FirstOrDefault(user => user.Id == new Guid(_claimService.GetUserId()));
        if (initiator == null)
        {
            throw new Exception("User not found."); // Handle the case where the user is not found
        }

        await SaveActivityLog(new CreateActivityLogModel
        {
            Title = $"{batch.BatchName} - status updated",
            Description = $"{batch.Status} by {initiator.Username}",
            Link = $"/payment-batch/details/{batch.Id}"
        });

        //construct email using template
        var emailTemplates = await _emailTemplateRepository.GetAllAsync(tl => tl.Id == new Guid("35b845b6-37be-4705-8e14-b2d009f73b16"));
        var emailTemplate = emailTemplates.FirstOrDefault();
        emailTemplate.Variables = await _emailTemplateVariableRepository.GetAllAsync(tl => tl.EmailTemplateId == emailTemplate.Id);
        var emailBody = _emailTemplateService.RenderTemplate(
                         emailTemplate,
                         new Dictionary<string, string>
                        {
                             { "Initiator_Name", initiator.Username },
                             { "Link", $"{_portalSettings.PortalUrl}/payment-batch/details/{batch.Id}" }

                        });

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

        return new UpdatePaymentBatchResponseModel
        {
            Id = id
        };
    }
    #endregion

    #region Payment Transition methods

    public EffectiveRole GetEffectiveRole(PaymentBatchResponseModel batch)
    {
        return batch.BatchStatus switch
        {
            BatchStatus.Initiated => EffectiveRole.Initiator,
            BatchStatus.UnderReview => EffectiveRole.Reviewer,
            BatchStatus.PendingApproval => EffectiveRole.Approver,
            BatchStatus.Rejected => EffectiveRole.Initiator,
            BatchStatus.ReviewRejected => EffectiveRole.Initiator,
            _ => EffectiveRole.ReadOnly,
        };
    }

    public TransitionResponse GetActionContext(PaymentBatchResponseModel batch)
    {
        var _currentUserId = _claimService.GetUserId();

        var role = GetEffectiveRole(batch);
        var response = new TransitionResponse
        {
            EffectiveRole = role.ToString(),
            NextActions = new List<ActionOption>(),
            IsSelfAction = batch.CreatedBy == _currentUserId
        };

        switch (batch.BatchStatus)
        {
            case BatchStatus.Initiated:
                if (role == EffectiveRole.Initiator)
                {
                    response.Message = "Send for review. A reviewer will take over.";
                    response.NextActions.Add(new ActionOption
                    {
                        Action = "send-for-review",
                        Label = "Send for Review",
                        RestrictedForMaker = false,
                    });
                }
                break;

            case BatchStatus.UnderReview:
                if (role == EffectiveRole.Reviewer)
                {
                    response.Message = "Start review to view details and take action.";
                    response.NextActions.Add(new ActionOption
                    {
                        Action = "start-review",
                        Label = "Start Review",
                        RestrictedForMaker = true
                    });
                }

                break;

            case BatchStatus.PendingApproval:
                if (role == EffectiveRole.Approver)
                {
                    response.Message = "You can approve or reject this batch.";
                    response.NextActions.Add(new ActionOption { Action = "approve", Label = "Approve Batch", RestrictedForMaker = true });
                    response.NextActions.Add(new ActionOption { Action = "reject", Label = "Reject Batch", RestrictedForMaker = true });
                }
                break;

            case BatchStatus.Rejected:
            case BatchStatus.ReviewRejected:
                if (role == EffectiveRole.Initiator)
                {
                    response.Message = "Please re-upload and resubmit.";
                    response.NextActions.Add(new ActionOption { Action = "resubmit", Label = "Resubmit Batch" });
                }
                break;

            case BatchStatus.Approved:
                //if (role == EffectiveRole.Approver)
                //{
                response.Message = "This payment batch is approved and the payment has been sent for processing.";
                response.NextActions.Add(new ActionOption { RestrictedForMaker = false });
                //}
                break;
        }

        return response;
    }


    public bool TryTransition(PaymentBatchResponseModel batch, TransitionRequest req, out string error)
    {
        var role = GetEffectiveRole(batch);
        error = string.Empty;

        switch (batch.BatchStatus)
        {
            case BatchStatus.Initiated:
                if (role == EffectiveRole.Initiator && req.Action == "send-for-review")
                {
                    batch.BatchStatus = BatchStatus.UnderReview;
                    return true;
                }
                break;

            case BatchStatus.UnderReview:
                if (role == EffectiveRole.Reviewer)
                {
                    batch.BatchStatus = req.Action == "approve" ? BatchStatus.PendingApproval : BatchStatus.ReviewRejected;
                    return true;
                }
                break;

            case BatchStatus.PendingApproval:
                if (role == EffectiveRole.Approver)
                {
                    batch.BatchStatus = req.Action == "approve" ? BatchStatus.Approved : BatchStatus.Rejected;
                    return true;
                }
                break;

            case BatchStatus.Rejected:
            case BatchStatus.ReviewRejected:
                if (role == EffectiveRole.Initiator && req.Action == "resubmit")
                {
                    batch.BatchStatus = BatchStatus.Initiated;
                    return true;
                }
                break;
        }

        error = "Invalid transition for this role or batch status.";
        return false;
    }
    #endregion

    private BatchStatus? MapActionToNextStatus(string action, BatchStatus currentStatus)
    {
        return (currentStatus, action.ToLower()) switch
        {
            (BatchStatus.Initiated, "send-for-review") => BatchStatus.UnderReview,
            (BatchStatus.UnderReview, "review_next") => BatchStatus.PendingApproval,
            (BatchStatus.UnderReview, "review_rejected") => BatchStatus.ReviewRejected,
            (BatchStatus.PendingApproval, "approved") => BatchStatus.Approved,
            (BatchStatus.PendingApproval, "rejected") => BatchStatus.Rejected,
            (BatchStatus.Rejected, "resubmit") => BatchStatus.Initiated,
            (BatchStatus.ReviewRejected, "resubmit") => BatchStatus.Initiated,
            _ => null // Invalid or unsupported transition
        };
    }

}
