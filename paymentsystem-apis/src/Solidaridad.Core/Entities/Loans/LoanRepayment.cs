using Solidaridad.Core.Common;

namespace Solidaridad.Core.Entities.Loans;

public class LoanRepayment : BaseEntity, IAuditedEntity
{
    public Guid LoanApplicationId { get; set; }

    public decimal AmountPaid { get; set; }

    public DateTime PaymentDate { get; set; }

    public string PaymentMode { get; set; } // Cash, Bank Transfer, etc.

    public string ReferenceNumber { get; set; } // Optional for tracking

    public Guid CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; }

    public Guid? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    //navigation property
    public LoanApplication LoanApplication { get; set; }
}
