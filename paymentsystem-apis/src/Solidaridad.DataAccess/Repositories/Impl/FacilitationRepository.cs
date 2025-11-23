using Solidaridad.Core.Entities;
using Solidaridad.Core.Entities.Payments;
using Solidaridad.DataAccess.Persistence;

namespace Solidaridad.DataAccess.Repositories.Impl;

internal class FacilitationRepository : BaseRepository<PaymentRequestFacilitation>, IFacilitationRepository
{
    public FacilitationRepository(DatabaseContext context) : base(context)
    {
    }
}
