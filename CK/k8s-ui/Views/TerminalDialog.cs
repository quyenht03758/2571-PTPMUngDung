using K8sManager.Services;
using System.ComponentModel;
using MaterialSkin.Controls;

namespace K8sManager.Views
{
    [DesignerCategory("Form")]
    public partial class TerminalDialog : MaterialForm
    {
        private MaterialTextBox2 _txtCommand = null!;
        private MaterialButton _btnExecute = null!;
        private TextBox _txtOutput = null!;
        private Panel _panel = null!;

        private readonly IK8sShellService _shellService;
        private readonly string _namespace;
        private readonly string _pod;
        private readonly string _container;

        public TerminalDialog(IK8sShellService shellService, string ns, string pod, string container = "")
        {
            InitializeComponent();
            
            _shellService = shellService;
            _namespace = ns;
            _pod = pod;
            _container = container;

            Text = $"Terminal - {pod}";

            _btnExecute.Click += async (s, e) => await ExecuteCommandAsync().ConfigureAwait(false);
            _txtCommand.KeyDown += async (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                    await ExecuteCommandAsync().ConfigureAwait(false);
                }
            };

            _txtOutput.AppendText($"Connected to pod: {_pod}\r\n");
            _txtOutput.AppendText($"Namespace: {_namespace}\r\n");
            if (!string.IsNullOrEmpty(_container))
                _txtOutput.AppendText($"Container: {_container}\r\n");
            _txtOutput.AppendText(new string('-', 60) + "\r\n\r\n");
        }

        private async Task ExecuteCommandAsync()
        {
            var command = _txtCommand.Text?.Trim();
            if (string.IsNullOrEmpty(command))
                return;

            _txtOutput.AppendText($"$ {command}\r\n");
            _btnExecute.Enabled = false;
            _txtCommand.Enabled = false;

            try
            {
                var output = await _shellService.ExecCommandAsync(_namespace, _pod, _container, command).ConfigureAwait(false);
                _txtOutput.AppendText(output);
                if (!output.EndsWith("\r\n"))
                    _txtOutput.AppendText("\r\n");
            }
            catch (Exception ex)
            {
                _txtOutput.AppendText($"Error: {ex.Message}\r\n");
            }
            finally
            {
                _txtOutput.AppendText("\r\n");
                _txtCommand.Text = "";
                _btnExecute.Enabled = true;
                _txtCommand.Enabled = true;
                _txtCommand.Focus();
            }
        }
    }
}
