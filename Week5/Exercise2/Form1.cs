using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Exercise2
{
    public partial class Form1 : Form
    {
        private readonly BindingSource _bs = new BindingSource();
        private readonly List<Project> _projects = new List<Project>
        {
            new Project{ ProjectId="P001", ProjectName="Project X", Location="HCM", StartDate=new DateTime(2024,3,1), ProjectManager="Alice" },
            new Project{ ProjectId="P002", ProjectName="Project Y", Location="HN",  StartDate=new DateTime(2024,4,15), ProjectManager="Bob"   }
        };
        public Form1()
        {
            InitializeComponent();

            // ---- Wire events (Designer-safe) ----
            this.Load += Form1_Load;
            btnSave.Click += BtnSave_Click;
            btnUpdate.Click += BtnUpdate_Click;
            btnDelete.Click += BtnDelete_Click;
            btnShowAll.Click += BtnShowAll_Click;

            // Navigator buttons use BindingNavigator's default behavior via BindingSource
            bindingNavigatorAddNewItem.Click += (s, ev) => _bs.AddNew();
            bindingNavigatorDeleteItem.Click += (s, ev) => { if (_bs.Current != null) _bs.RemoveCurrent(); };
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Set data source
            _bs.DataSource = _projects;

            // Complex binding
            dgvProjects.AutoGenerateColumns = true;
            dgvProjects.DataSource = _bs;

            // Simple binding (clear old first to avoid duplicates)
            txtId.DataBindings.Clear();
            txtName.DataBindings.Clear();
            txtLocation.DataBindings.Clear();
            dtpStartDate.DataBindings.Clear();
            txtManager.DataBindings.Clear();

            txtId.DataBindings.Add("Text", _bs, nameof(Project.ProjectId), true, DataSourceUpdateMode.OnPropertyChanged);
            txtName.DataBindings.Add("Text", _bs, nameof(Project.ProjectName), true, DataSourceUpdateMode.OnPropertyChanged);
            txtLocation.DataBindings.Add("Text", _bs, nameof(Project.Location), true, DataSourceUpdateMode.OnPropertyChanged);
            dtpStartDate.DataBindings.Add("Value", _bs, nameof(Project.StartDate), true, DataSourceUpdateMode.OnPropertyChanged);
            txtManager.DataBindings.Add("Text", _bs, nameof(Project.ProjectManager), true, DataSourceUpdateMode.OnPropertyChanged);

            // Bind navigator
            bindingNavigator.BindingSource = _bs;
        }

        // Save = commit current edits (from textboxes) back to the current Project object
        private void BtnSave_Click(object sender, EventArgs e)
        {
            _bs.EndEdit();              // push textbox changes
            dgvProjects.Refresh();      // refresh grid display
            MessageBox.Show("Data saved.", "Save", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Update = same as Save in this in-memory example (no DB). Kept for assignment’s buttons.
        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            _bs.EndEdit();
            dgvProjects.Refresh();
            MessageBox.Show("Updated.", "Update", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Delete current record
        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (_bs.Current != null)
            {
                _bs.RemoveCurrent();
                _bs.EndEdit();
                dgvProjects.Refresh();
            }
        }

        // Show All = rebind the grid to the BindingSource (in case you filtered later)
        private void BtnShowAll_Click(object sender, EventArgs e)
        {
            dgvProjects.DataSource = null;
            dgvProjects.DataSource = _bs;
            dgvProjects.Refresh();
        }
    }
}
