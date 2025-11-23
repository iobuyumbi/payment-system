using Solidaridad.Core.Entities;

namespace Solidaridad.Application.Models.LoanTerm;

public class MasterLoanTermResponseModel : BaseResponseModel
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

    public Guid CountryId { get; set; }
}

