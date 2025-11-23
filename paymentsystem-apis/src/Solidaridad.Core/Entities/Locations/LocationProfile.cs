using Solidaridad.Core.Common;

namespace Solidaridad.Core.Entities.Locations;

public class LocationProfile : BaseEntity, IAuditedEntity
{
    public Guid CountryId { get; set; }

    public Guid? AttachmentFileId { get; set; }
    public AttachmentFile AttachmentFile { get; set; }

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
    public bool IsPrimary { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; }

    public Guid? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }
}
