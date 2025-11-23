namespace Solidaridad.Application.Models.Associate;

public class UpdateAssociateModel
{
    public Guid FarmerId { get; set; }
    public Guid PaymentBatchId { get; set; }
}
public class UpdateAssociateResponseModel : BaseResponseModel { }
