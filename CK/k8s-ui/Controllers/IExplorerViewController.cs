namespace K8sManager.Controllers
{
    /// <summary>
    /// Interface for View communication - Controller talks to View through this
    /// </summary>
    public interface IExplorerViewController
    {
        // Context & Namespace binding
        void BindContexts(IEnumerable<string> contexts, string? currentContext = null);
        void BindNamespaces(IEnumerable<string> namespaces);

        // Resource counts
        void UpdateResourceCounts(Dictionary<string, int> counts);

        // List binding (generic)
        void BindResourceList<T>(IEnumerable<T> resources);

        // Status & Feedback
        void ShowStatusMessage(string message, StatusType type);
        void ShowError(string title, string message);
        void ShowSuccess(string title, string message);
        void ShowWarning(string title, string message);
        bool ShowConfirmation(string title, string message);

        // Dialogs
        int? ShowScaleDialog(string resourceName, int currentReplicas);

        // Properties
        string SelectedNamespace { get; }
        string SelectedContext { get; }
        string CurrentResourceKind { get; }
    }

    public enum StatusType
    {
        Info,
        Success,
        Warning,
        Error,
        Loading
    }
}
