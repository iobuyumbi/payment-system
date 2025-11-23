using Solidaridad.Core.Entities;
using Solidaridad.DataAccess.Persistence;

namespace Solidaridad.DataAccess.Repositories.Impl;

public class ModuleRepository : BaseRepository<Module> , IModuleRepository
{
    public ModuleRepository(DatabaseContext context) : base(context) { }
}
