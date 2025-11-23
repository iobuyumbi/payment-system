using Solidaridad.Core.Common;

namespace Solidaridad.Core.Entities;

public class MasterLoanTermAdditionalFeeMapping : BaseEntity
{
    public Guid LoanTermId { get; set; }
    public Guid AdditionalFeeId { get; set; }
}
