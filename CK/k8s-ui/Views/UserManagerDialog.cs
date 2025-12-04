using K8sManager.Services;
using System.ComponentModel;

namespace K8sManager.Views
{
    [DesignerCategory("Form")]
    public partial class UserManagerDialog : Form
    {
        private readonly ApiClient _apiClient;
        private ContextMenuStrip? _contextMenu;

        public UserManagerDialog(ApiClient apiClient)
        {
            _apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
            InitializeComponent();
            InitializeContextMenu();
            LoadUsersAsync();
        }

        private void InitializeContextMenu()
        {
            _contextMenu = new ContextMenuStrip();
            
            var editItem = new ToolStripMenuItem("✏️ Edit");
            editItem.Click += (s, e) => EditUser();
            
            var resetPasswordItem = new ToolStripMenuItem("🔑 Reset Password");
            resetPasswordItem.Click += (s, e) => ResetPassword();
            
            var lockItem = new ToolStripMenuItem("🔒 Lock User");
            lockItem.Click += (s, e) => LockUser();
            
            var unlockItem = new ToolStripMenuItem("🔓 Unlock User");
            unlockItem.Click += (s, e) => UnlockUser();
            
            var deleteItem = new ToolStripMenuItem("🗑️ Delete");
            deleteItem.Click += (s, e) => DeleteUser();
            
            _contextMenu.Items.AddRange(new ToolStripItem[] 
            { 
                editItem, 
                resetPasswordItem,
                new ToolStripSeparator(),
                lockItem, 
                unlockItem,
                new ToolStripSeparator(),
                deleteItem 
            });
        }

        private async void LoadUsersAsync()
        {
            try
            {
                var users = await _apiClient.GetUsersAsync<List<UserDto>>().ConfigureAwait(false);

                if (InvokeRequired)
                {
                    Invoke(() => BindUsers(users));
                }
                else
                {
                    BindUsers(users);
                }
            }
            catch (Exception ex)
            {
                if (InvokeRequired)
                {
                    Invoke(() => MessageBox.Show($"Error loading users: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error));
                }
                else
                {
                    MessageBox.Show($"Error loading users: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BindUsers(List<UserDto>? users)
        {
            if (users == null) return;

            var userData = users.Select(u => new
            {
                Id = u.Id,
                Username = u.Username,
                Email = u.Email ?? "",
                Role = u.Role,
                IsLocked = u.IsLocked ? "🔒 Locked" : "🔓 Active",
                CreatedAt = u.CreatedAt.ToString("yyyy-MM-dd HH:mm")
            }).ToList();

            _gridUsers.DataSource = userData;

            if (_gridUsers.Columns.Count > 0)
            {
                _gridUsers.Columns["Id"].Width = 60;
                _gridUsers.Columns["Username"].Width = 200;
                _gridUsers.Columns["Email"].Width = 250;
                _gridUsers.Columns["Role"].Width = 120;
                _gridUsers.Columns["IsLocked"].Width = 120;
                _gridUsers.Columns["CreatedAt"].Width = 150;
            }
        }

        private void GridUsers_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && e.RowIndex >= 0)
            {
                _gridUsers.ClearSelection();
                _gridUsers.Rows[e.RowIndex].Selected = true;
                _contextMenu?.Show(_gridUsers, _gridUsers.PointToClient(Cursor.Position));
            }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            var username = Microsoft.VisualBasic.Interaction.InputBox("Enter username:", "Create User", "");
            if (string.IsNullOrWhiteSpace(username)) return;

            var password = Microsoft.VisualBasic.Interaction.InputBox("Enter password:", "Create User", "");
            if (string.IsNullOrWhiteSpace(password)) return;

            var email = Microsoft.VisualBasic.Interaction.InputBox("Enter email (optional):", "Create User", "");
            var fullName = Microsoft.VisualBasic.Interaction.InputBox("Enter full name (optional):", "Create User", "");
            var role = Microsoft.VisualBasic.Interaction.InputBox("Enter role (Admin/Operator/Viewer):", "Create User", "Viewer");

            CreateUserAsync(username, password, email, fullName, role);
        }

        private async void CreateUserAsync(string username, string password, string email, string fullName, string role)
        {
            try
            {
                var request = new
                {
                    Username = username,
                    Password = password,
                    Email = email,
                    FullName = fullName,
                    Role = role
                };

                await _apiClient.CreateUserAsync<object, object>(request).ConfigureAwait(false);

                if (InvokeRequired)
                {
                    Invoke(() =>
                    {
                        MessageBox.Show("User created successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadUsersAsync();
                    });
                }
                else
                {
                    MessageBox.Show("User created successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadUsersAsync();
                }
            }
            catch (Exception ex)
            {
                if (InvokeRequired)
                {
                    Invoke(() => MessageBox.Show($"Error creating user: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error));
                }
                else
                {
                    MessageBox.Show($"Error creating user: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void EditUser()
        {
            if (_gridUsers.SelectedRows.Count == 0) return;

            var userId = (int)_gridUsers.SelectedRows[0].Cells["Id"].Value;
            var username = _gridUsers.SelectedRows[0].Cells["Username"].Value.ToString();
            var email = _gridUsers.SelectedRows[0].Cells["Email"].Value.ToString();
            var role = _gridUsers.SelectedRows[0].Cells["Role"].Value.ToString();

            var newEmail = Microsoft.VisualBasic.Interaction.InputBox($"Edit email for '{username}':", "Edit User", email ?? "");
            var newRole = Microsoft.VisualBasic.Interaction.InputBox($"Edit role for '{username}' (Admin/Operator/Viewer):", "Edit User", role ?? "Viewer");

            // Use current values if user left InputBox empty or cancelled
            var finalEmail = string.IsNullOrWhiteSpace(newEmail) ? email : newEmail;
            var finalRole = string.IsNullOrWhiteSpace(newRole) ? role : newRole;

            UpdateUserAsync(userId, finalEmail ?? "", finalRole ?? "Viewer");
        }

        private async void UpdateUserAsync(int userId, string email, string role)
        {
            try
            {
                var request = new { Email = email, Role = role };
                var success = await _apiClient.UpdateUserAsync(userId, request).ConfigureAwait(false);

                if (InvokeRequired)
                {
                    Invoke(() =>
                    {
                        if (success)
                        {
                            MessageBox.Show("User updated successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadUsersAsync();
                        }
                        else
                        {
                            MessageBox.Show("Failed to update user", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                if (InvokeRequired)
                {
                    Invoke(() => MessageBox.Show($"Error updating user: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error));
                }
            }
        }

        private void ResetPassword()
        {
            if (_gridUsers.SelectedRows.Count == 0) return;

            var userId = (int)_gridUsers.SelectedRows[0].Cells["Id"].Value;
            var username = _gridUsers.SelectedRows[0].Cells["Username"].Value.ToString();

            var newPassword = Microsoft.VisualBasic.Interaction.InputBox(
                $"Enter new password for user '{username}':",
                "Reset Password",
                "");

            if (!string.IsNullOrEmpty(newPassword))
            {
                ResetPasswordAsync(userId, newPassword);
            }
        }

        private async void ResetPasswordAsync(int userId, string newPassword)
        {
            try
            {
                var request = new { NewPassword = newPassword };
                var success = await _apiClient.ResetUserPasswordAsync(userId, request).ConfigureAwait(false);

                if (InvokeRequired)
                {
                    Invoke(() =>
                    {
                        if (success)
                        {
                            MessageBox.Show("Password reset successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Failed to reset password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                if (InvokeRequired)
                {
                    Invoke(() => MessageBox.Show($"Error resetting password: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error));
                }
            }
        }

        private async void LockUser()
        {
            if (_gridUsers.SelectedRows.Count == 0) return;

            var userId = (int)_gridUsers.SelectedRows[0].Cells["Id"].Value;

            try
            {
                var success = await _apiClient.LockUserAsync(userId).ConfigureAwait(false);

                if (InvokeRequired)
                {
                    Invoke(() =>
                    {
                        if (success)
                        {
                            MessageBox.Show("User locked successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadUsersAsync();
                        }
                        else
                        {
                            MessageBox.Show("Failed to lock user", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                if (InvokeRequired)
                {
                    Invoke(() => MessageBox.Show($"Error locking user: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error));
                }
            }
        }

        private async void UnlockUser()
        {
            if (_gridUsers.SelectedRows.Count == 0) return;

            var userId = (int)_gridUsers.SelectedRows[0].Cells["Id"].Value;

            try
            {
                var success = await _apiClient.UnlockUserAsync(userId).ConfigureAwait(false);

                if (InvokeRequired)
                {
                    Invoke(() =>
                    {
                        if (success)
                        {
                            MessageBox.Show("User unlocked successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadUsersAsync();
                        }
                        else
                        {
                            MessageBox.Show("Failed to unlock user", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                if (InvokeRequired)
                {
                    Invoke(() => MessageBox.Show($"Error unlocking user: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error));
                }
            }
        }

        private void DeleteUser()
        {
            if (_gridUsers.SelectedRows.Count == 0) return;

            var userId = (int)_gridUsers.SelectedRows[0].Cells["Id"].Value;
            var username = _gridUsers.SelectedRows[0].Cells["Username"].Value.ToString();

            var result = MessageBox.Show(
                $"Are you sure you want to delete user '{username}'?\nThis action cannot be undone.",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                DeleteUserAsync(userId);
            }
        }

        private async void DeleteUserAsync(int userId)
        {
            try
            {
                var success = await _apiClient.DeleteUserAsync(userId).ConfigureAwait(false);

                if (InvokeRequired)
                {
                    Invoke(() =>
                    {
                        if (success)
                        {
                            MessageBox.Show("User deleted successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadUsersAsync();
                        }
                        else
                        {
                            MessageBox.Show("Failed to delete user", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                if (InvokeRequired)
                {
                    Invoke(() => MessageBox.Show($"Error deleting user: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error));
                }
            }
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            LoadUsersAsync();
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }

    public class UserDto
    {
        public int Id { get; set; }
        public string Username { get; set; } = "";
        public string? Email { get; set; }
        public string? FullName { get; set; }
        
        // Role can be string ("Admin", "Operator", "Viewer") or int (0, 1, 2)
        private object? _role;
        
        [System.Text.Json.Serialization.JsonPropertyName("role")]
        public object? RoleRaw 
        { 
            get => _role; 
            set => _role = value; 
        }
        
        [System.Text.Json.Serialization.JsonIgnore]
        public string Role 
        { 
            get 
            {
                if (_role == null) return "Viewer";
                
                // If it's already a string, return it
                if (_role is string str) return str;
                
                // If it's a JsonElement, parse it
                if (_role is System.Text.Json.JsonElement element)
                {
                    if (element.ValueKind == System.Text.Json.JsonValueKind.String)
                        return element.GetString() ?? "Viewer";
                    
                    if (element.ValueKind == System.Text.Json.JsonValueKind.Number)
                    {
                        var num = element.GetInt32();
                        return num switch
                        {
                            0 => "Viewer",
                            1 => "Operator",
                            2 => "Admin",
                            _ => "Viewer"
                        };
                    }
                }
                
                // If it's an int, convert
                if (_role is int intRole)
                {
                    return intRole switch
                    {
                        0 => "Viewer",
                        1 => "Operator",
                        2 => "Admin",
                        _ => "Viewer"
                    };
                }
                
                return _role.ToString() ?? "Viewer";
            }
            set => _role = value;
        }
        
        public bool IsLocked { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
