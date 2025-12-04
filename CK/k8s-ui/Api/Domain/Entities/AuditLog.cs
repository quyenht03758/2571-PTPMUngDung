namespace K8sManager.Api.Domain.Entities;

/// <summary>
/// Audit log entity for tracking all user actions in the system
/// </summary>
public class AuditLog
{
    public long Id { get; set; }
    public int UserId { get; set; }
    public int? ClusterId { get; set; }
    public string Action { get; set; } = null!;
    public string? ResourceKind { get; set; }
    public string? ResourceName { get; set; }
    public string? Namespace { get; set; }
    public bool Success { get; set; }
    public string? ErrorMessage { get; set; }
    public string? RequestPayload { get; set; }
    public string? ResponseData { get; set; }
    public string? IpAddress { get; set; }
    public int? Duration { get; set; } // Duration in milliseconds
    public DateTime CreatedAt { get; set; }
    public string? Details { get; set; }
    public DateTime Timestamp { get; set; }

    // Navigation properties
    public string? Username { get; set; }
    public string? ClusterName { get; set; }

    public AuditLog() { }

    /// <summary>
    /// Factory method to create a new audit log entry
    /// </summary>
    public static AuditLog Create(
        int userId,
        string action,
        bool success,
        int? clusterId = null,
        string? resourceKind = null,
        string? resourceName = null,
        string? @namespace = null,
        string? errorMessage = null,
        string? requestPayload = null,
        string? responseData = null,
        string? ipAddress = null,
        int? duration = null)
    {
        if (string.IsNullOrWhiteSpace(action))
            throw new ArgumentException("Action cannot be empty", nameof(action));

        if (action.Length > 100)
            throw new ArgumentException("Action cannot exceed 100 characters", nameof(action));

        return new AuditLog
        {
            UserId = userId,
            ClusterId = clusterId,
            Action = action,
            ResourceKind = resourceKind,
            ResourceName = resourceName,
            Namespace = @namespace,
            Success = success,
            ErrorMessage = errorMessage,
            RequestPayload = requestPayload,
            ResponseData = responseData,
            IpAddress = ipAddress,
            Duration = duration,
            CreatedAt = DateTime.UtcNow
        };
    }

    /// <summary>
    /// Load existing audit log from database
    /// </summary>
    public static AuditLog Load(
        long id,
        int userId,
        string action,
        bool success,
        DateTime createdAt,
        int? clusterId = null,
        string? resourceKind = null,
        string? resourceName = null,
        string? @namespace = null,
        string? errorMessage = null,
        string? requestPayload = null,
        string? responseData = null,
        string? ipAddress = null,
        int? duration = null,
        string? username = null,
        string? clusterName = null)
    {
        return new AuditLog
        {
            Id = id,
            UserId = userId,
            ClusterId = clusterId,
            Action = action,
            ResourceKind = resourceKind,
            ResourceName = resourceName,
            Namespace = @namespace,
            Success = success,
            ErrorMessage = errorMessage,
            RequestPayload = requestPayload,
            ResponseData = responseData,
            IpAddress = ipAddress,
            Duration = duration,
            CreatedAt = createdAt,
            Username = username,
            ClusterName = clusterName
        };
    }

    /// <summary>
    /// Get formatted action description
    /// </summary>
    public string GetFormattedAction()
    {
        var parts = new List<string> { Action };

        if (!string.IsNullOrEmpty(ResourceKind))
            parts.Add(ResourceKind);

        if (!string.IsNullOrEmpty(ResourceName))
            parts.Add(ResourceName);

        if (!string.IsNullOrEmpty(Namespace))
            parts.Add($"in {Namespace}");

        return string.Join(" ", parts);
    }

    /// <summary>
    /// Check if this is a cluster-related action
    /// </summary>
    public bool IsClusterAction() => ClusterId.HasValue;

    /// <summary>
    /// Check if this is a resource-related action
    /// </summary>
    public bool IsResourceAction() => !string.IsNullOrEmpty(ResourceKind);
}
