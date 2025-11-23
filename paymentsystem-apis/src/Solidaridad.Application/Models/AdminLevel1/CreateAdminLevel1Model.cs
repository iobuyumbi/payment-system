namespace Solidaridad.Application.Models.County;

public class CreateAdminLevel1Model
{
    public string CountyName { get; set; }
    public string CountyCode { get; set; }
    public Guid CountryId { get; set; }

    public bool? IsActive { get; set; } = true;
}
public class CreateAdminLevel1ResponseModel : BaseResponseModel { }
