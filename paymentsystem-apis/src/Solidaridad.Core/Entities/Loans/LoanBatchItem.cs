using Solidaridad.Core.Common;

namespace Solidaridad.Core.Entities.Loans;

public class LoanBatchItem : BaseEntity , IAuditedEntity
{
    public LoanBatch LoanBatch { get; set; }

    public MasterLoanItem LoanItem { get; set; }
    public bool IsFree { get; set; }

    public string SupplierDetails { get; set; }

    public decimal Quantity { get; set; }

    public ItemUnit Unit { get; set; }

    public decimal? UnitPrice { get; set; }

    public Guid LoanBatchId { get; set; }

    public Guid LoanItemId { get; set; }

    public int UnitId { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }
    public Guid? UpdatedBy { get; set; }
    public DateTime? UpdatedOn { get; set; }
}
