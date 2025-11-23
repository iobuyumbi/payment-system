using Solidaridad.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solidaridad.Core.Entities.Loans;

public class LoanStatement : BaseEntity 
{
    public DateTime StatementDate { get; set; }
    public string TransactionReference { get; set; }
    public string TransactionType { get; set; }
    public string Description { get; set; }
    public decimal OpeningBalance { get; set; }
    public decimal DebitAmount { get; set; }
    public decimal CreditAmount { get; set; }
    public decimal LoanBalance { get; set; }
    public decimal AccuredInterest { get; set; }
    public decimal AccuredPrincipalPayment { get; set; }
    public decimal InterestPaid { get; set; }
    public decimal PrincipalPaid { get; set; }
    public Guid FarmerId { get; set; }
    public string SystemId { get; set; }
    public Guid ApplicationId { get; set; }
    public string ReferenceNumber { get; set; }
}
