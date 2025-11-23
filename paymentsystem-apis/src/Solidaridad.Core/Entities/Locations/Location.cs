using Solidaridad.Core.Common;

namespace Solidaridad.Core.Entities.Locations;

public class Location : BaseEntity, IAuditedEntity
{
    public string Name { get; set; }

    public Guid CountryId { get; set; }

    public Country Country { get; set; }

    public LocationProfile LocationProfile { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; }

    public Guid? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }
}


