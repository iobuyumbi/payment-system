namespace Solidaridad.Application.Models.AuditLog;

public class AuditLogResponse
{
public string ChangedBy { get; set; }
public DateTime ChangedOn { get; set; }
public string ChangeType { get; set; } // e.g., Updated, Deleted
public List<AuditChange> Changes { get; set; }
}
