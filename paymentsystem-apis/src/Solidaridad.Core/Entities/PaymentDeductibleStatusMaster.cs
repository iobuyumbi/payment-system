using Solidaridad.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solidaridad.Core.Entities;

public class PaymentDeductibleStatusMaster : BaseEntity
{
    public string Name { get; set; }
    public bool IsDeleted { get; set; }
}
