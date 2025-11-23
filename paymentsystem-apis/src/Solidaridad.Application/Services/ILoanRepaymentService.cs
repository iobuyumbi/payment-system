using Solidaridad.Application.Models.LoanRepayment;
using Solidaridad.Core.Entities;
using Solidaridad.Core.Entities.Loans;

namespace Solidaridad.Application.Services;

public interface ILoanRepaymentService
{
    void ApplyPayment(Guid loanApplicationId, decimal amount, string paymentMode, string referenceNumber);

    Task<CreateLoanRepaymentResponseModel> CreateAsync(CreateLoanRepaymentModel createLoanRepaymentModel);

    List<LoanRepaymentResponseModel> GetRepaymentHistory(Guid loanApplicationId);

    Task<List<LoanTransaction>> SimulateTransaction(Guid loanApplicationId);

    Task<string> GeneratePaymentSchedule(Guid loanApplicationId);
    Task<List<Guid>> GenerateLoanStatement(Guid loanApplicationId);
    Task<List<LoanStatementResponseModel>> GetApplicationStatementHistory(Guid loanApplicationId);
    Task<byte[]> GenerateLoanStatementPdf(Guid loanApplicationId);
    Task<Guid> GenerateMonthlyLoanStatement(Guid loanApplicationId);
    Task<Guid?> GenerateLatestPaymentBasedLoanStatement(Guid loanApplicationId, decimal farmerEarningsLC, string refernceId);
}
