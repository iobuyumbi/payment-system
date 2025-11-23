using Solidaridad.Core.Common;
using System.ComponentModel.DataAnnotations;

namespace Solidaridad.Core.Entities;

public class MasterLoanTermAdditionalFee : BaseEntity
{
    [StringLength(50)] public string FeeName { get; set; }

    [StringLength(50)] public string FeeType { get; set; }

    public decimal Value { get; set; }

    
}
