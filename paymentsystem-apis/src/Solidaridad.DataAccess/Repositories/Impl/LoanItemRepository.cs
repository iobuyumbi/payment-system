using Solidaridad.Core.Entities.Loans;
using Solidaridad.DataAccess.Persistence;

namespace Solidaridad.DataAccess.Repositories.Impl;

public class LoanItemRepository : BaseRepository<LoanItem>, ILoanItemRepository
{
    public LoanItemRepository(DatabaseContext context) : base(context) { }
}
