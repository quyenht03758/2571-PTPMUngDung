// Application/Templates/Commands/CreateTemplateCommand.cs
using K8sManager.Api.Domain.Entities;
using K8sManager.Api.Domain.Repositories;

namespace K8sManager.Api.Application.Templates.Commands;

public record CreateTemplateCommand(
    string Name,
    string Description,
    string Category,
    string YamlContent,
    int CreatedByUserId
);

public class CreateTemplateCommandHandler
{
    private readonly ITemplateRepository _templateRepo;
    private readonly IAuditLogRepository _auditRepo;

    public CreateTemplateCommandHandler(
        ITemplateRepository templateRepo,
        IAuditLogRepository auditRepo)
    {
        _templateRepo = templateRepo;
        _auditRepo = auditRepo;
    }

    public async Task<int> HandleAsync(CreateTemplateCommand command)
    {
        var template = new Template
        {
            Name = command.Name,
            Description = command.Description,
            Category = command.Category,
            YamlContent = command.YamlContent,
            CreatedBy = command.CreatedByUserId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            Version = 1
        };

        var templateId = await _templateRepo.CreateAsync(template).ConfigureAwait(false);

        await _auditRepo.CreateAsync(AuditLog.Create(
            userId: command.CreatedByUserId,
            action: "Create",
            success: true,
            resourceKind: "Template",
            resourceName: command.Name
        )).ConfigureAwait(false);

        return templateId;
    }
}
