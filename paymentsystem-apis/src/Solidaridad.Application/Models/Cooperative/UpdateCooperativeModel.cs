namespace Solidaridad.Application.Models.Cooperative;

public class UpdateCooperativeModel
{
    public string Name { get; set; }
    public Guid CountryId { get; set; }
    public string Description { get; set; }
}
public class UpdateCooperativeResponseModel : BaseResponseModel { }
