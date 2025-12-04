namespace K8sManager.Views
{
    partial class ApplyYamlDialog
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.RichTextBox _rtYaml;
        private MaterialSkin.Controls.MaterialButton _btnOpen;
        private MaterialSkin.Controls.MaterialButton _btnSave;
        private MaterialSkin.Controls.MaterialButton _btnApply;
        private MaterialSkin.Controls.MaterialButton _btnCancel;
        private System.Windows.Forms.TextBox _txtStatus;
        private System.Windows.Forms.FlowLayoutPanel toolbar;
        private System.Windows.Forms.Panel mainPanel;

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
            this.components = new System.ComponentModel.Container();
            this._rtYaml = new System.Windows.Forms.RichTextBox();
            this._btnOpen = new MaterialSkin.Controls.MaterialButton();
            this._btnSave = new MaterialSkin.Controls.MaterialButton();
            this._btnApply = new MaterialSkin.Controls.MaterialButton();
            this._btnCancel = new MaterialSkin.Controls.MaterialButton();
            this._txtStatus = new System.Windows.Forms.TextBox();
            this.toolbar = new System.Windows.Forms.FlowLayoutPanel();
            this.mainPanel = new System.Windows.Forms.Panel();
            this.toolbar.SuspendLayout();
            this.mainPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // _rtYaml
            // 
            this._rtYaml.AcceptsTab = true;
            this._rtYaml.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._rtYaml.Dock = System.Windows.Forms.DockStyle.Fill;
            this._rtYaml.Font = new System.Drawing.Font("Consolas", 10F);
            this._rtYaml.Location = new System.Drawing.Point(10, 10);
            this._rtYaml.Name = "_rtYaml";
            this._rtYaml.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Both;
            this._rtYaml.Size = new System.Drawing.Size(880, 570);
            this._rtYaml.TabIndex = 0;
            this._rtYaml.Text = "# Example YAML - replace with your configuration\\napiVersion: v1\\nkind: Pod\\nmetadata:\\n  name: my-pod\\n  namespace: default\\nspec:\\n  containers:\\n  - name: nginx\\n    image: nginx:latest";
            this._rtYaml.WordWrap = false;
            // 
            // _btnOpen
            // 
            this._btnOpen.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._btnOpen.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this._btnOpen.Depth = 0;
            this._btnOpen.HighEmphasis = true;
            this._btnOpen.Icon = null;
            this._btnOpen.Location = new System.Drawing.Point(14, 10);
            this._btnOpen.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this._btnOpen.MouseState = MaterialSkin.MouseState.HOVER;
            this._btnOpen.Name = "_btnOpen";
            this._btnOpen.NoAccentTextColor = System.Drawing.Color.Empty;
            this._btnOpen.Size = new System.Drawing.Size(120, 36);
            this._btnOpen.TabIndex = 0;
            this._btnOpen.Text = "📂 Open File";
            this._btnOpen.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this._btnOpen.UseAccentColor = false;
            this._btnOpen.UseVisualStyleBackColor = true;
            this._btnOpen.Click += new System.EventHandler(this.BtnOpen_Click);
            // 
            // _btnSave
            // 
            this._btnSave.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._btnSave.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this._btnSave.Depth = 0;
            this._btnSave.HighEmphasis = true;
            this._btnSave.Icon = null;
            this._btnSave.Location = new System.Drawing.Point(142, 10);
            this._btnSave.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this._btnSave.MouseState = MaterialSkin.MouseState.HOVER;
            this._btnSave.Name = "_btnSave";
            this._btnSave.NoAccentTextColor = System.Drawing.Color.Empty;
            this._btnSave.Size = new System.Drawing.Size(120, 36);
            this._btnSave.TabIndex = 1;
            this._btnSave.Text = "💾 Save File";
            this._btnSave.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Outlined;
            this._btnSave.UseAccentColor = false;
            this._btnSave.UseVisualStyleBackColor = true;
            this._btnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // _btnApply
            // 
            this._btnApply.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._btnApply.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this._btnApply.Depth = 0;
            this._btnApply.HighEmphasis = true;
            this._btnApply.Icon = null;
            this._btnApply.Location = new System.Drawing.Point(290, 10);
            this._btnApply.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this._btnApply.MouseState = MaterialSkin.MouseState.HOVER;
            this._btnApply.Name = "_btnApply";
            this._btnApply.NoAccentTextColor = System.Drawing.Color.Empty;
            this._btnApply.Size = new System.Drawing.Size(120, 36);
            this._btnApply.TabIndex = 2;
            this._btnApply.Text = "✅ Apply";
            this._btnApply.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this._btnApply.UseAccentColor = false;
            this._btnApply.UseVisualStyleBackColor = true;
            this._btnApply.Click += new System.EventHandler(this.BtnApply_Click);
            // 
            // _btnCancel
            // 
            this._btnCancel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._btnCancel.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this._btnCancel.Depth = 0;
            this._btnCancel.HighEmphasis = true;
            this._btnCancel.Icon = null;
            this._btnCancel.Location = new System.Drawing.Point(418, 10);
            this._btnCancel.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this._btnCancel.MouseState = MaterialSkin.MouseState.HOVER;
            this._btnCancel.Name = "_btnCancel";
            this._btnCancel.NoAccentTextColor = System.Drawing.Color.Empty;
            this._btnCancel.Size = new System.Drawing.Size(100, 36);
            this._btnCancel.TabIndex = 3;
            this._btnCancel.Text = "Cancel";
            this._btnCancel.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Text;
            this._btnCancel.UseAccentColor = false;
            this._btnCancel.UseVisualStyleBackColor = true;
            this._btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // _txtStatus
            // 
            this._txtStatus.BackColor = System.Drawing.Color.FromArgb(245, 245, 245);
            this._txtStatus.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._txtStatus.Font = new System.Drawing.Font("Consolas", 9F);
            this._txtStatus.Location = new System.Drawing.Point(10, 580);
            this._txtStatus.Multiline = true;
            this._txtStatus.Name = "_txtStatus";
            this._txtStatus.ReadOnly = true;
            this._txtStatus.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this._txtStatus.Size = new System.Drawing.Size(880, 60);
            this._txtStatus.TabIndex = 1;
            this._txtStatus.Text = "ℹ️ Paste your YAML configuration or open a file. Click Apply to create/update the resource.";
            // 
            // toolbar
            // 
            this.toolbar.BackColor = System.Drawing.Color.FromArgb(250, 250, 250);
            this.toolbar.Controls.Add(this._btnOpen);
            this.toolbar.Controls.Add(this._btnSave);
            this.toolbar.Controls.Add(this._btnApply);
            this.toolbar.Controls.Add(this._btnCancel);
            this.toolbar.Dock = System.Windows.Forms.DockStyle.Top;
            this.toolbar.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
            this.toolbar.Location = new System.Drawing.Point(0, 0);
            this.toolbar.Name = "toolbar";
            this.toolbar.Padding = new System.Windows.Forms.Padding(10);
            this.toolbar.Size = new System.Drawing.Size(900, 56);
            this.toolbar.TabIndex = 0;
            this.toolbar.WrapContents = false;
            // 
            // mainPanel
            // 
            this.mainPanel.Controls.Add(this._rtYaml);
            this.mainPanel.Controls.Add(this._txtStatus);
            this.mainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainPanel.Location = new System.Drawing.Point(0, 56);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Padding = new System.Windows.Forms.Padding(10);
            this.mainPanel.Size = new System.Drawing.Size(900, 644);
            this.mainPanel.TabIndex = 1;
            // 
            // ApplyYamlDialog
            // 
            this.ClientSize = new System.Drawing.Size(900, 700);
            this.Controls.Add(this.mainPanel);
            this.Controls.Add(this.toolbar);
            this.MinimumSize = new System.Drawing.Size(800, 600);
            this.Name = "ApplyYamlDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Apply YAML Configuration";
            this.toolbar.ResumeLayout(false);
            this.toolbar.PerformLayout();
            this.mainPanel.ResumeLayout(false);
            this.mainPanel.PerformLayout();
            this.ResumeLayout(false);
        }
    }
}
