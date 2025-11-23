namespace Solidaridad.Application.Models.LoanBatch;

public class CreateLoanBatchItemModel
{
    public SelectItemModel LoanBatch { get; set; }

    public SelectItemModel LoanItem { get; set; }

    public string SupplierDetails { get; set; }

    public decimal Quantity { get; set; }
    
    public bool IsFree { get; set; }

    public SelectItemModel Unit { get; set; }

    public decimal? UnitPrice { get; set; }
}

public class CreateLoanBatchItemResponseModel : BaseResponseModel { }
