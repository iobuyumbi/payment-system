using Solidaridad.Core.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace Solidaridad.Core.Entities.Wallets;
[Table("Wallets")]
public class Wallet : BaseEntity, IAuditedEntity
{
    public decimal Balance { get; private set; }

    public Guid OwnerId { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; }

    public Guid? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }
}
