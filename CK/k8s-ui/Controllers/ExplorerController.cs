using K8sManager.Infrastructure;
using K8sManager.Services;
using K8sManager.Views;

namespace K8sManager
{
    public class ExplorerController
    {
        private readonly MainViewLens _view;
        private readonly IK8sService _k8s;
        private readonly IK8sExplorerService _explorer;
        private readonly K8sClientFactory _factory;
        private readonly AuditService? _auditService;
        private int? _currentUserId;

        public ExplorerController(MainViewLens view, IK8sService baseService, IK8sExplorerService explorer, K8sClientFactory factory, AuditService? auditService = null)
        {
            _view = view;
            _k8s = baseService;
            _explorer = explorer;
            _factory = factory;
            _auditService = auditService;
            _view.Controller = this;
        }

        public void SetCurrentUser(int userId)
        {
            _currentUserId = userId;
        }

        public async Task InitAsync()
        {
            var logFile = System.IO.Path.Combine(
                System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) ?? ".",
                "error.log");

            try
            {
                System.IO.File.AppendAllText(logFile, "InitAsync: Loading contexts...\n");

                // Load available contexts
                var contexts = _factory.GetAvailableContexts();
                System.IO.File.AppendAllText(logFile, $"InitAsync: Found {contexts.Count} contexts\n");

                if (contexts.Count > 0)
                {
                    var currentContext = _factory.GetCurrentContext();
                    System.IO.File.AppendAllText(logFile, $"InitAsync: Current context = {currentContext}\n");
                    _view.BindContexts(contexts, currentContext);
                }

                System.IO.File.AppendAllText(logFile, "InitAsync: Loading namespaces...\n");
                var namespaces = await _k8s.ListNamespacesAsync().ConfigureAwait(false);
                System.IO.File.AppendAllText(logFile, $"InitAsync: Found {namespaces.Count} namespaces\n");

                _view.BindNamespaces(namespaces);
                if (namespaces.Count > 0)
                {
                    System.IO.File.AppendAllText(logFile, $"InitAsync: Loading resource counts for namespace '{namespaces[0]}'...\n");
                    await LoadAllForNamespaceAsync(namespaces[0]).ConfigureAwait(false);

                    // Auto-load Pods on startup
                    System.IO.File.AppendAllText(logFile, "InitAsync: Auto-loading Pods...\n");
                    await LoadListForKindAsync(namespaces[0], "Pods").ConfigureAwait(false);
                }

                System.IO.File.AppendAllText(logFile, "InitAsync: Completed successfully\n");
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText(logFile, $"InitAsync ERROR: {ex.GetType().Name}: {ex.Message}\n");
                System.IO.File.AppendAllText(logFile, $"InitAsync STACK: {ex.StackTrace}\n");

                System.Windows.Forms.MessageBox.Show(
                    $"Failed to initialize:\n{ex.Message}\n\nThe application will continue but may not be fully functional.",
                    "Initialization Warning",
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Warning
                );

                // Set empty defaults so UI still works
                _view.BindContexts(new List<string> { "No context available" });
                _view.BindNamespaces(new List<string> { "default" });
            }
        }

        public Task SwitchContextAsync(string context)
        {
            _factory.SetContext(context);
            return Task.CompletedTask;
        }

        public async Task LoadAllForNamespaceAsync(string ns)
        {
            var logFile = System.IO.Path.Combine(
                System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) ?? ".",
                "error.log");

            try
            {
                System.IO.File.AppendAllText(logFile, $"LoadAllForNamespaceAsync: Loading counts for namespace '{ns}'...\n");

                // Only load counts, not full data (lazy loading)
                var countsTask = Task.Run(async () =>
                {
                    var counts = new Dictionary<string, int>();

                    try { counts["Pods"] = (await _k8s.ListPodsAsync(ns).ConfigureAwait(false)).Count; } catch { counts["Pods"] = 0; }
                    try { counts["Deployments"] = (await _explorer.ListDeploymentsAsync(ns).ConfigureAwait(false)).Count; } catch { counts["Deployments"] = 0; }
                    try { counts["StatefulSets"] = (await _explorer.ListStatefulSetsAsync(ns).ConfigureAwait(false)).Count; } catch { counts["StatefulSets"] = 0; }
                    try { counts["DaemonSets"] = (await _explorer.ListDaemonSetsAsync(ns).ConfigureAwait(false)).Count; } catch { counts["DaemonSets"] = 0; }
                    try { counts["ReplicaSets"] = (await _explorer.ListReplicaSetsAsync(ns).ConfigureAwait(false)).Count; } catch { counts["ReplicaSets"] = 0; }
                    try { counts["Jobs"] = (await _explorer.ListJobsAsync(ns).ConfigureAwait(false)).Count; } catch { counts["Jobs"] = 0; }
                    try { counts["CronJobs"] = (await _explorer.ListCronJobsAsync(ns).ConfigureAwait(false)).Count; } catch { counts["CronJobs"] = 0; }
                    try { counts["Services"] = (await _explorer.ListServicesAsync(ns).ConfigureAwait(false)).Count; } catch { counts["Services"] = 0; }
                    try { counts["Ingresses"] = (await _explorer.ListIngressesAsync(ns).ConfigureAwait(false)).Count; } catch { counts["Ingresses"] = 0; }
                    try { counts["NetworkPolicies"] = (await _explorer.ListNetworkPoliciesAsync(ns).ConfigureAwait(false)).Count; } catch { counts["NetworkPolicies"] = 0; }
                    try { counts["ConfigMaps"] = (await _explorer.ListConfigMapsAsync(ns).ConfigureAwait(false)).Count; } catch { counts["ConfigMaps"] = 0; }
                    try { counts["Secrets"] = (await _explorer.ListSecretsAsync(ns).ConfigureAwait(false)).Count; } catch { counts["Secrets"] = 0; }
                    try { counts["PersistentVolumeClaims"] = (await _explorer.ListPersistentVolumeClaimsAsync(ns).ConfigureAwait(false)).Count; } catch { counts["PersistentVolumeClaims"] = 0; }
                    try { counts["ServiceAccounts"] = (await _explorer.ListServiceAccountsAsync(ns).ConfigureAwait(false)).Count; } catch { counts["ServiceAccounts"] = 0; }
                    try { counts["Roles"] = (await _explorer.ListRolesAsync(ns).ConfigureAwait(false)).Count; } catch { counts["Roles"] = 0; }
                    try { counts["RoleBindings"] = (await _explorer.ListRoleBindingsAsync(ns).ConfigureAwait(false)).Count; } catch { counts["RoleBindings"] = 0; }

                    return counts;
                });

                var counts = await countsTask.ConfigureAwait(false);

                System.IO.File.AppendAllText(logFile, $"LoadAllForNamespaceAsync: Updating explorer counts...\n");
                _view.UpdateExplorerCounts(counts);

                System.IO.File.AppendAllText(logFile, $"LoadAllForNamespaceAsync: Counts updated successfully\n");
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText(logFile, $"Error in LoadAllForNamespaceAsync: {ex.Message}\n{ex.StackTrace}\n");
            }
        }

        public async Task LoadListForKindAsync(string ns, string kind)
        {
            var logFile = System.IO.Path.Combine(
                System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) ?? ".",
                "error.log");
            System.IO.File.AppendAllText(logFile, $"LoadListForKindAsync: ns='{ns}', kind='{kind}'\n");

            switch (kind)
            {
                case "Pods":
                    System.IO.File.AppendAllText(logFile, $"LoadListForKindAsync: Loading Pods...\n");
                    var pods = await _k8s.ListPodsAsync(ns).ConfigureAwait(false);
                    System.IO.File.AppendAllText(logFile, $"LoadListForKindAsync: Got {pods.Count} pods\n");
                    _view.BindList(pods.Select(p => new
                    {
                        Name = p.Metadata?.Name ?? "",
                        Namespace = p.Metadata?.NamespaceProperty ?? "",
                        Status = GetPodStatus(p),
                        Ready = $"{p.Status?.ContainerStatuses?.Count(c => c.Ready) ?? 0}/{p.Status?.ContainerStatuses?.Count ?? 0}",
                        Restarts = p.Status?.ContainerStatuses?.Sum(c => c.RestartCount) ?? 0,
                        Age = GetAge(p.Metadata?.CreationTimestamp),
                        IP = p.Status?.PodIP ?? "-",
                        Node = p.Spec?.NodeName ?? "-",
                        QoS = p.Status?.QosClass ?? "-",
                        Containers = p.Spec?.Containers?.Count ?? 0
                    }));
                    break;
                case "Deployments":
                    System.IO.File.AppendAllText(logFile, $"LoadListForKindAsync: Loading Deployments...\n");
                    var deps = await _explorer.ListDeploymentsAsync(ns).ConfigureAwait(false);
                    System.IO.File.AppendAllText(logFile, $"LoadListForKindAsync: Got {deps.Count} deployments\n");
                    _view.BindList(deps.Select(d => new
                    {
                        Name = d.Metadata?.Name ?? "",
                        Namespace = d.Metadata?.NamespaceProperty ?? "",
                        Replicas = $"{d.Status?.ReadyReplicas ?? 0}/{d.Spec?.Replicas ?? 0}",
                        UpToDate = d.Status?.UpdatedReplicas ?? 0,
                        Available = d.Status?.AvailableReplicas ?? 0,
                        Age = GetAge(d.Metadata?.CreationTimestamp),
                        Conditions = GetDeploymentCondition(d)
                    }));
                    break;
                case "StatefulSets":
                    var sts = await _explorer.ListStatefulSetsAsync(ns).ConfigureAwait(false);
                    _view.BindList(sts.Select(s => new
                    {
                        Name = s.Metadata?.Name ?? "",
                        Namespace = s.Metadata?.NamespaceProperty ?? "",
                        ServiceName = s.Spec?.ServiceName ?? "None",
                        Replicas = $"{s.Status?.ReadyReplicas ?? 0}/{s.Spec?.Replicas ?? 0}",
                        Ready = s.Status?.ReadyReplicas ?? 0,
                        Current = s.Status?.CurrentReplicas ?? 0,
                        Updated = s.Status?.UpdatedReplicas ?? 0,
                        Age = GetAge(s.Metadata?.CreationTimestamp),
                        Conditions = GetStatefulSetCondition(s)
                    }));
                    break;
                case "DaemonSets":
                    var ds = await _explorer.ListDaemonSetsAsync(ns).ConfigureAwait(false);
                    _view.BindList(ds.Select(d => new
                    {
                        Name = d.Metadata?.Name,
                        Namespace = d.Metadata?.NamespaceProperty,
                        Desired = d.Status?.DesiredNumberScheduled,
                        Current = d.Status?.CurrentNumberScheduled,
                        Ready = d.Status?.NumberReady,
                        Available = d.Status?.NumberAvailable,
                        NodeSelector = d.Spec?.Template?.Spec?.NodeSelector?.Any() == true
                            ? string.Join(", ", d.Spec.Template.Spec.NodeSelector.Select(kv => $"{kv.Key}={kv.Value}"))
                            : "All nodes",
                        Age = GetAge(d.Metadata?.CreationTimestamp)
                    }));
                    break;
                case "ReplicaSets":
                    var rs = await _explorer.ListReplicaSetsAsync(ns).ConfigureAwait(false);
                    _view.BindList(rs.Select(r => new
                    {
                        Name = r.Metadata?.Name,
                        Namespace = r.Metadata?.NamespaceProperty,
                        Desired = r.Spec?.Replicas,
                        Current = r.Status?.Replicas,
                        Ready = r.Status?.ReadyReplicas,
                        Age = r.Metadata?.CreationTimestamp?.ToString("yyyy-MM-dd HH:mm:ss")
                    }));
                    break;
                case "Jobs":
                    var jobs = await _explorer.ListJobsAsync(ns).ConfigureAwait(false);
                    _view.BindList(jobs.Select(j => new
                    {
                        Name = j.Metadata?.Name,
                        Namespace = j.Metadata?.NamespaceProperty,
                        Completions = j.Spec?.Completions,
                        Successful = j.Status?.Succeeded,
                        Active = j.Status?.Active,
                        Failed = j.Status?.Failed,
                        Age = j.Metadata?.CreationTimestamp?.ToString("yyyy-MM-dd HH:mm:ss")
                    }));
                    break;
                case "CronJobs":
                    var cronjobs = await _explorer.ListCronJobsAsync(ns).ConfigureAwait(false);
                    _view.BindList(cronjobs.Select(c => new
                    {
                        Name = c.Metadata?.Name,
                        Namespace = c.Metadata?.NamespaceProperty,
                        Schedule = c.Spec?.Schedule,
                        Suspend = c.Spec?.Suspend,
                        Active = c.Status?.Active?.Count ?? 0,
                        LastSchedule = c.Status?.LastScheduleTime?.ToString("yyyy-MM-dd HH:mm:ss"),
                        Age = c.Metadata?.CreationTimestamp?.ToString("yyyy-MM-dd HH:mm:ss")
                    }));
                    break;
                case "Services":
                    var svcs = await _explorer.ListServicesAsync(ns).ConfigureAwait(false);
                    _view.BindList(svcs.Select(s => new
                    {
                        Name = s.Metadata?.Name,
                        Namespace = s.Metadata?.NamespaceProperty,
                        Type = s.Spec?.Type,
                        ClusterIP = s.Spec?.ClusterIP,
                        ExternalIP = string.Join(",", s.Spec?.ExternalIPs ?? new List<string>()),
                        Ports = string.Join(",", s.Spec?.Ports?.Select(p => $"{p.Port}/{p.Protocol}") ?? System.Linq.Enumerable.Empty<string>()),
                        Age = s.Metadata?.CreationTimestamp?.ToString("yyyy-MM-dd HH:mm:ss")
                    }));
                    break;
                case "Ingresses":
                    var ingresses = await _explorer.ListIngressesAsync(ns).ConfigureAwait(false);
                    _view.BindList(ingresses.Select(i => new
                    {
                        Name = i.Metadata?.Name,
                        Namespace = i.Metadata?.NamespaceProperty,
                        Class = i.Spec?.IngressClassName,
                        Hosts = string.Join(",", i.Spec?.Rules?.Select(r => r.Host) ?? System.Linq.Enumerable.Empty<string>()),
                        Address = string.Join(",", i.Status?.LoadBalancer?.Ingress?.Select(lb => lb.Ip ?? lb.Hostname) ?? System.Linq.Enumerable.Empty<string>()),
                        Age = i.Metadata?.CreationTimestamp?.ToString("yyyy-MM-dd HH:mm:ss")
                    }));
                    break;
                case "NetworkPolicies":
                    var netpols = await _explorer.ListNetworkPoliciesAsync(ns).ConfigureAwait(false);
                    _view.BindList(netpols.Select(n => new
                    {
                        Name = n.Metadata?.Name,
                        Namespace = n.Metadata?.NamespaceProperty,
                        PodSelector = n.Spec?.PodSelector?.ToString(),
                        Age = n.Metadata?.CreationTimestamp?.ToString("yyyy-MM-dd HH:mm:ss")
                    }));
                    break;
                case "ConfigMaps":
                    var configmaps = await _explorer.ListConfigMapsAsync(ns).ConfigureAwait(false);
                    _view.BindList(configmaps.Select(c => new
                    {
                        Name = c.Metadata?.Name,
                        Namespace = c.Metadata?.NamespaceProperty,
                        Keys = c.Data?.Count ?? 0,
                        Age = c.Metadata?.CreationTimestamp?.ToString("yyyy-MM-dd HH:mm:ss")
                    }));
                    break;
                case "Secrets":
                    var secrets = await _explorer.ListSecretsAsync(ns).ConfigureAwait(false);
                    _view.BindList(secrets.Select(s => new
                    {
                        Name = s.Metadata?.Name,
                        Namespace = s.Metadata?.NamespaceProperty,
                        Type = s.Type,
                        Keys = s.Data?.Count ?? 0,
                        Age = s.Metadata?.CreationTimestamp?.ToString("yyyy-MM-dd HH:mm:ss")
                    }));
                    break;
                case "PersistentVolumeClaims":
                    var pvcs = await _explorer.ListPersistentVolumeClaimsAsync(ns).ConfigureAwait(false);
                    _view.BindList(pvcs.Select(p => new
                    {
                        Name = p.Metadata?.Name,
                        Namespace = p.Metadata?.NamespaceProperty,
                        Status = p.Status?.Phase,
                        Volume = p.Spec?.VolumeName,
                        Capacity = p.Status?.Capacity?.ContainsKey("storage") == true ? p.Status.Capacity["storage"].ToString() : "",
                        AccessModes = string.Join(",", p.Spec?.AccessModes ?? new List<string>()),
                        StorageClass = p.Spec?.StorageClassName,
                        Age = p.Metadata?.CreationTimestamp?.ToString("yyyy-MM-dd HH:mm:ss")
                    }));
                    break;
                case "PersistentVolumes":
                    var pvs = await _explorer.ListPersistentVolumesAsync().ConfigureAwait(false);
                    _view.BindList(pvs.Select(p => new
                    {
                        Name = p.Metadata?.Name,
                        Capacity = p.Spec?.Capacity?.ContainsKey("storage") == true ? p.Spec.Capacity["storage"].ToString() : "",
                        AccessModes = string.Join(",", p.Spec?.AccessModes ?? new List<string>()),
                        ReclaimPolicy = p.Spec?.PersistentVolumeReclaimPolicy,
                        Status = p.Status?.Phase,
                        Claim = p.Spec?.ClaimRef != null ? $"{p.Spec.ClaimRef.NamespaceProperty}/{p.Spec.ClaimRef.Name}" : "",
                        StorageClass = p.Spec?.StorageClassName,
                        Age = p.Metadata?.CreationTimestamp?.ToString("yyyy-MM-dd HH:mm:ss")
                    }));
                    break;
                case "ServiceAccounts":
                    var sas = await _explorer.ListServiceAccountsAsync(ns).ConfigureAwait(false);
                    _view.BindList(sas.Select(s => new
                    {
                        Name = s.Metadata?.Name,
                        Namespace = s.Metadata?.NamespaceProperty,
                        Secrets = s.Secrets?.Count ?? 0,
                        Age = s.Metadata?.CreationTimestamp?.ToString("yyyy-MM-dd HH:mm:ss")
                    }));
                    break;
                case "Roles":
                    var roles = await _explorer.ListRolesAsync(ns).ConfigureAwait(false);
                    _view.BindList(roles.Select(r => new
                    {
                        Name = r.Metadata?.Name,
                        Namespace = r.Metadata?.NamespaceProperty,
                        Age = r.Metadata?.CreationTimestamp?.ToString("yyyy-MM-dd HH:mm:ss")
                    }));
                    break;
                case "RoleBindings":
                    var roleBindings = await _explorer.ListRoleBindingsAsync(ns).ConfigureAwait(false);
                    _view.BindList(roleBindings.Select(r => new
                    {
                        Name = r.Metadata?.Name,
                        Namespace = r.Metadata?.NamespaceProperty,
                        Role = r.RoleRef?.Name,
                        Age = r.Metadata?.CreationTimestamp?.ToString("yyyy-MM-dd HH:mm:ss")
                    }));
                    break;
                case "ClusterRoles":
                    var clusterRoles = await _explorer.ListClusterRolesAsync().ConfigureAwait(false);
                    _view.BindList(clusterRoles.Select(r => new
                    {
                        Name = r.Metadata?.Name,
                        Age = r.Metadata?.CreationTimestamp?.ToString("yyyy-MM-dd HH:mm:ss")
                    }));
                    break;
                case "ClusterRoleBindings":
                    var clusterRoleBindings = await _explorer.ListClusterRoleBindingsAsync().ConfigureAwait(false);
                    _view.BindList(clusterRoleBindings.Select(r => new
                    {
                        Name = r.Metadata?.Name,
                        Role = r.RoleRef?.Name,
                        Age = r.Metadata?.CreationTimestamp?.ToString("yyyy-MM-dd HH:mm:ss")
                    }));
                    break;
                case "Nodes":
                    var nodes = await _explorer.ListNodesAsync().ConfigureAwait(false);
                    _view.BindList(nodes.Select(n => new
                    {
                        Name = n.Metadata?.Name,
                        Status = n.Status?.Conditions?.LastOrDefault(c => c.Type == "Ready")?.Status,
                        Roles = string.Join(",", n.Metadata?.Labels?.Where(l => l.Key.StartsWith("node-role.kubernetes.io/")).Select(l => l.Key.Replace("node-role.kubernetes.io/", "")) ?? System.Linq.Enumerable.Empty<string>()),
                        Version = n.Status?.NodeInfo?.KubeletVersion,
                        InternalIP = n.Status?.Addresses?.FirstOrDefault(a => a.Type == "InternalIP")?.Address,
                        OS = $"{n.Status?.NodeInfo?.OperatingSystem}/{n.Status?.NodeInfo?.Architecture}",
                        Age = n.Metadata?.CreationTimestamp?.ToString("yyyy-MM-dd HH:mm:ss")
                    }));
                    break;
                case "Namespaces":
                    var namespaces = await _explorer.ListNamespacesAsync().ConfigureAwait(false);
                    _view.BindList(namespaces.Select(n => new
                    {
                        Name = n.Metadata?.Name,
                        Status = n.Status?.Phase,
                        Age = n.Metadata?.CreationTimestamp?.ToString("yyyy-MM-dd HH:mm:ss")
                    }));
                    break;
                case "Events":
                    var evts = await _explorer.ListEventsAsync(ns).ConfigureAwait(false);
                    var rows = evts.Select(e => new { e.Type, e.Reason, e.Message, e.Involved, e.At });
                    _view.BindEvents(rows);
                    break;
            }
        }

        public async Task LoadYamlAsync(string ns, string kind, string name)
        {
            string yaml = await _explorer.GetResourceYamlAsync(kind, ns, name).ConfigureAwait(false);
            _view.ShowYaml(yaml);
        }

        public async Task DescribeAsync(string ns, string kind, string name)
        {
            await LoadYamlAsync(ns, kind, name).ConfigureAwait(false);
        }

        public async Task TailLogsAsync(string ns, string pod)
        {
            await foreach (var line in _k8s.TailLogsAsync(ns, pod, null).ConfigureAwait(false))
                _view.AppendLog(line);
        }

        public async Task DeleteResourceAsync(string ns, string kind, string name)
        {
            var startTime = DateTime.UtcNow;
            var success = false;
            string? errorMessage = null;

            try
            {
                success = await _explorer.DeleteResourceAsync(kind, ns, name).ConfigureAwait(false);

                if (success)
                {
                    MessageBox.Show($"Successfully deleted {kind} '{name}'", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    errorMessage = "Delete operation returned false";
                    MessageBox.Show($"Failed to delete {kind} '{name}'", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                success = false;
                errorMessage = ex.Message;
                MessageBox.Show($"Failed to delete {kind} '{name}': {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Audit log
                if (_auditService != null && _currentUserId.HasValue)
                {
                    var duration = (DateTime.UtcNow - startTime).TotalMilliseconds;
                    await _auditService.LogAsync(
                        _currentUserId.Value,
                        "Delete",
                        kind,
                        name,
                        ns,
                        success,
                        errorMessage
                    ).ConfigureAwait(false);
                }
            }
        }

        private async Task<List<T>> SafeListAsync<T>(Func<Task<List<T>>> operation, string resourceType)
        {
            var logFile = System.IO.Path.Combine(
                System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) ?? ".",
                "error.log");

            try
            {
                return await operation().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText(logFile, $"Error loading {resourceType}: {ex.Message}\n");
                return new List<T>();  // Return empty list on error
            }
        }

        private async Task<List<T>> SafeCallAsync<T>(Task<List<T>> task, string logFile, string resourceType)
        {
            try
            {
                return await task.ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText(logFile, $"Error loading {resourceType}: {ex.Message}\n");
                return new List<T>();  // Return empty list on error
            }
        }

        // ============ WORKLOAD OPERATIONS ============

        public async Task ScaleWorkloadAsync(string ns, string kind, string name)
        {
            var startTime = DateTime.UtcNow;
            bool success = false;
            string? errorMessage = null;
            int? replicas = null;

            using var dialog = new ScaleDialog(name);
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    replicas = dialog.Replicas;
                    success = kind switch
                    {
                        "Deployment" => await _explorer.ScaleDeploymentAsync(ns, name, replicas.Value).ConfigureAwait(false),
                        "StatefulSet" => await _explorer.ScaleStatefulSetAsync(ns, name, replicas.Value).ConfigureAwait(false),
                        _ => false
                    };

                    if (success)
                    {
                        MessageBox.Show($"Successfully scaled {kind} '{name}' to {replicas} replicas",
                            "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        await ReloadCurrentViewAsync().ConfigureAwait(false);
                    }
                    else
                    {
                        errorMessage = "Scale operation returned false";
                        MessageBox.Show($"Failed to scale {kind} '{name}'",
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    success = false;
                    errorMessage = ex.Message;
                    MessageBox.Show($"Failed to scale {kind} '{name}': {ex.Message}",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    // Audit log
                    if (_auditService != null && _currentUserId.HasValue && replicas.HasValue)
                    {
                        await _auditService.LogAsync(
                            _currentUserId.Value,
                            "Scale",
                            kind,
                            name,
                            ns,
                            success,
                            errorMessage
                        ).ConfigureAwait(false);
                    }
                }
            }
        }

        public async Task RestartWorkloadAsync(string ns, string kind, string name)
        {
            var startTime = DateTime.UtcNow;
            bool success = false;
            string? errorMessage = null;

            var result = MessageBox.Show(
                $"Are you sure you want to restart {kind} '{name}'?\nThis will trigger a rolling restart of all pods.",
                "Confirm Restart",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    success = kind switch
                    {
                        "Deployment" => await _explorer.RestartDeploymentAsync(ns, name).ConfigureAwait(false),
                        "StatefulSet" => await _explorer.RestartStatefulSetAsync(ns, name).ConfigureAwait(false),
                        "DaemonSet" => await _explorer.RestartDaemonSetAsync(ns, name).ConfigureAwait(false),
                        _ => false
                    };

                    if (success)
                    {
                        MessageBox.Show($"Successfully restarted {kind} '{name}'",
                            "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        await ReloadCurrentViewAsync().ConfigureAwait(false);
                    }
                    else
                    {
                        errorMessage = "Restart operation returned false";
                        MessageBox.Show($"Failed to restart {kind} '{name}'",
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    success = false;
                    errorMessage = ex.Message;
                    MessageBox.Show($"Failed to restart {kind} '{name}': {ex.Message}",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    // Audit log
                    if (_auditService != null && _currentUserId.HasValue)
                    {
                        await _auditService.LogAsync(
                            _currentUserId.Value,
                            "Restart",
                            kind,
                            name,
                            ns,
                            success,
                            errorMessage
                        ).ConfigureAwait(false);
                    }
                }
            }
        }

        public async Task ViewWorkloadPodsAsync(string ns, string kind, string name)
        {
            using var dialog = new WorkloadPodsDialog(
                name,
                kind,
                async () => await _explorer.GetWorkloadPodsAsync(ns, name, kind).ConfigureAwait(false),
                async (pod) => await ViewPodLogsAsync(ns, pod).ConfigureAwait(false),
                (pod) => OpenShellToPod(ns, pod)
            );
            dialog.ShowDialog();
        }

        public async Task ViewPodLogsAsync(string ns, k8s.Models.V1Pod pod)
        {
            var podName = pod.Metadata?.Name ?? "";
            var containers = pod.Spec?.Containers?.Select(c => c.Name).ToArray() ?? Array.Empty<string>();

            using var dialog = new LogsDialog(
                podName,
                containers,
                async (container, tailLines) => await _explorer.GetPodLogsAsync(ns, podName, container, tailLines).ConfigureAwait(false)
            );
            dialog.ShowDialog();
        }

        public async Task ShowPodDetailsAsync(string ns, string podName)
        {
            try
            {
                var pods = await _k8s.ListPodsAsync(ns).ConfigureAwait(false);
                var pod = pods.FirstOrDefault(p => p.Metadata?.Name == podName);

                if (pod != null)
                {
                    using var dialog = new PodDetailsDialog(
                        pod,
                        async (p) => await ViewPodLogsAsync(ns, p).ConfigureAwait(false),
                        (p) => OpenShellToPod(ns, p)
                    );
                    dialog.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading pod details: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void OpenShellToPod(string ns, k8s.Models.V1Pod pod)
        {
            var podName = pod.Metadata?.Name ?? "";
            var containers = pod.Spec?.Containers?.Select(c => c.Name).ToArray() ?? Array.Empty<string>();
            var container = containers.FirstOrDefault() ?? "";

            // Need K8sShellService instance - get from DI or create
            var shellService = new K8sShellService(_factory);
            using var dialog = new TerminalDialog(shellService, ns, podName, container);
            dialog.ShowDialog();
        }

        public async Task ShowDeploymentDetailsAsync(string ns, string deploymentName)
        {
            try
            {
                var deployments = await _explorer.ListDeploymentsAsync(ns).ConfigureAwait(false);
                var deployment = deployments.FirstOrDefault(d => d.Metadata?.Name == deploymentName);

                if (deployment != null)
                {
                    using var dialog = new DeploymentDetailsDialog(
                        deployment,
                        async (dep) => await ScaleDeploymentDialogAsync(ns, dep).ConfigureAwait(false),
                        async (dep) => await RestartDeploymentDialogAsync(ns, dep).ConfigureAwait(false),
                        async (dep) => await ViewDeploymentPodsAsync(ns, dep).ConfigureAwait(false)
                    );
                    dialog.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading deployment details: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task ScaleDeploymentDialogAsync(string ns, k8s.Models.V1Deployment deployment)
        {
            var name = deployment.Metadata?.Name ?? "";
            var currentReplicas = deployment.Spec?.Replicas ?? 0;

            using var dialog = new ScaleDialog(name, (int)currentReplicas);

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                var replicas = dialog.Replicas;
                await _explorer.ScaleDeploymentAsync(ns, name, replicas).ConfigureAwait(false);
                MessageBox.Show($"Scaled {name} to {replicas} replicas", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private async Task RestartDeploymentDialogAsync(string ns, k8s.Models.V1Deployment deployment)
        {
            var name = deployment.Metadata?.Name ?? "";
            var result = MessageBox.Show(
                $"Are you sure you want to restart Deployment '{name}'?",
                "Confirm Restart",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                await _explorer.RestartDeploymentAsync(ns, name).ConfigureAwait(false);
                MessageBox.Show($"Restarted {name}", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private async Task ViewDeploymentPodsAsync(string ns, k8s.Models.V1Deployment deployment)
        {
            try
            {
                var name = deployment.Metadata?.Name ?? "";

                using var dialog = new WorkloadPodsDialog(
                    name,
                    "Deployment",
                    async () => await _explorer.GetWorkloadPodsAsync(ns, name, "Deployment").ConfigureAwait(false),
                    (pod) => Task.Run(async () => await ViewPodLogsAsync(ns, pod).ConfigureAwait(false)),
                    (pod) => OpenShellToPod(ns, pod)
                );
                dialog.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading deployment pods: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public async Task ShowStatefulSetDetailsAsync(string ns, string statefulSetName)
        {
            try
            {
                var statefulSets = await _explorer.ListStatefulSetsAsync(ns).ConfigureAwait(false);
                var statefulSet = statefulSets.FirstOrDefault(s => s.Metadata?.Name == statefulSetName);

                if (statefulSet != null)
                {
                    using var dialog = new StatefulSetDetailsDialog(
                        statefulSet,
                        async (sts) => await ScaleStatefulSetDialogAsync(ns, sts).ConfigureAwait(false),
                        async (sts) => await RestartStatefulSetDialogAsync(ns, sts).ConfigureAwait(false),
                        async (sts) => await ViewStatefulSetPodsAsync(ns, sts).ConfigureAwait(false)
                    );
                    dialog.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading statefulset details: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task ScaleStatefulSetDialogAsync(string ns, k8s.Models.V1StatefulSet statefulSet)
        {
            var name = statefulSet.Metadata?.Name ?? "";
            var currentReplicas = statefulSet.Spec?.Replicas ?? 0;

            using var dialog = new ScaleDialog(name, (int)currentReplicas);

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                var replicas = dialog.Replicas;
                await _explorer.ScaleStatefulSetAsync(ns, name, replicas).ConfigureAwait(false);
                MessageBox.Show($"Scaled {name} to {replicas} replicas", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private async Task RestartStatefulSetDialogAsync(string ns, k8s.Models.V1StatefulSet statefulSet)
        {
            var name = statefulSet.Metadata?.Name ?? "";
            var result = MessageBox.Show(
                $"Are you sure you want to restart StatefulSet '{name}'?",
                "Confirm Restart",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                await _explorer.RestartStatefulSetAsync(ns, name).ConfigureAwait(false);
                MessageBox.Show($"Restarted {name}", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private async Task ViewStatefulSetPodsAsync(string ns, k8s.Models.V1StatefulSet statefulSet)
        {
            try
            {
                var name = statefulSet.Metadata?.Name ?? "";

                using var dialog = new WorkloadPodsDialog(
                    name,
                    "StatefulSet",
                    async () => await _explorer.GetWorkloadPodsAsync(ns, name, "StatefulSet").ConfigureAwait(false),
                    (pod) => Task.Run(async () => await ViewPodLogsAsync(ns, pod).ConfigureAwait(false)),
                    (pod) => OpenShellToPod(ns, pod)
                );
                dialog.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading statefulset pods: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public async Task ShowDaemonSetDetailsAsync(string ns, string daemonSetName)
        {
            try
            {
                var daemonSets = await _explorer.ListDaemonSetsAsync(ns).ConfigureAwait(false);
                var daemonSet = daemonSets.FirstOrDefault(d => d.Metadata?.Name == daemonSetName);

                if (daemonSet != null)
                {
                    using var dialog = new DaemonSetDetailsDialog(
                        daemonSet,
                        async (ds) => await RestartDaemonSetDialogAsync(ns, ds).ConfigureAwait(false),
                        async (ds) => await ViewDaemonSetPodsAsync(ns, ds).ConfigureAwait(false)
                    );
                    dialog.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading daemonset details: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task RestartDaemonSetDialogAsync(string ns, k8s.Models.V1DaemonSet daemonSet)
        {
            var name = daemonSet.Metadata?.Name ?? "";
            var result = MessageBox.Show(
                $"Are you sure you want to restart DaemonSet '{name}'?",
                "Confirm Restart",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                await _explorer.RestartDaemonSetAsync(ns, name).ConfigureAwait(false);
                MessageBox.Show($"Restarted {name}", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private async Task ViewDaemonSetPodsAsync(string ns, k8s.Models.V1DaemonSet daemonSet)
        {
            try
            {
                var name = daemonSet.Metadata?.Name ?? "";

                using var dialog = new WorkloadPodsDialog(
                    name,
                    "DaemonSet",
                    async () => await _explorer.GetWorkloadPodsAsync(ns, name, "DaemonSet").ConfigureAwait(false),
                    (pod) => Task.Run(async () => await ViewPodLogsAsync(ns, pod).ConfigureAwait(false)),
                    (pod) => OpenShellToPod(ns, pod)
                );
                dialog.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading daemonset pods: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task ReloadCurrentViewAsync()
        {
            // This will trigger a reload of the current view
            var ns = _view.SelectedNamespace;
            if (!string.IsNullOrEmpty(ns))
            {
                await LoadAllForNamespaceAsync(ns).ConfigureAwait(false);
            }
        }

        // ============ HELPER METHODS ============

        private string GetPodStatus(k8s.Models.V1Pod pod)
        {
            var phase = pod.Status?.Phase ?? "Unknown";

            // Check for container statuses
            if (pod.Status?.ContainerStatuses != null)
            {
                foreach (var cs in pod.Status.ContainerStatuses)
                {
                    // Check waiting state
                    if (cs.State?.Waiting != null)
                    {
                        var reason = cs.State.Waiting.Reason;
                        if (reason == "CrashLoopBackOff" || reason == "ImagePullBackOff" ||
                            reason == "ErrImagePull" || reason == "CreateContainerError")
                            return reason;
                    }

                    // Check terminated state
                    if (cs.State?.Terminated != null && !cs.Ready)
                    {
                        return cs.State.Terminated.Reason ?? "Error";
                    }
                }
            }

            // Check init container statuses
            if (pod.Status?.InitContainerStatuses != null)
            {
                var totalInit = pod.Status.InitContainerStatuses.Count;
                var completedInit = pod.Status.InitContainerStatuses.Count(c => c.State?.Terminated?.Reason == "Completed");

                if (completedInit < totalInit)
                    return $"Init:{completedInit}/{totalInit}";
            }

            return phase;
        }

        private string GetAge(DateTime? creationTime)
        {
            if (!creationTime.HasValue) return "Unknown";

            var age = DateTime.UtcNow - creationTime.Value;

            if (age.TotalDays >= 365)
                return $"{(int)(age.TotalDays / 365)}y";
            if (age.TotalDays >= 30)
                return $"{(int)(age.TotalDays / 30)}mo";
            if (age.TotalDays >= 1)
                return $"{(int)age.TotalDays}d";
            if (age.TotalHours >= 1)
                return $"{(int)age.TotalHours}h";
            if (age.TotalMinutes >= 1)
                return $"{(int)age.TotalMinutes}m";
            return $"{(int)age.TotalSeconds}s";
        }

        private string GetDeploymentCondition(k8s.Models.V1Deployment deployment)
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

        private string GetStatefulSetCondition(k8s.Models.V1StatefulSet sts)
        {
            var ready = sts.Status?.ReadyReplicas ?? 0;
            var desired = sts.Spec?.Replicas ?? 0;

            if (ready == desired) return "Ready";
            if (ready > 0) return "Updating";
            return "NotReady";
        }
    }
}
