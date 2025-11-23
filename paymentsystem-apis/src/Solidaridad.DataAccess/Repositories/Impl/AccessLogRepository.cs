using Solidaridad.Core.Entities;
using Solidaridad.DataAccess.Persistence;

namespace Solidaridad.DataAccess.Repositories.Impl;

public class AccessLogRepository : BaseRepository<AccessLog>, IAccessLogRepository
{
    public AccessLogRepository(DatabaseContext context) : base(context)
    {
    }
}
