using Microsoft.AspNetCore.Identity;

namespace Solidaridad.DataAccess.Identity;

public class ApplicationRole : IdentityRole
{
    public ApplicationRole(string name) : base(name)
    { }

    public ApplicationRole()
    { }

     public Guid? CountryId { get; set; }
}
