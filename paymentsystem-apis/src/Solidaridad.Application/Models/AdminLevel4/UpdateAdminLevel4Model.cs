namespace Solidaridad.Application.Models.Village;

public class UpdateAdminLevel4Model
{
    public string VillageName { get; set; }
    public string VillageCode { get; set; }
    public Guid WardId { get; set; }
    public bool IsActive { get; set; } = true;
}
public class UpdateAdminLevel4ResponseModel : BaseResponseModel { }
