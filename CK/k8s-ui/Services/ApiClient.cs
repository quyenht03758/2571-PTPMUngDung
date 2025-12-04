// Services/ApiClient.cs - HTTP Client for WinForm to call REST API
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace K8sManager.Services;

/// <summary>
/// HTTP Client for calling K8s Manager REST API (SOA Architecture)
/// </summary>
public class ApiClient : IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;
    private string? _jwtToken;

    public ApiClient(string baseUrl = "http://localhost:5000")
    {
        _baseUrl = baseUrl;
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(_baseUrl),
            Timeout = TimeSpan.FromSeconds(30)
        };
        _httpClient.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
    }

    public void SetAuthToken(string token)
    {
        _jwtToken = token;
        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);
    }

    public void ClearAuthToken()
    {
        _jwtToken = null;
        _httpClient.DefaultRequestHeaders.Authorization = null;
    }

    // ============ AUTH ENDPOINTS ============

    public async Task<TResponse?> LoginAsync<TRequest, TResponse>(TRequest request)
        where TRequest : class
        where TResponse : class
    {
        var response = await _httpClient.PostAsJsonAsync("/api/auth/login", request).ConfigureAwait(false);
        
        // Don't throw on 401, let caller handle authentication failures
        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            var errorContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            // Try to parse error response
            try
            {
                return await response.Content.ReadFromJsonAsync<TResponse>().ConfigureAwait(false);
            }
            catch
            {
                return null;
            }
        }
        
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<TResponse>().ConfigureAwait(false);
    }

    public async Task<TResponse?> LogoutAsync<TResponse>()
        where TResponse : class
    {
        var response = await _httpClient.PostAsync("/api/auth/logout", null).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<TResponse>().ConfigureAwait(false);
    }

    public async Task<TResponse?> GetCurrentUserAsync<TResponse>()
        where TResponse : class
    {
        var response = await _httpClient.GetAsync("/api/auth/me").ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<TResponse>().ConfigureAwait(false);
    }

    // ============ AUDIT ENDPOINTS ============

    public async Task<TResponse?> CreateAuditLogAsync<TRequest, TResponse>(TRequest request)
        where TRequest : class
        where TResponse : class
    {
        var response = await _httpClient.PostAsJsonAsync("/api/auditlogs", request).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<TResponse>().ConfigureAwait(false);
    }

    public async Task<TResponse?> GetAuditLogsAsync<TResponse>(
        int? userId = null,
        string? action = null,
        DateTime? fromDate = null,
        DateTime? toDate = null,
        int pageNumber = 1,
        int pageSize = 50)
        where TResponse : class
    {
        var query = $"?skip={(pageNumber - 1) * pageSize}&take={pageSize}";
        if (userId.HasValue) query += $"&userId={userId}";
        if (!string.IsNullOrEmpty(action)) query += $"&action={Uri.EscapeDataString(action)}";
        if (fromDate.HasValue) query += $"&startDate={fromDate:O}";
        if (toDate.HasValue) query += $"&endDate={toDate:O}";

        var response = await _httpClient.GetAsync($"/api/auditlogs{query}").ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<TResponse>().ConfigureAwait(false);
    }

    public async Task<TResponse?> GetAuditStatsAsync<TResponse>(
        DateTime? fromDate = null,
        DateTime? toDate = null)
        where TResponse : class
    {
        var query = string.Empty;
        if (fromDate.HasValue) query += $"?fromDate={fromDate:O}";
        if (toDate.HasValue) query += $"{(query.Length > 0 ? "&" : "?")}toDate={toDate:O}";

        var response = await _httpClient.GetAsync($"/api/auditlogs/stats{query}").ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<TResponse>().ConfigureAwait(false);
    }

    // ============ TEMPLATE ENDPOINTS ============

    public async Task<TResponse?> GetTemplatesAsync<TResponse>(
        string? category = null,
        string? searchTerm = null,
        int pageNumber = 1,
        int pageSize = 20)
        where TResponse : class
    {
        var query = $"?PageNumber={pageNumber}&PageSize={pageSize}";
        if (!string.IsNullOrEmpty(category)) query += $"&Category={category}";
        if (!string.IsNullOrEmpty(searchTerm)) query += $"&search={searchTerm}";  // Changed from SearchTerm to search

        var response = await _httpClient.GetAsync($"/api/templates{query}").ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<TResponse>().ConfigureAwait(false);
    }

    public async Task<TResponse?> GetTemplateByIdAsync<TResponse>(int id)
        where TResponse : class
    {
        var response = await _httpClient.GetAsync($"/api/templates/{id}").ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<TResponse>().ConfigureAwait(false);
    }

    public async Task<TResponse?> CreateTemplateAsync<TRequest, TResponse>(TRequest request)
        where TRequest : class
        where TResponse : class
    {
        var response = await _httpClient.PostAsJsonAsync("/api/templates", request).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<TResponse>().ConfigureAwait(false);
    }

    public async Task UpdateTemplateAsync(int id, object request)
    {
        var response = await _httpClient.PutAsJsonAsync($"/api/templates/{id}", request).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteTemplateAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"/api/templates/{id}").ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
    }

    public async Task<TResponse?> CreateTemplateVersionAsync<TRequest, TResponse>(int templateId, TRequest request)
        where TRequest : class
        where TResponse : class
    {
        var response = await _httpClient.PostAsJsonAsync($"/api/templates/{templateId}/versions", request).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<TResponse>().ConfigureAwait(false);
    }

    public async Task<TResponse?> GetTemplateCategoriesAsync<TResponse>()
        where TResponse : class
    {
        var response = await _httpClient.GetAsync("/api/templates/categories").ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<TResponse>().ConfigureAwait(false);
    }

    // ============ FAVORITE ENDPOINTS ============

    public async Task<TResponse?> GetFavoritesAsync<TResponse>(
        string? resourceKind = null,
        string? namespace_ = null)
        where TResponse : class
    {
        // Controller returns List<Favorite> directly from GET /api/favorites
        var response = await _httpClient.GetAsync("/api/favorites").ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<TResponse>().ConfigureAwait(false);
    }

    public async Task<TResponse?> AddFavoriteAsync<TRequest, TResponse>(TRequest request)
        where TRequest : class
        where TResponse : class
    {
        var response = await _httpClient.PostAsJsonAsync("/api/favorites", request).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<TResponse>().ConfigureAwait(false);
    }

    public async Task<TResponse?> UpdateFavoriteAsync<TRequest, TResponse>(int id, TRequest request)
        where TRequest : class
        where TResponse : class
    {
        var response = await _httpClient.PutAsJsonAsync($"/api/favorites/{id}", request).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        
        // Backend returns 204 NoContent, so check before reading
        if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
            return null;
            
        return await response.Content.ReadFromJsonAsync<TResponse>().ConfigureAwait(false);
    }

    public async Task<TResponse?> RemoveFavoriteAsync<TResponse>(int id)
        where TResponse : class
    {
        var response = await _httpClient.DeleteAsync($"/api/favorites/{id}").ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        
        // Backend returns 204 NoContent, so check before reading
        if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
            return null;
            
        return await response.Content.ReadFromJsonAsync<TResponse>().ConfigureAwait(false);
    }

    // ============ SETTINGS ENDPOINTS ============

    public async Task<TResponse?> GetSettingsAsync<TResponse>()
        where TResponse : class
    {
        var response = await _httpClient.GetAsync("/api/settings").ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<TResponse>().ConfigureAwait(false);
    }

    public async Task<TResponse?> GetSettingByKeyAsync<TResponse>(string key)
        where TResponse : class
    {
        var response = await _httpClient.GetAsync($"/api/settings/{key}").ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<TResponse>().ConfigureAwait(false);
    }

    public async Task<TResponse?> SaveSettingAsync<TRequest, TResponse>(TRequest request)
        where TRequest : class
        where TResponse : class
    {
        var response = await _httpClient.PostAsJsonAsync("/api/settings", request).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<TResponse>().ConfigureAwait(false);
    }

    public async Task<TResponse?> GetSettingsByCategoryAsync<TResponse>(string category)
        where TResponse : class
    {
        var response = await _httpClient.GetAsync($"/api/settings/category/{category}").ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<TResponse>().ConfigureAwait(false);
    }

    // ============ GENERIC HELPERS ============

    public async Task<TResponse?> GetAsync<TResponse>(string endpoint)
        where TResponse : class
    {
        var response = await _httpClient.GetAsync(endpoint).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<TResponse>().ConfigureAwait(false);
    }

    public async Task<TResponse?> PostAsync<TRequest, TResponse>(string endpoint, TRequest request)
        where TRequest : class
        where TResponse : class
    {
        var response = await _httpClient.PostAsJsonAsync(endpoint, request).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<TResponse>().ConfigureAwait(false);
    }

    public async Task<TResponse?> PutAsync<TRequest, TResponse>(string endpoint, TRequest request)
        where TRequest : class
        where TResponse : class
    {
        var response = await _httpClient.PutAsJsonAsync(endpoint, request).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<TResponse>().ConfigureAwait(false);
    }

    public async Task<TResponse?> DeleteAsync<TResponse>(string endpoint)
        where TResponse : class
    {
        var response = await _httpClient.DeleteAsync(endpoint).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<TResponse>().ConfigureAwait(false);
    }

    // ============ USER MANAGEMENT ENDPOINTS ============

    public async Task<TResponse?> GetUsersAsync<TResponse>(bool? isActive = null)
        where TResponse : class
    {
        var query = isActive.HasValue ? $"?isActive={isActive.Value}" : "";
        var response = await _httpClient.GetAsync($"/api/users{query}").ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<TResponse>().ConfigureAwait(false);
    }

    public async Task<TResponse?> GetUserByIdAsync<TResponse>(int id)
        where TResponse : class
    {
        var response = await _httpClient.GetAsync($"/api/users/{id}").ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<TResponse>().ConfigureAwait(false);
    }

    public async Task<TResponse?> CreateUserAsync<TRequest, TResponse>(TRequest request)
        where TRequest : class
        where TResponse : class
    {
        var response = await _httpClient.PostAsJsonAsync("/api/users", request).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<TResponse>().ConfigureAwait(false);
    }

    public async Task<bool> UpdateUserAsync<TRequest>(int id, TRequest request)
        where TRequest : class
    {
        var response = await _httpClient.PutAsJsonAsync($"/api/users/{id}", request).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> ResetUserPasswordAsync<TRequest>(int id, TRequest request)
        where TRequest : class
    {
        var response = await _httpClient.PostAsJsonAsync($"/api/users/{id}/reset-password", request).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> LockUserAsync(int id)
    {
        var response = await _httpClient.PostAsync($"/api/users/{id}/lock", null).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UnlockUserAsync(int id)
    {
        var response = await _httpClient.PostAsync($"/api/users/{id}/unlock", null).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteUserAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"/api/users/{id}").ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        return response.IsSuccessStatusCode;
    }

    public void Dispose()
    {
        _httpClient?.Dispose();
    }
}
