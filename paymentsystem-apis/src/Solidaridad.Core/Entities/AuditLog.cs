using Solidaridad.Core.Common;

namespace Solidaridad.Core.Entities;

public class AuditLog : BaseEntity
{
    public string EntityType { get; set; } // "Invoice", "Customer", "Order", etc.
    
    public string EntityId { get; set; } // ID of the changed entity

    public string ChangedBy { get; set; } // User who made the change

    public DateTime ChangedOn { get; set; } = DateTime.UtcNow;

    public string ChangeType { get; set; } // "Created", "Updated", "Deleted"

    public string OldValue { get; set; } // Old value (as JSON)

    public string NewValue { get; set; } // New value (as JSON)
}
