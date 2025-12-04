// Services/ApiAuditService.cs - AuditService wrapper sử dụng ApiClient
using K8sManager.Models.Entities;

namespace K8sManager.Services;

/// <summary>
/// AuditService wrapper that calls REST API instead of direct database access
/// </summary>
public class ApiAuditService
{
    private readonly ApiClient _apiClient;

    public ApiAuditService(ApiClient apiClient)
    {
        _apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
    }

    public async Task LogAsync(
        int? userId,
        string action,
        string resourceKind,
        string resourceName,
        string? resourceNamespace,
        bool success,
        string? errorMessage = null,
        string? details = null)
    {
        try
        {
            var request = new
            {
                UserId = userId,
                Action = action,
                ResourceKind = resourceKind,
                ResourceName = resourceName,
                ResourceNamespace = resourceNamespace,
                Success = success,
                ErrorMessage = errorMessage,
                Details = details
            };

            await _apiClient.CreateAuditLogAsync<object, object>(request).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            // Log to console but don't throw - audit logging shouldn't break the app
            Console.WriteLine($"Failed to log audit: {ex.Message}");
        }
    }

    public async Task<List<AuditLog>> GetLogsAsync(
        int? userId = null,
        string? action = null,
        DateTime? fromDate = null,
        DateTime? toDate = null,
        int pageNumber = 1,
        int pageSize = 100)
    {
        try
        {
            // API returns List<AuditLog> directly, not wrapped in response object
            var logs = await _apiClient.GetAuditLogsAsync<List<AuditLog>>(
                userId, action, fromDate, toDate, pageNumber, pageSize).ConfigureAwait(false);

            return logs ?? new List<AuditLog>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to get audit logs: {ex.Message}");
            return new List<AuditLog>();
        }
    }
}

// DTO matching API response
public class AuditLogsResponse
{
    public List<AuditLog> Items { get; set; } = new();
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
}
