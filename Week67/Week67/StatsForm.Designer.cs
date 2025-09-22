using System.Drawing;
using System.Windows.Forms;

namespace Week67
{
    partial class StatsForm
    {
        private System.ComponentModel.IContainer components = null;
        private Panel header;
        private Label title;
        private Panel content;
        private GroupBox groupQueries;
        private FlowLayoutPanel buttonsBar;
        private Button btnBirthMonth, btnProjectsByDept, btnHoursByEmp;
        private DataGridView dgv;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            header = new Panel { Dock = DockStyle.Top, Height = 56, BackColor = Color.FromArgb(52, 120, 200) };
            title = new Label
            {
                Text = "Statistics",
                ForeColor = Color.White,
                AutoSize = true,
                Font = new Font("Segoe UI", 16f, FontStyle.Bold),
                Location = new Point(16, 12)
            };
            header.Controls.Add(title);

            content = new Panel { Dock = DockStyle.Fill, Padding = new Padding(12) };

            groupQueries = new GroupBox
            {
                Text = "Queries",
                Dock = DockStyle.Top,
                Height = 72,
                Font = new Font("Segoe UI", 10f, FontStyle.Regular)
            };

            buttonsBar = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight,
                Padding = new Padding(8, 8, 8, 0)
            };

            btnBirthMonth = MakeSmallButton("Birthdays this month");
            btnProjectsByDept = MakeSmallButton("Projects by Department");
            btnHoursByEmp = MakeSmallButton("Hours by Employee/Project");

            buttonsBar.Controls.AddRange(new Control[] { btnBirthMonth, btnProjectsByDept, btnHoursByEmp });
            groupQueries.Controls.Add(buttonsBar);

            dgv = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                EnableHeadersVisualStyles = false,
                ReadOnly = true
            };
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(235, 240, 245);
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 9F);

            content.Controls.Add(dgv);
            content.Controls.Add(groupQueries);

            this.Text = "Statistics";
            this.StartPosition = FormStartPosition.CenterParent;
            this.MinimumSize = new Size(820, 560);

            this.Controls.Add(content);
            this.Controls.Add(header);
        }

        private Button MakeSmallButton(string text)
        {
            return new Button
            {
                Text = text,
                Height = 32,
                Width = 220,
                Margin = new Padding(6, 4, 0, 4),
                FlatStyle = FlatStyle.System
            };
        }
    }
}
