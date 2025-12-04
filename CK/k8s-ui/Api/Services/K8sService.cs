using k8s;
using k8s.Models;
using K8sManager.Api.Domain.Repositories;

namespace K8sManager.Api.Services;

public class K8sService : IK8sService
{
    private readonly IClusterRepository _clusterRepo;

    public K8sService(IClusterRepository clusterRepo)
    {
        _clusterRepo = clusterRepo;
    }

    private async Task<IKubernetes> CreateClientAsync(string? kubeconfigPath = null, string? contextName = null)
    {
        if (string.IsNullOrEmpty(kubeconfigPath))
        {
            // Use default cluster
            var defaultCluster = await _clusterRepo.GetDefaultAsync().ConfigureAwait(false);
            if (defaultCluster == null)
                throw new InvalidOperationException("No default cluster configured");

            kubeconfigPath = defaultCluster.KubeconfigPath;
            contextName ??= defaultCluster.ContextName;
        }

        var expandedPath = Environment.ExpandEnvironmentVariables(kubeconfigPath);
        
        var config = string.IsNullOrWhiteSpace(contextName)
            ? KubernetesClientConfiguration.BuildConfigFromConfigFile(expandedPath)
            : KubernetesClientConfiguration.BuildConfigFromConfigFile(expandedPath, contextName);

        return new Kubernetes(config);
    }

    public async Task<IList<V1Namespace>> ListNamespacesAsync(string? kubeconfigPath = null, string? contextName = null)
    {
        using var client = await CreateClientAsync(kubeconfigPath, contextName).ConfigureAwait(false);
        var nsList = await client.CoreV1.ListNamespaceAsync().ConfigureAwait(false);
        return nsList.Items;
    }

    public async Task<V1Namespace> CreateNamespaceAsync(string name, string? kubeconfigPath = null, string? contextName = null)
    {
        using var client = await CreateClientAsync(kubeconfigPath, contextName).ConfigureAwait(false);
        
        var ns = new V1Namespace
        {
            Metadata = new V1ObjectMeta
            {
                Name = name
            }
        };

        return await client.CoreV1.CreateNamespaceAsync(body: ns).ConfigureAwait(false);
    }

    public async Task<IList<V1Pod>> ListPodsAsync(string @namespace, string? kubeconfigPath = null, string? contextName = null)
    {
        using var client = await CreateClientAsync(kubeconfigPath, contextName).ConfigureAwait(false);
        var pods = await client.CoreV1.ListNamespacedPodAsync(namespaceParameter: @namespace).ConfigureAwait(false);
        return pods.Items;
    }

    public async Task<V1Pod?> GetPodAsync(string @namespace, string name, string? kubeconfigPath = null, string? contextName = null)
    {
        using var client = await CreateClientAsync(kubeconfigPath, contextName).ConfigureAwait(false);
        try
        {
            return await client.CoreV1.ReadNamespacedPodAsync(name: name, namespaceParameter: @namespace).ConfigureAwait(false);
        }
        catch (k8s.Autorest.HttpOperationException ex) when (ex.Response.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return null;
        }
    }

    public async Task<string> GetPodLogsAsync(string @namespace, string name, string? container = null, string? kubeconfigPath = null, string? contextName = null)
    {
        using var client = await CreateClientAsync(kubeconfigPath, contextName).ConfigureAwait(false);
        
        try
        {
            using var stream = await client.CoreV1.ReadNamespacedPodLogAsync(
                name: name,
                namespaceParameter: @namespace,
                container: container,
                tailLines: 500
            ).ConfigureAwait(false);
            
            using var reader = new StreamReader(stream);
            return await reader.ReadToEndAsync().ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            return $"Error retrieving logs: {ex.Message}";
        }
    }

    public async Task<bool> DeletePodAsync(string @namespace, string name, string? kubeconfigPath = null, string? contextName = null)
    {
        using var client = await CreateClientAsync(kubeconfigPath, contextName).ConfigureAwait(false);
        
        try
        {
            var opts = new V1DeleteOptions();
            await client.CoreV1.DeleteNamespacedPodAsync(
                name: name,
                namespaceParameter: @namespace,
                body: opts
            ).ConfigureAwait(false);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<IList<V1Deployment>> ListDeploymentsAsync(string @namespace, string? kubeconfigPath = null, string? contextName = null)
    {
        using var client = await CreateClientAsync(kubeconfigPath, contextName).ConfigureAwait(false);
        var deployments = await client.AppsV1.ListNamespacedDeploymentAsync(namespaceParameter: @namespace).ConfigureAwait(false);
        return deployments.Items;
    }

    public async Task<bool> ScaleDeploymentAsync(string @namespace, string name, int replicas, string? kubeconfigPath = null, string? contextName = null)
    {
        using var client = await CreateClientAsync(kubeconfigPath, contextName).ConfigureAwait(false);
        
        try
        {
            var scale = new V1Scale
            {
                Metadata = new V1ObjectMeta
                {
                    Name = name,
                    NamespaceProperty = @namespace
                },
                Spec = new V1ScaleSpec
                {
                    Replicas = replicas
                }
            };

            await client.AppsV1.ReplaceNamespacedDeploymentScaleAsync(
                body: scale,
                name: name,
                namespaceParameter: @namespace
            ).ConfigureAwait(false);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<IList<V1Service>> ListServicesAsync(string @namespace, string? kubeconfigPath = null, string? contextName = null)
    {
        using var client = await CreateClientAsync(kubeconfigPath, contextName).ConfigureAwait(false);
        var services = await client.CoreV1.ListNamespacedServiceAsync(namespaceParameter: @namespace).ConfigureAwait(false);
        return services.Items;
    }

    public async Task<IList<V1ConfigMap>> ListConfigMapsAsync(string @namespace, string? kubeconfigPath = null, string? contextName = null)
    {
        using var client = await CreateClientAsync(kubeconfigPath, contextName).ConfigureAwait(false);
        var configMaps = await client.CoreV1.ListNamespacedConfigMapAsync(namespaceParameter: @namespace).ConfigureAwait(false);
        return configMaps.Items;
    }

    public async Task<IList<V1Secret>> ListSecretsAsync(string @namespace, string? kubeconfigPath = null, string? contextName = null)
    {
        using var client = await CreateClientAsync(kubeconfigPath, contextName).ConfigureAwait(false);
        var secrets = await client.CoreV1.ListNamespacedSecretAsync(namespaceParameter: @namespace).ConfigureAwait(false);
        return secrets.Items;
    }

    public async Task<(bool Success, string Message)> ApplyYamlAsync(string yamlContent, string? kubeconfigPath = null, string? contextName = null)
    {
        using var client = await CreateClientAsync(kubeconfigPath, contextName).ConfigureAwait(false);
        
        try
        {
            // Deserialize YAML to Kubernetes object
            var objects = KubernetesYaml.LoadAllFromString(yamlContent);
            var appliedResources = new List<string>();
            
            foreach (var obj in objects)
            {
                if (obj == null) continue;

                // Cast to IKubernetesObject<V1ObjectMeta> to access Kind and Metadata
                var k8sObject = obj as IKubernetesObject<V1ObjectMeta>;
                if (k8sObject == null) continue;

                var kind = k8sObject.Kind;
                var metadata = k8sObject.Metadata;
                var name = metadata?.Name;
                var ns = metadata?.NamespaceProperty;

                if (string.IsNullOrEmpty(name))
                {
                    return (false, "Resource name is required in YAML");
                }

                try
                {
                    // Try to create or replace the resource based on kind
                    switch (kind)
                    {
                        case "Pod":
                            var pod = KubernetesYaml.Deserialize<V1Pod>(yamlContent);
                            await client.CoreV1.ReplaceNamespacedPodAsync(pod, name, ns ?? "default").ConfigureAwait(false);
                            appliedResources.Add($"Pod/{name}");
                            break;

                        case "Deployment":
                            var deployment = KubernetesYaml.Deserialize<V1Deployment>(yamlContent);
                            await client.AppsV1.ReplaceNamespacedDeploymentAsync(deployment, name, ns ?? "default").ConfigureAwait(false);
                            appliedResources.Add($"Deployment/{name}");
                            break;

                        case "Service":
                            var service = KubernetesYaml.Deserialize<V1Service>(yamlContent);
                            await client.CoreV1.ReplaceNamespacedServiceAsync(service, name, ns ?? "default").ConfigureAwait(false);
                            appliedResources.Add($"Service/{name}");
                            break;

                        case "ConfigMap":
                            var configMap = KubernetesYaml.Deserialize<V1ConfigMap>(yamlContent);
                            await client.CoreV1.ReplaceNamespacedConfigMapAsync(configMap, name, ns ?? "default").ConfigureAwait(false);
                            appliedResources.Add($"ConfigMap/{name}");
                            break;

                        case "Secret":
                            var secret = KubernetesYaml.Deserialize<V1Secret>(yamlContent);
                            await client.CoreV1.ReplaceNamespacedSecretAsync(secret, name, ns ?? "default").ConfigureAwait(false);
                            appliedResources.Add($"Secret/{name}");
                            break;

                        default:
                            return (false, $"Unsupported resource kind: {kind}");
                    }
                }
                catch (k8s.Autorest.HttpOperationException ex) when (ex.Response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    // Resource doesn't exist, create it instead
                    switch (kind)
                    {
                        case "Pod":
                            var pod = KubernetesYaml.Deserialize<V1Pod>(yamlContent);
                            await client.CoreV1.CreateNamespacedPodAsync(pod, ns ?? "default").ConfigureAwait(false);
                            appliedResources.Add($"Pod/{name}");
                            break;

                        case "Deployment":
                            var deployment = KubernetesYaml.Deserialize<V1Deployment>(yamlContent);
                            await client.AppsV1.CreateNamespacedDeploymentAsync(deployment, ns ?? "default").ConfigureAwait(false);
                            appliedResources.Add($"Deployment/{name}");
                            break;

                        case "Service":
                            var service = KubernetesYaml.Deserialize<V1Service>(yamlContent);
                            await client.CoreV1.CreateNamespacedServiceAsync(service, ns ?? "default").ConfigureAwait(false);
                            appliedResources.Add($"Service/{name}");
                            break;

                        case "ConfigMap":
                            var configMap = KubernetesYaml.Deserialize<V1ConfigMap>(yamlContent);
                            await client.CoreV1.CreateNamespacedConfigMapAsync(configMap, ns ?? "default").ConfigureAwait(false);
                            appliedResources.Add($"ConfigMap/{name}");
                            break;

                        case "Secret":
                            var secret = KubernetesYaml.Deserialize<V1Secret>(yamlContent);
                            await client.CoreV1.CreateNamespacedSecretAsync(secret, ns ?? "default").ConfigureAwait(false);
                            appliedResources.Add($"Secret/{name}");
                            break;

                        default:
                            return (false, $"Unsupported resource kind: {kind}");
                    }
                }
            }

            var message = appliedResources.Any() 
                ? $"Successfully applied: {string.Join(", ", appliedResources)}" 
                : "No resources applied";
            
            return (true, message);
        }
        catch (Exception ex)
        {
            return (false, $"Error applying YAML: {ex.Message}");
        }
    }

    public async Task<string> GetPodYamlAsync(string @namespace, string name, string? kubeconfigPath = null, string? contextName = null)
    {
        using var client = await CreateClientAsync(kubeconfigPath, contextName).ConfigureAwait(false);
        var pod = await client.CoreV1.ReadNamespacedPodAsync(name: name, namespaceParameter: @namespace).ConfigureAwait(false);
        return KubernetesYaml.Serialize(pod);
    }

    public async Task<string> GetDeploymentYamlAsync(string @namespace, string name, string? kubeconfigPath = null, string? contextName = null)
    {
        using var client = await CreateClientAsync(kubeconfigPath, contextName).ConfigureAwait(false);
        var deployment = await client.AppsV1.ReadNamespacedDeploymentAsync(name: name, namespaceParameter: @namespace).ConfigureAwait(false);
        return KubernetesYaml.Serialize(deployment);
    }

    public async Task<string> GetServiceYamlAsync(string @namespace, string name, string? kubeconfigPath = null, string? contextName = null)
    {
        using var client = await CreateClientAsync(kubeconfigPath, contextName).ConfigureAwait(false);
        var service = await client.CoreV1.ReadNamespacedServiceAsync(name: name, namespaceParameter: @namespace).ConfigureAwait(false);
        return KubernetesYaml.Serialize(service);
    }

    public async Task<string> GetConfigMapYamlAsync(string @namespace, string name, string? kubeconfigPath = null, string? contextName = null)
    {
        using var client = await CreateClientAsync(kubeconfigPath, contextName).ConfigureAwait(false);
        var configMap = await client.CoreV1.ReadNamespacedConfigMapAsync(name: name, namespaceParameter: @namespace).ConfigureAwait(false);
        return KubernetesYaml.Serialize(configMap);
    }

    public async Task<string> GetSecretYamlAsync(string @namespace, string name, string? kubeconfigPath = null, string? contextName = null)
    {
        using var client = await CreateClientAsync(kubeconfigPath, contextName).ConfigureAwait(false);
        var secret = await client.CoreV1.ReadNamespacedSecretAsync(name: name, namespaceParameter: @namespace).ConfigureAwait(false);
        return KubernetesYaml.Serialize(secret);
    }
}
