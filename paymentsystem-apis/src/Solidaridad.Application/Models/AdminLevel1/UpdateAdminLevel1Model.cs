namespace Solidaridad.Application.Models.County;

public class UpdateAdminLevel1Model
{
    public string CountyName { get; set; }
    public string CountyCode { get; set; }
    public Guid CountryId { get; set; }

    public bool? IsActive { get; set; } = true;
}
public class UpdateAdminLevel1ResponseModel : BaseResponseModel { }
