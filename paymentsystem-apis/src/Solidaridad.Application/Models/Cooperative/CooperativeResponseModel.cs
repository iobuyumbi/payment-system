using Solidaridad.Application.Models.Country;

namespace Solidaridad.Application.Models.Cooperative;

public class CooperativeResponseModel : BaseResponseModel
{
    public string Name { get; set; }
    public Guid CountryId { get; set; }
    public string Description { get; set; }
    public CountryResponseModel Country{ get; set; }
     
}
