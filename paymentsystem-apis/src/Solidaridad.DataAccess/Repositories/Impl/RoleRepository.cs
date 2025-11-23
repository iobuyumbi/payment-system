using Solidaridad.Core.Entities;
using Solidaridad.DataAccess.Persistence;

namespace Solidaridad.DataAccess.Repositories.Impl;

public class RoleRepository :  BaseRepository<Role> , IRoleRepository
{
    public RoleRepository(DatabaseContext context) : base(context) { }
}
