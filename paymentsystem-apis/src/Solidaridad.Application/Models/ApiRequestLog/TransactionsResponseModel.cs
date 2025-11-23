using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solidaridad.Application.Models.ApiRequestLog;

public class TransactionsResponseModel
{
    public IEnumerable<ApiRequestLogResponseModel> ApiRequestLogResponseModel { get; set; }
    public LogCounterResponseModel LogCounterResponseModel { get; set; }
}
