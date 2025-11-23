using Solidaridad.Core.Entities;
using Solidaridad.Core.Entities.Payments;
using Solidaridad.DataAccess.Persistence;

namespace Solidaridad.DataAccess.Repositories.Impl;

internal class DisbursementRepository : BaseRepository<Disbursement>, IDisbursementRepository
{
    public DisbursementRepository(DatabaseContext context) : base(context)
    {
    }
}
