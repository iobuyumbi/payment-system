namespace Solidaridad.Application.Models.ApplicationStatusLog;

public class ApplicationStatusLogResponseModel : BaseResponseModel
{
    public Guid ApplicationId { get; set; }
    public Guid StatusId { get; set; }
    public string Comments { get; set; }
    public string Moderator { get; set; }
    public string ModeratorRole { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }
    public Guid? UpdatedBy { get; set; }
    public DateTime? UpdatedOn { get; set; }
    public string StatusText { get; set; }

    public Solidaridad.Core.Entities.Loans.ApplicationStatus ApplicationStatus { get; set; }
}
