using System.Diagnostics;
using System.ComponentModel;
using MaterialSkin;
using MaterialSkin.Controls;

namespace K8sManager.Views
{
    public partial class PortForwardManager : MaterialForm
    {
        private class PFItem
        {
            public string Namespace { get; set; } = "default";
            public string Pod { get; set; } = "";
            public int LocalPort { get; set; }
            public int RemotePort { get; set; }
            public string Status { get; set; } = "Running";
            public int Pid { get; set; }
        }

        private readonly BindingList<PFItem> _items = new BindingList<PFItem>();
        private readonly string? _initialNamespace;
        private readonly string? _initialPodName;

        public PortForwardManager(string? initialNamespace = null, string? initialPodName = null)
        {
            _initialNamespace = initialNamespace;
            _initialPodName = initialPodName;
            
            InitializeComponent();
            
            var skin = MaterialSkinManager.Instance;
            skin.AddFormToManage(this);

            InitializeControls();
            RefreshGrid();
        }

        private void InitializeControls()
        {
            _btnAdd.Click += (s, e) => AddPF();
            _btnStop.Click += (s, e) => StopSelected();
            _btnRefresh.Click += (s, e) => RefreshGrid();
            _btnClose.Click += (s, e) => Close();
            
            _grid.DefaultCellStyle.Padding = new Padding(5);
            _grid.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(63, 81, 181);
            _grid.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;
        }

        private void RefreshGrid()
        {
            // Check if processes are still alive and update status
            foreach (var item in _items.ToList())
            {
                try
                {
                    var p = Process.GetProcessById(item.Pid);
                    item.Status = p.HasExited ? "Stopped" : "Running";
                }
                catch
                {
                    item.Status = "Stopped";
                }
            }

            _grid.DataSource = null;
            _grid.DataSource = _items;
            _lblTotal.Text = $"Active forwards: {_items.Count(x => x.Status == "Running")} / {_items.Count}";
        }

        private void AddPF()
        {
            using var dlg = new PortForwardDialog(_initialNamespace, _initialPodName);
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                if (string.IsNullOrWhiteSpace(dlg.Pod))
                {
                    MessageBox.Show(this, "Pod name is required!", "Validation Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var psi = new ProcessStartInfo("kubectl", 
                    $"port-forward -n {dlg.Namespace} pod/{dlg.Pod} {dlg.LocalPort}:{dlg.RemotePort}")
                {
                    UseShellExecute = false,
                    CreateNoWindow = true,
                };
                
                try
                {
                    var p = Process.Start(psi);
                    if (p != null)
                    {
                        _items.Add(new PFItem 
                        { 
                            Namespace = dlg.Namespace, 
                            Pod = dlg.Pod, 
                            LocalPort = dlg.LocalPort, 
                            RemotePort = dlg.RemotePort, 
                            Status = "Running",
                            Pid = p.Id 
                        });
                        RefreshGrid();
                        MessageBox.Show(this, 
                            $"Port forward started!\n\nAccess at: http://localhost:{dlg.LocalPort}", 
                            "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, $"Failed to start port forward:\n{ex.Message}", 
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void StopSelected()
        {
            if (_grid.SelectedRows.Count == 0)
            {
                MessageBox.Show(this, "Please select a port forward to stop.", 
                    "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var item = _grid.SelectedRows[0].DataBoundItem as PFItem;
            if (item == null) return;

            var result = MessageBox.Show(this,
                $"Stop port forward?\n\n{item.Namespace}/{item.Pod}\n{item.LocalPort} → {item.RemotePort}",
                "Confirm Stop",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    var p = Process.GetProcessById(item.Pid);
                    p.Kill(true);
                    item.Status = "Stopped";
                    MessageBox.Show(this, "Port forward stopped successfully!", 
                        "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, $"Error stopping port forward:\n{ex.Message}", 
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                finally
                {
                    RefreshGrid();
                }
            }
        }
    }

    internal class PortForwardDialog : MaterialForm
    {
        private readonly MaterialTextBox2 _txtNamespace;
        private readonly MaterialTextBox2 _txtPod;
        private readonly MaterialTextBox2 _txtLocalPort;
        private readonly MaterialTextBox2 _txtRemotePort;
        private readonly MaterialButton _btnOk;
        private readonly MaterialButton _btnCancel;
        private readonly TableLayoutPanel _table;

        public string Namespace => _txtNamespace.Text.Trim();
        public string Pod => _txtPod.Text.Trim();
        public int LocalPort => int.TryParse(_txtLocalPort.Text, out var port) ? port : 8080;
        public int RemotePort => int.TryParse(_txtRemotePort.Text, out var port) ? port : 80;

        public PortForwardDialog(string? namespaceName = null, string? podName = null)
        {
            var skin = MaterialSkinManager.Instance;
            skin.AddFormToManage(this);

            Text = "Add Port Forward";
            Width = 500;
            Height = 450;
            StartPosition = FormStartPosition.CenterParent;

            // Create controls
            _txtNamespace = new MaterialTextBox2
            {
                Text = namespaceName ?? "default",
                Hint = "Namespace",
                Dock = DockStyle.Fill
            };

            _txtPod = new MaterialTextBox2
            {
                Text = podName ?? "",
                Hint = "Pod Name (required)",
                Dock = DockStyle.Fill
            };

            _txtLocalPort = new MaterialTextBox2
            {
                Text = "8080",
                Hint = "Local Port",
                Dock = DockStyle.Fill
            };

            _txtRemotePort = new MaterialTextBox2
            {
                Text = "80",
                Hint = "Remote Port",
                Dock = DockStyle.Fill
            };

            _btnOk = new MaterialButton
            {
                Text = "Start Forward",
                Type = MaterialButton.MaterialButtonType.Contained,
                Dock = DockStyle.Right,
                Width = 140,
                Height = 36,
                DialogResult = DialogResult.OK
            };

            _btnCancel = new MaterialButton
            {
                Text = "Cancel",
                Type = MaterialButton.MaterialButtonType.Text,
                Dock = DockStyle.Right,
                Width = 100,
                Height = 36,
                DialogResult = DialogResult.Cancel
            };

            // Layout
            var titleLabel = new Label
            {
                Text = "🔀 Configure Port Forward",
                Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold),
                Dock = DockStyle.Top,
                Height = 50,
                Padding = new Padding(16, 16, 16, 0),
                TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            };

            _table = new TableLayoutPanel
            {
                RowCount = 4,
                ColumnCount = 1,
                Dock = DockStyle.Fill,
                Padding = new Padding(16)
            };

            _table.RowStyles.Add(new RowStyle(SizeType.Absolute, 70F));
            _table.RowStyles.Add(new RowStyle(SizeType.Absolute, 70F));
            _table.RowStyles.Add(new RowStyle(SizeType.Absolute, 70F));
            _table.RowStyles.Add(new RowStyle(SizeType.Absolute, 70F));

            _table.Controls.Add(_txtNamespace, 0, 0);
            _table.Controls.Add(_txtPod, 0, 1);
            _table.Controls.Add(_txtLocalPort, 0, 2);
            _table.Controls.Add(_txtRemotePort, 0, 3);

            var buttonPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 60,
                Padding = new Padding(16, 12, 16, 12)
            };
            buttonPanel.Controls.Add(_btnCancel);
            buttonPanel.Controls.Add(_btnOk);

            Controls.Add(_table);
            Controls.Add(buttonPanel);
            Controls.Add(titleLabel);

            AcceptButton = _btnOk;
            CancelButton = _btnCancel;
        }
    }
}
