using Solidaridad.Core.Common;
using System.ComponentModel.DataAnnotations;

namespace Solidaridad.Core.Entities;

public class UserOtp : BaseEntity
{
    [StringLength(150)]
    public string UserId { get; set; }

    public string OtpHash { get; set; } = string.Empty;

    public DateTime ExpiresAt { get; set; }

    public bool IsUsed { get; set; }

    public int AttemptCount { get; set; } = 0; // For retry limit

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
