using LinqToDB.Mapping;
using Solidaridad.Core.Common;

namespace Solidaridad.Core.Entities;

[Table("Projects")]
public class Project : BaseEntity, IAuditedEntity
{
    public string ProjectName { get; set; }
    public Guid CountryId { get; set; }
    public Address Address { get; set; }
    public string ProjectCode { get; set; }
    public string Description { get; set; }
    public Guid CreatedBy { get ; set ; }
    public DateTime CreatedOn { get ; set ; }
    public Guid? UpdatedBy { get ; set ; }
    public DateTime? UpdatedOn { get ; set; }
}
