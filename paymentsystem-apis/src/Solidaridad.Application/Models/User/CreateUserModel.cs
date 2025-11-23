namespace Solidaridad.Application.Models.User;

public class CreateUserModel
{
    public string Username { get; set; }

    public string Email { get; set; }

    public string PhoneNumber { get; set; }

    public string Password { get; set; }

    public  List<Guid> CountryIds { get; set; }

    public Guid ProjectId { get; set; }

    public Guid? ProjectManagerId { get; set; }

    public List<string> RoleNames { get; set; }

    public bool IsLoginEnabled { get; set; } = true;
}

public class CreateUserResponseModel : BaseResponseModel { }
