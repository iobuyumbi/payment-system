using Microsoft.EntityFrameworkCore;
using Solidaridad.Core.Entities;
using Solidaridad.Core.Entities.Loans;
using Solidaridad.DataAccess.Persistence;

namespace Solidaridad.DataAccess.Repositories.Impl;

public class LoanRepaymentRepository : BaseRepository<LoanRepayment>, ILoanRepaymentRepository
{
    #region DI
    protected readonly DbSet<LoanRepayment> loanRepaymentSet;
    protected readonly DbSet<LoanApplication> loanApplicationSet;
    protected readonly DbSet<EMISchedule> emiScheduleSet;

    public LoanRepaymentRepository(DatabaseContext context) : base(context)
    {
        loanRepaymentSet = context.Set<LoanRepayment>();
        loanApplicationSet = context.Set<LoanApplication>();
        emiScheduleSet = context.Set<EMISchedule>();
    }
    #endregion

    #region Methods
    //public async Task<LoanRepayment> AddRepayment(Guid loanApplicationId, decimal repaymentAmount, decimal annualInterestRate)
    //{
    //    // Get the latest repayment
    //    var lastRepayment = loanRepaymentSet
    //        .Where(r => r.LoanApplicationId == loanApplicationId)
    //        .OrderByDescending(r => r.CreatedOn)
    //        .FirstOrDefault();

    //    decimal lastPrincipal = lastRepayment?.NewPrinciaplAmount ?? loanApplicationSet
    //        .Where(l => l.Id == loanApplicationId)
    //        .Select(l => l.PrincipalAmount)
    //        .FirstOrDefault();

    //    // Calculate interest for the month
    //    decimal monthlyRate = annualInterestRate / 12 / 100;
    //    decimal monthlyInterest = lastPrincipal * monthlyRate;

    //    // Calculate the new principal
    //    decimal newPrincipal = Math.Max(0, lastPrincipal + monthlyInterest - repaymentAmount);

    //    // Add new repayment record
    //    var addedEntity = await loanRepaymentSet.AddAsync(new LoanRepayment
    //    {
    //        LoanApplicationId = loanApplicationId,
    //        LoanAmount = lastPrincipal,
    //        LoanRepaid = repaymentAmount,
    //        NewPrinciaplAmount = newPrincipal,
    //        CreatedOn = DateTime.UtcNow
    //    });

    //    await Context.SaveChangesAsync();

    //    return addedEntity.Entity;
    //}

    public List<LoanRepayment> GetRepaymentHistory(Guid loanApplicationId)
    {
        return loanRepaymentSet
            .Where(r => r.LoanApplicationId == loanApplicationId)
            .OrderBy(r => r.CreatedOn)
            .ToList();
    }

    public void ApplyPayment(Guid loanApplicationId, decimal paymentAmount, string paymentMode, string referenceNumber)
    {
        var emis = emiScheduleSet
            .Where(e => e.LoanApplicationId == loanApplicationId && e.PaymentStatus != "Paid" && e.IsDeleted == false)
            .OrderBy(e => e.StartDate)
            .ToList();

        decimal remainingPayment = paymentAmount;

        //// 1. update emi schedule
        //foreach (var emi in emis)
        //{
        //    if (remainingPayment <= 0) break;

        //    decimal principalPayment = Math.Min(remainingPayment, emi.PrincipalAmount);
        //    emi.PrincipalAmount -= principalPayment;
        //    remainingPayment -= principalPayment;

        //    decimal interestPayment = Math.Min(remainingPayment, emi.InterestAmount);
        //    emi.InterestAmount -= interestPayment;
        //    remainingPayment -= interestPayment;

        //    emi.Balance = emi.PrincipalAmount + emi.InterestAmount;
        //    //var balance = emi.PrincipalAmount + emi.InterestAmount;
        //    emi.PaymentStatus = emi.Balance == 0 ? "Paid" : (emi.Balance < emi.Amount ? "Partially Paid" : "Pending");

        //    emiScheduleSet.Update(emi);
        //}

        // 2. Record the payment transaction
        var payment = new LoanRepayment
        {
            LoanApplicationId = loanApplicationId,
            AmountPaid = paymentAmount,
            PaymentDate = DateTime.UtcNow,
            PaymentMode = paymentMode,
            ReferenceNumber = referenceNumber,
            CreatedOn = DateTime.UtcNow,
        };
        loanRepaymentSet.Add(payment);

        //// 3. update payabale balance
        //var loan = loanApplicationSet.FirstOrDefault(l => l.Id == loanApplicationId);
        //if (loan != null)
        //{
        //    loan.RemainingBalance = CalculateNewPayable(loan.PrincipalAmount, loan.InterestAmount, paymentAmount);

        //    // If all EMIs are paid, take the last EMI's balance before full payment
        //    if (loan.RemainingBalance == 0 && emis.Any())
        //    {
        //        loan.RemainingBalance = emis.Last().Balance;
        //    }
        //}
        //loanApplicationSet.Update(loan);

        Context.SaveChanges();
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



    #endregion
}
