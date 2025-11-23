using Solidaridad.Core.Common;

namespace Solidaridad.Core.Entities;


public class CallBackRecords : BaseEntity
{
    
    public int Organization { get; set; }
    public string Amount { get; set; }
    public string Currency { get; set; } 
    public string PaymentType { get; set; } 
    public string Metadata { get; set; } 
    public string Description { get; set; } 
    public string PhoneNos { get; set; } 
    public string State { get; set; }
    public string? LastError { get; set; }
    public string? RejectedReason { get; set; }
    public int? RejectedBy { get; set; }
    public DateTime? RejectedTime { get; set; }
    public string? CancelledReason { get; set; }
    public int? CancelledBy { get; set; }
    public DateTime? CancelledTime { get; set; }
    public DateTime Created { get; set; }
    public int Author { get; set; }
    public DateTime Modified { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTime StartDate { get; set; }
    public string ReferenceId { get; set; }
    public string CallBackResponse { get; set; }
}


