using Solidaridad.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solidaridad.Core.Entities
{
    public class Role : BaseEntity
    {
        public string Name { get; set; }
        public string Slug { get; set; }
    }
}
