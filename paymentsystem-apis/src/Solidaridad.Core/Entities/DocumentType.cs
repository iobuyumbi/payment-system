using Solidaridad.Core.Common;
using System.ComponentModel.DataAnnotations;

namespace Solidaridad.Core.Entities;

public class DocumentType: BaseEntity
{
    [Required]
    [MaxLength(20)]
    public string TypeName { get; set; }
}
