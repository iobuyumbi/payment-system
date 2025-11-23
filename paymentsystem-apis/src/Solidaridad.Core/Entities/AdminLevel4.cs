using Solidaridad.Core.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace Solidaridad.Core.Entities;
[Table("AdminLevel4")]
public class AdminLevel4 : BaseEntity
{
    public string VillageName { get; set; }
    public string? VillageCode { get; set; }
    public Guid? WardId{ get; set; }
    public bool? IsActive { get; set; }
    public virtual AdminLevel3 Ward { get; set; }
}
