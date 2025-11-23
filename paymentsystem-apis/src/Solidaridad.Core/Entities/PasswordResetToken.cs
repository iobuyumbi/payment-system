using Solidaridad.Core.Common;
using System.ComponentModel.DataAnnotations;

namespace Solidaridad.Core.Entities;

public class PasswordResetToken : BaseEntity
{
    [Required]
    [StringLength(255)]
    public string UserId { get; set; }

    [Required]
    public string Token { get; set; }

    [Required]
    public DateTime ExpiryDate { get; set; }

    public bool IsUsed { get; set; } = false;

    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
}
