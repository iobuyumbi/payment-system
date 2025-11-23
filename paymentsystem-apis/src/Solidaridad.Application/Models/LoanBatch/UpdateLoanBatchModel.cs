using Solidaridad.Core.Entities.Loans;

namespace Solidaridad.Application.Models.LoanApplication;

public class UpdateLoanBatchModel
{
    public string Name { get; set; }

    public decimal InterestRate { get; set; }

    public string PaymentTerms { get; set; }

    public DateTime InitiationDate { get; set; }

    public Guid ProjectId { get; set; }

    public List<LoanBatchProcessingFee> ProcessingFees { get; set; }

    public int StatusId { get; set; }

    public int Tenure { get; set; }

    public int GracePeriod { get; set; }

    public string RateType { get; set; }

    public string CalculationTimeframe { get; set; }

    public decimal ProcessingFee { get; set; }

    public DateTime EffectiveDate { get; set; }

    public decimal MaxDeductiblePercent { get; set; }
}
public class UpdateLoanBatchResponseModel : BaseResponseModel { }
