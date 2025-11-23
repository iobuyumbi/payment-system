namespace Solidaridad.Application.Models.ApiRequestLog;

public class LogCounterResponseModel
{
    public int TotalTransactions { get; set; }
     public int Successful { get; set; }
    public int Unsuccessful { get; set; }
}
