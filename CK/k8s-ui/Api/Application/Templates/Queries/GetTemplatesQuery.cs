// Application/Templates/Queries/GetTemplatesQuery.cs
using K8sManager.Api.Domain.Entities;
using K8sManager.Api.Domain.Repositories;

namespace K8sManager.Api.Application.Templates.Queries;

public record GetTemplatesQuery(
    string? Category = null,
    string? SearchTerm = null
);

public class GetTemplatesQueryHandler
{
    private readonly ITemplateRepository _templateRepo;

    public GetTemplatesQueryHandler(ITemplateRepository templateRepo)
    {
        _templateRepo = templateRepo;
    }

    public async Task<List<Template>> HandleAsync(GetTemplatesQuery query)
    {
        var templates = await _templateRepo.GetAllAsync(query.Category, query.SearchTerm).ConfigureAwait(false);
        return templates.ToList();
    }
}
