namespace K8sManager.Api.Domain.Entities;

public class Favorite
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int ClusterId { get; set; }
    public string? Namespace { get; set; }
    public string? ResourceKind { get; set; }
    public string? ResourceType { get; set; }
    public string? ResourceName { get; set; }
    public string DisplayName { get; set; } = null!;
    public string? Notes { get; set; }
    public int SortOrder { get; set; }
    public DateTime CreatedAt { get; set; }

    // Navigation
    public string? ClusterName { get; set; }

    public Favorite() { }

    public static Favorite Create(
        int userId,
        int clusterId,
        string displayName,
        string? @namespace = null,
        string? resourceKind = null,
        string? resourceName = null,
        string? notes = null,
        int sortOrder = 0)
    {
        if (string.IsNullOrWhiteSpace(displayName))
            throw new ArgumentException("Display name cannot be empty", nameof(displayName));

        return new Favorite
        {
            UserId = userId,
            ClusterId = clusterId,
            Namespace = @namespace,
            ResourceKind = resourceKind,
            ResourceName = resourceName,
            DisplayName = displayName.Trim(),
            Notes = notes?.Trim(),
            SortOrder = sortOrder,
            CreatedAt = DateTime.UtcNow
        };
    }

    public static Favorite Load(
        int id,
        int userId,
        int clusterId,
        string displayName,
        DateTime createdAt,
        string? @namespace = null,
        string? resourceKind = null,
        string? resourceName = null,
        string? notes = null,
        int sortOrder = 0,
        string? clusterName = null)
    {
        return new Favorite
        {
            Id = id,
            UserId = userId,
            ClusterId = clusterId,
            Namespace = @namespace,
            ResourceKind = resourceKind,
            ResourceName = resourceName,
            DisplayName = displayName,
            Notes = notes,
            SortOrder = sortOrder,
            CreatedAt = createdAt,
            ClusterName = clusterName
        };
    }

    public void Update(string? displayName = null, string? notes = null, int? sortOrder = null)
    {
        if (!string.IsNullOrWhiteSpace(displayName))
            DisplayName = displayName.Trim();
        if (notes != null) Notes = notes.Trim();
        if (sortOrder.HasValue) SortOrder = sortOrder.Value;
    }
}
