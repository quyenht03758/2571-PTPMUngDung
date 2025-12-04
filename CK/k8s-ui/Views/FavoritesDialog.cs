using System.Data;
using System.ComponentModel;
using K8sManager.Models.Entities;
using K8sManager.Services;
using MaterialSkin;
using MaterialSkin.Controls;

namespace K8sManager.Views
{
    public partial class FavoritesDialog : MaterialForm
    {
        private readonly ApiFavoriteService _favoriteService;

        public string? SelectedResource { get; private set; }
        public string? SelectedNamespace { get; private set; }
        public string? SelectedResourceType { get; private set; }

        public FavoritesDialog(ApiFavoriteService favoriteService)
        {
            _favoriteService = favoriteService ?? throw new ArgumentNullException(nameof(favoriteService));

            InitializeComponent();
            InitializeControls(); // CRITICAL: Wire up button event handlers

            var skin = MaterialSkinManager.Instance;
            skin.AddFormToManage(this);

            _ = LoadFavoritesAsync();
        }

        private void InitializeControls()
        {
            // Event handlers setup
            _txtSearch.TextChanged += (s, e) => FilterFavorites();
            _cbResourceType.SelectedIndexChanged += (s, e) => FilterFavorites();
            _cbNamespace.SelectedIndexChanged += (s, e) => FilterFavorites();
            _btnRefresh.Click += async (s, e) => await LoadFavoritesAsync();
            _btnAdd.Click += (s, e) => AddFavorite();
            _btnEdit.Click += async (s, e) => await EditFavoriteAsync();
            _btnDelete.Click += async (s, e) => await DeleteFavoriteAsync();
            _btnGoto.Click += (s, e) => GotoResource();
            _btnClose.Click += (s, e) => Close();
            _grid.DoubleClick += (s, e) => GotoResource();
        }

        private List<Favorite> _allFavorites = new List<Favorite>();

        private async Task LoadFavoritesAsync()
        {
            try
            {
                _btnRefresh.Enabled = false;
                _btnRefresh.Text = "Loading...";
                Cursor = Cursors.WaitCursor;

                _allFavorites = await _favoriteService.GetFavoritesAsync();

                // Populate namespace filter
                var namespaces = _allFavorites
                    .Select(f => f.Namespace)
                    .Where(ns => !string.IsNullOrEmpty(ns))
                    .Distinct()
                    .OrderBy(ns => ns)
                    .ToList();

                _cbNamespace.Items.Clear();
                _cbNamespace.Items.Add("All Namespaces");
                foreach (var ns in namespaces)
                {
                    if (!string.IsNullOrEmpty(ns))
                        _cbNamespace.Items.Add(ns);
                }
                _cbNamespace.SelectedIndex = 0;

                FilterFavorites();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this,
                    $"Error loading favorites:\n{ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                _btnRefresh.Enabled = true;
                _btnRefresh.Text = "Refresh";
                Cursor = Cursors.Default;
            }
        }

        private void FilterFavorites()
        {
            var filtered = _allFavorites.AsEnumerable();

            // Filter by search text
            var searchText = _txtSearch.Text?.Trim().ToLower();
            if (!string.IsNullOrEmpty(searchText))
            {
                filtered = filtered.Where(f =>
                    (f.ResourceName?.ToLower().Contains(searchText) ?? false) ||
                    (f.Notes?.ToLower().Contains(searchText) ?? false));
            }

            // Filter by resource type
            if (_cbResourceType.SelectedIndex > 0)
            {
                var selectedType = _cbResourceType.SelectedItem?.ToString() ?? "";
                filtered = filtered.Where(f => f.ResourceKind == selectedType);
            }

            // Filter by namespace
            if (_cbNamespace.SelectedIndex > 0)
            {
                var selectedNs = _cbNamespace.SelectedItem?.ToString() ?? "";
                filtered = filtered.Where(f => f.Namespace == selectedNs);
            }

            var results = filtered.OrderByDescending(f => f.CreatedAt).ToList();
            _grid.DataSource = results;
            _lblTotal.Text = $"Total: {results.Count} favorites (of {_allFavorites.Count})";
        }

        private void AddFavorite()
        {
            using var dialog = new AddFavoriteDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                _ = CreateFavoriteAsync(
                    dialog.ResourceType,
                    dialog.ResourceName,
                    dialog.Namespace,
                    dialog.Notes);
            }
        }

        private async Task CreateFavoriteAsync(string resourceType, string resourceName, string ns, string notes)
        {
            try
            {
                await _favoriteService.CreateFavoriteAsync(resourceType, resourceName, ns, notes);
                await LoadFavoritesAsync();
                MessageBox.Show(this, "Favorite added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, $"Error adding favorite:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task EditFavoriteAsync()
        {
            if (_grid.SelectedRows.Count == 0) return;

            var favorite = _grid.SelectedRows[0].DataBoundItem as Favorite;
            if (favorite == null) return;

            var newNotes = Microsoft.VisualBasic.Interaction.InputBox(
                "Enter new notes for this favorite:",
                "Edit Notes",
                favorite.Notes ?? "",
                -1, -1);

            if (!string.IsNullOrEmpty(newNotes))
            {
                _btnEdit.Enabled = false;
                try
                {
                    await _favoriteService.UpdateFavoriteAsync(favorite.Id, newNotes);
                    await LoadFavoritesAsync();
                    MessageBox.Show(this, "Notes updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, $"Error updating notes:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    _btnEdit.Enabled = true;
                }
            }
        }

        private async Task DeleteFavoriteAsync()
        {
            if (_grid.SelectedRows.Count == 0) return;

            var favorite = _grid.SelectedRows[0].DataBoundItem as Favorite;
            if (favorite == null) return;

            var result = MessageBox.Show(this,
                $"Delete favorite:\n{favorite.ResourceKind}/{favorite.ResourceName}?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                _btnDelete.Enabled = false;
                try
                {
                    await _favoriteService.DeleteFavoriteAsync(favorite.Id);
                    await LoadFavoritesAsync();
                    MessageBox.Show(this, "Favorite deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, $"Error deleting favorite:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    _btnDelete.Enabled = true;
                }
            }
        }

        private void GotoResource()
        {
            if (_grid.SelectedRows.Count == 0) return;

            var favorite = _grid.SelectedRows[0].DataBoundItem as Favorite;
            if (favorite == null) return;

            SelectedResource = favorite.ResourceName;
            SelectedNamespace = favorite.Namespace;
            SelectedResourceType = favorite.ResourceKind;

            DialogResult = DialogResult.OK;
            Close();
        }
    }

    [DesignerCategory("Form")]
    public partial class AddFavoriteDialog : Form
    {
        private readonly ComboBox _cbResourceType = new ComboBox();
        private readonly TextBox _txtResourceName = new TextBox();
        private readonly TextBox _txtNamespace = new TextBox();
        private readonly TextBox _txtNotes = new TextBox();
        private readonly Button _btnOK = new Button();
        private readonly Button _btnCancel = new Button();

        public string ResourceType => _cbResourceType.SelectedItem?.ToString() ?? "";
        public string ResourceName => _txtResourceName.Text;
        public string Namespace => _txtNamespace.Text;
        public string Notes => _txtNotes.Text;

        public AddFavoriteDialog()
        {
            Text = "Add Favorite";
            Size = new Size(400, 300);
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;

            var panel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20),
                ColumnCount = 2,
                RowCount = 5
            };
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120));
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            // Resource Type
            panel.Controls.Add(new Label { Text = "Resource Type:", TextAlign = ContentAlignment.MiddleLeft }, 0, 0);
            _cbResourceType.DropDownStyle = ComboBoxStyle.DropDownList;
            _cbResourceType.Items.AddRange(new[] { "Pod", "Deployment", "Service", "StatefulSet", "DaemonSet", "ConfigMap", "Secret" });
            _cbResourceType.SelectedIndex = 0;
            _cbResourceType.Dock = DockStyle.Fill;
            panel.Controls.Add(_cbResourceType, 1, 0);

            // Resource Name
            panel.Controls.Add(new Label { Text = "Resource Name:", TextAlign = ContentAlignment.MiddleLeft }, 0, 1);
            _txtResourceName.Dock = DockStyle.Fill;
            panel.Controls.Add(_txtResourceName, 1, 1);

            // Namespace
            panel.Controls.Add(new Label { Text = "Namespace:", TextAlign = ContentAlignment.MiddleLeft }, 0, 2);
            _txtNamespace.Dock = DockStyle.Fill;
            _txtNamespace.Text = "default";
            panel.Controls.Add(_txtNamespace, 1, 2);

            // Notes
            panel.Controls.Add(new Label { Text = "Notes:", TextAlign = ContentAlignment.MiddleLeft }, 0, 3);
            _txtNotes.Multiline = true;
            _txtNotes.Height = 60;
            _txtNotes.Dock = DockStyle.Fill;
            panel.Controls.Add(_txtNotes, 1, 3);

            // Buttons
            var buttonPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.RightToLeft
            };
            _btnOK.Text = "Add";
            _btnOK.DialogResult = DialogResult.OK;
            _btnCancel.Text = "Cancel";
            _btnCancel.DialogResult = DialogResult.Cancel;
            buttonPanel.Controls.Add(_btnCancel);
            buttonPanel.Controls.Add(_btnOK);
            panel.Controls.Add(buttonPanel, 1, 4);

            Controls.Add(panel);
            AcceptButton = _btnOK;
            CancelButton = _btnCancel;
        }
    }
}
