using Solidaridad.Core.Entities;
using Solidaridad.DataAccess.Persistence;

namespace Solidaridad.DataAccess.Repositories.Impl;

public class AdminLevel2Repository : BaseRepository<AdminLevel2>, IAdminLevel2Repository
{
    public AdminLevel2Repository(DatabaseContext context) : base(context)
    {
    }
}
