namespace Solidaridad.Application.Models.LoanAttachmentModel;

public class UpdateAttachmentModel
{
    public Guid LoanApplicationId { get; set; }
    public Guid AttachmentId { get; set; }
}
public class UpdateAttachmentResponseModel : BaseResponseModel { }
