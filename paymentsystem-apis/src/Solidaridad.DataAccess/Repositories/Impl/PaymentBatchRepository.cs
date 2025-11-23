using Microsoft.EntityFrameworkCore;
using Solidaridad.Core.Entities;
using Solidaridad.Core.Entities.Loans;
using Solidaridad.Core.Entities.Payments;
using Solidaridad.DataAccess.Persistence;
using System.Linq.Expressions;

namespace Solidaridad.DataAccess.Repositories.Impl;

public class PaymentBatchRepository : BaseRepository<PaymentBatch>, IPaymentBatchRepository
{
    protected readonly DbSet<PaymentBatch> PaymentBatchSet;
    protected readonly DbSet<LoanBatch> LoanBatchSet;
    protected readonly DbSet<PaymenBatchLoanBatchMapping> PaymenBatchLoanBatchMappingSet;
    protected readonly DbSet<MasterPaymentApprovalStage> PaymentStage;
    protected readonly DbSet<PaymentBatchHistory> PaymentBatchHistory;

    public PaymentBatchRepository(DatabaseContext context) : base(context)
    {
        PaymentBatchSet = context.Set<PaymentBatch>();
        LoanBatchSet = context.Set<LoanBatch>();
        PaymenBatchLoanBatchMappingSet = context.Set<PaymenBatchLoanBatchMapping>();
        PaymentStage = context.Set<MasterPaymentApprovalStage>();
        PaymentBatchHistory = context.Set<PaymentBatchHistory>();
    }

    public async Task<IEnumerable<PaymentBatch>> GetFullAsync(Expression<Func<PaymentBatch, bool>> predicate)
    {
         var result = await PaymentBatchSet.Where(predicate)
            .Include(o=>o.Status)
            .ToListAsync();

        return result;
    }

    public object GetByProjectIds(List<Guid> projectIds)
    {
        var resultLambda = LoanBatchSet
                        .Join(PaymenBatchLoanBatchMappingSet,
                                loanBatch => loanBatch.Id,
                                mapping => mapping.LoanBatchId,
                                (loanBatch, mapping) => new { loanBatch, mapping })
                        .Join(PaymentBatchSet,
                                combined => combined.mapping.PaymentBatchId,
                                paymentBatch => paymentBatch.Id,
                                (combined, paymentBatch) => new { combined.loanBatch, combined.mapping, paymentBatch })
                        .Where(x => projectIds.Contains(x.loanBatch.ProjectId))
                        .Select(x => new
                        {
                            LoanBatch = x.loanBatch,
                            PaymentBatch = x.paymentBatch,
                            Mapping = x.mapping
                        });

        // Execute the query to get the result
        return resultLambda.ToList();
    }

    public async Task<IEnumerable<MasterPaymentApprovalStage>> GetPaymentApprovalStages()
    {
        return await PaymentStage.Where(c => c.IsDeleted == false).ToListAsync();
    }

    public async Task AddPaymentHistory(PaymentBatchHistory paymentBatchHistory)
    {
        await PaymentBatchHistory.AddAsync(paymentBatchHistory);
    }

    public async Task<IEnumerable<PaymentBatchHistory>> GetPaymentHistory(Guid paymentBatchId)
    {
        var result = await PaymentBatchHistory.Where(c => c.PaymentBatchId == paymentBatchId && c.IsDeleted == false).ToListAsync();
        return result;
    }
}

public class PaymenBatchLoanBatchMappingRepository : BaseRepository<PaymenBatchLoanBatchMapping>, IPaymenBatchLoanBatchMappingRepository
{
    public PaymenBatchLoanBatchMappingRepository(DatabaseContext context) : base(context) { }
}
public class PaymentBatchProjectMappingRepository : BaseRepository<PaymentBatchProjectMapping>, IPaymentBatchProjectMappingRepository
{
    public PaymentBatchProjectMappingRepository(DatabaseContext context) : base(context) { }
}
