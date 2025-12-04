using MaterialSkin.Controls;

namespace K8sManager.Views
{
    partial class FavoritesDialog
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
            this.topPanel = new System.Windows.Forms.TableLayoutPanel();
            this._txtSearch = new MaterialSkin.Controls.MaterialTextBox2();
            this._cbResourceType = new MaterialSkin.Controls.MaterialComboBox();
            this._cbNamespace = new MaterialSkin.Controls.MaterialComboBox();
            this._btnRefresh = new MaterialSkin.Controls.MaterialButton();
            this._btnAdd = new MaterialSkin.Controls.MaterialButton();
            this._grid = new System.Windows.Forms.DataGridView();
            this.ResourceKind = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ResourceName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Namespace = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Notes = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CreatedAt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bottomPanel = new System.Windows.Forms.Panel();
            this._lblTotal = new System.Windows.Forms.Label();
            this.rightPanel = new System.Windows.Forms.FlowLayoutPanel();
            this._btnClose = new MaterialSkin.Controls.MaterialButton();
            this._btnGoto = new MaterialSkin.Controls.MaterialButton();
            this._btnDelete = new MaterialSkin.Controls.MaterialButton();
            this._btnEdit = new MaterialSkin.Controls.MaterialButton();
            this.topPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._grid)).BeginInit();
            this.bottomPanel.SuspendLayout();
            this.rightPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // topPanel
            // 
            this.topPanel.ColumnCount = 6;
            this.topPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.topPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.topPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.topPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            this.topPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            this.topPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.topPanel.Controls.Add(this._txtSearch, 0, 0);
            this.topPanel.Controls.Add(this._cbResourceType, 1, 0);
            this.topPanel.Controls.Add(this._cbNamespace, 2, 0);
            this.topPanel.Controls.Add(this._btnRefresh, 3, 0);
            this.topPanel.Controls.Add(this._btnAdd, 4, 0);
            this.topPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.topPanel.Location = new System.Drawing.Point(0, 0);
            this.topPanel.Name = "topPanel";
            this.topPanel.Padding = new System.Windows.Forms.Padding(16, 12, 16, 12);
            this.topPanel.RowCount = 1;
            this.topPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.topPanel.Size = new System.Drawing.Size(1000, 80);
            this.topPanel.TabIndex = 0;
            // 
            // _txtSearch
            // 
            this._txtSearch.AnimateReadOnly = false;
            this._txtSearch.Depth = 0;
            this._txtSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this._txtSearch.Font = new System.Drawing.Font("Roboto", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this._txtSearch.Hint = "Search by resource name or notes...";
            this._txtSearch.LeadingIcon = null;
            this._txtSearch.Location = new System.Drawing.Point(19, 15);
            this._txtSearch.MaxLength = 50;
            this._txtSearch.MouseState = MaterialSkin.MouseState.OUT;
            this._txtSearch.Name = "_txtSearch";
            this._txtSearch.Size = new System.Drawing.Size(382, 50);
            this._txtSearch.TabIndex = 0;
            this._txtSearch.Text = "";
            this._txtSearch.TrailingIcon = null;
            // 
            // _cbResourceType
            // 
            this._cbResourceType.AutoResize = false;
            this._cbResourceType.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this._cbResourceType.Depth = 0;
            this._cbResourceType.Dock = System.Windows.Forms.DockStyle.Fill;
            this._cbResourceType.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this._cbResourceType.DropDownHeight = 174;
            this._cbResourceType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cbResourceType.DropDownWidth = 121;
            this._cbResourceType.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this._cbResourceType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this._cbResourceType.FormattingEnabled = true;
            this._cbResourceType.Hint = "Resource Type";
            this._cbResourceType.IntegralHeight = false;
            this._cbResourceType.ItemHeight = 43;
            this._cbResourceType.Items.AddRange(new object[] {
            "All Types",
            "Pod",
            "Deployment",
            "Service",
            "StatefulSet",
            "DaemonSet",
            "ConfigMap",
            "Secret"});
            this._cbResourceType.Location = new System.Drawing.Point(407, 15);
            this._cbResourceType.MaxDropDownItems = 4;
            this._cbResourceType.MouseState = MaterialSkin.MouseState.OUT;
            this._cbResourceType.Name = "_cbResourceType";
            this._cbResourceType.Size = new System.Drawing.Size(185, 49);
            this._cbResourceType.StartIndex = 0;
            this._cbResourceType.TabIndex = 1;
            // 
            // _cbNamespace
            // 
            this._cbNamespace.AutoResize = false;
            this._cbNamespace.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this._cbNamespace.Depth = 0;
            this._cbNamespace.Dock = System.Windows.Forms.DockStyle.Fill;
            this._cbNamespace.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this._cbNamespace.DropDownHeight = 174;
            this._cbNamespace.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cbNamespace.DropDownWidth = 121;
            this._cbNamespace.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this._cbNamespace.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this._cbNamespace.FormattingEnabled = true;
            this._cbNamespace.Hint = "Namespace";
            this._cbNamespace.IntegralHeight = false;
            this._cbNamespace.ItemHeight = 43;
            this._cbNamespace.Items.AddRange(new object[] {
            "All Namespaces"});
            this._cbNamespace.Location = new System.Drawing.Point(598, 15);
            this._cbNamespace.MaxDropDownItems = 4;
            this._cbNamespace.MouseState = MaterialSkin.MouseState.OUT;
            this._cbNamespace.Name = "_cbNamespace";
            this._cbNamespace.Size = new System.Drawing.Size(185, 49);
            this._cbNamespace.StartIndex = 0;
            this._cbNamespace.TabIndex = 2;
            // 
            // _btnRefresh
            // 
            this._btnRefresh.AutoSize = false;
            this._btnRefresh.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._btnRefresh.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this._btnRefresh.Depth = 0;
            this._btnRefresh.HighEmphasis = true;
            this._btnRefresh.Icon = null;
            this._btnRefresh.Location = new System.Drawing.Point(791, 18);
            this._btnRefresh.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this._btnRefresh.MouseState = MaterialSkin.MouseState.HOVER;
            this._btnRefresh.Name = "_btnRefresh";
            this._btnRefresh.NoAccentTextColor = System.Drawing.Color.Empty;
            this._btnRefresh.Size = new System.Drawing.Size(100, 44);
            this._btnRefresh.TabIndex = 3;
            this._btnRefresh.Text = "Refresh";
            this._btnRefresh.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this._btnRefresh.UseAccentColor = false;
            this._btnRefresh.UseVisualStyleBackColor = true;
            // 
            // _btnAdd
            // 
            this._btnAdd.AutoSize = false;
            this._btnAdd.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._btnAdd.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this._btnAdd.Depth = 0;
            this._btnAdd.HighEmphasis = true;
            this._btnAdd.Icon = null;
            this._btnAdd.Location = new System.Drawing.Point(899, 18);
            this._btnAdd.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this._btnAdd.MouseState = MaterialSkin.MouseState.HOVER;
            this._btnAdd.Name = "_btnAdd";
            this._btnAdd.NoAccentTextColor = System.Drawing.Color.Empty;
            this._btnAdd.Size = new System.Drawing.Size(80, 44);
            this._btnAdd.TabIndex = 4;
            this._btnAdd.Text = "+ Add";
            this._btnAdd.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Outlined;
            this._btnAdd.UseAccentColor = false;
            this._btnAdd.UseVisualStyleBackColor = true;
            // 
            // _grid
            // 
            this._grid.AllowUserToAddRows = false;
            this._grid.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this._grid.AutoGenerateColumns = false;
            this._grid.BackgroundColor = System.Drawing.Color.White;
            this._grid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._grid.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this._grid.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this._grid.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this._grid.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this._grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._grid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ResourceKind,
            this.ResourceName,
            this.Namespace,
            this.Notes,
            this.CreatedAt});
            this._grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this._grid.Location = new System.Drawing.Point(0, 80);
            this._grid.MultiSelect = false;
            this._grid.Name = "_grid";
            this._grid.ReadOnly = true;
            this._grid.RowHeadersVisible = false;
            this._grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this._grid.Size = new System.Drawing.Size(1000, 460);
            this._grid.TabIndex = 1;
            // 
            // ResourceKind
            // 
            this.ResourceKind.DataPropertyName = "ResourceKind";
            this.ResourceKind.HeaderText = "Type";
            this.ResourceKind.Name = "ResourceKind";
            this.ResourceKind.ReadOnly = true;
            this.ResourceKind.Width = 120;
            // 
            // ResourceName
            // 
            this.ResourceName.DataPropertyName = "ResourceName";
            this.ResourceName.HeaderText = "Resource Name";
            this.ResourceName.Name = "ResourceName";
            this.ResourceName.ReadOnly = true;
            this.ResourceName.Width = 200;
            // 
            // Namespace
            // 
            this.Namespace.DataPropertyName = "Namespace";
            this.Namespace.HeaderText = "Namespace";
            this.Namespace.Name = "Namespace";
            this.Namespace.ReadOnly = true;
            this.Namespace.Width = 120;
            // 
            // Notes
            // 
            this.Notes.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Notes.DataPropertyName = "Notes";
            this.Notes.HeaderText = "Notes";
            this.Notes.Name = "Notes";
            this.Notes.ReadOnly = true;
            // 
            // CreatedAt
            // 
            this.CreatedAt.DataPropertyName = "CreatedAt";
            this.CreatedAt.DefaultCellStyle.Format = "yyyy-MM-dd HH:mm";
            this.CreatedAt.HeaderText = "Created";
            this.CreatedAt.Name = "CreatedAt";
            this.CreatedAt.ReadOnly = true;
            this.CreatedAt.Width = 140;
            // 
            // bottomPanel
            // 
            this.bottomPanel.Controls.Add(this._lblTotal);
            this.bottomPanel.Controls.Add(this.rightPanel);
            this.bottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.bottomPanel.Location = new System.Drawing.Point(0, 520);
            this.bottomPanel.Name = "bottomPanel";
            this.bottomPanel.Padding = new System.Windows.Forms.Padding(16, 12, 16, 12);
            this.bottomPanel.Size = new System.Drawing.Size(1000, 80);
            this.bottomPanel.TabIndex = 2;
            // 
            // _lblTotal
            // 
            this._lblTotal.AutoSize = true;
            this._lblTotal.Dock = System.Windows.Forms.DockStyle.Left;
            this._lblTotal.Location = new System.Drawing.Point(16, 12);
            this._lblTotal.Name = "_lblTotal";
            this._lblTotal.Size = new System.Drawing.Size(95, 13);
            this._lblTotal.TabIndex = 0;
            this._lblTotal.Text = "Total: 0 favorites";
            this._lblTotal.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // rightPanel
            // 
            this.rightPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rightPanel.AutoSize = true;
            this.rightPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.rightPanel.Controls.Add(this._btnEdit);
            this.rightPanel.Controls.Add(this._btnDelete);
            this.rightPanel.Controls.Add(this._btnGoto);
            this.rightPanel.Controls.Add(this._btnClose);
            this.rightPanel.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
            this.rightPanel.Location = new System.Drawing.Point(450, 15);
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
            this._btnClose.TabIndex = 0;
            this._btnClose.Text = "Close";
            this._btnClose.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Text;
            this._btnClose.UseAccentColor = false;
            this._btnClose.UseVisualStyleBackColor = true;
            // 
            // _btnGoto
            // 
            this._btnGoto.AutoSize = false;
            this._btnGoto.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._btnGoto.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this._btnGoto.Depth = 0;
            this._btnGoto.HighEmphasis = true;
            this._btnGoto.Icon = null;
            this._btnGoto.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this._btnGoto.MouseState = MaterialSkin.MouseState.HOVER;
            this._btnGoto.Name = "_btnGoto";
            this._btnGoto.NoAccentTextColor = System.Drawing.Color.Empty;
            this._btnGoto.Size = new System.Drawing.Size(140, 36);
            this._btnGoto.TabIndex = 1;
            this._btnGoto.Text = "Go to Resource";
            this._btnGoto.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this._btnGoto.UseAccentColor = false;
            this._btnGoto.UseVisualStyleBackColor = true;
            // 
            // _btnDelete
            // 
            this._btnDelete.AutoSize = false;
            this._btnDelete.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._btnDelete.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this._btnDelete.Depth = 0;
            this._btnDelete.HighEmphasis = true;
            this._btnDelete.Icon = null;
            this._btnDelete.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this._btnDelete.MouseState = MaterialSkin.MouseState.HOVER;
            this._btnDelete.Name = "_btnDelete";
            this._btnDelete.NoAccentTextColor = System.Drawing.Color.Empty;
            this._btnDelete.Size = new System.Drawing.Size(120, 36);
            this._btnDelete.TabIndex = 2;
            this._btnDelete.Text = "🗑️ Delete";
            this._btnDelete.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Outlined;
            this._btnDelete.UseAccentColor = false;
            this._btnDelete.UseVisualStyleBackColor = true;
            // 
            // _btnEdit
            // 
            this._btnEdit.AutoSize = false;
            this._btnEdit.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._btnEdit.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this._btnEdit.Depth = 0;
            this._btnEdit.HighEmphasis = true;
            this._btnEdit.Icon = null;
            this._btnEdit.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this._btnEdit.MouseState = MaterialSkin.MouseState.HOVER;
            this._btnEdit.Name = "_btnEdit";
            this._btnEdit.NoAccentTextColor = System.Drawing.Color.Empty;
            this._btnEdit.Size = new System.Drawing.Size(140, 36);
            this._btnEdit.TabIndex = 3;
            this._btnEdit.Text = "✏️ Edit Notes";
            this._btnEdit.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Outlined;
            this._btnEdit.UseAccentColor = false;
            this._btnEdit.UseVisualStyleBackColor = true;
            // 
            // FavoritesDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1000, 600);
            this.Controls.Add(this._grid);
            this.Controls.Add(this.bottomPanel);
            this.Controls.Add(this.topPanel);
            this.MaximizeBox = true;
            this.Name = "FavoritesDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "⭐ Favorites Manager";
            this.topPanel.ResumeLayout(false);
            this.topPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._grid)).EndInit();
            this.bottomPanel.ResumeLayout(false);
            this.bottomPanel.PerformLayout();
            this.rightPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

            InitializeControls();
        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel topPanel;
        private MaterialTextBox2 _txtSearch;
        private MaterialComboBox _cbResourceType;
        private MaterialComboBox _cbNamespace;
        private MaterialButton _btnRefresh;
        private MaterialButton _btnAdd;
        private System.Windows.Forms.DataGridView _grid;
        private System.Windows.Forms.DataGridViewTextBoxColumn ResourceKind;
        private System.Windows.Forms.DataGridViewTextBoxColumn ResourceName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Namespace;
        private System.Windows.Forms.DataGridViewTextBoxColumn Notes;
        private System.Windows.Forms.DataGridViewTextBoxColumn CreatedAt;
        private System.Windows.Forms.Panel bottomPanel;
        private System.Windows.Forms.Label _lblTotal;
        private System.Windows.Forms.FlowLayoutPanel rightPanel;
        private MaterialButton _btnClose;
        private MaterialButton _btnGoto;
        private MaterialButton _btnDelete;
        private MaterialButton _btnEdit;
    }
}
