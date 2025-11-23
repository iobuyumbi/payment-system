using Solidaridad.Core.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace Solidaridad.Core.Entities;
[Table("AdminLevel2")]
public class AdminLevel2 : BaseEntity
{
    public string SubCountyName { get; set; }
    public string? SubCountyCode { get; set; }
    public Guid? CountyId{ get; set; }

    public bool? IsActive { get; set; }
    public virtual AdminLevel1 County { get; set; }
}
