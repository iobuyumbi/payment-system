using Solidaridad.Core.Entities;
using Solidaridad.DataAccess.Persistence;

namespace Solidaridad.DataAccess.Repositories.Impl;

public class ApiRequestLogRepository : BaseRepository<ApiRequestLog>, IApiRequestLogRepository
{
    public ApiRequestLogRepository(DatabaseContext context) : base(context) { }
}
