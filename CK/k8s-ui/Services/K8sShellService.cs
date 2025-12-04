using System.Net.WebSockets;
using System.Text;
using K8sManager.Infrastructure;

namespace K8sManager.Services
{
    public interface IK8sShellService
    {
        Task<string> ExecCommandAsync(string ns, string pod, string container, string command);
    }

    public class K8sShellService : IK8sShellService
    {
        private readonly K8sClientFactory _factory;

        public K8sShellService(K8sClientFactory factory)
        {
            _factory = factory;
        }

        public async Task<string> ExecCommandAsync(string ns, string pod, string container, string command)
        {
            try
            {
                using var client = _factory.Create();
                var webSocket = await client.WebSocketNamespacedPodExecAsync(
                    name: pod,
                    @namespace: ns,
                    container: container,
                    command: new[] { "/bin/sh", "-c", command },
                    stderr: true,
                    stdin: false,
                    stdout: true,
                    tty: false
                ).ConfigureAwait(false);

                var output = new StringBuilder();
                var buffer = new byte[4096];
                var segment = new ArraySegment<byte>(buffer);

                using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));

                while (!cts.Token.IsCancellationRequested)
                {
                    var result = await webSocket.ReceiveAsync(segment, cts.Token).ConfigureAwait(false);

                    if (result.MessageType == WebSocketMessageType.Close)
                        break;

                    // Skip the first byte (channel indicator)
                    if (result.Count > 1)
                    {
                        var text = Encoding.UTF8.GetString(buffer, 1, result.Count - 1);
                        output.Append(text);
                    }

                    if (result.EndOfMessage)
                        break;
                }

                await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Done", CancellationToken.None).ConfigureAwait(false);
                return output.ToString();
            }
            catch (Exception ex)
            {
                return $"Error executing command: {ex.Message}";
            }
        }
    }
}
