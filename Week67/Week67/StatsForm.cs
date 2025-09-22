using System;
using System.Windows.Forms;

namespace Week67
{
    public partial class StatsForm : Form
    {
        private ToolTip tips;

        public StatsForm()
        {
            InitializeComponent();

            // ToolTip setup
            tips = new ToolTip
            {
                InitialDelay = 0,
                ReshowDelay = 0,
                AutoPopDelay = 10000,
                ShowAlways = true
            };
            tips.SetToolTip(btnBirthMonth, "Show list of employees with birthdays this month");
            tips.SetToolTip(btnProjectsByDept, "Show projects grouped by department");
            tips.SetToolTip(btnHoursByEmp, "Show hours worked by employees on projects");
            tips.SetToolTip(dgv, "Results of the selected query will appear here");

            btnBirthMonth.Click += (s, e) => ShowBirthdays();
            btnProjectsByDept.Click += (s, e) => ShowProjectsByDept();
            btnHoursByEmp.Click += (s, e) => ShowHoursByEmp();
        }

        private void ShowBirthdays()
        {
            dgv.DataSource = Db.Query(@"
                SELECT e.Ssn AS [SSN],
                       e.Fname + ' ' + ISNULL(NULLIF(e.Minit,'' ) + '. ', '') + e.Lname AS [Full Name],
                       e.Bdate AS [Birth Date]
                FROM Employee e
                WHERE MONTH(e.Bdate) = MONTH(GETDATE())
                ORDER BY e.Lname, e.Fname
            ");
            dgv.Columns["Birth Date"].DefaultCellStyle.Format = "yyyy-MM-dd";
        }

        private void ShowProjectsByDept()
        {
            dgv.DataSource = Db.Query(@"
                SELECT d.Dnumber AS [Dept No],
                       d.Dname   AS [Department],
                       p.Pnumber AS [Project No],
                       p.Pname   AS [Project Name],
                       p.Plocation AS [Location]
                FROM Department d
                LEFT JOIN Project p ON p.Dnum = d.Dnumber
                ORDER BY d.Dnumber, p.Pnumber
            ");
        }

        private void ShowHoursByEmp()
        {
            dgv.DataSource = Db.Query(@"
                SELECT e.Ssn AS [SSN],
                       e.Fname + ' ' + e.Lname AS [Employee],
                       p.Pname AS [Project],
                       w.Hours AS [Hours]
                FROM WorksOn w
                JOIN Employee e ON e.Ssn = w.Essn
                JOIN Project  p ON p.Pnumber = w.Pno
                ORDER BY e.Ssn, p.Pnumber
            ");
            dgv.Columns["Hours"].DefaultCellStyle.Format = "N1";
        }
    }
}
