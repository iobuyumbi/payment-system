using Solidaridad.Core.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace Solidaridad.Core.Entities.Loans;

[Table("MasterLoanItems")]
public class MasterLoanItem : BaseEntity, IAuditedEntity
{
    public string ItemName { get; set; }

    public string Description { get; set; }

    public string Unit { get; set; }

    public Guid CategoryId { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; }

    public Guid? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public ItemCategory Category { get; set; }
}
