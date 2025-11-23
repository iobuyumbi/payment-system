namespace Solidaridad.Application.Models.Location;

public class LocationResponseModel : BaseResponseModel
{
    public string Name { get; set; }

    public Guid CountryId { get; set; }
}
