namespace Solidaridad.Application.Models.LoanRepayment;

public class LoanRepaymentResponseModel
{
    public Guid LoanApplicationId { get; set; }

    public decimal AmountPaid { get; set; }

    public DateTime PaymentDate { get; set; }

    public string PaymentMode { get; set; }

    public string ReferenceNumber { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; }

    public Guid? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

}
