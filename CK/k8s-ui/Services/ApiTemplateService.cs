// Services/ApiTemplateService.cs - Template service using API instead of direct database
using K8sManager.Models.Entities;

namespace K8sManager.Services;

/// <summary>
/// Template service wrapper that calls REST API instead of direct database access
/// </summary>
public class ApiTemplateService
{
    private readonly ApiClient _apiClient;

    public ApiTemplateService(ApiClient apiClient)
    {
        _apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
    }

    public async Task<List<Template>> GetTemplatesAsync(string? category = null, string? searchTerm = null)
    {
        try
        {
            // API returns array directly, not paginated response
            var response = await _apiClient.GetTemplatesAsync<List<Template>>(category, searchTerm).ConfigureAwait(false);
            return response ?? new List<Template>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to get templates: {ex.Message}");
            return new List<Template>();
        }
    }

    public async Task<Template?> GetTemplateByIdAsync(int id)
    {
        try
        {
            return await _apiClient.GetTemplateByIdAsync<Template>(id).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to get template {id}: {ex.Message}");
            return null;
        }
    }

    public async Task<int> CreateTemplateAsync(string name, string? description, string category, string yamlContent, int createdBy, string? tags = null, bool isPublic = false)
    {
        try
        {
            var request = new
            {
                Name = name,
                Description = description,
                Category = category,
                YamlContent = yamlContent,
                Tags = tags,
                IsPublic = isPublic,
                CreatedBy = createdBy
            };

            var response = await _apiClient.CreateTemplateAsync<object, CreateTemplateResponse>(request).ConfigureAwait(false);
            var templateId = response?.Id ?? 0;

            // Auto-create first version (v1) for the template
            if (templateId > 0 && !string.IsNullOrWhiteSpace(yamlContent))
            {
                try
                {
                    var versionRequest = new
                    {
                        Version = 1,
                        YamlContent = yamlContent,
                        ChangeLog = "Initial version"
                    };

                    await _apiClient.CreateTemplateVersionAsync<object, CreateTemplateVersionResponse>(templateId, versionRequest).ConfigureAwait(false);
                    Console.WriteLine($"Auto-created v1 for template {templateId}");
                }
                catch (Exception versionEx)
                {
                    Console.WriteLine($"Failed to auto-create version for template {templateId}: {versionEx.Message}");
                    // Don't fail the whole operation, just log
                }
            }

            return templateId;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to create template: {ex.Message}");
            throw;
        }
    }

    public async Task<bool> UpdateTemplateAsync(int id, string name, string? description, string category, string yamlContent, int updatedBy, string? tags = null, bool isPublic = false)
    {
        try
        {
            var request = new
            {
                Name = name,
                Description = description,
                Category = category,
                YamlContent = yamlContent,
                Tags = tags,
                IsPublic = isPublic,
                UpdatedBy = updatedBy
            };

            await _apiClient.UpdateTemplateAsync(id, request).ConfigureAwait(false);

            // Note: Version creation should be done separately via "Save as Version" button
            // The template update only modifies the current working copy, not creating a snapshot

            return true;
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Failed to update template (HTTP Error): {ex.Message}");
            if (ex.StatusCode.HasValue)
            {
                Console.WriteLine($"Status Code: {ex.StatusCode.Value}");
                if (ex.StatusCode == System.Net.HttpStatusCode.Forbidden ||
                    ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    throw new Exception("Access denied. You don't have permission to update templates.", ex);
                }
            }
            throw new Exception($"Failed to update template: {ex.Message}", ex);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to update template: {ex.Message}");
            Console.WriteLine($"Stack trace: {ex.StackTrace}");
            throw;
        }
    }

    public async Task<bool> DeleteTemplateAsync(int id)
    {
        try
        {
            await _apiClient.DeleteTemplateAsync(id).ConfigureAwait(false);
            return true;
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Failed to delete template (HTTP Error): {ex.Message}");
            if (ex.StatusCode.HasValue)
            {
                Console.WriteLine($"Status Code: {ex.StatusCode.Value}");
                if (ex.StatusCode == System.Net.HttpStatusCode.Forbidden ||
                    ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    throw new Exception("Access denied. Only Admins can delete templates.", ex);
                }
            }
            throw new Exception($"Failed to delete template: {ex.Message}", ex);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to delete template: {ex.Message}");
            Console.WriteLine($"Stack trace: {ex.StackTrace}");
            throw;
        }
    }

    public async Task<List<TemplateVersion>> GetTemplateVersionsAsync(int templateId)
    {
        try
        {
            var template = await GetTemplateByIdAsync(templateId).ConfigureAwait(false);
            return template?.Versions?.ToList() ?? new List<TemplateVersion>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to get template versions: {ex.Message}");
            return new List<TemplateVersion>();
        }
    }

    public async Task<int> CreateTemplateVersionAsync(int templateId, string version, string yamlContent, string? changeLog, int createdBy)
    {
        try
        {
            // Parse version string to number (e.g., "1" -> 1)
            int versionNumber = int.TryParse(version, out var vNum) ? vNum : 1;

            var request = new
            {
                Version = versionNumber,  // API expects int
                YamlContent = yamlContent,
                ChangeLog = changeLog
            };

            var response = await _apiClient.CreateTemplateVersionAsync<object, CreateTemplateVersionResponse>(templateId, request).ConfigureAwait(false);
            var versionId = response?.Id ?? 0;

            if (versionId <= 0)
            {
                Console.WriteLine($"API returned invalid version ID for template {templateId}");
            }

            return versionId;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to create template version: {ex.Message}");
            Console.WriteLine($"StackTrace: {ex.StackTrace}");
            throw;
        }
    }

    public async Task<List<string>> GetCategoriesAsync()
    {
        try
        {
            var response = await _apiClient.GetTemplateCategoriesAsync<CategoriesResponse>().ConfigureAwait(false);
            return response?.Categories ?? new List<string>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to get categories: {ex.Message}");
            return new List<string> { "Deployment", "Service", "ConfigMap", "Secret", "Ingress", "StatefulSet", "DaemonSet", "Job", "CronJob" };
        }
    }
}

// DTOs matching API responses
public class TemplatesResponse
{
    public List<Template> Items { get; set; } = new();
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
}

public class CreateTemplateResponse
{
    public int Id { get; set; }
}

public class CreateTemplateVersionResponse
{
    public int Id { get; set; }
}

public class CategoriesResponse
{
    public List<string> Categories { get; set; } = new();
}
