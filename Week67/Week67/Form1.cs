using System.Drawing;
using System.Windows.Forms;
using Week67;

namespace Week67
{
    public class MainForm : Form
    {
        private Panel header;
        private Label title;
        private Panel content;
        private TableLayoutPanel grid;
        private GroupBox groupActions;

        private Button btnEmp, btnDept, btnProj, btnWork, btnStats;

        public MainForm()
        {
            Text = "Company Manager (ADO.NET)";
            StartPosition = FormStartPosition.CenterScreen;
            MinimumSize = new Size(860, 520);

            header = new Panel { Dock = DockStyle.Top, Height = 60, BackColor = Color.FromArgb(52, 120, 200) };
            title = new Label
            {
                Text = "Company Manager",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 18f, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(16, 14)
            };
            header.Controls.Add(title);

            content = new Panel { Dock = DockStyle.Fill, Padding = new Padding(16) };

            grid = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 2 };
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            grid.RowStyles.Add(new RowStyle(SizeType.Percent, 60));
            grid.RowStyles.Add(new RowStyle(SizeType.Percent, 40));

            groupActions = new GroupBox { Text = "Open Modules", Dock = DockStyle.Fill, Font = new Font("Segoe UI", 10f, FontStyle.Regular) };

            var actions = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 3, RowCount = 2, Padding = new Padding(12) };
            for (int i = 0; i < 3; i++) actions.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3f));
            actions.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
            actions.RowStyles.Add(new RowStyle(SizeType.Percent, 50));

            btnEmp = MakeBig("Employees");
            btnDept = MakeBig("Departments");
            btnProj = MakeBig("Projects");
            btnWork = MakeBig("Works On");
            btnStats = MakeBig("Statistics");

            actions.Controls.Add(btnEmp, 0, 0);
            actions.Controls.Add(btnDept, 1, 0);
            actions.Controls.Add(btnProj, 2, 0);
            actions.Controls.Add(btnWork, 0, 1);
            actions.Controls.Add(btnStats, 1, 1);

            groupActions.Controls.Add(actions);
            grid.Controls.Add(groupActions, 0, 0);
            grid.SetColumnSpan(groupActions, 2);
            content.Controls.Add(grid);

            Controls.Add(content);
            Controls.Add(header);

            btnEmp.Click += (s, e) => new EmployeeForm().ShowDialog(this);
            btnDept.Click += (s, e) => new DepartmentForm().ShowDialog(this);
            btnProj.Click += (s, e) => new ProjectForm().ShowDialog(this);
            btnWork.Click += (s, e) => new WorksOnForm().ShowDialog(this);
            btnStats.Click += (s, e) => new StatsForm().ShowDialog(this);
        }

        private Button MakeBig(string text)
        {
            return new Button { Text = text, Dock = DockStyle.Fill, Font = new Font("Segoe UI", 12f, FontStyle.Bold), Margin = new Padding(8) };
        }
    }
}
