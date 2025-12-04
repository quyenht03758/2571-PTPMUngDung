using K8sManager.Api.Domain.Entities;

namespace K8sManager.Api.Domain.Repositories;

public interface ITemplateRepository
{
    Task<Template?> GetByIdAsync(int id, bool includeVersions = false);

    Task<(IEnumerable<Template> Items, int TotalCount)> GetPagedAsync(
        int pageNumber,
        int pageSize,
        int? userId = null,
        string? category = null,
        string? searchTerm = null,
        bool? isPublic = null);

    Task<int> CreateAsync(Template template);

    Task<bool> UpdateAsync(Template template);

    Task<bool> DeleteAsync(int id);

    Task<IEnumerable<Template>> GetAllAsync(string? category = null, string? search = null);

    Task<bool> ExistsAsync(string name, int? excludeId = null);

    // Template Version operations
    Task<TemplateVersion?> GetVersionByIdAsync(int versionId);

    Task<IEnumerable<TemplateVersion>> GetVersionsByTemplateIdAsync(int templateId);

    Task<TemplateVersion?> GetCurrentVersionAsync(int templateId);

    Task<int> CreateVersionAsync(TemplateVersion version);

    Task UpdateVersionAsync(TemplateVersion version);

    Task SetCurrentVersionAsync(int templateId, int versionId);
}
