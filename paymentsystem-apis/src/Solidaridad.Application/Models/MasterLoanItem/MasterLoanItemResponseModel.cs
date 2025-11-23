using Solidaridad.Application.Models.ItemCategory;

namespace Solidaridad.Application.Models.LoanItem;

public class MasterLoanItemResponseModel : BaseResponseModel
{
    public string ItemName { get; set; }

    public string Description { get; set; }

    public ItemCategoryResponseModel Category { get; set; }

    public Guid CategoryId { get; set; }

    public DateTime CreatedOn { get; set; }

    public string CreatedBy { get; set; }
}

