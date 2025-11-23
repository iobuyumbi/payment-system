namespace Solidaridad.Application.Models.PaymentBatch;

public class CreatePaymentBatchModel
{
    public string BatchName { get; set; }

    public DateTime DateCreated { get; set; }

    public List<SelectItemModel> LoanBatchIds { get; set; }

    public List<SelectItemModel> ProjectIds { get; set; }

    public string CountryId { get; set; }

    public int PaymentModule { get; set; }

    public Guid StatusId { get; set; }
}
public class CreatePaymentBatchResponseModel : BaseResponseModel { }
