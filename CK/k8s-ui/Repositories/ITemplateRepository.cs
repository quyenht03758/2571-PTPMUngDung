using K8sManager.Models.Entities;

namespace K8sManager.Repositories
{
    public interface ITemplateRepository
    {
        Task<int> AddAsync(Template t);
        Task<IEnumerable<Template>> GetAllAsync();
    }
}
