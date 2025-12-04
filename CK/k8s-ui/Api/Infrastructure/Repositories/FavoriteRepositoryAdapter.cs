using Dapper;
using K8sManager.Api.Domain.Entities;
using K8sManager.Api.Domain.Repositories;
using K8sManager.Infrastructure;

namespace K8sManager.Api.Infrastructure.Repositories;

public class FavoriteRepositoryAdapter : IFavoriteRepository
{
    private readonly DapperConnectionFactory _dbFactory;

    public FavoriteRepositoryAdapter(DapperConnectionFactory dbFactory)
    {
        _dbFactory = dbFactory;
    }

    public async Task<Favorite?> GetByIdAsync(int id)
    {
        using var conn = _dbFactory.Create();
        var sql = @"
            SELECT f.*, c.Name AS ClusterName
            FROM Favorite f
            LEFT JOIN ClusterConfig c ON f.ClusterId = c.Id
            WHERE f.Id = @Id";

        var dto = await conn.QueryFirstOrDefaultAsync<FavoriteDto>(sql, new { Id = id }).ConfigureAwait(false);
        return dto == null ? null : MapToDomain(dto);
    }

    public async Task<IEnumerable<Favorite>> GetByUserIdAsync(int userId)
    {
        using var conn = _dbFactory.Create();
        var sql = @"
            SELECT f.*, c.Name AS ClusterName
            FROM Favorite f
            LEFT JOIN ClusterConfig c ON f.ClusterId = c.Id
            WHERE f.UserId = @UserId
            ORDER BY f.SortOrder ASC, f.CreatedAt DESC";

        var rows = await conn.QueryAsync<FavoriteDto>(sql, new { UserId = userId }).ConfigureAwait(false);
        return rows.Select(MapToDomain);
    }

    public async Task<int> CreateAsync(Favorite favorite)
    {
        using var conn = _dbFactory.Create();
        var sql = @"
            INSERT INTO Favorite (UserId, ClusterId, Namespace, ResourceKind, ResourceName,
                                  DisplayName, Notes, SortOrder, CreatedAt)
            VALUES (@UserId, @ClusterId, @Namespace, @ResourceKind, @ResourceName,
                    @DisplayName, @Notes, @SortOrder, @CreatedAt);
            SELECT CAST(SCOPE_IDENTITY() AS int);";

        var id = await conn.ExecuteScalarAsync<int>(sql, new
        {
            favorite.UserId,
            favorite.ClusterId,
            favorite.Namespace,
            favorite.ResourceKind,
            favorite.ResourceName,
            favorite.DisplayName,
            favorite.Notes,
            favorite.SortOrder,
            favorite.CreatedAt
        }).ConfigureAwait(false);

        return id;
    }

    public async Task<bool> UpdateAsync(Favorite favorite)
    {
        using var conn = _dbFactory.Create();
        var sql = @"
            UPDATE Favorite
            SET DisplayName = @DisplayName,
                Notes = @Notes,
                SortOrder = @SortOrder
            WHERE Id = @Id";

        var rowsAffected = await conn.ExecuteAsync(sql, new
        {
            favorite.Id,
            favorite.DisplayName,
            favorite.Notes,
            favorite.SortOrder
        }).ConfigureAwait(false);
        return rowsAffected > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        using var conn = _dbFactory.Create();
        var rowsAffected = await conn.ExecuteAsync("DELETE FROM Favorite WHERE Id = @Id", new { Id = id }).ConfigureAwait(false);
        return rowsAffected > 0;
    }

    private static Favorite MapToDomain(FavoriteDto dto)
    {
        return Favorite.Load(
            id: dto.Id,
            userId: dto.UserId,
            clusterId: dto.ClusterId,
            displayName: dto.DisplayName,
            createdAt: dto.CreatedAt,
            @namespace: dto.Namespace,
            resourceKind: dto.ResourceKind,
            resourceName: dto.ResourceName,
            notes: dto.Notes,
            sortOrder: dto.SortOrder,
            clusterName: dto.ClusterName
        );
    }

    private class FavoriteDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ClusterId { get; set; }
        public string? Namespace { get; set; }
        public string? ResourceKind { get; set; }
        public string? ResourceName { get; set; }
        public string DisplayName { get; set; } = null!;
        public string? Notes { get; set; }
        public int SortOrder { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? ClusterName { get; set; }
    }
}
