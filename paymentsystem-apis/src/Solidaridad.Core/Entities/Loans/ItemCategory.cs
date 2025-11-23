using Solidaridad.Core.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace Solidaridad.Core.Entities.Loans;

[Table("ItemCategories")]
public class ItemCategory : BaseEntity, IAuditedEntity
{
    public string Name { get; set; }

    public string? Description { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; }

    public Guid? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }
}
