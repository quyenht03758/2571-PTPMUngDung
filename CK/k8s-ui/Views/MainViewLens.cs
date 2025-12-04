using K8sManager.Controllers;
using System.ComponentModel;
using K8sManager.Services;
using MaterialSkin;
using MaterialSkin.Controls;

namespace K8sManager.Views
{
    [DesignerCategory("Form")]
    public partial class MainViewLens : MaterialForm, IExplorerViewController
    {
        public dynamic? Controller { get; set; }  // Changed to dynamic to support both ExplorerController and CleanExplorerController

        // Authentication and Services
        public string? SessionToken { get; set; }
        public string? CurrentUsername { get; set; }
        public ApiSessionService? SessionService { get; set; }
        public ApiAuditService? AuditService { get; set; }
        public ApiSettingsService? SettingsService { get; set; }

        // API Services
        public ApiTemplateService? TemplateService { get; set; }
        public ApiFavoriteService? FavoriteService { get; set; }

        // K8s Explorer Service for template application
        public K8sExplorerService? K8sExplorerService { get; set; }
        public K8sShellService? ShellService { get; set; }

        public string SelectedNamespace => _cbNamespace.SelectedItem?.ToString() ?? "default";
        public string SelectedContext => _cbContext.SelectedItem?.ToString() ?? "";

        private bool _isBinding = false;  // Flag to prevent event re-entry during binding
        private bool _isLoading = false;  // Flag to prevent multiple simultaneous loads
        private System.Threading.CancellationTokenSource? _loadCancellation;

        // Auto-refresh timer
        private System.Windows.Forms.Timer? _autoRefreshTimer;
        private bool _autoRefreshEnabled = false;
        private int _autoRefreshInterval = 30;  // seconds

        // Dark mode state
        private bool _isDarkMode = false;

        private System.Windows.Forms.TableLayoutPanel _topPanel = null!;
        private MaterialListView _explorer = null!;
        private MaterialComboBox _cbNamespace = null!;
        private MaterialComboBox _cbContext = null!;
        private MaterialTextBox2 _txtSearch = null!;
        private MaterialButton _btnRefresh = null!;
        private MaterialButton _btnTail = null!;
        private MaterialButton _btnYaml = null!;
        private MaterialButton _btnDescribe = null!;
        private MaterialButton _btnApply = null!;
        private MaterialButton _btnPF = null!;
        private MaterialButton _btnShell = null!;
        private MaterialButton _btnDelete = null!;
        private MaterialButton _btnTheme = null!;

        // SQL Features buttons
        private MaterialButton _btnTemplates = null!;
        private MaterialButton _btnFavorites = null!;
        private MaterialButton _btnUsers = null!;
        private MaterialButton _btnSettings = null!;
        private MaterialButton _btnAuditLogs = null!;

        private ToolTip _tooltip = null!;

        private MaterialTabControl _tabs = null!;
        private TabPage _tabList = null!;
        private TabPage _tabYaml = null!;
        private TabPage _tabLogs = null!;
        private TabPage _tabEvents = null!;
        private TabPage _tabMetrics = null!;

        private DoubleBufferedDataGridView _grid = null!;
        private RichTextBox _rtYaml = null!;
        private TextBox _txtLogs = null!;
        private DoubleBufferedDataGridView _gridEvents = null!;

        private SplitContainer _split = null!;
        private StatusStrip _status = null!;
        private ToolStripStatusLabel _statusInfo = null!;
        private ToolStripStatusLabel _statusNs = null!;
        private ToolStripStatusLabel _statusKind = null!;

        public MainViewLens()
        {
            InitializeComponent();
            
            // Initialize _btnUsers (not in Designer)
            _btnUsers = new MaterialButton();
            
            try
            {
                Text = "K8s Manager — Lens Desktop";
                StartPosition = FormStartPosition.CenterScreen;
                WindowState = FormWindowState.Maximized;
                MinimumSize = new Size(1200, 700);
                BackColor = Color.FromArgb(250, 250, 250);

                // Initialize MaterialSkin theme manager
                var skin = MaterialSkinManager.Instance;
                skin.AddFormToManage(this);
                skin.Theme = MaterialSkinManager.Themes.LIGHT;
                skin.ColorScheme = new ColorScheme(
                    Primary.Indigo600,      // Modern indigo primary
                    Primary.Indigo800,      // Darker indigo
                    Primary.Indigo200,      // Light indigo
                    Accent.Cyan200,         // Cyan accent for highlights
                    TextShade.WHITE
                );

                _explorer.Columns.Add("Resource Type", 235);
                _explorer.Columns.Add("Count", 55);
                _explorer.BackColor = Color.White;
                _explorer.Font = new Font("Segoe UI", 9f);
                _explorer.GridLines = false;
                _explorer.HeaderStyle = ColumnHeaderStyle.None;
                _explorer.HoverSelection = false;
                _explorer.Cursor = Cursors.Hand;
                _explorer.Margin = new Padding(0);
                _explorer.Padding = new Padding(0);

                // Custom colors for selection
                _explorer.ForeColor = Color.Black;
                _explorer.BackColor = Color.White;

                // Owner draw to customize selection color
                _explorer.OwnerDraw = true;
                _explorer.DrawItem += (s, e) =>
                {
                    if (e.Item == null) return;

                    // Check if this is a category item (has gray background)
                    bool isCategory = e.Item.BackColor == Color.FromArgb(245, 245, 245);

                    if (isCategory)
                    {
                        // Draw category with gray background
                        e.Graphics.FillRectangle(
                            new SolidBrush(Color.FromArgb(245, 245, 245)),
                            e.Bounds);

                        var textBounds = new Rectangle(e.Bounds.X + 3, e.Bounds.Y, e.Bounds.Width - 6, e.Bounds.Height);
                        TextRenderer.DrawText(
                            e.Graphics,
                            e.Item.Text,
                            new Font("Segoe UI Semibold", 9f, FontStyle.Bold),
                            textBounds,
                            Color.FromArgb(100, 100, 100),
                            TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
                    }
                    else if (e.Item.Selected)
                    {
                        // Draw selected resource with indigo background
                        e.Graphics.FillRectangle(
                            new SolidBrush(Color.FromArgb(63, 81, 181)),
                            e.Bounds);

                        var textBounds = new Rectangle(e.Bounds.X + 3, e.Bounds.Y, e.Bounds.Width - 6, e.Bounds.Height);
                        TextRenderer.DrawText(
                            e.Graphics,
                            e.Item.Text,
                            _explorer.Font,
                            textBounds,
                            Color.White,
                            TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
                    }
                    else
                    {
                        // Draw normal resource
                        e.Graphics.FillRectangle(Brushes.White, e.Bounds);
                        var textBounds = new Rectangle(e.Bounds.X + 3, e.Bounds.Y, e.Bounds.Width - 6, e.Bounds.Height);
                        TextRenderer.DrawText(
                            e.Graphics,
                            e.Item.Text,
                            _explorer.Font,
                            textBounds,
                            Color.Black,
                            TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
                    }
                };

                _explorer.DrawSubItem += (s, e) =>
                {
                    if (e.Item == null) return;

                    bool isCategory = e.Item.BackColor == Color.FromArgb(245, 245, 245);

                    if (isCategory)
                    {
                        e.Graphics.FillRectangle(
                            new SolidBrush(Color.FromArgb(245, 245, 245)),
                            e.Bounds);

                        var textBounds = new Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width - 3, e.Bounds.Height);
                        TextRenderer.DrawText(
                            e.Graphics,
                            e.SubItem?.Text ?? "",
                            new Font("Segoe UI Semibold", 9f, FontStyle.Bold),
                            textBounds,
                            Color.FromArgb(100, 100, 100),
                            TextFormatFlags.Right | TextFormatFlags.VerticalCenter);
                    }
                    else if (e.Item.Selected)
                    {
                        e.Graphics.FillRectangle(
                            new SolidBrush(Color.FromArgb(63, 81, 181)),
                            e.Bounds);

                        var textBounds = new Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width - 3, e.Bounds.Height);
                        TextRenderer.DrawText(
                            e.Graphics,
                            e.SubItem?.Text ?? "",
                            _explorer.Font,
                            textBounds,
                            Color.White,
                            TextFormatFlags.Right | TextFormatFlags.VerticalCenter);
                    }
                    else
                    {
                        e.Graphics.FillRectangle(Brushes.White, e.Bounds);
                        var textBounds = new Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width - 8, e.Bounds.Height);
                        TextRenderer.DrawText(
                            e.Graphics,
                            e.SubItem?.Text ?? "",
                            _explorer.Font,
                            textBounds,
                            Color.Black,
                            TextFormatFlags.Right | TextFormatFlags.VerticalCenter);
                    }
                };

                _explorer.DrawColumnHeader += (s, e) =>
                {
                    e.Graphics.FillRectangle(Brushes.White, e.Bounds);
                };

                // Populate explorer with resource categories
                PopulateExplorer();

                // Configure _topPanel (already created by InitializeComponent)
                _topPanel.Dock = DockStyle.Top;
                _topPanel.Height = 120; // Increased height for more spacing
                _topPanel.ColumnCount = 18;
                _topPanel.RowCount = 2;
                _topPanel.Padding = new Padding(12, 10, 12, 15); // Increased bottom padding
                _topPanel.Margin = new Padding(0, 0, 0, 5); // Add bottom margin
                _topPanel.BackColor = Color.FromArgb(63, 81, 181); // Indigo background
                _topPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 200));
                _topPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 200));
                _topPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 350));
                _topPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
                for (int i = 4; i < 18; i++) _topPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

                // Row styles
                _topPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 44));
                _topPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 44));
                
                // Configure button properties
                _btnRefresh.Text = "🔄 Refresh";
                _btnRefresh.Type = MaterialButton.MaterialButtonType.Contained;
                _btnRefresh.HighEmphasis = true;
                
                _btnTail.Text = "📝 Logs";
                _btnTail.Type = MaterialButton.MaterialButtonType.Contained;
                _btnTail.HighEmphasis = true;
                
                _btnYaml.Text = "📄 YAML";
                _btnYaml.Type = MaterialButton.MaterialButtonType.Contained;
                
                _btnDescribe.Text = "📋 Describe";
                _btnDescribe.Type = MaterialButton.MaterialButtonType.Contained;
                
                _btnApply.Text = "✅ Apply";
                _btnApply.Type = MaterialButton.MaterialButtonType.Contained;
                
                _btnPF.Text = "🔌 Port Forward";
                _btnPF.Type = MaterialButton.MaterialButtonType.Contained;
                
                _btnShell.Text = "💻 Shell";
                _btnShell.Type = MaterialButton.MaterialButtonType.Contained;
                
                _btnDelete.Text = "🗑️ Delete";
                _btnDelete.Type = MaterialButton.MaterialButtonType.Contained;
                
                _btnTheme.Text = "🌙 Dark";
                _btnTheme.Type = MaterialButton.MaterialButtonType.Contained;
                
                _btnTemplates.Text = "📋 Templates";
                _btnTemplates.Type = MaterialButton.MaterialButtonType.Contained;
                
                _btnFavorites.Text = "⭐ Favorites";
                _btnFavorites.Type = MaterialButton.MaterialButtonType.Contained;
                
                _btnUsers.Text = "👥 Users";
                _btnUsers.Type = MaterialButton.MaterialButtonType.Contained;
                
                _btnSettings.Text = "⚙️ Settings";
                _btnSettings.Type = MaterialButton.MaterialButtonType.Contained;
                
                _btnAuditLogs.Text = "📊 Audit Logs";
                _btnAuditLogs.Type = MaterialButton.MaterialButtonType.Contained;
                
                _cbNamespace.Hint = "Namespace";
                _cbNamespace.DropDownStyle = ComboBoxStyle.DropDownList;
                _cbNamespace.DropDownWidth = 300;  // Wide enough for long namespace names
                _cbNamespace.ForeColor = Color.White;  // White text on blue background
                
                _cbContext.Hint = "Context";
                _cbContext.DropDownStyle = ComboBoxStyle.DropDownList;
                _cbContext.DropDownWidth = 400;  // Wide enough for long context names
                _cbContext.ForeColor = Color.White;  // White text on blue background
                
                _txtSearch.Hint = "🔍 Search resources by name...";
                _txtSearch.UseTallSize = false;
                _txtSearch.ForeColor = Color.White;  // White text on blue background
                
                // Row 0: Context, Namespace, Search in left columns
                _topPanel.Controls.Add(_cbContext, 0, 0); _topPanel.SetRowSpan(_cbContext, 2);
                _topPanel.Controls.Add(_cbNamespace, 1, 0); _topPanel.SetRowSpan(_cbNamespace, 2);
                _topPanel.Controls.Add(_txtSearch, 2, 0); _topPanel.SetRowSpan(_txtSearch, 2);

                // Row 0: Action buttons with spacing
                _btnRefresh.Margin = new Padding(4, 0, 4, 0);
                _btnYaml.Margin = new Padding(4, 0, 4, 0);
                _btnDescribe.Margin = new Padding(4, 0, 4, 0);
                _btnApply.Margin = new Padding(4, 0, 4, 0);
                _btnPF.Margin = new Padding(4, 0, 4, 0);
                _btnShell.Margin = new Padding(4, 0, 4, 0);
                _btnTail.Margin = new Padding(4, 0, 4, 0);
                _btnDelete.Margin = new Padding(4, 0, 4, 0);

                _topPanel.Controls.Add(_btnRefresh, 4, 0);
                _topPanel.Controls.Add(_btnYaml, 5, 0);
                _topPanel.Controls.Add(_btnDescribe, 6, 0);
                _topPanel.Controls.Add(_btnApply, 7, 0);
                _topPanel.Controls.Add(_btnPF, 8, 0);
                _topPanel.Controls.Add(_btnShell, 9, 0);
                _topPanel.Controls.Add(_btnTail, 10, 0);
                _topPanel.Controls.Add(_btnDelete, 11, 0);

                // Row 0: Management buttons (Templates, Favorites, Users, Settings, Audit Logs)
                _btnTemplates.Margin = new Padding(4, 0, 4, 0);
                _btnFavorites.Margin = new Padding(4, 0, 4, 0);
                _btnUsers.Margin = new Padding(4, 0, 4, 0);
                _btnSettings.Margin = new Padding(4, 0, 4, 0);
                _btnAuditLogs.Margin = new Padding(4, 0, 0, 0);

                _topPanel.Controls.Add(_btnTemplates, 12, 0);
                _topPanel.Controls.Add(_btnFavorites, 13, 0);
                _topPanel.Controls.Add(_btnUsers, 14, 0);
                _topPanel.Controls.Add(_btnSettings, 15, 0);
                _topPanel.Controls.Add(_btnAuditLogs, 16, 0);

                // Row 1: Help and Theme buttons
                var btnHelp = new Button()
                {
                    Text = "❓ Help",
                    AutoSize = false,
                    Size = new Size(90, 38),
                    BackColor = Color.FromArgb(83, 101, 201),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                    Cursor = Cursors.Hand,
                    Margin = new Padding(4, 0, 4, 0)
                };
                btnHelp.FlatAppearance.BorderSize = 0;
                _tooltip.SetToolTip(btnHelp, "View keyboard shortcuts (F1)");
                btnHelp.Click += (s, e) => ShowKeyboardShortcuts();

                _btnTheme.Margin = new Padding(4, 0, 0, 0);

                _topPanel.Controls.Add(btnHelp, 4, 1);
                _topPanel.Controls.Add(_btnTheme, 5, 1);

                // Configure split panel (controls already added by InitializeComponent)
                _split.Margin = new Padding(0, 5, 0, 0); // Add top margin to separate from toolbar
                _split.Panel1.BackColor = Color.White;
                _split.Panel2.BackColor = Color.FromArgb(250, 250, 250);
                _split.SplitterDistance = 280;
                _split.SplitterWidth = 3;

                _status.Items.AddRange(new ToolStripItem[] { _statusInfo, _statusNs, _statusKind });
                _status.BackColor = Color.FromArgb(245, 245, 245);
                _status.Font = new Font("Segoe UI", 9f);

                // Set docking (controls already added by InitializeComponent)
                _topPanel.Dock = DockStyle.Top;
                _split.Dock = DockStyle.Fill;
                _status.Dock = DockStyle.Bottom;

                // Enhanced grid styling
                _grid.BackgroundColor = Color.White;
                _grid.BorderStyle = BorderStyle.None;
                _grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
                _grid.GridColor = Color.FromArgb(240, 240, 240);
                _grid.RowTemplate.Height = 32;
                _grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(252, 252, 252);
                _grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(63, 81, 181);
                _grid.DefaultCellStyle.SelectionForeColor = Color.White;

                _gridEvents.BackgroundColor = Color.White;
                _gridEvents.BorderStyle = BorderStyle.None;
                _gridEvents.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
                _gridEvents.GridColor = Color.FromArgb(240, 240, 240);
                _gridEvents.RowTemplate.Height = 32;
                _gridEvents.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(252, 252, 252);

                _rtYaml.BackColor = Color.FromArgb(43, 43, 43);
                _rtYaml.ForeColor = Color.FromArgb(212, 212, 212);
                _rtYaml.Font = new Font("Cascadia Code", 10f);

                _txtLogs.BackColor = Color.FromArgb(30, 30, 30);
                _txtLogs.ForeColor = Color.FromArgb(0, 255, 127);
                _txtLogs.Font = new Font("Cascadia Code", 9.5f);

                foreach (var g in new[] { _grid, _gridEvents })
                {
                    g.EnableHeadersVisualStyles = false;
                    g.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(63, 81, 181);
                    g.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                    g.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 9.5f);
                    g.ColumnHeadersDefaultCellStyle.Padding = new Padding(8, 8, 8, 8);
                    g.ColumnHeadersHeight = 40;
                    g.DefaultCellStyle.Font = new Font("Segoe UI", 9.5f);
                    g.DefaultCellStyle.Padding = new Padding(8, 4, 8, 4);
                }

                // Add row formatting based on status
                _grid.CellFormatting += OnGridCellFormatting;
                _grid.CellDoubleClick += async (s, e) => await SafeExecuteAsync(OnGridDoubleClickAsync(e)).ConfigureAwait(false);

                _btnRefresh.Click += async (s, e) => await SafeExecuteAsync(ReloadAsync()).ConfigureAwait(false);
                _cbContext.SelectedIndexChanged += async (s, e) => await SafeExecuteAsync(OnContextChangedAsync()).ConfigureAwait(false);
                _cbNamespace.SelectedIndexChanged += async (s, e) => await SafeExecuteAsync(ReloadAsync()).ConfigureAwait(false);
                _explorer.SelectedIndexChanged += async (s, e) => await SafeExecuteAsync(LoadSelectedAsync()).ConfigureAwait(false);  // Changed from Click to SelectedIndexChanged
                _txtSearch.KeyDown += async (s, e) => { if (e.KeyCode == Keys.Enter) await SafeExecuteAsync(LoadSelectedAsync()).ConfigureAwait(false); };
                _btnYaml.Click += async (s, e) => await SafeExecuteAsync(LoadYamlAsync()).ConfigureAwait(false);
                _btnDescribe.Click += async (s, e) => await SafeExecuteAsync(DescribeAsync()).ConfigureAwait(false);
                _btnTail.Click += async (s, e) => await SafeExecuteAsync(TailLogsAsync()).ConfigureAwait(false);
                
                // Hide unused buttons on main toolbar (can access via context menu/dialogs)
                _btnApply.Visible = false;
                _btnPF.Visible = false;
                _btnShell.Visible = false;
                _btnDelete.Visible = false;
                
                _btnTheme.Click += (s, e) => ToggleTheme();

                // SQL Features event handlers
                _btnTemplates.Click += (s, e) => ShowTemplateManager();
                _btnFavorites.Click += (s, e) => ShowFavorites();
                _btnUsers.Click += (s, e) => ShowUserManager();
                _btnSettings.Click += (s, e) => ShowSettings();
                _btnAuditLogs.Click += (s, e) => ShowAuditLogs();

                // Setup tooltips for better UX
                _tooltip.SetToolTip(_cbContext, "Switch between Kubernetes contexts (clusters)");
                _tooltip.SetToolTip(_cbNamespace, "Select namespace to view resources");
                _tooltip.SetToolTip(_txtSearch, "Filter resources by name (press Enter to apply)");
                _tooltip.SetToolTip(_btnRefresh, "Reload current view (F5)");
                _tooltip.SetToolTip(_btnYaml, "View resource as YAML (Ctrl+Y)");
                _tooltip.SetToolTip(_btnDescribe, "Show detailed resource description (Ctrl+D)");
                _tooltip.SetToolTip(_btnTail, "View pod logs (Ctrl+L)");
                _tooltip.SetToolTip(_btnApply, "Apply YAML configuration (Ctrl+K)");
                _tooltip.SetToolTip(_btnPF, "Setup port forwarding to pod (Ctrl+P)");
                _tooltip.SetToolTip(_btnShell, "Open shell session in pod (Ctrl+S)");
                _tooltip.SetToolTip(_btnDelete, "Delete selected resource (Delete key)");
                _tooltip.SetToolTip(_btnTheme, "Toggle dark/light theme");

                // SQL Features tooltips
                _tooltip.SetToolTip(_btnTemplates, "Manage YAML templates library");
                _tooltip.SetToolTip(_btnFavorites, "View bookmarked resources");
                _tooltip.SetToolTip(_btnSettings, "Application settings");
                _tooltip.SetToolTip(_btnAuditLogs, "View audit logs and history");

                _tooltip.SetToolTip(_grid, "Double-click row for details, Right-click for more actions");
                _tooltip.SetToolTip(_explorer, "Click to load resource type");

                // Setup permissions
                UpdateButtonPermissions();

                // Keyboard shortcuts
                KeyPreview = true;
                KeyDown += OnKeyDown;

                // Add context menu for grid (dynamically populated based on resource type)
                var contextMenu = new ContextMenuStrip();
                contextMenu.Opening += (s, e) => BuildContextMenu(contextMenu);
                _grid.ContextMenuStrip = contextMenu;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error initializing MainViewLens:\n{ex.Message}\n\n{ex.StackTrace}",
                    "Initialization Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }

        private void PopulateExplorer()
        {
            _explorer.BeginUpdate();
            _explorer.Items.Clear();

            // Workloads category
            AddCategory("⚙️ WORKLOADS");
            AddResourceKind("📦 Pods");
            AddResourceKind("🚀 Deployments");
            AddResourceKind("💾 StatefulSets");
            AddResourceKind("🔄 DaemonSets");
            AddResourceKind("📋 ReplicaSets");
            AddResourceKind("⏱️ Jobs");
            AddResourceKind("🕐 CronJobs");

            // Configuration category
            AddCategory("⚙️ CONFIGURATION");
            AddResourceKind("📝 ConfigMaps");
            AddResourceKind("🔐 Secrets");

            // Network category
            AddCategory("🌐 NETWORK");
            AddResourceKind("🔌 Services");
            AddResourceKind("🌍 Ingresses");
            AddResourceKind("🛡️ NetworkPolicies");

            // Storage category
            AddCategory("💿 STORAGE");
            AddResourceKind("💽 PersistentVolumes");
            AddResourceKind("📁 PersistentVolumeClaims");

            // Access Control category
            AddCategory("🔒 ACCESS CONTROL");
            AddResourceKind("👤 ServiceAccounts");
            AddResourceKind("🎭 Roles");
            AddResourceKind("🔗 RoleBindings");
            AddResourceKind("👑 ClusterRoles");
            AddResourceKind("⛓️ ClusterRoleBindings");

            // Cluster category
            AddCategory("🖥️ CLUSTER");
            AddResourceKind("🖧 Nodes");
            AddResourceKind("📂 Namespaces");
            AddResourceKind("📢 Events");

            _explorer.EndUpdate();
        }

        private void AddCategory(string category)
        {
            var item = new ListViewItem(new[] { category, "" })
            {
                Font = new Font("Segoe UI Semibold", 9f, FontStyle.Bold),
                ForeColor = Color.FromArgb(100, 100, 100),
                BackColor = Color.FromArgb(245, 245, 245),
                UseItemStyleForSubItems = true
            };
            _explorer.Items.Add(item);
        }

        private void AddResourceKind(string kind)
        {
            var item = new ListViewItem(new[] { " " + kind, "" })
            {
                Font = new Font("Segoe UI", 9f),
                ForeColor = Color.FromArgb(60, 60, 60)
            };
            _explorer.Items.Add(item);
        }

        public void BindNamespaces(IEnumerable<string> namespaces)
        {
            if (InvokeRequired)
            {
                Invoke(() => BindNamespaces(namespaces));
                return;
            }

            _isBinding = true;  // Disable event handling

            _cbNamespace.BeginUpdate();
            _cbNamespace.Items.Clear();
            foreach (var ns in namespaces) _cbNamespace.Items.Add(ns);
            _cbNamespace.EndUpdate();
            if (_cbNamespace.Items.Count > 0 && _cbNamespace.SelectedIndex < 0)
                _cbNamespace.SelectedIndex = 0;

            _isBinding = false;  // Re-enable event handling
        }

        public void BindContexts(IEnumerable<string> contexts, string? currentContext = null)
        {
            if (InvokeRequired)
            {
                Invoke(() => BindContexts(contexts, currentContext));
                return;
            }

            _isBinding = true;  // Disable event handling

            _cbContext.BeginUpdate();
            _cbContext.Items.Clear();
            foreach (var ctx in contexts) _cbContext.Items.Add(ctx);
            _cbContext.EndUpdate();

            if (!string.IsNullOrEmpty(currentContext) && _cbContext.Items.Contains(currentContext))
            {
                _cbContext.SelectedItem = currentContext;
            }
            else if (_cbContext.Items.Count > 0 && _cbContext.SelectedIndex < 0)
            {
                _cbContext.SelectedIndex = 0;
            }

            _isBinding = false;  // Re-enable event handling
        }

        public void BindExplorer(string kind, IEnumerable<string> names)
        {
            // Update count for the specific kind
            foreach (ListViewItem item in _explorer.Items)
            {
                var itemKind = item.SubItems[0].Text.Trim();
                if (itemKind == kind)
                {
                    item.SubItems[1].Text = names.Count().ToString();
                    break;
                }
            }
        }

        public void UpdateExplorerCounts(Dictionary<string, int> counts)
        {
            if (InvokeRequired)
            {
                Invoke(() => UpdateExplorerCounts(counts));
                return;
            }

            _explorer.BeginUpdate();
            try
            {
                foreach (ListViewItem item in _explorer.Items)
                {
                    var text = item.SubItems[0].Text.Trim();
                    // Remove emoji and extra spaces
                    var kind = System.Text.RegularExpressions.Regex.Replace(text, @"[\p{So}\p{Sk}]", "").Trim();

                    if (counts.ContainsKey(kind))
                    {
                        // Only update if changed to avoid flicker
                        var newCount = counts[kind].ToString();
                        if (item.SubItems[1].Text != newCount)
                        {
                            item.SubItems[1].Text = newCount;
                            item.SubItems[1].ForeColor = Color.FromArgb(100, 100, 100);
                        }
                    }
                }
            }
            finally
            {
                _explorer.EndUpdate();
            }
        }

        public void BindList<T>(IEnumerable<T> items)
        {
            if (InvokeRequired)
            {
                Invoke(() => BindList(items));
                return;
            }

            var logFile = System.IO.Path.Combine(
                System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) ?? ".",
                "error.log");

            try
            {
                _grid.SuspendLayout();
                _btnRefresh.Enabled = false;
                _statusInfo.Text = "Loading data...";

                var list = items.ToList();
                System.IO.File.AppendAllText(logFile, $"BindList: Received {list.Count} items\n");

                // Apply search filter
                var searchText = _txtSearch.Text?.Trim();
                if (!string.IsNullOrEmpty(searchText))
                {
                    list = list.Where(item =>
                    {
                        var nameProperty = item?.GetType().GetProperty("Name");
                        if (nameProperty != null)
                        {
                            var nameValue = nameProperty.GetValue(item)?.ToString();
                            return nameValue?.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0;
                        }
                        return false;
                    }).ToList();
                    System.IO.File.AppendAllText(logFile, $"BindList: After filter {list.Count} items\n");
                }

                // Clear and rebind data source
                _grid.DataSource = null;
                _grid.DataSource = list;

                System.IO.File.AppendAllText(logFile, $"BindList: Grid rows count = {_grid.Rows.Count}\n");

                // Show empty state message if no data
                if (list.Count == 0)
                {
                    _statusInfo.Text = "ℹ️ No resources found";
                    _statusInfo.ForeColor = Color.FromArgb(255, 152, 0); // Orange
                }
                else
                {
                    _statusInfo.ForeColor = Color.FromArgb(0, 128, 0); // Green
                }

                // Auto-size columns for better visibility
                if (_grid.Columns.Count > 0)
                {
                    System.IO.File.AppendAllText(logFile, $"BindList: Grid columns count = {_grid.Columns.Count}\n");
                    
                    foreach (DataGridViewColumn col in _grid.Columns)
                    {
                        // Set specific widths for common columns
                        if (col.Name == "Name" || col.HeaderText == "Name")
                        {
                            col.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                            col.Width = 250;  // Wide enough for long names
                        }
                        else if (col.Name == "Namespace" || col.HeaderText == "Namespace")
                        {
                            col.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                            col.Width = 180;  // Wide enough for long namespaces
                        }
                        else if (col.Name == "Context" || col.HeaderText == "Context")
                        {
                            col.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                            col.Width = 200;  // Wide enough for context names
                        }
                        else if (col.Name == "Status" || col.HeaderText == "Status")
                        {
                            col.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                            col.Width = 120;
                        }
                        else if (col.Name == "Ready" || col.HeaderText == "Ready" || 
                                 col.Name == "Restarts" || col.HeaderText == "Restarts")
                        {
                            col.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                            col.Width = 80;
                        }
                        else if (col.Name == "Age" || col.HeaderText == "Age")
                        {
                            col.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                            col.Width = 100;
                        }
                        else
                        {
                            col.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                            col.MinimumWidth = 100;
                        }
                    }
                    
                    // Last column fill remaining space if it's a dynamic column
                    if (_grid.Columns.Count > 0)
                    {
                        var lastCol = _grid.Columns[_grid.Columns.Count - 1];
                        if (lastCol.AutoSizeMode == DataGridViewAutoSizeColumnMode.AllCells)
                        {
                            lastCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        }
                    }
                }

                _tabs.SelectedTab = _tabList;

                // Update status with color
                if (list.Count == 0)
                {
                    _statusInfo.Text = "ℹ️ No resources found";
                    _statusInfo.ForeColor = Color.FromArgb(255, 152, 0); // Orange
                }
                else
                {
                    _statusInfo.Text = $"✅ {_grid.Rows.Count} items loaded";
                    _statusInfo.ForeColor = Color.FromArgb(0, 128, 0); // Green
                }

                System.IO.File.AppendAllText(logFile, $"BindList: COMPLETED - Status: {_statusInfo.Text}\n");
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText(logFile, $"BindList ERROR: {ex.Message}\n{ex.StackTrace}\n");
                _statusInfo.Text = $"❌ Error loading data: {ex.Message}";
                _statusInfo.ForeColor = Color.Red;
                MessageBox.Show(this,
                    $"Failed to load data:\n\n{ex.Message}\n\nCheck error.log for details.",
                    "Error Loading Data",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                _grid.ResumeLayout();
                _btnRefresh.Enabled = true;
            }
        }

        public void ShowYaml(string yaml)
        {
            if (InvokeRequired)
            {
                Invoke(() => ShowYaml(yaml));
                return;
            }

            _rtYaml.Text = yaml;
            _tabs.SelectedTab = _tabYaml;
        }

        public void BindEvents(IEnumerable<object> rows)
        {
            if (InvokeRequired)
            {
                Invoke(() => BindEvents(rows));
                return;
            }

            _gridEvents.DataSource = rows.ToList();
            _tabs.SelectedTab = _tabEvents;
        }

        public void AppendLog(string line) => _txtLogs.AppendText(line + Environment.NewLine);

        public string CurrentNamespace => _cbNamespace.SelectedItem?.ToString() ?? "default";

        /// <summary>
        /// Shows a status message with color coding
        /// </summary>
        private void ShowStatus(string message, StatusType type = StatusType.Info)
        {
            if (InvokeRequired)
            {
                Invoke(() => ShowStatus(message, type));
                return;
            }

            _statusInfo.Text = message;
            _statusInfo.ForeColor = type switch
            {
                StatusType.Success => Color.FromArgb(0, 128, 0),     // Green
                StatusType.Warning => Color.FromArgb(255, 152, 0),   // Orange
                StatusType.Error => Color.Red,
                _ => Color.Black
            };
        }

        private enum StatusType
        {
            Info,
            Success,
            Warning,
            Error
        }

        public string? SelectedResourceKind()
        {
            if (_explorer.SelectedItems.Count == 0) return null;
            var text = _explorer.SelectedItems[0].SubItems[0].Text.Trim();

            // Log for debugging
            var logFile = System.IO.Path.Combine(
                System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) ?? ".",
                "error.log");
            System.IO.File.AppendAllText(logFile, $"SelectedResourceKind: raw text = '{text}'\n");

            // Remove emoji and extra spaces
            var kind = System.Text.RegularExpressions.Regex.Replace(text, @"[\u00A0-\u3300\uD83C-\uDBFF\uDC00-\uDFFF]+", "").Trim();

            System.IO.File.AppendAllText(logFile, $"SelectedResourceKind: after emoji removal = '{kind}'\n");

            // Filter out categories - they are ALL UPPERCASE like "WORKLOADS", "CONFIGURATION", etc.
            if (string.IsNullOrWhiteSpace(kind)) return null;
            if (kind.Length < 3) return null;

            // Category detection: all uppercase AND all letters
            bool isAllUppercase = kind == kind.ToUpper();
            bool isAllLetters = kind.All(c => char.IsLetter(c) || char.IsWhiteSpace(c));
            if (isAllUppercase && isAllLetters)
            {
                System.IO.File.AppendAllText(logFile, $"SelectedResourceKind: '{kind}' is a category, returning null\n");
                return null;  // This is a category header
            }

            System.IO.File.AppendAllText(logFile, $"SelectedResourceKind: returning '{kind}'\n");
            return kind;
        }

        // Implement IExplorerViewController interface
        public string CurrentResourceKind => SelectedResourceKind() ?? "Pods";

        public void UpdateResourceCounts(Dictionary<string, int> counts)
        {
            UpdateExplorerCounts(counts);
        }

        public void BindResourceList<T>(IEnumerable<T> resources)
        {
            BindList(resources);
        }

        public void ShowStatusMessage(string message, Controllers.StatusType type)
        {
            if (InvokeRequired)
            {
                Invoke(() => ShowStatusMessage(message, type));
                return;
            }

            _statusInfo.Text = message;

            // Set color based on type
            _statusInfo.ForeColor = type switch
            {
                Controllers.StatusType.Success => Color.Green,
                Controllers.StatusType.Warning => Color.Orange,
                Controllers.StatusType.Error => Color.Red,
                Controllers.StatusType.Loading => Color.Blue,
                _ => Color.Black
            };
        }

        public void ShowError(string title, string message)
        {
            MessageBox.Show(this, message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public void ShowSuccess(string title, string message)
        {
            MessageBox.Show(this, message, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public void ShowWarning(string title, string message)
        {
            MessageBox.Show(this, message, title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public bool ShowConfirmation(string title, string message)
        {
            var result = MessageBox.Show(this, message, title, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            return result == DialogResult.Yes;
        }

        public int? ShowScaleDialog(string resourceName, int currentReplicas)
        {
            var dialog = new ScaleDialog(resourceName, currentReplicas);
            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                return dialog.Replicas;
            }
            return null;
        }

        public (string kind, string name)? SelectedExplorer()
        {
            // This is kept for backward compatibility but now we'll use selected grid row for the name
            if (_grid.CurrentRow == null) return null;
            var kind = SelectedResourceKind();
            if (kind == null) return null;

            var nameCell = _grid.CurrentRow.Cells["Name"];
            if (nameCell?.Value == null) return null;

            return (kind, nameCell.Value.ToString() ?? "");
        }

        private void ToggleTheme()
        {
            try
            {
                _isDarkMode = !_isDarkMode;
                var skin = MaterialSkinManager.Instance;

                if (_isDarkMode)
                {
                    skin.Theme = MaterialSkinManager.Themes.DARK;
                    skin.ColorScheme = new ColorScheme(
                        Primary.BlueGrey800,
                        Primary.BlueGrey900,
                        Primary.BlueGrey500,
                        Accent.LightBlue200,
                        TextShade.WHITE
                    );
                    _btnTheme.Text = "☀️ Light";
                    ShowStatus("🌙 Dark mode enabled", StatusType.Info);
                    
                    // Apply dark theme to non-Material controls
                    ApplyDarkToCustomControls();
                }
                else
                {
                    skin.Theme = MaterialSkinManager.Themes.LIGHT;
                    skin.ColorScheme = new ColorScheme(
                        Primary.Indigo600,
                        Primary.Indigo800,
                        Primary.Indigo200,
                        Accent.Cyan200,
                        TextShade.WHITE
                    );
                    _btnTheme.Text = "🌙 Dark";
                    ShowStatus("☀️ Light mode enabled", StatusType.Info);
                    
                    // Apply light theme to non-Material controls
                    ApplyLightToCustomControls();
                }

                // Save theme preference
                if (SettingsService != null)
                {
                    _ = SettingsService.UpsertSettingAsync("App.Theme", _isDarkMode ? "Dark" : "Light");
                }

                Invalidate(true);
                Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this,
                    $"Failed to toggle theme:\n\n{ex.Message}",
                    "Theme Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void ApplyDarkToCustomControls()
        {
            var darkBg = Color.FromArgb(35, 38, 45);
            var darkText = Color.FromArgb(240, 240, 240);
            
            // DataGridViews
            ApplyDarkToGrid(_grid, darkBg, darkText);
            ApplyDarkToGrid(_gridEvents, darkBg, darkText);
            
            // TextBoxes
            _rtYaml.BackColor = darkBg;
            _rtYaml.ForeColor = darkText;
            _txtLogs.BackColor = darkBg;
            _txtLogs.ForeColor = darkText;
            
            // Tabs
            foreach (TabPage tab in _tabs.TabPages)
            {
                tab.BackColor = Color.FromArgb(26, 28, 34);
                tab.ForeColor = darkText;
            }
        }

        private void ApplyLightToCustomControls()
        {
            var lightBg = Color.White;
            var lightText = Color.FromArgb(33, 33, 33);
            
            // DataGridViews
            ApplyLightToGrid(_grid, lightBg, lightText);
            ApplyLightToGrid(_gridEvents, lightBg, lightText);
            
            // TextBoxes
            _rtYaml.BackColor = lightBg;
            _rtYaml.ForeColor = lightText;
            _txtLogs.BackColor = lightBg;
            _txtLogs.ForeColor = lightText;
            
            // Tabs
            foreach (TabPage tab in _tabs.TabPages)
            {
                tab.BackColor = lightBg;
                tab.ForeColor = lightText;
            }
        }

        private void ApplyDarkToGrid(DataGridView grid, Color bgColor, Color textColor)
        {
            grid.BackgroundColor = bgColor;
            grid.DefaultCellStyle.BackColor = bgColor;
            grid.DefaultCellStyle.ForeColor = textColor;
            grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(60, 110, 220);
            grid.DefaultCellStyle.SelectionForeColor = Color.White;
            grid.ColumnHeadersDefaultCellStyle.BackColor = bgColor;
            grid.ColumnHeadersDefaultCellStyle.ForeColor = textColor;
            grid.GridColor = Color.FromArgb(60, 60, 70);
            grid.EnableHeadersVisualStyles = false;
        }

        private void ApplyLightToGrid(DataGridView grid, Color bgColor, Color textColor)
        {
            grid.BackgroundColor = bgColor;
            grid.DefaultCellStyle.BackColor = bgColor;
            grid.DefaultCellStyle.ForeColor = textColor;
            grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(51, 103, 214);
            grid.DefaultCellStyle.SelectionForeColor = Color.White;
            grid.ColumnHeadersDefaultCellStyle.BackColor = Color.WhiteSmoke;
            grid.ColumnHeadersDefaultCellStyle.ForeColor = textColor;
            grid.GridColor = Color.FromArgb(230, 230, 230);
            grid.EnableHeadersVisualStyles = false;
        }

        private void OnGridCellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.CellStyle == null) return;

            if (_grid.Columns[e.ColumnIndex].Name == "Phase" ||
                _grid.Columns[e.ColumnIndex].Name == "Status")
            {
                var value = e.Value?.ToString();
                if (value != null)
                {
                    // Color code based on status/phase
                    if (value.Contains("Running") || value.Contains("Bound") || value.Contains("Active") || value.Contains("True"))
                    {
                        e.CellStyle.ForeColor = Color.Green;
                        e.CellStyle.Font = new Font(e.CellStyle.Font ?? _grid.Font, FontStyle.Bold);
                    }
                    else if (value.Contains("Pending") || value.Contains("Creating"))
                    {
                        e.CellStyle.ForeColor = Color.Orange;
                    }
                    else if (value.Contains("Failed") || value.Contains("Error") || value.Contains("CrashLoop") || value.Contains("False"))
                    {
                        e.CellStyle.ForeColor = Color.Red;
                        e.CellStyle.Font = new Font(e.CellStyle.Font ?? _grid.Font, FontStyle.Bold);
                    }
                    else if (value.Contains("Succeeded") || value.Contains("Completed"))
                    {
                        e.CellStyle.ForeColor = Color.Blue;
                    }
                }
            }
            else if (_grid.Columns[e.ColumnIndex].Name == "Ready")
            {
                var value = e.Value?.ToString();
                if (value != null && value.Contains("/"))
                {
                    var parts = value.Split('/');
                    if (parts.Length == 2 && int.TryParse(parts[0], out var ready) && int.TryParse(parts[1], out var total))
                    {
                        if (ready == total && ready > 0)
                        {
                            e.CellStyle.ForeColor = Color.Green;
                            e.CellStyle.Font = new Font(e.CellStyle.Font ?? _grid.Font, FontStyle.Bold);
                        }
                        else if (ready == 0)
                        {
                            e.CellStyle.ForeColor = Color.Red;
                        }
                        else
                        {
                            e.CellStyle.ForeColor = Color.Orange;
                        }
                    }
                }
            }
            else if (_grid.Columns[e.ColumnIndex].Name == "Type" && e.Value?.ToString() == "Warning")
            {
                e.CellStyle.ForeColor = Color.Orange;
                e.CellStyle.Font = new Font(e.CellStyle.Font ?? _grid.Font, FontStyle.Bold);
            }
        }

        private async System.Threading.Tasks.Task ReloadAsync()
        {
            var logFile = System.IO.Path.Combine(
                System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) ?? ".",
                "error.log");

            System.IO.File.AppendAllText(logFile, $"ReloadAsync CALLED: _isBinding={_isBinding}, Controller={Controller != null}, _isLoading={_isLoading}\n");

            if (_isBinding || Controller == null || _isLoading) return;  // Skip if binding or already loading

            try
            {
                _isLoading = true;
                _statusInfo.Text = "🔄 Refreshing...";
                _statusInfo.ForeColor = System.Drawing.Color.FromArgb(255, 152, 0); // Orange

                System.IO.File.AppendAllText(logFile, $"ReloadAsync: Namespace = '{CurrentNamespace}'\n");

                // Load resource counts
                await Controller!.LoadResourceCountsAsync(CurrentNamespace);

                System.IO.File.AppendAllText(logFile, "ReloadAsync: Counts loaded, now loading current resource...\n");

                // Reload current selected resource
                var kind = SelectedResourceKind();
                if (kind != null)
                {
                    System.IO.File.AppendAllText(logFile, $"ReloadAsync: Loading {kind}...\n");
                    await Controller!.LoadResourceListAsync(CurrentNamespace, kind);
                    _statusKind.Text = "kind: " + kind;
                }
                else
                {
                    // If no resource selected, auto-load Pods
                    System.IO.File.AppendAllText(logFile, "ReloadAsync: No resource selected, auto-loading Pods...\n");
                    await Controller.LoadResourceListAsync(CurrentNamespace, "Pods");
                    _statusKind.Text = "kind: Pods";
                }

                System.IO.File.AppendAllText(logFile, "ReloadAsync: Completed successfully\n");
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText(logFile, $"ReloadAsync ERROR: {ex.Message}\n");
            }
            finally
            {
                _isLoading = false;
            }
        }

        private async System.Threading.Tasks.Task OnContextChangedAsync()
        {
            if (_isBinding || Controller == null) return;  // Skip if binding
            var selectedContext = _cbContext.SelectedItem?.ToString();
            if (!string.IsNullOrEmpty(selectedContext))
            {
                await Controller!.SwitchContextAsync(selectedContext);
                await Controller!.InitializeAsync();
            }
        }

        private async System.Threading.Tasks.Task LoadSelectedAsync()
        {
            var logFile = System.IO.Path.Combine(
                System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) ?? ".",
                "error.log");

            System.IO.File.AppendAllText(logFile, $"LoadSelectedAsync ENTRY: Controller={Controller != null}, _isLoading={_isLoading}\n");

            if (Controller == null)
            {
                System.IO.File.AppendAllText(logFile, "LoadSelectedAsync: Controller is null, returning\n");
                return;
            }

            if (_isLoading)
            {
                System.IO.File.AppendAllText(logFile, "LoadSelectedAsync: Already loading, returning\n");
                return;
            }

            var kind = SelectedResourceKind();
            System.IO.File.AppendAllText(logFile, $"LoadSelectedAsync: kind = '{kind}'\n");

            if (kind != null)
            {
                try
                {
                    _isLoading = true;
                    System.IO.File.AppendAllText(logFile, $"LoadSelectedAsync: Loading {kind}...\n");

                    // Show loading indicator
                    _statusKind.Text = "kind: " + kind;
                    _statusInfo.Text = "⌛ Loading...";
                    _statusInfo.ForeColor = System.Drawing.Color.FromArgb(255, 152, 0); // Orange

                    // Cancel any pending load
                    _loadCancellation?.Cancel();
                    _loadCancellation = new System.Threading.CancellationTokenSource();

                    await Controller.LoadResourceListAsync(CurrentNamespace, kind);

                    System.IO.File.AppendAllText(logFile, $"LoadSelectedAsync: Loaded {kind} successfully\n");
                }
                catch (Exception ex)
                {
                    System.IO.File.AppendAllText(logFile, $"LoadSelectedAsync ERROR: {ex.Message}\n");
                }
                finally
                {
                    _isLoading = false;
                }
            }
        }

        private async System.Threading.Tasks.Task LoadYamlAsync()
        {
            if (Controller == null) return;
            if (SelectedExplorer() is { } s)
                await Controller.LoadYamlAsync(CurrentNamespace, s.kind, s.name);
        }

        private async System.Threading.Tasks.Task DescribeAsync()
        {
            if (Controller == null) return;
            if (SelectedExplorer() is { } s)
                await Controller.DescribeAsync(CurrentNamespace, s.kind, s.name);
        }

        private async System.Threading.Tasks.Task TailLogsAsync()
        {
            if (Controller == null) return;
            if (SelectedExplorer() is { } s && s.kind == "Pods")
                await Controller.TailLogsAsync(CurrentNamespace, s.name);
            else
                MessageBox.Show(this, "Chọn một Pod để tail logs.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ShowApplyYaml()
        {
            try
            {
                var dlg = new ApplyYamlDialog(K8sExplorerService, AuditService);
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    ShowStatus("✅ YAML applied successfully", StatusType.Success);
                    // Reload current view to show changes
                    _ = SafeExecuteAsync(ReloadAsync());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this,
                    $"Failed to open Apply YAML dialog:\n\n{ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void ShowPortForward()
        {
            // Get selected pod info if available
            string? podName = null;
            string? namespaceName = CurrentNamespace;
            
            if (SelectedExplorer() is { } s && s.kind == "Pods")
            {
                podName = s.name;
            }
            
            using var dlg = new PortForwardManager(namespaceName, podName);
            dlg.ShowDialog(this);
        }

        private void ShowShell()
        {
            if (SelectedExplorer() is { } s && s.kind == "Pods")
            {
                if (ShellService == null)
                {
                    MessageBox.Show(this,
                        "Shell service is not initialized.\n\nPlease restart the application.",
                        "Service Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                try
                {
                    var dlg = new TerminalDialog(ShellService, CurrentNamespace, s.name);
                    dlg.Show();  // Use Show() instead of ShowDialog() to allow non-blocking
                    ShowStatus($"💻 Opened shell to pod '{s.name}'", StatusType.Success);

                    // Log audit
                    if (AuditService != null && !string.IsNullOrEmpty(SessionToken))
                    {
                        _ = AuditService.LogAsync(
                            userId: 1,
                            action: "Shell",
                            resourceKind: "Pod",
                            resourceName: s.name,
                            resourceNamespace: CurrentNamespace,
                            success: true);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this,
                        $"Failed to open shell to pod '{s.name}':\n\n{ex.Message}",
                        "Shell Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show(this, "Please select a Pod to open shell.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private async System.Threading.Tasks.Task DeleteResourceAsync()
        {
            if (Controller == null) return;
            if (SelectedExplorer() is { } s)
            {
                var result = MessageBox.Show(this,
                    $"Are you sure you want to delete {s.kind} '{s.name}' in namespace '{CurrentNamespace}'?\n\n⚠️ This action cannot be undone!",
                    "Confirm Delete",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button2);  // Default to No

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        ShowStatus($"🗑️ Deleting {s.kind} '{s.name}'...", StatusType.Warning);
                        await Controller.DeleteResourceAsync(CurrentNamespace, s.kind, s.name);
                        ShowStatus($"✅ {s.kind} '{s.name}' deleted successfully", StatusType.Success);

                        // Log successful delete
                        if (AuditService != null)
                        {
                            _ = AuditService.LogAsync(
                                userId: 1,
                                action: "Delete",
                                resourceKind: s.kind,
                                resourceName: s.name,
                                resourceNamespace: CurrentNamespace,
                                success: true);
                        }

                        await ReloadAsync().ConfigureAwait(false);
                    }
                    catch (Exception ex)
                    {
                        ShowStatus($"❌ Failed to delete {s.kind}", StatusType.Error);

                        // Log failed delete
                        if (AuditService != null)
                        {
                            _ = AuditService.LogAsync(
                                userId: 1,
                                action: "Delete",
                                resourceKind: s.kind,
                                resourceName: s.name,
                                resourceNamespace: CurrentNamespace,
                                success: false,
                                errorMessage: ex.Message);
                        }

                        MessageBox.Show(this,
                            $"Failed to delete {s.kind} '{s.name}':\n\n{ex.Message}",
                            "Delete Failed",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show(this, "Please select a resource to delete.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BuildContextMenu(ContextMenuStrip menu)
        {
            menu.Items.Clear();

            if (SelectedExplorer() is { } s)
            {
                var isWorkload = s.kind is "Deployments" or "StatefulSets" or "DaemonSets";
                var isPod = s.kind == "Pods";
                var isDeployment = s.kind == "Deployments";
                var isStatefulSet = s.kind == "StatefulSets";
                var isDaemonSet = s.kind == "DaemonSets";

                // Common actions
                menu.Items.Add("View YAML", null, async (sender, e) => await SafeExecuteAsync(LoadYamlAsync()).ConfigureAwait(false));
                menu.Items.Add("Describe", null, async (sender, e) => await SafeExecuteAsync(DescribeAsync()).ConfigureAwait(false));

                // Workload-specific actions
                if (isWorkload)
                {
                    menu.Items.Add(new ToolStripSeparator());

                    // View Details for workloads
                    if (isDeployment)
                    {
                        menu.Items.Add("🔍 View Details", null, async (sender, e) =>
                            await SafeExecuteAsync(Controller?.ShowDeploymentDetailsAsync(CurrentNamespace, s.name) ?? System.Threading.Tasks.Task.CompletedTask));
                    }
                    else if (isStatefulSet)
                    {
                        menu.Items.Add("🔍 View Details", null, async (sender, e) =>
                            await SafeExecuteAsync(Controller?.ShowStatefulSetDetailsAsync(CurrentNamespace, s.name) ?? System.Threading.Tasks.Task.CompletedTask));
                    }
                    else if (isDaemonSet)
                    {
                        menu.Items.Add("🔍 View Details", null, async (sender, e) =>
                            await SafeExecuteAsync(Controller?.ShowDaemonSetDetailsAsync(CurrentNamespace, s.name) ?? System.Threading.Tasks.Task.CompletedTask));
                    }

                    if (s.kind != "DaemonSets")  // DaemonSets can't be scaled
                    {
                        menu.Items.Add("🔧 Scale", null, async (sender, e) =>
                            await SafeExecuteAsync(Controller?.ScaleWorkloadAsync(CurrentNamespace, s.kind, s.name) ?? System.Threading.Tasks.Task.CompletedTask));
                    }

                    menu.Items.Add("🔄 Restart", null, async (sender, e) =>
                        await SafeExecuteAsync(Controller?.RestartWorkloadAsync(CurrentNamespace, s.kind, s.name) ?? System.Threading.Tasks.Task.CompletedTask));

                    menu.Items.Add("📋 View Pods", null, async (sender, e) =>
                        await SafeExecuteAsync(Controller?.ViewWorkloadPodsAsync(CurrentNamespace, s.kind, s.name) ?? System.Threading.Tasks.Task.CompletedTask));
                }

                // Pod-specific actions
                if (isPod)
                {
                    menu.Items.Add(new ToolStripSeparator());
                    menu.Items.Add("🔍 View Details", null, async (sender, e) =>
                        await SafeExecuteAsync(Controller?.ShowPodDetailsAsync(CurrentNamespace, s.name) ?? System.Threading.Tasks.Task.CompletedTask));
                    menu.Items.Add("📝 View Logs", null, async (sender, e) => await SafeExecuteAsync(TailLogsAsync()).ConfigureAwait(false));
                    menu.Items.Add("💻 Shell", null, (sender, e) => ShowShell());
                    menu.Items.Add("🔌 Port Forward", null, (sender, e) => ShowPortForward());
                }

                menu.Items.Add(new ToolStripSeparator());
                menu.Items.Add("⭐ Add to Favorites", null, async (sender, e) => await SafeExecuteAsync(AddToFavoritesAsync()).ConfigureAwait(false));
                menu.Items.Add("🗑️ Delete", null, async (sender, e) => await SafeExecuteAsync(DeleteResourceAsync()).ConfigureAwait(false));
            }
        }

        private async System.Threading.Tasks.Task OnGridDoubleClickAsync(DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || Controller == null) return;

            // Get selected resource info
            if (SelectedExplorer() is { } s)
            {
                // For Pods, show detailed dialog
                if (s.kind == "Pods")
                {
                    await Controller!.ShowPodDetailsAsync(CurrentNamespace, s.name);
                }
                // For Deployments, show detailed dialog
                else if (s.kind == "Deployments")
                {
                    await Controller!.ShowDeploymentDetailsAsync(CurrentNamespace, s.name);
                }
                // For StatefulSets, show detailed dialog
                else if (s.kind == "StatefulSets")
                {
                    await Controller!.ShowStatefulSetDetailsAsync(CurrentNamespace, s.name);
                }
                // For DaemonSets, show detailed dialog
                else if (s.kind == "DaemonSets")
                {
                    await Controller!.ShowDaemonSetDetailsAsync(CurrentNamespace, s.name);
                }
                // For other resources, show YAML
                else
                {
                    await LoadYamlAsync().ConfigureAwait(false);
                }
            }
        }

        private async System.Threading.Tasks.Task SafeExecuteAsync(System.Threading.Tasks.Task task)
        {
            var logFile = System.IO.Path.Combine(
                System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) ?? ".",
                "error.log");

            try
            {
                await task.ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText(logFile, $"\n=== UI Event Error ===\n{ex}\n");
                MessageBox.Show(this,
                    $"An error occurred:\n{ex.Message}\n\nSee error.log for details",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void OnKeyDown(object? sender, KeyEventArgs e)
        {
            // Keyboard shortcuts
            if (e.Control)
            {
                switch (e.KeyCode)
                {
                    case Keys.Y:
                        e.Handled = true;
                        _ = SafeExecuteAsync(LoadYamlAsync());
                        break;
                    case Keys.D:
                        e.Handled = true;
                        _ = SafeExecuteAsync(DescribeAsync());
                        break;
                    case Keys.L:
                        e.Handled = true;
                        _ = SafeExecuteAsync(TailLogsAsync());
                        break;
                    case Keys.K:
                        e.Handled = true;
                        ShowApplyYaml();
                        break;
                    case Keys.P:
                        e.Handled = true;
                        ShowPortForward();
                        break;
                    case Keys.S:
                        e.Handled = true;
                        ShowShell();
                        break;
                }
            }
            else if (e.KeyCode == Keys.F5)
            {
                e.Handled = true;
                _ = SafeExecuteAsync(ReloadAsync());
            }
            else if (e.KeyCode == Keys.F1)
            {
                e.Handled = true;
                ShowKeyboardShortcuts();
            }
            else if (e.KeyCode == Keys.Delete)
            {
                e.Handled = true;
                _ = SafeExecuteAsync(DeleteResourceAsync());
            }
        }

        private void ShowKeyboardShortcuts()
        {
            var shortcuts = @"⌨️ KEYBOARD SHORTCUTS

VIEW & NAVIGATION:
  F5                 Refresh current view
  Ctrl + Y           View YAML
  Ctrl + D           Describe resource
  
POD OPERATIONS:
  Ctrl + L           View logs
  Ctrl + S           Open shell
  Ctrl + P           Port forward
  
RESOURCE MANAGEMENT:
  Ctrl + K           Apply YAML
  Delete             Delete resource
  
HELP:
  F1                 Show this help

💡 TIP: Hover over buttons to see their shortcuts!";

            MessageBox.Show(this,
                shortcuts,
                "Keyboard Shortcuts - K8s Manager",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void UpdateButtonPermissions()
        {
            if (SessionService == null || string.IsNullOrWhiteSpace(SessionToken))
            {
                // No session - disable write/delete operations
                _btnApply.Enabled = false;
                _btnDelete.Enabled = false;
                _btnShell.Enabled = false;
                _btnPF.Enabled = false;
                _statusInfo.Text = "⚠️ Not authenticated - read-only mode";
                _statusInfo.ForeColor = Color.Orange;
                return;
            }

            try
            {
                // Check permissions based on current user's role
                bool canWrite = SessionService.CanWrite();
                bool canDelete = SessionService.CanDelete();

                _btnApply.Enabled = canWrite;
                _btnShell.Enabled = canWrite;
                _btnPF.Enabled = canWrite;
                _btnDelete.Enabled = canDelete;

                // Update status bar with user info
                var user = SessionService.CurrentUser;
                if (user != null)
                {
                    var roleDisplay = user.Role switch
                    {
                        "Admin" => "👑 Admin",
                        "Operator" => "⚙️ Operator",
                        "Viewer" => "👁️ Viewer",
                        _ => $"📋 {user.Role}"
                    };
                    _statusInfo.Text = $"👤 {CurrentUsername} ({roleDisplay})";
                    _statusInfo.ForeColor = Color.Green;
                }
            }
            catch (Exception ex)
            {
                _statusInfo.Text = $"⚠️ Error checking permissions: {ex.Message}";
                _statusInfo.ForeColor = Color.Red;
            }
        }

        private void ShowTemplateManager()
        {
            if (TemplateService == null || SessionService == null || K8sExplorerService == null)
            {
                MessageBox.Show(this,
                    "Template Manager is not properly initialized.\n\n" +
                    "Required services are missing.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            try
            {
                using var dialog = new TemplateManagerDialog(
                    TemplateService,
                    SessionService,
                    Controller ?? throw new InvalidOperationException("Controller not initialized"));
                dialog.ShowDialog(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this,
                    $"Error opening Template Manager:\n{ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private async System.Threading.Tasks.Task AddToFavoritesAsync()
        {
            try
            {
                if (FavoriteService == null)
                {
                    MessageBox.Show(this, "Favorite service is not initialized.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (SelectedExplorer() is { } s)
                {
                    // Prompt for notes
                    var notes = Microsoft.VisualBasic.Interaction.InputBox(
                        $"Add notes for {s.kind}/{s.name}:",
                        "Add to Favorites",
                        $"Favorite {s.kind} in {CurrentNamespace}",
                        -1, -1);

                    if (string.IsNullOrEmpty(notes)) return; // User cancelled or empty

                    // Convert kind to singular form for API
                    var resourceType = s.kind?.TrimEnd('s') ?? s.kind ?? "Unknown";

                    await FavoriteService.CreateFavoriteAsync(resourceType, s.name, CurrentNamespace, notes);

                    MessageBox.Show(this,
                        $"Added '{s.name}' to Favorites!",
                        "Success",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(this, "Please select a resource to add to favorites.", "No Resource Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this,
                    $"Error adding to favorites:\n{ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void ShowFavorites()
        {
            try
            {
                if (FavoriteService == null)
                {
                    MessageBox.Show(this, "Favorite service is not initialized.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                using var dialog = new FavoritesDialog(FavoriteService);
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    // If user selected a resource to navigate to
                    if (!string.IsNullOrEmpty(dialog.SelectedResource))
                    {
                        // TODO: Navigate to the selected resource
                        MessageBox.Show(this,
                            $"Navigate to {dialog.SelectedResourceType}/{dialog.SelectedResource} in {dialog.SelectedNamespace}",
                            "Navigation",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this,
                    $"Error opening Favorites:\n{ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void ShowUserManager()
        {
            try
            {
                if (SessionService?.ApiClient == null)
                {
                    MessageBox.Show(this, "API Client is not initialized.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                using var dialog = new UserManagerDialog(SessionService.ApiClient);
                dialog.ShowDialog(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this,
                    $"Error opening User Manager:\n{ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void ShowSettings()
        {
            try
            {
                if (SettingsService == null)
                {
                    MessageBox.Show(this, "Settings service is not initialized.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                using var dialog = new SettingsDialog(SettingsService);
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    MessageBox.Show(this,
                        "Settings saved successfully!\n\nSome changes may require restarting the application.",
                        "Settings Updated",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this,
                    $"Error opening Settings:\n{ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void ShowAuditLogs()
        {
            try
            {
                if (AuditService == null)
                {
                    MessageBox.Show(this, "Audit service is not initialized.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                using var dialog = new AuditLogDialog(AuditService);
                dialog.ShowDialog(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this,
                    $"Error opening Audit Logs:\n{ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // ============ AUTO-REFRESH FUNCTIONALITY ============

        public void InitializeAutoRefresh()
        {
            _autoRefreshTimer = new System.Windows.Forms.Timer
            {
                Interval = _autoRefreshInterval * 1000  // Convert seconds to milliseconds
            };
            _autoRefreshTimer.Tick += AutoRefreshTimer_Tick;

            // Load saved preferences (auto-refresh and theme)
            if (SettingsService != null)
            {
                _ = Task.Run(async () =>
                {
                    try
                    {
                        var settings = await SettingsService.GetAllSettingsAsync();
                        var autoRefreshSetting = settings.FirstOrDefault(s => s.Key == "App.AutoRefresh");
                        var intervalSetting = settings.FirstOrDefault(s => s.Key == "App.RefreshInterval");
                        var themeSetting = settings.FirstOrDefault(s => s.Key == "App.Theme");

                        Invoke(() =>
                        {
                            // Load auto-refresh settings
                            if (autoRefreshSetting != null && bool.TryParse(autoRefreshSetting.Value, out var enabled))
                            {
                                _autoRefreshEnabled = enabled;
                                if (enabled) _autoRefreshTimer.Start();
                            }

                            if (intervalSetting != null && int.TryParse(intervalSetting.Value, out var interval) && interval >= 10)
                            {
                                _autoRefreshInterval = interval;
                                _autoRefreshTimer.Interval = interval * 1000;
                            }

                            // Load theme setting
                            if (themeSetting != null && themeSetting.Value == "Dark" && !_isDarkMode)
                            {
                                ToggleTheme();  // Switch to dark mode
                            }
                        });
                    }
                    catch
                    {
                        // Ignore settings load errors
                    }
                });
            }
        }

        private async void AutoRefreshTimer_Tick(object? sender, EventArgs e)
        {
            if (_isLoading) return;  // Skip if already loading
            if (Controller == null) return;

            try
            {
                await SafeExecuteAsync(ReloadAsync()).ConfigureAwait(false);
            }
            catch
            {
                // Ignore refresh errors
            }
        }

        public void ToggleAutoRefresh()
        {
            _autoRefreshEnabled = !_autoRefreshEnabled;

            if (_autoRefreshEnabled)
            {
                _autoRefreshTimer?.Start();
                ShowStatus($"🔄 Auto-refresh enabled ({_autoRefreshInterval}s interval)", StatusType.Success);
            }
            else
            {
                _autoRefreshTimer?.Stop();
                ShowStatus("⏸️ Auto-refresh disabled", StatusType.Info);
            }

            // Save preference
            if (SettingsService != null)
            {
                _ = SettingsService.UpsertSettingAsync("App.AutoRefresh", _autoRefreshEnabled.ToString());
            }
        }

        public void SetAutoRefreshInterval(int seconds)
        {
            if (seconds < 10)
            {
                MessageBox.Show(this,
                    "Auto-refresh interval must be at least 10 seconds.",
                    "Invalid Interval",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            _autoRefreshInterval = seconds;
            if (_autoRefreshTimer != null)
            {
                _autoRefreshTimer.Interval = seconds * 1000;
            }

            ShowStatus($"🔄 Refresh interval set to {seconds} seconds", StatusType.Success);

            // Save preference
            if (SettingsService != null)
            {
                _ = SettingsService.UpsertSettingAsync("App.RefreshInterval", seconds.ToString());
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _autoRefreshTimer?.Stop();
                _autoRefreshTimer?.Dispose();
                _loadCancellation?.Cancel();
                _loadCancellation?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
