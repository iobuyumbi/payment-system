namespace Solidaridad.Application.Models.Village;

public class AdminLevel4ResponseModel:BaseResponseModel
{
    public string VillageName { get; set; }
    public string VillageCode { get; set; }
    public Guid WardId { get; set; }
    public bool IsActive { get; set; } = true;
}
