using MaterialSkin;
using System.ComponentModel;
using MaterialSkin.Controls;

namespace K8sManager.Views
{
    [DesignerCategory("Form")]
    public partial class MainView : MaterialForm, IMainView
    {
        private MaterialComboBox _cbNamespace = null!;
        private MaterialTextBox2 _txtSearch = null!;
        private MaterialButton _btnRefresh = null!;
        private MaterialButton _btnScale = null!;
        private MaterialButton _btnRestart = null!;
        private MaterialButton _btnTail = null!;
        private MaterialButton _btnTheme = null!;
        private SplitContainer _split = null!;
        private DataGridView _grid = null!;
        private MaterialTabControl _tabs = null!;
        private TabPage _tabYaml = null!;
        private TabPage _tabDescribe = null!;
        private TabPage _tabLogs = null!;
        private RichTextBox _txtYaml = null!;
        private RichTextBox _txtDescribe = null!;
        private TextBox _txtLogs = null!;
        private StatusStrip _status = null!;
        private ToolStripStatusLabel _statusContext = null!;
        private ToolStripStatusLabel _statusInfo = null!;
        private ToolStripStatusLabel _statusLatency = null!;
        private TableLayoutPanel _topPanel = null!;

        public event EventHandler? RefreshRequested;
        public event EventHandler<ScaleRequest>? ScaleRequested;
        public event EventHandler<RestartPodRequest>? RestartPodRequested;
        public event EventHandler<TailLogsRequest>? TailLogsRequested;

        public MainView()
        {
            InitializeComponent();

            // Context menu
            var ctx = new ContextMenuStrip();
            ctx.Items.Add("Tail Logs", null, (s, e) => TailLogsSelected());
            ctx.Items.Add("Scale…", null, (s, e) => ScaleSelected());
            ctx.Items.Add("Restart Pod", null, (s, e) => RestartSelected());
            _grid.ContextMenuStrip = ctx;

            // Event handlers
            _btnRefresh.Click += (s, e) => RefreshRequested?.Invoke(this, EventArgs.Empty);
            _btnScale.Click += (s, e) => ScaleSelected();
            _btnRestart.Click += (s, e) => RestartSelected();
            _btnTail.Click += (s, e) => TailLogsSelected();
            _cbNamespace.SelectedIndexChanged += (s, e) => RefreshRequested?.Invoke(this, EventArgs.Empty);
            _txtSearch.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) RefreshRequested?.Invoke(this, EventArgs.Empty); };
            _btnTheme.Click += (s, e) => ToggleTheme();
        }

        private void ToggleTheme()
        {
            var m = MaterialSkinManager.Instance;
            if (m.Theme == MaterialSkinManager.Themes.DARK) { m.Theme = MaterialSkinManager.Themes.LIGHT; _btnTheme.Text = "Dark"; }
            else { m.Theme = MaterialSkinManager.Themes.DARK; _btnTheme.Text = "Light"; }
            Invalidate(true); Refresh();
        }

        private bool TryGetSelected(out string ns, out string name)
        {
            ns = CurrentNamespace;
            name = "";
            if (_grid.CurrentRow?.DataBoundItem is PodViewModel vm)
            {
                name = vm.Name;
                return true;
            }
            ShowMessage("Please select a row first.");
            return false;
        }

        private void ScaleSelected()
        {
            if (!TryGetSelected(out var ns, out var name)) return;
            using var dlg = new ScaleDialog(name);
            if (dlg.ShowDialog(this) == DialogResult.OK)
                ScaleRequested?.Invoke(this, new ScaleRequest(ns, name, dlg.Replicas));
        }

        private void RestartSelected()
        {
            if (!TryGetSelected(out var ns, out var name)) return;
            var ok = MessageBox.Show($"Restart pod '{name}'?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (ok == DialogResult.Yes)
                RestartPodRequested?.Invoke(this, new RestartPodRequest(ns, name));
        }

        private void TailLogsSelected()
        {
            if (!TryGetSelected(out var ns, out var name)) return;
            TailLogsRequested?.Invoke(this, new TailLogsRequest(ns, name, null));
            _tabs.SelectedTab = _tabLogs;
        }

        // ===== IMainView =====
        public string CurrentNamespace => _cbNamespace.SelectedItem?.ToString() ?? "default";

        // NEW: bind namespaces động
        public void BindNamespaces(IEnumerable<string> namespaces)
        {
            _cbNamespace.BeginUpdate();
            _cbNamespace.Items.Clear();
            foreach (var ns in namespaces)
                _cbNamespace.Items.Add(ns);
            _cbNamespace.EndUpdate();

            if (_cbNamespace.Items.Count > 0 && _cbNamespace.SelectedIndex < 0)
                _cbNamespace.SelectedIndex = 0;
        }

        public void BindPods(IEnumerable<PodViewModel> pods)
        {
            var list = pods.ToList();
            var q = _txtSearch.Text?.Trim();
            if (!string.IsNullOrWhiteSpace(q))
                list = list.Where(x => (x.Name?.IndexOf(q, StringComparison.OrdinalIgnoreCase) ?? -1) >= 0).ToList();

            _grid.DataSource = list;

            if (_grid.Columns.Count > 0)
            {
                foreach (DataGridViewColumn col in _grid.Columns)
                    col.FillWeight = 1f;
                if (_grid.Columns.Contains("Name")) _grid.Columns["Name"].FillWeight = 180f;
                if (_grid.Columns.Contains("Namespace")) _grid.Columns["Namespace"].FillWeight = 110f;
                if (_grid.Columns.Contains("Ready")) _grid.Columns["Ready"].FillWeight = 60f;
            }

            _statusInfo.Text = $"{list.Count} items";
        }

        public void AppendLog(string line) => _txtLogs.AppendText(line + Environment.NewLine);
        public void ShowMessage(string text) => MessageBox.Show(this, text, "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        public void ShowYaml(string yaml) { _txtYaml.Text = yaml; _tabs.SelectedTab = _tabYaml; }
        public void ShowDescribe(string desc) { _txtDescribe.Text = desc; _tabs.SelectedTab = _tabDescribe; }

       
    }
}
