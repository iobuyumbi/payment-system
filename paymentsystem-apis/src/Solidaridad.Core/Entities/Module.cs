using Solidaridad.Core.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace Solidaridad.Core.Entities;

[Table("Modules")]
public class Module : BaseEntity
{
    public string ModuleName { get; set; }

    public string Description { get; set; }
}
