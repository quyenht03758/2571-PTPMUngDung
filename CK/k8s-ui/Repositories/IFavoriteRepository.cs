using K8sManager.Models.Entities;

namespace K8sManager.Repositories
{
    public interface IFavoriteRepository
    {
        Task<int> AddAsync(Favorite f);
        Task<IEnumerable<Favorite>> ListByUserAsync(int userId);
    }
}
