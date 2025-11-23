using Solidaridad.Core.Entities;
using Solidaridad.DataAccess.Persistence;

namespace Solidaridad.DataAccess.Repositories.Impl;

public class ActivityLogRepository : BaseRepository<ActivityLog>, IActivityLogRepository
{
    public ActivityLogRepository(DatabaseContext context) : base(context) { }
}
