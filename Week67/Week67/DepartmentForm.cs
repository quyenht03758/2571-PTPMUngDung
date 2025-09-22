using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Week67
{
    public partial class DepartmentForm : Form
    {
        DataTable _table;

        public DepartmentForm()
        {
            InitializeComponent();

            Load += (s, e) => Reload();
            dgv.SelectionChanged += (s, e) => Pull();
            btnReload.Click += (s, e) => Reload();
            btnAdd.Click += (s, e) => Add();
            btnUpdate.Click += (s, e) => UpdateRow();
            btnDelete.Click += (s, e) => DeleteRow();
            btnClose.Click += (s, e) => Close();
        }

        void Reload()
        {
            _table = Db.Query("SELECT Dnumber,Dname,MgrSSN,MgrStartDate FROM Department ORDER BY Dnumber");
            dgv.DataSource = _table;
        }

        void Pull()
        {
            if (dgv.CurrentRow == null) return;
            var r = (dgv.CurrentRow.DataBoundItem as DataRowView)?.Row;
            if (r == null) return;
            txtDnumber.Text = r.Field<int>("Dnumber").ToString();
            txtDname.Text = r.Field<string>("Dname");
            txtMgrSSN.Text = r.Field<string>("MgrSSN");
            dtpMgrStart.Value = r.Field<DateTime?>("MgrStartDate") ?? DateTime.Today;
        }

        bool ValidateInputs(bool forUpdate)
        {
            if (!int.TryParse(txtDnumber.Text.Trim(), out _)) { MessageBox.Show("Dnumber must be integer."); return false; }
            if (string.IsNullOrWhiteSpace(txtDname.Text)) { MessageBox.Show("Dname required."); return false; }
            if (!forUpdate)
            {
                var ex = Db.Query("SELECT 1 FROM Department WHERE Dnumber=@id", new SqlParameter("@id", int.Parse(txtDnumber.Text)));
                if (ex.Rows.Count > 0) { MessageBox.Show("Dnumber already exists."); return false; }
            }
            return true;
        }

        void Add()
        {
            if (!ValidateInputs(false)) return;
            var rows = Db.Execute(@"INSERT INTO Department(Dnumber,Dname,MgrSSN,MgrStartDate)
                                    VALUES(@n,@name,@mgr,@start)",
                        new SqlParameter("@n", int.Parse(txtDnumber.Text)),
                        new SqlParameter("@name", txtDname.Text.Trim()),
                        new SqlParameter("@mgr", string.IsNullOrWhiteSpace(txtMgrSSN.Text) ? (object)DBNull.Value : txtMgrSSN.Text.Trim()),
                        new SqlParameter("@start", dtpMgrStart.Value.Date));
            if (rows > 0) { Reload(); MessageBox.Show("Added!"); }
        }

        void UpdateRow()
        {
            if (!ValidateInputs(true)) return;
            var rows = Db.Execute(@"UPDATE Department SET Dname=@name,MgrSSN=@mgr,MgrStartDate=@start WHERE Dnumber=@n",
                        new SqlParameter("@name", txtDname.Text.Trim()),
                        new SqlParameter("@mgr", string.IsNullOrWhiteSpace(txtMgrSSN.Text) ? (object)DBNull.Value : txtMgrSSN.Text.Trim()),
                        new SqlParameter("@start", dtpMgrStart.Value.Date),
                        new SqlParameter("@n", int.Parse(txtDnumber.Text)));
            if (rows > 0) { Reload(); MessageBox.Show("Updated!"); }
        }

        void DeleteRow()
        {
            if (dgv.CurrentRow == null) return;
            var id = txtDnumber.Text.Trim();
            if (MessageBox.Show($"Delete department {id}?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.Yes) return;
            var rows = Db.Execute("DELETE FROM Department WHERE Dnumber=@n", new SqlParameter("@n", int.Parse(id)));
            if (rows > 0) { Reload(); MessageBox.Show("Deleted."); }
        }
    }
}
