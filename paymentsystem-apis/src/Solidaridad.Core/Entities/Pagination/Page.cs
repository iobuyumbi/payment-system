using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solidaridad.Core.Entities.Pagination
{
    public class Page
    {
        public int Size { get; set; }
        //The total number of elements
        public int TotalElements { get; set; }
        //The total number of pages
        public int TotalPages { get; set; }
        //The current page number
        public int PageNumber { get; set; }
    }
}
