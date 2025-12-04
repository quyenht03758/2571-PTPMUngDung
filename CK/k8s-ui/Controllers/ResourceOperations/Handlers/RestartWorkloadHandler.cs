using K8sManager.Services;

namespace K8sManager.Controllers.ResourceOperations.Handlers
{
    /// <summary>
    /// Handles restart operations for workloads (Deployment, StatefulSet, DaemonSet)
    /// Single Responsibility: Only restart logic
    /// </summary>
    public class RestartWorkloadHandler : IResourceOperationHandler
    {
        private readonly IK8sExplorerService _explorer;
        private readonly ApiAuditService? _auditService;

        public RestartWorkloadHandler(IK8sExplorerService explorer, ApiAuditService? auditService = null)
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
                // Execute restart based on resource kind
                success = context.ResourceKind switch
                {
                    "Deployment" => await _explorer.RestartDeploymentAsync(
                        context.Namespace, context.ResourceName).ConfigureAwait(false),
                    "StatefulSet" => await _explorer.RestartStatefulSetAsync(
                        context.Namespace, context.ResourceName).ConfigureAwait(false),
                    "DaemonSet" => await _explorer.RestartDaemonSetAsync(
                        context.Namespace, context.ResourceName).ConfigureAwait(false),
                    _ => throw new System.InvalidOperationException(
                        $"Resource kind '{context.ResourceKind}' does not support restart")
                };

                // Audit log
                if (_auditService != null && context.UserId.HasValue)
                {
                    await _auditService.LogAsync(
                        context.UserId.Value,
                        "Restart",
                        context.ResourceKind,
                        context.ResourceName,
                        context.Namespace,
                        success,
                        errorMessage
                    ).ConfigureAwait(false);
                }

                return success
                    ? OperationResult.Ok($"Successfully restarted {context.ResourceKind} '{context.ResourceName}'")
                    : OperationResult.Fail("Restart operation returned false");
            }
            catch (System.Exception ex)
            {
                errorMessage = ex.Message;

                // Audit log for failure
                if (_auditService != null && context.UserId.HasValue)
                {
                    await _auditService.LogAsync(
                        context.UserId.Value,
                        "Restart",
                        context.ResourceKind,
                        context.ResourceName,
                        context.Namespace,
                        false,
                        errorMessage
                    ).ConfigureAwait(false);
                }

                return OperationResult.Fail($"Failed to restart: {errorMessage}");
            }
        }
    }
}
