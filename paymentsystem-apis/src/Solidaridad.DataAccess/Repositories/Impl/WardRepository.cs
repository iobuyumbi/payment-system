using Solidaridad.Core.Entities;
using Solidaridad.DataAccess.Persistence;

namespace Solidaridad.DataAccess.Repositories.Impl;

public class WardRepository : BaseRepository<Ward>, IWardRepository
{
    public WardRepository(DatabaseContext context) : base(context)
    {
    }
}
