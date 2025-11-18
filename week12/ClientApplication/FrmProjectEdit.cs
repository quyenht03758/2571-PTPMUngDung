using System;
using System.Windows.Forms;
using RemotingObjects.Models;

namespace ClientApplication
{
    /// <summary>
    /// Form for adding or editing project information.
    /// </summary>
    public partial class FrmProjectEdit : Form
    {
        public Project Project { get; private set; }
        private bool _isEditMode;

        /// <summary>
        /// Initializes a new instance for adding a new project.
        /// </summary>
        public FrmProjectEdit()
        {
            InitializeComponent();
            _isEditMode = false;
            this.Text = "Add New Project";
            Project = new Project();
            dtpStartDate.Value = DateTime.Now;
        }

        /// <summary>
        /// Initializes a new instance for editing an existing project.
        /// </summary>
        public FrmProjectEdit(Project project)
        {
            InitializeComponent();
            _isEditMode = true;
            this.Text = "Edit Project";
            Project = project;

            // Load existing data
            txtProjectNumber.Text = project.ProjectNumber;
            txtProjectNumber.ReadOnly = true; // Can't change project number
            txtProjectName.Text = project.ProjectName;
            txtLocation.Text = project.Location;
            dtpStartDate.Value = project.StartDate;
            txtProjectManager.Text = project.ProjectManager;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // Validation
            if (string.IsNullOrWhiteSpace(txtProjectNumber.Text))
            {
                MessageBox.Show("Project Number is required.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtProjectNumber.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtProjectName.Text))
            {
                MessageBox.Show("Project Name is required.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtProjectName.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtLocation.Text))
            {
                MessageBox.Show("Location is required.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtLocation.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtProjectManager.Text))
            {
                MessageBox.Show("Project Manager is required.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtProjectManager.Focus();
                return;
            }

            // Update project object
            Project.ProjectNumber = txtProjectNumber.Text.Trim();
            Project.ProjectName = txtProjectName.Text.Trim();
            Project.Location = txtLocation.Text.Trim();
            Project.StartDate = dtpStartDate.Value;
            Project.ProjectManager = txtProjectManager.Text.Trim();

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
