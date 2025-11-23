using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solidaridad.Application.Models.LoanBatch;

public class LoanBatchStatsResponseModel
{
    public float TotalLoanValue { get; set; }
    public int TotalLoanBatches { get; set; }
    public int ActiveLoanBatches { get; set; }
    public int ClosedLoanBatches { get; set; }
    public int OverdueLoanBatches { get; set; }
    public int TotalLoans { get; set; }
    public int ActiveLoans { get; set; }
    public int ClosedLoans { get; set; }
    public int OverdueLoans { get; set; }
    public int NonPerformingLoans { get; set; }
    public float NonPerformingValue { get; set; }


}
