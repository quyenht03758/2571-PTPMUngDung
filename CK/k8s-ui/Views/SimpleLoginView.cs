using K8sManager.Services;
using System.ComponentModel;
using System.Security.Cryptography;
using System.Text;

namespace K8sManager.Views
{
    [DesignerCategory("Form")]
    public partial class SimpleLoginView : Form
    {
        private readonly ApiSessionService _sessionService;
        private const string REGISTRY_KEY = @"Software\K8sManager";

        public SimpleLoginView(ApiSessionService sessionService)
        {
            _sessionService = sessionService ?? throw new ArgumentNullException(nameof(sessionService));
            InitializeComponent();
            LoadKubernetesLogo();
            LoadSavedCredentials();
        }

        private void LoadKubernetesLogo()
        {
            try
            {
                // Create Kubernetes logo text as image
                var logo = new Bitmap(200, 200);
                using (var g = Graphics.FromImage(logo))
                {
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    g.Clear(Color.FromArgb(50, 115, 220));
                    
                    // Draw K8s text
                    using (var font = new Font("Segoe UI", 48, FontStyle.Bold))
                    using (var brush = new SolidBrush(Color.White))
                    {
                        var text = "☸ K8s";
                        var size = g.MeasureString(text, font);
                        g.DrawString(text, font, brush, 
                            (200 - size.Width) / 2, 
                            (200 - size.Height) / 2);
                    }
                }
                _logoPictureBox.Image = logo;
            }
            catch
            {
                // If logo creation fails, just leave it empty
            }
        }

        private void LoadSavedCredentials()
        {
            try
            {
                using var key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(REGISTRY_KEY);
                if (key != null)
                {
                    var username = key.GetValue("Username") as string;
                    var encryptedPassword = key.GetValue("Password") as string;
                    var rememberMe = key.GetValue("RememberMe") as string;

                    if (!string.IsNullOrEmpty(username) && rememberMe == "true")
                    {
                        _usernameTextBox.Text = username;
                        _rememberMeCheckBox.Checked = true;

                        if (!string.IsNullOrEmpty(encryptedPassword))
                        {
                            try
                            {
                                var decrypted = ProtectedData.Unprotect(
                                    Convert.FromBase64String(encryptedPassword),
                                    Encoding.UTF8.GetBytes("K8sManager"),
                                    DataProtectionScope.CurrentUser);
                                _passwordTextBox.Text = Encoding.UTF8.GetString(decrypted);
                            }
                            catch
                            {
                                // If decryption fails, just clear the password
                            }
                        }
                    }
                }
            }
            catch
            {
                // If loading fails, just continue with empty fields
            }
        }

        private void SaveCredentials()
        {
            try
            {
                using var key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(REGISTRY_KEY);
                
                if (_rememberMeCheckBox.Checked)
                {
                    key.SetValue("Username", _usernameTextBox.Text);
                    key.SetValue("RememberMe", "true");

                    // Encrypt password before saving
                    var encrypted = ProtectedData.Protect(
                        Encoding.UTF8.GetBytes(_passwordTextBox.Text),
                        Encoding.UTF8.GetBytes("K8sManager"),
                        DataProtectionScope.CurrentUser);
                    key.SetValue("Password", Convert.ToBase64String(encrypted));
                }
                else
                {
                    key.DeleteValue("Username", false);
                    key.DeleteValue("Password", false);
                    key.DeleteValue("RememberMe", false);
                }
            }
            catch
            {
                // If saving fails, just continue
            }
        }

        private async void LoginButton_Click(object sender, EventArgs e)
        {
            var username = _usernameTextBox.Text.Trim();
            var password = _passwordTextBox.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                _statusLabel.Text = "Please enter username and password";
                _statusLabel.ForeColor = Color.Red;
                return;
            }

            _loginButton.Enabled = false;
            _statusLabel.Text = "Logging in...";
            _statusLabel.ForeColor = Color.FromArgb(50, 115, 220);

            try
            {
                var (success, _, message) = await _sessionService.LoginAsync(username, password).ConfigureAwait(false);

                if (success)
                {
                    if (InvokeRequired)
                    {
                        Invoke(() =>
                        {
                            _statusLabel.Text = "Login successful!";
                            _statusLabel.ForeColor = Color.Green;
                        });
                    }
                    else
                    {
                        _statusLabel.Text = "Login successful!";
                        _statusLabel.ForeColor = Color.Green;
                    }

                    // Save credentials if remember me is checked
                    SaveCredentials();

                    await Task.Delay(500).ConfigureAwait(false);
                    
                    if (InvokeRequired)
                    {
                        Invoke(() =>
                        {
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        });
                    }
                    else
                    {
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                }
                else
                {
                    if (InvokeRequired)
                    {
                        Invoke(() =>
                        {
                            _statusLabel.Text = message ?? "Invalid username or password";
                            _statusLabel.ForeColor = Color.Red;
                            _loginButton.Enabled = true;
                        });
                    }
                    else
                    {
                        _statusLabel.Text = message ?? "Invalid username or password";
                        _statusLabel.ForeColor = Color.Red;
                        _loginButton.Enabled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                if (InvokeRequired)
                {
                    Invoke(() =>
                    {
                        _statusLabel.Text = $"Error: {ex.Message}";
                        _statusLabel.ForeColor = Color.Red;
                        _loginButton.Enabled = true;

                        MessageBox.Show(
                            $"Login error: {ex.Message}\n\nPlease check if API server is running.",
                            "Login Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    });
                }
                else
                {
                    _statusLabel.Text = $"Error: {ex.Message}";
                    _statusLabel.ForeColor = Color.Red;
                    _loginButton.Enabled = true;

                    MessageBox.Show(
                        $"Login error: {ex.Message}\n\nPlease check if API server is running.",
                        "Login Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
        }
    }
}
