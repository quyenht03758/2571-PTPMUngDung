// Configuration/AppConfig.cs
namespace K8sManager
{
    public class AppConfig
    {
        public ConnectionStrings ConnectionStrings { get; set; } = new();
        public LoggingConfig Logging { get; set; } = new();
        public K8sConfig Kubernetes { get; set; } = new();
    }

    public class ConnectionStrings
    {
        public string Default { get; set; } = "";
    }

    public class LoggingConfig
    {
        public string FilePath { get; set; } = "logs/app.log";
    }

    public class K8sConfig
    {
        public string KubeconfigPath { get; set; } = "";
        public string? ContextName { get; set; }
    }
}
