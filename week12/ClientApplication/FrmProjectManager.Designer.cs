namespace ClientApplication
{
    partial class FrmProjectManager
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
            this.pnlTop = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.pnlControls = new System.Windows.Forms.Panel();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnFilterByLocation = new System.Windows.Forms.Button();
            this.txtLocation = new System.Windows.Forms.TextBox();
            this.lblLocation = new System.Windows.Forms.Label();
            this.btnLoadAll = new System.Windows.Forms.Button();
            this.dgvProjects = new System.Windows.Forms.DataGridView();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.lblProjectCount = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.pnlTop.SuspendLayout();
            this.pnlControls.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProjects)).BeginInit();
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            //
            // pnlTop
            //
            this.pnlTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.pnlTop.Controls.Add(this.lblTitle);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(1000, 60);
            this.pnlTop.TabIndex = 0;
            //
            // lblTitle
            //
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(0, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(1000, 60);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "PROJECT MANAGEMENT SYSTEM";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // pnlControls
            //
            this.pnlControls.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.pnlControls.Controls.Add(this.btnDelete);
            this.pnlControls.Controls.Add(this.btnEdit);
            this.pnlControls.Controls.Add(this.btnAdd);
            this.pnlControls.Controls.Add(this.btnRefresh);
            this.pnlControls.Controls.Add(this.btnFilterByLocation);
            this.pnlControls.Controls.Add(this.txtLocation);
            this.pnlControls.Controls.Add(this.lblLocation);
            this.pnlControls.Controls.Add(this.btnLoadAll);
            this.pnlControls.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlControls.Location = new System.Drawing.Point(0, 60);
            this.pnlControls.Name = "pnlControls";
            this.pnlControls.Padding = new System.Windows.Forms.Padding(10);
            this.pnlControls.Size = new System.Drawing.Size(1000, 110);
            this.pnlControls.TabIndex = 1;
            //
            // btnDelete
            //
            this.btnDelete.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(67)))), ((int)(((byte)(54)))));
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDelete.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnDelete.ForeColor = System.Drawing.Color.White;
            this.btnDelete.Location = new System.Drawing.Point(270, 60);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(120, 40);
            this.btnDelete.TabIndex = 7;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            //
            // btnEdit
            //
            this.btnEdit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(152)))), ((int)(((byte)(0)))));
            this.btnEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEdit.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnEdit.ForeColor = System.Drawing.Color.White;
            this.btnEdit.Location = new System.Drawing.Point(140, 60);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(120, 40);
            this.btnEdit.TabIndex = 6;
            this.btnEdit.Text = "Edit";
            this.btnEdit.UseVisualStyleBackColor = false;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            //
            // btnAdd
            //
            this.btnAdd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(175)))), ((int)(((byte)(80)))));
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdd.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnAdd.ForeColor = System.Drawing.Color.White;
            this.btnAdd.Location = new System.Drawing.Point(10, 60);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(120, 40);
            this.btnAdd.TabIndex = 5;
            this.btnAdd.Text = "Add New";
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            //
            // btnRefresh
            //
            this.btnRefresh.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(150)))), ((int)(((byte)(136)))));
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefresh.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnRefresh.ForeColor = System.Drawing.Color.White;
            this.btnRefresh.Location = new System.Drawing.Point(860, 15);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(120, 40);
            this.btnRefresh.TabIndex = 4;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = false;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            //
            // btnFilterByLocation
            //
            this.btnFilterByLocation.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(150)))), ((int)(((byte)(243)))));
            this.btnFilterByLocation.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFilterByLocation.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnFilterByLocation.ForeColor = System.Drawing.Color.White;
            this.btnFilterByLocation.Location = new System.Drawing.Point(480, 15);
            this.btnFilterByLocation.Name = "btnFilterByLocation";
            this.btnFilterByLocation.Size = new System.Drawing.Size(180, 40);
            this.btnFilterByLocation.TabIndex = 3;
            this.btnFilterByLocation.Text = "Filter by Location";
            this.btnFilterByLocation.UseVisualStyleBackColor = false;
            this.btnFilterByLocation.Click += new System.EventHandler(this.btnFilterByLocation_Click);
            //
            // txtLocation
            //
            this.txtLocation.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.txtLocation.Location = new System.Drawing.Point(250, 20);
            this.txtLocation.Name = "txtLocation";
            this.txtLocation.Size = new System.Drawing.Size(220, 27);
            this.txtLocation.TabIndex = 2;
            this.txtLocation.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtLocation_KeyPress);
            //
            // lblLocation
            //
            this.lblLocation.AutoSize = true;
            this.lblLocation.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblLocation.Location = new System.Drawing.Point(160, 23);
            this.lblLocation.Name = "lblLocation";
            this.lblLocation.Size = new System.Drawing.Size(84, 20);
            this.lblLocation.TabIndex = 1;
            this.lblLocation.Text = "Location:";
            //
            // btnLoadAll
            //
            this.btnLoadAll.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(175)))), ((int)(((byte)(80)))));
            this.btnLoadAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLoadAll.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnLoadAll.ForeColor = System.Drawing.Color.White;
            this.btnLoadAll.Location = new System.Drawing.Point(20, 15);
            this.btnLoadAll.Name = "btnLoadAll";
            this.btnLoadAll.Size = new System.Drawing.Size(130, 40);
            this.btnLoadAll.TabIndex = 0;
            this.btnLoadAll.Text = "Load All Projects";
            this.btnLoadAll.UseVisualStyleBackColor = false;
            this.btnLoadAll.Click += new System.EventHandler(this.btnLoadAll_Click);
            //
            // dgvProjects
            //
            this.dgvProjects.AllowUserToAddRows = false;
            this.dgvProjects.AllowUserToDeleteRows = false;
            this.dgvProjects.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvProjects.BackgroundColor = System.Drawing.Color.White;
            this.dgvProjects.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvProjects.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvProjects.Location = new System.Drawing.Point(0, 170);
            this.dgvProjects.Name = "dgvProjects";
            this.dgvProjects.ReadOnly = true;
            this.dgvProjects.RowHeadersWidth = 51;
            this.dgvProjects.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvProjects.Size = new System.Drawing.Size(1000, 380);
            this.dgvProjects.TabIndex = 2;
            //
            // pnlBottom
            //
            this.pnlBottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.pnlBottom.Controls.Add(this.lblProjectCount);
            this.pnlBottom.Controls.Add(this.lblStatus);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottom.Location = new System.Drawing.Point(0, 550);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(1000, 50);
            this.pnlBottom.TabIndex = 3;
            //
            // lblProjectCount
            //
            this.lblProjectCount.AutoSize = true;
            this.lblProjectCount.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblProjectCount.Location = new System.Drawing.Point(20, 15);
            this.lblProjectCount.Name = "lblProjectCount";
            this.lblProjectCount.Size = new System.Drawing.Size(128, 19);
            this.lblProjectCount.TabIndex = 1;
            this.lblProjectCount.Text = "Total Projects: 0";
            //
            // lblStatus
            //
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblStatus.ForeColor = System.Drawing.Color.Gray;
            this.lblStatus.Location = new System.Drawing.Point(760, 15);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(141, 19);
            this.lblStatus.TabIndex = 0;
            this.lblStatus.Text = "Status: Not connected";
            //
            // FrmProjectManager
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1000, 600);
            this.Controls.Add(this.dgvProjects);
            this.Controls.Add(this.pnlBottom);
            this.Controls.Add(this.pnlControls);
            this.Controls.Add(this.pnlTop);
            this.Name = "FrmProjectManager";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Project Management System - Client";
            this.pnlTop.ResumeLayout(false);
            this.pnlControls.ResumeLayout(false);
            this.pnlControls.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProjects)).EndInit();
            this.pnlBottom.ResumeLayout(false);
            this.pnlBottom.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel pnlControls;
        private System.Windows.Forms.Button btnLoadAll;
        private System.Windows.Forms.DataGridView dgvProjects;
        private System.Windows.Forms.Panel pnlBottom;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblLocation;
        private System.Windows.Forms.TextBox txtLocation;
        private System.Windows.Forms.Button btnFilterByLocation;
        private System.Windows.Forms.Label lblProjectCount;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnDelete;
    }
}
