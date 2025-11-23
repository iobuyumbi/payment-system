namespace Solidaridad.Application.Models.Ward;

public class AdminLevel3ResponseModel : BaseResponseModel
{
    public string WardName { get; set; }
    public string WardCode { get; set; }
    public Guid SubCountyId { get; set; }
    public string SubCountyName { get; set; }

    public bool IsActive { get; set; } = true;
}
