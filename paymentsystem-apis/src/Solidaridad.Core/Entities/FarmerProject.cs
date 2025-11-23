using Solidaridad.Core.Common;

namespace Solidaridad.Core.Entities;

public class FarmerProject: BaseEntity
{
    public Guid FarmerId { get; set; }
    public Guid ProjectId { get; set; }
    public Project Project { get; set; }
}
