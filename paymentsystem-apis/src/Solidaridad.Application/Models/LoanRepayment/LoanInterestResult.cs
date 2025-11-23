namespace Solidaridad.Application.Models.LoanRepayment;

public class LoanInterestResult
{
    public DateTime Month { get; set; }
    public decimal Interest { get; set; }
    public decimal Payment { get; set; }
    public decimal RemainingPrincipal { get; set; }
}
