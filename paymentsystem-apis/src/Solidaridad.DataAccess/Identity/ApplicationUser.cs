using Microsoft.AspNetCore.Identity;
using Solidaridad.Core.Entities;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Solidaridad.DataAccess.Identity;

public class ApplicationUser : IdentityUser
{
    public Guid ProjectId { get; set; }

    public Guid? ProjectManagerId { get; set; }

    public ICollection<UserCountry> UserCountries { get; set; }

    public bool ForcePasswordChange { get; set; }

    [MaxLength(20)]
    public string Source { get; set; }

    [DefaultValue(true)]
    public bool IsLoginEnabled { get; set; } = true;

    [DefaultValue(true)]
    public bool IsActive { get; set; } = true;
}
