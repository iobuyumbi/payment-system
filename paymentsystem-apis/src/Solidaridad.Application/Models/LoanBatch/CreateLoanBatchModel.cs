namespace Solidaridad.Application.Models.LoanApplication;

public class CreateLoanBatchModel
{
    public string Name { get; set; }

    public decimal InterestRate { get; set; }

    public string PaymentTerms { get; set; }

    public int Tenure { get; set; }

    public int GracePeriod { get; set; }

    public string RateType { get; set; }

    public string CalculationTimeframe { get; set; }

    public List<ProcessingFeesModel> ProcessingFees { get; set; }

    public DateTime? EffectiveDate { get; set; }

    public DateTime? InitiationDate { get; set; }

    public Guid ProjectId { get; set; }

    public int StatusId { get; set; }

    public decimal MaxDeductiblePercent { get; set; }
    public Guid? CountryId { get; set; }
}

public class ProcessingFeesModel
{
    public string FeeName { get; set; }

    public string FeeType { get; set; }

    public decimal Value { get; set; }
}

public class CreateLoanBatchResponseModel : BaseResponseModel { }
