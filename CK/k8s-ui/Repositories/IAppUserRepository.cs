using K8sManager.Models.Entities;

namespace K8sManager.Repositories
{
    public interface IAppUserRepository
    {
        Task<AppUser?> FindByUsernameAsync(string username);
        Task<AppUser?> GetByUsernameAsync(string username); // Alias for compatibility
        Task<AppUser?> GetByIdAsync(int id);
        Task<int> CreateAsync(AppUser user);
        Task<int> UpdateAsync(AppUser user);
    }
}
