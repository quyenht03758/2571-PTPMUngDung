namespace K8sManager.Api.Domain.Entities;

/// <summary>
/// Global application setting (system-wide, not user-specific)
/// Key is the primary key (not auto-increment Id)
/// </summary>
public class AppSetting
{
    public string Key { get; set; } = null!;
    public string Value { get; set; } = null!;
    public string? Category { get; set; }
    public string? Description { get; set; }
    public bool IsEncrypted { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navigation
    public string? UpdatedByUsername { get; set; }

    public AppSetting() { }

    /// <summary>
    /// Create new global setting
    /// </summary>
    public static AppSetting Create(
        string key,
        string value,
        string? category = null,
        string? description = null,
        bool isEncrypted = false,
        int? updatedBy = null)
    {
        if (string.IsNullOrWhiteSpace(key))
            throw new ArgumentException("Setting key cannot be empty", nameof(key));
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Setting value cannot be empty", nameof(value));

        return new AppSetting
        {
            Key = key.Trim(),
            Value = value,
            Category = category?.Trim(),
            Description = description?.Trim(),
            IsEncrypted = isEncrypted,
            UpdatedBy = updatedBy,
            UpdatedAt = DateTime.UtcNow
        };
    }

    /// <summary>
    /// Load existing setting from database
    /// </summary>
    public static AppSetting Load(
        string key,
        string value,
        string? category,
        string? description,
        bool isEncrypted,
        int? updatedBy,
        DateTime updatedAt,
        string? updatedByUsername = null)
    {
        return new AppSetting
        {
            Key = key,
            Value = value,
            Category = category,
            Description = description,
            IsEncrypted = isEncrypted,
            UpdatedBy = updatedBy,
            UpdatedAt = updatedAt,
            UpdatedByUsername = updatedByUsername
        };
    }

    /// <summary>
    /// Update setting value (UPSERT pattern)
    /// </summary>
    public void Update(string? value = null, string? description = null, int? updatedBy = null)
    {
        if (!string.IsNullOrWhiteSpace(value))
            Value = value;
        if (description != null)
            Description = description.Trim();
        if (updatedBy.HasValue)
            UpdatedBy = updatedBy;
        UpdatedAt = DateTime.UtcNow;
    }
}
