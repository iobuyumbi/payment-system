using Solidaridad.Core.Entities.Locations;
using Solidaridad.DataAccess.Persistence;

namespace Solidaridad.DataAccess.Repositories.Impl;

public class LocationRepository : BaseRepository<Location>, ILocationRepository
{
    public LocationRepository(DatabaseContext context) : base(context)
    {
    }
}

public class LocationProfileRepository : BaseRepository<LocationProfile>, ILocationProfileRepository
{
    public LocationProfileRepository(DatabaseContext context) : base(context)
    {
    }
}
