using Solidaridad.Core.Common;

namespace Solidaridad.Core.Entities;

public class Permission : BaseEntity
{
    public string PermissionName { get; set; }
    public Guid ModuleId { get; set; }
    public string Description { get; set; }
}

