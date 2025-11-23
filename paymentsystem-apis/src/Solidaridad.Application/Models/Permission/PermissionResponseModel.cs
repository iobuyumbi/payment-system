namespace Solidaridad.Application.Models.Permission;

public class PermissionResponseModel : BaseResponseModel
{
    

    public string ModuleName { get; set; }
    public string PermissionName { get; set; }

    public string PermissionDescription { get; set; }

    public bool Selected { get; set; }
}

public class PermissionViewModel
{
    public string RoleId { get; set; }
    public IList<RoleClaimsViewModel> RoleClaims { get; set; }
}
public class RoleClaimsViewModel
{
    public string Type { get; set; }
    public string Value { get; set; }
    public bool Selected { get; set; }
}
