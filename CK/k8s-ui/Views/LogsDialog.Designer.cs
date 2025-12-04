namespace K8sManager.Views
{
    partial class LogsDialog
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
            this._txtLogs = new System.Windows.Forms.TextBox();
            this._topPanel = new System.Windows.Forms.Panel();
            this._btnClose = new MaterialSkin.Controls.MaterialButton();
            this._btnRefresh = new MaterialSkin.Controls.MaterialButton();
            this._numTailLines = new System.Windows.Forms.NumericUpDown();
            this._lblTailLines = new System.Windows.Forms.Label();
            this._cmbContainer = new System.Windows.Forms.ComboBox();
            this._lblContainer = new System.Windows.Forms.Label();
            this._topPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._numTailLines)).BeginInit();
            this.SuspendLayout();
            // 
            // _txtLogs
            // 
            this._txtLogs.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this._txtLogs.Dock = System.Windows.Forms.DockStyle.Fill;
            this._txtLogs.Font = new System.Drawing.Font("Consolas", 9.5F);
            this._txtLogs.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this._txtLogs.Location = new System.Drawing.Point(0, 60);
            this._txtLogs.Multiline = true;
            this._txtLogs.Name = "_txtLogs";
            this._txtLogs.ReadOnly = true;
            this._txtLogs.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this._txtLogs.Size = new System.Drawing.Size(900, 540);
            this._txtLogs.TabIndex = 0;
            this._txtLogs.WordWrap = false;
            // 
            // _topPanel
            // 
            this._topPanel.Controls.Add(this._btnClose);
            this._topPanel.Controls.Add(this._btnRefresh);
            this._topPanel.Controls.Add(this._numTailLines);
            this._topPanel.Controls.Add(this._lblTailLines);
            this._topPanel.Controls.Add(this._cmbContainer);
            this._topPanel.Controls.Add(this._lblContainer);
            this._topPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this._topPanel.Location = new System.Drawing.Point(0, 0);
            this._topPanel.Name = "_topPanel";
            this._topPanel.Padding = new System.Windows.Forms.Padding(10);
            this._topPanel.Size = new System.Drawing.Size(900, 60);
            this._topPanel.TabIndex = 1;
            // 
            // _btnClose
            // 
            this._btnClose.AutoSize = false;
            this._btnClose.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._btnClose.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this._btnClose.Depth = 0;
            this._btnClose.HighEmphasis = true;
            this._btnClose.Icon = null;
            this._btnClose.Location = new System.Drawing.Point(620, 10);
            this._btnClose.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this._btnClose.MouseState = MaterialSkin.MouseState.HOVER;
            this._btnClose.Name = "_btnClose";
            this._btnClose.NoAccentTextColor = System.Drawing.Color.Empty;
            this._btnClose.Size = new System.Drawing.Size(100, 36);
            this._btnClose.TabIndex = 5;
            this._btnClose.Text = "Close";
            this._btnClose.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this._btnClose.UseAccentColor = false;
            this._btnClose.UseVisualStyleBackColor = true;
            // 
            // _btnRefresh
            // 
            this._btnRefresh.AutoSize = false;
            this._btnRefresh.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._btnRefresh.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this._btnRefresh.Depth = 0;
            this._btnRefresh.HighEmphasis = true;
            this._btnRefresh.Icon = null;
            this._btnRefresh.Location = new System.Drawing.Point(510, 10);
            this._btnRefresh.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this._btnRefresh.MouseState = MaterialSkin.MouseState.HOVER;
            this._btnRefresh.Name = "_btnRefresh";
            this._btnRefresh.NoAccentTextColor = System.Drawing.Color.Empty;
            this._btnRefresh.Size = new System.Drawing.Size(100, 36);
            this._btnRefresh.TabIndex = 4;
            this._btnRefresh.Text = "Refresh";
            this._btnRefresh.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this._btnRefresh.UseAccentColor = false;
            this._btnRefresh.UseVisualStyleBackColor = true;
            // 
            // _numTailLines
            // 
            this._numTailLines.Increment = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this._numTailLines.Location = new System.Drawing.Point(395, 12);
            this._numTailLines.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this._numTailLines.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this._numTailLines.Name = "_numTailLines";
            this._numTailLines.Size = new System.Drawing.Size(100, 27);
            this._numTailLines.TabIndex = 3;
            this._numTailLines.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // _lblTailLines
            // 
            this._lblTailLines.AutoSize = true;
            this._lblTailLines.Location = new System.Drawing.Point(320, 15);
            this._lblTailLines.Name = "_lblTailLines";
            this._lblTailLines.Size = new System.Drawing.Size(81, 20);
            this._lblTailLines.TabIndex = 2;
            this._lblTailLines.Text = "Tail Lines:";
            // 
            // _cmbContainer
            // 
            this._cmbContainer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cmbContainer.FormattingEnabled = true;
            this._cmbContainer.Location = new System.Drawing.Point(100, 12);
            this._cmbContainer.Name = "_cmbContainer";
            this._cmbContainer.Size = new System.Drawing.Size(200, 28);
            this._cmbContainer.TabIndex = 1;
            // 
            // _lblContainer
            // 
            this._lblContainer.AutoSize = true;
            this._lblContainer.Location = new System.Drawing.Point(10, 15);
            this._lblContainer.Name = "_lblContainer";
            this._lblContainer.Size = new System.Drawing.Size(84, 20);
            this._lblContainer.TabIndex = 0;
            this._lblContainer.Text = "Container:";
            // 
            // LogsDialog
            // 
            this.ClientSize = new System.Drawing.Size(900, 600);
            this.Controls.Add(this._txtLogs);
            this.Controls.Add(this._topPanel);
            this.MaximizeBox = true;
            this.MinimumSize = new System.Drawing.Size(600, 400);
            this.Name = "LogsDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this._topPanel.ResumeLayout(false);
            this._topPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._numTailLines)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
