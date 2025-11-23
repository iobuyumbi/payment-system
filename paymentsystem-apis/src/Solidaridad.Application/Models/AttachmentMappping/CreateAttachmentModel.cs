namespace Solidaridad.Application.Models.LoanAttachmentModel;

public class CreateAttachmentModel
{
    public Guid LoanApplicationId { get; set; }
    public Guid AttachmentId { get; set; }
}
public class CreateAttachmentResponseModel : BaseResponseModel { }
