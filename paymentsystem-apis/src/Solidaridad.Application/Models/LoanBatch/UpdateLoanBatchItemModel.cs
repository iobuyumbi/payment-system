namespace Solidaridad.Application.Models.LoanBatch;

public class UpdateLoanBatchItemModel
{
    public SelectItemModel LoanBatch { get; set; }

    public SelectItemModel LoanItem { get; set; }
    public bool IsFree { get; set; }

    public string SupplierDetails { get; set; }

    public decimal Quantity { get; set; }

    public SelectItemModel Unit { get; set; }

    public decimal? UnitPrice { get; set; }
}
public class UpdateLoanBatchItemResponseModel : BaseResponseModel { }
