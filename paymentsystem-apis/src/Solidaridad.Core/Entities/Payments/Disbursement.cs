using Solidaridad.Core.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace Solidaridad.Core.Entities.Payments;

[Table("Disbursements")]
public class Disbursement : BaseEntity
{
    public Guid FarmerId { get; set; }
    public Guid MethodId { get; set; }
    public decimal Amount { get; set; }
    public string CurrencyCode { get; set; }
    public DateTime Date { get; set; }
    public Guid StatusId { get; set; }
    
    public virtual Farmer Farmer {  get; set; }
    
}
