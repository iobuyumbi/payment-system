namespace Solidaridad.Application.Models.RolePermission;

public class RolePermissionResponseModel
{
    public Guid RoleId { get; set; }
    public int ActionId { get; set; }
    public bool IsAllowed { get; set; }
    public Guid ModuleId { get; set; }
}
