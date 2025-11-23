using Solidaridad.Core.Entities;
using Solidaridad.DataAccess.Persistence;

namespace Solidaridad.DataAccess.Repositories.Impl;

public class AdminLevel3Repository : BaseRepository<AdminLevel3>, IAdminLevel3Repository
{
    public AdminLevel3Repository(DatabaseContext context) : base(context)
    {
    }
}
