namespace Solidaridad.Application.Models.Reports;

public class DisbursedLoanReportResponseModel
{
    public string LoanNumber { get; set; }
    public Guid BatchId { get; set; }

    public DateTime DisbursementDate { get; set; }

    public DateTime MaturityDate { get; set; }

    public decimal Interest { get; set; }

    public int LoanTerm { get; set; }

    public string FarmerSystemId { get; set; }

    public string FarmerName { get; set; }

    public decimal PrincipalAmount { get; set; }

    public decimal FeesApplied { get; set; }

    public decimal EffectivePrincipal { get; set; }

    public decimal ExpectedInterestPerSchedule { get; set; }
    public string CurrentUserName { get; set; }
}
