using System.Drawing;
using System.Windows.Forms;

namespace ClientApplication
{
    partial class FrmSocialInsurance
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
            this.panelTop = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.groupBoxPerson = new System.Windows.Forms.GroupBox();
            this.lblSocialNumber = new System.Windows.Forms.Label();
            this.cboSocialNumber = new System.Windows.Forms.ComboBox();
            this.lblFullName = new System.Windows.Forms.Label();
            this.txtFullName = new System.Windows.Forms.TextBox();
            this.lblAddress = new System.Windows.Forms.Label();
            this.txtAddress = new System.Windows.Forms.TextBox();
            this.lblExpDate = new System.Windows.Forms.Label();
            this.txtExpDate = new System.Windows.Forms.TextBox();
            this.lblTreatmentLocation = new System.Windows.Forms.Label();
            this.txtTreatmentLocation = new System.Windows.Forms.TextBox();
            this.lblDescription = new System.Windows.Forms.Label();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.groupBoxQuery = new System.Windows.Forms.GroupBox();
            this.btnGetAll = new System.Windows.Forms.Button();
            this.lblBeginDate = new System.Windows.Forms.Label();
            this.dtpBeginDate = new System.Windows.Forms.DateTimePicker();
            this.lblEndDate = new System.Windows.Forms.Label();
            this.dtpEndDate = new System.Windows.Forms.DateTimePicker();
            this.btnGetByDate = new System.Windows.Forms.Button();
            this.lblCompany = new System.Windows.Forms.Label();
            this.txtCompany = new System.Windows.Forms.TextBox();
            this.btnGetByCompany = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.groupBoxResults = new System.Windows.Forms.GroupBox();
            this.dgvProcesses = new System.Windows.Forms.DataGridView();
            this.lblRecordCount = new System.Windows.Forms.Label();
            this.panelTop.SuspendLayout();
            this.groupBoxPerson.SuspendLayout();
            this.groupBoxQuery.SuspendLayout();
            this.groupBoxResults.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProcesses)).BeginInit();
            this.SuspendLayout();
            //
            // panelTop
            //
            this.panelTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(150)))), ((int)(((byte)(243)))));
            this.panelTop.Controls.Add(this.lblTitle);
            this.panelTop.Controls.Add(this.lblStatus);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(1400, 70);
            this.panelTop.TabIndex = 0;
            //
            // lblTitle
            //
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(15, 15);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(475, 32);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Social Insurance Management System";
            //
            // lblStatus
            //
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblStatus.ForeColor = System.Drawing.Color.White;
            this.lblStatus.Location = new System.Drawing.Point(18, 47);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(172, 15);
            this.lblStatus.TabIndex = 1;
            this.lblStatus.Text = "Status: Connecting to service...";
            //
            // groupBoxPerson
            //
            this.groupBoxPerson.Controls.Add(this.lblSocialNumber);
            this.groupBoxPerson.Controls.Add(this.cboSocialNumber);
            this.groupBoxPerson.Controls.Add(this.lblFullName);
            this.groupBoxPerson.Controls.Add(this.txtFullName);
            this.groupBoxPerson.Controls.Add(this.lblAddress);
            this.groupBoxPerson.Controls.Add(this.txtAddress);
            this.groupBoxPerson.Controls.Add(this.lblExpDate);
            this.groupBoxPerson.Controls.Add(this.txtExpDate);
            this.groupBoxPerson.Controls.Add(this.lblTreatmentLocation);
            this.groupBoxPerson.Controls.Add(this.txtTreatmentLocation);
            this.groupBoxPerson.Controls.Add(this.lblDescription);
            this.groupBoxPerson.Controls.Add(this.txtDescription);
            this.groupBoxPerson.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.groupBoxPerson.Location = new System.Drawing.Point(15, 85);
            this.groupBoxPerson.Name = "groupBoxPerson";
            this.groupBoxPerson.Size = new System.Drawing.Size(450, 280);
            this.groupBoxPerson.TabIndex = 1;
            this.groupBoxPerson.TabStop = false;
            this.groupBoxPerson.Text = "Person Information";
            //
            // lblSocialNumber
            //
            this.lblSocialNumber.AutoSize = true;
            this.lblSocialNumber.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblSocialNumber.Location = new System.Drawing.Point(15, 30);
            this.lblSocialNumber.Name = "lblSocialNumber";
            this.lblSocialNumber.Size = new System.Drawing.Size(90, 15);
            this.lblSocialNumber.TabIndex = 0;
            this.lblSocialNumber.Text = "Social Number:";
            //
            // cboSocialNumber
            //
            this.cboSocialNumber.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSocialNumber.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cboSocialNumber.FormattingEnabled = true;
            this.cboSocialNumber.Location = new System.Drawing.Point(150, 27);
            this.cboSocialNumber.Name = "cboSocialNumber";
            this.cboSocialNumber.Size = new System.Drawing.Size(280, 23);
            this.cboSocialNumber.TabIndex = 1;
            this.cboSocialNumber.SelectedIndexChanged += new System.EventHandler(this.cboSocialNumber_SelectedIndexChanged);
            //
            // lblFullName
            //
            this.lblFullName.AutoSize = true;
            this.lblFullName.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblFullName.Location = new System.Drawing.Point(15, 65);
            this.lblFullName.Name = "lblFullName";
            this.lblFullName.Size = new System.Drawing.Size(64, 15);
            this.lblFullName.TabIndex = 2;
            this.lblFullName.Text = "Full Name:";
            //
            // txtFullName
            //
            this.txtFullName.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtFullName.Location = new System.Drawing.Point(150, 62);
            this.txtFullName.Name = "txtFullName";
            this.txtFullName.ReadOnly = true;
            this.txtFullName.Size = new System.Drawing.Size(280, 23);
            this.txtFullName.TabIndex = 3;
            //
            // lblAddress
            //
            this.lblAddress.AutoSize = true;
            this.lblAddress.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblAddress.Location = new System.Drawing.Point(15, 100);
            this.lblAddress.Name = "lblAddress";
            this.lblAddress.Size = new System.Drawing.Size(52, 15);
            this.lblAddress.TabIndex = 4;
            this.lblAddress.Text = "Address:";
            //
            // txtAddress
            //
            this.txtAddress.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtAddress.Location = new System.Drawing.Point(150, 97);
            this.txtAddress.Name = "txtAddress";
            this.txtAddress.ReadOnly = true;
            this.txtAddress.Size = new System.Drawing.Size(280, 23);
            this.txtAddress.TabIndex = 5;
            //
            // lblExpDate
            //
            this.lblExpDate.AutoSize = true;
            this.lblExpDate.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblExpDate.Location = new System.Drawing.Point(15, 135);
            this.lblExpDate.Name = "lblExpDate";
            this.lblExpDate.Size = new System.Drawing.Size(96, 15);
            this.lblExpDate.TabIndex = 6;
            this.lblExpDate.Text = "Expiration Date:";
            //
            // txtExpDate
            //
            this.txtExpDate.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtExpDate.Location = new System.Drawing.Point(150, 132);
            this.txtExpDate.Name = "txtExpDate";
            this.txtExpDate.ReadOnly = true;
            this.txtExpDate.Size = new System.Drawing.Size(280, 23);
            this.txtExpDate.TabIndex = 7;
            //
            // lblTreatmentLocation
            //
            this.lblTreatmentLocation.AutoSize = true;
            this.lblTreatmentLocation.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblTreatmentLocation.Location = new System.Drawing.Point(15, 170);
            this.lblTreatmentLocation.Name = "lblTreatmentLocation";
            this.lblTreatmentLocation.Size = new System.Drawing.Size(118, 15);
            this.lblTreatmentLocation.TabIndex = 8;
            this.lblTreatmentLocation.Text = "Treatment Location:";
            //
            // txtTreatmentLocation
            //
            this.txtTreatmentLocation.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtTreatmentLocation.Location = new System.Drawing.Point(150, 167);
            this.txtTreatmentLocation.Name = "txtTreatmentLocation";
            this.txtTreatmentLocation.ReadOnly = true;
            this.txtTreatmentLocation.Size = new System.Drawing.Size(280, 23);
            this.txtTreatmentLocation.TabIndex = 9;
            //
            // lblDescription
            //
            this.lblDescription.AutoSize = true;
            this.lblDescription.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblDescription.Location = new System.Drawing.Point(15, 205);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(70, 15);
            this.lblDescription.TabIndex = 10;
            this.lblDescription.Text = "Description:";
            //
            // txtDescription
            //
            this.txtDescription.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtDescription.Location = new System.Drawing.Point(150, 202);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.ReadOnly = true;
            this.txtDescription.Size = new System.Drawing.Size(280, 60);
            this.txtDescription.TabIndex = 11;
            //
            // groupBoxQuery
            //
            this.groupBoxQuery.Controls.Add(this.btnGetAll);
            this.groupBoxQuery.Controls.Add(this.lblBeginDate);
            this.groupBoxQuery.Controls.Add(this.dtpBeginDate);
            this.groupBoxQuery.Controls.Add(this.lblEndDate);
            this.groupBoxQuery.Controls.Add(this.dtpEndDate);
            this.groupBoxQuery.Controls.Add(this.btnGetByDate);
            this.groupBoxQuery.Controls.Add(this.lblCompany);
            this.groupBoxQuery.Controls.Add(this.txtCompany);
            this.groupBoxQuery.Controls.Add(this.btnGetByCompany);
            this.groupBoxQuery.Controls.Add(this.btnClear);
            this.groupBoxQuery.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.groupBoxQuery.Location = new System.Drawing.Point(480, 85);
            this.groupBoxQuery.Name = "groupBoxQuery";
            this.groupBoxQuery.Size = new System.Drawing.Size(905, 280);
            this.groupBoxQuery.TabIndex = 2;
            this.groupBoxQuery.TabStop = false;
            this.groupBoxQuery.Text = "Query Options";
            //
            // btnGetAll
            //
            this.btnGetAll.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(150)))), ((int)(((byte)(243)))));
            this.btnGetAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGetAll.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnGetAll.ForeColor = System.Drawing.Color.White;
            this.btnGetAll.Location = new System.Drawing.Point(20, 40);
            this.btnGetAll.Name = "btnGetAll";
            this.btnGetAll.Size = new System.Drawing.Size(250, 40);
            this.btnGetAll.TabIndex = 0;
            this.btnGetAll.Text = "Get All by Social Number";
            this.btnGetAll.UseVisualStyleBackColor = false;
            this.btnGetAll.Click += new System.EventHandler(this.btnGetAll_Click);
            //
            // lblBeginDate
            //
            this.lblBeginDate.AutoSize = true;
            this.lblBeginDate.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblBeginDate.Location = new System.Drawing.Point(20, 105);
            this.lblBeginDate.Name = "lblBeginDate";
            this.lblBeginDate.Size = new System.Drawing.Size(70, 15);
            this.lblBeginDate.TabIndex = 1;
            this.lblBeginDate.Text = "Begin Date:";
            //
            // dtpBeginDate
            //
            this.dtpBeginDate.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.dtpBeginDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpBeginDate.Location = new System.Drawing.Point(110, 100);
            this.dtpBeginDate.Name = "dtpBeginDate";
            this.dtpBeginDate.Size = new System.Drawing.Size(160, 23);
            this.dtpBeginDate.TabIndex = 2;
            //
            // lblEndDate
            //
            this.lblEndDate.AutoSize = true;
            this.lblEndDate.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblEndDate.Location = new System.Drawing.Point(290, 105);
            this.lblEndDate.Name = "lblEndDate";
            this.lblEndDate.Size = new System.Drawing.Size(57, 15);
            this.lblEndDate.TabIndex = 3;
            this.lblEndDate.Text = "End Date:";
            //
            // dtpEndDate
            //
            this.dtpEndDate.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.dtpEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpEndDate.Location = new System.Drawing.Point(360, 100);
            this.dtpEndDate.Name = "dtpEndDate";
            this.dtpEndDate.Size = new System.Drawing.Size(160, 23);
            this.dtpEndDate.TabIndex = 4;
            //
            // btnGetByDate
            //
            this.btnGetByDate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(175)))), ((int)(((byte)(80)))));
            this.btnGetByDate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGetByDate.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnGetByDate.ForeColor = System.Drawing.Color.White;
            this.btnGetByDate.Location = new System.Drawing.Point(540, 90);
            this.btnGetByDate.Name = "btnGetByDate";
            this.btnGetByDate.Size = new System.Drawing.Size(340, 40);
            this.btnGetByDate.TabIndex = 5;
            this.btnGetByDate.Text = "Get by Date Range";
            this.btnGetByDate.UseVisualStyleBackColor = false;
            this.btnGetByDate.Click += new System.EventHandler(this.btnGetByDate_Click);
            //
            // lblCompany
            //
            this.lblCompany.AutoSize = true;
            this.lblCompany.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblCompany.Location = new System.Drawing.Point(20, 165);
            this.lblCompany.Name = "lblCompany";
            this.lblCompany.Size = new System.Drawing.Size(63, 15);
            this.lblCompany.TabIndex = 6;
            this.lblCompany.Text = "Company:";
            //
            // txtCompany
            //
            this.txtCompany.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtCompany.Location = new System.Drawing.Point(110, 160);
            this.txtCompany.Name = "txtCompany";
            this.txtCompany.Size = new System.Drawing.Size(410, 23);
            this.txtCompany.TabIndex = 7;
            //
            // btnGetByCompany
            //
            this.btnGetByCompany.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(152)))), ((int)(((byte)(0)))));
            this.btnGetByCompany.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGetByCompany.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnGetByCompany.ForeColor = System.Drawing.Color.White;
            this.btnGetByCompany.Location = new System.Drawing.Point(540, 150);
            this.btnGetByCompany.Name = "btnGetByCompany";
            this.btnGetByCompany.Size = new System.Drawing.Size(340, 40);
            this.btnGetByCompany.TabIndex = 8;
            this.btnGetByCompany.Text = "Get by Company Name";
            this.btnGetByCompany.UseVisualStyleBackColor = false;
            this.btnGetByCompany.Click += new System.EventHandler(this.btnGetByCompany_Click);
            //
            // btnClear
            //
            this.btnClear.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(158)))), ((int)(((byte)(158)))), ((int)(((byte)(158)))));
            this.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClear.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnClear.ForeColor = System.Drawing.Color.White;
            this.btnClear.Location = new System.Drawing.Point(20, 220);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(860, 40);
            this.btnClear.TabIndex = 9;
            this.btnClear.Text = "Clear / Reset";
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            //
            // groupBoxResults
            //
            this.groupBoxResults.Controls.Add(this.dgvProcesses);
            this.groupBoxResults.Controls.Add(this.lblRecordCount);
            this.groupBoxResults.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.groupBoxResults.Location = new System.Drawing.Point(15, 380);
            this.groupBoxResults.Name = "groupBoxResults";
            this.groupBoxResults.Size = new System.Drawing.Size(1370, 370);
            this.groupBoxResults.TabIndex = 3;
            this.groupBoxResults.TabStop = false;
            this.groupBoxResults.Text = "Insurance Process Records";
            //
            // dgvProcesses
            //
            this.dgvProcesses.AllowUserToAddRows = false;
            this.dgvProcesses.AllowUserToDeleteRows = false;
            this.dgvProcesses.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvProcesses.BackgroundColor = System.Drawing.Color.White;
            this.dgvProcesses.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvProcesses.Location = new System.Drawing.Point(15, 30);
            this.dgvProcesses.Name = "dgvProcesses";
            this.dgvProcesses.ReadOnly = true;
            this.dgvProcesses.RowHeadersWidth = 25;
            this.dgvProcesses.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvProcesses.Size = new System.Drawing.Size(1340, 290);
            this.dgvProcesses.TabIndex = 0;
            //
            // lblRecordCount
            //
            this.lblRecordCount.AutoSize = true;
            this.lblRecordCount.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblRecordCount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(150)))), ((int)(((byte)(243)))));
            this.lblRecordCount.Location = new System.Drawing.Point(15, 330);
            this.lblRecordCount.Name = "lblRecordCount";
            this.lblRecordCount.Size = new System.Drawing.Size(129, 19);
            this.lblRecordCount.TabIndex = 1;
            this.lblRecordCount.Text = "Total Records: 0";
            //
            // FrmSocialInsurance
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1400, 765);
            this.Controls.Add(this.groupBoxResults);
            this.Controls.Add(this.groupBoxQuery);
            this.Controls.Add(this.groupBoxPerson);
            this.Controls.Add(this.panelTop);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FrmSocialInsurance";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Social Insurance Management - WCF Client";
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.groupBoxPerson.ResumeLayout(false);
            this.groupBoxPerson.PerformLayout();
            this.groupBoxQuery.ResumeLayout(false);
            this.groupBoxQuery.PerformLayout();
            this.groupBoxResults.ResumeLayout(false);
            this.groupBoxResults.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProcesses)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.GroupBox groupBoxPerson;
        private System.Windows.Forms.Label lblSocialNumber;
        private System.Windows.Forms.ComboBox cboSocialNumber;
        private System.Windows.Forms.Label lblFullName;
        private System.Windows.Forms.TextBox txtFullName;
        private System.Windows.Forms.Label lblAddress;
        private System.Windows.Forms.TextBox txtAddress;
        private System.Windows.Forms.Label lblExpDate;
        private System.Windows.Forms.TextBox txtExpDate;
        private System.Windows.Forms.Label lblTreatmentLocation;
        private System.Windows.Forms.TextBox txtTreatmentLocation;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.GroupBox groupBoxQuery;
        private System.Windows.Forms.Button btnGetAll;
        private System.Windows.Forms.Label lblBeginDate;
        private System.Windows.Forms.DateTimePicker dtpBeginDate;
        private System.Windows.Forms.Label lblEndDate;
        private System.Windows.Forms.DateTimePicker dtpEndDate;
        private System.Windows.Forms.Button btnGetByDate;
        private System.Windows.Forms.Label lblCompany;
        private System.Windows.Forms.TextBox txtCompany;
        private System.Windows.Forms.Button btnGetByCompany;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.GroupBox groupBoxResults;
        private System.Windows.Forms.DataGridView dgvProcesses;
        private System.Windows.Forms.Label lblRecordCount;
    }
}
