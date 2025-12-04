using K8sManager.Models.Entities;

namespace K8sManager.Repositories;

public interface IUserSessionRepository
{
    Task<UserSession?> GetByTokenAsync(string token);
    Task<int> CreateAsync(UserSession session);
    Task<int> DeleteExpiredAsync();
    Task<int> DeleteByUserIdAsync(int userId);
}

public interface IUserClusterAccessRepository
{
    Task<IEnumerable<UserClusterAccess>> GetByUserIdAsync(int userId);
    Task<UserClusterAccess?> GetByUserAndClusterAsync(int userId, int clusterId);
    Task<int> CreateAsync(UserClusterAccess access);
    Task<int> UpdateAsync(UserClusterAccess access);
    Task<int> DeleteAsync(int id);
}

public interface ITemplateVersionRepository
{
    Task<IEnumerable<TemplateVersion>> GetByTemplateIdAsync(int templateId);
    Task<TemplateVersion?> GetCurrentVersionAsync(int templateId);
    Task<TemplateVersion?> GetByIdAsync(int id);
    Task<int> CreateAsync(TemplateVersion version);
    Task<int> SetCurrentVersionAsync(int templateId, int versionNumber);
}

public interface IPortForwardProfileRepository
{
    Task<IEnumerable<PortForwardProfile>> GetByUserIdAsync(int userId);
    Task<PortForwardProfile?> GetByIdAsync(int id);
    Task<int> CreateAsync(PortForwardProfile profile);
    Task<int> UpdateAsync(PortForwardProfile profile);
    Task<int> DeleteAsync(int id);
}

public interface IActivePortForwardRepository
{
    Task<IEnumerable<ActivePortForward>> GetActiveAsync(int userId);
    Task<ActivePortForward?> GetByIdAsync(int id);
    Task<int> CreateAsync(ActivePortForward forward);
    Task<int> UpdateStatusAsync(int id, string status, DateTime? stoppedAt = null);
    Task<int> StopAllByUserAsync(int userId);
}

public interface ITaskHistoryRepository
{
    Task<IEnumerable<TaskHistory>> GetByUserIdAsync(int userId, int limit = 100);
    Task<IEnumerable<TaskHistory>> GetByClusterIdAsync(int clusterId, int limit = 100);
    Task<IEnumerable<TaskHistory>> GetRecentAsync(int days = 7, int limit = 100);
    Task<TaskHistory?> GetByIdAsync(long id);
    Task<long> CreateAsync(TaskHistory task);
    Task<int> UpdateStatusAsync(long id, string status, DateTime? completedAt = null, string? result = null, string? errorMessage = null, int? duration = null);
}

public interface IResourceCacheRepository
{
    Task<ResourceCache?> GetAsync(int clusterId, string? @namespace, string resourceKind);
    Task<int> UpsertAsync(ResourceCache cache);
    Task<int> DeleteExpiredAsync();
    Task<int> InvalidateAsync(int clusterId, string? @namespace, string? resourceKind = null);
}

public interface INotificationRepository
{
    Task<IEnumerable<Notification>> GetByUserIdAsync(int userId, bool unreadOnly = false, int limit = 50);
    Task<int> GetUnreadCountAsync(int userId);
    Task<long> CreateAsync(Notification notification);
    Task<int> MarkAsReadAsync(long id);
    Task<int> MarkAllAsReadAsync(int userId);
    Task<int> DeleteOldAsync(int days = 30);
}

public interface IAppSettingRepository
{
    Task<AppSetting?> GetByKeyAsync(string key);
    Task<IEnumerable<AppSetting>> GetByCategoryAsync(string category);
    Task<IEnumerable<AppSetting>> GetAllAsync();
    Task<int> UpsertAsync(AppSetting setting);
    Task<int> DeleteAsync(string key);
}
