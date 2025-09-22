using System.Drawing;
using System.Windows.Forms;

namespace Week67
{
    partial class DepartmentForm
    {
        private System.ComponentModel.IContainer components = null;
        private DataGridView dgv;
        private GroupBox group;
        private Label lblDno, lblDname, lblMgrSSN, lblMgrStart;
        private TextBox txtDnumber, txtDname, txtMgrSSN;
        private DateTimePicker dtpMgrStart;
        private FlowLayoutPanel bar;
        private Button btnAdd, btnUpdate, btnDelete, btnReload, btnClose;

        protected override void Dispose(bool disposing) { if (disposing && (components != null)) components.Dispose(); base.Dispose(disposing); }

        private void InitializeComponent()
        {
            dgv = new DataGridView { Dock = DockStyle.Top, Height = 240, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, SelectionMode = DataGridViewSelectionMode.FullRowSelect, ReadOnly = true };
            group = new GroupBox { Text = "Department Details", Dock = DockStyle.Fill, Font = new Font("Segoe UI", 9.5f) };

            lblDno = new Label { Text = "Dnumber:", AutoSize = true };
            lblDname = new Label { Text = "Dname:", AutoSize = true };
            lblMgrSSN = new Label { Text = "MgrSSN:", AutoSize = true };
            lblMgrStart = new Label { Text = "MgrStartDate:", AutoSize = true };

            txtDnumber = new TextBox();
            txtDname = new TextBox();
            txtMgrSSN = new TextBox();
            dtpMgrStart = new DateTimePicker();

            var grid = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 4, RowCount = 2, Padding = new Padding(10) };
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110));
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110));
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            grid.RowStyles.Add(new RowStyle(SizeType.Absolute, 36));
            grid.RowStyles.Add(new RowStyle(SizeType.Absolute, 36));

            grid.Controls.Add(lblDno, 0, 0); grid.Controls.Add(txtDnumber, 1, 0);
            grid.Controls.Add(lblDname, 2, 0); grid.Controls.Add(txtDname, 3, 0);
            grid.Controls.Add(lblMgrSSN, 0, 1); grid.Controls.Add(txtMgrSSN, 1, 1);
            grid.Controls.Add(lblMgrStart, 2, 1); grid.Controls.Add(dtpMgrStart, 3, 1);

            bar = new FlowLayoutPanel { Dock = DockStyle.Bottom, Height = 48, FlowDirection = FlowDirection.RightToLeft, Padding = new Padding(10, 8, 10, 8) };
            btnAdd = new Button { Text = "Add", Width = 90, Height = 30 };
            btnUpdate = new Button { Text = "Update", Width = 90, Height = 30 };
            btnDelete = new Button { Text = "Delete", Width = 90, Height = 30 };
            btnReload = new Button { Text = "Reload", Width = 90, Height = 30 };
            btnClose = new Button { Text = "Close", Width = 90, Height = 30 };
            bar.Controls.AddRange(new Control[] { btnClose, btnReload, btnDelete, btnUpdate, btnAdd });

            group.Controls.Add(grid);
            group.Controls.Add(bar);

            this.Text = "Departments";
            this.StartPosition = FormStartPosition.CenterParent;
            this.MinimumSize = new Size(820, 520);
            this.Controls.Add(group);
            this.Controls.Add(dgv);
        }
    }
}
