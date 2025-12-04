namespace K8sManager.Views
{
    partial class UserManagerDialog
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
        private System.Windows.Forms.Button _btnAdd;
        private System.Windows.Forms.Button _btnRefresh;
        private System.Windows.Forms.Button _btnClose;
        private System.Windows.Forms.DataGridView _gridUsers;
        private System.Windows.Forms.Panel _bottomPanel;

        private void InitializeComponent()
        {
            this._topPanel = new System.Windows.Forms.Panel();
            this._lblTitle = new System.Windows.Forms.Label();
            this._btnAdd = new System.Windows.Forms.Button();
            this._btnRefresh = new System.Windows.Forms.Button();
            this._btnClose = new System.Windows.Forms.Button();
            this._gridUsers = new System.Windows.Forms.DataGridView();
            this._bottomPanel = new System.Windows.Forms.Panel();
            this._topPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._gridUsers)).BeginInit();
            this.SuspendLayout();
            // 
            // _topPanel
            // 
            this._topPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(115)))), ((int)(((byte)(220)))));
            this._topPanel.Controls.Add(this._lblTitle);
            this._topPanel.Controls.Add(this._btnAdd);
            this._topPanel.Controls.Add(this._btnRefresh);
            this._topPanel.Controls.Add(this._btnClose);
            this._topPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this._topPanel.Location = new System.Drawing.Point(0, 0);
            this._topPanel.Name = "_topPanel";
            this._topPanel.Size = new System.Drawing.Size(1000, 70);
            this._topPanel.TabIndex = 0;
            // 
            // _lblTitle
            // 
            this._lblTitle.AutoSize = true;
            this._lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this._lblTitle.ForeColor = System.Drawing.Color.White;
            this._lblTitle.Location = new System.Drawing.Point(20, 20);
            this._lblTitle.Name = "_lblTitle";
            this._lblTitle.Size = new System.Drawing.Size(200, 30);
            this._lblTitle.TabIndex = 0;
            this._lblTitle.Text = "👥 User Management";
            // 
            // _btnAdd
            // 
            this._btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._btnAdd.BackColor = System.Drawing.Color.White;
            this._btnAdd.FlatAppearance.BorderSize = 0;
            this._btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._btnAdd.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this._btnAdd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(115)))), ((int)(((byte)(220)))));
            this._btnAdd.Location = new System.Drawing.Point(680, 18);
            this._btnAdd.Name = "_btnAdd";
            this._btnAdd.Size = new System.Drawing.Size(90, 35);
            this._btnAdd.TabIndex = 1;
            this._btnAdd.Text = "➕ Add User";
            this._btnAdd.UseVisualStyleBackColor = false;
            this._btnAdd.Click += new System.EventHandler(this.BtnAdd_Click);
            // 
            // _btnRefresh
            // 
            this._btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._btnRefresh.BackColor = System.Drawing.Color.White;
            this._btnRefresh.FlatAppearance.BorderSize = 0;
            this._btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._btnRefresh.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this._btnRefresh.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(115)))), ((int)(((byte)(220)))));
            this._btnRefresh.Location = new System.Drawing.Point(780, 18);
            this._btnRefresh.Name = "_btnRefresh";
            this._btnRefresh.Size = new System.Drawing.Size(90, 35);
            this._btnRefresh.TabIndex = 2;
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
            this._btnClose.Location = new System.Drawing.Point(880, 18);
            this._btnClose.Name = "_btnClose";
            this._btnClose.Size = new System.Drawing.Size(100, 35);
            this._btnClose.TabIndex = 3;
            this._btnClose.Text = "✖ Close";
            this._btnClose.UseVisualStyleBackColor = false;
            this._btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // _gridUsers
            // 
            this._gridUsers.AllowUserToAddRows = false;
            this._gridUsers.AllowUserToDeleteRows = false;
            this._gridUsers.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this._gridUsers.BackgroundColor = System.Drawing.Color.White;
            this._gridUsers.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._gridUsers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._gridUsers.Dock = System.Windows.Forms.DockStyle.Fill;
            this._gridUsers.Location = new System.Drawing.Point(0, 70);
            this._gridUsers.MultiSelect = false;
            this._gridUsers.Name = "_gridUsers";
            this._gridUsers.ReadOnly = true;
            this._gridUsers.RowHeadersVisible = false;
            this._gridUsers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this._gridUsers.Size = new System.Drawing.Size(1000, 480);
            this._gridUsers.TabIndex = 1;
            this._gridUsers.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.GridUsers_CellMouseDown);
            // 
            // _bottomPanel
            // 
            this._bottomPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this._bottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._bottomPanel.Location = new System.Drawing.Point(0, 550);
            this._bottomPanel.Name = "_bottomPanel";
            this._bottomPanel.Size = new System.Drawing.Size(1000, 50);
            this._bottomPanel.TabIndex = 2;
            // 
            // UserManagerDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1000, 600);
            this.Controls.Add(this._gridUsers);
            this.Controls.Add(this._bottomPanel);
            this.Controls.Add(this._topPanel);
            this.MaximizeBox = true;
            this.MinimizeBox = false;
            this.Name = "UserManagerDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "User Management";
            this._topPanel.ResumeLayout(false);
            this._topPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._gridUsers)).EndInit();
            this.ResumeLayout(false);
        }
    }
}
