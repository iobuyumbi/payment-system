namespace Solidaridad.Application.Models.Role;

public class CreateRoleModel
{
    public string Name { get; set; }
    public List<Guid> CountryIds { get; set; }
}
public class CreateRoleResponseModel : BaseResponseModel { }
