namespace Solidaridad.Application.Models.LoanItem;

public class UpdateMasterLoanItemModel
{
    public string ItemName { get; set; }
    public string? Description { get; set; }
    public Guid CategoryId { get; set; }
    public float? Cost { get; set; }
}
public class UpdateMasterLoanItemResponseModel : BaseResponseModel { }
