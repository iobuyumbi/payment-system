using Solidaridad.Core.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace Solidaridad.Core.Entities;

[Table("Countries")]
public class Country : BaseEntity
{
    public string CountryName { get; set; }
    public string Code { get; set; }
    public bool IsActive { get; set; }
    public string CurrencyName { get; set; }
    public string CurrencyPrefix {  get; set; }
    public string? CurrencySuffix { get; set; }
}
