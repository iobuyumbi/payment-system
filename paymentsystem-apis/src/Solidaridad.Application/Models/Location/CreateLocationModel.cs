namespace Solidaridad.Application.Models.Location;

public class CreateLocationModel
{
    public string Name { get; set; }

    public Guid CountryId { get; set; }
}

public class CreateLocationResponseModel : BaseResponseModel { }
