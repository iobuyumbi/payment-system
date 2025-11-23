using Solidaridad.Application.Models.Project;

namespace Solidaridad.Application.Models.LoanApplication;

public class LoanBatchResponseModel : BaseResponseModel
{
    public string Name { get; set; }

    public decimal InterestRate { get; set; }

    public string PaymentTerms { get; set; }

    public Guid InitiatedBy { get; set; }

    public int Tenure { get; set; }

    public int GracePeriod { get; set; }

    public string RateType { get; set; }

    public string CalculationTimeframe { get; set; }

    public decimal ProcessingFee { get; set; }

    public DateTime EffectiveDate { get; set; }

    public DateTime InitiationDate { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime UpdatedOn { get; set; }

    public Guid ProjectId { get; set; }

    public int StatusId { get; set; }

    public ProjectResponseModel Project { get; set; }

    public List<ProcessingFeesModel> ProcessingFees { get; set; }
    
    public int TotalBatches {  get; set; }
    public int TotalApplications { get; set; }
    public float  TotalBatchAmount { get; set; }
    public int TotalDraft { get; set; }
    public int TotalAccepted { get; set; }
    public int TotalRejected { get; set; }
    public int TotalClosed { get; set; }
    public int TotalDisbursed { get; set; }

    public decimal MaxDeductiblePercent { get; set; }

    public string StageText { get; set; }
}


public class LoanBatchCountsResponseModel
{
    public int TotalBatches { get; set; }
    public int TotalApplications { get; set; }
    public int TotalDraft { get; set; }
    public int TotalAccepted { get; set; }
    public int TotalRejected { get; set; }
    public int TotalClosed { get; set; }
    public int TotalDisbursed { get; set; }
}

public class LoanBatchesResponseModel
{
    public IEnumerable<LoanBatchResponseModel> LoanBatches { get; set; }
    public LoanBatchCountsResponseModel LoanCounts { get; set; }
}
