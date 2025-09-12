using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace EmployeeManager
{
    public partial class Form1 : Form
    {
        private readonly BindingList<Employee> _employees = new BindingList<Employee>();
        private readonly ErrorProvider _err = new ErrorProvider();
        public Form1()
        {
            InitializeComponent();

            // Setup DataGridView columns
            dgvEmployees.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Id", HeaderText = "ID" });
            dgvEmployees.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Name", HeaderText = "Name" });
            dgvEmployees.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Address", HeaderText = "Address" });
            dgvEmployees.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Birthday", HeaderText = "Birthday" });
            dgvEmployees.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Email", HeaderText = "Email" });
            dgvEmployees.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Phone", HeaderText = "Phone" });

            dgvEmployees.DataSource = _employees;

            // Wire up buttons
            btnAdd.Click += btnAdd_Click;
            btnUpdate.Click += btnUpdate_Click;
            btnDelete.Click += btnDelete_Click;
            btnExit.Click += (s, e) => Close();

            dgvEmployees.SelectionChanged += dgvEmployees_SelectionChanged;

            _err.BlinkStyle = ErrorBlinkStyle.NeverBlink;
            _err.ContainerControl = this;
        }

        private bool ValidateInputs(bool forUpdate = false)
        {
            bool ok = true;

            _err.SetError(txtId, "");
            _err.SetError(txtName, "");
            _err.SetError(txtPhone, "");
            _err.SetError(txtEmail, "");

            if (string.IsNullOrWhiteSpace(txtId.Text))
            {
                _err.SetError(txtId, "Employee ID cannot be empty");
                ok = false;
            }

            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                _err.SetError(txtName, "Name cannot be empty");
                ok = false;
            }

            if (string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                _err.SetError(txtPhone, "Phone cannot be empty");
                ok = false;
            }

            if (!string.IsNullOrWhiteSpace(txtEmail.Text) && !txtEmail.Text.Contains("@"))
            {
                _err.SetError(txtEmail, "Email must contain @");
                ok = false;
            }

            if (!forUpdate)
            {
                if (_employees.Any(e => e.Id.Equals(txtId.Text.Trim(), StringComparison.OrdinalIgnoreCase)))
                {
                    _err.SetError(txtId, "Employee ID already exists");
                    ok = false;
                }
            }

            return ok;
        }

        private void ClearInputs()
        {
            txtId.Clear();
            txtName.Clear();
            txtAddress.Clear();
            txtEmail.Clear();
            txtPhone.Clear();
            dtpBirthday.Value = DateTime.Today;
            txtId.Focus();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs(false)) return;

            var emp = new Employee
            {
                Id = txtId.Text.Trim(),
                Name = txtName.Text.Trim(),
                Address = txtAddress.Text.Trim(),
                Birthday = dtpBirthday.Value.Date,
                Email = txtEmail.Text.Trim(),
                Phone = txtPhone.Text.Trim()
            };

            _employees.Add(emp);
            ClearInputs();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dgvEmployees.CurrentRow == null) return;
            if (!ValidateInputs(true)) return;

            var current = dgvEmployees.CurrentRow.DataBoundItem as Employee;
            if (current == null) return;

            string newId = txtId.Text.Trim();

            // Check duplicate ID
            if (!current.Id.Equals(newId, StringComparison.OrdinalIgnoreCase) &&
                _employees.Any(emp => emp.Id.Equals(newId, StringComparison.OrdinalIgnoreCase)))
            {
                _err.SetError(txtId, "Employee ID already exists");
                return;
            }

            // Update thông tin
            current.Id = newId;
            current.Name = txtName.Text.Trim();
            current.Address = txtAddress.Text.Trim();
            current.Birthday = dtpBirthday.Value.Date;
            current.Email = txtEmail.Text.Trim();
            current.Phone = txtPhone.Text.Trim();

            dgvEmployees.Refresh();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvEmployees.CurrentRow == null) return;
            var emp = dgvEmployees.CurrentRow.DataBoundItem as Employee;
            if (emp != null) _employees.Remove(emp);
            ClearInputs();
        }

        private void dgvEmployees_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvEmployees.CurrentRow?.DataBoundItem is Employee emp)
            {
                txtId.Text = emp.Id;
                txtName.Text = emp.Name;
                txtAddress.Text = emp.Address;
                dtpBirthday.Value = emp.Birthday;
                txtEmail.Text = emp.Email;
                txtPhone.Text = emp.Phone;
            }
        }
    }
}
