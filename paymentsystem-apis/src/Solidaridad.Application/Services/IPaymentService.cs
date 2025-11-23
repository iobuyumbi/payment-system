using Microsoft.AspNetCore.Http;
using Solidaridad.Application.Models.ExcelImport;
using Solidaridad.Core.Entities.Loans;

namespace Solidaridad.Application.Services;

public interface IPaymentService
{
    Task<List<LoanInterest>> CalculateLoanInterestAsync(Guid loanApplicationId, DateTime currentDate);
    Task<List<LoanInterest>> CalculateLoanInterestSingleMonthAsync(Guid loanApplicationId);
    Task ImportPaymentRequestDeductible(IFormFile file, Guid? id,Guid batchId);
    Task ImportPaymentRequestDeductibleMultiBatch(IFormFile file, Guid? id, Guid batchId);
    Task ImportPaymentRequestFacilitation(IFormFile file, Guid? id, Guid batchId);
}
