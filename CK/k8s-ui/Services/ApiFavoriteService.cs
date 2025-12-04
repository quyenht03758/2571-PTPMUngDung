// Services/ApiFavoriteService.cs - Favorite service using API instead of direct database
using K8sManager.Models.Entities;

namespace K8sManager.Services;

/// <summary>
/// Favorite service wrapper that calls REST API instead of direct database access
/// </summary>
public class ApiFavoriteService
{
    private readonly ApiClient _apiClient;

    public ApiFavoriteService(ApiClient apiClient)
    {
        _apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
    }

    public async Task<List<Favorite>> GetFavoritesAsync(string? resourceKind = null, string? namespace_ = null)
    {
        try
        {
            // API returns List<Favorite> directly, not wrapped in response object
            var favorites = await _apiClient.GetFavoritesAsync<List<Favorite>>(resourceKind, namespace_).ConfigureAwait(false);
            return favorites ?? new List<Favorite>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to get favorites: {ex.Message}");
            return new List<Favorite>();
        }
    }

    public async Task<int> AddFavoriteAsync(int userId, string resourceKind, string resourceName, string? clusterName, string? namespace_, string? notes)
    {
        try
        {
            var request = new
            {
                ResourceType = resourceKind,
                ResourceName = resourceName,
                ClusterName = clusterName,
                Namespace = namespace_,
                Notes = notes
            };

            var response = await _apiClient.AddFavoriteAsync<object, CreateFavoriteResponse>(request).ConfigureAwait(false);
            return response?.Id ?? 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to add favorite: {ex.Message}");
            return 0;
        }
    }

    public async Task<bool> UpdateFavoriteAsync(int id, string? notes)
    {
        try
        {
            var request = new
            {
                Notes = notes
            };

            // Backend returns 204 NoContent on success
            await _apiClient.UpdateFavoriteAsync<object, object>(id, request).ConfigureAwait(false);
            return true;
        }
        catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            Console.WriteLine($"Favorite {id} not found");
            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to update favorite: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> DeleteFavoriteAsync(int id)
    {
        return await RemoveFavoriteAsync(id);
    }

    public async Task CreateFavoriteAsync(string resourceType, string resourceName, string ns, string notes)
    {
        await AddFavoriteAsync(0, resourceType, resourceName, null, ns, notes);
    }

    public async Task<bool> RemoveFavoriteAsync(int id)
    {
        try
        {
            // Backend returns 204 NoContent on success
            await _apiClient.RemoveFavoriteAsync<object>(id).ConfigureAwait(false);
            return true;
        }
        catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            Console.WriteLine($"Favorite {id} not found");
            return false;
        }
        catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.Forbidden)
        {
            Console.WriteLine($"Not authorized to delete favorite {id}");
            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to remove favorite: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> IsFavoriteAsync(int userId, string resourceKind, string resourceName, string? namespace_)
    {
        try
        {
            var favorites = await GetFavoritesAsync(resourceKind, namespace_).ConfigureAwait(false);
            return favorites.Exists(f =>
                f.ResourceKind == resourceKind &&
                f.ResourceName == resourceName &&
                f.Namespace == namespace_);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to check favorite: {ex.Message}");
            return false;
        }
    }
}

// DTOs matching API responses
public class FavoritesResponse
{
    public List<Favorite> Items { get; set; } = new();
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
}

public class CreateFavoriteResponse
{
    public int Id { get; set; }
}
