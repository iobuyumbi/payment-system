using Solidaridad.Core.Common;

namespace Solidaridad.Core.Entities;

public class AccessLog : BaseEntity
{
    public string UserId { get; set; }

    public string UserName { get; set; }

    public DateTime AccessTime { get; set; } = DateTime.UtcNow;

    public AccessType AccessType { get; set; }

    public string IpAddress { get; set; }

    public string UserAgent { get; set; }

    public AccessStatus Status { get; set; }

    public string Message { get; set; }

    public Guid CountryId { get; set; }
}

public enum AccessType
{
    Login,
    Logout,
    FailedLogin,
    PasswordChange,
    TokenRefresh
}

public enum AccessStatus
{
    Success,
    Failure
}
