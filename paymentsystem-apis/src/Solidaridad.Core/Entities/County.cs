using Solidaridad.Core.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solidaridad.Core.Entities
{
    [Table("Counties")]
    public class County : BaseEntity
    {
       public string CountyName { get; set; }
       public string Code { get; set; }
       public bool IsActive { get; set; }
       public Guid CountryId { get; set; }
    }
}
