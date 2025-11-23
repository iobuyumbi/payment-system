using Solidaridad.Core.Common;
using Solidaridad.Core.Entities.Loans;
using Solidaridad.Core.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Solidaridad.Core.Entities.Payments;

[Table("PaymentBatches")]
public class PaymentBatch : BaseEntity, IAuditedEntity
{
    public string BatchName { get; set; }

    public DateTime DateCreated { get; set; } = DateTime.UtcNow;

    public Guid LoanBatchId { get; set; }

    public int PaymentModule { get; set; }

    public Guid ProjectId { get; set; }

    public Guid CountryId { get; set; }

    public virtual Country Country { get; set; }

    public Guid StatusId { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; }

    public Guid? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public MasterPaymentApprovalStage Status { get; set; }

    public List<PaymenBatchLoanBatchMapping> PaymenBatchLoanBatchMapping { get; set; }

    public bool IsExcelUploaded { get; set; } = false;

    public BatchStatus BatchStatus { get; set; }
    public string ReferenceNumber { get; set; }
}
