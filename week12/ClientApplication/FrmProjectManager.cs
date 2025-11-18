using System;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Http;
using System.Windows.Forms;
using RemotingObjects;
using RemotingObjects.Models;

namespace ClientApplication
{
    /// <summary>
    /// Main form for the Project Management client application.
    /// Connects to remote ProjectManager service via .NET Remoting.
    /// </summary>
    public partial class FrmProjectManager : Form
    {
        private IProjectManager _projectManager;
        private Project[] _currentProjects;

        public FrmProjectManager()
        {
            InitializeComponent();
            InitializeRemotingConnection();
            // Don't load projects in constructor - let user click the button
            // This avoids connection timing issues
        }

        /// <summary>
        /// Initializes the .NET Remoting connection to the host server.
        /// </summary>
        private void InitializeRemotingConnection()
        {
            try
            {
                // Register HTTP channel for client communication
                HttpChannel channel = new HttpChannel();
                ChannelServices.RegisterChannel(channel, false);

                // Get reference to remote object
                _projectManager = (IProjectManager)Activator.GetObject(
                    typeof(IProjectManager),
                    "http://localhost:8080/ProjectManager"
                );

                lblStatus.Text = "Status: Connected to server";
                lblStatus.ForeColor = Color.Green;
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Status: Connection failed";
                lblStatus.ForeColor = Color.Red;
                MessageBox.Show(
                    $"Failed to connect to remoting server.\n\nError: {ex.Message}\n\nPlease ensure the Host application is running.",
                    "Connection Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        /// <summary>
        /// Loads all projects from the remote server.
        /// </summary>
        private void LoadAllProjects()
        {
            try
            {
                if (_projectManager == null)
                {
                    MessageBox.Show(
                        "Not connected to server.\n\nPlease ensure the Host application is running and restart the client.",
                        "Connection Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                _currentProjects = _projectManager.GetAllProjects();
                DisplayProjects(_currentProjects);
                txtLocation.Clear();
                UpdateProjectCount();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Failed to retrieve projects.\n\nError: {ex.Message}\n\nPlease ensure the Host application is running.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        /// <summary>
        /// Loads projects filtered by location.
        /// </summary>
        private void LoadProjectsByLocation()
        {
            try
            {
                if (_projectManager == null)
                {
                    MessageBox.Show("Not connected to server.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string location = txtLocation.Text.Trim();
                if (string.IsNullOrWhiteSpace(location))
                {
                    MessageBox.Show("Please enter a location to filter.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                _currentProjects = _projectManager.GetAllProjectsByLocation(location);
                DisplayProjects(_currentProjects);
                UpdateProjectCount();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Failed to retrieve projects by location.\n\nError: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        /// <summary>
        /// Displays the project list in the DataGridView.
        /// </summary>
        private void DisplayProjects(Project[] projects)
        {
            dgvProjects.DataSource = null;
            dgvProjects.DataSource = projects;

            // Customize column headers
            if (dgvProjects.Columns.Count > 0)
            {
                dgvProjects.Columns["ProjectNumber"].HeaderText = "Project Number";
                dgvProjects.Columns["ProjectNumber"].Width = 120;

                dgvProjects.Columns["ProjectName"].HeaderText = "Project Name";
                dgvProjects.Columns["ProjectName"].Width = 250;

                dgvProjects.Columns["Location"].HeaderText = "Location";
                dgvProjects.Columns["Location"].Width = 120;

                dgvProjects.Columns["StartDate"].HeaderText = "Start Date";
                dgvProjects.Columns["StartDate"].Width = 120;
                dgvProjects.Columns["StartDate"].DefaultCellStyle.Format = "dd/MM/yyyy";

                dgvProjects.Columns["ProjectManager"].HeaderText = "Project Manager";
                dgvProjects.Columns["ProjectManager"].Width = 180;
            }
        }

        /// <summary>
        /// Updates the project count label.
        /// </summary>
        private void UpdateProjectCount()
        {
            int count = _currentProjects != null ? _currentProjects.Length : 0;
            lblProjectCount.Text = $"Total Projects: {count}";
        }

        // Event handlers
        private void btnLoadAll_Click(object sender, EventArgs e)
        {
            LoadAllProjects();
        }

        private void btnFilterByLocation_Click(object sender, EventArgs e)
        {
            LoadProjectsByLocation();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadAllProjects();
        }

        private void txtLocation_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                LoadProjectsByLocation();
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                var editForm = new FrmProjectEdit();
                if (editForm.ShowDialog() == DialogResult.OK)
                {
                    bool success = _projectManager.AddProject(editForm.Project);
                    if (success)
                    {
                        MessageBox.Show("Project added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadAllProjects();
                    }
                    else
                    {
                        MessageBox.Show("Failed to add project. Project number may already exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding project: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvProjects.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Please select a project to edit.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                var selectedProject = dgvProjects.SelectedRows[0].DataBoundItem as Project;
                if (selectedProject == null) return;

                // Get fresh copy from server
                var project = _projectManager.GetProjectByNumber(selectedProject.ProjectNumber);
                if (project == null)
                {
                    MessageBox.Show("Project not found on server.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var editForm = new FrmProjectEdit(project);
                if (editForm.ShowDialog() == DialogResult.OK)
                {
                    bool success = _projectManager.UpdateProject(editForm.Project);
                    if (success)
                    {
                        MessageBox.Show("Project updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadAllProjects();
                    }
                    else
                    {
                        MessageBox.Show("Failed to update project.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating project: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvProjects.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Please select a project to delete.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                var selectedProject = dgvProjects.SelectedRows[0].DataBoundItem as Project;
                if (selectedProject == null) return;

                var result = MessageBox.Show(
                    $"Are you sure you want to delete project '{selectedProject.ProjectNumber}'?\n\n{selectedProject.ProjectName}",
                    "Confirm Delete",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    bool success = _projectManager.DeleteProject(selectedProject.ProjectNumber);
                    if (success)
                    {
                        MessageBox.Show("Project deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadAllProjects();
                    }
                    else
                    {
                        MessageBox.Show("Failed to delete project.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting project: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
