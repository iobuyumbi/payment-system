namespace Solidaridad.Application.Models.ApiRequestLog;

public class ApiRequestLogResponseModel : BaseResponseModel
{
    public string ApiName { get; set; }
    public bool IsSuccessful { get; set; }
    public DateTime RequestTimestamp { get; set; }
    public DateTime? ResponseTimestamp { get; set; }
    public string ErrorMessage { get; set; }

    //public int? StatusCode { get; set; }

    //public string ResponseHeaders { get; set; }

    //public string ResponseBody { get; set; }


    //public string CorrelationId { get; set; }
}
