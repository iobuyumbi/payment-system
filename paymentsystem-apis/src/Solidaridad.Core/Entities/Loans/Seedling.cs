using LinqToDB.Mapping;
using Solidaridad.Core.Common;

namespace Solidaridad.Core.Entities.Loans;

[Table("Seedlings")]
public class Seedling : BaseEntity
{
    public string Name { get; set; }
    public int Quantity {  get; set; }
    public float PricePerSeedling { get; set; }

}
