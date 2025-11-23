using Solidaridad.Core.Entities;
using Solidaridad.Core.Entities.Payments;
using Solidaridad.DataAccess.Persistence;

namespace Solidaridad.DataAccess.Repositories.Impl;

public class AssociateRepository : BaseRepository<AssociateMap>, IAssociateRepository
{
    public AssociateRepository(DatabaseContext context) : base(context)
    {
    }
}
