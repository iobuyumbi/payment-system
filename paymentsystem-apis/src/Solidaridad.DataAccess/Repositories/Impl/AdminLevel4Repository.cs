using Solidaridad.Core.Entities;
using Solidaridad.DataAccess.Persistence;

namespace Solidaridad.DataAccess.Repositories.Impl;

public class AdminLevel4Repository : BaseRepository<AdminLevel4>, IAdminLevel4Repository
{
    public AdminLevel4Repository(DatabaseContext context) : base(context)
    {
    }
}
