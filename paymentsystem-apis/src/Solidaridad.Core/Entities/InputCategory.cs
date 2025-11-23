using LinqToDB.Mapping;
using Solidaridad.Core.Common;

namespace Solidaridad.Core.Entities
{
    [Table("InputCategories")]
    public class InputCategory : BaseEntity
    {
        public string Name { get; set; }
    }
}
