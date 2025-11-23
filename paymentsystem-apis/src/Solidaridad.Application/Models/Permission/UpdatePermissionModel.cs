namespace Solidaridad.Application.Models.Permission;

public class UpdatePermissionModel
{
    public Guid DesignationId { get; set; }

    public string PermissionName { get; set; }
}

public class UpdatePermissionResponseModel : BaseResponseModel { }

