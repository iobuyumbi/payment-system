namespace Solidaridad.Application.Models.County;

public class AdminLevel1ResponseModel : BaseResponseModel
{
    public string CountyName { get; set; }
    public string CountyCode { get; set; }
    public Guid CountryId { get; set; }
    public string CountryName { get; set; }

    public bool? IsActive { get; set; } = true;
}
