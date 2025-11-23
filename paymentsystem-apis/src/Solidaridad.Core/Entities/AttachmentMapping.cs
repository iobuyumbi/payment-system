using Solidaridad.Core.Common;

namespace Solidaridad.Core.Entities;

public class AttachmentMapping : BaseEntity
{
    public Guid LoanApplicationId { get; set; }
    public Guid AttachmentId {  get; set; } 

}
