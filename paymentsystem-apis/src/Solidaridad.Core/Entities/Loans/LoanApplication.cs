using Solidaridad.Core.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace Solidaridad.Core.Entities.Loans;

[Table("LoanApplications")]
public class LoanApplication : BaseEntity, IAuditedEntity
{
    public Guid FarmerId { get; set; }

    public string WitnessFullName { get; set; }

    public string WitnessNationalId { get; set; }

    public string WitnessPhoneNo { get; set; }

    public string WitnessRelation { get; set; }

    public DateTime DateOfWitness { get; set; }

    public string EnumeratorFullName { get; set; }

    public Guid LoanBatchId { get; set; }
    
    public Guid Status { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; }

    public Guid? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public virtual Farmer Farmer { get; set; }

    public virtual List<LoanItem> LoanItems { get; set; }
    
    public decimal PrincipalAmount { get; set; }

    public decimal EffectivePrincipal { get; set; }

    public decimal AccruedInterest { get; set; }

    public decimal InterestAmount { get; set; }

    public decimal InterestRate { get; set; }

    public string LoanNumber { get; set; }

    public decimal RemainingBalance { get; set; }

    public decimal FeeApplied { get; set; }
    public DateTime? DisbursementDate { get; set; } = null;
    public  Guid OfficerId { get; set; }
}
