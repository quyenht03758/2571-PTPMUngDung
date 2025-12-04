using System.Data;
using System.ComponentModel;
using K8sManager.Models.Entities;
using K8sManager.Services;
using MaterialSkin;
using MaterialSkin.Controls;

namespace K8sManager.Views
{
    [DesignerCategory("Form")]
    public partial class AuditLogDialog : MaterialForm
    {
        private readonly ApiAuditService _auditService;
        private readonly DataGridView _grid = new DataGridView();
        private readonly MaterialTextBox2 _txtSearch = new MaterialTextBox2();
        private readonly MaterialComboBox _cbAction = new MaterialComboBox();
        private readonly DateTimePicker _dateFrom = new DateTimePicker { Value = DateTime.Now.AddMonths(-2) };
        private readonly DateTimePicker _dateTo = new DateTimePicker { Value = DateTime.Now.AddDays(1) };
        private readonly MaterialButton _btnRefresh = new MaterialButton();
        private readonly MaterialButton _btnExport = new MaterialButton();
        private readonly MaterialButton _btnClose = new MaterialButton();
        private readonly Label _lblTotal = new Label();

        public AuditLogDialog(ApiAuditService auditService)
        {
            InitializeComponent();
            
            _auditService = auditService ?? throw new ArgumentNullException(nameof(auditService));
            
            Text = "📊 Audit Logs Viewer";
            Size = new Size(1200, 700);
            StartPosition = FormStartPosition.CenterScreen;
            MaximizeBox = true;

            var skin = MaterialSkinManager.Instance;
            skin.AddFormToManage(this);

            InitializeControls();
            
            // Load data AFTER controls are initialized
            this.Load += async (s, e) => await LoadAuditLogsAsync();
        }

        private void InitializeControls()
        {
            // Top panel with filters
            var topPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 100,
                ColumnCount = 6,
                RowCount = 2,
                Padding = new Padding(16, 12, 16, 12)
            };
            topPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30));
            topPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20));
            topPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
            topPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
            topPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            topPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

            // Search box
            _txtSearch.Hint = "Search by username, resource, or description...";
            _txtSearch.TextChanged += (s, e) => FilterLogs();

            // Action filter
            _cbAction.Hint = "Action Type";
            _cbAction.Items.Add("All Actions");
            _cbAction.Items.Add("Login");
            _cbAction.Items.Add("Logout");
            _cbAction.Items.Add("Create");
            _cbAction.Items.Add("Update");
            _cbAction.Items.Add("Delete");
            _cbAction.Items.Add("Apply");
            _cbAction.Items.Add("Scale");
            _cbAction.Items.Add("Restart");
            _cbAction.SelectedIndex = 0;
            _cbAction.Text = "All Actions";  // Set text explicitly for MaterialComboBox
            _cbAction.SelectedIndexChanged += (s, e) => FilterLogs();

            // Date filters - Set values BEFORE attaching event handlers
            _dateFrom.Format = DateTimePickerFormat.Short;
            _dateFrom.Value = DateTime.Now.AddMonths(-2);

            _dateTo.Format = DateTimePickerFormat.Short;
            _dateTo.Value = DateTime.Now;
            
            // Attach event handlers AFTER setting values
            _dateFrom.ValueChanged += (s, e) => FilterLogs();
            _dateTo.ValueChanged += (s, e) => FilterLogs();

            // Buttons
            _btnRefresh.Text = "Refresh";
            _btnRefresh.Type = MaterialButton.MaterialButtonType.Contained;
            _btnRefresh.Click += async (s, e) => await LoadAuditLogsAsync();

            _btnExport.Text = "Export CSV";
            _btnExport.Type = MaterialButton.MaterialButtonType.Outlined;
            _btnExport.Click += (s, e) => ExportToCsv();

            topPanel.Controls.Add(_txtSearch, 0, 0);
            topPanel.SetRowSpan(_txtSearch, 2);
            topPanel.Controls.Add(_cbAction, 1, 0);
            topPanel.SetRowSpan(_cbAction, 2);
            topPanel.Controls.Add(_dateFrom, 2, 0);
            topPanel.Controls.Add(_dateTo, 2, 1);
            topPanel.Controls.Add(_btnRefresh, 4, 0);
            topPanel.Controls.Add(_btnExport, 5, 0);

            // Grid
            _grid.Dock = DockStyle.Fill;
            _grid.ReadOnly = true;
            _grid.AutoGenerateColumns = false;
            _grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            _grid.MultiSelect = false;
            _grid.RowHeadersVisible = false;
            _grid.AllowUserToAddRows = false;
            _grid.BackgroundColor = Color.White;
            _grid.BorderStyle = BorderStyle.None;
            _grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            _grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            _grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            _grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);
            _grid.ColumnHeadersDefaultCellStyle.Font = new Font(_grid.Font, FontStyle.Bold);
            _grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(250, 250, 250);

            _grid.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "CreatedAt",
                HeaderText = "Timestamp",
                DataPropertyName = "CreatedAt",
                Width = 160,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "yyyy-MM-dd HH:mm:ss" }
            });
            _grid.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "UserId",
                HeaderText = "User ID",
                DataPropertyName = "UserId",
                Width = 80
            });
            _grid.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Action",
                HeaderText = "Action",
                DataPropertyName = "Action",
                Width = 100
            });
            _grid.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ResourceKind",
                HeaderText = "Resource Type",
                DataPropertyName = "ResourceKind",
                Width = 120
            });
            _grid.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ResourceName",
                HeaderText = "Resource Name",
                DataPropertyName = "ResourceName",
                Width = 180
            });
            _grid.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Namespace",
                HeaderText = "Namespace",
                DataPropertyName = "Namespace",
                Width = 120
            });
            _grid.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ErrorMessage",
                HeaderText = "Error Message",
                DataPropertyName = "ErrorMessage",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });
            _grid.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Success",
                HeaderText = "Success",
                DataPropertyName = "Success",
                Width = 80
            });
            _grid.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Duration",
                HeaderText = "Duration (ms)",
                DataPropertyName = "Duration",
                Width = 100
            });

            // Bottom panel
            var bottomPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 50,
                Padding = new Padding(16, 8, 16, 8)
            };

            _lblTotal.Text = "Total: 0 records";
            _lblTotal.Dock = DockStyle.Left;
            _lblTotal.AutoSize = true;
            _lblTotal.TextAlign = ContentAlignment.MiddleLeft;

            _btnClose.Text = "Close";
            _btnClose.Type = MaterialButton.MaterialButtonType.Text;
            _btnClose.Dock = DockStyle.Right;
            _btnClose.Click += (s, e) => Close();

            bottomPanel.Controls.Add(_lblTotal);
            bottomPanel.Controls.Add(_btnClose);

            // Main layout
            Controls.Add(_grid);
            Controls.Add(topPanel);
            Controls.Add(bottomPanel);
        }

        private List<AuditLog> _allLogs = new List<AuditLog>();
        private List<AuditLog> _filteredLogs = new List<AuditLog>();

        private async Task LoadAuditLogsAsync()
        {
            try
            {
                _btnRefresh.Enabled = false;
                _btnRefresh.Text = "Loading...";
                Cursor = Cursors.WaitCursor;

                _allLogs = await _auditService.GetLogsAsync(
                    fromDate: _dateFrom.Value,
                    toDate: _dateTo.Value.AddDays(1).AddSeconds(-1),
                    pageSize: 1000
                );

                FilterLogs();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, 
                    $"Error loading audit logs:\n{ex.Message}", 
                    "Error", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);
            }
            finally
            {
                _btnRefresh.Enabled = true;
                _btnRefresh.Text = "Refresh";
                Cursor = Cursors.Default;
            }
        }

        private void FilterLogs()
        {
            if (_allLogs == null || !_allLogs.Any())
            {
                _filteredLogs = new List<AuditLog>();
                _grid.DataSource = null;
                _lblTotal.Text = "Total: 0 records";
                return;
            }

            var filtered = _allLogs.AsEnumerable();

            // Filter by search text
            var searchText = _txtSearch.Text?.Trim().ToLower();
            if (!string.IsNullOrEmpty(searchText))
            {
                filtered = filtered.Where(log =>
                    (log.ResourceName?.ToLower().Contains(searchText) ?? false) ||
                    (log.ResourceKind?.ToLower().Contains(searchText) ?? false) ||
                    (log.ErrorMessage?.ToLower().Contains(searchText) ?? false));
            }

            // Filter by action (skip if "All Actions" is selected - index 0)
            if (_cbAction.SelectedIndex > 0)
            {
                var selectedAction = _cbAction.SelectedItem?.ToString() ?? "";
                filtered = filtered.Where(log => log.Action == selectedAction);
            }

            _filteredLogs = filtered.ToList();
            _grid.DataSource = null;  // Clear first
            _grid.DataSource = _filteredLogs;  // Then rebind
            _lblTotal.Text = $"Total: {_filteredLogs.Count} records (filtered from {_allLogs.Count})";
        }

        private void ExportToCsv()
        {
            try
            {
                var logsToExport = _filteredLogs.Any() ? _filteredLogs : _allLogs;
                
                if (!logsToExport.Any())
                {
                    MessageBox.Show(this,
                        "No audit logs to export.",
                        "Export",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    return;
                }

                using var sfd = new SaveFileDialog
                {
                    Filter = "CSV Files (*.csv)|*.csv",
                    FileName = $"AuditLogs_{DateTime.Now:yyyyMMdd_HHmmss}.csv"
                };

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    var csv = new System.Text.StringBuilder();
                    
                    // Header
                    csv.AppendLine("Timestamp,UserID,Action,ResourceType,ResourceName,Namespace,ErrorMessage,Success,Duration(ms)");
                    
                    // Data
                    foreach (var log in logsToExport)
                    {
                        csv.AppendLine($"\"{log.CreatedAt:yyyy-MM-dd HH:mm:ss}\"," +
                            $"\"{log.UserId}\"," +
                            $"\"{log.Action}\"," +
                            $"\"{log.ResourceKind ?? ""}\"," +
                            $"\"{log.ResourceName ?? ""}\"," +
                            $"\"{log.Namespace ?? ""}\"," +
                            $"\"{(log.ErrorMessage ?? "").Replace("\"", "\"\"")}\"," +
                            $"\"{log.Success}\"," +
                            $"\"{log.Duration ?? 0}\"");
                    }

                    File.WriteAllText(sfd.FileName, csv.ToString());
                    
                    MessageBox.Show(this, 
                        $"Successfully exported {logsToExport.Count} records to:\n\n{sfd.FileName}", 
                        "Export Successful", 
                        MessageBoxButtons.OK, 
                        MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, 
                    $"Error exporting to CSV:\n\n{ex.Message}", 
                    "Export Error", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);
            }
        }
    }
}
