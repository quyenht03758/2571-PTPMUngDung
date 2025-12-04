namespace K8sManager.Controllers.ResourceOperations
{
    /// <summary>
    /// Base interface for all resource operations (Scale, Restart, Delete, etc.)
    /// Each operation implements this to handle specific logic
    /// </summary>
    public interface IResourceOperationHandler
    {
        Task<OperationResult> ExecuteAsync(OperationContext context);
    }

    /// <summary>
    /// Context passed to operation handlers
    /// </summary>
    public class OperationContext
    {
        public required string Namespace { get; init; }
        public required string ResourceKind { get; init; }
        public required string ResourceName { get; init; }
        public int? UserId { get; init; }
        public Dictionary<string, object>? Parameters { get; init; }
    }

    /// <summary>
    /// Result of an operation
    /// </summary>
    public class OperationResult
    {
        public bool Success { get; init; }
        public string? Message { get; init; }
        public string? ErrorMessage { get; init; }
        public object? Data { get; init; }

        public static OperationResult Ok(string? message = null, object? data = null)
            => new() { Success = true, Message = message, Data = data };

        public static OperationResult Fail(string errorMessage)
            => new() { Success = false, ErrorMessage = errorMessage };
    }
}
