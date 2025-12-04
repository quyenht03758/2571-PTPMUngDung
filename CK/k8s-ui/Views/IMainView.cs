namespace K8sManager.Views
{
    public interface IMainView
    {
        event EventHandler RefreshRequested;
        event EventHandler<ScaleRequest> ScaleRequested;
        event EventHandler<RestartPodRequest> RestartPodRequested;
        event EventHandler<TailLogsRequest> TailLogsRequested;

        string CurrentNamespace { get; }

        // NEW
        void BindNamespaces(IEnumerable<string> namespaces);

        void BindPods(IEnumerable<PodViewModel> pods);
        void AppendLog(string line);
        void ShowMessage(string text);

        void ShowYaml(string yaml);
        void ShowDescribe(string desc);
    }

    public record ScaleRequest(string Namespace, string Name, int Replicas);
    public record RestartPodRequest(string Namespace, string Name);
    public record TailLogsRequest(string Namespace, string Pod, string? Container);

    public class PodViewModel
    {
        public string Name { get; set; } = "";
        public string Namespace { get; set; } = "";
        public string Node { get; set; } = "";
        public string Phase { get; set; } = "";
        public string Ready { get; set; } = "";
    }
}
