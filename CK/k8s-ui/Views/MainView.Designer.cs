using MaterialSkin.Controls;

namespace K8sManager.Views
{
    partial class MainView
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            _topPanel = new TableLayoutPanel();
            _cbNamespace = new MaterialComboBox();
            _txtSearch = new MaterialTextBox2();
            _btnRefresh = new MaterialButton();
            _btnScale = new MaterialButton();
            _btnRestart = new MaterialButton();
            _btnTail = new MaterialButton();
            _btnTheme = new MaterialButton();
            _split = new SplitContainer();
            _grid = new DataGridView();
            _tabs = new MaterialTabControl();
            _tabYaml = new TabPage();
            _txtYaml = new RichTextBox();
            _tabDescribe = new TabPage();
            _txtDescribe = new RichTextBox();
            _tabLogs = new TabPage();
            _txtLogs = new TextBox();
            _status = new StatusStrip();
            _statusContext = new ToolStripStatusLabel();
            _statusInfo = new ToolStripStatusLabel();
            _statusLatency = new ToolStripStatusLabel();
            _topPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_split).BeginInit();
            _split.Panel1.SuspendLayout();
            _split.Panel2.SuspendLayout();
            _split.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_grid).BeginInit();
            _tabs.SuspendLayout();
            _tabYaml.SuspendLayout();
            _tabDescribe.SuspendLayout();
            _tabLogs.SuspendLayout();
            _status.SuspendLayout();
            SuspendLayout();
            // 
            // _topPanel
            // 
            _topPanel.ColumnCount = 7;
            _topPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 220F));
            _topPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 320F));
            _topPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            _topPanel.ColumnStyles.Add(new ColumnStyle());
            _topPanel.ColumnStyles.Add(new ColumnStyle());
            _topPanel.ColumnStyles.Add(new ColumnStyle());
            _topPanel.ColumnStyles.Add(new ColumnStyle());
            _topPanel.Controls.Add(_cbNamespace, 0, 0);
            _topPanel.Controls.Add(_txtSearch, 1, 0);
            _topPanel.Controls.Add(_btnRefresh, 3, 0);
            _topPanel.Controls.Add(_btnScale, 4, 0);
            _topPanel.Controls.Add(_btnRestart, 5, 0);
            _topPanel.Controls.Add(_btnTail, 6, 0);
            _topPanel.Controls.Add(_btnTheme, 6, 1);
            _topPanel.Dock = DockStyle.Top;
            _topPanel.Location = new Point(3, 64);
            _topPanel.Name = "_topPanel";
            _topPanel.Padding = new Padding(12, 8, 12, 8);
            _topPanel.RowCount = 2;
            _topPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            _topPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            _topPanel.Size = new Size(1194, 80);
            _topPanel.TabIndex = 0;
            // 
            // _cbNamespace
            // 
            _cbNamespace.AutoResize = false;
            _cbNamespace.BackColor = Color.FromArgb(255, 255, 255);
            _cbNamespace.Depth = 0;
            _cbNamespace.Dock = DockStyle.Fill;
            _cbNamespace.DrawMode = DrawMode.OwnerDrawVariable;
            _cbNamespace.DropDownHeight = 174;
            _cbNamespace.DropDownStyle = ComboBoxStyle.DropDownList;
            _cbNamespace.DropDownWidth = 121;
            _cbNamespace.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Bold, GraphicsUnit.Pixel);
            _cbNamespace.ForeColor = Color.FromArgb(222, 0, 0, 0);
            _cbNamespace.FormattingEnabled = true;
            _cbNamespace.Hint = "Namespace";
            _cbNamespace.IntegralHeight = false;
            _cbNamespace.ItemHeight = 43;
            _cbNamespace.Location = new Point(15, 11);
            _cbNamespace.MaxDropDownItems = 4;
            _cbNamespace.MouseState = MaterialSkin.MouseState.OUT;
            _cbNamespace.Name = "_cbNamespace";
            _topPanel.SetRowSpan(_cbNamespace, 2);
            _cbNamespace.Size = new Size(214, 49);
            _cbNamespace.StartIndex = 0;
            _cbNamespace.TabIndex = 0;
            // 
            // _txtSearch
            // 
            _txtSearch.AnimateReadOnly = false;
            _txtSearch.BackgroundImageLayout = ImageLayout.None;
            _txtSearch.CharacterCasing = CharacterCasing.Normal;
            _txtSearch.Depth = 0;
            _txtSearch.Dock = DockStyle.Fill;
            _txtSearch.Font = new Font("Microsoft Sans Serif", 16F, FontStyle.Regular, GraphicsUnit.Pixel);
            _txtSearch.HideSelection = true;
            _txtSearch.Hint = "Search name/label…";
            _txtSearch.LeadingIcon = null;
            _txtSearch.Location = new Point(235, 11);
            _txtSearch.MaxLength = 32767;
            _txtSearch.MouseState = MaterialSkin.MouseState.OUT;
            _txtSearch.Name = "_txtSearch";
            _txtSearch.PasswordChar = '\0';
            _txtSearch.PrefixSuffixText = null;
            _txtSearch.ReadOnly = false;
            _txtSearch.RightToLeft = RightToLeft.No;
            _topPanel.SetRowSpan(_txtSearch, 2);
            _txtSearch.SelectedText = "";
            _txtSearch.SelectionLength = 0;
            _txtSearch.SelectionStart = 0;
            _txtSearch.ShortcutsEnabled = true;
            _txtSearch.Size = new Size(314, 36);
            _txtSearch.TabIndex = 1;
            _txtSearch.TabStop = false;
            _txtSearch.TextAlign = HorizontalAlignment.Left;
            _txtSearch.TrailingIcon = null;
            _txtSearch.UseSystemPasswordChar = false;
            _txtSearch.UseTallSize = false;
            // 
            // _btnRefresh
            // 
            _btnRefresh.AutoSize = false;
            _btnRefresh.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _btnRefresh.Density = MaterialButton.MaterialButtonDensity.Default;
            _btnRefresh.Depth = 0;
            _btnRefresh.HighEmphasis = true;
            _btnRefresh.Icon = null;
            _btnRefresh.Location = new Point(794, 12);
            _btnRefresh.Margin = new Padding(4);
            _btnRefresh.MouseState = MaterialSkin.MouseState.HOVER;
            _btnRefresh.Name = "_btnRefresh";
            _btnRefresh.NoAccentTextColor = Color.Empty;
            _btnRefresh.Size = new Size(90, 24);
            _btnRefresh.TabIndex = 2;
            _btnRefresh.Text = "Refresh";
            _btnRefresh.Type = MaterialButton.MaterialButtonType.Contained;
            _btnRefresh.UseAccentColor = false;
            _btnRefresh.UseVisualStyleBackColor = true;
            // 
            // _btnScale
            // 
            _btnScale.AutoSize = false;
            _btnScale.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _btnScale.Density = MaterialButton.MaterialButtonDensity.Default;
            _btnScale.Depth = 0;
            _btnScale.HighEmphasis = true;
            _btnScale.Icon = null;
            _btnScale.Location = new Point(892, 12);
            _btnScale.Margin = new Padding(4);
            _btnScale.MouseState = MaterialSkin.MouseState.HOVER;
            _btnScale.Name = "_btnScale";
            _btnScale.NoAccentTextColor = Color.Empty;
            _btnScale.Size = new Size(80, 24);
            _btnScale.TabIndex = 3;
            _btnScale.Text = "Scale";
            _btnScale.Type = MaterialButton.MaterialButtonType.Contained;
            _btnScale.UseAccentColor = false;
            _btnScale.UseVisualStyleBackColor = true;
            // 
            // _btnRestart
            // 
            _btnRestart.AutoSize = false;
            _btnRestart.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _btnRestart.Density = MaterialButton.MaterialButtonDensity.Default;
            _btnRestart.Depth = 0;
            _btnRestart.HighEmphasis = true;
            _btnRestart.Icon = null;
            _btnRestart.Location = new Point(980, 12);
            _btnRestart.Margin = new Padding(4);
            _btnRestart.MouseState = MaterialSkin.MouseState.HOVER;
            _btnRestart.Name = "_btnRestart";
            _btnRestart.NoAccentTextColor = Color.Empty;
            _btnRestart.Size = new Size(90, 24);
            _btnRestart.TabIndex = 4;
            _btnRestart.Text = "Restart";
            _btnRestart.Type = MaterialButton.MaterialButtonType.Contained;
            _btnRestart.UseAccentColor = false;
            _btnRestart.UseVisualStyleBackColor = true;
            // 
            // _btnTail
            // 
            _btnTail.AutoSize = false;
            _btnTail.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _btnTail.Density = MaterialButton.MaterialButtonDensity.Default;
            _btnTail.Depth = 0;
            _btnTail.HighEmphasis = true;
            _btnTail.Icon = null;
            _btnTail.Location = new Point(1078, 12);
            _btnTail.Margin = new Padding(4);
            _btnTail.MouseState = MaterialSkin.MouseState.HOVER;
            _btnTail.Name = "_btnTail";
            _btnTail.NoAccentTextColor = Color.Empty;
            _btnTail.Size = new Size(100, 24);
            _btnTail.TabIndex = 5;
            _btnTail.Text = "Tail Logs";
            _btnTail.Type = MaterialButton.MaterialButtonType.Contained;
            _btnTail.UseAccentColor = false;
            _btnTail.UseVisualStyleBackColor = true;
            // 
            // _btnTheme
            // 
            _btnTheme.AutoSize = false;
            _btnTheme.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _btnTheme.Density = MaterialButton.MaterialButtonDensity.Default;
            _btnTheme.Depth = 0;
            _btnTheme.HighEmphasis = true;
            _btnTheme.Icon = null;
            _btnTheme.Location = new Point(1078, 44);
            _btnTheme.Margin = new Padding(4);
            _btnTheme.MouseState = MaterialSkin.MouseState.HOVER;
            _btnTheme.Name = "_btnTheme";
            _btnTheme.NoAccentTextColor = Color.Empty;
            _btnTheme.Size = new Size(100, 24);
            _btnTheme.TabIndex = 6;
            _btnTheme.Text = "Dark";
            _btnTheme.Type = MaterialButton.MaterialButtonType.Contained;
            _btnTheme.UseAccentColor = false;
            _btnTheme.UseVisualStyleBackColor = true;
            // 
            // _split
            // 
            _split.Dock = DockStyle.Fill;
            _split.Location = new Point(3, 144);
            _split.Name = "_split";
            // 
            // _split.Panel1
            // 
            _split.Panel1.Controls.Add(_grid);
            // 
            // _split.Panel2
            // 
            _split.Panel2.Controls.Add(_tabs);
            _split.Size = new Size(1194, 581);
            _split.SplitterDistance = 597;
            _split.SplitterWidth = 6;
            _split.TabIndex = 1;
            // 
            // _grid
            // 
            _grid.AllowUserToAddRows = false;
            _grid.AllowUserToDeleteRows = false;
            _grid.BackgroundColor = Color.White;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = Color.WhiteSmoke;
            dataGridViewCellStyle1.Font = new Font("Segoe UI Semibold", 9F);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            _grid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            _grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = SystemColors.Window;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle2.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            _grid.DefaultCellStyle = dataGridViewCellStyle2;
            _grid.Dock = DockStyle.Fill;
            _grid.EnableHeadersVisualStyles = false;
            _grid.GridColor = Color.FromArgb(230, 230, 230);
            _grid.Location = new Point(0, 0);
            _grid.MultiSelect = false;
            _grid.Name = "_grid";
            _grid.ReadOnly = true;
            _grid.RowHeadersVisible = false;
            _grid.RowHeadersWidth = 51;
            _grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            _grid.Size = new Size(597, 581);
            _grid.TabIndex = 0;
            // 
            // _tabs
            // 
            _tabs.Controls.Add(_tabYaml);
            _tabs.Controls.Add(_tabDescribe);
            _tabs.Controls.Add(_tabLogs);
            _tabs.Depth = 0;
            _tabs.Dock = DockStyle.Fill;
            _tabs.Location = new Point(0, 0);
            _tabs.MouseState = MaterialSkin.MouseState.HOVER;
            _tabs.Multiline = true;
            _tabs.Name = "_tabs";
            _tabs.SelectedIndex = 0;
            _tabs.Size = new Size(591, 581);
            _tabs.TabIndex = 0;
            // 
            // _tabYaml
            // 
            _tabYaml.Controls.Add(_txtYaml);
            _tabYaml.Location = new Point(4, 24);
            _tabYaml.Name = "_tabYaml";
            _tabYaml.Padding = new Padding(3);
            _tabYaml.Size = new Size(583, 553);
            _tabYaml.TabIndex = 0;
            _tabYaml.Text = "YAML";
            _tabYaml.UseVisualStyleBackColor = true;
            // 
            // _txtYaml
            // 
            _txtYaml.BorderStyle = BorderStyle.None;
            _txtYaml.Dock = DockStyle.Fill;
            _txtYaml.Font = new Font("Consolas", 9F);
            _txtYaml.Location = new Point(3, 3);
            _txtYaml.Name = "_txtYaml";
            _txtYaml.Size = new Size(577, 547);
            _txtYaml.TabIndex = 0;
            _txtYaml.Text = "";
            // 
            // _tabDescribe
            // 
            _tabDescribe.Controls.Add(_txtDescribe);
            _tabDescribe.Location = new Point(4, 24);
            _tabDescribe.Name = "_tabDescribe";
            _tabDescribe.Padding = new Padding(3);
            _tabDescribe.Size = new Size(583, 553);
            _tabDescribe.TabIndex = 1;
            _tabDescribe.Text = "Describe";
            _tabDescribe.UseVisualStyleBackColor = true;
            // 
            // _txtDescribe
            // 
            _txtDescribe.BorderStyle = BorderStyle.None;
            _txtDescribe.Dock = DockStyle.Fill;
            _txtDescribe.Font = new Font("Consolas", 9F);
            _txtDescribe.Location = new Point(3, 3);
            _txtDescribe.Name = "_txtDescribe";
            _txtDescribe.Size = new Size(577, 547);
            _txtDescribe.TabIndex = 0;
            _txtDescribe.Text = "";
            // 
            // _tabLogs
            // 
            _tabLogs.Controls.Add(_txtLogs);
            _tabLogs.Location = new Point(4, 24);
            _tabLogs.Name = "_tabLogs";
            _tabLogs.Padding = new Padding(3);
            _tabLogs.Size = new Size(583, 553);
            _tabLogs.TabIndex = 2;
            _tabLogs.Text = "Logs";
            _tabLogs.UseVisualStyleBackColor = true;
            // 
            // _txtLogs
            // 
            _txtLogs.BorderStyle = BorderStyle.None;
            _txtLogs.Dock = DockStyle.Fill;
            _txtLogs.Font = new Font("Consolas", 9F);
            _txtLogs.Location = new Point(3, 3);
            _txtLogs.Multiline = true;
            _txtLogs.Name = "_txtLogs";
            _txtLogs.ScrollBars = ScrollBars.Both;
            _txtLogs.Size = new Size(577, 547);
            _txtLogs.TabIndex = 0;
            // 
            // _status
            // 
            _status.ImageScalingSize = new Size(20, 20);
            _status.Items.AddRange(new ToolStripItem[] { _statusContext, _statusInfo, _statusLatency });
            _status.Location = new Point(3, 725);
            _status.Name = "_status";
            _status.Size = new Size(1194, 22);
            _status.TabIndex = 2;
            // 
            // _statusContext
            // 
            _statusContext.Name = "_statusContext";
            _statusContext.Size = new Size(59, 17);
            _statusContext.Text = "Context: -";
            // 
            // _statusInfo
            // 
            _statusInfo.Name = "_statusInfo";
            _statusInfo.Size = new Size(1061, 17);
            _statusInfo.Spring = true;
            _statusInfo.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // _statusLatency
            // 
            _statusLatency.Name = "_statusLatency";
            _statusLatency.Size = new Size(59, 17);
            _statusLatency.Text = "Latency: -";
            // 
            // MainView
            // 
            ClientSize = new Size(1200, 750);
            Controls.Add(_split);
            Controls.Add(_status);
            Controls.Add(_topPanel);
            MinimumSize = new Size(1200, 750);
            Name = "MainView";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "K8s Manager — Material UI";
            _topPanel.ResumeLayout(false);
            _split.Panel1.ResumeLayout(false);
            _split.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)_split).EndInit();
            _split.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)_grid).EndInit();
            _tabs.ResumeLayout(false);
            _tabYaml.ResumeLayout(false);
            _tabDescribe.ResumeLayout(false);
            _tabLogs.ResumeLayout(false);
            _tabLogs.PerformLayout();
            _status.ResumeLayout(false);
            _status.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
    }
}
