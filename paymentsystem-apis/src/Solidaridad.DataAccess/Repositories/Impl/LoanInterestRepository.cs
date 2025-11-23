using Solidaridad.Core.Entities.Loans;
using Solidaridad.DataAccess.Persistence;

namespace Solidaridad.DataAccess.Repositories.Impl;

public class LoanInterestRepository: BaseRepository<LoanInterest>, ILoanInterestRepository
{
    public LoanInterestRepository(DatabaseContext context) : base(context) { }
}
