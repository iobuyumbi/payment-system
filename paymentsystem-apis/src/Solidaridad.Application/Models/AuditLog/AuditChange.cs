namespace Solidaridad.Application.Models.AuditLog;

public class AuditChange
{
    public string Field { get; set; }
    public object OldValue { get; set; }
    public object NewValue { get; set; }
}
