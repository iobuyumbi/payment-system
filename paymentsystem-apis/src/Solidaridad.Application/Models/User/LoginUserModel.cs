using Solidaridad.Application.Models.Country;
using Solidaridad.Application.Models.Project;

namespace Solidaridad.Application.Models.User;

public class LoginUserModel
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}

public class UserResponseModel : BaseResponseModel
{

    public string Username { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public string RoleId { get; set; }
    public string RoleName { get; set; }
    public Guid ProjectId { get; set; }
    // public CountryResponseModel Country { get; set; }
    public ProjectResponseModel Project { get; set; }
    public Guid? ProjectManagerId { get; set; }
    // public Guid? CountryId { get; internal set; }
    //public List<CountryResponseModel> UserCountries { get; set; }
    public string UserCountriesStr { get; set; }
    public bool IsDeleted { get; set; } = false;
    public bool IsLoginEnabled { get; set; }
    public bool IsActive { get; set; }
}

public class LoginResponseModel
{
    public string Username { get; set; }

    public string Email { get; set; }

    public string api_token { get; set; }

    public string refresh_token { get; set; }

    public string token_type { get; set; }

    public TimeSpan expire_duration { get; internal set; }

    public IEnumerable<string> Permissions { get; internal set; }

    public List<string> Roles { get; internal set; }

    public IEnumerable<CountryResponseModel> Countries { get; set; }

    public bool RequiresPasswordChange { get; set; }

    public string UserId { get; set; }
}
