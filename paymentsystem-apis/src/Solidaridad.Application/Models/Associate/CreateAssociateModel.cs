namespace Solidaridad.Application.Models.Associate;

public class CreateAssociateModel
{
    public Guid FarmerId { get; set; }
    public Guid PaymentBatchId { get; set; }
}
public class CreateAssociateResponseModel : BaseResponseModel { }
