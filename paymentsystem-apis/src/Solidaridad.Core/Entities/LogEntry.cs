using System.ComponentModel.DataAnnotations.Schema;

namespace Solidaridad.Core.Entities;

[Table("LogEntry")]
public class LogEntry 
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("Id")]
    public long Id { get; set; }
    
    public DateTime Timestamp { get; set; }
    public string Level { get; set; }
    public string Message { get; set; }
    public string Exception { get; set; }
    public string StackTrace { get; set; }
}

