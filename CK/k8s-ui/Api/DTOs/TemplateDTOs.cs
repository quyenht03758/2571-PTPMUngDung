// Api/DTOs/TemplateDTOs.cs
namespace K8sManager.Api.DTOs;

public class CreateTemplateRequest
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string YamlContent { get; set; } = string.Empty;
}

public class UpdateTemplateRequest
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string YamlContent { get; set; } = string.Empty;
}

public class CreateTemplateVersionRequest
{
    public int Version { get; set; }
    public string YamlContent { get; set; } = string.Empty;
    public string? ChangeLog { get; set; }
}

public record TemplateRequest(
    string Name,
    string Category,
    string? Description = null,
    string? Tags = null
);

public record TemplateResponse(
    int Id,
    string Name,
    string Category,
    string? Description,
    string? Tags,
    int CreatedBy,
    DateTime CreatedAt,
    List<TemplateVersionInfo>? Versions = null
);

public record TemplateVersionInfo(
    int Id,
    string Version,
    string Content,
    string? ChangeLog,
    DateTime CreatedAt,
    bool IsActive
);

public record TemplateFilter(
    string? Category = null,
    string? SearchTerm = null,
    int? CreatedBy = null,
    int PageNumber = 1,
    int PageSize = 20
);

