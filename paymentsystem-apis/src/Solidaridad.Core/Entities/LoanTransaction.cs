using Solidaridad.Core.Common;

namespace Solidaridad.Core.Entities;

public class LoanTransaction: BaseEntity
{
    public DateTime Date { get; set; }
    
    public string Description { get; set; }
    
    public decimal EffectivePrincipal { get; set; }
    
    public decimal InterestAccrued { get; set; }
    
    public decimal PaymentReceived { get; set; }
    
    public decimal PrincipalPaid { get; set; }
    
    public decimal InterestPaid { get; set; }
    
    public decimal CumulativeBalance { get; set; }
    
    public Guid LoanId { get; set; }
    
    public Guid? PaymentBatchId { get; set; }  // Nullable for non-payment transactions
}
