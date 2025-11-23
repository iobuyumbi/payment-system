using Solidaridad.Core.Entities;
using Solidaridad.Core.Entities.Payments;
using System.Linq.Expressions;

namespace Solidaridad.DataAccess.Repositories;

public interface IPaymentBatchRepository : IBaseRepository<PaymentBatch>
{
    Task AddPaymentHistory(PaymentBatchHistory paymentBatchHistory);

    object GetByProjectIds(List<Guid> projectIds);
    Task<IEnumerable<PaymentBatch>> GetFullAsync(Expression<Func<PaymentBatch, bool>> predicate);
    Task<IEnumerable<MasterPaymentApprovalStage>> GetPaymentApprovalStages();

    Task<IEnumerable<PaymentBatchHistory>> GetPaymentHistory(Guid paymentBatchId);
}

public interface IPaymenBatchLoanBatchMappingRepository : IBaseRepository<PaymenBatchLoanBatchMapping> { }

public interface IPaymentBatchProjectMappingRepository : IBaseRepository<PaymentBatchProjectMapping> { }

