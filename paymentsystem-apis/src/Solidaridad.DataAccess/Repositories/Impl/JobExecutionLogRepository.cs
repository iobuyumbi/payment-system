using Solidaridad.Core.Entities;
using Solidaridad.DataAccess.Persistence;

namespace Solidaridad.DataAccess.Repositories.Impl;

public class JobExecutionLogRepository : BaseRepository<JobExecutionLog>, IJobExecutionLogRepository
{
    public JobExecutionLogRepository(DatabaseContext context) : base(context) { }
}
