namespace Solidaridad.Application.Models.ActivityLog;

public class ActivityLogResponseModel : BaseResponseModel
{
    public string Description { get; set; }

    public string Title { get; set; }
    
    public string Link { get; set; }

    public string CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; }
}


