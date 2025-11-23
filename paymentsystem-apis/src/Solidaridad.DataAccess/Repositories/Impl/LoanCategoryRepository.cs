using Solidaridad.Core.Entities.Loans;
using Solidaridad.DataAccess.Persistence;

namespace Solidaridad.DataAccess.Repositories.Impl;

public class LoanCategoryRepository : BaseRepository<LoanCategory>, ILoanCategoryRepository
{
    public LoanCategoryRepository(DatabaseContext context) : base(context)
{
}
}
