using Microsoft.EntityFrameworkCore;
using Solidaridad.Core.Entities.Loans;
using Solidaridad.DataAccess.Persistence;
using System.Linq.Expressions;

namespace Solidaridad.DataAccess.Repositories.Impl;

public class ApplicationStatusLogRepository : BaseRepository<ApplicationStatusLog>, IApplicationStatusLogRepository
{
    protected readonly DbSet<ApplicationStatusLog> ApplicationStatusLogSet;
    public ApplicationStatusLogRepository(DatabaseContext context) : base(context)
    {
        ApplicationStatusLogSet = context.Set<ApplicationStatusLog>();
    }

    public async Task<List<ApplicationStatusLog>> GetFull(Expression<Func<ApplicationStatusLog, bool>> predicate)
    {
        return await ApplicationStatusLogSet
                                 .Where(predicate)
                                 .Include(c => c.ApplicationStatus)
                                 .ToListAsync();
    }
}
