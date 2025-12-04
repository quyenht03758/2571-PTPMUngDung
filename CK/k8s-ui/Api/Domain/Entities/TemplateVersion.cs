namespace K8sManager.Api.Domain.Entities;

/// <summary>
/// Template version entity for version control
/// </summary>
public class TemplateVersion
{
    public int Id { get; set; }
    public int TemplateId { get; set; }
    public int VersionNumber { get; set; }
    public int Version { get; set; }
    public string YamlContent { get; set; } = null!;
    public string? ChangeLog { get; set; }
    public bool IsCurrent { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }

    // Navigation properties
    public string? CreatedByUsername { get; set; }

    public TemplateVersion() { }

    /// <summary>
    /// Create a new template version
    /// </summary>
    public static TemplateVersion Create(
        int templateId,
        int versionNumber,
        string yamlContent,
        int createdBy,
        string? changeLog = null,
        bool isCurrent = true)
    {
        if (string.IsNullOrWhiteSpace(yamlContent))
            throw new ArgumentException("YAML content cannot be empty", nameof(yamlContent));

        if (versionNumber < 1)
            throw new ArgumentException("Version number must be at least 1", nameof(versionNumber));

        return new TemplateVersion
        {
            TemplateId = templateId,
            VersionNumber = versionNumber,
            YamlContent = yamlContent,
            ChangeLog = changeLog?.Trim(),
            IsCurrent = isCurrent,
            CreatedBy = createdBy,
            CreatedAt = DateTime.UtcNow
        };
    }

    /// <summary>
    /// Load existing template version from database
    /// </summary>
    public static TemplateVersion Load(
        int id,
        int templateId,
        int versionNumber,
        string yamlContent,
        int createdBy,
        DateTime createdAt,
        bool isCurrent,
        string? changeLog = null,
        string? createdByUsername = null)
    {
        return new TemplateVersion
        {
            Id = id,
            TemplateId = templateId,
            VersionNumber = versionNumber,
            YamlContent = yamlContent,
            ChangeLog = changeLog,
            IsCurrent = isCurrent,
            CreatedBy = createdBy,
            CreatedAt = createdAt,
            CreatedByUsername = createdByUsername
        };
    }

    /// <summary>
    /// Mark this version as current
    /// </summary>
    public void SetAsCurrent()
    {
        IsCurrent = true;
    }

    /// <summary>
    /// Mark this version as not current
    /// </summary>
    public void UnsetAsCurrent()
    {
        IsCurrent = false;
    }

    /// <summary>
    /// Get YAML content line count
    /// </summary>
    public int GetLineCount()
    {
        return YamlContent.Split('\n').Length;
    }
}
