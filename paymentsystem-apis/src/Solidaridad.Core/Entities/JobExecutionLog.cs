using Solidaridad.Core.Common;

namespace Solidaridad.Core.Entities;

public class JobExecutionLog : BaseEntity
{
    public string JobName { get; set; } // Name of the job
    public DateTime StartTime { get; set; } // When the job started
    public DateTime EndTime { get; set; } // When the job ended
    public bool IsSuccess { get; set; } // Success or failure
    public string ErrorMessage { get; set; } // Error message if any
}
