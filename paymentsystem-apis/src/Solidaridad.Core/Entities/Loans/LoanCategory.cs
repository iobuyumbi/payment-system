using LinqToDB.Mapping;
using Solidaridad.Core.Common;

namespace Solidaridad.Core.Entities.Loans;

[Table("LoanCategories")]
public class LoanCategory : BaseEntity
{
    public string Name { get; set; }
    public string? Description { get; set; }
}
