namespace Solidaridad.Application.Models.LoanItem;

public class CreateMasterLoanItemModel
{
    public string ItemName { get; set; }

    public string Description { get; set; }

    public Guid CategoryId { get; set; }
}
public class CreateMasterLoanItemResponseModel : BaseResponseModel { }
