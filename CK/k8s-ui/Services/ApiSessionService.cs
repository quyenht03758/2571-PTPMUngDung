// Services/ApiSessionService.cs - SessionService wrapper sử dụng ApiClient
using K8sManager.Models.Entities;

namespace K8sManager.Services;

/// <summary>
/// SessionService wrapper that calls REST API instead of direct database access
/// </summary>
public class ApiSessionService
{
    private readonly ApiClient _apiClient;

    public UserSession? CurrentUser { get; private set; }
    
    /// <summary>
    /// Expose ApiClient for direct API calls
    /// </summary>
    public ApiClient ApiClient => _apiClient;

    /// <summary>
    /// Check if user is authenticated (has valid session)
    /// </summary>
    public bool IsAuthenticated => CurrentUser != null && CurrentUser.ExpiresAt > DateTime.UtcNow;

    public ApiSessionService(ApiClient apiClient)
    {
        _apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
    }

    public async Task<(bool Success, string? SessionToken, string? Message)> LoginAsync(
        string username,
        string password,
        string? ipAddress = null,
        string? userAgent = null)
    {
        try
        {
            var request = new
            {
                Username = username,
                Password = password
            };

            var response = await _apiClient.LoginAsync<object, LoginResponse>(request).ConfigureAwait(false);

            if (response?.Success == true && !string.IsNullOrEmpty(response.Token))
            {
                // Set JWT token for subsequent requests
                _apiClient.SetAuthToken(response.Token);

                // Create CurrentUser from response
                if (response.User != null)
                {
                    CurrentUser = new UserSession
                    {
                        UserId = response.User.Id,
                        SessionToken = response.Token,
                        Role = response.User.Role,
                        ExpiresAt = DateTime.UtcNow.AddHours(8),
                        CreatedAt = DateTime.UtcNow
                    };
                }

                return (true, response.Token, response.Message ?? "Đăng nhập thành công");
            }

            return (false, null, response?.Message ?? "Tên đăng nhập hoặc mật khẩu không đúng");
        }
        catch (HttpRequestException ex)
        {
            // Network/connection errors
            return (false, null, $"Không thể kết nối đến API server. Vui lòng kiểm tra kết nối mạng hoặc API server có đang chạy.");
        }
        catch (TaskCanceledException)
        {
            // Timeout
            return (false, null, "Kết nối đến API server bị timeout. Vui lòng thử lại.");
        }
        catch (Exception ex)
        {
            return (false, null, $"Lỗi không xác định: {ex.Message}");
        }
    }

    public async Task<bool> LogoutAsync(string sessionToken)
    {
        try
        {
            await _apiClient.LogoutAsync<object>().ConfigureAwait(false);
            _apiClient.ClearAuthToken();
            CurrentUser = null;
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> ValidateSessionAsync(string sessionToken)
    {
        try
        {
            _apiClient.SetAuthToken(sessionToken);
            var user = await _apiClient.GetCurrentUserAsync<UserInfoResponse>().ConfigureAwait(false);

            if (user != null)
            {
                CurrentUser = new UserSession
                {
                    UserId = user.Id,
                    SessionToken = sessionToken,
                    Role = user.Role,
                    ExpiresAt = DateTime.UtcNow.AddHours(8),
                    CreatedAt = user.CreatedAt
                };
                return true;
            }

            return false;
        }
        catch
        {
            return false;
        }
    }

    public async Task<UserSession?> GetUserByTokenAsync(string sessionToken)
    {
        try
        {
            _apiClient.SetAuthToken(sessionToken);
            var user = await _apiClient.GetCurrentUserAsync<UserInfoResponse>().ConfigureAwait(false);

            if (user != null)
            {
                return new UserSession
                {
                    UserId = user.Id,
                    SessionToken = sessionToken,
                    Role = user.Role,
                    ExpiresAt = DateTime.UtcNow.AddHours(8),
                    CreatedAt = user.CreatedAt
                };
            }

            return null;
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Checks if current user has any of the specified roles
    /// </summary>
    public bool HasRole(params string[] roles)
    {
        if (CurrentUser == null)
            return false;

        return roles.Contains(CurrentUser.Role);
    }

    /// <summary>
    /// Checks if current user can perform write operations
    /// </summary>
    public bool CanWrite()
    {
        return HasRole("Admin", "Operator");
    }

    /// <summary>
    /// Checks if current user can delete resources
    /// </summary>
    public bool CanDelete()
    {
        return HasRole("Admin");
    }
}

// DTOs matching API responses
public class LoginResponse
{
    public bool Success { get; set; }
    public string? Token { get; set; }
    public string? Message { get; set; }
    public UserInfoResponse? User { get; set; }
}

public class UserInfoResponse
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? FullName { get; set; }
    public string Role { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public bool IsActive { get; set; }
}
