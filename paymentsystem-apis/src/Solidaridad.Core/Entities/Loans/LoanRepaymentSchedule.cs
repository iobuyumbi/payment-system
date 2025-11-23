using Solidaridad.Core.Common;

namespace Solidaridad.Core.Entities.Loans;

public class LoanRepaymentSchedule : BaseEntity, IAuditedEntity
{
    public Guid LoanApplicationId { get; set; }

    public LoanApplication LoanApplication { get; set; }

    public Guid FarmerId { get; set; }

    public Farmer Farmer { get; set; }

    public string Period { get; set; }

    public decimal BeginningBalance { get; set; }

    public decimal Payment { get; set; }

    public decimal Interest { get; set; }

    public decimal Principal { get; set; }

    public decimal EndingBalance { get; set; }

    public int Installment { get; set; }
    public bool IsPaid { get; set; } = false;

    public Guid CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; }

    public Guid? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }
}
