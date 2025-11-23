using Solidaridad.Core.Common;
using System.ComponentModel.DataAnnotations;

namespace Solidaridad.Core.Entities;

public class PaymentBatchHistory : BaseEntity, IAuditedEntity
{
    public Guid StageId { get; set; }

    public Guid PaymentBatchId { get; set; }

    [StringLength(250)] public string Action { get; set; }

    [StringLength(1000)] public string Comments { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; }

    public Guid? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }
}
