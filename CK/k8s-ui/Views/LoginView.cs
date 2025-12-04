using K8sManager.Services;
using MaterialSkin;
using MaterialSkin.Controls;

namespace K8sManager.Views
{
    public partial class LoginView : MaterialForm
    {
        private readonly ApiSessionService _sessionService;
        private MaterialLabel _titleLabel = null!;
        private MaterialTextBox2 _usernameTextBox = null!;
        private MaterialTextBox2 _passwordTextBox = null!;
        private MaterialButton _loginButton = null!;
        private MaterialButton _exitButton = null!;
        private MaterialLabel _statusLabel = null!;

        public string? SessionToken { get; private set; }
        public string? Username { get; private set; }

        public LoginView(ApiSessionService sessionService)
        {
            _sessionService = sessionService ?? throw new ArgumentNullException(nameof(sessionService));
            InitializeComponent();
            
            _passwordTextBox.KeyDown += PasswordTextBox_KeyDown;
            _loginButton.Click += LoginButton_Click;
            _exitButton.Click += ExitButton_Click;
        }

        private void PasswordTextBox_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                LoginButton_Click(sender, e);
            }
        }

        private void ExitButton_Click(object? sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private async void LoginButton_Click(object? sender, EventArgs e)
        {
            var username = _usernameTextBox.Text?.Trim();
            var password = _passwordTextBox.Text;

            // Validation
            if (string.IsNullOrWhiteSpace(username))
            {
                ShowError("Vui lòng nhập tên đăng nhập");
                _usernameTextBox.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                ShowError("Vui lòng nhập mật khẩu");
                _passwordTextBox.Focus();
                return;
            }

            // Disable controls during login
            SetControlsEnabled(false);
            ShowStatus("Đang đăng nhập...", Color.Blue);

            try
            {
                var (success, token, message) = await _sessionService.LoginAsync(username, password, GetClientIp(), GetUserAgent()).ConfigureAwait(false);

                if (success && !string.IsNullOrEmpty(token))
                {
                    SessionToken = token;
                    Username = username;
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    ShowError(message ?? "Đăng nhập thất bại");
                    _passwordTextBox.Clear();
                    _passwordTextBox.Focus();
                }
            }
            catch (Exception ex)
            {
                ShowError($"Lỗi: {ex.Message}");
            }
            finally
            {
                SetControlsEnabled(true);
            }
        }

        private void SetControlsEnabled(bool enabled)
        {
            _usernameTextBox.Enabled = enabled;
            _passwordTextBox.Enabled = enabled;
            _loginButton.Enabled = enabled;
            _exitButton.Enabled = enabled;
        }

        private void ShowStatus(string message, Color color)
        {
            _statusLabel.Text = message;
            _statusLabel.ForeColor = color;
            _statusLabel.Visible = true;
        }

        private void ShowError(string message)
        {
            ShowStatus(message, Color.Red);
        }

        private string GetClientIp()
        {
            try
            {
                var host = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());
                foreach (var ip in host.AddressList)
                {
                    if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        return ip.ToString();
                    }
                }
            }
            catch { }
            return "127.0.0.1";
        }

        private string GetUserAgent()
        {
            return $"K8sManager/{Application.ProductVersion} ({Environment.OSVersion})";
        }
    }
}
