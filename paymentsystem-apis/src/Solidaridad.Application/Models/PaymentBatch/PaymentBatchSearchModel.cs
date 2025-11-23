using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solidaridad.Application.Models.PaymentBatch;

public class PaymentBatchSearchModel
{
    public PaymentBatchResponseModel PaymentBatch { get; set; }
    public PaymentStatsResponseModel PaymentStats { get; set; }

}
