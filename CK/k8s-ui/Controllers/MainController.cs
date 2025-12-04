// Controllers/MainController.cs
using K8sManager.Services;
using K8sManager.Views;

namespace K8sManager.Controllers
{
    public class MainController
    {
        private readonly IMainView _view;
        private readonly IK8sService _k8s;
        private readonly IAuditService _audit;
        private readonly ITemplateService _templates; // chưa dùng cũng không sao
        private readonly AppConfig _cfg;              // giữ để dùng về sau (context, v.v.)
        private int _userId = 1; // demo

        public MainController(
            IMainView view,
            IK8sService k8s,
            IAuditService audit,
            ITemplateService templates,
            AppConfig cfg)
        {
            _view = view; _k8s = k8s; _audit = audit; _templates = templates; _cfg = cfg;

            _view.RefreshRequested += async (s, e) => await LoadPodsAsync().ConfigureAwait(false);
            _view.ScaleRequested += async (s, req) => await ScaleAsync(req.Namespace, req.Name, req.Replicas).ConfigureAwait(false);
            _view.RestartPodRequested += async (s, req) => await RestartPodAsync(req.Namespace, req.Name).ConfigureAwait(false);
            _view.TailLogsRequested += async (s, req) => await TailLogsAsync(req.Namespace, req.Pod, req.Container).ConfigureAwait(false);
        }

        public async Task LoadNamespacesAsync()
        {
            try
            {
                var namespaces = await _k8s.ListNamespacesAsync().ConfigureAwait(false);
                _view.BindNamespaces(namespaces);
            }
            catch (System.Exception ex)
            {
                _view.ShowMessage("Không tải được namespaces: " + ex.Message);
            }
        }

        public async Task LoadPodsAsync()
        {
            try
            {
                var pods = await _k8s.ListPodsAsync(_view.CurrentNamespace).ConfigureAwait(false);
                var vms = pods.Select(p => new K8sManager.Views.PodViewModel
                {
                    Name = p.Metadata?.Name ?? "",
                    Namespace = p.Metadata?.NamespaceProperty ?? "",
                    Node = p.Spec?.NodeName ?? "",
                    Phase = p.Status?.Phase ?? "",
                    Ready = $"{p.Status?.ContainerStatuses?.Count(c => c.Ready) ?? 0}/{p.Status?.ContainerStatuses?.Count ?? 0}"
                }).ToList();

                _view.BindPods(vms);
            }
            catch (System.Exception ex)
            {
                _view.ShowMessage("Không tải được Pods: " + ex.Message);
            }
        }

        private async Task ScaleAsync(string ns, string name, int replicas)
        {
            var ok = await _k8s.ScaleDeploymentAsync(ns, name, replicas).ConfigureAwait(false);
            await _audit.LogAsync(_userId, "ScaleDeployment", "Deployment", name, ns, ok, ok ? null : "Failed").ConfigureAwait(false);
            _view.ShowMessage(ok ? "Scaled successfully" : "Scale failed");
            await LoadPodsAsync().ConfigureAwait(false);
        }

        private async Task RestartPodAsync(string ns, string name)
        {
            var ok = await _k8s.DeletePodAsync(ns, name).ConfigureAwait(false);
            await _audit.LogAsync(_userId, "RestartPod(Delete)", "Pod", name, ns, ok, ok ? null : "Failed").ConfigureAwait(false);
            _view.ShowMessage(ok ? "Restart requested" : "Restart failed");
            await LoadPodsAsync().ConfigureAwait(false);
        }

        private async Task TailLogsAsync(string ns, string pod, string? container)
        {
            await foreach (var line in _k8s.TailLogsAsync(ns, pod, container).ConfigureAwait(false))
                _view.AppendLog(line);
        }
    }
}
