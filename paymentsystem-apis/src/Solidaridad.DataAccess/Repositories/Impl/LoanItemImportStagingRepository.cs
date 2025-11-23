using Solidaridad.Core.Entities.Loans;
using Solidaridad.DataAccess.Persistence;

namespace Solidaridad.DataAccess.Repositories.Impl;

public class LoanItemImportStagingRepository : BaseRepository<LoanItemImportStaging>, ILoanItemImportStagingRepository
{
    public LoanItemImportStagingRepository(DatabaseContext context) : base(context)
    {
    }
}
