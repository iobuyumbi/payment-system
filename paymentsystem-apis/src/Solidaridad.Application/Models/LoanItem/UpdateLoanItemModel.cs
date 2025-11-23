using Solidaridad.Core.Entities.Loans;

namespace Solidaridad.Application.Models.LoanItem;

public class UpdateLoanItemModel
{
    public string ItemName { get; set; }

    public string Description { get; set; }

    public Guid CategoryId { get; set; }

    public decimal Quantity { get; set; }

    public decimal? UnitPrice { get; set; }

    public Guid MasterLoanItemId { get; set; }

    public Guid LoanApplicationId { get; set; }

    public int UnitId { get; set; }

    public ItemUnit Unit { get; set; }

    public  MasterLoanItem MasterLoanItem { get; set; }
    public  SelectItemModel LoanApplication { get; set; }
}
public class UpdateLoanItemResponseModel : BaseResponseModel { }
