using Microsoft.EntityFrameworkCore;
using Solidaridad.Core.Entities.Payments;
using Solidaridad.DataAccess.Persistence;
using System.Linq.Expressions;

namespace Solidaridad.DataAccess.Repositories.Impl;

public class PaymentRequestDeductibleRepository : BaseRepository<PaymentRequestDeductible>, IPaymentRequestDeductibleRepository
{
    #region DI
    protected readonly DbSet<PaymentImportSummary> _paymentImportSummary;

    public PaymentRequestDeductibleRepository(DatabaseContext context) : base(context)
    {
        _paymentImportSummary = context.Set<PaymentImportSummary>();
    }
    #endregion

    #region Methods
    public async Task SavePaymentImportSummary(PaymentImportSummary paymentImportSummary)
    {
        await _paymentImportSummary.AddAsync(paymentImportSummary);
    }

    public async Task<IEnumerable<PaymentImportSummary>> GetPaymentImportSummary(Expression<Func<PaymentImportSummary, bool>> predicate)
    {
        return await _paymentImportSummary.Where(predicate).ToListAsync();
    }
    #endregion
}
