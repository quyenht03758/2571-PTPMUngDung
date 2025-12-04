// Application/Templates/Queries/GetTemplateByIdQuery.cs
using K8sManager.Api.Domain.Entities;
using K8sManager.Api.Domain.Repositories;

namespace K8sManager.Api.Application.Templates.Queries;

public record GetTemplateByIdQuery(int Id);

public class GetTemplateByIdQueryHandler
{
    private readonly ITemplateRepository _templateRepo;

    public GetTemplateByIdQueryHandler(ITemplateRepository templateRepo)
    {
        _templateRepo = templateRepo;
    }

    public async Task<Template?> HandleAsync(GetTemplateByIdQuery query)
    {
        // Always include versions when getting template by ID
        return await _templateRepo.GetByIdAsync(query.Id, includeVersions: true).ConfigureAwait(false);
    }
}
