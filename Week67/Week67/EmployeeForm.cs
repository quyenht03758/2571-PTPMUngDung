using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Windows.Forms;

namespace Week67
{
    public partial class EmployeeForm : Form
    {
        DataTable _table, _depts;

        public EmployeeForm()
        {
            InitializeComponent();

            // 🔹 ToolTip setup
            var tips = new ToolTip
            {
                InitialDelay = 0,
                ReshowDelay = 0,
                AutoPopDelay = 10000,
                ShowAlways = true
            };

            tips.SetToolTip(txtSsn, "Social Security Number (9 digits, unique ID)");
            tips.SetToolTip(txtFname, "First name of employee");
            tips.SetToolTip(txtMinit, "Middle initial (one character, optional)");
            tips.SetToolTip(txtLname, "Last name (family name)");
            tips.SetToolTip(dtpBdate, "Date of birth");
            tips.SetToolTip(txtAddress, "Home address");
            tips.SetToolTip(txtSex, "Gender: M = Male, F = Female");
            tips.SetToolTip(txtSalary, "Monthly salary (numeric value)");
            tips.SetToolTip(txtSuper, "Supervisor's SSN (optional)");
            tips.SetToolTip(cboDno, "Select the employee's department");

            // 🔹 Events
            Load += (s, e) => { LoadDepartments(); Reload(); };
            dgv.SelectionChanged += (s, e) => PullRowToInputs();

            btnReload.Click += (s, e) => Reload();
            btnAdd.Click += (s, e) => AddEmployee();
            btnUpdate.Click += (s, e) => UpdateEmployee();
            btnDelete.Click += (s, e) => DeleteEmployee();
            btnClose.Click += (s, e) => Close();
        }

        private void LoadDepartments()
        {
            _depts = Db.Query("SELECT Dnumber, Dname FROM Department ORDER BY Dnumber");
            cboDno.DataSource = _depts;
            cboDno.DisplayMember = "Dname";
            cboDno.ValueMember = "Dnumber";
        }

        private void Reload()
        {
            _table = Db.Query(@"
                SELECT  e.Ssn,
                        e.Fname,
                        e.Minit,
                        e.Lname,
                        (e.Fname + ' ' + ISNULL(NULLIF(e.Minit,'' ) + '. ', '') + e.Lname) AS FullName,
                        e.Bdate,
                        e.Address,
                        CASE WHEN UPPER(e.Sex)='M' THEN 'Male'
                             WHEN UPPER(e.Sex)='F' THEN 'Female'
                             ELSE '—' END AS SexText,
                        e.Salary,
                        e.Super_ssn,
                        e.Dno,
                        d.Dname AS Department
                FROM Employee e
                LEFT JOIN Department d ON d.Dnumber = e.Dno
                ORDER BY e.Ssn");
            dgv.DataSource = _table;
            FormatEmployeeGrid();
        }

        private void FormatEmployeeGrid()
        {
            var g = dgv;
            g.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            g.Columns["Ssn"].HeaderText = "SSN";
            g.Columns["FullName"].HeaderText = "Full Name";
            g.Columns["Bdate"].HeaderText = "Birth Date";
            g.Columns["Address"].HeaderText = "Address";
            g.Columns["SexText"].HeaderText = "Sex";
            g.Columns["Salary"].HeaderText = "Salary";
            g.Columns["Super_ssn"].HeaderText = "Supervisor SSN";
            g.Columns["Department"].HeaderText = "Department";

            g.Columns["Fname"].Visible = false;
            g.Columns["Minit"].Visible = false;
            g.Columns["Lname"].Visible = false;
            g.Columns["Dno"].Visible = false;

            g.Columns["Bdate"].DefaultCellStyle.Format = "yyyy-MM-dd";
            g.Columns["Salary"].DefaultCellStyle.Format = "N0";

            g.Columns["FullName"].DisplayIndex = 0;
            g.Columns["Ssn"].DisplayIndex = 1;
            g.Columns["Department"].DisplayIndex = 2;
            g.Columns["SexText"].DisplayIndex = 3;
            g.Columns["Salary"].DisplayIndex = 4;
            g.Columns["Bdate"].DisplayIndex = 5;
            g.Columns["Address"].DisplayIndex = 6;
        }

        private void PullRowToInputs()
        {
            if (dgv.CurrentRow == null) return;
            var r = (dgv.CurrentRow.DataBoundItem as DataRowView)?.Row; if (r == null) return;

            txtSsn.Text = r.Field<string>("Ssn");
            txtFname.Text = r.Field<string>("Fname");
            txtMinit.Text = r.Field<string>("Minit");
            txtLname.Text = r.Field<string>("Lname");
            dtpBdate.Value = r.Field<DateTime?>("Bdate") ?? DateTime.Today;
            txtAddress.Text = r.Field<string>("Address");
            txtSex.Text = r.Field<string>("SexText") == "Male" ? "M" :
                              r.Field<string>("SexText") == "Female" ? "F" : "";
            txtSalary.Text = r.Field<decimal?>("Salary")?.ToString(CultureInfo.InvariantCulture) ?? "";
            txtSuper.Text = r.Field<string>("Super_ssn");
            var dno = r.Field<int?>("Dno");
            if (dno.HasValue) cboDno.SelectedValue = dno.Value;
        }

        private bool ValidateInputs(bool forUpdate = false)
        {
            if (string.IsNullOrWhiteSpace(txtSsn.Text) || txtSsn.Text.Trim().Length != 9)
            { MessageBox.Show("SSN must be 9 characters."); return false; }

            if (string.IsNullOrWhiteSpace(txtFname.Text) || string.IsNullOrWhiteSpace(txtLname.Text))
            { MessageBox.Show("First and Last name are required."); return false; }

            if (!string.IsNullOrWhiteSpace(txtSex.Text) &&
                !(txtSex.Text.Equals("M", StringComparison.OrdinalIgnoreCase) ||
                  txtSex.Text.Equals("F", StringComparison.OrdinalIgnoreCase)))
            { MessageBox.Show("Sex must be M or F."); return false; }

            if (!string.IsNullOrWhiteSpace(txtSalary.Text) &&
                !decimal.TryParse(txtSalary.Text, NumberStyles.Number, CultureInfo.InvariantCulture, out _))
            { MessageBox.Show("Salary must be numeric."); return false; }

            if (cboDno.SelectedValue == null)
            { MessageBox.Show("Please select a Department."); return false; }

            if (!forUpdate)
            {
                var exists = Db.Query("SELECT 1 FROM Employee WHERE Ssn=@id",
                                      new SqlParameter("@id", txtSsn.Text.Trim()));
                if (exists.Rows.Count > 0)
                { MessageBox.Show("SSN already exists."); return false; }
            }
            return true;
        }

        private void AddEmployee()
        {
            if (!ValidateInputs(false)) return;

            var sql = @"INSERT INTO Employee(Fname,Minit,Lname,Ssn,Bdate,Address,Sex,Salary,Super_ssn,Dno)
                        VALUES(@Fname,@Minit,@Lname,@Ssn,@Bdate,@Address,@Sex,@Salary,@Super,@Dno)";
            var rows = Db.Execute(sql,
                new SqlParameter("@Fname", txtFname.Text.Trim()),
                new SqlParameter("@Minit", (object)txtMinit.Text ?? DBNull.Value),
                new SqlParameter("@Lname", txtLname.Text.Trim()),
                new SqlParameter("@Ssn", txtSsn.Text.Trim()),
                new SqlParameter("@Bdate", dtpBdate.Value.Date),
                new SqlParameter("@Address", string.IsNullOrWhiteSpace(txtAddress.Text) ? (object)DBNull.Value : txtAddress.Text.Trim()),
                new SqlParameter("@Sex", string.IsNullOrWhiteSpace(txtSex.Text) ? (object)DBNull.Value : txtSex.Text.Trim().ToUpperInvariant()),
                new SqlParameter("@Salary", string.IsNullOrWhiteSpace(txtSalary.Text) ? (object)DBNull.Value : decimal.Parse(txtSalary.Text.Trim(), CultureInfo.InvariantCulture)),
                new SqlParameter("@Super", string.IsNullOrWhiteSpace(txtSuper.Text) ? (object)DBNull.Value : txtSuper.Text.Trim()),
                new SqlParameter("@Dno", (int)cboDno.SelectedValue)
            );
            if (rows > 0) { Reload(); MessageBox.Show("Added!"); }
        }

        private void UpdateEmployee()
        {
            if (dgv.CurrentRow == null) { MessageBox.Show("Select a row."); return; }
            if (!ValidateInputs(true)) return;

            var sql = @"UPDATE Employee
                        SET Fname=@Fname, Minit=@Minit, Lname=@Lname, Bdate=@Bdate,
                            Address=@Address, Sex=@Sex, Salary=@Salary, Super_ssn=@Super, Dno=@Dno
                        WHERE Ssn=@Ssn";
            var rows = Db.Execute(sql,
                new SqlParameter("@Fname", txtFname.Text.Trim()),
                new SqlParameter("@Minit", (object)txtMinit.Text ?? DBNull.Value),
                new SqlParameter("@Lname", txtLname.Text.Trim()),
                new SqlParameter("@Bdate", dtpBdate.Value.Date),
                new SqlParameter("@Address", string.IsNullOrWhiteSpace(txtAddress.Text) ? (object)DBNull.Value : txtAddress.Text.Trim()),
                new SqlParameter("@Sex", string.IsNullOrWhiteSpace(txtSex.Text) ? (object)DBNull.Value : txtSex.Text.Trim().ToUpperInvariant()),
                new SqlParameter("@Salary", string.IsNullOrWhiteSpace(txtSalary.Text) ? (object)DBNull.Value : decimal.Parse(txtSalary.Text.Trim(), CultureInfo.InvariantCulture)),
                new SqlParameter("@Super", string.IsNullOrWhiteSpace(txtSuper.Text) ? (object)DBNull.Value : txtSuper.Text.Trim()),
                new SqlParameter("@Dno", (int)cboDno.SelectedValue),
                new SqlParameter("@Ssn", txtSsn.Text.Trim())
            );
            if (rows > 0) { Reload(); MessageBox.Show("Updated!"); }
        }

        private void DeleteEmployee()
        {
            if (dgv.CurrentRow == null) return;
            var id = txtSsn.Text.Trim();
            if (MessageBox.Show("Delete employee " + id + "?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.Yes) return;

            var rows = Db.Execute("DELETE FROM Employee WHERE Ssn=@id", new SqlParameter("@id", id));
            if (rows > 0) { Reload(); MessageBox.Show("Deleted."); }
        }
    }
}
