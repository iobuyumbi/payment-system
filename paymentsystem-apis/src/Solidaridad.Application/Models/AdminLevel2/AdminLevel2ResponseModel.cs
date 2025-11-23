namespace Solidaridad.Application.Models.SubCounty;

public class AdminLevel2ResponseModel : BaseResponseModel
{
    public string SubCountyName { get; set; }
    public string SubCountyCode { get; set; }
    public Guid CountyId { get; set; }
    public string CountyName { get; set; }

    public bool? IsActive { get; set; } = true; 
}
