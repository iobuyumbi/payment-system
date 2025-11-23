using Solidaridad.Core.Common;

namespace Solidaridad.Core.Entities;

public class UserCountry : BaseEntity
{
    public string UserId { get; set; }
    public Guid CountryId { get; set; }
    public Country Country { get; set; }
}
