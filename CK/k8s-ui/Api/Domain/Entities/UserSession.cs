namespace K8sManager.Api.Domain.Entities;

public class UserSession
{
    public long Id { get; set; }
    public int UserId { get; set; }
    public string SessionToken { get; set; } = null!;
    public string? IpAddress { get; set; }
    public string? UserAgent { get; set; }
    public DateTime ExpiresAt { get; set; }
    public DateTime CreatedAt { get; set; }

    // Navigation
    public string? Username { get; set; }

    public UserSession() { }

    public static UserSession Create(
        int userId,
        string sessionToken,
        DateTime expiresAt,
        string? ipAddress = null,
        string? userAgent = null)
    {
        if (string.IsNullOrWhiteSpace(sessionToken))
            throw new ArgumentException("Session token cannot be empty", nameof(sessionToken));

        if (expiresAt <= DateTime.UtcNow)
            throw new ArgumentException("Expiration must be in the future", nameof(expiresAt));

        return new UserSession
        {
            UserId = userId,
            SessionToken = sessionToken,
            IpAddress = ipAddress,
            UserAgent = userAgent,
            ExpiresAt = expiresAt,
            CreatedAt = DateTime.UtcNow
        };
    }

    public static UserSession Load(
        long id,
        int userId,
        string sessionToken,
        DateTime expiresAt,
        DateTime createdAt,
        string? ipAddress = null,
        string? userAgent = null,
        string? username = null)
    {
        return new UserSession
        {
            Id = id,
            UserId = userId,
            SessionToken = sessionToken,
            IpAddress = ipAddress,
            UserAgent = userAgent,
            ExpiresAt = expiresAt,
            CreatedAt = createdAt,
            Username = username
        };
    }

    public bool IsExpired() => DateTime.UtcNow > ExpiresAt;

    public bool IsActive() => !IsExpired();
}
