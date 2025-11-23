namespace Solidaridad.Application.Models.Disbursement;

public class CreateDisbursementModel
{
    public Guid FarmerId { get; set; }
    public Guid MethodId { get; set; }
    public decimal Amount { get; set; }
    public string CurrencyCode { get; set; }
    public DateTime Date { get; set; }
    public Guid StatusId { get; set; }

}
public class CreateDisbursementResponseModel : BaseResponseModel { }
