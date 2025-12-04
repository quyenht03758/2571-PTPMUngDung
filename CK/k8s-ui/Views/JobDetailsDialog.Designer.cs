namespace K8sManager.Views
{
    partial class JobDetailsDialog
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

        private System.Windows.Forms.Panel _topPanel;
        private System.Windows.Forms.Label _lblTitle;
        private System.Windows.Forms.Button _btnClose;
        private System.Windows.Forms.Button _btnRefresh;
        private System.Windows.Forms.TabControl _tabControl;
        private System.Windows.Forms.TabPage _tabOverview;
        private System.Windows.Forms.TabPage _tabPods;
        private System.Windows.Forms.TabPage _tabYaml;
        private System.Windows.Forms.DataGridView _gridPods;
        private System.Windows.Forms.RichTextBox _txtYaml;
        private System.Windows.Forms.TableLayoutPanel _detailsPanel;

        private void InitializeComponent()
        {
            this._topPanel = new System.Windows.Forms.Panel();
            this._lblTitle = new System.Windows.Forms.Label();
            this._btnRefresh = new System.Windows.Forms.Button();
            this._btnClose = new System.Windows.Forms.Button();
            this._tabControl = new System.Windows.Forms.TabControl();
            this._tabOverview = new System.Windows.Forms.TabPage();
            this._detailsPanel = new System.Windows.Forms.TableLayoutPanel();
            this._tabPods = new System.Windows.Forms.TabPage();
            this._gridPods = new System.Windows.Forms.DataGridView();
            this._tabYaml = new System.Windows.Forms.TabPage();
            this._txtYaml = new System.Windows.Forms.RichTextBox();
            this._topPanel.SuspendLayout();
            this._tabControl.SuspendLayout();
            this._tabOverview.SuspendLayout();
            this._tabPods.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._gridPods)).BeginInit();
            this._tabYaml.SuspendLayout();
            this.SuspendLayout();
            // 
            // _topPanel
            // 
            this._topPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(115)))), ((int)(((byte)(220)))));
            this._topPanel.Controls.Add(this._lblTitle);
            this._topPanel.Controls.Add(this._btnRefresh);
            this._topPanel.Controls.Add(this._btnClose);
            this._topPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this._topPanel.Location = new System.Drawing.Point(0, 0);
            this._topPanel.Name = "_topPanel";
            this._topPanel.Size = new System.Drawing.Size(900, 70);
            this._topPanel.TabIndex = 0;
            // 
            // _lblTitle
            // 
            this._lblTitle.AutoSize = true;
            this._lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this._lblTitle.ForeColor = System.Drawing.Color.White;
            this._lblTitle.Location = new System.Drawing.Point(20, 20);
            this._lblTitle.Name = "_lblTitle";
            this._lblTitle.Size = new System.Drawing.Size(150, 30);
            this._lblTitle.TabIndex = 0;
            this._lblTitle.Text = "Job Details";
            // 
            // _btnRefresh
            // 
            this._btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._btnRefresh.BackColor = System.Drawing.Color.White;
            this._btnRefresh.FlatAppearance.BorderSize = 0;
            this._btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._btnRefresh.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this._btnRefresh.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(115)))), ((int)(((byte)(220)))));
            this._btnRefresh.Location = new System.Drawing.Point(700, 18);
            this._btnRefresh.Name = "_btnRefresh";
            this._btnRefresh.Size = new System.Drawing.Size(90, 35);
            this._btnRefresh.TabIndex = 1;
            this._btnRefresh.Text = "🔄 Refresh";
            this._btnRefresh.UseVisualStyleBackColor = false;
            this._btnRefresh.Click += new System.EventHandler(this.BtnRefresh_Click);
            // 
            // _btnClose
            // 
            this._btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._btnClose.BackColor = System.Drawing.Color.White;
            this._btnClose.FlatAppearance.BorderSize = 0;
            this._btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._btnClose.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this._btnClose.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(115)))), ((int)(((byte)(220)))));
            this._btnClose.Location = new System.Drawing.Point(800, 18);
            this._btnClose.Name = "_btnClose";
            this._btnClose.Size = new System.Drawing.Size(80, 35);
            this._btnClose.TabIndex = 2;
            this._btnClose.Text = "✖ Close";
            this._btnClose.UseVisualStyleBackColor = false;
            this._btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // _tabControl
            // 
            this._tabControl.Controls.Add(this._tabOverview);
            this._tabControl.Controls.Add(this._tabPods);
            this._tabControl.Controls.Add(this._tabYaml);
            this._tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tabControl.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this._tabControl.Location = new System.Drawing.Point(0, 70);
            this._tabControl.Name = "_tabControl";
            this._tabControl.SelectedIndex = 0;
            this._tabControl.Size = new System.Drawing.Size(900, 530);
            this._tabControl.TabIndex = 1;
            // 
            // _tabOverview
            // 
            this._tabOverview.BackColor = System.Drawing.Color.White;
            this._tabOverview.Controls.Add(this._detailsPanel);
            this._tabOverview.Location = new System.Drawing.Point(4, 26);
            this._tabOverview.Name = "_tabOverview";
            this._tabOverview.Padding = new System.Windows.Forms.Padding(20);
            this._tabOverview.Size = new System.Drawing.Size(892, 500);
            this._tabOverview.TabIndex = 0;
            this._tabOverview.Text = "Overview";
            // 
            // _detailsPanel
            // 
            this._detailsPanel.AutoScroll = true;
            this._detailsPanel.ColumnCount = 2;
            this._detailsPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this._detailsPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._detailsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._detailsPanel.Location = new System.Drawing.Point(20, 20);
            this._detailsPanel.Name = "_detailsPanel";
            this._detailsPanel.RowCount = 1;
            this._detailsPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._detailsPanel.Size = new System.Drawing.Size(852, 460);
            this._detailsPanel.TabIndex = 0;
            // 
            // _tabPods
            // 
            this._tabPods.BackColor = System.Drawing.Color.White;
            this._tabPods.Controls.Add(this._gridPods);
            this._tabPods.Location = new System.Drawing.Point(4, 26);
            this._tabPods.Name = "_tabPods";
            this._tabPods.Padding = new System.Windows.Forms.Padding(10);
            this._tabPods.Size = new System.Drawing.Size(892, 500);
            this._tabPods.TabIndex = 1;
            this._tabPods.Text = "Pods";
            // 
            // _gridPods
            // 
            this._gridPods.AllowUserToAddRows = false;
            this._gridPods.AllowUserToDeleteRows = false;
            this._gridPods.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this._gridPods.BackgroundColor = System.Drawing.Color.White;
            this._gridPods.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._gridPods.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._gridPods.Dock = System.Windows.Forms.DockStyle.Fill;
            this._gridPods.Location = new System.Drawing.Point(10, 10);
            this._gridPods.Name = "_gridPods";
            this._gridPods.ReadOnly = true;
            this._gridPods.RowHeadersVisible = false;
            this._gridPods.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this._gridPods.Size = new System.Drawing.Size(872, 480);
            this._gridPods.TabIndex = 0;
            // 
            // _tabYaml
            // 
            this._tabYaml.BackColor = System.Drawing.Color.White;
            this._tabYaml.Controls.Add(this._txtYaml);
            this._tabYaml.Location = new System.Drawing.Point(4, 26);
            this._tabYaml.Name = "_tabYaml";
            this._tabYaml.Padding = new System.Windows.Forms.Padding(10);
            this._tabYaml.Size = new System.Drawing.Size(892, 500);
            this._tabYaml.TabIndex = 2;
            this._tabYaml.Text = "YAML";
            // 
            // _txtYaml
            // 
            this._txtYaml.BackColor = System.Drawing.Color.White;
            this._txtYaml.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._txtYaml.DetectUrls = false;
            this._txtYaml.Dock = System.Windows.Forms.DockStyle.Fill;
            this._txtYaml.Font = new System.Drawing.Font("Consolas", 12F);
            this._txtYaml.Location = new System.Drawing.Point(10, 10);
            this._txtYaml.Name = "_txtYaml";
            this._txtYaml.ReadOnly = true;
            this._txtYaml.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Both;
            this._txtYaml.Size = new System.Drawing.Size(872, 480);
            this._txtYaml.TabIndex = 0;
            this._txtYaml.Text = "";
            this._txtYaml.WordWrap = false;
            // 
            // JobDetailsDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(900, 600);
            this.Controls.Add(this._tabControl);
            this.Controls.Add(this._topPanel);
            this.MaximizeBox = true;
            this.MinimizeBox = false;
            this.Name = "JobDetailsDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Job Details";
            this._topPanel.ResumeLayout(false);
            this._topPanel.PerformLayout();
            this._tabControl.ResumeLayout(false);
            this._tabOverview.ResumeLayout(false);
            this._tabPods.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._gridPods)).EndInit();
            this._tabYaml.ResumeLayout(false);
            this.ResumeLayout(false);
        }
    }
}
