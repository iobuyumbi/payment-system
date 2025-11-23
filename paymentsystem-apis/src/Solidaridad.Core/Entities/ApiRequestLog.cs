using Solidaridad.Core.Common;

namespace Solidaridad.Core.Entities;

public class ApiRequestLog : BaseEntity
{
    public string ApiName { get; set; }

    public string HttpMethod { get; set; }

    public string EndpointUrl { get; set; }

    public string RequestHeaders { get; set; }

    public string RequestBody { get; set; }

    public int? StatusCode { get; set; }

    public string ResponseHeaders { get; set; }

    public string ResponseBody { get; set; }

    public bool IsSuccessful { get; set; }

    public DateTime RequestTimestamp { get; set; }

    public DateTime? ResponseTimestamp { get; set; }

    public string ErrorMessage { get; set; }

    public string CorrelationId { get; set; }
}
