using K8sManager.Services;

namespace K8sManager.Controllers.ResourceOperations.Handlers
{
    /// <summary>
    /// Handles delete operations for any Kubernetes resource
    /// Single Responsibility: Only delete logic
    /// </summary>
    public class DeleteResourceHandler : IResourceOperationHandler
    {
        private readonly IK8sExplorerService _explorer;
        private readonly ApiAuditService? _auditService;

        public DeleteResourceHandler(IK8sExplorerService explorer, ApiAuditService? auditService = null)
        {
            _explorer = explorer;
            _auditService = auditService;
        }

        public async Task<OperationResult> ExecuteAsync(OperationContext context)
        {
            bool success = false;
            string? errorMessage = null;

            try
            {
                success = await _explorer.DeleteResourceAsync(
                    context.ResourceKind,
                    context.Namespace,
                    context.ResourceName).ConfigureAwait(false);

                // Audit log
                if (_auditService != null && context.UserId.HasValue)
                {
                    await _auditService.LogAsync(
                        context.UserId.Value,
                        "Delete",
                        context.ResourceKind,
                        context.ResourceName,
                        context.Namespace,
                        success,
                        errorMessage
                    ).ConfigureAwait(false);
                }

                return success
                    ? OperationResult.Ok($"Successfully deleted {context.ResourceKind} '{context.ResourceName}'")
                    : OperationResult.Fail("Delete operation returned false");
            }
            catch (System.Exception ex)
            {
                errorMessage = ex.Message;

                // Audit log for failure
                if (_auditService != null && context.UserId.HasValue)
                {
                    await _auditService.LogAsync(
                        context.UserId.Value,
                        "Delete",
                        context.ResourceKind,
                        context.ResourceName,
                        context.Namespace,
                        false,
                        errorMessage
                    ).ConfigureAwait(false);
                }

                return OperationResult.Fail($"Failed to delete: {errorMessage}");
            }
        }
    }
}
