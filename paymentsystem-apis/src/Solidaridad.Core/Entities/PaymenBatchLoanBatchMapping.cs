using Solidaridad.Core.Common;

namespace Solidaridad.Core.Entities;

public class PaymenBatchLoanBatchMapping : BaseEntity
{
    public Guid PaymentBatchId { get; set; }

    public Guid LoanBatchId { get; set; }
}
