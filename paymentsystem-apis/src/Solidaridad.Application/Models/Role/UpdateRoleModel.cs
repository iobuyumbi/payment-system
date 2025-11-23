namespace Solidaridad.Application.Models.Role;

public class UpdateRoleModel
{
    public string Name { get; set; }
    public List<Guid> CountryIds { get; set; }

}

public class UpdateRoleResponseModel : BaseResponseModel { }
