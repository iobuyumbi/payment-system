using Solidaridad.Core.Entities.Payments;
using System.Linq.Expressions;

namespace Solidaridad.DataAccess.Repositories;

public interface IPaymentRequestDeductibleRepository : IBaseRepository<PaymentRequestDeductible>
{
    Task<IEnumerable<PaymentImportSummary>> GetPaymentImportSummary(Expression<Func<PaymentImportSummary, bool>> predicate);
    Task SavePaymentImportSummary(PaymentImportSummary paymentImportSummary);
}
