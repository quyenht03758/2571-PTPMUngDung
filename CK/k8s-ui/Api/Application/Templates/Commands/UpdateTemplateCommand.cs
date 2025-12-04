// Application/Templates/Commands/UpdateTemplateCommand.cs
using K8sManager.Api.Domain.Entities;
using K8sManager.Api.Domain.Repositories;

namespace K8sManager.Api.Application.Templates.Commands;

public record UpdateTemplateCommand(
    int Id,
    string Name,
    string Description,
    string Category,
    string YamlContent,
    int UpdatedByUserId
);

public class UpdateTemplateCommandHandler
{
    private readonly ITemplateRepository _templateRepo;
    private readonly IAuditLogRepository _auditRepo;

    public UpdateTemplateCommandHandler(
        ITemplateRepository templateRepo,
        IAuditLogRepository auditRepo)
    {
        _templateRepo = templateRepo;
        _auditRepo = auditRepo;
    }

    public async Task<bool> HandleAsync(UpdateTemplateCommand command)
    {
        var existing = await _templateRepo.GetByIdAsync(command.Id).ConfigureAwait(false);
        if (existing == null) return false;

        existing.Name = command.Name;
        existing.Description = command.Description;
        existing.Category = command.Category;
        existing.YamlContent = command.YamlContent;
        existing.UpdatedAt = DateTime.UtcNow;
        existing.Version = existing.Version + 1;

        var result = await _templateRepo.UpdateAsync(existing).ConfigureAwait(false);

        if (result)
        {
            await _auditRepo.CreateAsync(AuditLog.Create(
                userId: command.UpdatedByUserId,
                action: "Update",
                success: true,
                resourceKind: "Template",
                resourceName: command.Name
            )).ConfigureAwait(false);
        }

        return result;
    }
}
