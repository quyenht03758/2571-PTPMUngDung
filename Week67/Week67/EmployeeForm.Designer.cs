using System.Drawing;
using System.Windows.Forms;

namespace Week67
{
    partial class EmployeeForm
    {
        private System.ComponentModel.IContainer components = null;

        private DataGridView dgv;

        private GroupBox group;
        private TableLayoutPanel layout;
        private Label lblSsn, lblFname, lblMinit, lblLname, lblBdate, lblAddress, lblSex, lblSalary, lblSuper, lblDno;
        private TextBox txtSsn, txtFname, txtMinit, txtLname, txtAddress, txtSex, txtSalary, txtSuper;
        private DateTimePicker dtpBdate;
        private ComboBox cboDno;

        private FlowLayoutPanel bar;
        private Button btnAdd, btnUpdate, btnDelete, btnReload, btnClose;

        protected override void Dispose(bool disposing) { if (disposing && (components != null)) components.Dispose(); base.Dispose(disposing); }

        private void InitializeComponent()
        {
            dgv = new DataGridView
            {
                Dock = DockStyle.Top,
                Height = 260,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                ReadOnly = true
            };

            group = new GroupBox { Text = "Employee Details", Dock = DockStyle.Fill, Font = new Font("Segoe UI", 9.5f) };

            layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 4,
                RowCount = 5,
                Padding = new Padding(10)
            };
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            for (int i = 0; i < 5; i++) layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 36));

            lblSsn = new Label { Text = "Ssn:", AutoSize = true };
            lblFname = new Label { Text = "Fname:", AutoSize = true };
            lblMinit = new Label { Text = "Middle initial:", AutoSize = true };
            lblLname = new Label { Text = "Lname:", AutoSize = true };
            lblBdate = new Label { Text = "Bdate:", AutoSize = true };
            lblAddress = new Label { Text = "Address:", AutoSize = true };
            lblSex = new Label { Text = "Sex:", AutoSize = true };
            lblSalary = new Label { Text = "Salary:", AutoSize = true };
            lblSuper = new Label { Text = "Supervisor SSN:", AutoSize = true };
            lblDno = new Label { Text = "Department:", AutoSize = true };

            txtSsn = new TextBox(); txtFname = new TextBox();
            txtMinit = new TextBox(); txtLname = new TextBox();
            dtpBdate = new DateTimePicker();
            txtAddress = new TextBox(); txtSex = new TextBox();
            txtSalary = new TextBox(); txtSuper = new TextBox();
            cboDno = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList };

            // Row 1
            layout.Controls.Add(lblSsn, 0, 0); layout.Controls.Add(txtSsn, 1, 0);
            layout.Controls.Add(lblFname, 2, 0); layout.Controls.Add(txtFname, 3, 0);
            // Row 2
            layout.Controls.Add(lblMinit, 0, 1); layout.Controls.Add(txtMinit, 1, 1);
            layout.Controls.Add(lblLname, 2, 1); layout.Controls.Add(txtLname, 3, 1);
            // Row 3
            layout.Controls.Add(lblBdate, 0, 2); layout.Controls.Add(dtpBdate, 1, 2);
            layout.Controls.Add(lblAddress, 2, 2); layout.Controls.Add(txtAddress, 3, 2);
            // Row 4
            layout.Controls.Add(lblSex, 0, 3); layout.Controls.Add(txtSex, 1, 3);
            layout.Controls.Add(lblSalary, 2, 3); layout.Controls.Add(txtSalary, 3, 3);
            // Row 5
            layout.Controls.Add(lblSuper, 0, 4); layout.Controls.Add(txtSuper, 1, 4);
            layout.Controls.Add(lblDno, 2, 4); layout.Controls.Add(cboDno, 3, 4);

            bar = new FlowLayoutPanel { Dock = DockStyle.Bottom, Height = 48, FlowDirection = FlowDirection.RightToLeft, Padding = new Padding(10, 8, 10, 8) };
            btnAdd = new Button { Text = "Add", Width = 90, Height = 30 };
            btnUpdate = new Button { Text = "Update", Width = 90, Height = 30 };
            btnDelete = new Button { Text = "Delete", Width = 90, Height = 30 };
            btnReload = new Button { Text = "Reload", Width = 90, Height = 30 };
            btnClose = new Button { Text = "Close", Width = 90, Height = 30 };
            bar.Controls.AddRange(new Control[] { btnClose, btnReload, btnDelete, btnUpdate, btnAdd });

            group.Controls.Add(layout);
            group.Controls.Add(bar);

            this.Text = "Employees";
            this.StartPosition = FormStartPosition.CenterParent;
            this.MinimumSize = new Size(900, 600);
            this.Controls.Add(group);
            this.Controls.Add(dgv);
        }
    }
}
