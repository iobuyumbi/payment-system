using Solidaridad.Core.Common;
using System.ComponentModel.DataAnnotations;

namespace Solidaridad.Core.Entities.Loans;

public class LoanBatchProcessingFee : BaseEntity
{
    public string FeeName { get; set; }

    public string FeeType { get; set; }

    public decimal Value { get; set; }

    public Guid LoanBatchId { get; set; }
}


