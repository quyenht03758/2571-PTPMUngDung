using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Windows.Forms;


namespace Week67
{
    public partial class WorksOnForm : Form
    {
        DataTable _table, _emp, _proj;
        private ToolTip tips;

        public WorksOnForm()
        {
            InitializeComponent();

            tips = new ToolTip
            {
                InitialDelay = 0,
                ReshowDelay = 0,
                AutoPopDelay = 10000,
                ShowAlways = true
            };
            tips.SetToolTip(cboEmp, "Select an employee");
            tips.SetToolTip(cboProj, "Select a project");
            tips.SetToolTip(txtHours, "Number of hours worked on this project");
            tips.SetToolTip(dgv, "List of assignments (employee ↔ project)");
            tips.SetToolTip(btnAdd, "Assign employee to project");
            tips.SetToolTip(btnUpdate, "Update hours worked");
            tips.SetToolTip(btnDelete, "Remove assignment");
            tips.SetToolTip(btnReload, "Reload list from database");
            tips.SetToolTip(btnClose, "Close this window");

            Load += (s, e) => { LoadLookups(); Reload(); };
            dgv.SelectionChanged += (s, e) => Pull();
            btnReload.Click += (s, e) => Reload();
            btnAdd.Click += (s, e) => Add();
            btnUpdate.Click += (s, e) => UpdateRow();
            btnDelete.Click += (s, e) => DeleteRow();
            btnClose.Click += (s, e) => Close();
        }

        void LoadLookups()
        {
            _emp = Db.Query("SELECT SSN, (Fname + ' ' + Lname) AS FullName FROM Employee ORDER BY SSN");
            cboEmp.DataSource = _emp; cboEmp.DisplayMember = "FullName"; cboEmp.ValueMember = "SSN";

            _proj = Db.Query("SELECT Pnumber, Pname FROM Project ORDER BY Pnumber");
            cboProj.DataSource = _proj; cboProj.DisplayMember = "Pname"; cboProj.ValueMember = "Pnumber";
        }

        void Reload()
        {
            _table = Db.Query(@"
                SELECT w.Essn, 
                    e.Fname + ' ' + e.Lname AS Employee,
                    w.Pno,
                    p.Pname AS Project,
                    w.Hours
                FROM WorksOn w
                JOIN Employee e ON e.Ssn = w.Essn
                JOIN Project  p ON p.Pnumber = w.Pno
                ORDER BY e.Ssn, w.Pno");
            dgv.DataSource = _table;

            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.Columns["Employee"].HeaderText = "Employee";
            dgv.Columns["Project"].HeaderText = "Project";
            dgv.Columns["Hours"].HeaderText = "Hours";
            dgv.Columns["Essn"].Visible = false;
            dgv.Columns["Pno"].Visible = false;
        }


        void Pull()
        {
            if (dgv.CurrentRow == null) return;
            var r = (dgv.CurrentRow.DataBoundItem as DataRowView)?.Row;
            if (r == null) return;
            cboEmp.SelectedValue = r.Field<string>("Essn");
            cboProj.SelectedValue = r.Field<int>("Pno");
            txtHours.Text = r.Field<decimal>("Hours").ToString(CultureInfo.InvariantCulture);
        }

        bool ValidateInputs(bool forUpdate)
        {
            if (cboEmp.SelectedValue == null) { MessageBox.Show("Select Employee."); return false; }
            if (cboProj.SelectedValue == null) { MessageBox.Show("Select Project."); return false; }
            if (!decimal.TryParse(txtHours.Text.Trim(), NumberStyles.Number, CultureInfo.InvariantCulture, out _))
            { MessageBox.Show("Hours must be number."); return false; }

            if (!forUpdate)
            {
                var ex = Db.Query("SELECT 1 FROM WorksOn WHERE Essn=@e AND Pno=@p",
                    new SqlParameter("@e", (string)cboEmp.SelectedValue),
                    new SqlParameter("@p", (int)cboProj.SelectedValue));
                if (ex.Rows.Count > 0) { MessageBox.Show("Assignment already exists."); return false; }
            }
            return true;
        }

        void Add()
        {
            if (!ValidateInputs(false)) return;
            var rows = Db.Execute(@"INSERT INTO WorksOn(Essn,Pno,Hours)
                                    VALUES(@e,@p,@h)",
                        new SqlParameter("@e", (string)cboEmp.SelectedValue),
                        new SqlParameter("@p", (int)cboProj.SelectedValue),
                        new SqlParameter("@h", decimal.Parse(txtHours.Text.Trim(), CultureInfo.InvariantCulture)));
            if (rows > 0) { Reload(); MessageBox.Show("Added!"); }
        }

        void UpdateRow()
        {
            if (!ValidateInputs(true)) return;
            var rows = Db.Execute(@"UPDATE WorksOn SET Hours=@h WHERE Essn=@e AND Pno=@p",
                        new SqlParameter("@h", decimal.Parse(txtHours.Text.Trim(), CultureInfo.InvariantCulture)),
                        new SqlParameter("@e", (string)cboEmp.SelectedValue),
                        new SqlParameter("@p", (int)cboProj.SelectedValue));
            if (rows > 0) { Reload(); MessageBox.Show("Updated!"); }
        }

        void DeleteRow()
        {
            if (dgv.CurrentRow == null) return;
            var e = (string)cboEmp.SelectedValue;
            var p = (int)cboProj.SelectedValue;
            if (MessageBox.Show($"Delete assignment {e}-{p}?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.Yes) return;
            var rows = Db.Execute("DELETE FROM WorksOn WHERE Essn=@e AND Pno=@p",
                        new SqlParameter("@e", e), new SqlParameter("@p", p));
            if (rows > 0) { Reload(); MessageBox.Show("Deleted."); }
        }
    }
}
