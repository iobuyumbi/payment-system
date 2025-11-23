namespace Solidaridad.Application.Models.RolePermission;

public class UpdateRolePermissionModel
{
    public string PermissionName { get; set; }

    public string ModuleName { get; set; }

    public bool Selected { get; set; }
}

public class UpdateRolePermissionResponseModel : BaseResponseModel {
    public string Message { get; set; }
}
