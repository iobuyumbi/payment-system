namespace Solidaridad.Application.Models.ExcelImport;

public class PaymentDeductibleStatsModel
{
    public int TotalBeneficiaries { get; set; }
    public float TotalAmount { get; set; }
    public int SuccessfulTransactions { get; set; }
    public float TotalLoanDeductions {  get; set; }
    public float TotalPaymentCost {  get; set; }
    public float FailedTransactions { get; set; }
    public int TotalPayment {  get; set; }
  
}
