using Solidaridad.Core.Common;


namespace Solidaridad.Core.Entities;

public class PaymentBatchProjectMapping : BaseEntity
{
        public Guid PaymentBatchId { get; set; }

        public Guid ProjectId { get; set; }
}
