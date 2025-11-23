using Solidaridad.Core.Common;

namespace Solidaridad.Core.Entities;

public class BatchAttachmentMapping : BaseEntity
{
    public Guid LoanBatchId { get; set; }
    public Guid AttachmentId { get; set; }

}
