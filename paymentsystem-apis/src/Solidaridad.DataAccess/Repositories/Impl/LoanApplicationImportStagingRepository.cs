using Solidaridad.Core.Entities.Loans;
using Solidaridad.DataAccess.Persistence;

namespace Solidaridad.DataAccess.Repositories.Impl;

public class LoanApplicationImportStagingRepository : BaseRepository<LoanApplicationImportStaging>, ILoanApplicationImportStagingRepository
{
    public LoanApplicationImportStagingRepository(DatabaseContext context) : base(context)
    {
    }
}
