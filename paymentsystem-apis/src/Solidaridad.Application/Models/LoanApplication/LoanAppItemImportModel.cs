namespace Solidaridad.Application.Models.LoanApplication;

public class LoanAppItemImportModel : BaseResponseModel
{
    public string FarmerSystemId { get; set; }

    public string ItemName { get; set; }

    public decimal Quantity { get; set; }

    public string Unit { get; set; }

    public int UnitId { get; set; }

    public decimal? UnitPrice { get; set; }
    public Guid MasterLoanItemId { get; set; }
}
