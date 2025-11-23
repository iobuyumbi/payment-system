using Solidaridad.Core.Entities;
using Solidaridad.DataAccess.Persistence;

namespace Solidaridad.DataAccess.Repositories.Impl;

public class CountryRepository : BaseRepository<Country>, ICountryRepository
{
    public CountryRepository(DatabaseContext context) : base(context)
    {
    }
}
