namespace K8sManager.Api.Domain.Entities;

/// <summary>
/// Template entity for Kubernetes resource templates
/// </summary>
public class Template
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Category { get; set; }
    public string? Description { get; set; }
    public string? Tags { get; set; }
    public string? YamlContent { get; set; }
    public int Version { get; set; }
    public bool IsPublic { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navigation properties
    public string? CreatedByUsername { get; set; }
    public List<TemplateVersion> Versions { get; set; } = new();

    public Template() { }

    /// <summary>
    /// Create a new template
    /// </summary>
    public static Template Create(
        string name,
        int createdBy,
        string? category = null,
        string? description = null,
        string? tags = null,
        bool isPublic = false)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Template name cannot be empty", nameof(name));

        if (name.Length > 200)
            throw new ArgumentException("Template name cannot exceed 200 characters", nameof(name));

        return new Template
        {
            Name = name.Trim(),
            Category = category?.Trim(),
            Description = description?.Trim(),
            Tags = tags?.Trim(),
            IsPublic = isPublic,
            CreatedBy = createdBy,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    /// <summary>
    /// Load existing template from database
    /// </summary>
    public static Template Load(
        int id,
        string name,
        int createdBy,
        DateTime createdAt,
        DateTime updatedAt,
        string? category = null,
        string? description = null,
        string? tags = null,
        bool isPublic = false,
        string? createdByUsername = null,
        string? yamlContent = null)
    {
        return new Template
        {
            Id = id,
            Name = name,
            Category = category,
            Description = description,
            Tags = tags,
            YamlContent = yamlContent,
            IsPublic = isPublic,
            CreatedBy = createdBy,
            CreatedAt = createdAt,
            UpdatedAt = updatedAt,
            CreatedByUsername = createdByUsername
        };
    }

    /// <summary>
    /// Update template details
    /// </summary>
    public void Update(
        string? name = null,
        string? category = null,
        string? description = null,
        string? tags = null,
        bool? isPublic = null)
    {
        if (!string.IsNullOrWhiteSpace(name))
        {
            if (name.Length > 200)
                throw new ArgumentException("Template name cannot exceed 200 characters", nameof(name));
            Name = name.Trim();
        }

        if (category != null) Category = category.Trim();
        if (description != null) Description = description.Trim();
        if (tags != null) Tags = tags.Trim();
        if (isPublic.HasValue) IsPublic = isPublic.Value;

        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Add a version to this template
    /// </summary>
    public void AddVersion(TemplateVersion version)
    {
        Versions.Add(version);
    }

    /// <summary>
    /// Check if user can access this template
    /// </summary>
    public bool CanBeAccessedBy(int userId)
    {
        return IsPublic || CreatedBy == userId;
    }

    /// <summary>
    /// Get tags as list
    /// </summary>
    public List<string> GetTagsList()
    {
        if (string.IsNullOrEmpty(Tags))
            return new List<string>();

        return Tags.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .ToList();
    }
}
