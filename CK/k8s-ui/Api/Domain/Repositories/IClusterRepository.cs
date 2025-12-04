using K8sManager.Api.Domain.Entities;

namespace K8sManager.Api.Domain.Repositories;

public interface IClusterRepository
{
    Task<int> AddAsync(ClusterConfig cluster);
    Task<IEnumerable<ClusterConfig>> GetAllAsync();
    Task<ClusterConfig?> GetByIdAsync(int id);
    Task<ClusterConfig?> GetDefaultAsync();
    Task UpdateAsync(ClusterConfig cluster);
    Task DeleteAsync(int id);
    Task SetDefaultAsync(int id);
}
