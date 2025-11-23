using Solidaridad.Core.Entities.Loans;
using Solidaridad.DataAccess.Persistence;

namespace Solidaridad.DataAccess.Repositories.Impl;

public class LoanBatchItemRepository : BaseRepository<LoanBatchItem>, ILoanBatchItemRepository
{
    public LoanBatchItemRepository(DatabaseContext context) : base(context) { }
}
