
namespace Solidaridad.Application.Models.User;

public class UpdateUserModel
{
    public string Username { get; set; }

    public string PhoneNumber { get; set; }

    public string Email { get; set; }

    public string Password { get; set; }

    public Guid CountryId { get; set; }

    public Guid ProjectId { get; set; }

    public Guid? ProjectManagerId { get; set; }

    public List<string> RoleNames { get; set; }

    public List<Guid> CountryIds { get; set; }

    public bool IsLoginEnabled { get; set; }

    public bool IsActive { get; set; }
}

public class UpdateUserResponseModel : BaseResponseModel { }
