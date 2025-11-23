using Solidaridad.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solidaridad.Core.Entities.Loans;

public class LoanItemMapping : BaseEntity
{
    public Guid ItemId { get; set; }
    public Guid ApplicationId { get; set; }
    public float Quantity { get; set; }
}
