using System.Drawing;
using System.Windows.Forms;

namespace Week67
{
    partial class WorksOnForm
    {
        private System.ComponentModel.IContainer components = null;
        private DataGridView dgv;
        private GroupBox group;
        private Label lblEmp, lblProj, lblHours;
        private ComboBox cboEmp, cboProj;
        private TextBox txtHours;
        private FlowLayoutPanel bar;
        private Button btnAdd, btnUpdate, btnDelete, btnReload, btnClose;

        protected override void Dispose(bool disposing) { if (disposing && (components != null)) components.Dispose(); base.Dispose(disposing); }

        private void InitializeComponent()
        {
            dgv = new DataGridView { Dock = DockStyle.Top, Height = 260, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, SelectionMode = DataGridViewSelectionMode.FullRowSelect, ReadOnly = true };
            group = new GroupBox { Text = "Assignment Details", Dock = DockStyle.Fill, Font = new Font("Segoe UI", 9.5f) };

            lblEmp = new Label { Text = "Employee (SSN):", AutoSize = true };
            lblProj = new Label { Text = "Project (Pnumber):", AutoSize = true };
            lblHours = new Label { Text = "Hours:", AutoSize = true };

            cboEmp = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList };
            cboProj = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList };
            txtHours = new TextBox();

            var grid = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 4, RowCount = 2, Padding = new Padding(10) };
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 140));
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110));
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            grid.RowStyles.Add(new RowStyle(SizeType.Absolute, 36));
            grid.RowStyles.Add(new RowStyle(SizeType.Absolute, 36));

            grid.Controls.Add(lblEmp, 0, 0); grid.Controls.Add(cboEmp, 1, 0);
            grid.Controls.Add(lblProj, 2, 0); grid.Controls.Add(cboProj, 3, 0);
            grid.Controls.Add(lblHours, 0, 1); grid.Controls.Add(txtHours, 1, 1);

            bar = new FlowLayoutPanel { Dock = DockStyle.Bottom, Height = 48, FlowDirection = FlowDirection.RightToLeft, Padding = new Padding(10, 8, 10, 8) };
            btnAdd = new Button { Text = "Add", Width = 90, Height = 30 };
            btnUpdate = new Button { Text = "Update", Width = 90, Height = 30 };
            btnDelete = new Button { Text = "Delete", Width = 90, Height = 30 };
            btnReload = new Button { Text = "Reload", Width = 90, Height = 30 };
            btnClose = new Button { Text = "Close", Width = 90, Height = 30 };
            bar.Controls.AddRange(new Control[] { btnClose, btnReload, btnDelete, btnUpdate, btnAdd });

            group.Controls.Add(grid);
            group.Controls.Add(bar);

            this.Text = "Works On";
            this.StartPosition = FormStartPosition.CenterParent;
            this.MinimumSize = new Size(820, 520);
            this.Controls.Add(group);
            this.Controls.Add(dgv);
        }
    }
}
