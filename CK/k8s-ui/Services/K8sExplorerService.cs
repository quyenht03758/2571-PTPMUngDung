using k8s;
using k8s.Autorest;
using k8s.Models;
using K8sManager.Infrastructure;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace K8sManager.Services
{
    // ✅ DTO ổn định, không phụ thuộc model SDK
    public record EventRow(string? Type, string? Reason, string? Message, string? Involved, DateTime? At);

    public interface IK8sExplorerService
    {
        // Workloads
        Task<IList<V1Deployment>> ListDeploymentsAsync(string ns);
        Task<IList<V1StatefulSet>> ListStatefulSetsAsync(string ns);
        Task<IList<V1DaemonSet>> ListDaemonSetsAsync(string ns);
        Task<IList<V1Job>> ListJobsAsync(string ns);
        Task<IList<V1CronJob>> ListCronJobsAsync(string ns);
        Task<IList<V1ReplicaSet>> ListReplicaSetsAsync(string ns);

        // Network
        Task<IList<V1Service>> ListServicesAsync(string ns);
        Task<IList<V1Ingress>> ListIngressesAsync(string ns);
        Task<IList<V1NetworkPolicy>> ListNetworkPoliciesAsync(string ns);

        // Config & Storage
        Task<IList<V1ConfigMap>> ListConfigMapsAsync(string ns);
        Task<IList<V1Secret>> ListSecretsAsync(string ns);
        Task<IList<V1PersistentVolumeClaim>> ListPersistentVolumeClaimsAsync(string ns);
        Task<IList<V1PersistentVolume>> ListPersistentVolumesAsync();

        // RBAC
        Task<IList<V1ServiceAccount>> ListServiceAccountsAsync(string ns);
        Task<IList<V1Role>> ListRolesAsync(string ns);
        Task<IList<V1RoleBinding>> ListRoleBindingsAsync(string ns);
        Task<IList<V1ClusterRole>> ListClusterRolesAsync();
        Task<IList<V1ClusterRoleBinding>> ListClusterRoleBindingsAsync();

        // Cluster
        Task<IList<V1Node>> ListNodesAsync();
        Task<IList<V1Namespace>> ListNamespacesAsync();

        // Events
        Task<IList<EventRow>> ListEventsAsync(string ns);

        // YAML operations
        Task<string> GetResourceYamlAsync(string kind, string ns, string name);
        Task<bool> ApplyYamlAsync(string yaml);
        Task<bool> DeleteResourceAsync(string kind, string ns, string name);

        // Port Forward & Shell
        Task PortForwardAsync(string ns, string podName, int podPort, int localPort, CancellationToken cancellationToken);
        Task ExecShellAsync(string ns, string podName, string containerName);
        Task<V1Pod> GetPodAsync(string ns, string name);

        // Workload Operations
        Task<bool> ScaleDeploymentAsync(string ns, string name, int replicas);
        Task<bool> ScaleStatefulSetAsync(string ns, string name, int replicas);
        Task<bool> RestartDeploymentAsync(string ns, string name);
        Task<bool> RestartStatefulSetAsync(string ns, string name);
        Task<bool> RestartDaemonSetAsync(string ns, string name);
        Task<string> GetPodLogsAsync(string ns, string name, string? container = null, int tailLines = 100);
        Task<IList<V1Pod>> GetWorkloadPodsAsync(string ns, string workloadName, string kind);
    }

    public class K8sExplorerService : IK8sExplorerService
    {
        private readonly K8sClientFactory _factory;
        private readonly ISerializer _yaml;

        public K8sExplorerService(K8sClientFactory factory)
        {
            _factory = factory;
            _yaml = new SerializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .ConfigureDefaultValuesHandling(DefaultValuesHandling.OmitNull)
                .Build();
        }

        public async Task<IList<V1Deployment>> ListDeploymentsAsync(string ns)
        {
            using var c = _factory.Create();
            var res = await c.AppsV1.ListNamespacedDeploymentAsync(namespaceParameter: ns).ConfigureAwait(false);
            return res.Items;
        }

        public async Task<IList<V1Service>> ListServicesAsync(string ns)
        {
            using var c = _factory.Create();
            var res = await c.CoreV1.ListNamespacedServiceAsync(namespaceParameter: ns).ConfigureAwait(false);
            return res.Items;
        }

        // ✅ Lấy events theo core/v1 (ổn định với đa số cluster), map -> EventRow
        public async Task<IList<EventRow>> ListEventsAsync(string ns)
        {
            using var c = _factory.Create();

            // Thử CoreV1 trước (đa số cluster có)
            try
            {
                var res = await c.CoreV1.ListNamespacedEventAsync(namespaceParameter: ns).ConfigureAwait(false);
                return res.Items.Select(e => new EventRow(
                    e.Type,
                    e.Reason,
                    e.Message,
                    $"{e.InvolvedObject?.Kind}/{e.InvolvedObject?.Name}",
                    // ưu tiên LastTimestamp, fallback FirstTimestamp
                    e.LastTimestamp ?? e.FirstTimestamp
                )).ToList();
            }
            catch
            {
                // Fallback: events.k8s.io/v1 ở một số cluster mới
                var res2 = await c.EventsV1.ListNamespacedEventAsync(namespaceParameter: ns).ConfigureAwait(false);

                return res2.Items.Select(e =>
                {
                    // ⚠️ Ở SDK bản của bạn, EventTime là DateTime? (không có UtcDateTime)
                    // Lấy EventTime nếu có, nếu không fallback deprecated timestamps
                    DateTime? at = e.EventTime
                                   ?? e.DeprecatedLastTimestamp
                                   ?? e.DeprecatedFirstTimestamp;

                    return new EventRow(
                        e.Type,
                        e.Reason,
                        // events.k8s.io/v1 dùng "Note" thay cho "Message"
                        e.Note,
                        $"{e.Regarding?.Kind}/{e.Regarding?.Name}",
                        at
                    );
                }).ToList();
            }
        }


        public async Task<string> GetPodYamlAsync(string ns, string name)
        {
            using var c = _factory.Create();
            var obj = await c.CoreV1.ReadNamespacedPodAsync(name, ns).ConfigureAwait(false);
            return _yaml.Serialize(obj);
        }

        public async Task<string> GetDeploymentYamlAsync(string ns, string name)
        {
            using var c = _factory.Create();
            var obj = await c.AppsV1.ReadNamespacedDeploymentAsync(name, ns).ConfigureAwait(false);
            return _yaml.Serialize(obj);
        }

        public async Task<string> GetServiceYamlAsync(string ns, string name)
        {
            using var c = _factory.Create();
            var obj = await c.CoreV1.ReadNamespacedServiceAsync(name, ns).ConfigureAwait(false);
            return _yaml.Serialize(obj);
        }

        // ===== Workloads =====
        public async Task<IList<V1StatefulSet>> ListStatefulSetsAsync(string ns)
        {
            using var c = _factory.Create();
            var res = await c.AppsV1.ListNamespacedStatefulSetAsync(namespaceParameter: ns).ConfigureAwait(false);
            return res.Items;
        }

        public async Task<IList<V1DaemonSet>> ListDaemonSetsAsync(string ns)
        {
            using var c = _factory.Create();
            var res = await c.AppsV1.ListNamespacedDaemonSetAsync(namespaceParameter: ns).ConfigureAwait(false);
            return res.Items;
        }

        public async Task<IList<V1Job>> ListJobsAsync(string ns)
        {
            using var c = _factory.Create();
            var res = await c.BatchV1.ListNamespacedJobAsync(namespaceParameter: ns).ConfigureAwait(false);
            return res.Items;
        }

        public async Task<IList<V1CronJob>> ListCronJobsAsync(string ns)
        {
            using var c = _factory.Create();
            var res = await c.BatchV1.ListNamespacedCronJobAsync(namespaceParameter: ns).ConfigureAwait(false);
            return res.Items;
        }

        public async Task<IList<V1ReplicaSet>> ListReplicaSetsAsync(string ns)
        {
            using var c = _factory.Create();
            var res = await c.AppsV1.ListNamespacedReplicaSetAsync(namespaceParameter: ns).ConfigureAwait(false);
            return res.Items;
        }

        // ===== Network =====
        public async Task<IList<V1Ingress>> ListIngressesAsync(string ns)
        {
            using var c = _factory.Create();
            var res = await c.NetworkingV1.ListNamespacedIngressAsync(namespaceParameter: ns).ConfigureAwait(false);
            return res.Items;
        }

        public async Task<IList<V1NetworkPolicy>> ListNetworkPoliciesAsync(string ns)
        {
            using var c = _factory.Create();
            var res = await c.NetworkingV1.ListNamespacedNetworkPolicyAsync(namespaceParameter: ns).ConfigureAwait(false);
            return res.Items;
        }

        // ===== Config & Storage =====
        public async Task<IList<V1ConfigMap>> ListConfigMapsAsync(string ns)
        {
            using var c = _factory.Create();
            var res = await c.CoreV1.ListNamespacedConfigMapAsync(namespaceParameter: ns).ConfigureAwait(false);
            return res.Items;
        }

        public async Task<IList<V1Secret>> ListSecretsAsync(string ns)
        {
            using var c = _factory.Create();
            var res = await c.CoreV1.ListNamespacedSecretAsync(namespaceParameter: ns).ConfigureAwait(false);
            return res.Items;
        }

        public async Task<IList<V1PersistentVolumeClaim>> ListPersistentVolumeClaimsAsync(string ns)
        {
            using var c = _factory.Create();
            var res = await c.CoreV1.ListNamespacedPersistentVolumeClaimAsync(namespaceParameter: ns).ConfigureAwait(false);
            return res.Items;
        }

        public async Task<IList<V1PersistentVolume>> ListPersistentVolumesAsync()
        {
            using var c = _factory.Create();
            var res = await c.CoreV1.ListPersistentVolumeAsync().ConfigureAwait(false);
            return res.Items;
        }

        // ===== RBAC =====
        public async Task<IList<V1ServiceAccount>> ListServiceAccountsAsync(string ns)
        {
            using var c = _factory.Create();
            var res = await c.CoreV1.ListNamespacedServiceAccountAsync(namespaceParameter: ns).ConfigureAwait(false);
            return res.Items;
        }

        public async Task<IList<V1Role>> ListRolesAsync(string ns)
        {
            using var c = _factory.Create();
            var res = await c.RbacAuthorizationV1.ListNamespacedRoleAsync(namespaceParameter: ns).ConfigureAwait(false);
            return res.Items;
        }

        public async Task<IList<V1RoleBinding>> ListRoleBindingsAsync(string ns)
        {
            using var c = _factory.Create();
            var res = await c.RbacAuthorizationV1.ListNamespacedRoleBindingAsync(namespaceParameter: ns).ConfigureAwait(false);
            return res.Items;
        }

        public async Task<IList<V1ClusterRole>> ListClusterRolesAsync()
        {
            using var c = _factory.Create();
            var res = await c.RbacAuthorizationV1.ListClusterRoleAsync().ConfigureAwait(false);
            return res.Items;
        }

        public async Task<IList<V1ClusterRoleBinding>> ListClusterRoleBindingsAsync()
        {
            using var c = _factory.Create();
            var res = await c.RbacAuthorizationV1.ListClusterRoleBindingAsync().ConfigureAwait(false);
            return res.Items;
        }

        // ===== Cluster =====
        public async Task<IList<V1Node>> ListNodesAsync()
        {
            using var c = _factory.Create();
            var res = await c.CoreV1.ListNodeAsync().ConfigureAwait(false);
            return res.Items;
        }

        public async Task<IList<V1Namespace>> ListNamespacesAsync()
        {
            using var c = _factory.Create();
            var res = await c.CoreV1.ListNamespaceAsync().ConfigureAwait(false);
            return res.Items;
        }

        // ===== Generic YAML operations =====
        public async Task<string> GetResourceYamlAsync(string kind, string ns, string name)
        {
            using var c = _factory.Create();
            object? obj = kind switch
            {
                "Pods" => await c.CoreV1.ReadNamespacedPodAsync(name, ns).ConfigureAwait(false),
                "Deployments" => await c.AppsV1.ReadNamespacedDeploymentAsync(name, ns).ConfigureAwait(false),
                "StatefulSets" => await c.AppsV1.ReadNamespacedStatefulSetAsync(name, ns).ConfigureAwait(false),
                "DaemonSets" => await c.AppsV1.ReadNamespacedDaemonSetAsync(name, ns).ConfigureAwait(false),
                "ReplicaSets" => await c.AppsV1.ReadNamespacedReplicaSetAsync(name, ns).ConfigureAwait(false),
                "Jobs" => await c.BatchV1.ReadNamespacedJobAsync(name, ns).ConfigureAwait(false),
                "CronJobs" => await c.BatchV1.ReadNamespacedCronJobAsync(name, ns).ConfigureAwait(false),
                "Services" => await c.CoreV1.ReadNamespacedServiceAsync(name, ns).ConfigureAwait(false),
                "Ingresses" => await c.NetworkingV1.ReadNamespacedIngressAsync(name, ns).ConfigureAwait(false),
                "NetworkPolicies" => await c.NetworkingV1.ReadNamespacedNetworkPolicyAsync(name, ns).ConfigureAwait(false),
                "ConfigMaps" => await c.CoreV1.ReadNamespacedConfigMapAsync(name, ns).ConfigureAwait(false),
                "Secrets" => await c.CoreV1.ReadNamespacedSecretAsync(name, ns).ConfigureAwait(false),
                "PersistentVolumeClaims" => await c.CoreV1.ReadNamespacedPersistentVolumeClaimAsync(name, ns).ConfigureAwait(false),
                "PersistentVolumes" => await c.CoreV1.ReadPersistentVolumeAsync(name).ConfigureAwait(false),
                "ServiceAccounts" => await c.CoreV1.ReadNamespacedServiceAccountAsync(name, ns).ConfigureAwait(false),
                "Roles" => await c.RbacAuthorizationV1.ReadNamespacedRoleAsync(name, ns).ConfigureAwait(false),
                "RoleBindings" => await c.RbacAuthorizationV1.ReadNamespacedRoleBindingAsync(name, ns).ConfigureAwait(false),
                "ClusterRoles" => await c.RbacAuthorizationV1.ReadClusterRoleAsync(name).ConfigureAwait(false),
                "ClusterRoleBindings" => await c.RbacAuthorizationV1.ReadClusterRoleBindingAsync(name).ConfigureAwait(false),
                "Nodes" => await c.CoreV1.ReadNodeAsync(name).ConfigureAwait(false),
                "Namespaces" => await c.CoreV1.ReadNamespaceAsync(name).ConfigureAwait(false),
                _ => null
            };
            return obj != null ? _yaml.Serialize(obj) : $"# Kind '{kind}' not supported";
        }

        public async Task<bool> ApplyYamlAsync(string yaml)
        {
            try
            {
                using var c = _factory.Create();
                var deserializer = new DeserializerBuilder()
                    .WithNamingConvention(CamelCaseNamingConvention.Instance)
                    .Build();

                var obj = deserializer.Deserialize<Dictionary<string, object>>(yaml);
                if (!obj.ContainsKey("kind") || !obj.ContainsKey("apiVersion"))
                    return false;

                var kind = obj["kind"].ToString();
                var metadata = obj.ContainsKey("metadata") ? obj["metadata"] as Dictionary<object, object> : null;
                var ns = metadata?.ContainsKey("namespace") == true ? metadata["namespace"]?.ToString() : "default";

                Console.WriteLine($"[ApplyYaml] Applying {kind} to namespace {ns}");

                // Apply resource based on kind
                switch (kind)
                {
                    case "Deployment":
                        var deployment = KubernetesYaml.Deserialize<V1Deployment>(yaml);
                        try
                        {
                            await c.AppsV1.CreateNamespacedDeploymentAsync(deployment, ns).ConfigureAwait(false);
                            Console.WriteLine($"[ApplyYaml] Created deployment {deployment.Metadata.Name}");
                        }
                        catch (HttpOperationException ex) when (ex.Response.StatusCode == System.Net.HttpStatusCode.Conflict)
                        {
                            // Resource exists, update it
                            await c.AppsV1.ReplaceNamespacedDeploymentAsync(deployment, deployment.Metadata.Name, ns).ConfigureAwait(false);
                            Console.WriteLine($"[ApplyYaml] Updated deployment {deployment.Metadata.Name}");
                        }
                        break;

                    case "Service":
                        var service = KubernetesYaml.Deserialize<V1Service>(yaml);
                        try
                        {
                            await c.CoreV1.CreateNamespacedServiceAsync(service, ns).ConfigureAwait(false);
                            Console.WriteLine($"[ApplyYaml] Created service {service.Metadata.Name}");
                        }
                        catch (HttpOperationException ex) when (ex.Response.StatusCode == System.Net.HttpStatusCode.Conflict)
                        {
                            await c.CoreV1.ReplaceNamespacedServiceAsync(service, service.Metadata.Name, ns).ConfigureAwait(false);
                            Console.WriteLine($"[ApplyYaml] Updated service {service.Metadata.Name}");
                        }
                        break;

                    case "ConfigMap":
                        var configMap = KubernetesYaml.Deserialize<V1ConfigMap>(yaml);
                        try
                        {
                            await c.CoreV1.CreateNamespacedConfigMapAsync(configMap, ns).ConfigureAwait(false);
                            Console.WriteLine($"[ApplyYaml] Created configmap {configMap.Metadata.Name}");
                        }
                        catch (HttpOperationException ex) when (ex.Response.StatusCode == System.Net.HttpStatusCode.Conflict)
                        {
                            await c.CoreV1.ReplaceNamespacedConfigMapAsync(configMap, configMap.Metadata.Name, ns).ConfigureAwait(false);
                            Console.WriteLine($"[ApplyYaml] Updated configmap {configMap.Metadata.Name}");
                        }
                        break;

                    case "Secret":
                        var secret = KubernetesYaml.Deserialize<V1Secret>(yaml);
                        try
                        {
                            await c.CoreV1.CreateNamespacedSecretAsync(secret, ns).ConfigureAwait(false);
                            Console.WriteLine($"[ApplyYaml] Created secret {secret.Metadata.Name}");
                        }
                        catch (HttpOperationException ex) when (ex.Response.StatusCode == System.Net.HttpStatusCode.Conflict)
                        {
                            await c.CoreV1.ReplaceNamespacedSecretAsync(secret, secret.Metadata.Name, ns).ConfigureAwait(false);
                            Console.WriteLine($"[ApplyYaml] Updated secret {secret.Metadata.Name}");
                        }
                        break;

                    case "StatefulSet":
                        var statefulSet = KubernetesYaml.Deserialize<V1StatefulSet>(yaml);
                        try
                        {
                            await c.AppsV1.CreateNamespacedStatefulSetAsync(statefulSet, ns).ConfigureAwait(false);
                            Console.WriteLine($"[ApplyYaml] Created statefulset {statefulSet.Metadata.Name}");
                        }
                        catch (HttpOperationException ex) when (ex.Response.StatusCode == System.Net.HttpStatusCode.Conflict)
                        {
                            await c.AppsV1.ReplaceNamespacedStatefulSetAsync(statefulSet, statefulSet.Metadata.Name, ns).ConfigureAwait(false);
                            Console.WriteLine($"[ApplyYaml] Updated statefulset {statefulSet.Metadata.Name}");
                        }
                        break;

                    case "DaemonSet":
                        var daemonSet = KubernetesYaml.Deserialize<V1DaemonSet>(yaml);
                        try
                        {
                            await c.AppsV1.CreateNamespacedDaemonSetAsync(daemonSet, ns).ConfigureAwait(false);
                            Console.WriteLine($"[ApplyYaml] Created daemonset {daemonSet.Metadata.Name}");
                        }
                        catch (HttpOperationException ex) when (ex.Response.StatusCode == System.Net.HttpStatusCode.Conflict)
                        {
                            await c.AppsV1.ReplaceNamespacedDaemonSetAsync(daemonSet, daemonSet.Metadata.Name, ns).ConfigureAwait(false);
                            Console.WriteLine($"[ApplyYaml] Updated daemonset {daemonSet.Metadata.Name}");
                        }
                        break;

                    case "Pod":
                        var pod = KubernetesYaml.Deserialize<V1Pod>(yaml);
                        try
                        {
                            await c.CoreV1.CreateNamespacedPodAsync(pod, ns).ConfigureAwait(false);
                            Console.WriteLine($"[ApplyYaml] Created pod {pod.Metadata.Name}");
                        }
                        catch (HttpOperationException ex) when (ex.Response.StatusCode == System.Net.HttpStatusCode.Conflict)
                        {
                            // Pods cannot be updated, delete and recreate
                            await c.CoreV1.DeleteNamespacedPodAsync(pod.Metadata.Name, ns).ConfigureAwait(false);
                            await c.CoreV1.CreateNamespacedPodAsync(pod, ns).ConfigureAwait(false);
                            Console.WriteLine($"[ApplyYaml] Recreated pod {pod.Metadata.Name}");
                        }
                        break;

                    default:
                        Console.WriteLine($"[ApplyYaml] Unsupported resource kind: {kind}");
                        return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ApplyYaml] Error: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteResourceAsync(string kind, string ns, string name)
        {
            try
            {
                using var c = _factory.Create();
                var opts = new V1DeleteOptions();

                switch (kind)
                {
                    case "Pods":
                        await c.CoreV1.DeleteNamespacedPodAsync(name, ns, body: opts).ConfigureAwait(false);
                        break;
                    case "Deployments":
                        await c.AppsV1.DeleteNamespacedDeploymentAsync(name, ns, body: opts).ConfigureAwait(false);
                        break;
                    case "StatefulSets":
                        await c.AppsV1.DeleteNamespacedStatefulSetAsync(name, ns, body: opts).ConfigureAwait(false);
                        break;
                    case "DaemonSets":
                        await c.AppsV1.DeleteNamespacedDaemonSetAsync(name, ns, body: opts).ConfigureAwait(false);
                        break;
                    case "Services":
                        await c.CoreV1.DeleteNamespacedServiceAsync(name, ns, body: opts).ConfigureAwait(false);
                        break;
                    case "ConfigMaps":
                        await c.CoreV1.DeleteNamespacedConfigMapAsync(name, ns, body: opts).ConfigureAwait(false);
                        break;
                    case "Secrets":
                        await c.CoreV1.DeleteNamespacedSecretAsync(name, ns, body: opts).ConfigureAwait(false);
                        break;
                    default:
                        return false;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        // ============ WORKLOAD OPERATIONS ============

        /// <summary>Scale Deployment to specified replicas</summary>
        public async Task<bool> ScaleDeploymentAsync(string ns, string name, int replicas)
        {
            try
            {
                using var c = _factory.Create();
                var scale = new V1Scale
                {
                    Metadata = new V1ObjectMeta { Name = name, NamespaceProperty = ns },
                    Spec = new V1ScaleSpec { Replicas = replicas }
                };
                await c.AppsV1.ReplaceNamespacedDeploymentScaleAsync(scale, name, ns).ConfigureAwait(false);
                return true;
            }
            catch { return false; }
        }

        /// <summary>Scale StatefulSet to specified replicas</summary>
        public async Task<bool> ScaleStatefulSetAsync(string ns, string name, int replicas)
        {
            try
            {
                using var c = _factory.Create();
                var scale = new V1Scale
                {
                    Metadata = new V1ObjectMeta { Name = name, NamespaceProperty = ns },
                    Spec = new V1ScaleSpec { Replicas = replicas }
                };
                await c.AppsV1.ReplaceNamespacedStatefulSetScaleAsync(scale, name, ns).ConfigureAwait(false);
                return true;
            }
            catch { return false; }
        }

        /// <summary>Restart Deployment by adding/updating restart annotation</summary>
        public async Task<bool> RestartDeploymentAsync(string ns, string name)
        {
            try
            {
                using var c = _factory.Create();
                var deployment = await c.AppsV1.ReadNamespacedDeploymentAsync(name, ns).ConfigureAwait(false);

                if (deployment?.Spec?.Template?.Metadata == null) return false;

                // Add restart annotation
                if (deployment.Spec.Template.Metadata.Annotations == null)
                    deployment.Spec.Template.Metadata.Annotations = new Dictionary<string, string>();

                deployment.Spec.Template.Metadata.Annotations["kubectl.kubernetes.io/restartedAt"] =
                    DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ");

                await c.AppsV1.ReplaceNamespacedDeploymentAsync(deployment, name, ns).ConfigureAwait(false);
                return true;
            }
            catch { return false; }
        }

        /// <summary>Restart StatefulSet by adding/updating restart annotation</summary>
        public async Task<bool> RestartStatefulSetAsync(string ns, string name)
        {
            try
            {
                using var c = _factory.Create();
                var sts = await c.AppsV1.ReadNamespacedStatefulSetAsync(name, ns).ConfigureAwait(false);

                if (sts?.Spec?.Template?.Metadata == null) return false;

                // Add restart annotation
                if (sts.Spec.Template.Metadata.Annotations == null)
                    sts.Spec.Template.Metadata.Annotations = new Dictionary<string, string>();

                sts.Spec.Template.Metadata.Annotations["kubectl.kubernetes.io/restartedAt"] =
                    DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ");

                await c.AppsV1.ReplaceNamespacedStatefulSetAsync(sts, name, ns).ConfigureAwait(false);
                return true;
            }
            catch { return false; }
        }

        /// <summary>Restart DaemonSet by adding/updating restart annotation</summary>
        public async Task<bool> RestartDaemonSetAsync(string ns, string name)
        {
            try
            {
                using var c = _factory.Create();
                var ds = await c.AppsV1.ReadNamespacedDaemonSetAsync(name, ns).ConfigureAwait(false);

                if (ds?.Spec?.Template?.Metadata == null) return false;

                // Add restart annotation
                if (ds.Spec.Template.Metadata.Annotations == null)
                    ds.Spec.Template.Metadata.Annotations = new Dictionary<string, string>();

                ds.Spec.Template.Metadata.Annotations["kubectl.kubernetes.io/restartedAt"] =
                    DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ");

                await c.AppsV1.ReplaceNamespacedDaemonSetAsync(ds, name, ns).ConfigureAwait(false);
                return true;
            }
            catch { return false; }
        }

        /// <summary>Get Pod logs</summary>
        public async Task<string> GetPodLogsAsync(string ns, string name, string? container = null, int tailLines = 100)
        {
            try
            {
                using var c = _factory.Create();
                var logsStream = await c.CoreV1.ReadNamespacedPodLogAsync(
                    name,
                    ns,
                    container: container,
                    tailLines: tailLines
                ).ConfigureAwait(false);

                using var reader = new StreamReader(logsStream);
                return await reader.ReadToEndAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                return $"Error getting logs: {ex.Message}";
            }
        }

        /// <summary>Get workload pods</summary>
        public async Task<IList<V1Pod>> GetWorkloadPodsAsync(string ns, string workloadName, string kind)
        {
            try
            {
                using var c = _factory.Create();

                // Get label selector based on workload type
                string? labelSelector = kind switch
                {
                    "Deployment" => $"app={workloadName}",
                    "StatefulSet" => $"app={workloadName}",
                    "DaemonSet" => $"app={workloadName}",
                    _ => null
                };

                if (string.IsNullOrEmpty(labelSelector))
                    return new List<V1Pod>();

                var pods = await c.CoreV1.ListNamespacedPodAsync(ns, labelSelector: labelSelector).ConfigureAwait(false);
                return pods.Items;
            }
            catch
            {
                return new List<V1Pod>();
            }
        }

        /// <summary>Get single pod</summary>
        public async Task<V1Pod> GetPodAsync(string ns, string name)
        {
            using var c = _factory.Create();
            return await c.CoreV1.ReadNamespacedPodAsync(name, ns).ConfigureAwait(false);
        }

        /// <summary>Port forward to pod</summary>
        public async Task PortForwardAsync(string ns, string podName, int podPort, int localPort, CancellationToken cancellationToken)
        {
            using var c = _factory.Create();
            
            // Start port forwarding using WebSocket
            var webSocket = await c.WebSocketNamespacedPodPortForwardAsync(
                name: podName,
                @namespace: ns,
                ports: new[] { podPort },
                cancellationToken: cancellationToken
            ).ConfigureAwait(false);

            // Keep the connection alive until cancelled
            var buffer = new byte[4096];
            var segment = new ArraySegment<byte>(buffer);

            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    var result = await webSocket.ReceiveAsync(segment, cancellationToken).ConfigureAwait(false);
                    
                    if (result.MessageType == System.Net.WebSockets.WebSocketMessageType.Close)
                        break;
                }
            }
            finally
            {
                if (webSocket.State == System.Net.WebSockets.WebSocketState.Open)
                {
                    await webSocket.CloseAsync(
                        System.Net.WebSockets.WebSocketCloseStatus.NormalClosure,
                        "Closing",
                        CancellationToken.None
                    ).ConfigureAwait(false);
                }
            }
        }

        /// <summary>Open interactive shell in external terminal</summary>
        public async Task ExecShellAsync(string ns, string podName, string containerName)
        {
            // Get current context from kubeconfig
            var context = _factory.GetCurrentContext();
            
            // Build kubectl command for external terminal
            var kubectlCmd = $"kubectl exec -it {podName} -n {ns} -c {containerName} -- /bin/sh";
            
            if (!string.IsNullOrEmpty(context))
            {
                kubectlCmd = $"kubectl --context={context} exec -it {podName} -n {ns} -c {containerName} -- /bin/sh";
            }

            // Launch in external terminal
            var psi = new System.Diagnostics.ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = $"/k {kubectlCmd}",
                UseShellExecute = true,
                CreateNoWindow = false
            };

            System.Diagnostics.Process.Start(psi);
            
            await Task.CompletedTask.ConfigureAwait(false);
        }
    }
}
