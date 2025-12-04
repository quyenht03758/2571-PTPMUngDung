using K8sManager.Services;

namespace K8sManager.Controllers.ResourceOperations.Handlers
{
    /// <summary>
    /// Handles scaling operations for workloads (Deployment, StatefulSet)
    /// Single Responsibility: Only scale logic
    /// </summary>
    public class ScaleWorkloadHandler : IResourceOperationHandler
    {
        private readonly IK8sExplorerService _explorer;
        private readonly ApiAuditService? _auditService;

        public ScaleWorkloadHandler(IK8sExplorerService explorer, ApiAuditService? auditService = null)
        {
            _explorer = explorer;
            _auditService = auditService;
        }

        public async Task<OperationResult> ExecuteAsync(OperationContext context)
        {
            // Validate parameters
            if (context.Parameters == null ||
                !context.Parameters.TryGetValue("Replicas", out var replicasObj) ||
                replicasObj == null)
            {
                return OperationResult.Fail("Replicas parameter is required");
            }

            if (!int.TryParse(replicasObj.ToString(), out int replicas))
            {
                return OperationResult.Fail("Invalid replicas value");
            }

            if (replicas < 0)
            {
                return OperationResult.Fail("Replicas cannot be negative");
            }

            bool success = false;
            string? errorMessage = null;

            try
            {
                // Execute scaling based on resource kind
                success = context.ResourceKind switch
                {
                    "Deployment" => await _explorer.ScaleDeploymentAsync(
                        context.Namespace, context.ResourceName, replicas).ConfigureAwait(false),
                    "StatefulSet" => await _explorer.ScaleStatefulSetAsync(
                        context.Namespace, context.ResourceName, replicas).ConfigureAwait(false),
                    _ => throw new System.InvalidOperationException(
                        $"Resource kind '{context.ResourceKind}' does not support scaling")
                };

                // Audit log
                if (_auditService != null && context.UserId.HasValue)
                {
                    await _auditService.LogAsync(
                        context.UserId.Value,
                        "Scale",
                        context.ResourceKind,
                        context.ResourceName,
                        context.Namespace,
                        success,
                        errorMessage
                    ).ConfigureAwait(false);
                }

                return success
                    ? OperationResult.Ok($"Successfully scaled {context.ResourceKind} '{context.ResourceName}' to {replicas} replicas")
                    : OperationResult.Fail("Scale operation returned false");
            }
            catch (System.Exception ex)
            {
                errorMessage = ex.Message;

                // Audit log for failure
                if (_auditService != null && context.UserId.HasValue)
                {
                    await _auditService.LogAsync(
                        context.UserId.Value,
                        "Scale",
                        context.ResourceKind,
                        context.ResourceName,
                        context.Namespace,
                        false,
                        errorMessage
                    ).ConfigureAwait(false);
                }

                return OperationResult.Fail($"Failed to scale: {errorMessage}");
            }
        }
    }
}
