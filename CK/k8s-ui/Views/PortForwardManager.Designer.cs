using MaterialSkin.Controls;

namespace K8sManager.Views
{
    partial class PortForwardManager
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.topPanel = new System.Windows.Forms.Panel();
            this._btnRefresh = new MaterialSkin.Controls.MaterialButton();
            this._btnAdd = new MaterialSkin.Controls.MaterialButton();
            this._lblTitle = new System.Windows.Forms.Label();
            this._grid = new System.Windows.Forms.DataGridView();
            this.Namespace = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Pod = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LocalPort = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RemotePort = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Pid = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bottomPanel = new System.Windows.Forms.Panel();
            this._lblTotal = new System.Windows.Forms.Label();
            this.rightPanel = new System.Windows.Forms.FlowLayoutPanel();
            this._btnClose = new MaterialSkin.Controls.MaterialButton();
            this._btnStop = new MaterialSkin.Controls.MaterialButton();
            this.topPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._grid)).BeginInit();
            this.bottomPanel.SuspendLayout();
            this.rightPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // topPanel
            // 
            this.topPanel.Controls.Add(this._btnRefresh);
            this.topPanel.Controls.Add(this._btnAdd);
            this.topPanel.Controls.Add(this._lblTitle);
            this.topPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.topPanel.Location = new System.Drawing.Point(0, 0);
            this.topPanel.Name = "topPanel";
            this.topPanel.Padding = new System.Windows.Forms.Padding(16, 12, 16, 12);
            this.topPanel.Size = new System.Drawing.Size(900, 70);
            this.topPanel.TabIndex = 0;
            // 
            // _btnRefresh
            // 
            this._btnRefresh.AutoSize = false;
            this._btnRefresh.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._btnRefresh.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this._btnRefresh.Depth = 0;
            this._btnRefresh.Dock = System.Windows.Forms.DockStyle.Right;
            this._btnRefresh.HighEmphasis = true;
            this._btnRefresh.Icon = null;
            this._btnRefresh.Location = new System.Drawing.Point(664, 12);
            this._btnRefresh.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this._btnRefresh.MouseState = MaterialSkin.MouseState.HOVER;
            this._btnRefresh.Name = "_btnRefresh";
            this._btnRefresh.NoAccentTextColor = System.Drawing.Color.Empty;
            this._btnRefresh.Size = new System.Drawing.Size(100, 46);
            this._btnRefresh.TabIndex = 1;
            this._btnRefresh.Text = "🔄 Refresh";
            this._btnRefresh.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Outlined;
            this._btnRefresh.UseAccentColor = false;
            this._btnRefresh.UseVisualStyleBackColor = true;
            // 
            // _btnAdd
            // 
            this._btnAdd.AutoSize = false;
            this._btnAdd.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._btnAdd.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this._btnAdd.Depth = 0;
            this._btnAdd.Dock = System.Windows.Forms.DockStyle.Right;
            this._btnAdd.HighEmphasis = true;
            this._btnAdd.Icon = null;
            this._btnAdd.Location = new System.Drawing.Point(764, 12);
            this._btnAdd.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this._btnAdd.MouseState = MaterialSkin.MouseState.HOVER;
            this._btnAdd.Name = "_btnAdd";
            this._btnAdd.NoAccentTextColor = System.Drawing.Color.Empty;
            this._btnAdd.Size = new System.Drawing.Size(120, 46);
            this._btnAdd.TabIndex = 2;
            this._btnAdd.Text = "➕ Add New";
            this._btnAdd.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this._btnAdd.UseAccentColor = false;
            this._btnAdd.UseVisualStyleBackColor = true;
            // 
            // _lblTitle
            // 
            this._lblTitle.AutoSize = true;
            this._lblTitle.Dock = System.Windows.Forms.DockStyle.Left;
            this._lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this._lblTitle.Location = new System.Drawing.Point(16, 12);
            this._lblTitle.Name = "_lblTitle";
            this._lblTitle.Padding = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this._lblTitle.Size = new System.Drawing.Size(251, 40);
            this._lblTitle.TabIndex = 0;
            this._lblTitle.Text = "🔀 Port Forward Manager";
            this._lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _grid
            // 
            this._grid.AllowUserToAddRows = false;
            this._grid.AllowUserToDeleteRows = false;
            this._grid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this._grid.BackgroundColor = System.Drawing.Color.White;
            this._grid.ColumnHeadersHeight = 40;
            this._grid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Namespace,
            this.Pod,
            this.LocalPort,
            this.RemotePort,
            this.Status,
            this.Pid});
            this._grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this._grid.Location = new System.Drawing.Point(0, 70);
            this._grid.MultiSelect = false;
            this._grid.Name = "_grid";
            this._grid.ReadOnly = true;
            this._grid.RowHeadersVisible = false;
            this._grid.RowHeadersWidth = 51;
            this._grid.RowTemplate.Height = 35;
            this._grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this._grid.Size = new System.Drawing.Size(900, 450);
            this._grid.TabIndex = 1;
            // 
            // Namespace
            // 
            this.Namespace.DataPropertyName = "Namespace";
            this.Namespace.HeaderText = "Namespace";
            this.Namespace.MinimumWidth = 6;
            this.Namespace.Name = "Namespace";
            this.Namespace.ReadOnly = true;
            // 
            // Pod
            // 
            this.Pod.DataPropertyName = "Pod";
            this.Pod.HeaderText = "Pod Name";
            this.Pod.MinimumWidth = 6;
            this.Pod.Name = "Pod";
            this.Pod.ReadOnly = true;
            // 
            // LocalPort
            // 
            this.LocalPort.DataPropertyName = "LocalPort";
            this.LocalPort.HeaderText = "Local Port";
            this.LocalPort.MinimumWidth = 6;
            this.LocalPort.Name = "LocalPort";
            this.LocalPort.ReadOnly = true;
            // 
            // RemotePort
            // 
            this.RemotePort.DataPropertyName = "RemotePort";
            this.RemotePort.HeaderText = "Remote Port";
            this.RemotePort.MinimumWidth = 6;
            this.RemotePort.Name = "RemotePort";
            this.RemotePort.ReadOnly = true;
            // 
            // Status
            // 
            this.Status.DataPropertyName = "Status";
            this.Status.HeaderText = "Status";
            this.Status.MinimumWidth = 6;
            this.Status.Name = "Status";
            this.Status.ReadOnly = true;
            // 
            // Pid
            // 
            this.Pid.DataPropertyName = "Pid";
            this.Pid.HeaderText = "Process ID";
            this.Pid.MinimumWidth = 6;
            this.Pid.Name = "Pid";
            this.Pid.ReadOnly = true;
            this.Pid.Visible = false;
            // 
            // bottomPanel
            // 
            this.bottomPanel.Controls.Add(this._lblTotal);
            this.bottomPanel.Controls.Add(this.rightPanel);
            this.bottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.bottomPanel.Location = new System.Drawing.Point(0, 520);
            this.bottomPanel.Name = "bottomPanel";
            this.bottomPanel.Padding = new System.Windows.Forms.Padding(16, 12, 16, 12);
            this.bottomPanel.Size = new System.Drawing.Size(900, 80);
            this.bottomPanel.TabIndex = 2;
            // 
            // _lblTotal
            // 
            this._lblTotal.AutoSize = true;
            this._lblTotal.Dock = System.Windows.Forms.DockStyle.Left;
            this._lblTotal.Font = new System.Drawing.Font("Segoe UI", 10F);
            this._lblTotal.Location = new System.Drawing.Point(16, 12);
            this._lblTotal.Name = "_lblTotal";
            this._lblTotal.Padding = new System.Windows.Forms.Padding(0, 12, 0, 0);
            this._lblTotal.Size = new System.Drawing.Size(149, 35);
            this._lblTotal.TabIndex = 0;
            this._lblTotal.Text = "Active forwards: 0";
            this._lblTotal.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // rightPanel
            // 
            this.rightPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rightPanel.AutoSize = true;
            this.rightPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.rightPanel.Controls.Add(this._btnClose);
            this.rightPanel.Controls.Add(this._btnStop);
            this.rightPanel.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
            this.rightPanel.Location = new System.Drawing.Point(600, 15);
            this.rightPanel.Name = "rightPanel";
            this.rightPanel.Padding = new System.Windows.Forms.Padding(4);
            this.rightPanel.TabIndex = 1;
            // 
            // _btnClose
            // 
            this._btnClose.AutoSize = false;
            this._btnClose.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._btnClose.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this._btnClose.Depth = 0;
            this._btnClose.HighEmphasis = true;
            this._btnClose.Icon = null;
            this._btnClose.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this._btnClose.MouseState = MaterialSkin.MouseState.HOVER;
            this._btnClose.Name = "_btnClose";
            this._btnClose.NoAccentTextColor = System.Drawing.Color.Empty;
            this._btnClose.Size = new System.Drawing.Size(100, 36);
            this._btnClose.TabIndex = 1;
            this._btnClose.Text = "Close";
            this._btnClose.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Text;
            this._btnClose.UseAccentColor = false;
            this._btnClose.UseVisualStyleBackColor = true;
            // 
            // _btnStop
            // 
            this._btnStop.AutoSize = false;
            this._btnStop.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._btnStop.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this._btnStop.Depth = 0;
            this._btnStop.HighEmphasis = true;
            this._btnStop.Icon = null;
            this._btnStop.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this._btnStop.MouseState = MaterialSkin.MouseState.HOVER;
            this._btnStop.Name = "_btnStop";
            this._btnStop.NoAccentTextColor = System.Drawing.Color.Empty;
            this._btnStop.Size = new System.Drawing.Size(140, 36);
            this._btnStop.TabIndex = 0;
            this._btnStop.Text = "⏹️ Stop Selected";
            this._btnStop.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this._btnStop.UseAccentColor = false;
            this._btnStop.UseVisualStyleBackColor = true;
            // 
            // PortForwardManager
            // 
            this.ClientSize = new System.Drawing.Size(900, 600);
            this.Controls.Add(this._grid);
            this.Controls.Add(this.bottomPanel);
            this.Controls.Add(this.topPanel);
            this.Name = "PortForwardManager";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Port Forward Manager";
            this.topPanel.ResumeLayout(false);
            this.topPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._grid)).EndInit();
            this.bottomPanel.ResumeLayout(false);
            this.bottomPanel.PerformLayout();
            this.rightPanel.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel topPanel;
        private MaterialSkin.Controls.MaterialButton _btnRefresh;
        private MaterialSkin.Controls.MaterialButton _btnAdd;
        private System.Windows.Forms.Label _lblTitle;
        private System.Windows.Forms.DataGridView _grid;
        private System.Windows.Forms.DataGridViewTextBoxColumn Namespace;
        private System.Windows.Forms.DataGridViewTextBoxColumn Pod;
        private System.Windows.Forms.DataGridViewTextBoxColumn LocalPort;
        private System.Windows.Forms.DataGridViewTextBoxColumn RemotePort;
        private System.Windows.Forms.DataGridViewTextBoxColumn Status;
        private System.Windows.Forms.DataGridViewTextBoxColumn Pid;
        private System.Windows.Forms.Panel bottomPanel;
        private System.Windows.Forms.Label _lblTotal;
        private System.Windows.Forms.FlowLayoutPanel rightPanel;
        private MaterialSkin.Controls.MaterialButton _btnClose;
        private MaterialSkin.Controls.MaterialButton _btnStop;
    }
}
