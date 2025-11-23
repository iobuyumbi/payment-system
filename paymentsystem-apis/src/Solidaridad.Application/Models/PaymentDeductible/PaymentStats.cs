namespace Solidaridad.Application.Models.PaymentDeductible;

public class PaymentStats
{
    public decimal TotalAmount { get; set; }
    public int BeneficiaryCount { get; set; }

}

public class PaymentStatusResponseModel
{
    public int TotalPayment { get; set; }
    public int CompletedPayments { get; set; }
    public int PendingPayments { get; set; }

}

