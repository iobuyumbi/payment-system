using Solidaridad.Core.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace Solidaridad.Core.Entities.Payments;

[Table("AssociateMaps")]
public class AssociateMap : BaseEntity
{
    public Guid FarmerId { get; set; }
    public Guid PaymentBatchId { get; set; }
}
