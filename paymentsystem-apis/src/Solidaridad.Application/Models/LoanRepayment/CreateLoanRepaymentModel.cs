namespace Solidaridad.Application.Models.LoanRepayment;

public class CreateLoanRepaymentModel
{
    public Guid LoanApplicationId { get; set; }

    public decimal RepaymentAmount { get; set; }
}

public class CreateLoanRepaymentResponseModel : BaseResponseModel { }
