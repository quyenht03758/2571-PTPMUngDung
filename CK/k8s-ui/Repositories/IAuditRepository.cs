using K8sManager.Models.Entities;

namespace K8sManager.Repositories
{
    public interface IAuditRepository
    {
        Task<int> AddAsync(AuditLog entry);
        Task<int> LogAsync(AuditLog entry); // Alias for compatibility
        Task<IEnumerable<AuditLog>> GetRecentAsync(int days = 7, bool? success = null, int limit = 100);
        Task<IEnumerable<AuditLog>> GetByUserIdAsync(int userId, int limit = 100);
    }
}
