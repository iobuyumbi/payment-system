using Solidaridad.Core.Common;

namespace Solidaridad.Core.Entities;

public class PaymentRequestDeductible : BaseEntity, IAuditedEntity
{
    public string SystemId { get; set; }

    public decimal CruCount { get; set; }
    public Guid PaymentBatchId { get; set; }

    public decimal PricePerCru { get; set; }

    public decimal PurchasePriceSum { get; set; }

    public decimal FarmerShareUsd { get; set; }

    public decimal ConvertedPurchasePrice { get; set; }

    public decimal AdminDeduction { get; set; }

    public decimal FarmerShareConverted { get; set; }

    public decimal LoanAdjustmentAmount { get; set; }

    public decimal NetDisbursementAmount { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; }

    public Guid? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }
}
