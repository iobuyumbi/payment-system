using Solidaridad.Core.Common;

namespace Solidaridad.Core.Entities;

public class RolePermission : BaseEntity, IAuditedEntity
{
    public Guid RoleId { get; set; }

    public Guid PermissionId { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; }

    public Guid? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public virtual Permission Permission { get; set; }
}
