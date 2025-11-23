using Solidaridad.Core.Entities.Loans;

namespace Solidaridad.DataAccess.Repositories;

public interface ILoanRepaymentRepository : IBaseRepository<LoanRepayment>
{
    // Task<LoanRepayment> AddRepayment(Guid loanApplicationId, decimal repaymentAmount, decimal annualInterestRate);

    void ApplyPayment(Guid loanApplicationId, decimal paymentAmount, string paymentMode, string referenceNumber);

    List<LoanRepayment> GetRepaymentHistory(Guid loanApplicationId);
}
