using Solidaridad.Core.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace Solidaridad.Core.Entities.Loans;

[Table("LoanBatches")]
public class LoanBatch : BaseEntity, IAuditedEntity
{
    public string Name { get; set; }

    public Guid InitiatedBy { get; set; }

    public DateTime InitiationDate { get; set; }

    public Guid ProjectId { get; set; }

    public int StatusId { get; set; }

    public decimal InterestRate { get; set; }

    public string PaymentTerms { get; set; }

    public int? Tenure { get; set; }

    public int? GracePeriod { get; set; }

    public string RateType { get; set; }

    public string CalculationTimeframe { get; set; }

    public DateTime EffectiveDate { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; }

    public Guid? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public virtual Project Project { get; set; }

    public decimal MaxDeductiblePercent { get; set; }

    public List<LoanBatchProcessingFee> ProcessingFees { get; set; }

    public string StageText { get; set; } = "Initiated";

    public Guid CountryId { get; set; }
}
