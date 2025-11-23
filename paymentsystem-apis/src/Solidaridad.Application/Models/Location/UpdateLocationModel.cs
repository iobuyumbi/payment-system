namespace Solidaridad.Application.Models.Location;

public class UpdateLocationModel
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public Guid CountryId { get; set; }
}

public class UpdateLocationResponseModel : BaseResponseModel { }
