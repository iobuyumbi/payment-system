namespace Solidaridad.Application.Models.LoanProcessingFee;

public class LoanProcessingFeeResponseModel : BaseResponseModel
{
    public string FeeName { get; set; }

    public string FeeType { get; set; }

    public decimal? Value { get; set; }
}
