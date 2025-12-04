namespace K8sManager.Views
{
    partial class WorkloadPodsDialog
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

        private void InitializeComponent()
        {
            this._listPods = new System.Windows.Forms.ListView();
            this._bottomPanel = new System.Windows.Forms.Panel();
            this._btnClose = new MaterialSkin.Controls.MaterialButton();
            this._btnShell = new MaterialSkin.Controls.MaterialButton();
            this._btnLogs = new MaterialSkin.Controls.MaterialButton();
            this._btnRefresh = new MaterialSkin.Controls.MaterialButton();
            this._bottomPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // _listPods
            // 
            this._listPods.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            new System.Windows.Forms.ColumnHeader { Text = "Name", Width = 300 },
            new System.Windows.Forms.ColumnHeader { Text = "Status", Width = 120 },
            new System.Windows.Forms.ColumnHeader { Text = "Ready", Width = 80 },
            new System.Windows.Forms.ColumnHeader { Text = "Restarts", Width = 80 },
            new System.Windows.Forms.ColumnHeader { Text = "Age", Width = 120 },
            new System.Windows.Forms.ColumnHeader { Text = "IP", Width = 120 }});
            this._listPods.Dock = System.Windows.Forms.DockStyle.Fill;
            this._listPods.Font = new System.Drawing.Font("Segoe UI", 10F);
            this._listPods.FullRowSelect = true;
            this._listPods.GridLines = true;
            this._listPods.Location = new System.Drawing.Point(0, 0);
            this._listPods.Name = "_listPods";
            this._listPods.Size = new System.Drawing.Size(900, 440);
            this._listPods.TabIndex = 0;
            this._listPods.UseCompatibleStateImageBehavior = false;
            this._listPods.View = System.Windows.Forms.View.Details;
            // 
            // _bottomPanel
            // 
            this._bottomPanel.Controls.Add(this._btnClose);
            this._bottomPanel.Controls.Add(this._btnShell);
            this._bottomPanel.Controls.Add(this._btnLogs);
            this._bottomPanel.Controls.Add(this._btnRefresh);
            this._bottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._bottomPanel.Location = new System.Drawing.Point(0, 440);
            this._bottomPanel.Name = "_bottomPanel";
            this._bottomPanel.Padding = new System.Windows.Forms.Padding(10);
            this._bottomPanel.Size = new System.Drawing.Size(900, 60);
            this._bottomPanel.TabIndex = 1;
            // 
            // _btnClose
            // 
            this._btnClose.AutoSize = false;
            this._btnClose.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._btnClose.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this._btnClose.Depth = 0;
            this._btnClose.HighEmphasis = true;
            this._btnClose.Icon = null;
            this._btnClose.Location = new System.Drawing.Point(360, 12);
            this._btnClose.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this._btnClose.MouseState = MaterialSkin.MouseState.HOVER;
            this._btnClose.Name = "_btnClose";
            this._btnClose.NoAccentTextColor = System.Drawing.Color.Empty;
            this._btnClose.Size = new System.Drawing.Size(100, 36);
            this._btnClose.TabIndex = 3;
            this._btnClose.Text = "Close";
            this._btnClose.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this._btnClose.UseAccentColor = false;
            this._btnClose.UseVisualStyleBackColor = true;
            // 
            // _btnShell
            // 
            this._btnShell.AutoSize = false;
            this._btnShell.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._btnShell.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this._btnShell.Depth = 0;
            this._btnShell.Enabled = false;
            this._btnShell.HighEmphasis = true;
            this._btnShell.Icon = null;
            this._btnShell.Location = new System.Drawing.Point(250, 12);
            this._btnShell.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this._btnShell.MouseState = MaterialSkin.MouseState.HOVER;
            this._btnShell.Name = "_btnShell";
            this._btnShell.NoAccentTextColor = System.Drawing.Color.Empty;
            this._btnShell.Size = new System.Drawing.Size(100, 36);
            this._btnShell.TabIndex = 2;
            this._btnShell.Text = "Shell";
            this._btnShell.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this._btnShell.UseAccentColor = false;
            this._btnShell.UseVisualStyleBackColor = true;
            // 
            // _btnLogs
            // 
            this._btnLogs.AutoSize = false;
            this._btnLogs.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._btnLogs.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this._btnLogs.Depth = 0;
            this._btnLogs.Enabled = false;
            this._btnLogs.HighEmphasis = true;
            this._btnLogs.Icon = null;
            this._btnLogs.Location = new System.Drawing.Point(120, 12);
            this._btnLogs.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this._btnLogs.MouseState = MaterialSkin.MouseState.HOVER;
            this._btnLogs.Name = "_btnLogs";
            this._btnLogs.NoAccentTextColor = System.Drawing.Color.Empty;
            this._btnLogs.Size = new System.Drawing.Size(120, 36);
            this._btnLogs.TabIndex = 1;
            this._btnLogs.Text = "View Logs";
            this._btnLogs.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this._btnLogs.UseAccentColor = false;
            this._btnLogs.UseVisualStyleBackColor = true;
            // 
            // _btnRefresh
            // 
            this._btnRefresh.AutoSize = false;
            this._btnRefresh.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._btnRefresh.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this._btnRefresh.Depth = 0;
            this._btnRefresh.HighEmphasis = true;
            this._btnRefresh.Icon = null;
            this._btnRefresh.Location = new System.Drawing.Point(10, 12);
            this._btnRefresh.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this._btnRefresh.MouseState = MaterialSkin.MouseState.HOVER;
            this._btnRefresh.Name = "_btnRefresh";
            this._btnRefresh.NoAccentTextColor = System.Drawing.Color.Empty;
            this._btnRefresh.Size = new System.Drawing.Size(100, 36);
            this._btnRefresh.TabIndex = 0;
            this._btnRefresh.Text = "Refresh";
            this._btnRefresh.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this._btnRefresh.UseAccentColor = false;
            this._btnRefresh.UseVisualStyleBackColor = true;
            // 
            // WorkloadPodsDialog
            // 
            this.ClientSize = new System.Drawing.Size(900, 500);
            this.Controls.Add(this._listPods);
            this.Controls.Add(this._bottomPanel);
            this.MaximizeBox = true;
            this.MinimumSize = new System.Drawing.Size(700, 400);
            this.Name = "WorkloadPodsDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this._bottomPanel.ResumeLayout(false);
            this.ResumeLayout(false);
        }
    }
}
