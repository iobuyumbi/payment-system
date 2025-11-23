using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solidaridad.Application.Models.Reports;

public class RepaymentReportsResponseModel
{
    public string Month { get; set; }
    public decimal RepaymentAmount { get; set; }
    //public decimal AccuredAmount { get; set; }
}
