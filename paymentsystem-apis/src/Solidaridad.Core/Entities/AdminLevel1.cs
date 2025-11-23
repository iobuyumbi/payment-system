using Solidaridad.Core.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace Solidaridad.Core.Entities;

[Table("AdminLevel1")]
public class AdminLevel1 : BaseEntity
{
    public string CountyName { get; set; }
    public string? CountyCode { get; set; }
    public Guid CountryId { get; set; }

    public bool? IsActive { get; set; }
    public virtual Country Country { get; set; }
}
