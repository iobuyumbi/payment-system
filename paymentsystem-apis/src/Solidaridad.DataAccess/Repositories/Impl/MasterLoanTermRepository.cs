using Solidaridad.Core.Entities.Loans;
using Solidaridad.DataAccess.Persistence;

namespace Solidaridad.DataAccess.Repositories.Impl;

public class MasterLoanTermRepository : BaseRepository<MasterLoanTerm>, IMasterLoanTermRepository
{
    public MasterLoanTermRepository(DatabaseContext context) : base(context)
    {
    }
}
