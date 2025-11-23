using Solidaridad.Core.Entities;
using Solidaridad.Core.Entities.Loans;
using Solidaridad.DataAccess.Persistence;

namespace Solidaridad.DataAccess.Repositories.Impl;

public class ApplicationStatusRepository : BaseRepository<ApplicationStatus>, IApplicationStatusRepository
{
    public ApplicationStatusRepository(DatabaseContext context) : base(context)
    {
    }
}
