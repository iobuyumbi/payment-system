namespace Solidaridad.Application.Models.ActivityLog;

public class UpdateActivityLogModel
{
    public Guid ActivityTypeId { get; set; }
    public string Description { get; set; }
    public string Title { get; set; }
}

public class UpdateActivityLogResponseModel : BaseResponseModel { }

