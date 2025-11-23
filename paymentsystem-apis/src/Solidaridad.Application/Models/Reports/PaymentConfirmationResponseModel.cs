using Solidaridad.Application.Models.PaymentBatch;
using Solidaridad.Application.Models.PaymentDeductible;

namespace Solidaridad.Application.Models.Reports;

public class PaymentConfirmationResponseModel
{
    public IEnumerable<PaymentDeductibleResponseModel> PaymentDeductibles { get; set; }
    public PaymentStatusResponseModel PaymentStatus { get; set; }

}
