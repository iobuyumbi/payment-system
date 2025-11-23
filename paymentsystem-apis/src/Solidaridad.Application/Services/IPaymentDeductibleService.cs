using Solidaridad.Application.Models;
using Solidaridad.Application.Models.ExcelImport;
using Solidaridad.Application.Models.PaymentDeductible;
using Solidaridad.Core.Entities.Base;

namespace Solidaridad.Application.Services;

public interface IPaymentDeductibleService
{
    Task<CreatePaymentDeductibleResponseModel> CreateAsync(CreatePaymentDeductibleModel createLoanTermModel);

    Task<BaseResponseModel> DeleteAsync(Guid id);

    Task<IEnumerable<PaymentDeductibleResponseModel>> GetAllAsync(DeductibleSearchParams searchParams);
    Task<IEnumerable<PaymentRequestDeductibleModel>> GetAllForReportAsync();

    Task<PaymentDeductibleResponseModel> GetByIdAsync(Guid id);
    
    Task<PaymentStats> GetPaymentBatchStats(Guid paymentBatchId);
    
    Task<UpdatePaymentDeductibleResponseModel> UpdateAsync(Guid id, UpdatePaymentDeductibleModel updateLoanTermModel);
    Task<IEnumerable<PaymentSummaryResponseModel>> GetAllPaymentsData();
    Task<PaymentStats> GetFacilitationPaymentBatchStats(Guid paymentBatchId);
}
