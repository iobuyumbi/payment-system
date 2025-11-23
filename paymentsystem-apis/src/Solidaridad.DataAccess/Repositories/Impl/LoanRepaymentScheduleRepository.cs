using Solidaridad.Core.Entities.Loans;
using Solidaridad.DataAccess.Persistence;

namespace Solidaridad.DataAccess.Repositories.Impl;

public class LoanRepaymentScheduleRepository : BaseRepository<LoanRepaymentSchedule>, ILoanRepaymentScheduleRepository
{
    public LoanRepaymentScheduleRepository(DatabaseContext context) : base(context)
    {
    }
}

