namespace Solidaridad.Application.Models.LoanProcessingFee;

public class CreateLoanProcessingFeeModel
{
    public string FeeName { get; set; }

    public string FeeType { get; set; }

    public decimal? Value { get; set; }
}

public class CreateLoanProcessingFeeResponseModel : BaseResponseModel { }
