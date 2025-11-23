using Solidaridad.Core.Entities;
using Solidaridad.Core.Entities.Loans;
using Solidaridad.DataAccess.Persistence;

namespace Solidaridad.DataAccess.Repositories.Impl;

public class PaymentDeductibleStatusMasterRepository : BaseRepository<PaymentDeductibleStatusMaster>, IPaymentDeductibleStatusMasterRepository
{
    public PaymentDeductibleStatusMasterRepository(DatabaseContext context) : base(context)
    {
    }
}
