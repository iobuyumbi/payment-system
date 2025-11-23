using Solidaridad.Application.Models.LoanApplication;
using Solidaridad.Application.Models.LoanBatch;

namespace Solidaridad.Application.Models.Reports;

public class LoanBatchReportResponseModel
{

    public LoanBatchStatsResponseModel LoanBatchStatsResponseModel { get; set; }
    public IEnumerable<LoanApplicationResponseModel> LoanApplications { get; set; }
}
