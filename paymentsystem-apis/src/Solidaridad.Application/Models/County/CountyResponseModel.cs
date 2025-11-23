namespace Solidaridad.Application.Models.County;

public class CountyResponseModel : BaseResponseModel
{
    public string CountyName { get; set; }
    public string Code { get; set; }
    public bool IsActive { get; set; }
}
