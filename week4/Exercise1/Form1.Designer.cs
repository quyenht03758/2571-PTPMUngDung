namespace EmployeeManager
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.DataGridView dgvEmployees;
        private System.Windows.Forms.TextBox txtId;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.TextBox txtAddress;
        private System.Windows.Forms.DateTimePicker dtpBirthday;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.TextBox txtPhone;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label lblId;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblAddress;
        private System.Windows.Forms.Label lblBirthday;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.Label lblPhone;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.dgvEmployees = new System.Windows.Forms.DataGridView();
            this.txtId = new System.Windows.Forms.TextBox();
            this.txtName = new System.Windows.Forms.TextBox();
            this.txtAddress = new System.Windows.Forms.TextBox();
            this.dtpBirthday = new System.Windows.Forms.DateTimePicker();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.txtPhone = new System.Windows.Forms.TextBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.lblId = new System.Windows.Forms.Label();
            this.lblName = new System.Windows.Forms.Label();
            this.lblAddress = new System.Windows.Forms.Label();
            this.lblBirthday = new System.Windows.Forms.Label();
            this.lblEmail = new System.Windows.Forms.Label();
            this.lblPhone = new System.Windows.Forms.Label();

            ((System.ComponentModel.ISupportInitialize)(this.dgvEmployees)).BeginInit();
            this.SuspendLayout();

            // DataGridView
            this.dgvEmployees.Location = new System.Drawing.Point(20, 20);
            this.dgvEmployees.Name = "dgvEmployees";
            this.dgvEmployees.Size = new System.Drawing.Size(720, 200);
            this.dgvEmployees.TabIndex = 0;
            this.dgvEmployees.AutoGenerateColumns = false;
            this.dgvEmployees.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;

            // Labels
            this.lblId.Text = "Employee ID:";
            this.lblId.Location = new System.Drawing.Point(20, 240);

            this.lblName.Text = "Name:";
            this.lblName.Location = new System.Drawing.Point(20, 270);

            this.lblAddress.Text = "Address:";
            this.lblAddress.Location = new System.Drawing.Point(20, 300);

            this.lblBirthday.Text = "Birthday:";
            this.lblBirthday.Location = new System.Drawing.Point(20, 330);

            this.lblEmail.Text = "Email:";
            this.lblEmail.Location = new System.Drawing.Point(20, 360);

            this.lblPhone.Text = "Phone:";
            this.lblPhone.Location = new System.Drawing.Point(20, 390);

            // TextBoxes + DateTimePicker
            this.txtId.Location = new System.Drawing.Point(120, 240);
            this.txtId.Width = 200;

            this.txtName.Location = new System.Drawing.Point(120, 270);
            this.txtName.Width = 200;

            this.txtAddress.Location = new System.Drawing.Point(120, 300);
            this.txtAddress.Width = 200;

            this.dtpBirthday.Location = new System.Drawing.Point(120, 330);
            this.dtpBirthday.Width = 200;

            this.txtEmail.Location = new System.Drawing.Point(120, 360);
            this.txtEmail.Width = 200;

            this.txtPhone.Location = new System.Drawing.Point(120, 390);
            this.txtPhone.Width = 200;

            // Buttons
            this.btnAdd.Text = "Add";
            this.btnAdd.Location = new System.Drawing.Point(400, 240);

            this.btnUpdate.Text = "Update";
            this.btnUpdate.Location = new System.Drawing.Point(400, 280);

            this.btnDelete.Text = "Delete";
            this.btnDelete.Location = new System.Drawing.Point(400, 320);

            this.btnExit.Text = "Exit";
            this.btnExit.Location = new System.Drawing.Point(400, 360);

            // Form1
            this.ClientSize = new System.Drawing.Size(760, 450);
            this.Controls.Add(this.dgvEmployees);
            this.Controls.Add(this.lblId);
            this.Controls.Add(this.txtId);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.lblAddress);
            this.Controls.Add(this.txtAddress);
            this.Controls.Add(this.lblBirthday);
            this.Controls.Add(this.dtpBirthday);
            this.Controls.Add(this.lblEmail);
            this.Controls.Add(this.txtEmail);
            this.Controls.Add(this.lblPhone);
            this.Controls.Add(this.txtPhone);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnExit);
            this.Text = "Employee Manager";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;

            ((System.ComponentModel.ISupportInitialize)(this.dgvEmployees)).EndInit();
            this.ResumeLayout(false);
        }
    }
}

