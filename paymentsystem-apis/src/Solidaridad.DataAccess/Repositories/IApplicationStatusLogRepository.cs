using Solidaridad.Core.Entities.Loans;
using System.Linq.Expressions;

namespace Solidaridad.DataAccess.Repositories;

public interface IApplicationStatusLogRepository : IBaseRepository<ApplicationStatusLog>
{
    Task<List<ApplicationStatusLog>> GetFull(Expression<Func<ApplicationStatusLog, bool>> predicate);
}

