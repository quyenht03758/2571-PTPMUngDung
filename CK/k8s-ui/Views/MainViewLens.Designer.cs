namespace K8sManager.Views
{
    partial class MainViewLens
    {
        private void InitializeComponent()
        {
            this._split = new System.Windows.Forms.SplitContainer();
            this._explorer = new MaterialSkin.Controls.MaterialListView();
            this._tabs = new MaterialSkin.Controls.MaterialTabControl();
            this._tabList = new System.Windows.Forms.TabPage();
            this._grid = new K8sManager.Views.DoubleBufferedDataGridView();
            this._tabYaml = new System.Windows.Forms.TabPage();
            this._rtYaml = new System.Windows.Forms.RichTextBox();
            this._tabLogs = new System.Windows.Forms.TabPage();
            this._txtLogs = new System.Windows.Forms.TextBox();
            this._tabEvents = new System.Windows.Forms.TabPage();
            this._gridEvents = new K8sManager.Views.DoubleBufferedDataGridView();
            this._tabMetrics = new System.Windows.Forms.TabPage();
            this._status = new System.Windows.Forms.StatusStrip();
            this._statusInfo = new System.Windows.Forms.ToolStripStatusLabel();
            this._statusNs = new System.Windows.Forms.ToolStripStatusLabel();
            this._statusKind = new System.Windows.Forms.ToolStripStatusLabel();
            this._topPanel = new System.Windows.Forms.TableLayoutPanel();
            this._cbNamespace = new MaterialSkin.Controls.MaterialComboBox();
            this._cbContext = new MaterialSkin.Controls.MaterialComboBox();
            this._txtSearch = new MaterialSkin.Controls.MaterialTextBox2();
            this._btnRefresh = new MaterialSkin.Controls.MaterialButton();
            this._btnTail = new MaterialSkin.Controls.MaterialButton();
            this._btnYaml = new MaterialSkin.Controls.MaterialButton();
            this._btnDescribe = new MaterialSkin.Controls.MaterialButton();
            this._btnApply = new MaterialSkin.Controls.MaterialButton();
            this._btnPF = new MaterialSkin.Controls.MaterialButton();
            this._btnShell = new MaterialSkin.Controls.MaterialButton();
            this._btnDelete = new MaterialSkin.Controls.MaterialButton();
            this._btnTheme = new MaterialSkin.Controls.MaterialButton();
            this._btnTemplates = new MaterialSkin.Controls.MaterialButton();
            this._btnFavorites = new MaterialSkin.Controls.MaterialButton();
            this._btnSettings = new MaterialSkin.Controls.MaterialButton();
            this._btnAuditLogs = new MaterialSkin.Controls.MaterialButton();
            this._tooltip = new System.Windows.Forms.ToolTip();
            ((System.ComponentModel.ISupportInitialize)(this._split)).BeginInit();
            this._split.Panel1.SuspendLayout();
            this._split.Panel2.SuspendLayout();
            this._split.SuspendLayout();
            this._tabs.SuspendLayout();
            this._tabList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._grid)).BeginInit();
            this._tabYaml.SuspendLayout();
            this._tabLogs.SuspendLayout();
            this._tabEvents.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._gridEvents)).BeginInit();
            this._status.SuspendLayout();
            this._topPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // _split
            // 
            this._split.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._split.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this._split.IsSplitterFixed = false;
            this._split.Location = new System.Drawing.Point(0, 130);
            this._split.Name = "_split";
            this._split.Orientation = System.Windows.Forms.Orientation.Vertical;
            // 
            // _split.Panel1
            // 
            this._split.Panel1.Controls.Add(this._explorer);
            // 
            // _split.Panel2
            // 
            this._split.Panel2.Controls.Add(this._tabs);
            this._split.Size = new System.Drawing.Size(1400, 744);
            this._split.SplitterDistance = 320;
            this._split.SplitterWidth = 3;
            this._split.TabIndex = 0;
            // 
            // _explorer
            // 
            this._explorer.AutoArrange = false;
            this._explorer.BackColor = System.Drawing.Color.White;
            this._explorer.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._explorer.Cursor = System.Windows.Forms.Cursors.Hand;
            this._explorer.Depth = 0;
            this._explorer.Dock = System.Windows.Forms.DockStyle.Fill;
            this._explorer.Font = new System.Drawing.Font("Segoe UI", 9F);
            this._explorer.ForeColor = System.Drawing.Color.Black;
            this._explorer.FullRowSelect = true;
            this._explorer.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this._explorer.HideSelection = false;
            this._explorer.HoverSelection = false;
            this._explorer.Location = new System.Drawing.Point(0, 0);
            this._explorer.Margin = new System.Windows.Forms.Padding(0);
            this._explorer.MouseLocation = new System.Drawing.Point(-1, -1);
            this._explorer.MouseState = MaterialSkin.MouseState.OUT;
            this._explorer.MultiSelect = false;
            this._explorer.Name = "_explorer";
            this._explorer.OwnerDraw = true;
            this._explorer.Padding = new System.Windows.Forms.Padding(0);
            this._explorer.Size = new System.Drawing.Size(320, 874);
            this._explorer.TabIndex = 0;
            this._explorer.UseCompatibleStateImageBehavior = false;
            this._explorer.View = System.Windows.Forms.View.Details;
            // 
            // _tabs
            // 
            this._tabs.Controls.Add(this._tabList);
            this._tabs.Controls.Add(this._tabYaml);
            this._tabs.Controls.Add(this._tabLogs);
            this._tabs.Controls.Add(this._tabEvents);
            this._tabs.Controls.Add(this._tabMetrics);
            this._tabs.Depth = 0;
            this._tabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tabs.Location = new System.Drawing.Point(0, 0);
            this._tabs.MouseState = MaterialSkin.MouseState.HOVER;
            this._tabs.Multiline = true;
            this._tabs.Name = "_tabs";
            this._tabs.SelectedIndex = 0;
            this._tabs.Size = new System.Drawing.Size(1077, 874);
            this._tabs.TabIndex = 0;
            // 
            // _tabList
            // 
            this._tabList.Controls.Add(this._grid);
            this._tabList.Location = new System.Drawing.Point(4, 29);
            this._tabList.Name = "_tabList";
            this._tabList.Padding = new System.Windows.Forms.Padding(3);
            this._tabList.Size = new System.Drawing.Size(1069, 841);
            this._tabList.TabIndex = 0;
            this._tabList.Text = "List";
            this._tabList.UseVisualStyleBackColor = true;
            // 
            // _grid
            // 
            this._grid.AllowUserToAddRows = false;
            this._grid.AllowUserToDeleteRows = false;
            this._grid.AllowUserToResizeRows = false;
            this._grid.AutoGenerateColumns = true;
            this._grid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this._grid.ColumnHeadersHeight = 40;
            this._grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this._grid.DefaultCellStyle.Padding = new System.Windows.Forms.Padding(5);
            this._grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this._grid.Location = new System.Drawing.Point(3, 3);
            this._grid.MultiSelect = false;
            this._grid.Name = "_grid";
            this._grid.ReadOnly = true;
            this._grid.RowHeadersVisible = false;
            this._grid.RowHeadersWidth = 51;
            this._grid.RowTemplate.Height = 35;
            this._grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this._grid.Size = new System.Drawing.Size(1063, 835);
            this._grid.TabIndex = 0;
            // 
            // _tabYaml
            // 
            this._tabYaml.Controls.Add(this._rtYaml);
            this._tabYaml.Location = new System.Drawing.Point(4, 29);
            this._tabYaml.Name = "_tabYaml";
            this._tabYaml.Padding = new System.Windows.Forms.Padding(3);
            this._tabYaml.Size = new System.Drawing.Size(1069, 841);
            this._tabYaml.TabIndex = 1;
            this._tabYaml.Text = "YAML";
            this._tabYaml.UseVisualStyleBackColor = true;
            // 
            // _rtYaml
            // 
            this._rtYaml.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._rtYaml.Dock = System.Windows.Forms.DockStyle.Fill;
            this._rtYaml.Font = new System.Drawing.Font("Consolas", 10F);
            this._rtYaml.Location = new System.Drawing.Point(3, 3);
            this._rtYaml.Name = "_rtYaml";
            this._rtYaml.Size = new System.Drawing.Size(1063, 835);
            this._rtYaml.TabIndex = 0;
            this._rtYaml.Text = "";
            // 
            // _tabLogs
            // 
            this._tabLogs.Controls.Add(this._txtLogs);
            this._tabLogs.Location = new System.Drawing.Point(4, 29);
            this._tabLogs.Name = "_tabLogs";
            this._tabLogs.Padding = new System.Windows.Forms.Padding(3);
            this._tabLogs.Size = new System.Drawing.Size(1069, 841);
            this._tabLogs.TabIndex = 2;
            this._tabLogs.Text = "Logs";
            this._tabLogs.UseVisualStyleBackColor = true;
            // 
            // _txtLogs
            // 
            this._txtLogs.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._txtLogs.Dock = System.Windows.Forms.DockStyle.Fill;
            this._txtLogs.Font = new System.Drawing.Font("Consolas", 10F);
            this._txtLogs.Location = new System.Drawing.Point(0, 0);
            this._txtLogs.Multiline = true;
            this._txtLogs.Name = "_txtLogs";
            this._txtLogs.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this._txtLogs.Size = new System.Drawing.Size(1069, 841);
            this._txtLogs.TabIndex = 0;
            // 
            // _tabEvents
            // 
            this._tabEvents.Controls.Add(this._gridEvents);
            this._tabEvents.Location = new System.Drawing.Point(4, 29);
            this._tabEvents.Name = "_tabEvents";
            this._tabEvents.Padding = new System.Windows.Forms.Padding(3);
            this._tabEvents.Size = new System.Drawing.Size(1069, 841);
            this._tabEvents.TabIndex = 3;
            this._tabEvents.Text = "Events";
            this._tabEvents.UseVisualStyleBackColor = true;
            // 
            // _gridEvents
            // 
            this._gridEvents.AllowUserToAddRows = false;
            this._gridEvents.AllowUserToDeleteRows = false;
            this._gridEvents.AutoGenerateColumns = true;
            this._gridEvents.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this._gridEvents.ColumnHeadersHeight = 40;
            this._gridEvents.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this._gridEvents.DefaultCellStyle.Padding = new System.Windows.Forms.Padding(5);
            this._gridEvents.Dock = System.Windows.Forms.DockStyle.Fill;
            this._gridEvents.Location = new System.Drawing.Point(0, 0);
            this._gridEvents.MultiSelect = false;
            this._gridEvents.Name = "_gridEvents";
            this._gridEvents.ReadOnly = true;
            this._gridEvents.RowHeadersVisible = false;
            this._gridEvents.RowHeadersWidth = 51;
            this._gridEvents.RowTemplate.Height = 35;
            this._gridEvents.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this._gridEvents.Size = new System.Drawing.Size(1069, 841);
            this._gridEvents.TabIndex = 0;
            // 
            // _tabMetrics
            // 
            this._tabMetrics.Location = new System.Drawing.Point(4, 29);
            this._tabMetrics.Name = "_tabMetrics";
            this._tabMetrics.Size = new System.Drawing.Size(1069, 841);
            this._tabMetrics.TabIndex = 4;
            this._tabMetrics.Text = "Metrics";
            this._tabMetrics.UseVisualStyleBackColor = true;
            // 
            // _status
            // 
            this._status.ImageScalingSize = new System.Drawing.Size(20, 20);
            this._status.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._statusInfo,
            this._statusNs,
            this._statusKind});
            this._status.Location = new System.Drawing.Point(0, 874);
            this._status.Name = "_status";
            this._status.Size = new System.Drawing.Size(1400, 26);
            this._status.TabIndex = 1;
            // 
            // _statusInfo
            // 
            this._statusInfo.Name = "_statusInfo";
            this._statusInfo.Size = new System.Drawing.Size(1310, 20);
            this._statusInfo.Spring = true;
            this._statusInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _statusNs
            // 
            this._statusNs.Name = "_statusNs";
            this._statusNs.Size = new System.Drawing.Size(35, 20);
            this._statusNs.Text = "ns: -";
            // 
            // _statusKind
            // 
            this._statusKind.Name = "_statusKind";
            this._statusKind.Size = new System.Drawing.Size(40, 20);
            this._statusKind.Text = "kind: Pods";
            // 
            // MainViewLens
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.ClientSize = new System.Drawing.Size(1400, 900);
            this.Controls.Add(this._split);
            this.Controls.Add(this._status);
            this.Controls.Add(this._topPanel);
            this.MinimumSize = new System.Drawing.Size(1200, 700);
            this.Name = "MainViewLens";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "K8s Manager — Lens Desktop";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this._split.Panel1.ResumeLayout(false);
            this._split.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._split)).EndInit();
            this._split.ResumeLayout(false);
            this._tabs.ResumeLayout(false);
            this._tabList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._grid)).EndInit();
            this._tabYaml.ResumeLayout(false);
            this._tabLogs.ResumeLayout(false);
            this._tabLogs.PerformLayout();
            this._tabEvents.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._gridEvents)).EndInit();
            this._status.ResumeLayout(false);
            this._status.PerformLayout();
            this._topPanel.ResumeLayout(false);
            this._topPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
