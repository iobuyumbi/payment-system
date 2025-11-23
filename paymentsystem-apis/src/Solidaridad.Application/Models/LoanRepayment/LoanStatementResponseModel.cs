namespace Solidaridad.Application.Models.LoanRepayment;

public class LoanStatementResponseModel
{
    public DateTime StatementDate { get; set; }
    public string TransactionReference { get; set; }
    public string TransactionType { get; set; }
    public string Description { get; set; }
    public decimal OpeningBalance { get; set; }
    public decimal DebitAmount { get; set; }
    public decimal CreditAmount { get; set; }
    public decimal LoanBalance { get; set; }
    public Guid FarmerId { get; set; }
    public string SystemId { get; set; }
    public Guid ApplicationId { get; set; }
    public decimal InterestPaid { get; set; }
    public decimal PrincipalPaid { get; set; }
}
