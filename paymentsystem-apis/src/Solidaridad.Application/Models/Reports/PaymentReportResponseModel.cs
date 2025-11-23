using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solidaridad.Application.Models.Reports;

public class PaymentReportResponseModel
{
    public DateTime PaymentDate { get; set; }
    public string FarmerSystemId { get; set; }
    public string FarmerName { get; set; }
    public decimal FarmerEarnings { get; set; }
    public decimal FarmerPayableEarnings { get; set; }
    public decimal LoanDeduction { get; set; }
    public decimal LoanOpeningBalance { get; set; }
    public decimal LoanClosingBalance { get; set; }
    public string ProcessingMethod { get; set; }
    public string PaymentReference { get; set; }
    public string ReceivingMobileNo { get; set; }
    public string Status { get; set; }
    public Guid PaymentBatchId { get; set; }
    public string CurrentUserName { get; set; }
    public List<StatusChangeHistory> BatchStatusHistory { get; set; } 
    public string Remarks { get; set; }
}
public class StatusChangeHistory
{
    public string Status { get; set; }           // e.g., "Reviewed"
    public string UpdatedByUserName { get; set; } // e.g., "adminuser"
    public DateTime UpdatedAt { get; set; }       
}
