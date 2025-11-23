namespace Solidaridad.Application.Models.Cooperative;

public class CreateCooperativeModel
{
    public string Name { get; set; }
    public Guid CountryId { get; set; }
    public string Description { get; set; }

}
public class CreateCooperativeResponseModel : BaseResponseModel { }
