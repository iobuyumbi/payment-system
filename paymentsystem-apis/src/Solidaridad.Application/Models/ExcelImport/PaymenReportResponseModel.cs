using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solidaridad.Application.Models.ExcelImport;

public class PaymenReportResponseModel
{
    public PaymentDeductibleStatsModel PaymentDeductibleStats { get; set; } 
    public IEnumerable<PaymentRequestDeductibleModel> PaymentRequestDeductibleModels { get; set; }
}
