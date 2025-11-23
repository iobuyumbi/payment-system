namespace Solidaridad.Application.Models.LocationProfiles;

public class CreateLocationProfileModel
{
    public Guid CountryId { get; set; }

    public string LogoUrl { get; set; }

    public string AddressLine1 { get; set; }

    public string AddressLine2 { get; set; }

    public string City { get; set; }

    public string State { get; set; }

    public string ZipCode { get; set; }

    public string SupportEmail { get; set; }

    public string PhoneNumber { get; set; }
    public string AlternateNumber { get; set; }
    public string Website { get; set; }

    public string[] AttachmentIds { get; set; }
}

public class CreateLocationProfileResponseModel : BaseResponseModel { }
