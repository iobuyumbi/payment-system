using Solidaridad.Core.Common;

namespace Solidaridad.Core.Entities;

public class ActivityLog : BaseEntity
{
    public string Title { get; set; }

    public string Description { get; set; }

    public string Link { get; set; }

     public string CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; }

    public Guid? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public Guid? CountryId { get; set; }
}
