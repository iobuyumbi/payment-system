using Solidaridad.Core.Entities;
using Solidaridad.Core.Entities.Loans;
using Solidaridad.DataAccess.Persistence;

namespace Solidaridad.DataAccess.Repositories.Impl;

public class LoanProcessingFeeRepository : BaseRepository<MasterLoanTermAdditionalFee>, ILoanProcessingFeeRepository
{
    public LoanProcessingFeeRepository(DatabaseContext context) : base(context) { }
}
