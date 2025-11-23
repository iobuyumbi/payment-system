using Solidaridad.Core.Common;

namespace Solidaridad.Core.Entities.Loans;

public class LoanInterest : BaseEntity, IAuditedEntity
{
    public decimal MonthlyPayment { get; set; }

    public decimal AccruedInterest { get; set; }

    public decimal RemainingPrincipal { get; set; }

    public DateTime CalculationMonth { get; set; }

    public Guid LoanApplicationId { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; }

    public Guid? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }
}

