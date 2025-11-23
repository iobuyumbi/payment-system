using Solidaridad.Core.Entities;
using Solidaridad.DataAccess.Persistence;

namespace Solidaridad.DataAccess.Repositories.Impl;

public class CountyRepository : BaseRepository<County>, ICountyRepository
{
    public CountyRepository(DatabaseContext context) : base(context)
    {
    }
}
