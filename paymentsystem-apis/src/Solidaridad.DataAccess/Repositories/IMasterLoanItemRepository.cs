using Solidaridad.Core.Entities.Loans;
using System.Linq.Expressions;

namespace Solidaridad.DataAccess.Repositories;

public interface IMasterLoanItemRepository : IBaseRepository<MasterLoanItem>
{
    Task<IEnumerable<MasterLoanItem>> GetFullAsync(Expression<Func<MasterLoanItem, bool>> predicate);
}
