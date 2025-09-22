using System.Drawing;
using System.Windows.Forms;

namespace Week67
{
    partial class ProjectForm
    {
        private System.ComponentModel.IContainer components = null;
        private DataGridView dgv;
        private GroupBox group;
        private Label lblPno, lblPname, lblLoc, lblDept;
        private TextBox txtPnumber, txtPname, txtLocation;
        private ComboBox cboDept;
        private FlowLayoutPanel bar;
        private Button btnAdd, btnUpdate, btnDelete, btnReload, btnClose;

        protected override void Dispose(bool disposing) { if (disposing && (components != null)) components.Dispose(); base.Dispose(disposing); }

        private void InitializeComponent()
        {
            dgv = new DataGridView { Dock = DockStyle.Top, Height = 240, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, SelectionMode = DataGridViewSelectionMode.FullRowSelect, ReadOnly = true };
            group = new GroupBox { Text = "Project Details", Dock = DockStyle.Fill, Font = new Font("Segoe UI", 9.5f) };

            lblPno = new Label { Text = "Pnumber:", AutoSize = true };
            lblPname = new Label { Text = "Pname:", AutoSize = true };
            lblLoc = new Label { Text = "Location:", AutoSize = true };
            lblDept = new Label { Text = "Department:", AutoSize = true };

            txtPnumber = new TextBox();
            txtPname = new TextBox();
            txtLocation = new TextBox();
            cboDept = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList };

            var grid = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 4, RowCount = 2, Padding = new Padding(10) };
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110));
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110));
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            grid.RowStyles.Add(new RowStyle(SizeType.Absolute, 36));
            grid.RowStyles.Add(new RowStyle(SizeType.Absolute, 36));

            grid.Controls.Add(lblPno, 0, 0); grid.Controls.Add(txtPnumber, 1, 0);
            grid.Controls.Add(lblPname, 2, 0); grid.Controls.Add(txtPname, 3, 0);
            grid.Controls.Add(lblLoc, 0, 1); grid.Controls.Add(txtLocation, 1, 1);
            grid.Controls.Add(lblDept, 2, 1); grid.Controls.Add(cboDept, 3, 1);

            bar = new FlowLayoutPanel { Dock = DockStyle.Bottom, Height = 48, FlowDirection = FlowDirection.RightToLeft, Padding = new Padding(10, 8, 10, 8) };
            btnAdd = new Button { Text = "Add", Width = 90, Height = 30 };
            btnUpdate = new Button { Text = "Update", Width = 90, Height = 30 };
            btnDelete = new Button { Text = "Delete", Width = 90, Height = 30 };
            btnReload = new Button { Text = "Reload", Width = 90, Height = 30 };
            btnClose = new Button { Text = "Close", Width = 90, Height = 30 };
            bar.Controls.AddRange(new Control[] { btnClose, btnReload, btnDelete, btnUpdate, btnAdd });

            group.Controls.Add(grid);
            group.Controls.Add(bar);

            this.Text = "Projects";
            this.StartPosition = FormStartPosition.CenterParent;
            this.MinimumSize = new Size(820, 520);
            this.Controls.Add(group);
            this.Controls.Add(dgv);
        }
    }
}
