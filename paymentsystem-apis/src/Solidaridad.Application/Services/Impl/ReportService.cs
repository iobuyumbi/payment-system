using AutoMapper;
using Dapper;
using LinqToDB;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Npgsql;
using NPOI.SS.Formula.Functions;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Solidaridad.Application.Models;
using Solidaridad.Application.Models.Farmer;
using Solidaridad.Application.Models.LoanApplication;
using Solidaridad.Application.Models.LoanBatch;
using Solidaridad.Application.Models.LoanItem;
using Solidaridad.Application.Models.PaymentDeductible;
using Solidaridad.Application.Models.Reports;
using Solidaridad.Core.Entities;
using Solidaridad.Core.Entities.Base;
using Solidaridad.Core.Entities.Loans;
using Solidaridad.Core.Entities.Payments;
using PaymentRequestDeductible = Solidaridad.Core.Entities.Payments.PaymentRequestDeductible;
using Solidaridad.DataAccess.Identity;
using Solidaridad.DataAccess.Migrations;
using Solidaridad.DataAccess.Repositories;
using Solidaridad.DataAccess.Repositories.Impl;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using static Solidaridad.Application.Services.Impl.ReportService;
using static Solidaridad.DataAccess.Persistence.Seeding.Permission.Permissions;

namespace Solidaridad.Application.Services.Impl;

public class ReportService : IReportService
{
    #region DI
    private readonly IFarmerRepository _farmerRepo;
    private readonly IFarmerCooperativeRepository _farmerCooperativeRepository;
    private readonly ILoanBatchRepository _loanBatchRepository;
    private readonly ICooperativeRepository _cooperativeRepository;
    private readonly IJobExecutionLogRepository _jobExecutionLogRepository;
    private readonly IPaymentRequestDeductibleRepository _paymentRequestDeductibleRepository;
    private readonly IMapper _mapper;
    private readonly ILoanItemRepository _loanItemRepository;
    private readonly ILoanApplicationRepository _loanApplicationRepository;
    private readonly IApplicationStatusRepository _statusRepository;
    private readonly ILoanRepaymentRepository _loanRepaymentRepository;
    private readonly IEMIScheduleRepository _emiScheduleRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly IApplicationStatusLogRepository _applicationStatusLogRepository;
    private readonly IConfiguration _configuration;
    private readonly string _connectionString;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILoanStatementRepository _loanStatementRepository;
    private readonly IPaymentBatchRepository _paymentBatchRepository;
    private readonly IPaymentDeductibleStatusMasterRepository _paymentDeductibleStatusMasterRepository;
    private readonly ICountryRepository _countryRepository;
    private readonly IPaymentBatchProjectMappingRepository _paymentBatchProjectMappingRepository;
    private readonly IPaymenBatchLoanBatchMappingRepository _paymentBatchLoanBatchMappingRepository;
    private readonly ICallBackRepository _callBackRepository;






    public ReportService(IFarmerRepository farmerRepo, ICallBackRepository callBackRepository, IFarmerCooperativeRepository farmerCooperativeRepository,
        IPaymenBatchLoanBatchMappingRepository paymentBatchLoanBatchMappingRepository,
        IPaymentBatchProjectMappingRepository paymentBatchProjectMappingRepository,
        ILoanApplicationRepository loanApplicationRepository, 
        IPaymentDeductibleStatusMasterRepository paymentDeductibleStatusMasterRepository,
        ICountryRepository countryRepository,
        ILoanStatementRepository loanStatementRepository,
         IApplicationStatusLogRepository applicationStatusLogRepository,
         IProjectRepository projectRepository,
         ILoanRepaymentRepository loanRepaymentRepository,
         ILoanItemRepository loanItemRepository, 
         ILoanBatchRepository loanBatchRepository,
         IPaymentBatchRepository paymentBatchRepository,
        
         IEMIScheduleRepository emiScheduleRepository, 
         IMapper mapper,
         IPaymentRequestDeductibleRepository paymentRequestDeductibleRepository,
         IJobExecutionLogRepository jobExecutionLogRepository , 
         IApplicationStatusRepository statusRepository,
         ICooperativeRepository cooperativeRepository, 
         UserManager<ApplicationUser> userManager, 
         IConfiguration configuration)
    {
        _farmerRepo = farmerRepo;
        _loanBatchRepository = loanBatchRepository;
        _cooperativeRepository = cooperativeRepository;
        _jobExecutionLogRepository = jobExecutionLogRepository;
        _paymentRequestDeductibleRepository = paymentRequestDeductibleRepository;
        _mapper = mapper;
        _loanItemRepository = loanItemRepository;
        _loanApplicationRepository = loanApplicationRepository;
        _statusRepository = statusRepository;
        _loanRepaymentRepository = loanRepaymentRepository;
        _emiScheduleRepository = emiScheduleRepository;
        _projectRepository = projectRepository;
        _applicationStatusLogRepository = applicationStatusLogRepository;
        _userManager = userManager;
        _configuration = configuration;
        _connectionString = _configuration.GetSection("ConnectionStrings").Get<DatabaseConfiguration>().DefaultConnection;
        _loanStatementRepository = loanStatementRepository;
        _paymentBatchRepository = paymentBatchRepository;
        _paymentDeductibleStatusMasterRepository = paymentDeductibleStatusMasterRepository;
        _countryRepository = countryRepository;
        _paymentBatchLoanBatchMappingRepository = paymentBatchLoanBatchMappingRepository;
        _paymentBatchProjectMappingRepository = paymentBatchProjectMappingRepository;
        _callBackRepository = callBackRepository;
        _farmerCooperativeRepository = farmerCooperativeRepository;




    }

    #endregion

    #region Methods

    #region Mostly Outdated
    public async Task<List<KeyMetricsModel>> GetStats(int year, Guid countryId)
    {
        var keyMetrics = new List<KeyMetricsModel>();

        // keyMetrics.Add(new KeyMetricsModel { Title = "Countries", Value = 3 });
        keyMetrics.Add(new KeyMetricsModel { Title = "Farmers", Value = _farmerRepo.GetCount(countryId) });
        keyMetrics.Add(new KeyMetricsModel { Title = "Projects", Value = _loanBatchRepository.GetProjectCount(countryId) });
        keyMetrics.Add(new KeyMetricsModel { Title = "Co-operatives", Value = _cooperativeRepository.GetCount(countryId) });
        keyMetrics.Add(new KeyMetricsModel { Title = "Loan Products", Value = _loanBatchRepository.GetLoanBatchCount(countryId) });

        return keyMetrics;
    }

    public async Task<IEnumerable<JobExecutionLog>> GetJobExecutionLog()
    {
        return await _jobExecutionLogRepository.GetAllAsync(c => 1 == 1);
    }

    #region PaymentBatch 
    public async Task<PaymentConfirmationResponseModel> GetPaymentConfirmationReport(SearchParams searchParams)
    {
        var _payments = await _paymentRequestDeductibleRepository.GetAllAsync(c => c.CreatedOn <searchParams.ToDate && c.CreatedOn > searchParams.FromDate );
        var payments = _mapper.Map<IEnumerable<PaymentDeductibleResponseModel>>(_payments);
        PaymentStatusResponseModel stats = new PaymentStatusResponseModel
        {
            TotalPayment=_payments.Count(),
            PendingPayments=_payments.Count(c=> !c.IsPaymentComplete),
            CompletedPayments= _payments.Count(c => c.IsPaymentComplete),
        };

        return new PaymentConfirmationResponseModel { 
            PaymentDeductibles= payments,
            PaymentStatus = stats
        } ;
    }

    #endregion

    #region Loan Account Reports
    public async Task<LoanAccountResponseModel> GetLoanAccountReports(SearchParams searchParams)
    {
        try
        {
            var _loanItems = await _loanItemRepository.GetAllAsync(c => true) ?? Enumerable.Empty<LoanItem>();
            var _loanApplications = await _loanApplicationRepository.GetAllAsync(c => true) ?? Enumerable.Empty<LoanApplication>();

            LoanItemStatsResponseModel loanItemStats = new LoanItemStatsResponseModel
            {
                TotalItemsLoaned = _loanItems.Count(),
                TotalItemValue = (float)_loanItems.Sum(c => (c.Quantity * c.UnitPrice)),
                TotalFeeCharged = (float)_loanApplications.Sum(c => c.FeeApplied),
                PrincipleAmount = (float)_loanApplications.Sum(c => c.PrincipalAmount),
                EffectiveBalance = (float)_loanApplications.Sum(c => c.EffectivePrincipal),
                InterestEarned = (float)_loanApplications.Sum(c => c.InterestAmount),
                EffectiveLoanBalance = (float)_loanApplications.Sum(c => c.RemainingBalance)
            };

            var _payments = await _paymentRequestDeductibleRepository.GetAllAsync(c => c.CreatedOn < searchParams.ToDate && c.CreatedOn > searchParams.FromDate)
                             ?? Enumerable.Empty<PaymentRequestDeductible>();

            var payments = _mapper.Map<IEnumerable<PaymentDeductibleResponseModel>>(_payments);

            PaymentStatusResponseModel stats = new PaymentStatusResponseModel
            {
                TotalPayment = _payments.Count(),
                PendingPayments = _payments.Count(c => !c.IsPaymentComplete),
                CompletedPayments = _payments.Count(c => c.IsPaymentComplete),
            };

            var loanApplications = _mapper.Map<ReadOnlyCollection<LoanApplicationResponseModel>>(_loanApplications);

            return new LoanAccountResponseModel
            {
                LoanApplications = loanApplications,
                LoanItemStats = loanItemStats,
            };
        }
        catch (Exception ex)
        {
            // Log the error (use a proper logging framework like Serilog, NLog, or ILogger)
            Console.WriteLine($"Error fetching loan account reports: {ex.Message}");

            // Return a safe fallback response
            return new LoanAccountResponseModel
            {
                LoanApplications = new ReadOnlyCollection<LoanApplicationResponseModel>(new List<LoanApplicationResponseModel>()),
                LoanItemStats = new LoanItemStatsResponseModel()
            };
        }
    }

  


    #endregion

    #region Loan Batch Reports 
      public async Task<LoanBatchReportResponseModel> GetLoanBatchReports(SearchParams searchParams)
    {
        try
        {
            var projects = await _projectRepository.GetAllAsync(c => c.CountryId == searchParams.CountryId && !c.IsDeleted);
            var projectIds = projects.Select(p => p.Id).ToList();

            var _loanBatches = await _loanBatchRepository.GetAllAsync(x => projectIds.Contains(x.ProjectId) && !x.IsDeleted && x.StatusId != 1);
            var loanBatchIds = _loanBatches.Select(lb => lb.Id).ToList();

            var _loanApplications = await _loanApplicationRepository.GetAllAsync(x => loanBatchIds.Contains(x.LoanBatchId)
            && !x.IsDeleted 
            && x.Status != new Guid("3118a07e-013a-4b3a-a2c1-74c921feeba1"));

            var _loanItems = await _loanItemRepository.GetAllAsync(c => true) ?? Enumerable.Empty<LoanItem>();
           
            var _status = await _statusRepository.GetAllAsync(c => 1 == 1);
            

            LoanBatchStatsResponseModel loanItemStats = new LoanBatchStatsResponseModel
            {    
                TotalLoanValue= (float)_loanApplications.Sum(c => c.PrincipalAmount),
                TotalLoanBatches = _loanBatches.Count(),
                TotalLoans=_loanApplications.Count(),
                ActiveLoanBatches = _loanBatches.Count(c=> c.StatusId != 4),
                ClosedLoanBatches = _loanBatches.Count(c => c.StatusId == 4),
                ActiveLoans=_loanApplications.Count(c=> c.Status != new Guid("f49faffa-b113-4546-ac7f-485164e5a822") && c.Status != new Guid("3118a07e-013a-4b3a-a2c1-74c921feeba1")),
                ClosedLoans= _loanApplications.Count(c => c.Status == new Guid("f49faffa-b113-4546-ac7f-485164e5a822")),
                OverdueLoans =0,
                NonPerformingLoans =0,
                NonPerformingValue =0,
               
            };

            //var _payments = await _paymentRequestDeductibleRepository.GetAllAsync(c => c.CreatedOn < searchParams.ToDate && c.CreatedOn > searchParams.FromDate)
            //                 ?? Enumerable.Empty<PaymentRequestDeductible>();

            //var payments = _mapper.Map<IEnumerable<PaymentDeductibleResponseModel>>(_payments);

          

            var loanApplications = _mapper.Map<ReadOnlyCollection<LoanApplicationResponseModel>>(_loanApplications);

            return new LoanBatchReportResponseModel
            {
                LoanApplications = loanApplications,
                LoanBatchStatsResponseModel = loanItemStats,
            };
        }
        catch (Exception ex)
        {
            // Log the error (use a proper logging framework like Serilog, NLog, or ILogger)
            Console.WriteLine($"Error fetching loan portfolio reports: {ex.Message}");

            // Return a safe fallback response
            return new LoanBatchReportResponseModel
            {
                LoanApplications = new ReadOnlyCollection<LoanApplicationResponseModel>(new List<LoanApplicationResponseModel>()),
                LoanBatchStatsResponseModel = new LoanBatchStatsResponseModel()
            };
        }
    }

    public async Task<IEnumerable<RepaymentReportsResponseModel>> GetRepaymentReports(Guid? countryId)
    {
        var projects = await _projectRepository.GetAllAsync(c => c.CountryId == countryId && !c.IsDeleted);
        var projectIds = projects.Select(p => p.Id).ToList();

        var loanBatches = await _loanBatchRepository.GetAllAsync(x => projectIds.Contains(x.ProjectId) && x.StatusId != 1);
        var loanBatchIds = loanBatches.Select(lb => lb.Id).ToList();

        var loanApplications = await _loanApplicationRepository.GetAllAsync(x => loanBatchIds.Contains(x.LoanBatchId) && !x.IsDeleted 
        && x.Status == new Guid("e24d24a8-fc69-4527-a92a-97f6648a43c5"));
        var loanApplicationIds = loanApplications.Select(app => app.Id).ToList();

        var currentYear = DateTime.UtcNow.Year;
        var repayments = await _loanRepaymentRepository.GetAllAsync(x =>
            loanApplicationIds.Contains(x.LoanApplicationId) &&
            !x.IsDeleted &&
            x.PaymentDate.Year == currentYear
        );

        // Step 1: Generate empty report for all 12 months
        var allMonths = Enumerable.Range(1, 12)
            .Select(m => new RepaymentReportsResponseModel
            {
                Month = new DateTime(currentYear, m, 1).ToString("MMMM"),
                RepaymentAmount = 0
            })
            .ToList();

        // Step 2: Group actual repayments by month
        var repaymentByMonth = repayments
            .GroupBy(r => r.PaymentDate.Month)
            .ToDictionary(
                g => g.Key,
                g => g.Sum(r => r.AmountPaid)
            );

        // Step 3: Merge real data into allMonths list
        foreach (var report in allMonths)
        {
            var monthNum = DateTime.ParseExact(report.Month, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
            if (repaymentByMonth.TryGetValue(monthNum, out var total))
            {
                report.RepaymentAmount = total;
            }
        }

        return allMonths;
    }
    #endregion

    #endregion



    #region LoanApplicationReport

    #region GeneralApplicationReport
    public async Task<IEnumerable<LoanApplicationResponseModel>> LoanApplicationReports(PortolioSearchParams portolioSearchParams)
    {
        var projects = await _projectRepository.GetAllAsync(c => c.CountryId == portolioSearchParams.CountryId && !c.IsDeleted);
        var projectIds = projects.Select(p => p.Id).ToList();
       

        var batchIds = portolioSearchParams.BatchIds;
        var _loanBatches = await _loanBatchRepository.GetAllAsync(x => projectIds.Contains(x.ProjectId) && x.StatusId != 1 && batchIds.Contains(x.Id));
        var loanBatchIds = _loanBatches.Select(lb => lb.Id).ToList();

       

      

        var _loanApplications = await _loanApplicationRepository.GetAllAsync(x =>
            batchIds.Contains(x.LoanBatchId) &&
            !x.IsDeleted &&
          ( portolioSearchParams.StatusId == ""||Guid.Parse(portolioSearchParams.StatusId) == x.Status) &&
            (portolioSearchParams.OfficerId == "" || x.OfficerId == Guid.Parse(portolioSearchParams.OfficerId)) 
          
        );





        var loanApplications = _mapper.Map<ReadOnlyCollection<LoanApplicationResponseModel>>(_loanApplications);
        var currentUser = await _userManager.FindByIdAsync(portolioSearchParams.CurrentUser.ToString());
        foreach (var loanApp in loanApplications)
        {
            loanApp.CurrentUserName = currentUser.UserName; 
        }
        var farmerList = await _farmerRepo.GetAllAsync(c => c.Id == c.Id);
        var applicationStatusList = await _statusRepository.GetAllAsync(c => true);


        

        foreach (var loanApplication in loanApplications)
        {
            // farmer
            var farmers = farmerList.ToList().Where(c => c.Id == loanApplication.FarmerId);
            if (farmers != null && farmers.Any())
            {
                loanApplication.Farmer = _mapper.Map<FarmerResponseModel>(farmers.FirstOrDefault());
            }
            var latestStatements = (await _loanStatementRepository.GetAllAsync(
     x => x.ApplicationId == loanApplication.Id && x.StatementDate <= portolioSearchParams.EndDate))
     .OrderByDescending(x => x.StatementDate);
            decimal accuredInterest = 0;
            foreach(var statement in latestStatements)
            {
                accuredInterest += statement.DebitAmount;

            }


            var latest = latestStatements.FirstOrDefault();

            loanApplication.PrincipalDue = latest?.AccuredPrincipalPayment ?? 0;
            loanApplication.PrincipalReceived = latest?.PrincipalPaid ?? 0;
            loanApplication.TotalInterest = loanApplication.InterestAmount;
            loanApplication.InterestDue = latest?.AccuredInterest ?? 0;
            loanApplication.InterestReceived = latest?.InterestPaid ?? 0;
            loanApplication.InterestArrears = latest?.AccuredInterest - latest?.InterestPaid ?? 0;
            loanApplication.PrincipalArrears = (latest?.AccuredPrincipalPayment ?? 0) - (latest?.PrincipalPaid ?? 0);
            loanApplication.TotalArrears = (latest?.AccuredInterest - latest?.InterestPaid) + (latest?.AccuredPrincipalPayment - latest?.PrincipalPaid) ?? 0;
            loanApplication.TotalExpected = latest?.AccuredInterest + latest?.AccuredPrincipalPayment ?? 0;
            loanApplication.ArrearsRate = ((loanApplication?.TotalArrears ?? 0) / (loanApplication?.TotalExpected == null || loanApplication.TotalExpected == 0 ? 1 : loanApplication.TotalExpected));
            //loan batches
            var loanBatches = _loanBatches.ToList().Where(c => c.Id == loanApplication.LoanBatchId);
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
            var disbursedStatusId = Guid.Parse("e24d24a8-fc69-4527-a92a-97f6648a43c5");
          

            var disbursedLog = (await _applicationStatusLogRepository.GetAllAsync(c =>
                c.ApplicationId == loanApplication.Id && c.StatusId == disbursedStatusId))
                .OrderByDescending(x => x.CreatedOn)
                .FirstOrDefault();

            if (disbursedLog != null)
            {
                loanApplication.DisbursedOn = disbursedLog.CreatedOn;
            }

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
 
    public async Task<byte[]> GenerateLoanPortfolioPdfReport(PortolioSearchParams searchParams)
    {
        var loanApplications = (await LoanApplicationReports(searchParams)).ToList();

        if (!loanApplications.Any())
            throw new Exception("No loan applications found for the provided filters.");

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4.Landscape());
                page.Margin(5, Unit.Millimetre);
                page.DefaultTextStyle(x => x.FontSize(7).FontFamily("Arial"));

                page.Header().PaddingVertical(6).Row(row =>
                {
                    row.ConstantItem(100).Image("wwwroot/logos/logo.png", ImageScaling.FitWidth);
                    var address = CountryAddress.GetAddressByCountryId((Guid)searchParams.CountryId);
                    row.RelativeItem().Column(col =>
                    {
                        col.Item().AlignRight().Text("Solidaridad").Bold().FontSize(12);
                        col.Item().AlignRight().Text(address.Line1);
                        col.Item().AlignRight().Text(address.Line2);
                        col.Item().AlignRight().Text(address.Line3);
                        col.Item().AlignRight().Text(address.Line4);
                        col.Item().AlignRight().Text(address.Phone);
                        col.Item().AlignRight().Text(address.Email);


                        //col.Item().AlignRight().Text("Solidaridad").Bold().FontSize(12);
                        //col.Item().AlignRight().Text("Kilimani Business Centre, Kirichwa Road");
                        //col.Item().AlignRight().Text("P.O. Box 42234 – 00100");
                        //col.Item().AlignRight().Text("Nairobi, Kenya");
                        //col.Item().AlignRight().Text("Phone: +254 (0) 716 666 862");
                        //col.Item().AlignRight().Text("Email: info.secaec@solidaridadnetwork.org");
                    });
                });

                page.Content().Column(col =>
                {
                    col.Spacing(20);
                    var eatNow = TimeZoneInfo.ConvertTimeFromUtc(searchParams.EndDate, TimeZoneInfo.FindSystemTimeZoneById("E. Africa Standard Time"));
                    col.Item().AlignCenter().Text($"Loan Portfolio Report ").Bold().FontSize(14);

                    // Net total accumulators
                    decimal netDisbursedTotal = 0,
                            netPrincipalDue = 0,
                            netPrincipalReceived = 0,
                            netPrincipalArrears = 0,
                            netRemainingBalance = 0,
                            netTotalInterest = 0,
                            netInterestDue = 0,
                            netInterestReceived = 0,
                            netInterestArrears = 0,
                            netTotalArrears = 0,
                            netTotalExpected = 0;

                    var batches = loanApplications.GroupBy(x => x.LoanBatch.Id).Select(g => new
                    {
                        Batch = g.First().LoanBatch,
                        Applications = g.ToList()
                    });

                    foreach (var batchGroup in batches)
                    {
                        var batch = batchGroup.Batch;
                        var apps = batchGroup.Applications;

                        col.Item().PaddingVertical(4).Background(Colors.Grey.Lighten3).Padding(5)
                            .Text($"Loan Product Name as of {eatNow:yyyy-MM-dd HH:mm} EAT: {batch.Name}")
                            .Bold().FontSize(11).FontColor(Colors.Black);

                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                for (int i = 0; i < 18; i++) columns.RelativeColumn();
                            });

                            string[] headers = new[]
                            {
                            "Loan account number","Loan disbursement date", "Interest Rate", "Loan Term", "Farmer System Id", "Farmer name",
                            "Amount Disbursed", "Per schedule principal due to date", "Principal received to date",
                            "Principal Balance", "Principal in Arrears",
                            "Total Interest", "Per schedule Interest due to date", "Interest Received to Date",
                            "Interest Arrears", "Total Arrears", "Total Expected", "Arrears Rate [%]"
                        };

                            foreach (var header in headers)
                                table.Cell().Element(HeaderCellStyle).Text(header).Bold();

                            // Batch totals
                            decimal disbursedTotal = 0,
                                    principalDueTotal = 0,
                                    principalReceivedTotal = 0,
                                    principalArrearsTotal = 0,
                                    remainingBalanceTotal = 0,
                                    totalInterest = 0,
                                    interestDueTotal = 0,
                                    interestReceivedTotal = 0,
                                    interestArrearsTotal = 0,
                                    totalArrears = 0,
                                    totalExpected = 0;

                            foreach (var app in apps)
                            {
                                var remainingBalance = app.PrincipalDue - app.PrincipalReceived;

                                table.Cell().Element(CellStyle).Text(app.LoanNumber);
                                table.Cell().Element(CellStyle).Text(TimeZoneInfo.ConvertTimeFromUtc(app.DisbursedOn, TimeZoneInfo.FindSystemTimeZoneById("E. Africa Standard Time")).ToString("yyyy-MM-dd HH:mm") + " EAT");
                                table.Cell().Element(CellStyle).Text(app.LoanBatch.InterestRate.ToString("N2"));
                                table.Cell().Element(CellStyle).Text(app.LoanBatch.Tenure.ToString());
                                table.Cell().Element(CellStyle).Text(app.Farmer?.SystemId);
                                table.Cell().Element(CellStyle).Text($"{app.Farmer?.FirstName} {app.Farmer?.OtherNames}");
                                table.Cell().Element(CellStyle).Text(app.PrincipalAmount.ToString("N2"));
                                table.Cell().Element(CellStyle).Text(app.PrincipalDue.ToString("N2"));
                                table.Cell().Element(CellStyle).Text(app.PrincipalReceived.ToString("N2"));
                                table.Cell().Element(CellStyle).Text(remainingBalance.ToString("N2"));
                                table.Cell().Element(CellStyle).Text(app.PrincipalArrears.ToString("N2"));
                                table.Cell().Element(CellStyle).Text(app.TotalInterest.ToString("N2"));
                                table.Cell().Element(CellStyle).Text(app.InterestDue.ToString("N2"));
                                table.Cell().Element(CellStyle).Text(app.InterestReceived.ToString("N2"));
                                table.Cell().Element(CellStyle).Text(app.InterestArrears.ToString("N2"));
                                table.Cell().Element(CellStyle).Text(app.TotalArrears.ToString("N2"));
                                table.Cell().Element(CellStyle).Text(app.TotalExpected.ToString("N2"));
                                table.Cell().Element(CellStyle).Text((app.ArrearsRate * 100).ToString("N2"));

                                // Add to batch totals
                                disbursedTotal += app.PrincipalAmount;
                                principalDueTotal += app.PrincipalDue;
                                principalReceivedTotal += app.PrincipalReceived;
                                principalArrearsTotal += app.PrincipalArrears;
                                remainingBalanceTotal += remainingBalance;
                                totalInterest += app.TotalInterest;
                                interestDueTotal += app.InterestDue;
                                interestReceivedTotal += app.InterestReceived;
                                interestArrearsTotal += app.InterestArrears;
                                totalArrears += app.TotalArrears;
                                totalExpected += app.TotalExpected;
                            }

                            decimal totalArrearsRate = (totalExpected == 0 ? 0 : (totalArrears / totalExpected));

                            void TotalCell(string text) => table.Cell().Element(FooterCellStyle).Text(text).Bold();

                            TotalCell("TOTAL"); TotalCell(""); TotalCell(""); TotalCell(""); TotalCell(""); TotalCell("");
                            TotalCell(disbursedTotal.ToString("N2"));
                            TotalCell(principalDueTotal.ToString("N2"));
                            TotalCell(principalReceivedTotal.ToString("N2"));
                            TotalCell(remainingBalanceTotal.ToString("N2"));
                            TotalCell(principalArrearsTotal.ToString("N2"));
                            TotalCell(totalInterest.ToString("N2"));
                            TotalCell(interestDueTotal.ToString("N2"));
                            TotalCell(interestReceivedTotal.ToString("N2"));
                            TotalCell(interestArrearsTotal.ToString("N2"));
                            TotalCell(totalArrears.ToString("N2"));
                            TotalCell(totalExpected.ToString("N2"));
                            TotalCell((totalArrearsRate * 100).ToString("N2"));

                            // Add to net totals
                            netDisbursedTotal += disbursedTotal;
                            netPrincipalDue += principalDueTotal;
                            netPrincipalReceived += principalReceivedTotal;
                            netPrincipalArrears += principalArrearsTotal;
                            netRemainingBalance += remainingBalanceTotal;
                            netTotalInterest += totalInterest;
                            netInterestDue += interestDueTotal;
                            netInterestReceived += interestReceivedTotal;
                            netInterestArrears += interestArrearsTotal;
                            netTotalArrears += totalArrears;
                            netTotalExpected += totalExpected;
                        });

                        col.Item().PaddingVertical(10).LineHorizontal(1).LineColor(Colors.Grey.Lighten2);
                    }

                    // Net Totals Table
                    col.Item().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            for (int i = 0; i < 18; i++) columns.RelativeColumn();
                        });

                        void TotalCell(string text) => table.Cell().Element(FooterCellStyle).Text(text).Bold();

                        decimal netArrearsRate = (netTotalExpected == 0 ? 0 : (netTotalArrears / netTotalExpected));

                        TotalCell("NET TOTAL"); TotalCell(""); TotalCell(""); TotalCell(""); TotalCell(""); TotalCell("");
                        TotalCell(netDisbursedTotal.ToString("N2"));
                        TotalCell(netPrincipalDue.ToString("N2"));
                        TotalCell(netPrincipalReceived.ToString("N2"));
                        TotalCell(netRemainingBalance.ToString("N2"));
                        TotalCell(netPrincipalArrears.ToString("N2"));
                        TotalCell(netTotalInterest.ToString("N2"));
                        TotalCell(netInterestDue.ToString("N2"));
                        TotalCell(netInterestReceived.ToString("N2"));
                        TotalCell(netInterestArrears.ToString("N2"));
                        TotalCell(netTotalArrears.ToString("N2"));
                        TotalCell(netTotalExpected.ToString("N2"));
                        TotalCell((netArrearsRate * 100).ToString("N2"));
                    });
                });

                page.Footer().AlignCenter().Column(col =>
                {
                    col.Item().Text(txt =>
                    {
                        txt.Span("Generated on: ").SemiBold();
                        txt.Span(TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,
                            TimeZoneInfo.FindSystemTimeZoneById("E. Africa Standard Time"))
                            .ToString("yyyy-MM-dd HH:mm") + " EAT");
                    });

                    col.Item().Text(txt =>
                    {
                        txt.Span("Generated by: ").SemiBold();
                        txt.Span(loanApplications[0].CurrentUserName);
                    });
                });
            });
        });

        return document.GeneratePdf();

        static IContainer CellStyle(IContainer container) =>
            container.PaddingVertical(3).PaddingHorizontal(2).BorderBottom(1).BorderColor(Colors.Grey.Lighten1);

        static IContainer HeaderCellStyle(IContainer container) =>
            container.PaddingVertical(4).PaddingHorizontal(2);

        static IContainer FooterCellStyle(IContainer container) =>
            container.Background(Colors.Grey.Lighten3).PaddingVertical(4).PaddingHorizontal(2);
    }



    #endregion

    #region GlobalLoanApplicationReport
    public async Task<IEnumerable<GlobalLoanApplicationReportResponseModel>> GlobalLoanApplicationReports(PortolioSearchParams portolioSearchParams)
    {
        var result = new List<GlobalLoanApplicationReportResponseModel>();
        var projects = await _projectRepository.GetAllAsync(c =>  !c.IsDeleted);
        var projectIds = projects.Select(p => p.Id).ToList();
        var countries = await _countryRepository.GetAllAsync(x => true);
        var cooperatives = await _cooperativeRepository.GetAllAsync(c => true);


        var _loanBatches = await _loanBatchRepository.GetAllAsync(x => projectIds.Contains(x.ProjectId) && x.StatusId != 1 );
        var loanBatchIds = _loanBatches.Select(lb => lb.Id).ToList();





        var _loanApplications = await _loanApplicationRepository.GetAllAsync(x =>
            !x.IsDeleted &&
          (portolioSearchParams.StatusId == "" || Guid.Parse(portolioSearchParams.StatusId) == x.Status) &&
            (portolioSearchParams.OfficerId == "" || x.OfficerId == Guid.Parse(portolioSearchParams.OfficerId))

        );





        var loanApplications = _mapper.Map<ReadOnlyCollection<LoanApplicationResponseModel>>(_loanApplications);
        var currentUser = await _userManager.FindByIdAsync(portolioSearchParams.CurrentUser.ToString());
        foreach (var loanApp in loanApplications)
        {
            loanApp.CurrentUserName = currentUser.UserName;
        }
        var farmerList = await _farmerRepo.GetAllAsync(c => c.Id == c.Id);
        var applicationStatusList = await _statusRepository.GetAllAsync(c => true);




        foreach (var loanApplication in loanApplications)
        {
            // farmer
            var farmers = farmerList.ToList().Where(c => c.Id == loanApplication.FarmerId);
            if (farmers != null && farmers.Any())
            {
                loanApplication.Farmer = _mapper.Map<FarmerResponseModel>(farmers.FirstOrDefault());
            }
            var latestStatements = (await _loanStatementRepository.GetAllAsync(
     x => x.ApplicationId == loanApplication.Id && x.StatementDate <= portolioSearchParams.EndDate))
     .OrderByDescending(x => x.StatementDate);
            decimal accuredInterest = 0;
            foreach (var statement in latestStatements)
            {
                accuredInterest += statement.DebitAmount;

            }


            var latest = latestStatements.FirstOrDefault();

            loanApplication.PrincipalDue = latest?.AccuredPrincipalPayment ?? 0;
            loanApplication.PrincipalReceived = latest?.PrincipalPaid ?? 0;
            loanApplication.TotalInterest = loanApplication.InterestAmount;
            loanApplication.InterestDue = latest?.AccuredInterest ?? 0;
            loanApplication.InterestReceived = latest?.InterestPaid ?? 0;
            loanApplication.InterestArrears = latest?.AccuredInterest - latest?.InterestPaid ?? 0;
            loanApplication.PrincipalArrears = (latest?.AccuredPrincipalPayment ?? 0) - (latest?.PrincipalPaid ?? 0);
            loanApplication.TotalArrears = (latest?.AccuredInterest - latest?.InterestPaid) + (latest?.AccuredPrincipalPayment - latest?.PrincipalPaid) ?? 0;
            loanApplication.TotalExpected = latest?.AccuredInterest + latest?.AccuredPrincipalPayment ?? 0;
            loanApplication.ArrearsRate = ((loanApplication?.TotalArrears ?? 0) / (loanApplication?.TotalExpected == null || loanApplication.TotalExpected == 0 ? 1 : loanApplication.TotalExpected));
            //loan batches
            var loanBatches = _loanBatches.ToList().Where(c => c.Id == loanApplication.LoanBatchId);
            if (loanBatches != null && loanBatches.Any())
            {
                loanApplication.LoanBatch = _mapper.Map<LoanBatchResponseModel>(loanBatches.FirstOrDefault());
            }
            else { continue; }

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
            var disbursedStatusId = Guid.Parse("e24d24a8-fc69-4527-a92a-97f6648a43c5");


            var disbursedLog = (await _applicationStatusLogRepository.GetAllAsync(c =>
                c.ApplicationId == loanApplication.Id && c.StatusId == disbursedStatusId))
                .OrderByDescending(x => x.CreatedOn)
                .FirstOrDefault();

            if (disbursedLog != null)
            {
                loanApplication.DisbursedOn = disbursedLog.CreatedOn;
            }

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

            var cooperative = await GetCooperatives(loanApplication.Farmer.Id);
            string cooperativeNames = string.Join(", ", cooperative.Select(c => c.Label));

            var application = new GlobalLoanApplicationReportResponseModel {
            LoanNumber = loanApplication.LoanNumber,
            LoanProduct=loanApplication.LoanBatch.Name,
            DateOfWitness = loanApplication.DateOfWitness,
            CreatedOn =loanApplication.CreatedOn,
            Status =loanApplication.StatusText,
            InterestAmount =loanApplication.InterestAmount,
            PrincipalAmount =loanApplication.PrincipalAmount,
            RemainingBalance =loanApplication.RemainingBalance,
            WitnessName =loanApplication.WitnessFullName,
            WitnessPhoneNumber =loanApplication.WitnessPhoneNo,
            WitnessNationalId =loanApplication.WitnessNationalId,
            WitnessRelation =loanApplication.WitnessRelation,
            FeeApplied =loanApplication.FeeApplied,
            ProjectName =loanApplication.LoanBatch.Project.ProjectName,
            InterestRate=loanApplication.LoanBatch.InterestRate,
            InterestType=loanApplication.LoanBatch.RateType,
            InterestCalculationType ="Monthly",
            Tenure=loanApplication.LoanBatch.Tenure,
            SystemId =loanApplication.Farmer.SystemId,
            FirstName = loanApplication.Farmer.FirstName,
            OtherNames= loanApplication.Farmer.OtherNames,
            CountryName= countries.FirstOrDefault(x=> x.Id == loanApplication.LoanBatch.Project.CountryId).CountryName,
            Mobile = loanApplication.Farmer.Mobile,
            PaymentPhoneNumber = loanApplication.Farmer.PaymentPhoneNumber,
            AccessToMobile = loanApplication.Farmer.AccessToMobile,
            AlternateContactNumber = loanApplication.Farmer.AlternateContactNumber,
            BeneficiaryId = loanApplication.Farmer.BeneficiaryId,
            BirthMonth = loanApplication.Farmer.BirthMonth,
            BirthYear = loanApplication.Farmer.BirthYear,
            CooperativeName = cooperativeNames != null ? cooperativeNames : "",
            EnumerationDate = loanApplication.Farmer.EnumerationDate,

};

            result.Add(application);
        }

        return _mapper.Map<IEnumerable<GlobalLoanApplicationReportResponseModel>>(result);
    }

    public async Task<byte[]> GenerateGlobalLoanApplicationPdfReport(PortolioSearchParams searchParams)
    {
        var loanApplications = (await GlobalLoanApplicationReports(searchParams)).ToList();

        if (!loanApplications.Any())
            throw new Exception("No loan applications found for the provided filters.");

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A3.Landscape());
                page.Margin(5, Unit.Millimetre);
                page.DefaultTextStyle(x => x.FontSize(5).FontFamily("Arial"));

                // Header
                page.Header().PaddingVertical(6).Row(row =>
                {
                    row.ConstantItem(100).Image("wwwroot/logos/logo.png", ImageScaling.FitWidth);
                    var address = CountryAddress.GetAddressByCountryId((Guid)searchParams.CountryId);
                    row.RelativeItem().Column(col =>
                    {
                        col.Item().AlignRight().Text("Solidaridad").Bold().FontSize(12);
                        col.Item().AlignRight().Text(address.Line1);
                        col.Item().AlignRight().Text(address.Line2);
                        col.Item().AlignRight().Text(address.Line3);
                        col.Item().AlignRight().Text(address.Line4);
                        col.Item().AlignRight().Text(address.Phone);
                        col.Item().AlignRight().Text(address.Email);
                    });
                });

                page.Content().Column(col =>
                {
                    col.Spacing(20);
                    var eatNow = TimeZoneInfo.ConvertTimeFromUtc(searchParams.EndDate,
                                    TimeZoneInfo.FindSystemTimeZoneById("E. Africa Standard Time"));

                    col.Item().AlignCenter()
                        .Text($"Loan Applications Report as of {eatNow:yyyy-MM-dd HH:mm} EAT")
                        .Bold().FontSize(14);

                    // Table
                    col.Item().Table(table =>
                    {
                        // Define columns for each property in the model
                        string[] headers = new[]
                        {
                        "Loan Number",
                        "Loan Product",
                        "System Id",
                        "Beneficiary Id",
                        "First Name",
                        "Other Names",
                        "Mobile",
                        "Payment Phone",
                        "Principal Amount",
                        "Remaining Balance",
                        "Interest Amount",
                        "Fee Applied",
                        "Status",
                        "Witness Name",
                        "Witness Phone",
                        "Witness National ID",
                        "Project Name",
                        "Interest Rate",
                        "Interest Type",
                        "Tenure",
                        "Country",
                        "Cooperative",
                        "Created On",
                    };

                        // Create equal column widths
                        table.ColumnsDefinition(columns =>
                        {
                            for (int i = 0; i < headers.Length; i++)
                                columns.RelativeColumn();
                        });

                        // Add headers
                        foreach (var header in headers)
                            table.Cell().Element(HeaderCellStyle).Text(header).Bold();

                        // Add data rows
                        foreach (var app in loanApplications)
                        {
                            table.Cell().Element(CellStyle).Text(app.LoanNumber);
                            table.Cell().Element(CellStyle).Text(app.LoanProduct);
                            table.Cell().Element(CellStyle).Text(app.SystemId);
                            table.Cell().Element(CellStyle).Text(app.BeneficiaryId);
                            table.Cell().Element(CellStyle).Text(app.FirstName);
                            table.Cell().Element(CellStyle).Text(app.OtherNames);
                            table.Cell().Element(CellStyle).Text(app.Mobile);
                            table.Cell().Element(CellStyle).Text(app.PaymentPhoneNumber);
                            table.Cell().Element(CellStyle).Text(app.PrincipalAmount.ToString("N2"));
                            table.Cell().Element(CellStyle).Text(app.RemainingBalance.ToString("N2"));
                            table.Cell().Element(CellStyle).Text(app.InterestAmount.ToString("N2"));
                            table.Cell().Element(CellStyle).Text(app.FeeApplied.ToString("N2"));
                            table.Cell().Element(CellStyle).Text(app.Status);
                            table.Cell().Element(CellStyle).Text(app.WitnessName);
                            table.Cell().Element(CellStyle).Text(app.WitnessPhoneNumber);
                            table.Cell().Element(CellStyle).Text(app.WitnessNationalId);
                            table.Cell().Element(CellStyle).Text(app.ProjectName);
                            table.Cell().Element(CellStyle).Text(app.InterestRate.ToString("N2"));
                            table.Cell().Element(CellStyle).Text(app.InterestType);
                            table.Cell().Element(CellStyle).Text(app.Tenure.ToString());
                            table.Cell().Element(CellStyle).Text(app.CountryName);
                            table.Cell().Element(CellStyle).Text(app.CooperativeName);
                            table.Cell().Element(CellStyle).Text(app.CreatedOn.ToString("yyyy-MM-dd"));

                        }
                    });
                });

                // Footer
                page.Footer().AlignCenter().Column(col =>
                {
                    col.Item().Text(txt =>
                    {
                        txt.Span("Generated on: ").SemiBold();
                        txt.Span(TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,
                            TimeZoneInfo.FindSystemTimeZoneById("E. Africa Standard Time"))
                            .ToString("yyyy-MM-dd HH:mm") + " EAT");
                    });

                    col.Item().Text(txt =>
                    {
                        txt.Span("Generated by: ").SemiBold();
                        txt.Span(loanApplications[0].LoanNumber); // <-- replace with CurrentUserName if available
                    });
                });
            });
        });

        return document.GeneratePdf();

        // Styles
        static IContainer CellStyle(IContainer container) =>
            container.PaddingVertical(3).PaddingHorizontal(2).BorderBottom(1).BorderColor(Colors.Grey.Lighten1);

        static IContainer HeaderCellStyle(IContainer container) =>
            container.PaddingVertical(4).PaddingHorizontal(2);

        static IContainer FooterCellStyle(IContainer container) =>
            container.Background(Colors.Grey.Lighten3).PaddingVertical(4).PaddingHorizontal(2);
    }



    #endregion

    #region CountryApplicationReport

    public async Task<IEnumerable<LoanApplicationResponseModel>> CountryLoanApplicationReports(PortolioSearchParams portolioSearchParams)
    {
        var projects = await _projectRepository.GetAllAsync(c => c.CountryId == portolioSearchParams.CountryId && !c.IsDeleted);
        var projectIds = projects.Select(p => p.Id).ToList();
        Guid? statusId = null;
        if (!string.IsNullOrEmpty(portolioSearchParams.StatusId))
        {
            statusId = Guid.Parse(portolioSearchParams.StatusId);
        }

        var allApplication = await _loanApplicationRepository.GetAllAsync(x =>
            !x.IsDeleted &&
            (statusId == null || x.Status == statusId.Value) &&
            (string.IsNullOrEmpty(portolioSearchParams.OfficerId) || x.OfficerId == Guid.Parse(portolioSearchParams.OfficerId))
        );


        var _loanBatches = await _loanBatchRepository.GetAllAsync(x => projectIds.Contains(x.ProjectId) );
        var loanBatchIds = _loanBatches.Select(lb => lb.Id).ToList();





        var _loanApplications = allApplication.Where(x => loanBatchIds.Contains(x.LoanBatchId));





        var loanApplications = _mapper.Map<IEnumerable<LoanApplicationResponseModel>>(_loanApplications);
        var currentUser = await _userManager.FindByIdAsync(portolioSearchParams.CurrentUser.ToString());
        foreach (var loanApp in loanApplications)
        {
            loanApp.CurrentUserName = currentUser.UserName;
        }
        var farmerList = await _farmerRepo.GetAllAsync(c => c.Id == c.Id);
        var applicationStatusList = await _statusRepository.GetAllAsync(c => true);




        foreach (var loanApplication in loanApplications)
        {
            // farmer
            var farmers = farmerList.ToList().Where(c => c.Id == loanApplication.FarmerId);
            if (farmers != null && farmers.Any())
            {
                loanApplication.Farmer = _mapper.Map<FarmerResponseModel>(farmers.FirstOrDefault());
            }
            var latestStatements = (await _loanStatementRepository.GetAllAsync(
     x => x.ApplicationId == loanApplication.Id && x.StatementDate <= portolioSearchParams.EndDate && !x.IsDeleted) )
     .OrderByDescending(x => x.StatementDate);
            decimal accuredInterest = 0;
            foreach (var statement in latestStatements)
            {
                accuredInterest += statement.DebitAmount;

            }


            var latest = latestStatements.FirstOrDefault();

            loanApplication.PrincipalDue = latest?.AccuredPrincipalPayment ?? 0;
            loanApplication.PrincipalReceived = latest?.PrincipalPaid ?? 0;
            loanApplication.TotalInterest = loanApplication.InterestAmount;
            loanApplication.InterestDue = latest?.AccuredInterest ?? 0;
            loanApplication.InterestReceived = latest?.InterestPaid ?? 0;
            loanApplication.InterestArrears = latest?.AccuredInterest - latest?.InterestPaid ?? 0;
            loanApplication.PrincipalArrears = (latest?.AccuredPrincipalPayment ?? 0) - (latest?.PrincipalPaid ?? 0);
            loanApplication.TotalArrears = (latest?.AccuredInterest - latest?.InterestPaid) + (latest?.AccuredPrincipalPayment - latest?.PrincipalPaid) ?? 0;
            loanApplication.TotalExpected = latest?.AccuredInterest + latest?.AccuredPrincipalPayment ?? 0;
            loanApplication.ArrearsRate = ((loanApplication?.TotalArrears ?? 0) / (loanApplication?.TotalExpected == null || loanApplication.TotalExpected == 0 ? 1 : loanApplication.TotalExpected));
            //loan batches
            var loanBatches = _loanBatches.ToList().Where(c => c.Id == loanApplication.LoanBatchId);
            if (loanBatches != null && loanBatches.Any())
            {
                loanApplication.LoanBatch = _mapper.Map<LoanBatchResponseModel>(loanBatches.FirstOrDefault());
            }
            else
            {
                

                // With this corrected line:
                loanApplications = loanApplications.Where(app => app != loanApplication).ToList();
                continue;
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
            var disbursedStatusId = Guid.Parse("e24d24a8-fc69-4527-a92a-97f6648a43c5");


            var disbursedLog = (await _applicationStatusLogRepository.GetAllAsync(c =>
                c.ApplicationId == loanApplication.Id && c.StatusId == disbursedStatusId))
                .OrderByDescending(x => x.CreatedOn)
                .FirstOrDefault();

            if (disbursedLog != null)
            {
                loanApplication.DisbursedOn = disbursedLog.CreatedOn;
            }

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


    public async Task<byte[]> GenerateCountryLoanPortfolioPdfReport(PortolioSearchParams searchParams)
    {
        var loanApplications = (await CountryLoanApplicationReports(searchParams)).ToList();
        var country = await _countryRepository.GetFirstAsync(c => c.Id == searchParams.CountryId);
        if (!loanApplications.Any())
            throw new Exception("No loan applications found for the provided filters.");

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4.Landscape());
                page.Margin(5, Unit.Millimetre);
                page.DefaultTextStyle(x => x.FontSize(7).FontFamily("Arial"));

                // Header
                page.Header().PaddingVertical(6).Row(row =>
                {
                    row.ConstantItem(100).Image("wwwroot/logos/logo.png", ImageScaling.FitWidth);
                    var address = CountryAddress.GetAddressByCountryId((Guid)searchParams.CountryId);
                    row.RelativeItem().Column(col =>
                    {
                        col.Item().AlignRight().Text("Solidaridad").Bold().FontSize(12);
                        col.Item().AlignRight().Text(address.Line1);
                        col.Item().AlignRight().Text(address.Line2);
                        col.Item().AlignRight().Text(address.Line3);
                        col.Item().AlignRight().Text(address.Line4);
                        col.Item().AlignRight().Text(address.Phone);
                        col.Item().AlignRight().Text(address.Email);
                    });
                });

                page.Content().Column(col =>
                {
                    col.Spacing(20);
                    var eatNow = TimeZoneInfo.ConvertTimeFromUtc(searchParams.EndDate,
                        TimeZoneInfo.FindSystemTimeZoneById("E. Africa Standard Time"));

                    col.Item().AlignCenter().Text($"{country.CountryName} Loan Portfolio Report ").Bold().FontSize(14);

                    // Net totals accumulators
                    decimal netDisbursedTotal = 0,
                            netPrincipalDue = 0,
                            netPrincipalReceived = 0,
                            netPrincipalArrears = 0,
                            netRemainingBalance = 0,
                            netTotalInterest = 0,
                            netInterestDue = 0,
                            netInterestReceived = 0,
                            netInterestArrears = 0,
                            netTotalArrears = 0,
                            netTotalExpected = 0;

                    var batches = loanApplications.GroupBy(x => x.LoanBatchId).Select(g => new
                    {
                        Batch = g.First().LoanBatch,
                        Applications = g.ToList()
                    });

                    // Create ONE table for all data
                    col.Item().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            for (int i = 0; i < 15; i++) columns.RelativeColumn();
                        });

                        void HeaderCell(string text) =>
                            table.Cell().Element(HeaderCellStyle).Text(text).Bold();

                        void TotalCell(string text) =>
                            table.Cell().Element(FooterCellStyle).Text(text).Bold();

                        void DataCell(string text) =>
                            table.Cell().Element(CellStyle).Text(text);

                        // Headers
                        string[] headers = new[]
                        {
                        "Loan Product Name",
                        "Interest Rate(PM)",
                        "Loan Term",
                        "Amount Disbursed",
                        "Per schedule principal due to date",
                        "Principal received to date",
                        "Principal Balance",
                        "Principal in Arrears",
                        "Total Interest",
                        "Per schedule Interest due to date",
                        "Interest Received to Date",
                        "Interest Arrears",
                        "Total Arrears",
                        "Total Expected",
                        "Arrears Rate [%]"
                    };

                        foreach (var header in headers)
                            HeaderCell(header);

                        // Batch rows
                        foreach (var batchGroup in batches)
                        {
                            var batch = batchGroup.Batch;
                            var apps = batchGroup.Applications;

                            decimal disbursedTotal = 0,
                                    principalDueTotal = 0,
                                    principalReceivedTotal = 0,
                                    principalArrearsTotal = 0,
                                    remainingBalanceTotal = 0,
                                    totalInterest = 0,
                                    interestDueTotal = 0,
                                    interestReceivedTotal = 0,
                                    interestArrearsTotal = 0,
                                    totalArrears = 0,
                                    totalExpected = 0;

                            foreach (var app in apps)
                            {
                                var remainingBalance = app.PrincipalDue - app.PrincipalReceived;

                                disbursedTotal += app.PrincipalAmount;
                                principalDueTotal += app.PrincipalDue;
                                principalReceivedTotal += app.PrincipalReceived;
                                principalArrearsTotal += app.PrincipalArrears;
                                remainingBalanceTotal += remainingBalance;
                                totalInterest += app.TotalInterest;
                                interestDueTotal += app.InterestDue;
                                interestReceivedTotal += app.InterestReceived;
                                interestArrearsTotal += app.InterestArrears;
                                totalArrears += app.TotalArrears;
                                totalExpected += app.TotalExpected;
                            }

                            decimal totalArrearsRate = (totalExpected == 0 ? 0 : (totalArrears / totalExpected));

                            DataCell(batch.Name);
                            DataCell(batch.InterestRate.ToString("N2"));
                            DataCell(batch.Tenure.ToString("N0"));
                            DataCell(disbursedTotal.ToString("N2"));
                            DataCell(principalDueTotal.ToString("N2"));
                            DataCell(principalReceivedTotal.ToString("N2"));
                            DataCell(remainingBalanceTotal.ToString("N2"));
                            DataCell(principalArrearsTotal.ToString("N2"));
                            DataCell(totalInterest.ToString("N2"));
                            DataCell(interestDueTotal.ToString("N2"));
                            DataCell(interestReceivedTotal.ToString("N2"));
                            DataCell(interestArrearsTotal.ToString("N2"));
                            DataCell(totalArrears.ToString("N2"));
                            DataCell(totalExpected.ToString("N2"));
                            DataCell((totalArrearsRate * 100).ToString("N2") + "%");

                            // accumulate into net totals
                            netDisbursedTotal += disbursedTotal;
                            netPrincipalDue += principalDueTotal;
                            netPrincipalReceived += principalReceivedTotal;
                            netPrincipalArrears += principalArrearsTotal;
                            netRemainingBalance += remainingBalanceTotal;
                            netTotalInterest += totalInterest;
                            netInterestDue += interestDueTotal;
                            netInterestReceived += interestReceivedTotal;
                            netInterestArrears += interestArrearsTotal;
                            netTotalArrears += totalArrears;
                            netTotalExpected += totalExpected;
                        }

                        // Net Totals Row
                        decimal netArrearsRate = (netTotalExpected == 0 ? 0 : (netTotalArrears / netTotalExpected));

                        TotalCell("NET TOTAL");
                        TotalCell(""); // empty cells to align
                        TotalCell("");
                        TotalCell(netDisbursedTotal.ToString("N2"));
                        TotalCell(netPrincipalDue.ToString("N2"));
                        TotalCell(netPrincipalReceived.ToString("N2"));
                        TotalCell(netRemainingBalance.ToString("N2"));
                        TotalCell(netPrincipalArrears.ToString("N2"));
                        TotalCell(netTotalInterest.ToString("N2"));
                        TotalCell(netInterestDue.ToString("N2"));
                        TotalCell(netInterestReceived.ToString("N2"));
                        TotalCell(netInterestArrears.ToString("N2"));
                        TotalCell(netTotalArrears.ToString("N2"));
                        TotalCell(netTotalExpected.ToString("N2"));
                        TotalCell((netArrearsRate * 100).ToString("N2") + "%");
                    });
                });

                // Footer
                page.Footer().AlignCenter().Column(col =>
                {
                    col.Item().Text(txt =>
                    {
                        txt.Span("Generated on: ").SemiBold();
                        txt.Span(TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,
                            TimeZoneInfo.FindSystemTimeZoneById("E. Africa Standard Time"))
                            .ToString("yyyy-MM-dd HH:mm") + " EAT");
                    });

                    col.Item().Text(txt =>
                    {
                        txt.Span("Generated by: ").SemiBold();
                        txt.Span(loanApplications[0].CurrentUserName);
                    });
                });
            });
        });

        return document.GeneratePdf();

        static IContainer CellStyle(IContainer container) =>
            container.PaddingVertical(3).PaddingHorizontal(2).BorderBottom(1).BorderColor(Colors.Grey.Lighten1);

        static IContainer HeaderCellStyle(IContainer container) =>
            container.PaddingVertical(4).PaddingHorizontal(2);

        static IContainer FooterCellStyle(IContainer container) =>
            container.Background(Colors.Grey.Lighten3).PaddingVertical(4).PaddingHorizontal(2);
    }

    #endregion

    #region GlobalPortfolioReport

   public async Task<IEnumerable<GlobalLoanPortfolioReportResponseModel>> GlobalLoanPortfolioReports(PortolioSearchParams portolioSearchParams)
{
    var countries = await _countryRepository.GetAllAsync(c => true);
    var projects = await _projectRepository.GetAllAsync(c => !c.IsDeleted);
        Guid? statusId = null;
        if (!string.IsNullOrEmpty(portolioSearchParams.StatusId))
        {
            statusId = Guid.Parse(portolioSearchParams.StatusId);
        }

        var allApplication = await _loanApplicationRepository.GetAllAsync(x =>
            !x.IsDeleted &&
            (statusId == null || x.Status == statusId.Value) &&
            (string.IsNullOrEmpty(portolioSearchParams.OfficerId) || x.OfficerId == Guid.Parse(portolioSearchParams.OfficerId))
        );
        if(allApplication == null)
        {
            return new List<GlobalLoanPortfolioReportResponseModel>();
        }
        
        var results = new List<GlobalLoanPortfolioReportResponseModel>();
        var currentUser = await _userManager.FindByIdAsync(portolioSearchParams.CurrentUser.ToString());
        
        foreach (var country in countries)
    {
        var projectIds = projects.Where(c => c.CountryId == country.Id)
                                 .Select(p => p.Id)
                                 .ToList();

       
        var loanBatches = await _loanBatchRepository.GetAllAsync(x =>
            projectIds.Contains(x.ProjectId) &&
            x.StatusId != 1 
            );

        var loanBatchIds = loanBatches.Select(lb => lb.Id).ToList();

        var loanApplications = allApplication.Where(x => loanBatchIds.Contains(x.LoanBatchId));


        // Collect totals
        decimal amountDisbursed = 0;
        decimal principalDue = 0;
        decimal principalReceived = 0;
        decimal principalArrears = 0;
        decimal interestDue = 0;
        decimal interestReceived = 0;
        decimal interestArrears = 0;
        decimal totalExpected = 0;
        decimal totalArrears = 0;

        foreach (var loanApp in loanApplications)
        {
            var latestStatements = (await _loanStatementRepository.GetAllAsync(
                x => x.ApplicationId == loanApp.Id &&
                     x.StatementDate <= portolioSearchParams.EndDate && !x.IsDeleted)).OrderByDescending(x => x.StatementDate);


                var latest = latestStatements.FirstOrDefault();

            if (latest != null)
            {
                principalDue      += latest.AccuredPrincipalPayment ;
                principalReceived += latest.PrincipalPaid ;
                interestDue       += latest.AccuredInterest ;
                interestReceived  += latest.InterestPaid ;
                interestArrears   += (latest.AccuredInterest ) - (latest.InterestPaid );
                principalArrears  += (latest.AccuredPrincipalPayment ) - (latest.PrincipalPaid);
                totalArrears      += ((latest.AccuredInterest ) - (latest.InterestPaid )) +
                                     ((latest.AccuredPrincipalPayment ) - (latest.PrincipalPaid ));
                totalExpected     += (latest.AccuredInterest ) + (latest.AccuredPrincipalPayment);
            
              
            }
                else
                {
                    principalDue += 0;
                    principalReceived += 0;
                    interestDue += 0;
                    interestReceived += 0;
                    interestArrears +=0;
                    principalArrears += 0;
                    totalArrears += 0;
                    totalExpected += 0;
                }
                    amountDisbursed += loanApp.PrincipalAmount;
            }

        // Add summary row for this country
        results.Add(new GlobalLoanPortfolioReportResponseModel
        {
            CountryName      = country.CountryName,
            AmountDisbursed  = amountDisbursed,
            PrincipalDue     = principalDue,
            PrincipalReceived= principalReceived,
            PrincipalBalance = principalDue - principalReceived,
            PrincipalArrears = principalArrears,
            TotalInterest    = interestDue, 
            InterestDue      = interestDue,
            InterestReceived = interestReceived,
            InterestArrears  = interestArrears,
            TotalArrears     = totalArrears,
            TotalExpected    = totalExpected,
            ArrearsRate      = totalExpected == 0 ? 0 : totalArrears / totalExpected,
            CurrentUserName = currentUser.UserName
        });
    }

    return results;
}

    public async Task<byte[]> GenerateGlobalLoanPortfolioPdfReport(PortolioSearchParams searchParams)
    {
        var summaries = (await GlobalLoanPortfolioReports(searchParams)).ToList();

        if (!summaries.Any())
            throw new Exception("No loan portfolio data found for the provided filters.");

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4.Landscape());
                page.Margin(5, Unit.Millimetre);
                page.DefaultTextStyle(x => x.FontSize(7).FontFamily("Arial"));

                // === HEADER ===
                page.Header().PaddingVertical(6).Row(row =>
                {
                    row.ConstantItem(100).Image("wwwroot/logos/logo.png", ImageScaling.FitWidth);
                    var address = CountryAddress.GetAddressByCountryId((Guid)searchParams.CountryId);
                    row.RelativeItem().Column(col =>
                    {
                        col.Item().AlignRight().Text("Solidaridad").Bold().FontSize(12);
                        col.Item().AlignRight().Text(address.Line1);
                        col.Item().AlignRight().Text(address.Line2);
                        col.Item().AlignRight().Text(address.Line3);
                        col.Item().AlignRight().Text(address.Line4);
                        col.Item().AlignRight().Text(address.Phone);
                        col.Item().AlignRight().Text(address.Email);
                    });
                });

                // === CONTENT ===
                page.Content().Column(col =>
                {
                    col.Spacing(20);
                    var eatNow = TimeZoneInfo.ConvertTimeFromUtc(
                        searchParams.EndDate,
                        TimeZoneInfo.FindSystemTimeZoneById("E. Africa Standard Time")
                    );

                    col.Item().AlignCenter().Text($"Global Loan Portfolio Report").Bold().FontSize(14);
                    col.Item().AlignCenter().Text($"As of {eatNow:yyyy-MM-dd HH:mm} EAT").FontSize(10);

                    // NET TOTAL accumulators
                    decimal netDisbursed = 0,
                            netPrincipalDue = 0,
                            netPrincipalReceived = 0,
                            netPrincipalBalance = 0,
                            netPrincipalArrears = 0,
                            netTotalInterest = 0,
                            netInterestDue = 0,
                            netInterestReceived = 0,
                            netInterestArrears = 0,
                            netTotalArrears = 0,
                            netTotalExpected = 0;

                    // === TABLE ===
                    col.Item().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            for (int i = 0; i < 13; i++) columns.RelativeColumn();
                        });

                        string[] headers = new[]
                        {
                        "Country Name","Amount Disbursed","Principal Due","Principal Received",
                        "Principal Balance","Principal Arrears","Total Interest","Interest Due",
                        "Interest Received","Interest Arrears","Total Arrears","Total Expected","Arrears Rate [%]"
                    };

                        foreach (var header in headers)
                            table.Cell().Element(HeaderCellStyle).Text(header).Bold();

                        foreach (var summary in summaries)
                        {
                            table.Cell().Element(CellStyle).Text(summary.CountryName);
                            table.Cell().Element(CellStyle).Text(summary.AmountDisbursed.ToString("N2"));
                            table.Cell().Element(CellStyle).Text(summary.PrincipalDue.ToString("N2"));
                            table.Cell().Element(CellStyle).Text(summary.PrincipalReceived.ToString("N2"));
                            table.Cell().Element(CellStyle).Text(summary.PrincipalBalance.ToString("N2"));
                            table.Cell().Element(CellStyle).Text(summary.PrincipalArrears.ToString("N2"));
                            table.Cell().Element(CellStyle).Text(summary.TotalInterest.ToString("N2"));
                            table.Cell().Element(CellStyle).Text(summary.InterestDue.ToString("N2"));
                            table.Cell().Element(CellStyle).Text(summary.InterestReceived.ToString("N2"));
                            table.Cell().Element(CellStyle).Text(summary.InterestArrears.ToString("N2"));
                            table.Cell().Element(CellStyle).Text(summary.TotalArrears.ToString("N2"));
                            table.Cell().Element(CellStyle).Text(summary.TotalExpected.ToString("N2"));
                            table.Cell().Element(CellStyle).Text((summary.ArrearsRate * 100).ToString("N2"));

                            // add to NET totals
                            netDisbursed += summary.AmountDisbursed;
                            netPrincipalDue += summary.PrincipalDue;
                            netPrincipalReceived += summary.PrincipalReceived;
                            netPrincipalBalance += summary.PrincipalBalance;
                            netPrincipalArrears += summary.PrincipalArrears;
                            netTotalInterest += summary.TotalInterest;
                            netInterestDue += summary.InterestDue;
                            netInterestReceived += summary.InterestReceived;
                            netInterestArrears += summary.InterestArrears;
                            netTotalArrears += summary.TotalArrears;
                            netTotalExpected += summary.TotalExpected;
                        }

                        // NET TOTAL ROW
                        decimal netArrearsRate = netTotalExpected == 0 ? 0 : netTotalArrears / netTotalExpected;

                        void TotalCell(string text) => table.Cell().Element(FooterCellStyle).Text(text).Bold();

                        TotalCell("NET TOTAL");
                        TotalCell(netDisbursed.ToString("N2"));
                        TotalCell(netPrincipalDue.ToString("N2"));
                        TotalCell(netPrincipalReceived.ToString("N2"));
                        TotalCell(netPrincipalBalance.ToString("N2"));
                        TotalCell(netPrincipalArrears.ToString("N2"));
                        TotalCell(netTotalInterest.ToString("N2"));
                        TotalCell(netInterestDue.ToString("N2"));
                        TotalCell(netInterestReceived.ToString("N2"));
                        TotalCell(netInterestArrears.ToString("N2"));
                        TotalCell(netTotalArrears.ToString("N2"));
                        TotalCell(netTotalExpected.ToString("N2"));
                        TotalCell((netArrearsRate * 100).ToString("N2"));
                    });
                });

                // === FOOTER ===
                page.Footer().AlignCenter().Column(col =>
                {
                    col.Item().Text(txt =>
                    {
                        txt.Span("Generated on: ").SemiBold();
                        txt.Span(TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,
                            TimeZoneInfo.FindSystemTimeZoneById("E. Africa Standard Time"))
                            .ToString("yyyy-MM-dd HH:mm") + " EAT");
                    });

                    col.Item().Text(txt =>
                    {
                        txt.Span("Generated by: ").SemiBold();
                        txt.Span(summaries[0].CurrentUserName);
                    });
                });
            });
        });

        return document.GeneratePdf();

        // === STYLES ===
        static IContainer CellStyle(IContainer container) =>
            container.PaddingVertical(3).PaddingHorizontal(2).BorderBottom(1).BorderColor(Colors.Grey.Lighten1);

        static IContainer HeaderCellStyle(IContainer container) =>
            container.PaddingVertical(4).PaddingHorizontal(2);

        static IContainer FooterCellStyle(IContainer container) =>
            container.Background(Colors.Grey.Lighten3).PaddingVertical(4).PaddingHorizontal(2);
    }


  



    #endregion

    #endregion

    #region Disbursed Loan Reports
    public async Task<IEnumerable<DisbursedLoanReportResponseModel>> DisbursedLoanReports(DisbursedSearchParams disbursedSearchParams)
    {
        var projects = await _projectRepository.GetAllAsync(c => c.CountryId == disbursedSearchParams.CountryId && !c.IsDeleted);
        var projectIds = projects.Select(p => p.Id).ToList();

        var batchIds = disbursedSearchParams.BatchIds;
        var _loanBatches = await _loanBatchRepository.GetAllAsync(x => projectIds.Contains(x.ProjectId) && x.StatusId != 1 && batchIds.Contains(x.Id));
        var loanBatchIds = _loanBatches.Select(lb => lb.Id).ToList();

        var _loanApplications = await _loanApplicationRepository.GetAllAsync(x =>
            loanBatchIds.Contains(x.LoanBatchId) &&
            !x.IsDeleted &&
            x.Status == new Guid("e24d24a8-fc69-4527-a92a-97f6648a43c5")
        );

        var farmerList = await _farmerRepo.GetAllAsync(c => c.Id == c.Id);
        var applicationStatusList = await _statusRepository.GetAllAsync(c => true);

        var disbursedReports = new List<DisbursedLoanReportResponseModel>();
        var currentUser = await _userManager.FindByIdAsync(disbursedSearchParams.CurrentUser.ToString());

        foreach (var loanApplication in _loanApplications)
        {
            var farmer = farmerList.FirstOrDefault(f => f.Id == loanApplication.FarmerId);
            var loanBatch = _loanBatches.FirstOrDefault(lb => lb.Id == loanApplication.LoanBatchId);

            var latestStatements = (await _loanStatementRepository.GetAllAsync(
                x => x.ApplicationId == loanApplication.Id && x.StatementDate <= disbursedSearchParams.EndDate))
                .OrderByDescending(x => x.StatementDate)
                .ToList();

            var latest = latestStatements.FirstOrDefault();

            decimal? interestDue = latest?.AccuredInterest ?? 0;
            decimal? feesApplied = loanApplication.FeeApplied ;

            var disbursedLog = (await _applicationStatusLogRepository.GetAllAsync(c =>
                c.ApplicationId == loanApplication.Id &&
                c.StatusId == new Guid("e24d24a8-fc69-4527-a92a-97f6648a43c5")
                && c.CreatedOn >= disbursedSearchParams.StartDate && c.CreatedOn <= disbursedSearchParams.EndDate
                )
                )
                .OrderByDescending(x => x.CreatedOn)
                .FirstOrDefault();

            var disbursedReport = new DisbursedLoanReportResponseModel
            {
                LoanNumber = loanApplication.LoanNumber,
                DisbursementDate = (DateTime)(disbursedLog?.CreatedOn),
                MaturityDate = (DateTime)(disbursedLog?.CreatedOn.AddMonths((int)(loanBatch.GracePeriod + loanBatch.Tenure))),
                Interest = loanBatch.InterestRate,
                LoanTerm = (int)(loanBatch?.Tenure),
                FarmerSystemId = farmer?.SystemId,
                FarmerName = $"{farmer?.FirstName} {farmer?.OtherNames}",
                PrincipalAmount = loanApplication.PrincipalAmount,
                FeesApplied = (decimal)feesApplied,
                EffectivePrincipal = (decimal)(loanApplication.PrincipalAmount - feesApplied),
                ExpectedInterestPerSchedule = (decimal)interestDue,
                BatchId = loanBatch.Id,
                CurrentUserName=currentUser.UserName
            };

            disbursedReports.Add(disbursedReport);
        }

        return disbursedReports;
    }




    public async Task<byte[]> GenerateLoanDisbursementPdfReport(DisbursedSearchParams searchParams)
    {
        var loanApplications = (await DisbursedLoanReports(searchParams)).ToList();

        if (!loanApplications.Any())
            throw new Exception("No loan applications found for the provided filters.");

        var loanBatchIds = loanApplications.Select(x => x.BatchId).Distinct().ToList();
        var loanBatches = await _loanBatchRepository.GetAllAsync(x => loanBatchIds.Contains(x.Id));

        var startDate = TimeZoneInfo.ConvertTimeFromUtc(searchParams.StartDate, TimeZoneInfo.FindSystemTimeZoneById("E. Africa Standard Time"));
        var endDate = TimeZoneInfo.ConvertTimeFromUtc(searchParams.EndDate, TimeZoneInfo.FindSystemTimeZoneById("E. Africa Standard Time"));

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4.Landscape());
                page.Margin(5, Unit.Millimetre);
                page.DefaultTextStyle(x => x.FontSize(7).FontFamily("Arial"));

                page.Header().Row(row =>
                {
                    row.ConstantItem(100).Image("wwwroot/logos/logo.png", ImageScaling.FitWidth);
                    var address = CountryAddress.GetAddressByCountryId((Guid)searchParams.CountryId);
                    row.RelativeItem().Column(col =>
                    {
                        col.Item().AlignRight().Text("Solidaridad").Bold().FontSize(12);
                        col.Item().AlignRight().Text(address.Line1);
                        col.Item().AlignRight().Text(address.Line2);
                        col.Item().AlignRight().Text(address.Line3);
                        col.Item().AlignRight().Text(address.Line4);
                        col.Item().AlignRight().Text(address.Phone);
                        col.Item().AlignRight().Text(address.Email);


                        //col.Item().AlignRight().Text("Solidaridad").Bold().FontSize(12);
                        //col.Item().AlignRight().Text("Kilimani Business Centre, Kirichwa Road");
                        //col.Item().AlignRight().Text("P.O. Box 42234 – 00100");
                        //col.Item().AlignRight().Text("Nairobi, Kenya");
                        //col.Item().AlignRight().Text("Phone: +254 (0) 716 666 862");
                        //col.Item().AlignRight().Text("Email: info.secaec@solidaridadnetwork.org");
                    });
                });

                page.Content().Column(col =>
                {
                    col.Spacing(20);

                    col.Item().AlignCenter().Text("Loan Disbursement Report").Bold().FontSize(14);
                    col.Item().AlignCenter().Text($"From {startDate:yyyy-MM-dd HH:mm} EAT to {endDate:yyyy-MM-dd HH:mm} EAT").FontSize(10);

                    var batches = loanApplications.GroupBy(x => x.BatchId).Select(g => new
                    {
                        Batch = loanBatches.FirstOrDefault(b => b.Id == g.Key),
                        Applications = g.ToList()
                    });

                    decimal grandPrincipalAmount = 0, grandFeesApplied = 0, grandEffectivePrincipal = 0, netInterest = 0;

                    foreach (var batchGroup in batches)
                    {
                        var batch = batchGroup.Batch;
                        var apps = batchGroup.Applications;

                        col.Item().PaddingVertical(4).Background(Colors.Grey.Lighten3).Padding(5)
                            .Text($"Loan Product: {batch?.Name ?? "N/A"}").Bold().FontSize(11).FontColor(Colors.Black);

                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                for (int i = 0; i < 10; i++) columns.RelativeColumn();
                            });

                            string[] headers = new[]
                            {
                            "Loan Account", "Disbursement Date", "Maturity Date", "Interest (P.A.)", "Loan Term",
                            "Farmer System ID", "Farmer Name", "Principle Amount", "Fees Applied", "Effective Principle"
                        };

                            foreach (var header in headers)
                                table.Cell().Element(HeaderCellStyle).Text(header).Bold();

                            decimal totalPrincipalAmount = 0, totalFeesApplied = 0, totalEffectivePrincipal = 0, totalInterest = 0;

                            foreach (var app in apps)
                            {
                                var maturityDate = app.MaturityDate;
                                var effectivePrincipal = app.EffectivePrincipal;

                                table.Cell().Element(CellStyle).Text(app.LoanNumber);
                                table.Cell().Element(CellStyle).Text(app.DisbursementDate.ToString("yyyy-MM-dd HH:mm") + " EAT");
                                table.Cell().Element(CellStyle).Text(maturityDate.ToString("yyyy-MM-dd"));
                                table.Cell().Element(CellStyle).Text($"{batch.InterestRate:N2}%");
                                table.Cell().Element(CellStyle).Text(app.LoanTerm.ToString());

                                table.Cell().Element(CellStyle).Text(app.FarmerSystemId ?? "-");
                                table.Cell().Element(CellStyle).Text(app.FarmerName ?? "");

                                table.Cell().Element(CellStyle).Text(app.PrincipalAmount.ToString("N2"));
                                table.Cell().Element(CellStyle).Text(app.FeesApplied.ToString("N2") ?? "0.00");
                                table.Cell().Element(CellStyle).Text(effectivePrincipal.ToString("N2"));

                                totalPrincipalAmount += app.PrincipalAmount;
                                totalFeesApplied += app.FeesApplied;
                                totalEffectivePrincipal += effectivePrincipal;
                                //totalInterest += app.Interest;
                            }

                            // Totals per batch
                            table.Cell().ColumnSpan(1).Element(FooterCellStyle).Text("TOTAL:").Bold();
                            table.Cell().ColumnSpan(6).Element(FooterCellStyle).AlignRight().Text("").Bold();
                            table.Cell().Element(FooterCellStyle).Text(totalPrincipalAmount.ToString("N2")).Bold();
                            table.Cell().Element(FooterCellStyle).Text(totalFeesApplied.ToString("N2")).Bold();
                            table.Cell().Element(FooterCellStyle).Text(totalEffectivePrincipal.ToString("N2")).Bold();

                            // Accumulate grand totals
                            grandPrincipalAmount += totalPrincipalAmount;
                            grandFeesApplied += totalFeesApplied;
                            grandEffectivePrincipal += totalEffectivePrincipal;
                            //netInterest += totalInterest;
                        });

                        col.Item().PaddingVertical(10).LineHorizontal(1).LineColor(Colors.Grey.Lighten2);
                    }

                    // Grand totals
                    col.Item().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            for (int i = 0; i < 10; i++) columns.RelativeColumn();
                        });

                        table.Cell().ColumnSpan(1).Element(FooterCellStyle).Text("NET TOTAL:").Bold();
                        table.Cell().ColumnSpan(6).Element(FooterCellStyle).AlignRight().Text("").Bold();
                        table.Cell().Element(FooterCellStyle).Text(grandPrincipalAmount.ToString("N2")).Bold();
                        table.Cell().Element(FooterCellStyle).Text(grandFeesApplied.ToString("N2")).Bold();
                        table.Cell().Element(FooterCellStyle).Text(grandEffectivePrincipal.ToString("N2")).Bold();
                    });
                });

                page.Footer().AlignCenter().Column(col =>
                {
                    col.Item().Text(txt =>
                    {
                        txt.Span("Generated on: ").SemiBold();
                        txt.Span(TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,
                            TimeZoneInfo.FindSystemTimeZoneById("E. Africa Standard Time"))
                            .ToString("yyyy-MM-dd HH:mm") + " EAT");
                    });

                    col.Item().Text(txt =>
                    {
                        txt.Span("Generated by: ").SemiBold();
                        txt.Span(loanApplications[0].CurrentUserName);
                    });
                });
            });
        });

        return document.GeneratePdf();

        static IContainer CellStyle(IContainer container) =>
            container.PaddingVertical(3).PaddingHorizontal(2).BorderBottom(1).BorderColor(Colors.Grey.Lighten1);

        static IContainer HeaderCellStyle(IContainer container) =>
            container.PaddingVertical(4).PaddingHorizontal(2);

        static IContainer FooterCellStyle(IContainer container) =>
            container.Background(Colors.Grey.Lighten3).PaddingVertical(4).PaddingHorizontal(2);
    }



    #endregion

    #region Payment Reports

    public async Task<IEnumerable<PaymentReportResponseModel>> PaymentReports(PaymentReportSearchParams disbursedSearchParams)
    {
        var completedStatusGuid = new Guid("271d9c1a-2c4f-4ee2-ad0f-d7dc36bd255f");
        var users = await _userManager.Users.ToListAsync();
        var paymentBatches = await _paymentBatchRepository.GetAllAsync(c =>
            c.CountryId == disbursedSearchParams.CountryId &&
            !c.IsDeleted &&
            disbursedSearchParams.PaymentBatchIds.Contains(c.Id) &&
            c.CreatedOn >= disbursedSearchParams.StartDate
        );
       
        var masterApprovalStages = await _paymentBatchRepository.GetPaymentApprovalStages();

        if (paymentBatches == null || !paymentBatches.Any())
            return Enumerable.Empty<PaymentReportResponseModel>();

        var farmerList = await _farmerRepo.GetAllAsync(f => !f.IsDeleted);
        var reports = new List<PaymentReportResponseModel>();
        var paymentStatus = await _paymentDeductibleStatusMasterRepository.GetAllAsync(c =>  !c.IsDeleted);
        int index = 1;
        var currentUser = await _userManager.FindByIdAsync(disbursedSearchParams.CurrentUser.ToString());
        foreach (var batch in paymentBatches)
        {
            var batchStatusHistory = await _paymentBatchRepository.GetPaymentHistory(batch.Id);
            List<StatusChangeHistory> statusChangeHistories = new List<StatusChangeHistory>();
            foreach (var history in batchStatusHistory)
            {
                var statusName = masterApprovalStages.FirstOrDefault(x => x.Id == history.StageId).StageText;
                var responsibleUser = users.FirstOrDefault(x => new Guid(x.Id) == history.CreatedBy).UserName;
                if (statusName != null && responsibleUser != null)
                {
                    statusChangeHistories.Add(new StatusChangeHistory
                    {
                        Status = statusName,
                        UpdatedByUserName = responsibleUser,
                        UpdatedAt = history.CreatedOn
                    });
                }
            }
            var payments = await _paymentRequestDeductibleRepository.GetAllAsync(p => p.PaymentBatchId == batch.Id &&
           
            (
                disbursedSearchParams.Status == -1 || // No filter
                (disbursedSearchParams.Status == 1 && p.PaymentStatus == completedStatusGuid) || // Completed only
                (disbursedSearchParams.Status == 0 && p.PaymentStatus != completedStatusGuid) // Incomplete only
            ));
            if (payments == null || !payments.Any())
            {
               continue;
            }

            foreach (var payment in payments)
            {
                var farmer = farmerList.FirstOrDefault(f => f.SystemId == payment.SystemId && f.IsDeleted == false);
                if (farmer == null) continue;

                var report = new PaymentReportResponseModel
                {
                  
                    PaymentDate =  batch.CreatedOn,
                    FarmerSystemId = farmer.SystemId,
                    FarmerName = $"{farmer.FirstName} {farmer.OtherNames}",
                    FarmerEarnings = payment.TotalUnitsEarningLc ,
                    FarmerPayableEarnings = payment.FarmerPayableEarningsLc ,
                    LoanDeduction = payment.FarmerLoansDeductionsLc ,
                    LoanOpeningBalance = payment.FarmerLoansBalanceLc ,
                    LoanClosingBalance = payment.FarmerLoansBalanceLc - payment.FarmerLoansDeductionsLc,
                    ProcessingMethod = completedStatusGuid == payment.PaymentStatus ? payment.IsManual == true? "Manually Processed" : "Processed via onafriq" :  "",
                    PaymentReference = batch.ReferenceNumber ?? "",
                    ReceivingMobileNo = farmer.PaymentPhoneNumber ?? "N/A",
                    Status = paymentStatus.FirstOrDefault(x => x.Id == payment.PaymentStatus)?.Name ?? "Unknown",
                    PaymentBatchId =  batch.Id,
                    CurrentUserName = currentUser.UserName,
                    BatchStatusHistory= statusChangeHistories,
                    Remarks=payment.Remarks
                };

                reports.Add(report);
            }
        }

        return reports;
    }

    public async Task<byte[]> GeneratePaymentsPdfReport(PaymentReportSearchParams searchParams)
    {
        var payments = (await PaymentReports(searchParams)).ToList();
        var paymentBatches = await _paymentBatchRepository.GetAllAsync(c =>
            c.CountryId == searchParams.CountryId &&
            !c.IsDeleted &&
            searchParams.PaymentBatchIds.Contains(c.Id) &&
            c.CreatedOn >= searchParams.StartDate &&
            c.CreatedOn <= searchParams.EndDate);
        var country = await _countryRepository.GetFirstAsync(c=> c.Id == searchParams.CountryId);
        var loanBatches = await _loanBatchRepository.GetAllAsync(c => c.CountryId == searchParams.CountryId);
        var projects = await _projectRepository.GetAllAsync(c => c.CountryId == searchParams.CountryId);

        var projectMapping = await _paymentBatchProjectMappingRepository.GetAllAsync(c => true);

        var loanBatchMapping = await _paymentBatchLoanBatchMappingRepository.GetAllAsync(c => true);
        var callBacks = await _callBackRepository.GetAllAsync(c => true);
        callBacks = callBacks.OrderByDescending(c => c.Created).ToList();


        if (!payments.Any())
            throw new Exception("No payments found for the provided filters.");

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4.Landscape());
                page.Margin(5, Unit.Millimetre);
                page.DefaultTextStyle(x => x.FontSize(7).FontFamily("Arial"));

                page.Header().PaddingVertical(4).Row(row =>
                {
                    row.ConstantItem(100).Element(container =>
                    {
                        var imageDescriptor = container.Image("wwwroot/logos/custom2.png");
                        imageDescriptor.FitWidth();
                    });
                    var address = CountryAddress.GetAddressByCountryId((Guid)searchParams.CountryId);
                    row.RelativeItem().Column(col =>
                    {
                        col.Item().AlignRight().Text("Solidaridad").Bold().FontSize(12);
                        col.Item().AlignRight().Text(address.Line1);
                        col.Item().AlignRight().Text(address.Line2);
                        col.Item().AlignRight().Text(address.Line3);
                        col.Item().AlignRight().Text(address.Line4);
                        col.Item().AlignRight().Text(address.Phone);
                        col.Item().AlignRight().Text(address.Email);

                   
                        //col.Item().AlignRight().Text("Solidaridad").Bold().FontSize(12);
                        //col.Item().AlignRight().Text("Kilimani Business Centre, Kirichwa Road");
                        //col.Item().AlignRight().Text("P.O. Box 42234 – 00100");
                        //col.Item().AlignRight().Text("Nairobi, Kenya");
                        //col.Item().AlignRight().Text("Phone: +254 (0) 716 666 862");
                        //col.Item().AlignRight().Text("Email: info.secaec@solidaridadnetwork.org");
                    });
                });

                page.Content().Column(async col =>
                {
                    col.Spacing(20);
                    var eatNow = TimeZoneInfo.ConvertTimeFromUtc(searchParams.EndDate, TimeZoneInfo.FindSystemTimeZoneById("E. Africa Standard Time"));
                    col.Item().AlignCenter().Text($"{country.CountryName} Payments Report").Bold().FontSize(14);
                    col.Item().AlignCenter().Text($"From {searchParams.StartDate:yyyy-MM-dd HH:mm} EAT to {searchParams.EndDate:yyyy-MM-dd HH:mm} EAT").FontSize(10);

                    var groupedBatches = payments.GroupBy(x => x.PaymentBatchId)
                        .Select(g => new
                        {
                            Batch = paymentBatches.FirstOrDefault(p => p.Id == g.Key),
                            Payments = g.ToList()
                        }).ToList();

                    decimal netLoanDeduction = 0, netOpening = 0, netClosing = 0, netFarmerEarnings = 0, netPayableEarnings = 0;

                    foreach (var group in groupedBatches)
                    {
                        var batch = group.Batch;
                        var apps = group.Payments;
                        batch.Country = country;

                        var projectIdMaps = projectMapping.Where(x => x.PaymentBatchId == batch.Id);
                        var loanBatchIdMaps = loanBatchMapping.Where(x => x.PaymentBatchId == batch.Id);

                        var projectNames = (from pm in projectIdMaps
                                            join p in projects on pm.ProjectId equals p.Id
                                            select p.ProjectName).ToList();

                        var projectNamesText = projectNames.Any() ? string.Join(", ", projectNames) : "N/A";

                        var loanBatchNames = (from lm in loanBatchIdMaps
                                              join lb in loanBatches on lm.LoanBatchId equals lb.Id
                                              select lb.Name).ToList();

                        var loanBatchNamesText = loanBatchNames.Any() ? string.Join(", ", loanBatchNames) : "N/A";
                        string callBackState = "N/A";
                        if (batch.ReferenceNumber != null && batch.ReferenceNumber != "N/A")
                        {
                            
                            callBackState = callBacks.FirstOrDefault(c => c.ReferenceId == batch.ReferenceNumber)?.State;
                           
                        }
                        var total = apps?.Count() ?? 0;
                        var completed = apps?.Count(a => a.Status == "Complete") ?? 0;
                        var percentage = total > 0 ? (decimal)(completed / total * 100) : 0;

                        // Row: label + progress bar


                        col.Item().Row(row =>
                        {
                            // LEFT COLUMN
                            row.RelativeItem().Column(innerCol =>
                            {
                                innerCol.Item().PaddingVertical(4).PaddingRight(5)
                                .Row(r =>
                                {
                                    r.AutoItem().Text("Payment Batch Name : ").FontSize(8).Bold().FontColor(Colors.Black);
                                    r.AutoItem().Text(batch?.BatchName ?? "N/A").FontSize(8).FontColor(Colors.Black);
                                });

                                innerCol.Item().PaddingVertical(4).PaddingRight(5)
                                .Row(r =>
                                {
                                    r.AutoItem().Text("Payment Type : ").FontSize(8).Bold().FontColor(Colors.Black);
                                    r.AutoItem().Text(batch?.PaymentModule == 3 ?"Deductible Payment" : "Facilitation Payment" ).FontSize(8).FontColor(Colors.Black);
                                });

                                innerCol.Item().PaddingVertical(4).PaddingRight(5)
                                .Row(r =>
                                {
                                    r.AutoItem().Text("Payment Batch ID : ").FontSize(8).Bold().FontColor(Colors.Black);
                                    r.AutoItem().Text(batch?.Id.ToString() ?? "N/A").FontSize(8).FontColor(Colors.Black);
                                });

                                innerCol.Item().PaddingVertical(4).PaddingRight(5)
                                .Row(r =>
                                {
                                    r.AutoItem().Text("Project(s) : ").FontSize(8).Bold().FontColor(Colors.Black);
                                    r.AutoItem().Text(projectNamesText  ?? "N/A").FontSize(8).FontColor(Colors.Black);
                                });

                                innerCol.Item().PaddingVertical(4).PaddingRight(5)
                                .Row(r =>
                                {
                                    r.AutoItem().Text("Loan Product(s) : ").FontSize(8).Bold().FontColor(Colors.Black);
                                    r.AutoItem().Text(loanBatchNamesText  ?? "N/A").FontSize(8).FontColor(Colors.Black);
                                });

                                innerCol.Item().PaddingVertical(4).PaddingRight(5)
                                .Row(r =>
                                {
                                    r.AutoItem().Text("Payment Batch Initiation Date : ").FontSize(8).Bold().FontColor(Colors.Black);
                                    r.AutoItem().Text(batch?.CreatedOn.ToString("yyyy-MM-dd") ?? "N/A").FontSize(8).FontColor(Colors.Black);
                                });

                                innerCol.Item().PaddingVertical(4).PaddingRight(5)
                                .Row(r =>
                                {
                                    r.AutoItem().Text("Country : ").FontSize(8).Bold().FontColor(Colors.Black);
                                    r.AutoItem().Text(batch?.Country.CountryName ?? "N/A").FontSize(8).FontColor(Colors.Black);
                                });

                                innerCol.Item().PaddingVertical(4).PaddingRight(5)
                                .Row(r =>
                                {
                                    r.AutoItem().Text("Currency : ").FontSize(8).Bold().FontColor(Colors.Black);
                                    r.AutoItem().Text(batch?.Country.Code ?? "N/A").FontSize(8).FontColor(Colors.Black);
                                });

                                innerCol.Item().PaddingVertical(4).PaddingRight(5)
                                .Row(r =>
                                {
                                    r.AutoItem().Text("Total Beneficiary Count : ").FontSize(8).Bold().FontColor(Colors.Black);
                                    r.AutoItem().Text(total.ToString() ?? "0").FontSize(8).FontColor(Colors.Black);
                                });

                                innerCol.Item().PaddingVertical(4).PaddingRight(5)
                                .Row(r =>
                                {
                                    r.AutoItem().Text("Total Farmer Earnings (LC) : ").FontSize(8).Bold().FontColor(Colors.Black);
                                    r.AutoItem().Text(apps.Sum(x => x.FarmerEarnings).ToString() ?? "0.00").FontSize(8).FontColor(Colors.Black);
                                });

                                innerCol.Item().PaddingVertical(4).PaddingRight(5)
                                .Row(r =>
                                {
                                    r.AutoItem().Text("Total Payable Farmer Earnings (LC) : ").FontSize(8).Bold().FontColor(Colors.Black);
                                    r.AutoItem().Text(apps.Sum(x => x.FarmerPayableEarnings).ToString() ?? "0.00").FontSize(8).FontColor(Colors.Black);
                                });

                                innerCol.Item().PaddingVertical(4).PaddingRight(5)
                                .Row(r =>
                                {
                                    r.AutoItem().Text("Total Loan Deductions (LC) : ").FontSize(8).Bold().FontColor(Colors.Black);
                                    r.AutoItem().Text(apps.Sum(x => x.LoanDeduction).ToString() ?? "0.00").FontSize(8).FontColor(Colors.Black);
                                });

                                innerCol.Item().PaddingVertical(4).PaddingRight(5)
                                .Row(r =>
                                {
                                    r.AutoItem().Text("Processing Method : ").FontSize(8).Bold().FontColor(Colors.Black);
                                    r.AutoItem().Text(batch?.ReferenceNumber ?? "N/A").FontSize(8).FontColor(Colors.Black);
                                });
                            });

                            // RIGHT COLUMN
                            row.RelativeItem().Column(innerCol =>
                            {
                            innerCol.Item().PaddingVertical(4).PaddingRight(5)
                            .Row(r =>
                            {
                                r.AutoItem().Text("Payment Request Creation Date : ").FontSize(8).Bold().FontColor(Colors.Black);
                                r.AutoItem().Text(batch?.CreatedOn.ToString("yyyy-MM-dd") ?? "N/A").FontSize(8).FontColor(Colors.Black);
                            });

                            innerCol.Item().PaddingVertical(4).PaddingRight(5)
                            .Row(r =>
                            {
                                r.AutoItem().Text("Payment Activity Status : ").FontSize(8).Bold().FontColor(Colors.Black);
                                r.AutoItem().Text(callBackState ?? "N/A").FontSize(8).FontColor(Colors.Black);
                            });

                            innerCol.Item().PaddingVertical(4).PaddingRight(5)
                            .Row(r =>
                            {
                                r.AutoItem().Text("Authorization Status : ").FontSize(8).Bold().FontColor(Colors.Black);
                                r.AutoItem().Text( "Authorised").FontSize(8).FontColor(Colors.Black);
                            });

                            innerCol.Item().PaddingVertical(4).PaddingRight(5)
                            .Row(r =>
                            {
                            r.AutoItem().Text("Progress : ").FontSize(8).Bold().FontColor(Colors.Black);
                          
                                    r.ConstantItem(100).Height(10).Background(Colors.Grey.Lighten2).Row(bar =>
                                    {
                                        // Filled portion
                                        bar.ConstantItem((float)(100 * percentage))
                                            .Background(Colors.Green.Medium);

                                        // Empty portion
                                        bar.RelativeItem().Background(Colors.Transparent);
                                    });
                               
                                r.AutoItem().Text(" : "+percentage + "%").FontSize(8).FontColor(Colors.Black);
                            });

                                innerCol.Item().PaddingVertical(4).PaddingRight(5)
                                .Row(r =>
                                {
                                    r.AutoItem().Text("Total Paid Count : ").FontSize(8).Bold().FontColor(Colors.Black);
                                    r.AutoItem().Text(completed.ToString() ?? "0").FontSize(8).FontColor(Colors.Black);
                                });

                                innerCol.Item().PaddingVertical(4).PaddingRight(5)
                                .Row(r =>
                                {
                                    r.AutoItem().Text("Total Unpaid Count : ").FontSize(8).Bold().FontColor(Colors.Black);
                                    r.AutoItem().Text((total - completed).ToString() ?? "0").FontSize(8).FontColor(Colors.Black);
                                });

                                innerCol.Item().PaddingVertical(4).PaddingRight(5)
                                .Row(r =>
                                {
                                    r.AutoItem().Text("Total Amount Paid (LC) : ").FontSize(8).Bold().FontColor(Colors.Black);
                                    r.AutoItem().Text(apps.Where(x=> x.Status == "Complete").Sum(x => x.FarmerPayableEarnings).ToString() ?? "0.00").FontSize(8).FontColor(Colors.Black);
                                });

                                innerCol.Item().PaddingVertical(4).PaddingRight(5)
                                .Row(r =>
                                {
                                    r.AutoItem().Text("Total Amount Not Paid (LC) : ").FontSize(8).Bold().FontColor(Colors.Black);
                                    r.AutoItem().Text(apps.Where(x => x.Status != "Complete").Sum(x => x.FarmerPayableEarnings).ToString() ?? "0.00").FontSize(8).FontColor(Colors.Black);
                                });
                            });
                        });

                        col.Item().PaddingVertical(10).LineHorizontal(1).LineColor(Colors.Grey.Lighten2);

                        col.Item().Row(row =>
                        {
                            // LEFT COLUMN
                            row.RelativeItem().Column(innerCol =>
                            {
                                innerCol.Item().PaddingVertical(4).PaddingRight(5)
                                .Column(c =>
                                {
                                    c.Item().PaddingBottom(5).Text("Payment Batch Authorisation Log :")
                                        .FontSize(8).Bold().FontColor(Colors.Black);

                                    c.Item().Table(table =>
                                    {
                                        // Define columns
                                        table.ColumnsDefinition(columns =>
                                        {
                                            columns.ConstantColumn(120); // Status
                                            columns.RelativeColumn();    // Actioned By
                                            columns.ConstantColumn(140); // Date Time
                                        });

                                        // Header Row
                                        table.Header(header =>
                                        {
                                            header.Cell().Text("Status").Bold().FontSize(9).FontColor(Colors.Black);
                                            header.Cell().Text("Actioned By").Bold().FontSize(9).FontColor(Colors.Black);
                                            header.Cell().Text("Date Time").Bold().FontSize(9).FontColor(Colors.Black);
                                        });

                                        // Data Rows
                                        foreach (var status in apps[0].BatchStatusHistory)
                                        {
                                            table.Cell().Text(status.Status).FontSize(9).FontColor(Colors.Grey.Darken2);
                                            table.Cell().Text(status.UpdatedByUserName ?? "N/A").FontSize(9).FontColor(Colors.Grey.Darken2);
                                            table.Cell().Text(status.UpdatedAt.ToString("yyyy-MM-dd HH:mm")).FontSize(9).FontColor(Colors.Grey.Darken2);
                                        }
                                    });
                                });
                            });

                            // RIGHT COLUMN
                            row.RelativeItem().Column(innerCol =>
                            {
                                innerCol.Item().PaddingVertical(4).PaddingRight(5)
                                .Row(r =>
                                {
                                    r.AutoItem().Text("Report generated on : ").FontSize(8).Bold().FontColor(Colors.Black);
                                    r.AutoItem().Text(TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,
                            TimeZoneInfo.FindSystemTimeZoneById("E. Africa Standard Time"))
                            .ToString("yyyy-MM-dd HH:mm") + " EAT").FontSize(8).FontColor(Colors.Black);
                                });

                                innerCol.Item().PaddingVertical(4).PaddingRight(5)
                                .Row(r =>
                                {
                                    r.AutoItem().Text("Report generated by : ").FontSize(8).Bold().FontColor(Colors.Black);
                                    r.AutoItem().Text(payments[0].CurrentUserName ?? "N/A").FontSize(8).FontColor(Colors.Black);
                                });


                            });
                        });

                        col.Item().PaddingVertical(10).LineHorizontal(1).LineColor(Colors.Grey.Lighten2);


                        col.Item().PaddingVertical(2).Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                for (int i = 0; i < 13; i++) columns.RelativeColumn();
                            });

                            string[] headers = new[]
                            {
                             "Payment Date", "Farmer System ID", "Farmer Name", "Farmer Earnings",
                            "Payable Earnings", "Loan Deduction", "Loan O/B", "Loan C/B",
                            "Processing Method", "Provider Reference", "Mobile No", "Status","Error Message"
                        };

                            foreach (var header in headers)
                                table.Cell().Element(HeaderCellStyle).Text(header).Bold();

                            int count = 1;
                            decimal totalLoanDeduction = 0, totalOpening = 0, totalClosing = 0,totalFarmerEarning = 0, totalPayableEarnings=0;

                            foreach (var app in apps)
                            {
                              
                                table.Cell().Element(CellStyle).Text(TimeZoneInfo.ConvertTimeFromUtc(app.PaymentDate, TimeZoneInfo.FindSystemTimeZoneById("E. Africa Standard Time")).ToString("yyyy-MM-dd HH:mm"));
                                table.Cell().Element(CellStyle).Text(app.FarmerSystemId);
                                table.Cell().Element(CellStyle).Text(app.FarmerName);
                                table.Cell().Element(CellStyle).Text(app.FarmerEarnings.ToString("N2"));
                                table.Cell().Element(CellStyle).Text(app.FarmerPayableEarnings.ToString("N2"));
                                table.Cell().Element(CellStyle).Text(app.LoanDeduction.ToString("N2"));
                                table.Cell().Element(CellStyle).Text(app.LoanOpeningBalance.ToString("N2"));
                                table.Cell().Element(CellStyle).Text(app.LoanClosingBalance.ToString("N2"));
                                table.Cell().Element(CellStyle).Text(app.ProcessingMethod);
                                table.Cell().Element(CellStyle).Text(app.PaymentReference);
                                table.Cell().Element(CellStyle).Text(app.ReceivingMobileNo);
                                table.Cell().Element(CellStyle).AlignMiddle().Row(row =>
                                {
                                    row.RelativeColumn().AlignBottom().Column(col =>
                                    {
                                        col.Item().Row(innerRow =>
                                        {
                                            
                                            innerRow.RelativeColumn().Text(txt =>
                                            {
                                                txt.Span(app.Status == "Complete" ? "Yes" : "No")
                                                    .FontColor(app.Status == "Complete" ? Colors.Green.Medium : Colors.Red.Medium)
                                                    .Bold();

                                                
                                            });
                                            innerRow.ConstantColumn(20)
                                               .Height(10)
                                               .Background(app.Status == "Complete" ? Colors.Green.Medium : Colors.Red.Medium);

                                        });
                                    });
                                });
                                table.Cell().Element(CellStyle).Text(app.Remarks ?? "N/A");

                                totalLoanDeduction += app.LoanDeduction;
                                totalOpening += app.LoanOpeningBalance;
                                totalClosing += app.LoanClosingBalance;
                                totalFarmerEarning += app.FarmerEarnings;
                                totalPayableEarnings += app.FarmerPayableEarnings;
                            }

                            netLoanDeduction += totalLoanDeduction;
                            netOpening += totalOpening;
                            netClosing += totalClosing;
                            netFarmerEarnings += totalFarmerEarning;
                            netPayableEarnings += totalPayableEarnings;

                            void TotalCell(string text) => table.Cell().Element(FooterCellStyle).Text(text);

                            TotalCell("TOTAL"); TotalCell(""); TotalCell(""); TotalCell(totalFarmerEarning.ToString("N2")); TotalCell(totalPayableEarnings.ToString("N2"));
                            TotalCell(totalLoanDeduction.ToString("N2"));
                            TotalCell(totalOpening.ToString("N2")); TotalCell(totalClosing.ToString("N2"));
                            TotalCell(""); TotalCell(""); TotalCell(""); TotalCell("");
                        });

                      

                        col.Item().PageBreak();
                    }

                    col.Item().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            for (int i = 0; i < 12; i++) columns.RelativeColumn();
                        });

                        void TotalCell(string text) => table.Cell().Element(FooterCellStyle).Text(text).Bold();

                        TotalCell("NET TOTALS");
                        TotalCell(""); TotalCell(""); TotalCell(netFarmerEarnings.ToString("N2")); TotalCell(netPayableEarnings.ToString("N2"));
                        TotalCell(netLoanDeduction.ToString("N2"));
                        TotalCell(netOpening.ToString("N2"));
                        TotalCell(netClosing.ToString("N2"));
                        TotalCell(""); TotalCell(""); TotalCell("");
                    });
                });

                page.Footer().AlignCenter().Column(col =>
                {
                    col.Item().Text(txt =>
                    {
                        txt.Span("Generated on: ").SemiBold();
                        txt.Span(TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,
                            TimeZoneInfo.FindSystemTimeZoneById("E. Africa Standard Time"))
                            .ToString("yyyy-MM-dd HH:mm") + " EAT");
                    });

                    col.Item().Text(txt =>
                    {
                        txt.Span("Generated by: ").SemiBold();
                        txt.Span(payments[0].CurrentUserName);
                    });
                });
            });
        });

        return document.GeneratePdf();

        static IContainer CellStyle(IContainer container) =>
            container.PaddingVertical(3).PaddingHorizontal(2).BorderBottom(1).BorderColor(Colors.Grey.Lighten1);

        static IContainer HeaderCellStyle(IContainer container) =>
            container.PaddingVertical(4).PaddingHorizontal(2);

        static IContainer FooterCellStyle(IContainer container) =>
            container.Background(Colors.Grey.Lighten3).PaddingVertical(4).PaddingHorizontal(2);
    }

    //public Task<byte[]> GeneratePaymentsPdfReport(PaymentReportSearchParams searchParams)
    //{
    //    throw new NotImplementedException();
    //}

    #endregion


    #endregion

    #region Helpers and Connections

    public static class CountryIds
    {
        public static readonly Guid Kenya = Guid.Parse("a82beb82-2d92-11ef-ad6b-46477cdd49d1");
        public static readonly Guid Uganda = Guid.Parse("a82c10f6-2d92-11ef-ad6b-46477cdd49d1");
        public static readonly Guid Tanzania = Guid.Parse("e31fb4b1-1916-43ef-a86a-c4c32d20fec4");
        // Ethiopia is omitted for now
    }
    public class CountryAddress
    {
        public static (string Line1, string Line2, string Line3, string Line4, string Phone, string Email) GetAddressByCountryId(Guid countryId)
        {
            if (countryId == CountryIds.Kenya)
            {
                return (
                    "Europa Towers, 5th Floor, Lantana Road, Westlands",
                    "P.O. Box 42234 – 00100 ,GPO",
                    "Nairobi, Kenya",
                    "",
                    "Phone: +254 (0) 716 666 862",
                    "Email: info.secaec@solidaridadnetwork.org"
                );
            }
            else if (countryId == CountryIds.Uganda)
            {
                return (
                    "Plot 5C, Off Martyrs Road, Ntinda Ministers' Village, Kampala,",
                    "P.O. Box 75478, Clock Tower",
                    "Kampala, Uganda",
                    "",
                    "Phone: +256 (0) 773 730 369 / +256 (0) 704 309 037",
                    "Email: info.uganda@solidaridadnetwork.org"
                );
            }
            else if (countryId == CountryIds.Tanzania)
            {
                return (
                    "Uzunguni Area, Sekouture Road, Arusha ",
                    "P.O Box 544",
                    "Arusha, Tanzania",
                    "",
                    "Phone: +255 (0) 784 936 392",
                    "Email: info.secaec@solidaridadnetwork.org"
                );
            }

            // Default if unknown
            return ("", "", "", "", "", "");
        }
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




    protected NpgsqlConnection GetConnection()
    {
        return new NpgsqlConnection(_connectionString);
    }

   

    protected class DatabaseConfiguration
    {
        public string DefaultConnection { get; set; }
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
    #endregion
}
