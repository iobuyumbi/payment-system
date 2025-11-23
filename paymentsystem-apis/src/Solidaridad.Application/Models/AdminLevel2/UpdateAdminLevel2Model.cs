namespace Solidaridad.Application.Models.SubCounty;

public class UpdateAdminLevel2Model
{
    public string SubCountyName { get; set; }
    public string SubCountyCode { get; set; }
    public Guid CountyId { get; set; }

    public bool? IsActive { get; set; } = true;
}
public class UpdateAdminLevel2ResponseModel : BaseResponseModel { }
