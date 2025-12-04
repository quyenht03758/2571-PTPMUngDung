using System.Data;
using System.ComponentModel;
using K8sManager.Models.Entities;
using K8sManager.Services;
using MaterialSkin;
using MaterialSkin.Controls;

namespace K8sManager.Views
{
    [DesignerCategory("Form")]

    public partial class SettingsDialog : MaterialForm
    {
        private readonly ApiSettingsService _settingsService;
        private readonly MaterialTabControl _tabs = new MaterialTabControl();
        private readonly MaterialButton _btnSave = new MaterialButton();
        private readonly MaterialButton _btnCancel = new MaterialButton();

        // General Settings
        private readonly MaterialComboBox _cbTheme = new MaterialComboBox();
        private readonly MaterialComboBox _cbLanguage = new MaterialComboBox();
        private readonly MaterialSwitch _swAutoRefresh = new MaterialSwitch();
        private readonly MaterialTextBox2 _txtRefreshInterval = new MaterialTextBox2();

        // Security Settings
        private readonly MaterialTextBox2 _txtSessionTimeout = new MaterialTextBox2();
        private readonly MaterialSwitch _swRequire2FA = new MaterialSwitch();
        private readonly MaterialTextBox2 _txtPasswordMinLength = new MaterialTextBox2();

        // Integration Settings
        private readonly MaterialTextBox2 _txtSlackWebhook = new MaterialTextBox2();
        private readonly MaterialTextBox2 _txtEmailSmtp = new MaterialTextBox2();
        private readonly MaterialTextBox2 _txtEmailFrom = new MaterialTextBox2();
        private readonly MaterialSwitch _swNotifyOnError = new MaterialSwitch();

        // UI Settings
        private readonly MaterialComboBox _cbGridSize = new MaterialComboBox();
        private readonly MaterialSwitch _swAnimations = new MaterialSwitch();
        private readonly MaterialSwitch _swShowIcons = new MaterialSwitch();

        public SettingsDialog(ApiSettingsService settingsService)
        {
            InitializeComponent();
            
            _settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));

            Text = "⚙️ Application Settings";
            Size = new Size(800, 600);
            StartPosition = FormStartPosition.CenterScreen;
            MaximizeBox = false;

            var skin = MaterialSkinManager.Instance;
            skin.AddFormToManage(this);

            InitializeControls();
            _ = LoadSettingsAsync();
        }

        private void InitializeControls()
        {
            _tabs.Dock = DockStyle.Fill;

            // General Tab
            var tabGeneral = new TabPage("General");
            InitializeGeneralTab(tabGeneral);
            _tabs.TabPages.Add(tabGeneral);

            // Security Tab
            var tabSecurity = new TabPage("Security");
            InitializeSecurityTab(tabSecurity);
            _tabs.TabPages.Add(tabSecurity);

            // Integration Tab
            var tabIntegration = new TabPage("Integration");
            InitializeIntegrationTab(tabIntegration);
            _tabs.TabPages.Add(tabIntegration);

            // UI Tab
            var tabUI = new TabPage("UI Preferences");
            InitializeUITab(tabUI);
            _tabs.TabPages.Add(tabUI);

            // Bottom panel
            var bottomPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 60,
                Padding = new Padding(16, 12, 16, 12)
            };

            _btnSave.Text = "Save Settings";
            _btnSave.Type = MaterialButton.MaterialButtonType.Contained;
            _btnSave.Dock = DockStyle.Right;
            _btnSave.Click += async (s, e) => await SaveSettingsAsync();

            _btnCancel.Text = "Cancel";
            _btnCancel.Type = MaterialButton.MaterialButtonType.Text;
            _btnCancel.Dock = DockStyle.Right;
            _btnCancel.Click += (s, e) => Close();

            bottomPanel.Controls.Add(_btnSave);
            bottomPanel.Controls.Add(_btnCancel);

            Controls.Add(_tabs);
            Controls.Add(bottomPanel);
        }

        private void InitializeGeneralTab(TabPage tab)
        {
            var panel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20),
                ColumnCount = 2,
                RowCount = 5
            };
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40));
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60));

            // Theme
            panel.Controls.Add(new Label { Text = "Theme:", TextAlign = ContentAlignment.MiddleLeft, Dock = DockStyle.Fill }, 0, 0);
            _cbTheme.Hint = "Select Theme";
            _cbTheme.Items.AddRange(new[] { "Light", "Dark", "Auto" });
            _cbTheme.SelectedIndex = 0;
            panel.Controls.Add(_cbTheme, 1, 0);

            // Language
            panel.Controls.Add(new Label { Text = "Language:", TextAlign = ContentAlignment.MiddleLeft, Dock = DockStyle.Fill }, 0, 1);
            _cbLanguage.Hint = "Select Language";
            _cbLanguage.Items.AddRange(new[] { "English", "Vietnamese", "Chinese" });
            _cbLanguage.SelectedIndex = 0;
            panel.Controls.Add(_cbLanguage, 1, 1);

            // Auto Refresh
            panel.Controls.Add(new Label { Text = "Auto Refresh:", TextAlign = ContentAlignment.MiddleLeft, Dock = DockStyle.Fill }, 0, 2);
            _swAutoRefresh.Text = "Enable automatic refresh";
            _swAutoRefresh.Checked = true;
            panel.Controls.Add(_swAutoRefresh, 1, 2);

            // Refresh Interval
            panel.Controls.Add(new Label { Text = "Refresh Interval (seconds):", TextAlign = ContentAlignment.MiddleLeft, Dock = DockStyle.Fill }, 0, 3);
            _txtRefreshInterval.Hint = "Seconds";
            _txtRefreshInterval.Text = "30";
            panel.Controls.Add(_txtRefreshInterval, 1, 3);

            tab.Controls.Add(panel);
        }

        private void InitializeSecurityTab(TabPage tab)
        {
            var panel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20),
                ColumnCount = 2,
                RowCount = 4
            };
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40));
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60));

            // Session Timeout
            panel.Controls.Add(new Label { Text = "Session Timeout (minutes):", TextAlign = ContentAlignment.MiddleLeft, Dock = DockStyle.Fill }, 0, 0);
            _txtSessionTimeout.Hint = "Minutes";
            _txtSessionTimeout.Text = "60";
            panel.Controls.Add(_txtSessionTimeout, 1, 0);

            // 2FA
            panel.Controls.Add(new Label { Text = "Two-Factor Authentication:", TextAlign = ContentAlignment.MiddleLeft, Dock = DockStyle.Fill }, 0, 1);
            _swRequire2FA.Text = "Require 2FA for all users";
            _swRequire2FA.Checked = false;
            panel.Controls.Add(_swRequire2FA, 1, 1);

            // Password Min Length
            panel.Controls.Add(new Label { Text = "Password Min Length:", TextAlign = ContentAlignment.MiddleLeft, Dock = DockStyle.Fill }, 0, 2);
            _txtPasswordMinLength.Hint = "Characters";
            _txtPasswordMinLength.Text = "8";
            panel.Controls.Add(_txtPasswordMinLength, 1, 2);

            tab.Controls.Add(panel);
        }

        private void InitializeIntegrationTab(TabPage tab)
        {
            var panel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20),
                ColumnCount = 2,
                RowCount = 5
            };
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40));
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60));

            // Slack Webhook
            panel.Controls.Add(new Label { Text = "Slack Webhook URL:", TextAlign = ContentAlignment.MiddleLeft, Dock = DockStyle.Fill }, 0, 0);
            _txtSlackWebhook.Hint = "https://hooks.slack.com/services/...";
            panel.Controls.Add(_txtSlackWebhook, 1, 0);

            // SMTP Server
            panel.Controls.Add(new Label { Text = "SMTP Server:", TextAlign = ContentAlignment.MiddleLeft, Dock = DockStyle.Fill }, 0, 1);
            _txtEmailSmtp.Hint = "smtp.gmail.com:587";
            panel.Controls.Add(_txtEmailSmtp, 1, 1);

            // Email From
            panel.Controls.Add(new Label { Text = "Email From Address:", TextAlign = ContentAlignment.MiddleLeft, Dock = DockStyle.Fill }, 0, 2);
            _txtEmailFrom.Hint = "noreply@company.com";
            panel.Controls.Add(_txtEmailFrom, 1, 2);

            // Notify on Error
            panel.Controls.Add(new Label { Text = "Notifications:", TextAlign = ContentAlignment.MiddleLeft, Dock = DockStyle.Fill }, 0, 3);
            _swNotifyOnError.Text = "Send notifications on errors";
            _swNotifyOnError.Checked = true;
            panel.Controls.Add(_swNotifyOnError, 1, 3);

            tab.Controls.Add(panel);
        }

        private void InitializeUITab(TabPage tab)
        {
            var panel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20),
                ColumnCount = 2,
                RowCount = 4
            };
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40));
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60));

            // Grid Size
            panel.Controls.Add(new Label { Text = "Grid Row Height:", TextAlign = ContentAlignment.MiddleLeft, Dock = DockStyle.Fill }, 0, 0);
            _cbGridSize.Hint = "Select Size";
            _cbGridSize.Items.AddRange(new[] { "Compact", "Normal", "Large" });
            _cbGridSize.SelectedIndex = 1;
            panel.Controls.Add(_cbGridSize, 1, 0);

            // Animations
            panel.Controls.Add(new Label { Text = "Animations:", TextAlign = ContentAlignment.MiddleLeft, Dock = DockStyle.Fill }, 0, 1);
            _swAnimations.Text = "Enable UI animations";
            _swAnimations.Checked = true;
            panel.Controls.Add(_swAnimations, 1, 1);

            // Show Icons
            panel.Controls.Add(new Label { Text = "Icons:", TextAlign = ContentAlignment.MiddleLeft, Dock = DockStyle.Fill }, 0, 2);
            _swShowIcons.Text = "Show icons in resource list";
            _swShowIcons.Checked = true;
            panel.Controls.Add(_swShowIcons, 1, 2);

            tab.Controls.Add(panel);
        }

        private async Task LoadSettingsAsync()
        {
            try
            {
                var settings = await _settingsService.GetAllSettingsAsync();

                foreach (var setting in settings)
                {
                    if (setting.Value != null)
                    {
                        ApplySetting(setting.Key, setting.Value);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this,
                    $"Error loading settings:\n{ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void ApplySetting(string key, string value)
        {
            switch (key)
            {
                case "Theme":
                    _cbTheme.SelectedItem = value;
                    break;
                case "Language":
                    _cbLanguage.SelectedItem = value;
                    break;
                case "AutoRefresh":
                    _swAutoRefresh.Checked = bool.Parse(value);
                    break;
                case "RefreshInterval":
                    _txtRefreshInterval.Text = value;
                    break;
                case "SessionTimeout":
                    _txtSessionTimeout.Text = value;
                    break;
                case "Require2FA":
                    _swRequire2FA.Checked = bool.Parse(value);
                    break;
                case "PasswordMinLength":
                    _txtPasswordMinLength.Text = value;
                    break;
                case "SlackWebhook":
                    _txtSlackWebhook.Text = value;
                    break;
                case "EmailSmtp":
                    _txtEmailSmtp.Text = value;
                    break;
                case "EmailFrom":
                    _txtEmailFrom.Text = value;
                    break;
                case "NotifyOnError":
                    _swNotifyOnError.Checked = bool.Parse(value);
                    break;
                case "GridSize":
                    _cbGridSize.SelectedItem = value;
                    break;
                case "Animations":
                    _swAnimations.Checked = bool.Parse(value);
                    break;
                case "ShowIcons":
                    _swShowIcons.Checked = bool.Parse(value);
                    break;
            }
        }

        private async Task SaveSettingsAsync()
        {
            try
            {
                _btnSave.Enabled = false;
                _btnSave.Text = "Saving...";

                var settings = new Dictionary<string, string>
                {
                    { "Theme", _cbTheme.SelectedItem?.ToString() ?? "Light" },
                    { "Language", _cbLanguage.SelectedItem?.ToString() ?? "English" },
                    { "AutoRefresh", _swAutoRefresh.Checked.ToString() },
                    { "RefreshInterval", _txtRefreshInterval.Text },
                    { "SessionTimeout", _txtSessionTimeout.Text },
                    { "Require2FA", _swRequire2FA.Checked.ToString() },
                    { "PasswordMinLength", _txtPasswordMinLength.Text },
                    { "SlackWebhook", _txtSlackWebhook.Text },
                    { "EmailSmtp", _txtEmailSmtp.Text },
                    { "EmailFrom", _txtEmailFrom.Text },
                    { "NotifyOnError", _swNotifyOnError.Checked.ToString() },
                    { "GridSize", _cbGridSize.SelectedItem?.ToString() ?? "Normal" },
                    { "Animations", _swAnimations.Checked.ToString() },
                    { "ShowIcons", _swShowIcons.Checked.ToString() }
                };

                foreach (var setting in settings)
                {
                    await _settingsService.UpsertSettingAsync(setting.Key, setting.Value);
                }

                MessageBox.Show(this,
                    "Settings saved successfully!",
                    "Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this,
                    $"Error saving settings:\n{ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                _btnSave.Enabled = true;
                _btnSave.Text = "Save Settings";
            }
        }
    }
}
