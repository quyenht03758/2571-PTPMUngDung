using K8sManager.Models.Entities;

namespace K8sManager.Repositories
{
    public interface IClusterRepository
    {
        Task<int> AddAsync(ClusterConfig c);
        Task<IEnumerable<ClusterConfig>> GetAllAsync();
    }
}
