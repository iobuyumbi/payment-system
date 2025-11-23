using Solidaridad.Core.Entities;
using Solidaridad.DataAccess.Persistence;

namespace Solidaridad.DataAccess.Repositories.Impl;

public class AdminLevel1Repository : BaseRepository<AdminLevel1>, IAdminLevel1Repository
{
    public AdminLevel1Repository(DatabaseContext context) : base(context)
    {
    }
}
