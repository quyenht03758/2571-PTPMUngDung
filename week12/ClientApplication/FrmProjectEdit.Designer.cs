namespace ClientApplication
{
    partial class FrmProjectEdit
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
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
            this.lblProjectNumber = new System.Windows.Forms.Label();
            this.txtProjectNumber = new System.Windows.Forms.TextBox();
            this.lblProjectName = new System.Windows.Forms.Label();
            this.txtProjectName = new System.Windows.Forms.TextBox();
            this.lblLocation = new System.Windows.Forms.Label();
            this.txtLocation = new System.Windows.Forms.TextBox();
            this.lblStartDate = new System.Windows.Forms.Label();
            this.dtpStartDate = new System.Windows.Forms.DateTimePicker();
            this.lblProjectManager = new System.Windows.Forms.Label();
            this.txtProjectManager = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            //
            // lblProjectNumber
            //
            this.lblProjectNumber.AutoSize = true;
            this.lblProjectNumber.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblProjectNumber.Location = new System.Drawing.Point(30, 30);
            this.lblProjectNumber.Name = "lblProjectNumber";
            this.lblProjectNumber.Size = new System.Drawing.Size(119, 19);
            this.lblProjectNumber.TabIndex = 0;
            this.lblProjectNumber.Text = "Project Number:";
            //
            // txtProjectNumber
            //
            this.txtProjectNumber.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtProjectNumber.Location = new System.Drawing.Point(180, 27);
            this.txtProjectNumber.Name = "txtProjectNumber";
            this.txtProjectNumber.Size = new System.Drawing.Size(300, 25);
            this.txtProjectNumber.TabIndex = 1;
            //
            // lblProjectName
            //
            this.lblProjectName.AutoSize = true;
            this.lblProjectName.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblProjectName.Location = new System.Drawing.Point(30, 70);
            this.lblProjectName.Name = "lblProjectName";
            this.lblProjectName.Size = new System.Drawing.Size(104, 19);
            this.lblProjectName.TabIndex = 2;
            this.lblProjectName.Text = "Project Name:";
            //
            // txtProjectName
            //
            this.txtProjectName.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtProjectName.Location = new System.Drawing.Point(180, 67);
            this.txtProjectName.Name = "txtProjectName";
            this.txtProjectName.Size = new System.Drawing.Size(300, 25);
            this.txtProjectName.TabIndex = 3;
            //
            // lblLocation
            //
            this.lblLocation.AutoSize = true;
            this.lblLocation.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblLocation.Location = new System.Drawing.Point(30, 110);
            this.lblLocation.Name = "lblLocation";
            this.lblLocation.Size = new System.Drawing.Size(71, 19);
            this.lblLocation.TabIndex = 4;
            this.lblLocation.Text = "Location:";
            //
            // txtLocation
            //
            this.txtLocation.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtLocation.Location = new System.Drawing.Point(180, 107);
            this.txtLocation.Name = "txtLocation";
            this.txtLocation.Size = new System.Drawing.Size(300, 25);
            this.txtLocation.TabIndex = 5;
            //
            // lblStartDate
            //
            this.lblStartDate.AutoSize = true;
            this.lblStartDate.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblStartDate.Location = new System.Drawing.Point(30, 150);
            this.lblStartDate.Name = "lblStartDate";
            this.lblStartDate.Size = new System.Drawing.Size(81, 19);
            this.lblStartDate.TabIndex = 6;
            this.lblStartDate.Text = "Start Date:";
            //
            // dtpStartDate
            //
            this.dtpStartDate.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.dtpStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpStartDate.Location = new System.Drawing.Point(180, 147);
            this.dtpStartDate.Name = "dtpStartDate";
            this.dtpStartDate.Size = new System.Drawing.Size(300, 25);
            this.dtpStartDate.TabIndex = 7;
            //
            // lblProjectManager
            //
            this.lblProjectManager.AutoSize = true;
            this.lblProjectManager.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblProjectManager.Location = new System.Drawing.Point(30, 190);
            this.lblProjectManager.Name = "lblProjectManager";
            this.lblProjectManager.Size = new System.Drawing.Size(128, 19);
            this.lblProjectManager.TabIndex = 8;
            this.lblProjectManager.Text = "Project Manager:";
            //
            // txtProjectManager
            //
            this.txtProjectManager.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtProjectManager.Location = new System.Drawing.Point(180, 187);
            this.txtProjectManager.Name = "txtProjectManager";
            this.txtProjectManager.Size = new System.Drawing.Size(300, 25);
            this.txtProjectManager.TabIndex = 9;
            //
            // btnSave
            //
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(175)))), ((int)(((byte)(80)))));
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(280, 240);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(100, 35);
            this.btnSave.TabIndex = 10;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            //
            // btnCancel
            //
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(67)))), ((int)(((byte)(54)))));
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(390, 240);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 35);
            this.btnCancel.TabIndex = 11;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            //
            // FrmProjectEdit
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(520, 300);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.txtProjectManager);
            this.Controls.Add(this.lblProjectManager);
            this.Controls.Add(this.dtpStartDate);
            this.Controls.Add(this.lblStartDate);
            this.Controls.Add(this.txtLocation);
            this.Controls.Add(this.lblLocation);
            this.Controls.Add(this.txtProjectName);
            this.Controls.Add(this.lblProjectName);
            this.Controls.Add(this.txtProjectNumber);
            this.Controls.Add(this.lblProjectNumber);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmProjectEdit";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Project Edit";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblProjectNumber;
        private System.Windows.Forms.TextBox txtProjectNumber;
        private System.Windows.Forms.Label lblProjectName;
        private System.Windows.Forms.TextBox txtProjectName;
        private System.Windows.Forms.Label lblLocation;
        private System.Windows.Forms.TextBox txtLocation;
        private System.Windows.Forms.Label lblStartDate;
        private System.Windows.Forms.DateTimePicker dtpStartDate;
        private System.Windows.Forms.Label lblProjectManager;
        private System.Windows.Forms.TextBox txtProjectManager;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
    }
}
