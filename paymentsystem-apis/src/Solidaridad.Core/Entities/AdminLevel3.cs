using Solidaridad.Core.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace Solidaridad.Core.Entities
{
    [Table("AdminLevel3")]
    public class AdminLevel3 : BaseEntity
    {
        public string WardName { get; set; }
        public string? WardCode { get; set; }
        public Guid? SubCountyId{ get; set; }

        public bool? IsActive { get; set; }
        public virtual AdminLevel2 SubCounty { get; set; }
    }
}
