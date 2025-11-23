using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solidaridad.Application.Models.Reports;

public class GlobalLoanPortfolioReportResponseModel
{
    public string CountryName { get; set; }
    public decimal AmountDisbursed { get; set; }
    public decimal PrincipalDue { get; set; }
    public decimal PrincipalReceived { get; set; }
    public decimal PrincipalBalance { get; set; }
    public decimal PrincipalArrears { get; set; }
    public decimal TotalInterest { get; set; }
    public decimal InterestDue { get; set; }
    public decimal InterestReceived { get; set; }
    public decimal InterestArrears { get; set; }
    public decimal TotalArrears { get; set; }
    public decimal TotalExpected { get; set; }
    public decimal ArrearsRate { get; set; }
    public string CurrentUserName { get; set; }

}
