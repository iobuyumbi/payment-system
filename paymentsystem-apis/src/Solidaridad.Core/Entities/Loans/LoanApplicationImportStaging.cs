using Solidaridad.Core.Common;
using System.ComponentModel.DataAnnotations;

namespace Solidaridad.Core.Entities.Loans;

public class LoanApplicationImportStaging : BaseEntity, IAuditedEntity
{
    public Guid? FarmerId { get; set; }

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

    public decimal PrincipalAmount { get; set; }

    public decimal EffectivePrincipal { get; set; }

    public decimal AccruedInterest { get; set; }

    public decimal InterestAmount { get; set; }

    public decimal InterestRate { get; set; }

    public string LoanNumber { get; set; }

    public decimal RemainingBalance { get; set; }

    public decimal FeeApplied { get; set; }

    public Guid ExcelImportId { get; set; }

    public int RowNumber { get; set; }

    [StringLength(50)] public string ValidationStatus { get; set; }

    public string ValidationErrors { get; set; }
    
    public short StatusId { get; set; } = 0;

    public Guid? CountryId { get; set; }
    public Guid? OfficerId { get; set; }
}
