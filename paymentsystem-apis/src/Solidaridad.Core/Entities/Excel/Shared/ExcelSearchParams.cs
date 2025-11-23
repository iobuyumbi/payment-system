using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solidaridad.Core.Entities.Excel.Shared
{
    public class ExcelApiSearchParams
    {
        public ExcelApiSearchParams()
        {
            FileName = new List<string>();
            Date = new List<string>();
        }
        public virtual List<string> FileName { get; set; }
        public virtual List<string> Date { get; set; }
        public int ModuleId { get; set; }
        public string ModuleName { get; set; }
       
    }
}
