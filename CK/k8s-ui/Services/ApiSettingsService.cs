// Services/ApiSettingsService.cs - Settings service using API instead of direct database
using K8sManager.Models.Entities;

namespace K8sManager.Services;

/// <summary>
/// Settings service wrapper that calls REST API instead of direct database access
/// </summary>
public class ApiSettingsService
{
    private readonly ApiClient _apiClient;

    public ApiSettingsService(ApiClient apiClient)
    {
        _apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
    }

    public async Task<List<AppSetting>> GetAllSettingsAsync()
    {
        try
        {
            var response = await _apiClient.GetSettingsAsync<SettingsResponse>().ConfigureAwait(false);
            return response?.Items ?? new List<AppSetting>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to get settings: {ex.Message}");
            return new List<AppSetting>();
        }
    }

    public async Task<AppSetting?> GetSettingAsync(string key)
    {
        try
        {
            return await _apiClient.GetSettingByKeyAsync<AppSetting>(key).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to get setting {key}: {ex.Message}");
            return null;
        }
    }

    public async Task<string?> GetSettingValueAsync(string key, string? defaultValue = null)
    {
        try
        {
            var setting = await GetSettingAsync(key).ConfigureAwait(false);
            return setting?.Value ?? defaultValue;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to get setting value {key}: {ex.Message}");
            return defaultValue;
        }
    }

    public async Task<bool> SaveSettingAsync(string key, string value, string? description = null, string? category = null)
    {
        try
        {
            var request = new
            {
                Key = key,
                Value = value,
                Description = description,
                Category = category
            };

            await _apiClient.SaveSettingAsync<object, object>(request).ConfigureAwait(false);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to save setting: {ex.Message}");
            return false;
        }
    }

    public async Task UpsertSettingAsync(string key, string value)
    {
        await SaveSettingAsync(key, value);
    }

    public async Task<List<AppSetting>> GetSettingsByCategoryAsync(string category)
    {
        try
        {
            var response = await _apiClient.GetSettingsByCategoryAsync<SettingsResponse>(category).ConfigureAwait(false);
            return response?.Items ?? new List<AppSetting>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to get settings by category: {ex.Message}");
            return new List<AppSetting>();
        }
    }
}

// DTOs matching API responses
public class SettingsResponse
{
    public List<AppSetting> Items { get; set; } = new();
}
