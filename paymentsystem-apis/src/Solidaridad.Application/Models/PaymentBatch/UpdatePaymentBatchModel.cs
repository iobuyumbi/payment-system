namespace Solidaridad.Application.Models.PaymentBatch;

public class UpdatePaymentBatchModel
{
    public string BatchName { get; set; }

    public DateTime DateCreated { get; set; }

    public Guid LoanBatchId { get; set; }
    // public Guid ProjectId { get; set; }
    public Guid CountryId { get; set; }
    public int PaymentModule { get; set; }
    public Guid StatusId { get; set; }

}
public class UpdatePaymentBatchResponseModel : BaseResponseModel { }
