using Solidaridad.Core.Entities;
using Solidaridad.DataAccess.Persistence;

namespace Solidaridad.DataAccess.Repositories.Impl
{
    public class ProjectRepository : BaseRepository<Project> , IProjectRepository
    {
        public ProjectRepository(DatabaseContext context) : base(context){}
    }
}
