namespace Solidaridad.Application.Models.EmiSchedule;

public class EMIScheduleResponseModel
{
    public decimal Amount { get; set; }

    public decimal PrincipalAmount { get; set; }

    public decimal InterestAmount { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public decimal Balance { get; set; }

    public string PaymentStatus { get; set; }

    public Guid LoanApplicationId { get; set; }
}
