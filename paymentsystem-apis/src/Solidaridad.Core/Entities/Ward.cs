using Solidaridad.Core.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solidaridad.Core.Entities
{
    [Table("Wards")]
    public class Ward : BaseEntity
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public bool IsActive { get; set; }
        public Guid CostituencyId { get; set; }
    }
}
