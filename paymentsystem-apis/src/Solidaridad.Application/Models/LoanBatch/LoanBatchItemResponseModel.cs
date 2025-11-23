using Solidaridad.Application.Models.LoanApplication;
using Solidaridad.Application.Models.LoanItem;

namespace Solidaridad.Application.Models.LoanBatch;

public class LoanBatchItemResponseModel : BaseResponseModel
{
    public SelectItemModel LoanBatch { get; set; }

    public SelectItemModel LoanItem { get; set; }

    public string SupplierDetails { get; set; }

    public decimal Quantity { get; set; }

    public SelectItemModel Unit { get; set; }

    public decimal? UnitPrice { get; set; }

    public Guid LoanItemId { get; set; }
    public bool IsFree { get; set; }

    public Guid LoanBatchId { get; set; }

    public int UnitId { get; set; }
}
