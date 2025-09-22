using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using Week67;

namespace Week67
{
    public partial class ProjectForm : Form
    {
        DataTable _table, _dept;
        private ToolTip tips;

        public ProjectForm()
        {
            InitializeComponent();

            tips = new ToolTip
            {
                InitialDelay = 0,
                ReshowDelay = 0,
                AutoPopDelay = 10000,
                ShowAlways = true
            };
            
            tips.SetToolTip(txtPname, "Name of the project");
            tips.SetToolTip(txtLocation, "Location of the project");
            tips.SetToolTip(cboDept, "Department responsible for this project");
            tips.SetToolTip(dgv, "List of all projects");
            tips.SetToolTip(btnAdd, "Add new project");
            tips.SetToolTip(btnUpdate, "Update selected project");
            tips.SetToolTip(btnDelete, "Delete selected project");
            tips.SetToolTip(btnReload, "Reload list from database");
            tips.SetToolTip(btnClose, "Close this window");

            Load += (s, e) => { LoadDepts(); Reload(); };
            dgv.SelectionChanged += (s, e) => Pull();
            btnReload.Click += (s, e) => Reload();
            btnAdd.Click += (s, e) => Add();
            btnUpdate.Click += (s, e) => UpdateRow();
            btnDelete.Click += (s, e) => DeleteRow();
            btnClose.Click += (s, e) => Close();
        }

        void LoadDepts()
        {
            _dept = Db.Query("SELECT Dnumber, Dname FROM Department ORDER BY Dnumber");
            cboDept.DataSource = _dept;
            cboDept.DisplayMember = "Dname";
            cboDept.ValueMember = "Dnumber";
        }

        void Reload()
        {
            _table = Db.Query(@"
                SELECT p.Pnumber,
                    p.Pname,
                    p.Plocation,
                    p.Dnum,
                    d.Dname AS Department
                FROM Project p
                LEFT JOIN Department d ON d.Dnumber = p.Dnum
                ORDER BY p.Pnumber");
            dgv.DataSource = _table;

            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.Columns["Pnumber"].HeaderText = "Project No";
            dgv.Columns["Pname"].HeaderText = "Project Name";
            dgv.Columns["Plocation"].HeaderText = "Location";
            dgv.Columns["Department"].HeaderText = "Department";
            dgv.Columns["Dnum"].Visible = false; 
        }

        void Pull()
        {
            if (dgv.CurrentRow == null) return;
            var r = (dgv.CurrentRow.DataBoundItem as DataRowView)?.Row;
            if (r == null) return;
            txtPnumber.Text = r.Field<int>("Pnumber").ToString();
            txtPname.Text = r.Field<string>("Pname");
            txtLocation.Text = r.Field<string>("Plocation");
            var dno = r.Field<int?>("Dnum");
            if (dno.HasValue) cboDept.SelectedValue = dno.Value;
        }

        bool ValidateInputs(bool forUpdate)
        {
            if (!int.TryParse(txtPnumber.Text.Trim(), out _)) { MessageBox.Show("Pnumber must be integer."); return false; }
            if (string.IsNullOrWhiteSpace(txtPname.Text)) { MessageBox.Show("Pname required."); return false; }
            if (cboDept.SelectedValue == null) { MessageBox.Show("Select Department."); return false; }

            if (!forUpdate)
            {
                var ex = Db.Query("SELECT 1 FROM Project WHERE Pnumber=@id", new SqlParameter("@id", int.Parse(txtPnumber.Text)));
                if (ex.Rows.Count > 0) { MessageBox.Show("Pnumber already exists."); return false; }
            }
            return true;
        }

        void Add()
        {
            if (!ValidateInputs(false)) return;
            var rows = Db.Execute(@"INSERT INTO Project(Pnumber,Pname,Plocation,Dnum)
                                    VALUES(@n,@name,@loc,@d)",
                        new SqlParameter("@n", int.Parse(txtPnumber.Text)),
                        new SqlParameter("@name", txtPname.Text.Trim()),
                        new SqlParameter("@loc", string.IsNullOrWhiteSpace(txtLocation.Text) ? (object)DBNull.Value : txtLocation.Text.Trim()),
                        new SqlParameter("@d", (int)cboDept.SelectedValue));
            if (rows > 0) { Reload(); MessageBox.Show("Added!"); }
        }

        void UpdateRow()
        {
            if (!ValidateInputs(true)) return;
            var rows = Db.Execute(@"UPDATE Project
                                    SET Pname=@name,Plocation=@loc,Dnum=@d
                                    WHERE Pnumber=@n",
                        new SqlParameter("@name", txtPname.Text.Trim()),
                        new SqlParameter("@loc", string.IsNullOrWhiteSpace(txtLocation.Text) ? (object)DBNull.Value : txtLocation.Text.Trim()),
                        new SqlParameter("@d", (int)cboDept.SelectedValue),
                        new SqlParameter("@n", int.Parse(txtPnumber.Text)));
            if (rows > 0) { Reload(); MessageBox.Show("Updated!"); }
        }

        void DeleteRow()
        {
            if (dgv.CurrentRow == null) return;
            var id = txtPnumber.Text.Trim();
            if (MessageBox.Show($"Delete project {id}?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.Yes) return;
            var rows = Db.Execute("DELETE FROM Project WHERE Pnumber=@n", new SqlParameter("@n", int.Parse(id)));
            if (rows > 0) { Reload(); MessageBox.Show("Deleted."); }
        }
    }
}
