using Solidaridad.Core.Common;

namespace Solidaridad.Core.Entities;

public class FarmerCooperative: BaseEntity
{
    public Guid FarmerId { get; set; }

    public Guid CooperativeId { get; set; }
}
