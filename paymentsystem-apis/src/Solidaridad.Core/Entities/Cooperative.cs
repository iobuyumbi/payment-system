using LinqToDB.Mapping;
using Solidaridad.Core.Common;

namespace Solidaridad.Core.Entities;

[Table("Cooperatives")]
public class Cooperative : BaseEntity, IAuditedEntity
{
    public string Name { get; set; }

    public Guid CountryId { get; set; }

    public string Description { get; set; }

    public virtual Country Country { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; }

    public Guid? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }
}
