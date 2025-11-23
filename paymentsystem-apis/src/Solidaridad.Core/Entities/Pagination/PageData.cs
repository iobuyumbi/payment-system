using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solidaridad.Core.Entities.Pagination
{
    public class PagedData<T>
    {
        public T Result { get; set; }
        public Page Page { get; set; }

        public PagedData()
        {
            Page = new Page();
        }
    }
}
