using Solidaridad.Core.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace Solidaridad.Core.Entities.Loans;

[Table("ApplicationStatusLogs")]
public class ApplicationStatusLog : BaseEntity , IAuditedEntity
{
    public Guid ApplicationId { get; set; }
    public Guid StatusId { get; set; }
    public string Comments { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }
    public Guid? UpdatedBy { get; set; }
    public DateTime? UpdatedOn { get; set; }

    public ApplicationStatus ApplicationStatus { get; set; }
}
