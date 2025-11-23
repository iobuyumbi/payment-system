using Solidaridad.Core.Entities;
using Solidaridad.DataAccess.Persistence;

namespace Solidaridad.DataAccess.Repositories.Impl;

public class ConstituencyRepository : BaseRepository<Constituency>, IConstituencyRepository
{
    public ConstituencyRepository(DatabaseContext context) : base(context)
    {
    }
}
