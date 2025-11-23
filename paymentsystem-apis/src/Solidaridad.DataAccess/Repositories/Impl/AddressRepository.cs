using Solidaridad.Core.Entities;
using Solidaridad.DataAccess.Persistence;

namespace Solidaridad.DataAccess.Repositories.Impl;

public class AddressRepository : BaseRepository<Address>, IAddressRepository
{
    public AddressRepository(DatabaseContext context) : base(context) { }
}
