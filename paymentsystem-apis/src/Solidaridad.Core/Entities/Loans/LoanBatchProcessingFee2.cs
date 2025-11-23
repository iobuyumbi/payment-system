using Solidaridad.Core.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace Solidaridad.Core.Entities.Loans;
[Table("LoanBatchProcessingFee2")]
public class LoanBatchProcessingFee2 : BaseEntity
{
    public string FeeName { get; set; }

    public string FeeType { get; set; }

    public decimal? Amount { get; set; }

    public decimal? Percent { get; set; }

    public Guid LoanBatchId { get; set; }

    public bool IsDeleted { get; set; }
}
