using Solidaridad.Application.Models.LoanApplication;
using Solidaridad.Application.Models.LoanItem;

namespace Solidaridad.Application.Models.Reports;

public class LoanAccountResponseModel
{
    public LoanItemStatsResponseModel LoanItemStats { get; set; }
    public IEnumerable<LoanApplicationResponseModel> LoanApplications { get; set; }

}
