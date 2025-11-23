using AutoMapper;
using Solidaridad.Application.Models.LoanRepayment;
using Solidaridad.Core.Entities;
using Solidaridad.Core.Entities.Loans;
using Solidaridad.DataAccess.Repositories;
using Solidaridad.DataAccess.Repositories.Impl;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Collections.Generic;
using Solidaridad.Application.Models.Farmer;

namespace Solidaridad.Application.Services.Impl;

public class LoanRepaymentService : ILoanRepaymentService
{
    #region DI
    private IMapper _mapper;
    private ILoanRepaymentRepository _loanRepaymentRepository;
    private ILoanBatchRepository _loanBatchRepository;
    private ILoanApplicationRepository _loanApplicationRepository;
    public ILoanStatementRepository _loanStatementRepository;
    private IEMIScheduleRepository _emiScheduleRepository;
    private IProjectRepository _projectRepository;
    private readonly IFarmerRepository _farmerRepository;
    private readonly ICountryRepository _countryRepository;
    private ILocationProfileRepository _locationProfileRepository;



    private readonly ILoanRepaymentScheduleRepository _loanRepaymentScheduleRepository;
    public LoanRepaymentService(ILoanRepaymentRepository loanItemRepository, IProjectRepository projectRepository, ICountryRepository countryRepository,
        ILocationProfileRepository locationProfileRepository,
        ILoanApplicationRepository loanApplicationRepository,
        ILoanBatchRepository loanBatchRepository, IMapper mapper,
        IEMIScheduleRepository emiScheduleRepository, IFarmerRepository farmerRepository, ILoanStatementRepository loanStatementRepository, ILoanRepaymentScheduleRepository loanRepaymentScheduleRepository)
    {
        _loanRepaymentRepository = loanItemRepository;
        _loanBatchRepository = loanBatchRepository;
        _loanApplicationRepository = loanApplicationRepository;
        _mapper = mapper;
        _emiScheduleRepository = emiScheduleRepository;
        _loanRepaymentScheduleRepository = loanRepaymentScheduleRepository;
        _loanStatementRepository = loanStatementRepository;
        _farmerRepository = farmerRepository;
        _projectRepository = projectRepository;
        _countryRepository = countryRepository;
        _locationProfileRepository = locationProfileRepository;
    }
    #endregion

    public async Task<List<LoanTransaction>> SimulateTransaction(Guid loanApplicationId)
    {
        var loanApplication = await _loanApplicationRepository.GetFirstAsync(x => x.Id == loanApplicationId);
        var loanBatch = await _loanBatchRepository.GetFirstAsync(x => x.Id == loanApplication.LoanBatchId);

        if (loanApplication == null) throw new Exception("Loan not found");

        //var startDate = loanBatch.EffectiveDate.AddMonths(loanBatch.GracePeriod.Value).AddDays(1); // Loan start date (e.g., Jan 1, 2025)
        var graceDate = loanBatch.EffectiveDate.AddMonths(loanBatch.GracePeriod.Value);
        var startDate = new DateTime(graceDate.Year, graceDate.Month, 1);

        var monthlyRate = loanBatch.CalculationTimeframe == "Monthly" ? loanBatch.InterestRate : loanBatch.InterestRate * loanBatch.Tenure;
        monthlyRate = monthlyRate / 100; // Convert to decimal


        decimal initialPrincipal = loanApplication.PrincipalAmount; //2500;
        decimal monthlyInterestRate = (decimal)monthlyRate; // 1% Monthly

        // Payments (Date, Amount, Batch ID)
        var payments = new List<(DateTime, decimal, Guid)>
        {
            (new DateTime(2025, 5, 15), 200, new Guid()),
            (new DateTime(2025, 6, 10), 2350, new Guid()),
            (new DateTime(2025, 6, 12), 23, new Guid())
        };

        var transactions = GenerateLoanReport(loanApplicationId, initialPrincipal, monthlyInterestRate, payments, startDate);

        return transactions;
    }

    public async Task<string> GeneratePaymentSchedule(Guid loanApplicationId)
    {
        var loanApplication = await _loanApplicationRepository.GetFirstAsync(x => x.Id == loanApplicationId);
        if (loanApplication != null)
        {
            if (loanApplication.PrincipalAmount == 0)
            {
                return "Loan application has no principal amount. Cannot generate payment schedule.";
            }
        }

        var loanBatch = await _loanBatchRepository.GetFirstAsync(x => x.Id == loanApplication.LoanBatchId);

        if (loanApplication == null) throw new Exception("Loan not found");

        // Fix for CS1061: Ensure nullability is handled before calling AddMonths on a nullable DateTime
        var graceDate = loanApplication.DisbursementDate.HasValue
            ? loanApplication.DisbursementDate.Value.AddMonths(loanBatch.GracePeriod.Value)
            : throw new Exception("Disbursement date is not set.");

        //var graceDate = new DateTime(2025, 1, 1);

        // Else, add another month and use the 1st of that month
        var startDate = graceDate.Day == 1
            ? graceDate : new DateTime(graceDate.AddMonths(1).Year, graceDate.AddMonths(1).Month, 1);

        var monthlyRate = loanBatch.CalculationTimeframe == "Monthly" ? loanBatch.InterestRate : loanBatch.InterestRate / 12;

        decimal initialPrincipal = loanApplication.PrincipalAmount;
        decimal monthlyInterestRate = (decimal)monthlyRate;

        var transactions = loanBatch.RateType == "Flat" ? CalculateFlatLoanSchedules(loanApplicationId, initialPrincipal, monthlyInterestRate, startDate, (int)loanBatch.Tenure) : CalculateReducingBalanceLoanSchedules(loanApplicationId, initialPrincipal, monthlyInterestRate, startDate, (int)loanBatch.Tenure);

        var getPrevious = await _emiScheduleRepository.GetAllAsync(c => c.LoanApplicationId == loanApplicationId);
        if (getPrevious != null)
        {
            foreach (var p in getPrevious)
            {
                p.IsDeleted = true;

            }
            await _emiScheduleRepository.UpdateRange(getPrevious);
        }
        await _emiScheduleRepository.AddRange(transactions);

        return "Ok";
    }


    public static List<EMISchedule> CalculateFlatLoanSchedules(
       Guid loanApplicationId,
       decimal initialPrincipal,
       decimal monthlyInterestRate,
       DateTime startDate,
       int tenure)
    {
        List<EMISchedule> transactions = new List<EMISchedule>();

        decimal currentPrincipal = initialPrincipal;
        decimal cumulativeBalance = initialPrincipal;
        decimal interest = Math.Round(initialPrincipal * monthlyInterestRate / 100, 2);
        // DateTime startDate = new DateTime(2025, 1, 1);
        DateTime currentDate = startDate;

        int i = 0;
        //while (currentPrincipal > 0 || payments.Any() && i < 6)
        while (i < tenure)
        {
            i++;
            // Add interest for the month


            transactions.Add(new EMISchedule
            {

                StartDate = startDate.AddMonths(i-1),
                EndDate = startDate.AddMonths(i).AddDays(-1),
                PrincipalAmount = currentPrincipal,
                InterestAmount = interest,
                Balance = cumulativeBalance - (initialPrincipal / tenure),
                LoanApplicationId = loanApplicationId,
                Amount = (initialPrincipal / tenure) + interest,
                PaymentStatus = "Pending"
            });

            cumulativeBalance = cumulativeBalance - (initialPrincipal / tenure);
            currentPrincipal = currentPrincipal - (initialPrincipal / tenure);



        }

        return transactions;
    }

    public static List<EMISchedule> CalculateReducingBalanceLoanSchedules(
    Guid loanApplicationId,
    decimal initialPrincipal,
    decimal monthlyInterestRate,
    DateTime startDate,
    int tenure)
    {
        List<EMISchedule> transactions = new List<EMISchedule>();

        decimal currentPrincipal = initialPrincipal;
        decimal monthlyRate = monthlyInterestRate / 100;

        // Calculate fixed EMI (monthly payment)
        decimal emi = monthlyRate == 0
            ? initialPrincipal / tenure
            : (initialPrincipal * monthlyRate) /
              (1 - (decimal)Math.Pow((double)(1 + monthlyRate), -tenure));

        DateTime currentDate = startDate;

        for (int i = 0; i < tenure; i++)
        {
            decimal interest = Math.Round(currentPrincipal * monthlyRate, 2);
            decimal principalPart = Math.Round(emi - interest, 2);
            decimal balance = Math.Max(currentPrincipal - principalPart, 0);

            var periodStart = new DateTime(currentDate.Year, currentDate.Month, 1).AddMonths(i);
            var periodEnd = periodStart.AddMonths(1).AddDays(-1); // End of the month

            transactions.Add(new EMISchedule
            {
                StartDate = periodStart,
                EndDate = periodEnd,
                PrincipalAmount = currentPrincipal,
                InterestAmount = interest,
                Balance = Math.Round(balance, 2),
                LoanApplicationId = loanApplicationId,
                Amount = Math.Round(emi, 2),
                PaymentStatus = "Pending"
            });

            currentPrincipal = balance;
        }

        return transactions;
    }




    public static List<LoanTransaction> GenerateLoanReport(
       Guid loanApplicationId,
       decimal initialPrincipal,
       decimal monthlyInterestRate,
       List<(DateTime Date, decimal Amount, Guid PaymentBatchId)> payments,
       DateTime startDate)
    {
        List<LoanTransaction> transactions = new List<LoanTransaction>();

        decimal currentPrincipal = initialPrincipal;
        decimal cumulativeBalance = initialPrincipal;

        // DateTime startDate = new DateTime(2025, 1, 1);
        DateTime currentDate = startDate;

        int i = 0;
        //while (currentPrincipal > 0 || payments.Any() && i < 6)
        while (i < 6)
        {
            i++;
            // Add interest for the month
            decimal interest = Math.Round(currentPrincipal * monthlyInterestRate, 2);

            transactions.Add(new LoanTransaction
            {
                Date = currentDate,
                Description = $"Interest for {currentDate:MMM yyyy}",
                EffectivePrincipal = currentPrincipal,
                InterestAccrued = interest,
                CumulativeBalance = cumulativeBalance + interest,
                LoanId = loanApplicationId
            });

            cumulativeBalance += interest;

            // Process payments due in the current month
            var monthlyPayments = payments.Where(p => p.Date.Year == currentDate.Year && p.Date.Month == currentDate.Month).ToList();

            foreach (var payment in monthlyPayments)
            {
                decimal paymentAmount = payment.Amount;
                decimal principalPaid = 0;
                decimal interestPaid = 0;

                if (paymentAmount >= interest)
                {
                    interestPaid = interest;
                    paymentAmount -= interest;
                    interest = 0;
                }
                else
                {
                    interestPaid = paymentAmount;
                    paymentAmount = 0;
                }

                if (paymentAmount > 0)
                {
                    principalPaid = Math.Min(currentPrincipal, paymentAmount);
                    currentPrincipal -= principalPaid;
                }

                transactions.Add(new LoanTransaction
                {
                    Date = payment.Date,
                    Description = $"Payment - Batch Test",
                    PaymentReceived = payment.Amount,
                    PrincipalPaid = principalPaid,
                    InterestPaid = interestPaid,
                    CumulativeBalance = cumulativeBalance - (principalPaid + interestPaid),
                    LoanId = loanApplicationId,
                    PaymentBatchId = payment.PaymentBatchId
                });

                cumulativeBalance -= (principalPaid + interestPaid);
            }

            // Remove processed payments
            payments.RemoveAll(p => p.Date.Year == currentDate.Year && p.Date.Month == currentDate.Month);

            // Move to the next month
            currentDate = currentDate.AddMonths(1);
        }

        return transactions;
    }





    public async Task<CreateLoanRepaymentResponseModel> CreateAsync(CreateLoanRepaymentModel createLoanRepaymentModel)
    {
        try
        {
            return null;

            //decimal annualInterestRate = 0;
            //var loanBatch = await _loanBatchRepository.GetAllAsync(x => createLoanRepaymentModel.LoanApplicationId == createLoanRepaymentModel.LoanApplicationId);
            //if (loanBatch != null)
            //{
            //    annualInterestRate = loanBatch.FirstOrDefault().InterestRate;
            //}
            //var addedItem = await _loanRepaymentRepository.AddRepayment(createLoanRepaymentModel.LoanApplicationId,
            //    createLoanRepaymentModel.RepaymentAmount, annualInterestRate);

            //return new CreateLoanRepaymentResponseModel
            //{
            //    Id = addedItem.Id
            //};
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public List<LoanRepaymentResponseModel> GetRepaymentHistory(Guid loanApplicationId)
    {
        return _mapper.Map<List<LoanRepaymentResponseModel>>
            (_loanRepaymentRepository.GetRepaymentHistory(loanApplicationId));
    }

    public void ApplyPayment(Guid loanApplicationId, decimal amount, string paymentMode, string referenceNumber)
    {
        _loanRepaymentRepository.ApplyPayment(loanApplicationId, amount, paymentMode, referenceNumber);
    }






    #region Statement

    #region Cron job methods for statement generation
    // Generates a monthly loan statement for the given loan application ID final iteration for automation before testing for production
    public async Task<Guid> GenerateMonthlyLoanStatement(Guid loanApplicationId)
    {
        var loanApplication = await _loanApplicationRepository.GetFirstAsync(x => x.Id == loanApplicationId && x.IsDeleted == false) ;
        if (loanApplication == null) throw new Exception("Loan not found");

        var loanBatch = await _loanBatchRepository.GetFirstAsync(x => x.Id == loanApplication.LoanBatchId);

        var graceDate = loanApplication.DisbursementDate.HasValue
            ? loanApplication.DisbursementDate.Value.AddMonths(loanBatch.GracePeriod ?? 0)
            : throw new Exception("Disbursement date is not set.");

        // Else, add another month and use the 1st of that month
        var startDate = graceDate.Day == 1
            ? graceDate : new DateTime(graceDate.AddMonths(1).Year, graceDate.AddMonths(1).Month, 1);

        if (DateTime.UtcNow < startDate)
        {
            //throw new Exception("Cannot generate statement before the grace period ends.");
            return Guid.Empty; // Return empty Guid if statement cannot be generated
        }
        var repaymentSchedule = (await _emiScheduleRepository.GetAllAsync(x => x.LoanApplicationId == loanApplicationId && !x.IsDeleted))
                                .OrderBy(x => x.StartDate).ToList();

        DateTime currentDate = DateTime.UtcNow;

        var scheduleThisMonth = repaymentSchedule
            .Where(x => x.StartDate.Date == currentDate.Date).FirstOrDefault();


        var allStatements = await _loanStatementRepository.GetAllAsync(x => x.ApplicationId == loanApplicationId);
        var latestStatement = allStatements
            .OrderByDescending(x => x.StatementDate)
            .FirstOrDefault();

        decimal openingBalance = latestStatement?.LoanBalance ?? loanApplication.PrincipalAmount;
        decimal debitAmount = scheduleThisMonth.InterestAmount;
        decimal loanBalance = openingBalance + debitAmount;

        // Calculate accrued interest 
        decimal scheduledInterestPayment = repaymentSchedule
            .Where(x => x.StartDate.Date <= currentDate.Date)
            .Sum(x => x.InterestAmount);

        decimal paidInterest = allStatements != null ? allStatements
            .Where(x => x.StatementDate.Date <= currentDate.Date)
            .Sum(x => x.InterestPaid) : 0;

        decimal accuredInterest = scheduledInterestPayment - paidInterest;

        //Calculate accrued principal payment
        decimal scheduledPrincipalPayment = repaymentSchedule
           .Where(x => x.StartDate.Date <= currentDate.Date)
           .Sum(x => x.PrincipalAmount);

        decimal paidPrinicpal = allStatements != null ? allStatements
          .Where(x => x.StatementDate.Date <= currentDate.Date)
          .Sum(x => x.PrincipalPaid) : 0;

        decimal accuredPrincipalPayment = scheduledPrincipalPayment - paidPrinicpal;
        // Not sorted yet


        decimal tempPrincipal = loanApplication.PrincipalAmount ;

        string monthName = DateTime.UtcNow.ToString("MMMM yyyy");


        var loanStatement = new LoanStatement
        {
            StatementDate = DateTime.UtcNow,
            FarmerId = loanApplication.FarmerId,
            SystemId = loanApplication.LoanNumber,
            ApplicationId = loanApplication.Id,
            TransactionReference = "N/A",
            TransactionType = "Interest Accumulation",
            Description = $"{monthName} Interest",
            OpeningBalance = openingBalance,
            DebitAmount = debitAmount,
            CreditAmount = 0,
            LoanBalance = loanBalance,
            AccuredInterest = accuredInterest,
            AccuredPrincipalPayment = accuredPrincipalPayment,
            InterestPaid = 0,
            PrincipalPaid = 0
        };

        var savedStatement = await _loanStatementRepository.AddAsync(loanStatement);
        return savedStatement.Id;
    }

    #endregion


    #region Repayment Statement generation
    public async Task<Guid?> GenerateLatestPaymentBasedLoanStatement(Guid loanApplicationId, decimal farmerEarningsLC, string referenceId)
    {
        var loanApplication = await _loanApplicationRepository.GetFirstAsync(x => x.Id == loanApplicationId)
         ?? throw new Exception("Loan not found");

        var loanBatch = await _loanBatchRepository.GetFirstAsync(x => x.Id == loanApplication.LoanBatchId)
            ?? throw new Exception("Loan batch not found");

        var latestRepayment = await _loanRepaymentRepository.GetFirstAsync(
            x => x.LoanApplicationId == loanApplicationId && !x.IsDeleted && x.ReferenceNumber == referenceId)
            ?? throw new Exception("Repayment not found");

        var existingStatements = (await _loanStatementRepository.GetAllAsync(x => x.ApplicationId == loanApplicationId))
            .OrderByDescending(x => x.StatementDate)
            .ToList();

        var latestStatement = existingStatements?.FirstOrDefault();

        decimal openingBalance = latestStatement != null ? latestStatement.LoanBalance : loanApplication.PrincipalAmount;

        decimal paymentAmount = latestRepayment.AmountPaid;

        decimal closingBalance = openingBalance - paymentAmount;

        decimal interestAccruedBeforePayment = latestStatement?.AccuredInterest ?? 0;

        decimal principalAccruedBeforePayment = latestStatement?.AccuredPrincipalPayment ?? 0;

        decimal totalPrincipalPaidBeforePayment = existingStatements?.Sum(x => x.PrincipalPaid)??0;

        decimal totalInterestPaidBeforePayment = existingStatements?.Sum(x => x.InterestPaid) ?? 0;

        //decimal principalRemainingBeforePayment = loanApplication.PrincipalAmount - totalPrincipalPaidBeforePayment;

        decimal principalPaidForThisPayment = 0;

        decimal interestPaidForThisPayment = 0;

        decimal interestAccruedAfterPayment = 0;
        decimal principalAccruedAfterPayment = 0;


        if(principalAccruedBeforePayment + interestAccruedBeforePayment > 0)
        {
             if(principalAccruedBeforePayment - paymentAmount >= 0)
            {
                principalPaidForThisPayment = paymentAmount;
                principalAccruedAfterPayment = principalAccruedBeforePayment - paymentAmount;
            }
            else
            {
                var pendingBalance = paymentAmount - principalAccruedBeforePayment;
                
                principalPaidForThisPayment = principalAccruedBeforePayment;

                principalAccruedAfterPayment = 0; 

                if(interestAccruedBeforePayment - pendingBalance >=0)
                {
                    interestPaidForThisPayment = pendingBalance;

                    interestAccruedAfterPayment = interestAccruedBeforePayment - pendingBalance;
                }
                else
                {
                
                    interestPaidForThisPayment = interestAccruedBeforePayment;

                    interestAccruedAfterPayment = 0;

                    principalPaidForThisPayment += (pendingBalance - interestAccruedBeforePayment);

                    principalAccruedAfterPayment = interestAccruedAfterPayment - pendingBalance ; 
                }

            }
        }
        else
        {
            principalPaidForThisPayment = paymentAmount;

            principalAccruedAfterPayment -= paymentAmount;
        }
        string paymentType = interestPaidForThisPayment > 0 && principalPaidForThisPayment > 0 ? "Principal and interest repayment" : principalPaidForThisPayment > 0 ? "Principal Repayment" : "Interest Repayment"; 


        var statement = new LoanStatement
        {
            StatementDate = DateTime.UtcNow,
            ApplicationId = loanApplication.Id,
            FarmerId = loanApplication.FarmerId,
            SystemId = loanApplication.LoanNumber,
            TransactionReference = referenceId != "" ? referenceId : "",
            TransactionType = paymentType,
            Description = "Repayment",
            DebitAmount = 0,
            OpeningBalance = openingBalance,
            CreditAmount = paymentAmount,
            LoanBalance = closingBalance,
            PrincipalPaid = principalPaidForThisPayment,
            InterestPaid = interestPaidForThisPayment,
            AccuredInterest = interestAccruedAfterPayment,
            AccuredPrincipalPayment = principalAccruedAfterPayment,
        };

        
        var saved = await _loanStatementRepository.AddAsync(statement);

        if(closingBalance <= 0)
        {
            loanApplication.Status = new Guid("f49faffa-b113-4546-ac7f-485164e5a822") ;
            await _loanApplicationRepository.UpdateAsync(loanApplication);
        }
        return saved?.Id;
    }




    #endregion



    // This is referenced but not required anymore
    public async Task<List<Guid>> GenerateLoanStatement(Guid loanApplicationId)
    {
        var loanApplication = await _loanApplicationRepository.GetFirstAsync(x => x.Id == loanApplicationId)
            ?? throw new Exception("Loan not found");
        var existingStatements = await _loanStatementRepository.GetAllAsync(x => x.ApplicationId == loanApplicationId);

        if (existingStatements != null)
        {
            foreach (var exist in existingStatements)
            {
                await _loanStatementRepository.DeleteLoanStatement(exist.Id);
            }
        }


        var loanBatch = await _loanBatchRepository.GetFirstAsync(x => x.Id == loanApplication.LoanBatchId);
        var repayments = (await _loanRepaymentRepository.GetAllAsync(x => x.LoanApplicationId == loanApplicationId && !x.IsDeleted))
            .OrderBy(x => x.PaymentDate).ToList();

        var repaymentSchedule = (await _emiScheduleRepository.GetAllAsync(x => x.LoanApplicationId == loanApplicationId && !x.IsDeleted))
            .OrderBy(x => x.EndDate).ToList();

        var statementsToSave = new List<LoanStatement>();

        var graceDate = loanBatch.EffectiveDate.AddMonths(loanBatch.GracePeriod ?? 0);
       
        // Else, add another month and use the 1st of that month
        var startMonth = graceDate.Day == 1
            ? graceDate : new DateTime(graceDate.AddMonths(1).Year, graceDate.AddMonths(1).Month, 1);



        var currentMonth = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);

        decimal principalRemaining = loanApplication.PrincipalAmount;
        decimal interestAccruedTotal = 0;
        decimal totalScheduledPrincipal = 0;
        decimal totalPrincipalPaid = 0;
        decimal openingBalance = loanApplication.PrincipalAmount;

        decimal loanInterestMonthly = loanBatch.CalculationTimeframe == "Monthly"
            ? loanBatch.InterestRate
            : loanBatch.InterestRate / 12;

        for (var month = startMonth; month <= currentMonth; month = month.AddMonths(1))
        {
            string monthName = month.ToString("MMMM yyyy");

            var monthStart = new DateTime(month.Year, month.Month, 1);
            var monthEnd = monthStart.AddMonths(1).AddTicks(-1);

            // Scheduled principal for this month
            var scheduledPrincipalThisMonth = repaymentSchedule
                .Where(s => s.StartDate.Date <= monthStart.Date && s.EndDate.Date > monthStart.Date)
                .Sum(s => s.Amount - s.InterestAmount);

            totalScheduledPrincipal += scheduledPrincipalThisMonth;

            // Interest accumulation
            var interestThisMonth = Math.Round(principalRemaining * loanInterestMonthly / 100, 2);
            interestAccruedTotal += interestThisMonth;
            openingBalance += interestThisMonth;

            statementsToSave.Add(new LoanStatement
            {
                StatementDate = month,
                ApplicationId = loanApplication.Id,
                FarmerId = loanApplication.FarmerId,
                SystemId = loanApplication.LoanNumber,
                TransactionType = "Interest Accumulation",
                TransactionReference = GenerateTransactionCode(),
                Description = $"{monthName} Interest",
                OpeningBalance = openingBalance - interestThisMonth,
                DebitAmount = interestThisMonth,
                CreditAmount = 0,
                LoanBalance = openingBalance,
                AccuredInterest = interestAccruedTotal,
                AccuredPrincipalPayment = totalScheduledPrincipal - totalPrincipalPaid,
                InterestPaid = 0,
                PrincipalPaid = 0
            });

            // Repayment in this month
            var monthlyRepayments = repayments
                .Where(x => x.PaymentDate >= monthStart && x.PaymentDate <= monthEnd)
                .ToList();

            if (monthlyRepayments.Any())
            {
                foreach (var payment in monthlyRepayments)
                {
                    decimal principalPaid = 0;
                    decimal interestPaid = 0;

                    if (principalRemaining > 0)
                    {
                        if (payment.AmountPaid <= principalRemaining)
                        {
                            principalPaid = payment.AmountPaid;
                            principalRemaining -= principalPaid;
                        }
                        else
                        {
                            principalPaid = principalRemaining;
                            interestPaid = payment.AmountPaid - principalRemaining;
                            principalRemaining = 0;
                            interestAccruedTotal -= interestPaid;
                        }
                    }
                    else
                    {
                        // Only pay interest
                        interestPaid = Math.Min(interestAccruedTotal, payment.AmountPaid);
                        interestAccruedTotal -= interestPaid;
                    }

                    
                    decimal closingBalance = principalRemaining + interestAccruedTotal;

                


                    statementsToSave.Add(new LoanStatement
                    {
                        StatementDate = payment.PaymentDate,
                        ApplicationId = loanApplication.Id,
                        FarmerId = loanApplication.FarmerId,
                        SystemId = loanApplication.LoanNumber,
                        TransactionType = loanApplication.EffectivePrincipal - principalPaid < 0 ? "Interest Repayment" :"Principal Repayment" ,
                        TransactionReference = GenerateTransactionCode(),
                        Description = $"{payment.PaymentDate:MMMM yyyy} Repayment",
                        OpeningBalance = openingBalance ,
                        DebitAmount = 0,
                        CreditAmount = payment.AmountPaid,
                        LoanBalance = openingBalance - payment.AmountPaid,
                        AccuredInterest = interestAccruedTotal,
                        AccuredPrincipalPayment = totalScheduledPrincipal - principalPaid,
                        InterestPaid = interestPaid,
                        PrincipalPaid = principalPaid
                    });
                }
            }



        }

        // Save all statements
        var ids = new List<Guid>();
        foreach (var statement in statementsToSave)
        {
            var saved = await _loanStatementRepository.AddAsync(statement);
            ids.Add(saved.Id);
        }

        return ids;
    }


    // Random transaction code generator
    private string GenerateTransactionCode()
    {
        var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var random = new Random();
        return new string(Enumerable.Repeat(chars, 7)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }


    public async Task<List<LoanStatementResponseModel>> GetApplicationStatementHistory(Guid loanApplicationId)
    {
        var statements = await _loanStatementRepository.GetAllAsync(c => c.ApplicationId == loanApplicationId);
        var ordered = statements.OrderBy(c => c.StatementDate);
        return _mapper.Map<List<LoanStatementResponseModel>>(ordered);
    }


    public async Task<byte[]> GenerateLoanStatementPdf(Guid loanApplicationId)
    {
        var statements = (await _loanStatementRepository.GetAllAsync(c => c.ApplicationId == loanApplicationId))
                         .OrderByDescending(s => s.StatementDate)
                         .ToList();

        if (statements.Count == 0)
            throw new Exception("No loan statements found for this application.");

        var firstStatement = statements.First();
        var farmer = await _farmerRepository.GetFirstAsync(c => c.Id == firstStatement.FarmerId);
        var loanApplication = await _loanApplicationRepository.GetFirstAsync(x => x.Id == loanApplicationId);
        var loanBatch = await _loanBatchRepository.GetFirstAsync(c => c.Id == loanApplication.LoanBatchId);
        var project = await _projectRepository.GetFirstAsync(c => c.Id == loanBatch.ProjectId);
        var country = await _countryRepository.GetFirstAsync(c => c.Id == project.CountryId);
        var graceDate = loanBatch.EffectiveDate.AddMonths(loanBatch.GracePeriod ?? 0);
        var startDate = new DateTime(graceDate.Year, graceDate.Month, 1);
        var endDate = startDate.AddMonths(loanBatch.Tenure ?? 0);


        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(1, Unit.Centimetre);
                page.DefaultTextStyle(x => x.FontSize(8).FontFamily("Arial"));

                // Header
                page.Header().Row(row =>
                {
                    // Left: Logo
                    row.ConstantItem(100).Image("wwwroot/logos/logo.png", ImageScaling.FitWidth); // Adjust path as needed

                    // Right: Address / Organization Info
                    row.RelativeItem().Column(col =>
                    {
                        col.Item().AlignRight().Text("Solidaridad").Bold().FontSize(12);
                        col.Item().AlignRight().Text("Kilimani Business Centre, Kirichwa Road");
                        col.Item().AlignRight().Text("P.O. Box 42234 – 00100");
                        col.Item().AlignRight().Text("Nairobi, Kenya");
                        col.Item().AlignRight().Text("Phone: +254 (0) 716 666 862");
                        col.Item().AlignRight().Text("Email: info.secaec@solidaridadnetwork.org");
                    });
                });

                // Main content
                page.Content().Column(col =>
                {
                    col.Spacing(5);
                    col.Item().LineHorizontal(1).LineColor(Colors.Grey.Lighten1);

                    // Basic Details Section
                    col.Item().Row(row =>
                    {
                        row.RelativeItem().Column(colLeft =>
                        {
                            colLeft.Spacing(10);
                            void AddDetail(string label, string value)
                            {
                                colLeft.Item().Row(innerRow =>
                                {
                                    innerRow.ConstantItem(150).Text($"{label}:").Bold();
                                    innerRow.RelativeItem().Text(value);
                                });
                            }

                            AddDetail("Statement Date", DateTime.UtcNow.ToString("dd MMM yyyy HH:mm "));
                            AddDetail("Farmer ID", farmer.SystemId);
                            AddDetail("Farmer Name", farmer.FirstName + " " + farmer.OtherNames);
                            AddDetail("Currency", country.CurrencyName != null ? country.CurrencyPrefix : "");
                            AddDetail("Total Items Value", (loanApplication.EffectivePrincipal - loanApplication.FeeApplied).ToString("N2") ?? "N/A");
                            AddDetail("Fees Applied", loanApplication.FeeApplied.ToString("N2"));
                            AddDetail("Total Principal", loanApplication.EffectivePrincipal.ToString("N2"));
                            AddDetail("Interest Rate", loanBatch.InterestRate + "% " + loanBatch.RateType);
                            AddDetail("Interest Calculation", loanBatch.CalculationTimeframe);
                            AddDetail("Tenure", loanBatch.Tenure.ToString());

                        });

                        row.RelativeItem().Column(colRight =>
                        {
                            colRight.Spacing(10);
                            void AddDetail(string label, string value)
                            {
                                colRight.Item().Row(innerRow =>
                                {
                                    innerRow.ConstantItem(150).Text($"{label}:").Bold();
                                    innerRow.RelativeItem().Text(value);
                                });
                            }
                            AddDetail("Loan Product", loanBatch.Name);
                            AddDetail("Disbursed Date", startDate.ToString("dd MMM yyyy HH:mm "));
                            AddDetail("Grace period (in months)", loanBatch.GracePeriod.ToString() ?? "");
                            AddDetail("Loan Number", loanApplication.LoanNumber);
                            AddDetail("Loan Maturity Date", endDate.ToString("dd MMM yyyy HH:mm "));
                        });
                    });

                    // Table Header
                    col.Item().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn(); // Statement Date
                            columns.RelativeColumn(); // Transaction Reference
                            columns.RelativeColumn(); // Transaction Type
                            columns.RelativeColumn(); // Description
                            columns.RelativeColumn(); // Opening Balance
                            columns.RelativeColumn(); // Debit Amount
                            columns.RelativeColumn(); // Credit Amount
                            columns.RelativeColumn(); // Loan
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                        });

                        IContainer HeaderCellStyle(IContainer container) => container
                            .PaddingVertical(5)
                            .BorderBottom(1)
                            .BorderColor(Colors.Black);

                        IContainer DataCellStyle(IContainer container) => container
                            .PaddingVertical(5);

                        // Header Row
                        table.Cell().Element(HeaderCellStyle).Text("Statement Date").Bold();
                        table.Cell().Element(HeaderCellStyle).Text("Transaction Reference").Bold();
                        table.Cell().Element(HeaderCellStyle).Text("Transaction Type").Bold();
                        table.Cell().Element(HeaderCellStyle).Text("Description").Bold();
                        table.Cell().Element(HeaderCellStyle).Text("Opening Balance").Bold();
                        table.Cell().Element(HeaderCellStyle).Text("Debit Amount").Bold();
                        table.Cell().Element(HeaderCellStyle).Text("Credit Amount").Bold();
                        table.Cell().Element(HeaderCellStyle).Text("Loan Balance").Bold();
                        table.Cell().Element(HeaderCellStyle).Text("Principal Repaid").Bold();
                        table.Cell().Element(HeaderCellStyle).Text("Interest Repaid").Bold();



                        // Data Rows
                        foreach (var s in statements)
                        {
                            table.Cell().Element(DataCellStyle).Text(s.StatementDate.ToString("yyyy-MM-dd HH:mm"));
                            table.Cell().Element(DataCellStyle).Text($"{s.TransactionReference:N2}");
                            table.Cell().Element(DataCellStyle).Text($"{s.TransactionType:N2}");
                            table.Cell().Element(DataCellStyle).Text($"{s.Description:N2}");
                            table.Cell().Element(DataCellStyle).Text(s.OpeningBalance.ToString("N2") ?? "N/A");
                            table.Cell().Element(DataCellStyle).Text(s.DebitAmount.ToString("N2") ?? "N/A");
                            table.Cell().Element(DataCellStyle).Text(s.CreditAmount.ToString("N2") ?? "N/A");
                            table.Cell().Element(DataCellStyle).Text(s.LoanBalance.ToString("N2") ?? "N/A");
                            table.Cell().Element(DataCellStyle).Text(s.PrincipalPaid.ToString("N2") ?? "N/A");
                            table.Cell().Element(DataCellStyle).Text(s.InterestPaid.ToString("N2") ?? "N/A");

                        }
                    });
                });

                // Footer
                page.Footer().AlignCenter().Text(txt =>
                {
                    txt.Span("Generated on: ").SemiBold();
                    txt.Span(TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("E. Africa Standard Time"))
       .ToString("yyyy-MM-dd HH:mm") + " EAT");

                });
            });
        });

        return await Task.FromResult(document.GeneratePdf());
    }



    #endregion
}
