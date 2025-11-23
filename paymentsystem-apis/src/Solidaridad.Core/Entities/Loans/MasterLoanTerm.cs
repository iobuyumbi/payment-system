using Solidaridad.Core.Common;

namespace Solidaridad.Core.Entities.Loans;

public class MasterLoanTerm : BaseEntity, IAuditedEntity
{
    public string DescriptiveName { get; set; }

    public string InterestRateType { get; set; }

    public decimal InterestRate { get; set; }

    public string InterestApplication { get; set; }

    public int Tenure { get; set; }

    public int GracePeriod { get; set; }

    public bool HasAdditionalFee { get; set; } = false;

    public decimal MaxDeductiblePercent { get; set; }

    public List<MasterLoanTermAdditionalFee> AdditionalFee { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; }

    public Guid? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public Guid CountryId { get; set; }
}
