namespace Solidaridad.Application.Models.ActivityLog;

public class CreateActivityLogModel
{
    public string Description { get; set; }
    public string Title { get; set; }
    public string Link { get; set; }
}

public class CreateActivityLogResponseModel : BaseResponseModel { }

