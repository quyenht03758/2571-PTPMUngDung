using K8sManager.Controllers.ResourceOperations;
using K8sManager.Controllers.ResourceOperations.Handlers;
using K8sManager.Infrastructure;
using K8sManager.Services;

namespace K8sManager.Controllers
{
    /// <summary>
    /// Clean Controller - Orchestrates operations between View and Services
    /// Responsibilities:
    /// 1. Coordinate calls to services
    /// 2. Delegate operations to specialized handlers
    /// 3. Pass results back to view
    /// Does NOT: Create dialogs, show MessageBoxes, format data for UI
    /// </summary>
    public class CleanExplorerController
    {
        private readonly IExplorerViewController _view;
        private readonly IK8sService _k8sService;
        private readonly IK8sExplorerService _explorerService;
        private readonly K8sClientFactory _clientFactory;
        private readonly ApiAuditService? _auditService;

        // Operation handlers (Strategy pattern)
        private readonly IResourceOperationHandler _scaleHandler;
        private readonly IResourceOperationHandler _restartHandler;
        private readonly IResourceOperationHandler _deleteHandler;

        private int? _currentUserId;

        public CleanExplorerController(
            IExplorerViewController view,
            IK8sService k8sService,
            IK8sExplorerService explorerService,
            K8sClientFactory clientFactory,
            ApiAuditService? auditService = null)
        {
            _view = view;
            _k8sService = k8sService;
            _explorerService = explorerService;
            _clientFactory = clientFactory;
            _auditService = auditService;

            // Initialize operation handlers
            _scaleHandler = new ScaleWorkloadHandler(_explorerService, _auditService);
            _restartHandler = new RestartWorkloadHandler(_explorerService, _auditService);
            _deleteHandler = new DeleteResourceHandler(_explorerService, _auditService);
        }

        public void SetCurrentUser(int userId)
        {
            _currentUserId = userId;
        }

        // ============ INITIALIZATION ============

        public async Task InitializeAsync()
        {
            try
            {
                _view.ShowStatusMessage("Initializing...", StatusType.Loading);

                // Load contexts from kubeconfig (this always works even without real cluster)
                var contexts = _clientFactory.GetAvailableContexts();
                var currentContext = _clientFactory.GetCurrentContext();

                if (contexts.Count == 0)
                {
                    _view.ShowWarning("No Contexts", "No Kubernetes contexts found in kubeconfig.\n\nPlease configure your kubeconfig file or connect to a cluster.");
                    _view.BindContexts(new[] { "No context available" });
                    _view.BindNamespaces(new[] { "default" });
                    _view.ShowStatusMessage("No cluster connected", StatusType.Warning);
                    return;
                }

                // Bind contexts to UI (this should work with mock kubeconfig)
                _view.BindContexts(contexts, currentContext);
                _view.ShowStatusMessage($"Loaded {contexts.Count} contexts", StatusType.Info);

                // Try to connect to cluster (this may fail if no real cluster)
                try
                {
                    var namespaces = await _k8sService.ListNamespacesAsync().ConfigureAwait(false);
                    _view.BindNamespaces(namespaces);

                    if (namespaces.Count > 0)
                    {
                        // Load resource counts for first namespace
                        await LoadResourceCountsAsync(namespaces[0]).ConfigureAwait(false);

                        // Auto-load pods
                        await LoadResourceListAsync(namespaces[0], "Pods").ConfigureAwait(false);
                    }

                    _view.ShowStatusMessage("Connected to cluster", StatusType.Success);
                }
                catch (Exception clusterEx)
                {
                    // Cluster connection failed but we still have contexts
                    _view.BindNamespaces(new[] { "default" });
                    _view.ShowStatusMessage($"Contexts loaded, but cluster unreachable: {clusterEx.Message}", StatusType.Warning);
                    // Don't show error dialog - just warn in status bar
                }
            }
            catch (Exception ex)
            {
                _view.ShowError("Initialization Error",
                    $"Failed to initialize: {ex.Message}\n\nThe application may not be fully functional.");
                _view.BindContexts(new[] { "No context available" });
                _view.BindNamespaces(new[] { "default" });
            }
        }

        // ============ CONTEXT & NAMESPACE OPERATIONS ============

        public async Task SwitchContextAsync(string context)
        {
            try
            {
                _view.ShowStatusMessage($"Switching to {context}...", StatusType.Loading);
                _clientFactory.SetContext(context);

                // Reload namespaces for new context
                var namespaces = await _k8sService.ListNamespacesAsync().ConfigureAwait(false);
                _view.BindNamespaces(namespaces);

                if (namespaces.Count > 0)
                {
                    await LoadResourceCountsAsync(namespaces[0]).ConfigureAwait(false);
                }

                _view.ShowStatusMessage($"Connected to {context}", StatusType.Success);
            }
            catch (Exception ex)
            {
                _view.ShowError("Context Switch Failed", ex.Message);
            }
        }

        public async Task LoadResourceCountsAsync(string namespaceName)
        {
            try
            {
                _view.ShowStatusMessage("Loading resource counts...", StatusType.Loading);

                var counts = new Dictionary<string, int>();

                // Execute all count queries in parallel
                var tasks = new Dictionary<string, Task<int>>
                {
                    ["Pods"] = CountResourcesAsync(() => _k8sService.ListPodsAsync(namespaceName)),
                    ["Deployments"] = CountResourcesAsync(() => _explorerService.ListDeploymentsAsync(namespaceName)),
                    ["StatefulSets"] = CountResourcesAsync(() => _explorerService.ListStatefulSetsAsync(namespaceName)),
                    ["DaemonSets"] = CountResourcesAsync(() => _explorerService.ListDaemonSetsAsync(namespaceName)),
                    ["ReplicaSets"] = CountResourcesAsync(() => _explorerService.ListReplicaSetsAsync(namespaceName)),
                    ["Jobs"] = CountResourcesAsync(() => _explorerService.ListJobsAsync(namespaceName)),
                    ["CronJobs"] = CountResourcesAsync(() => _explorerService.ListCronJobsAsync(namespaceName)),
                    ["Services"] = CountResourcesAsync(() => _explorerService.ListServicesAsync(namespaceName)),
                    ["Ingresses"] = CountResourcesAsync(() => _explorerService.ListIngressesAsync(namespaceName)),
                    ["ConfigMaps"] = CountResourcesAsync(() => _explorerService.ListConfigMapsAsync(namespaceName)),
                    ["Secrets"] = CountResourcesAsync(() => _explorerService.ListSecretsAsync(namespaceName)),
                    ["PersistentVolumeClaims"] = CountResourcesAsync(() => _explorerService.ListPersistentVolumeClaimsAsync(namespaceName)),
                };

                await Task.WhenAll(tasks.Values).ConfigureAwait(false);

                foreach (var kvp in tasks)
                {
                    counts[kvp.Key] = kvp.Value.Result;
                }

                _view.UpdateResourceCounts(counts);
                _view.ShowStatusMessage($"Loaded counts for namespace '{namespaceName}'", StatusType.Success);
            }
            catch (Exception ex)
            {
                _view.ShowError("Failed to Load Counts", ex.Message);
            }
        }

        private async Task<int> CountResourcesAsync<T>(Func<Task<IList<T>>> operation)
        {
            try
            {
                var result = await operation().ConfigureAwait(false);
                return result.Count;
            }
            catch
            {
                return 0; // Return 0 on error instead of throwing
            }
        }

        // ============ RESOURCE LIST LOADING ============

        public async Task LoadResourceListAsync(string namespaceName, string resourceKind)
        {
            try
            {
                _view.ShowStatusMessage($"Loading {resourceKind}...", StatusType.Loading);

                // Delegate to specialized loaders
                await ((resourceKind switch
                {
                    "Pods" => LoadPodsAsync(namespaceName),
                    "Deployments" => LoadDeploymentsAsync(namespaceName),
                    "StatefulSets" => LoadStatefulSetsAsync(namespaceName),
                    "DaemonSets" => LoadDaemonSetsAsync(namespaceName),
                    "ReplicaSets" => LoadReplicaSetsAsync(namespaceName),
                    "Jobs" => LoadJobsAsync(namespaceName),
                    "CronJobs" => LoadCronJobsAsync(namespaceName),
                    "Services" => LoadServicesAsync(namespaceName),
                    "ConfigMaps" => LoadConfigMapsAsync(namespaceName),
                    "Secrets" => LoadSecretsAsync(namespaceName),
                    "ServiceAccounts" => LoadServiceAccountsAsync(namespaceName),
                    "Roles" => LoadRolesAsync(namespaceName),
                    "Nodes" => LoadNodesAsync(),
                    "Namespaces" => LoadNamespacesAsync(),
                    _ => throw new NotSupportedException($"Resource kind '{resourceKind}' is not supported")
                }).ConfigureAwait(false));

                _view.ShowStatusMessage($"Loaded {resourceKind}", StatusType.Success);
            }
            catch (Exception ex)
            {
                _view.ShowError($"Failed to Load {resourceKind}", ex.Message);
            }
        }

        private async Task LoadPodsAsync(string namespaceName)
        {
            var pods = await _k8sService.ListPodsAsync(namespaceName).ConfigureAwait(false);
            var viewModels = pods.Select(p => new PodViewModel(p));
            _view.BindResourceList(viewModels);
        }

        private async Task LoadDeploymentsAsync(string namespaceName)
        {
            var deployments = await _explorerService.ListDeploymentsAsync(namespaceName).ConfigureAwait(false);
            var viewModels = deployments.Select(d => new DeploymentViewModel(d));
            _view.BindResourceList(viewModels);
        }

        private async Task LoadStatefulSetsAsync(string namespaceName)
        {
            var statefulSets = await _explorerService.ListStatefulSetsAsync(namespaceName).ConfigureAwait(false);
            var viewModels = statefulSets.Select(s => new StatefulSetViewModel(s));
            _view.BindResourceList(viewModels);
        }

        private async Task LoadDaemonSetsAsync(string namespaceName)
        {
            var daemonSets = await _explorerService.ListDaemonSetsAsync(namespaceName).ConfigureAwait(false);
            var viewModels = daemonSets.Select(d => new DaemonSetViewModel(d));
            _view.BindResourceList(viewModels);
        }

        private async Task LoadServicesAsync(string namespaceName)
        {
            var services = await _explorerService.ListServicesAsync(namespaceName).ConfigureAwait(false);
            var viewModels = services.Select(s => new ServiceViewModel(s));
            _view.BindResourceList(viewModels);
        }

        private async Task LoadConfigMapsAsync(string namespaceName)
        {
            var configMaps = await _explorerService.ListConfigMapsAsync(namespaceName).ConfigureAwait(false);
            var viewModels = configMaps.Select(c => new ConfigMapViewModel(c));
            _view.BindResourceList(viewModels);
        }

        private async Task LoadSecretsAsync(string namespaceName)
        {
            var secrets = await _explorerService.ListSecretsAsync(namespaceName).ConfigureAwait(false);
            var viewModels = secrets.Select(s => new SecretViewModel(s));
            _view.BindResourceList(viewModels);
        }

        private async Task LoadReplicaSetsAsync(string namespaceName)
        {
            var replicaSets = await _explorerService.ListReplicaSetsAsync(namespaceName).ConfigureAwait(false);
            var viewModels = replicaSets.Select(rs => new ReplicaSetViewModel(rs));
            _view.BindResourceList(viewModels);
        }

        private async Task LoadJobsAsync(string namespaceName)
        {
            var jobs = await _explorerService.ListJobsAsync(namespaceName).ConfigureAwait(false);
            var viewModels = jobs.Select(j => new JobViewModel(j));
            _view.BindResourceList(viewModels);
        }

        private async Task LoadCronJobsAsync(string namespaceName)
        {
            var cronJobs = await _explorerService.ListCronJobsAsync(namespaceName).ConfigureAwait(false);
            var viewModels = cronJobs.Select(cj => new CronJobViewModel(cj));
            _view.BindResourceList(viewModels);
        }

        private async Task LoadServiceAccountsAsync(string namespaceName)
        {
            var serviceAccounts = await _explorerService.ListServiceAccountsAsync(namespaceName).ConfigureAwait(false);
            var viewModels = serviceAccounts.Select(sa => new ServiceAccountViewModel(sa));
            _view.BindResourceList(viewModels);
        }

        private async Task LoadRolesAsync(string namespaceName)
        {
            var roles = await _explorerService.ListRolesAsync(namespaceName).ConfigureAwait(false);
            var viewModels = roles.Select(r => new RoleViewModel(r));
            _view.BindResourceList(viewModels);
        }

        private async Task LoadNodesAsync()
        {
            var nodes = await _explorerService.ListNodesAsync().ConfigureAwait(false);
            var viewModels = nodes.Select(n => new NodeViewModel(n));
            _view.BindResourceList(viewModels);
        }

        private async Task LoadNamespacesAsync()
        {
            var namespaces = await _explorerService.ListNamespacesAsync().ConfigureAwait(false);
            var viewModels = namespaces.Select(ns => new NamespaceViewModel(ns));
            _view.BindResourceList(viewModels);
        }

        // ============ WORKLOAD OPERATIONS ============

        public async Task ScaleWorkloadAsync(string namespaceName, string resourceKind, string resourceName)
        {
            // Get current replicas (for dialog pre-fill)
            var currentReplicas = await GetCurrentReplicasAsync(namespaceName, resourceKind, resourceName).ConfigureAwait(false);

            // Ask view to show scale dialog
            var newReplicas = _view.ShowScaleDialog(resourceName, currentReplicas);

            if (!newReplicas.HasValue)
            {
                return; // User cancelled
            }

            // Execute operation via handler
            var context = new OperationContext
            {
                Namespace = namespaceName,
                ResourceKind = resourceKind,
                ResourceName = resourceName,
                UserId = _currentUserId,
                Parameters = new Dictionary<string, object>
                {
                    ["Replicas"] = newReplicas.Value
                }
            };

            var result = await _scaleHandler.ExecuteAsync(context).ConfigureAwait(false);

            // Show result to user
            if (result.Success)
            {
                _view.ShowSuccess("Scale Successful", result.Message ?? "Operation completed");
                await ReloadCurrentViewAsync().ConfigureAwait(false);
            }
            else
            {
                _view.ShowError("Scale Failed", result.ErrorMessage ?? "Unknown error");
            }
        }

        public async Task RestartWorkloadAsync(string namespaceName, string resourceKind, string resourceName)
        {
            // Ask for confirmation
            var confirmed = _view.ShowConfirmation(
                "Confirm Restart",
                $"Are you sure you want to restart {resourceKind} '{resourceName}'?\nThis will trigger a rolling restart of all pods.");

            if (!confirmed)
            {
                return;
            }

            // Execute operation via handler
            var context = new OperationContext
            {
                Namespace = namespaceName,
                ResourceKind = resourceKind,
                ResourceName = resourceName,
                UserId = _currentUserId
            };

            var result = await _restartHandler.ExecuteAsync(context).ConfigureAwait(false);

            // Show result to user
            if (result.Success)
            {
                _view.ShowSuccess("Restart Successful", result.Message ?? "Operation completed");
                await ReloadCurrentViewAsync().ConfigureAwait(false);
            }
            else
            {
                _view.ShowError("Restart Failed", result.ErrorMessage ?? "Unknown error");
            }
        }

        // ============ APPLY YAML ============
        
        public async Task ApplyYamlAsync(string yaml)
        {
            try
            {
                _view.ShowStatusMessage("Applying YAML...", StatusType.Loading);
                
                await _explorerService.ApplyYamlAsync(yaml).ConfigureAwait(false);
                
                _view.ShowSuccess("Apply Successful", "YAML applied successfully");
                _view.ShowStatusMessage("YAML applied", StatusType.Success);
                
                // Audit log
                if (_auditService != null && _currentUserId.HasValue)
                {
                    await _auditService.LogAsync(_currentUserId.Value, "Apply", "YAML", yaml.Substring(0, Math.Min(100, yaml.Length)), null, true).ConfigureAwait(false);
                }
                
                await ReloadCurrentViewAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _view.ShowError("Apply Failed", $"Failed to apply YAML:\n{ex.Message}");
                _view.ShowStatusMessage($"Apply failed: {ex.Message}", StatusType.Error);
            }
        }

        // ============ PORT FORWARD ============
        
        public async Task StartPortForwardAsync(string namespaceName, string podName, int podPort, int localPort)
        {
            try
            {
                _view.ShowStatusMessage($"Starting port forward {localPort} → {podPort}...", StatusType.Loading);
                
                // Note: Port forwarding needs to run in background
                var cts = new System.Threading.CancellationTokenSource();
                
                _ = Task.Run(async () =>
                {
                    try
                    {
                        await _explorerService.PortForwardAsync(namespaceName, podName, podPort, localPort, cts.Token).ConfigureAwait(false);
                    }
                    catch (Exception ex)
                    {
                        _view.ShowError("Port Forward Failed", $"Port forwarding stopped:\n{ex.Message}");
                    }
                }, cts.Token);
                
                _view.ShowSuccess("Port Forward Started", $"Port forwarding active:\nlocalhost:{localPort} → {podName}:{podPort}\n\nClick OK to stop.");
                _view.ShowStatusMessage($"Port forward: {localPort} → {podName}:{podPort}", StatusType.Success);
                
                // Audit log
                if (_auditService != null && _currentUserId.HasValue)
                {
                    await _auditService.LogAsync(_currentUserId.Value, "PortForward", "Pod", $"{podName}:{podPort}→{localPort}", namespaceName, true).ConfigureAwait(false);
                }
                
                // Stop when user closes dialog
                cts.Cancel();
            }
            catch (Exception ex)
            {
                _view.ShowError("Port Forward Failed", $"Failed to start port forward:\n{ex.Message}");
                _view.ShowStatusMessage($"Port forward failed: {ex.Message}", StatusType.Error);
            }
        }

        // ============ SHELL / EXEC ============
        
        public async Task OpenShellAsync(string namespaceName, string podName, string containerName = null)
        {
            try
            {
                _view.ShowStatusMessage($"Opening shell to {podName}...", StatusType.Loading);
                
                // Get pod to check container
                var pod = await _explorerService.GetPodAsync(namespaceName, podName).ConfigureAwait(false);
                
                if (pod == null)
                {
                    _view.ShowError("Shell Failed", $"Pod '{podName}' not found");
                    return;
                }
                
                // If container not specified, use first container
                if (string.IsNullOrEmpty(containerName))
                {
                    containerName = pod.Spec?.Containers?.FirstOrDefault()?.Name ?? string.Empty;
                }
                
                if (string.IsNullOrEmpty(containerName))
                {
                    _view.ShowError("Shell Failed", "No container found in pod");
                    return;
                }
                
                // Open shell in external terminal
                await _explorerService.ExecShellAsync(namespaceName, podName, containerName).ConfigureAwait(false);
                
                _view.ShowSuccess("Shell Opened", $"Shell opened to:\n{podName}/{containerName}");
                _view.ShowStatusMessage($"Shell: {podName}/{containerName}", StatusType.Success);
                
                // Audit log
                if (_auditService != null && _currentUserId.HasValue)
                {
                    await _auditService.LogAsync(_currentUserId.Value, "Shell", "Pod", $"{podName}/{containerName}", namespaceName, true).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                _view.ShowError("Shell Failed", $"Failed to open shell:\n{ex.Message}");
                _view.ShowStatusMessage($"Shell failed: {ex.Message}", StatusType.Error);
            }
        }

        // ============ DELETE RESOURCE ============

        public async Task DeleteResourceAsync(string namespaceName, string resourceKind, string resourceName)
        {
            // Ask for confirmation
            var confirmed = _view.ShowConfirmation(
                "Confirm Delete",
                $"Are you sure you want to delete {resourceKind} '{resourceName}'?\nThis action cannot be undone.");

            if (!confirmed)
            {
                return;
            }

            // Execute operation via handler
            var context = new OperationContext
            {
                Namespace = namespaceName,
                ResourceKind = resourceKind,
                ResourceName = resourceName,
                UserId = _currentUserId
            };

            var result = await _deleteHandler.ExecuteAsync(context).ConfigureAwait(false);

            // Show result to user
            if (result.Success)
            {
                _view.ShowSuccess("Delete Successful", result.Message ?? "Operation completed");
                await ReloadCurrentViewAsync().ConfigureAwait(false);
            }
            else
            {
                _view.ShowError("Delete Failed", result.ErrorMessage ?? "Unknown error");
            }
        }

        // ============ HELPER METHODS ============

        private async Task<int> GetCurrentReplicasAsync(string namespaceName, string resourceKind, string resourceName)
        {
            try
            {
                return resourceKind switch
                {
                    "Deployment" => (await _explorerService.ListDeploymentsAsync(namespaceName).ConfigureAwait(false))
                        .FirstOrDefault(d => d.Metadata?.Name == resourceName)?.Spec?.Replicas ?? 0,
                    "StatefulSet" => (await _explorerService.ListStatefulSetsAsync(namespaceName).ConfigureAwait(false))
                        .FirstOrDefault(s => s.Metadata?.Name == resourceName)?.Spec?.Replicas ?? 0,
                    _ => 0
                };
            }
            catch
            {
                return 0;
            }
        }

        private async Task ReloadCurrentViewAsync()
        {
            var ns = _view.SelectedNamespace;
            var kind = _view.CurrentResourceKind;

            if (!string.IsNullOrEmpty(ns) && !string.IsNullOrEmpty(kind))
            {
                await LoadResourceListAsync(ns, kind).ConfigureAwait(false);
            }
        }

        // ============ DIALOG/DETAIL METHODS ============

        public async Task LoadYamlAsync(string namespaceName, string resourceKind, string resourceName)
        {
            try
            {
                string yaml = await _explorerService.GetResourceYamlAsync(resourceKind, namespaceName, resourceName).ConfigureAwait(false);

                // MainViewLens has ShowYaml method (not in interface)
                if (_view is Views.MainViewLens lens)
                {
                    lens.ShowYaml(yaml);
                }
                else
                {
                    _view.ShowSuccess("YAML", yaml);
                }
            }
            catch (Exception ex)
            {
                _view.ShowError("YAML Load Error", $"Failed to load YAML: {ex.Message}");
            }
        }

        public async Task DescribeAsync(string namespaceName, string resourceKind, string resourceName)
        {
            // Describe is essentially same as viewing YAML
            await LoadYamlAsync(namespaceName, resourceKind, resourceName).ConfigureAwait(false);
        }

        public async Task TailLogsAsync(string namespaceName, string podName)
        {
            try
            {
                // Get pod to find containers
                var pods = await _k8sService.ListPodsAsync(namespaceName).ConfigureAwait(false);
                var pod = pods.FirstOrDefault(p => p.Metadata?.Name == podName);

                if (pod != null)
                {
                    await ViewPodLogsAsync(namespaceName, pod).ConfigureAwait(false);
                }
                else
                {
                    _view.ShowWarning("Pod Not Found", $"Pod '{podName}' not found in namespace '{namespaceName}'");
                }
            }
            catch (Exception ex)
            {
                _view.ShowError("Logs Error", $"Failed to tail logs: {ex.Message}");
            }
        }

        public async Task ShowPodDetailsAsync(string namespaceName, string podName)
        {
            try
            {
                var pods = await _k8sService.ListPodsAsync(namespaceName).ConfigureAwait(false);
                var pod = pods.FirstOrDefault(p => p.Metadata?.Name == podName);

                if (pod != null)
                {
                    if (_view is Views.MainViewLens lens)
                    {
                        // MainViewLens will handle showing the dialog
                        using var dialog = new Views.PodDetailsDialog(
                            pod,
                            async (p) => await ViewPodLogsAsync(namespaceName, p).ConfigureAwait(false),
                            (p) => OpenShellToPod(namespaceName, p)
                        );
                        dialog.ShowDialog();
                    }
                    else
                    {
                        _view.ShowWarning("Not Supported", "Pod details dialog is only available in MainViewLens");
                    }
                }
                else
                {
                    _view.ShowWarning("Pod Not Found", $"Pod '{podName}' not found in namespace '{namespaceName}'");
                }
            }
            catch (Exception ex)
            {
                _view.ShowError("Pod Details Error", $"Failed to load pod details: {ex.Message}");
            }
        }

        private async Task ViewPodLogsAsync(string namespaceName, k8s.Models.V1Pod pod)
        {
            var podName = pod.Metadata?.Name ?? "";
            var containers = pod.Spec?.Containers?.Select(c => c.Name).ToArray() ?? Array.Empty<string>();

            // Create load logs function
            Func<string?, int, Task<string>> loadLogsFunc = async (containerName, lines) =>
            {
                var logs = new System.Text.StringBuilder();
                try
                {
                    await foreach (var line in _k8sService.TailLogsAsync(namespaceName, podName, containerName).ConfigureAwait(false))
                    {
                        logs.AppendLine(line);
                        if (logs.Length > lines * 100) break; // Approximate line count
                    }
                }
                catch (Exception ex)
                {
                    logs.AppendLine($"Error: {ex.Message}");
                }
                return logs.ToString();
            };

            using var dialog = new Views.LogsDialog(podName, containers, loadLogsFunc);
            dialog.ShowDialog();
        }

        private void OpenShellToPod(string namespaceName, k8s.Models.V1Pod pod)
        {
            var podName = pod.Metadata?.Name ?? "";
            var containers = pod.Spec?.Containers?.Select(c => c.Name).ToArray() ?? Array.Empty<string>();
            var container = containers.FirstOrDefault() ?? "";

            var shellService = new K8sShellService(_clientFactory);
            using var dialog = new Views.TerminalDialog(shellService, namespaceName, podName, container);
            dialog.ShowDialog();
        }

        public async Task ShowDeploymentDetailsAsync(string namespaceName, string deploymentName)
        {
            try
            {
                var deployments = await _explorerService.ListDeploymentsAsync(namespaceName).ConfigureAwait(false);
                var deployment = deployments.FirstOrDefault(d => d.Metadata?.Name == deploymentName);

                if (deployment != null)
                {
                    if (_view is Views.MainViewLens lens)
                    {
                        using var dialog = new Views.DeploymentDetailsDialog(
                            deployment,
                            async (dep) => await ScaleDeploymentDialogAsync(namespaceName, dep).ConfigureAwait(false),
                            async (dep) => await RestartDeploymentDialogAsync(namespaceName, dep).ConfigureAwait(false),
                            async (dep) => await ViewDeploymentPodsAsync(namespaceName, dep).ConfigureAwait(false)
                        );
                        dialog.ShowDialog();
                    }
                    else
                    {
                        _view.ShowWarning("Not Supported", "Deployment details dialog is only available in MainViewLens");
                    }
                }
                else
                {
                    _view.ShowWarning("Deployment Not Found", $"Deployment '{deploymentName}' not found in namespace '{namespaceName}'");
                }
            }
            catch (Exception ex)
            {
                _view.ShowError("Deployment Details Error", $"Failed to load deployment details: {ex.Message}");
            }
        }

        private async Task ScaleDeploymentDialogAsync(string namespaceName, k8s.Models.V1Deployment deployment)
        {
            var deploymentName = deployment.Metadata?.Name ?? "";
            var currentReplicas = deployment.Spec?.Replicas ?? 0;
            var newReplicas = _view.ShowScaleDialog(deploymentName, currentReplicas);

            if (newReplicas.HasValue && newReplicas.Value != currentReplicas)
            {
                await ScaleWorkloadAsync(namespaceName, "Deployment", deploymentName).ConfigureAwait(false);
            }
        }

        private async Task RestartDeploymentDialogAsync(string namespaceName, k8s.Models.V1Deployment deployment)
        {
            var deploymentName = deployment.Metadata?.Name ?? "";
            var confirmed = _view.ShowConfirmation("Restart Deployment", $"Are you sure you want to restart '{deploymentName}'?");

            if (confirmed)
            {
                await RestartWorkloadAsync(namespaceName, "Deployment", deploymentName).ConfigureAwait(false);
            }
        }

        private async Task ViewDeploymentPodsAsync(string namespaceName, k8s.Models.V1Deployment deployment)
        {
            var labelSelector = deployment.Spec?.Selector?.MatchLabels != null
                ? string.Join(",", deployment.Spec.Selector.MatchLabels.Select(kv => $"{kv.Key}={kv.Value}"))
                : null;

            if (!string.IsNullOrEmpty(labelSelector))
            {
                // This would switch to Pods view with filter
                _view.ShowStatusMessage($"Filter: {labelSelector}", StatusType.Info);
            }
        }

        public async Task ShowStatefulSetDetailsAsync(string namespaceName, string statefulSetName)
        {
            try
            {
                var statefulSets = await _explorerService.ListStatefulSetsAsync(namespaceName).ConfigureAwait(false);
                var statefulSet = statefulSets.FirstOrDefault(s => s.Metadata?.Name == statefulSetName);

                if (statefulSet != null)
                {
                    // For now, just show YAML
                    string yaml = await _explorerService.GetResourceYamlAsync("StatefulSet", namespaceName, statefulSetName).ConfigureAwait(false);
                    if (_view is Views.MainViewLens lens)
                    {
                        lens.ShowYaml(yaml);
                    }
                    else
                    {
                        _view.ShowSuccess("StatefulSet YAML", yaml);
                    }
                }
                else
                {
                    _view.ShowWarning("StatefulSet Not Found", $"StatefulSet '{statefulSetName}' not found in namespace '{namespaceName}'");
                }
            }
            catch (Exception ex)
            {
                _view.ShowError("StatefulSet Details Error", $"Failed to load StatefulSet details: {ex.Message}");
            }
        }

        public async Task ShowDaemonSetDetailsAsync(string namespaceName, string daemonSetName)
        {
            try
            {
                var daemonSets = await _explorerService.ListDaemonSetsAsync(namespaceName).ConfigureAwait(false);
                var daemonSet = daemonSets.FirstOrDefault(d => d.Metadata?.Name == daemonSetName);

                if (daemonSet != null)
                {
                    if (_view is Views.MainViewLens lens)
                    {
                        using var dialog = new Views.DaemonSetDetailsDialog(
                            daemonSet,
                            async (ds) => await RestartWorkloadAsync(namespaceName, "DaemonSet", ds.Metadata?.Name ?? "").ConfigureAwait(false),
                            async (ds) => await ViewDaemonSetPodsAsync(namespaceName, ds).ConfigureAwait(false)
                        );
                        dialog.ShowDialog();
                    }
                    else
                    {
                        // Fallback to YAML
                        string yaml = await _explorerService.GetResourceYamlAsync("DaemonSet", namespaceName, daemonSetName).ConfigureAwait(false);
                        _view.ShowSuccess("DaemonSet YAML", yaml);
                    }
                }
                else
                {
                    _view.ShowWarning("DaemonSet Not Found", $"DaemonSet '{daemonSetName}' not found in namespace '{namespaceName}'");
                }
            }
            catch (Exception ex)
            {
                _view.ShowError("DaemonSet Details Error", $"Failed to load DaemonSet details: {ex.Message}");
            }
        }

        private async Task ViewDaemonSetPodsAsync(string namespaceName, k8s.Models.V1DaemonSet daemonSet)
        {
            var labelSelector = daemonSet.Spec?.Selector?.MatchLabels != null
                ? string.Join(",", daemonSet.Spec.Selector.MatchLabels.Select(kv => $"{kv.Key}={kv.Value}"))
                : null;

            if (!string.IsNullOrEmpty(labelSelector))
            {
                _view.ShowStatusMessage($"Filter: {labelSelector}", StatusType.Info);
            }

            await Task.CompletedTask.ConfigureAwait(false);
        }
    }

    // ============ VIEW MODELS (Separation of concerns) ============

    public class PodViewModel
    {
        public string Name { get; }
        public string Namespace { get; }
        public string Status { get; }
        public string Ready { get; }
        public int Restarts { get; }
        public string Age { get; }
        public string IP { get; }
        public string Node { get; }

        public PodViewModel(k8s.Models.V1Pod pod)
        {
            Name = pod.Metadata?.Name ?? "";
            Namespace = pod.Metadata?.NamespaceProperty ?? "";
            Status = CalculateStatus(pod);
            Ready = $"{pod.Status?.ContainerStatuses?.Count(c => c.Ready) ?? 0}/{pod.Status?.ContainerStatuses?.Count ?? 0}";
            Restarts = pod.Status?.ContainerStatuses?.Sum(c => c.RestartCount) ?? 0;
            Age = CalculateAge(pod.Metadata?.CreationTimestamp);
            IP = pod.Status?.PodIP ?? "-";
            Node = pod.Spec?.NodeName ?? "-";
        }

        private static string CalculateStatus(k8s.Models.V1Pod pod)
        {
            var phase = pod.Status?.Phase ?? "Unknown";

            if (pod.Status?.ContainerStatuses != null)
            {
                foreach (var cs in pod.Status.ContainerStatuses)
                {
                    if (cs.State?.Waiting != null)
                    {
                        var reason = cs.State.Waiting.Reason;
                        if (reason == "CrashLoopBackOff" || reason == "ImagePullBackOff" ||
                            reason == "ErrImagePull" || reason == "CreateContainerError")
                            return reason;
                    }

                    if (cs.State?.Terminated != null && !cs.Ready)
                    {
                        return cs.State.Terminated.Reason ?? "Error";
                    }
                }
            }

            return phase;
        }

        public static string CalculateAge(DateTime? creationTime)
        {
            if (!creationTime.HasValue) return "Unknown";

            var age = DateTime.UtcNow - creationTime.Value;

            if (age.TotalDays >= 365) return $"{(int)(age.TotalDays / 365)}y";
            if (age.TotalDays >= 30) return $"{(int)(age.TotalDays / 30)}mo";
            if (age.TotalDays >= 1) return $"{(int)age.TotalDays}d";
            if (age.TotalHours >= 1) return $"{(int)age.TotalHours}h";
            if (age.TotalMinutes >= 1) return $"{(int)age.TotalMinutes}m";
            return $"{(int)age.TotalSeconds}s";
        }
    }

    public class DeploymentViewModel
    {
        public string Name { get; }
        public string Namespace { get; }
        public string Replicas { get; }
        public int UpToDate { get; }
        public int Available { get; }
        public string Age { get; }
        public string Conditions { get; }

        public DeploymentViewModel(k8s.Models.V1Deployment deployment)
        {
            Name = deployment.Metadata?.Name ?? "";
            Namespace = deployment.Metadata?.NamespaceProperty ?? "";
            Replicas = $"{deployment.Status?.ReadyReplicas ?? 0}/{deployment.Spec?.Replicas ?? 0}";
            UpToDate = deployment.Status?.UpdatedReplicas ?? 0;
            Available = deployment.Status?.AvailableReplicas ?? 0;
            Age = PodViewModel.CalculateAge(deployment.Metadata?.CreationTimestamp);
            Conditions = GetCondition(deployment);
        }

        private static string GetCondition(k8s.Models.V1Deployment deployment)
        {
            var condition = deployment.Status?.Conditions?.LastOrDefault();
            if (condition == null) return "Unknown";

            return condition.Type switch
            {
                "Available" when condition.Status == "True" => "Available",
                "Progressing" when condition.Status == "True" => "Progressing",
                "ReplicaFailure" when condition.Status == "True" => "ReplicaFailure",
                _ => condition.Type ?? "Unknown"
            };
        }
    }

    public class StatefulSetViewModel
    {
        public string Name { get; }
        public string Namespace { get; }
        public string ServiceName { get; }
        public string Replicas { get; }
        public int Ready { get; }
        public int Current { get; }
        public int Updated { get; }
        public string Age { get; }

        public StatefulSetViewModel(k8s.Models.V1StatefulSet sts)
        {
            Name = sts.Metadata?.Name ?? "";
            Namespace = sts.Metadata?.NamespaceProperty ?? "";
            ServiceName = sts.Spec?.ServiceName ?? "None";
            Replicas = $"{sts.Status?.ReadyReplicas ?? 0}/{sts.Spec?.Replicas ?? 0}";
            Ready = sts.Status?.ReadyReplicas ?? 0;
            Current = sts.Status?.CurrentReplicas ?? 0;
            Updated = sts.Status?.UpdatedReplicas ?? 0;
            Age = PodViewModel.CalculateAge(sts.Metadata?.CreationTimestamp);
        }
    }

    public class DaemonSetViewModel
    {
        public string Name { get; }
        public string Namespace { get; }
        public int Desired { get; }
        public int Current { get; }
        public int Ready { get; }
        public int Available { get; }
        public string NodeSelector { get; }
        public string Age { get; }

        public DaemonSetViewModel(k8s.Models.V1DaemonSet ds)
        {
            Name = ds.Metadata?.Name ?? "";
            Namespace = ds.Metadata?.NamespaceProperty ?? "";
            Desired = ds.Status?.DesiredNumberScheduled ?? 0;
            Current = ds.Status?.CurrentNumberScheduled ?? 0;
            Ready = ds.Status?.NumberReady ?? 0;
            Available = ds.Status?.NumberAvailable ?? 0;
            NodeSelector = ds.Spec?.Template?.Spec?.NodeSelector?.Any() == true
                ? string.Join(", ", ds.Spec.Template.Spec.NodeSelector.Select(kv => $"{kv.Key}={kv.Value}"))
                : "All nodes";
            Age = PodViewModel.CalculateAge(ds.Metadata?.CreationTimestamp);
        }
    }

    public class ServiceViewModel
    {
        public string Name { get; }
        public string Namespace { get; }
        public string Type { get; }
        public string ClusterIP { get; }
        public string ExternalIP { get; }
        public string Ports { get; }
        public string Age { get; }

        public ServiceViewModel(k8s.Models.V1Service svc)
        {
            Name = svc.Metadata?.Name ?? "";
            Namespace = svc.Metadata?.NamespaceProperty ?? "";
            Type = svc.Spec?.Type ?? "";
            ClusterIP = svc.Spec?.ClusterIP ?? "";
            ExternalIP = string.Join(",", svc.Spec?.ExternalIPs ?? new List<string>());
            Ports = string.Join(",", svc.Spec?.Ports?.Select(p => $"{p.Port}/{p.Protocol}") ?? Enumerable.Empty<string>());
            Age = PodViewModel.CalculateAge(svc.Metadata?.CreationTimestamp);
        }
    }

    public class ConfigMapViewModel
    {
        public string Name { get; }
        public string Namespace { get; }
        public int Keys { get; }
        public string Age { get; }

        public ConfigMapViewModel(k8s.Models.V1ConfigMap cm)
        {
            Name = cm.Metadata?.Name ?? "";
            Namespace = cm.Metadata?.NamespaceProperty ?? "";
            Keys = cm.Data?.Count ?? 0;
            Age = PodViewModel.CalculateAge(cm.Metadata?.CreationTimestamp);
        }
    }

    public class SecretViewModel
    {
        public string Name { get; }
        public string Namespace { get; }
        public string Type { get; }
        public int Keys { get; }
        public string Age { get; }

        public SecretViewModel(k8s.Models.V1Secret secret)
        {
            Name = secret.Metadata?.Name ?? "";
            Namespace = secret.Metadata?.NamespaceProperty ?? "";
            Type = secret.Type ?? "";
            Keys = secret.Data?.Count ?? 0;
            Age = PodViewModel.CalculateAge(secret.Metadata?.CreationTimestamp);
        }
    }

    public class ReplicaSetViewModel
    {
        public string Name { get; }
        public string Namespace { get; }
        public int Desired { get; }
        public int Current { get; }
        public int Ready { get; }
        public string Age { get; }

        public ReplicaSetViewModel(k8s.Models.V1ReplicaSet rs)
        {
            Name = rs.Metadata?.Name ?? "";
            Namespace = rs.Metadata?.NamespaceProperty ?? "";
            Desired = rs.Spec?.Replicas ?? 0;
            Current = rs.Status?.Replicas ?? 0;
            Ready = rs.Status?.ReadyReplicas ?? 0;
            Age = PodViewModel.CalculateAge(rs.Metadata?.CreationTimestamp);
        }
    }

    public class JobViewModel
    {
        public string Name { get; }
        public string Namespace { get; }
        public string Status { get; }
        public int Completions { get; }
        public int Succeeded { get; }
        public string Age { get; }

        public JobViewModel(k8s.Models.V1Job job)
        {
            Name = job.Metadata?.Name ?? "";
            Namespace = job.Metadata?.NamespaceProperty ?? "";
            
            var succeeded = job.Status?.Succeeded ?? 0;
            var completions = job.Spec?.Completions ?? 1;
            Completions = completions;
            Succeeded = succeeded;
            
            Status = succeeded >= completions ? "Complete" : 
                     (job.Status?.Active ?? 0) > 0 ? "Running" : "Pending";
            
            Age = PodViewModel.CalculateAge(job.Metadata?.CreationTimestamp);
        }
    }

    public class CronJobViewModel
    {
        public string Name { get; }
        public string Namespace { get; }
        public string Schedule { get; }
        public bool Suspend { get; }
        public int Active { get; }
        public string LastSchedule { get; }
        public string Age { get; }

        public CronJobViewModel(k8s.Models.V1CronJob cj)
        {
            Name = cj.Metadata?.Name ?? "";
            Namespace = cj.Metadata?.NamespaceProperty ?? "";
            Schedule = cj.Spec?.Schedule ?? "";
            Suspend = cj.Spec?.Suspend ?? false;
            Active = cj.Status?.Active?.Count ?? 0;
            LastSchedule = cj.Status?.LastScheduleTime.HasValue == true
                ? PodViewModel.CalculateAge(cj.Status.LastScheduleTime)
                : "Never";
            Age = PodViewModel.CalculateAge(cj.Metadata?.CreationTimestamp);
        }
    }

    public class ServiceAccountViewModel
    {
        public string Name { get; }
        public string Namespace { get; }
        public int Secrets { get; }
        public string Age { get; }

        public ServiceAccountViewModel(k8s.Models.V1ServiceAccount sa)
        {
            Name = sa.Metadata?.Name ?? "";
            Namespace = sa.Metadata?.NamespaceProperty ?? "";
            Secrets = sa.Secrets?.Count ?? 0;
            Age = PodViewModel.CalculateAge(sa.Metadata?.CreationTimestamp);
        }
    }

    public class RoleViewModel
    {
        public string Name { get; }
        public string Namespace { get; }
        public int Rules { get; }
        public string Age { get; }

        public RoleViewModel(k8s.Models.V1Role role)
        {
            Name = role.Metadata?.Name ?? "";
            Namespace = role.Metadata?.NamespaceProperty ?? "";
            Rules = role.Rules?.Count ?? 0;
            Age = PodViewModel.CalculateAge(role.Metadata?.CreationTimestamp);
        }
    }

    public class NodeViewModel
    {
        public string Name { get; }
        public string Status { get; }
        public string Roles { get; }
        public string Version { get; }
        public string InternalIP { get; }
        public string Age { get; }

        public NodeViewModel(k8s.Models.V1Node node)
        {
            Name = node.Metadata?.Name ?? "";
            
            // Get node status
            var readyCondition = node.Status?.Conditions?
                .FirstOrDefault(c => c.Type == "Ready");
            Status = readyCondition?.Status == "True" ? "Ready" : "NotReady";
            
            // Get node roles from labels
            var labels = node.Metadata?.Labels ?? new Dictionary<string, string>();
            var roles = labels
                .Where(l => l.Key.StartsWith("node-role.kubernetes.io/"))
                .Select(l => l.Key.Replace("node-role.kubernetes.io/", ""))
                .ToList();
            Roles = roles.Any() ? string.Join(",", roles) : "<none>";
            
            Version = node.Status?.NodeInfo?.KubeletVersion ?? "";
            
            // Get internal IP
            InternalIP = node.Status?.Addresses?
                .FirstOrDefault(a => a.Type == "InternalIP")?.Address ?? "";
            
            Age = PodViewModel.CalculateAge(node.Metadata?.CreationTimestamp);
        }
    }

    public class NamespaceViewModel
    {
        public string Name { get; }
        public string Status { get; }
        public string Age { get; }

        public NamespaceViewModel(k8s.Models.V1Namespace ns)
        {
            Name = ns.Metadata?.Name ?? "";
            Status = ns.Status?.Phase ?? "Unknown";
            Age = PodViewModel.CalculateAge(ns.Metadata?.CreationTimestamp);
        }
    }
}
