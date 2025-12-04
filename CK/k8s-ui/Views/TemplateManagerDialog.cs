using System.Data;
using System.ComponentModel;
using K8sManager.Controllers;
using K8sManager.Models.Entities;
using K8sManager.Services;
using MaterialSkin;
using MaterialSkin.Controls;

namespace K8sManager.Views
{
    [DesignerCategory("Form")]
    public partial class TemplateManagerDialog : MaterialForm
    {
        private readonly ApiTemplateService _templateService;
        private readonly ApiSessionService _sessionService;
        private readonly CleanExplorerController _controller;

        private MaterialLabel _lblCategory = null!;
        private MaterialComboBox _cmbCategory = null!;
        private MaterialTextBox2 _txtSearch = null!;
        private DataGridView _gridTemplates = null!;
        private MaterialLabel _lblVersions = null!;
        private MaterialComboBox _cmbVersion = null!;
        private RichTextBox _txtYaml = null!;
        private MaterialButton _btnApply = null!;
        private MaterialButton _btnNew = null!;
        private MaterialButton _btnEdit = null!;
        private MaterialButton _btnDelete = null!;
        private MaterialButton _btnClose = null!;

        private Template? _selectedTemplate;

        public TemplateManagerDialog(
            ApiTemplateService templateService,
            ApiSessionService sessionService,
            CleanExplorerController controller)
        {
            _templateService = templateService;
            _sessionService = sessionService;
            _controller = controller;

            InitializeComponent();
            InitializeFormControls();
            LoadTemplatesAsync().ConfigureAwait(false);
        }

        private void InitializeFormControls()
        {
            // MaterialSkin setup
            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            materialSkinManager.ColorScheme = new ColorScheme(
                Primary.Indigo500, Primary.Indigo700,
                Primary.Indigo100, Accent.Pink200,
                TextShade.WHITE
            );

            Text = "📋 Template Manager";
            Size = new Size(1400, 850);
            StartPosition = FormStartPosition.CenterParent;
            MaximizeBox = true;
            MinimizeBox = false;
            Padding = new Padding(5, 70, 5, 5);

            // Top panel: Category filter and search
            var topPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 70,
                Padding = new Padding(15, 10, 15, 10)
            };

            _lblCategory = new MaterialLabel
            {
                Text = "Category:",
                Location = new Point(15, 15),
                AutoSize = true,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold)
            };

            _cmbCategory = new MaterialComboBox
            {
                Location = new Point(110, 10),
                Width = 220,
                Hint = "All Categories",
                Font = new Font("Segoe UI", 10F)
            };
            _cmbCategory.Items.AddRange(new object[]
            {
                "All",
                "Workload",
                "Network",
                "Storage",
                "Config"
            });
            _cmbCategory.SelectedIndex = 0;
            _cmbCategory.SelectedIndexChanged += async (s, e) => await LoadTemplatesAsync().ConfigureAwait(false);

            _txtSearch = new MaterialTextBox2
            {
                Location = new Point(350, 10),
                Width = 400,
                Hint = "🔍 Search templates...",
                Font = new Font("Segoe UI", 10F)
            };
            _txtSearch.TextChanged += async (s, e) => await LoadTemplatesAsync().ConfigureAwait(false);

            topPanel.Controls.AddRange(new Control[] { _lblCategory, _cmbCategory, _txtSearch });

            // Main split container
            var splitContainer = new SplitContainer
            {
                Dock = DockStyle.Fill,
                Orientation = Orientation.Horizontal,
                SplitterDistance = 280,
                SplitterWidth = 8,
                FixedPanel = FixedPanel.None
            };

            // Top panel: Templates grid
            _gridTemplates = new DataGridView
            {
                Dock = DockStyle.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                RowHeadersVisible = false,
                Font = new Font("Segoe UI", 10F),
                RowTemplate = { Height = 35 },
                ColumnHeadersHeight = 40,
                ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
                {
                    Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                    BackColor = Color.FromArgb(63, 81, 181),
                    ForeColor = Color.White,
                    Padding = new Padding(5)
                }
            };
            _gridTemplates.SelectionChanged += OnTemplateSelected;

            splitContainer.Panel1.Controls.Add(_gridTemplates);
            splitContainer.Panel1.Padding = new Padding(10, 5, 10, 5);

            // Bottom panel: Version selector and YAML viewer
            var bottomPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(10, 5, 10, 5)
            };

            _lblVersions = new MaterialLabel
            {
                Text = "Version:",
                Location = new Point(10, 12),
                AutoSize = true,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold)
            };

            _cmbVersion = new MaterialComboBox
            {
                Location = new Point(90, 7),
                Width = 180,
                Font = new Font("Segoe UI", 10F)
            };
            _cmbVersion.SelectedIndexChanged += OnVersionChanged;

            var lblYaml = new MaterialLabel
            {
                Text = "YAML Content:",
                Location = new Point(290, 12),
                AutoSize = true,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.FromArgb(100, 100, 100)
            };

            // Use RichTextBox instead for better formatting and scrolling
            var yamlPanel = new Panel
            {
                Location = new Point(10, 45),
                Size = new Size(1360, 390),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.White
            };

            _txtYaml = new RichTextBox
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                Font = new Font("Consolas", 11F),
                BackColor = Color.White,
                BorderStyle = BorderStyle.None,
                WordWrap = false,
                ScrollBars = RichTextBoxScrollBars.Both,
                DetectUrls = false,
                Multiline = true
            };

            yamlPanel.Controls.Add(_txtYaml);

            bottomPanel.Controls.AddRange(new Control[] { _lblVersions, _cmbVersion, lblYaml, yamlPanel });
            splitContainer.Panel2.Controls.Add(bottomPanel);
            splitContainer.Panel2.Padding = new Padding(10, 5, 10, 5);

            // Bottom button panel
            var buttonPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 70,
                Padding = new Padding(15, 10, 15, 10),
                BackColor = Color.FromArgb(245, 245, 245)
            };

            _btnApply = new MaterialButton
            {
                Text = "✅ Apply to Cluster",
                Location = new Point(15, 12),
                Size = new Size(190, 45),
                Type = MaterialButton.MaterialButtonType.Contained,
                Enabled = false,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold)
            };
            _btnApply.Click += BtnApply_Click;

            _btnNew = new MaterialButton
            {
                Text = "➕ New Template",
                Location = new Point(220, 12),
                Size = new Size(170, 45),
                Type = MaterialButton.MaterialButtonType.Outlined,
                Font = new Font("Segoe UI", 10F)
            };
            _btnNew.Click += BtnNew_Click;

            _btnEdit = new MaterialButton
            {
                Text = "✏️ Edit",
                Location = new Point(405, 12),
                Size = new Size(130, 45),
                Type = MaterialButton.MaterialButtonType.Outlined,
                Enabled = false,
                Font = new Font("Segoe UI", 10F)
            };
            _btnEdit.Click += BtnEdit_Click;

            _btnDelete = new MaterialButton
            {
                Text = "🗑️ Delete",
                Location = new Point(550, 12),
                Size = new Size(130, 45),
                Type = MaterialButton.MaterialButtonType.Outlined,
                Enabled = false,
                Font = new Font("Segoe UI", 10F)
            };
            _btnDelete.Click += BtnDelete_Click;

            var _btnRefresh = new MaterialButton
            {
                Text = "🔄 Refresh",
                Location = new Point(695, 12),
                Size = new Size(130, 45),
                Type = MaterialButton.MaterialButtonType.Outlined,
                Font = new Font("Segoe UI", 10F)
            };
            _btnRefresh.Click += async (s, e) => await LoadTemplatesAsync().ConfigureAwait(true);

            _btnClose = new MaterialButton
            {
                Text = "Close",
                Location = new Point(1255, 12),
                Size = new Size(120, 45),
                Type = MaterialButton.MaterialButtonType.Text,
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Font = new Font("Segoe UI", 10F)
            };
            _btnClose.Click += (s, e) => Close();

            buttonPanel.Controls.AddRange(new Control[] { _btnApply, _btnNew, _btnEdit, _btnDelete, _btnRefresh, _btnClose });

            // Add all panels to form
            Controls.Add(splitContainer);
            Controls.Add(topPanel);
            Controls.Add(buttonPanel);
        }

        private async Task LoadTemplatesAsync()
        {
            try
            {
                var category = _cmbCategory.SelectedItem?.ToString();
                var searchText = _txtSearch.Text;

                var templates = await _templateService.GetTemplatesAsync(
                    category == "All" ? null : category,
                    string.IsNullOrEmpty(searchText) ? null : searchText).ConfigureAwait(false);

                // Filtering is now done by API

                // Bind to grid
                var gridData = templates.Select(t => new
                {
                    t.Id,
                    t.Name,
                    Category = t.Category ?? "Other",
                    t.Description,
                    Tags = t.Tags ?? "",
                    IsPublic = t.IsPublic ? "✅ Public" : "🔒 Private",
                    CreatedAt = t.CreatedAt.ToString("yyyy-MM-dd")
                }).ToList();

                if (_gridTemplates.InvokeRequired)
                {
                    _gridTemplates.Invoke(() =>
                    {
                        _gridTemplates.DataSource = gridData;
                        if (_gridTemplates.Columns["Id"] != null)
                            _gridTemplates.Columns["Id"].Visible = false;
                    });
                }
                else
                {
                    _gridTemplates.DataSource = gridData;
                    if (_gridTemplates.Columns["Id"] != null)
                        _gridTemplates.Columns["Id"].Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading templates: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void OnTemplateSelected(object? sender, EventArgs e)
        {
            if (_gridTemplates.SelectedRows.Count == 0)
            {
                _selectedTemplate = null;
                _btnApply.Enabled = false;
                _btnEdit.Enabled = false;
                _btnDelete.Enabled = false;
                _cmbVersion.Items.Clear();
                _txtYaml.Text = "";
                return;
            }

            var row = _gridTemplates.SelectedRows[0];
            var templateId = (int)row.Cells["Id"].Value;

            // Get template with versions from API
            _selectedTemplate = await _templateService.GetTemplateByIdAsync(templateId).ConfigureAwait(false);
            if (_selectedTemplate == null) return;

            // Load versions from template
            var versions = await _templateService.GetTemplateVersionsAsync(templateId).ConfigureAwait(false);
            _cmbVersion.Items.Clear();

            foreach (var ver in versions.OrderByDescending(v => v.VersionNumber))
            {
                var isCurrent = ver.IsCurrent;
                _cmbVersion.Items.Add($"v{ver.VersionNumber}{(isCurrent ? " (Current)" : "")}");
            }

            if (_cmbVersion.Items.Count > 0)
            {
                _cmbVersion.SelectedIndex = 0;

                // Load YAML content for the first/current version
                var currentVersion = versions.OrderByDescending(v => v.VersionNumber).FirstOrDefault();
                if (currentVersion != null && !string.IsNullOrWhiteSpace(currentVersion.YamlContent))
                {
                    FormatYamlContent(currentVersion.YamlContent);
                }
            }
            else
            {
                // No versions exist - this shouldn't happen with auto-create, but handle gracefully
                _txtYaml.Text = "⚠️ Template has no versions. Please edit the template to create initial version.";
            }

            // Enable buttons based on permissions
            // Apply: Any authenticated user can apply templates to cluster
            _btnApply.Enabled = _sessionService.IsAuthenticated && _cmbVersion.Items.Count > 0;
            _btnEdit.Enabled = _sessionService.CanWrite();
            _btnDelete.Enabled = _sessionService.CanDelete();

            Console.WriteLine($"[TemplateManager] Apply enabled: {_btnApply.Enabled}, IsAuth: {_sessionService.IsAuthenticated}, VersionCount: {_cmbVersion.Items.Count}");
        }

        private async void OnVersionChanged(object? sender, EventArgs e)
        {
            if (_selectedTemplate == null || _cmbVersion.SelectedItem == null) return;

            var versionText = _cmbVersion.SelectedItem.ToString();
            if (versionText == null) return;

            // Extract version number from "v1 (Current)" or "v2"
            var versionNumber = int.Parse(versionText.Split(' ')[0].Substring(1));

            var versions = await _templateService.GetTemplateVersionsAsync(_selectedTemplate.Id).ConfigureAwait(false);
            var version = versions.FirstOrDefault(v => v.VersionNumber == versionNumber);
            if (version != null)
            {
                FormatYamlContent(version.YamlContent);
            }
        }

        private void FormatYamlContent(string yamlContent)
        {
            if (string.IsNullOrWhiteSpace(yamlContent))
            {
                _txtYaml.Text = "⚠️ Empty YAML content";
                return;
            }

            _txtYaml.Clear();
            _txtYaml.Text = yamlContent;

            // Apply syntax highlighting
            var lines = yamlContent.Split('\n');
            int position = 0;

            foreach (var line in lines)
            {
                var trimmedLine = line.TrimStart();

                // Comments (lines starting with #)
                if (trimmedLine.StartsWith("#"))
                {
                    _txtYaml.Select(position, line.Length);
                    _txtYaml.SelectionColor = Color.FromArgb(106, 153, 85); // Green
                    _txtYaml.SelectionFont = new Font(_txtYaml.Font, FontStyle.Italic);
                }
                // Keys (text before colon)
                else if (trimmedLine.Contains(":"))
                {
                    var colonIndex = line.IndexOf(':');
                    if (colonIndex > 0)
                    {
                        // Highlight key
                        _txtYaml.Select(position, colonIndex);
                        _txtYaml.SelectionColor = Color.FromArgb(0, 92, 197); // Blue
                        _txtYaml.SelectionFont = new Font(_txtYaml.Font, FontStyle.Bold);

                        // Highlight value
                        var valueStart = position + colonIndex + 1;
                        var valueLength = line.Length - colonIndex - 1;
                        if (valueLength > 0)
                        {
                            _txtYaml.Select(valueStart, valueLength);
                            var value = line.Substring(colonIndex + 1).Trim();

                            // Numbers
                            if (int.TryParse(value, out _) || value == "true" || value == "false")
                            {
                                _txtYaml.SelectionColor = Color.FromArgb(9, 134, 88); // Dark green
                            }
                            // Strings with quotes
                            else if (value.StartsWith("\"") || value.StartsWith("'"))
                            {
                                _txtYaml.SelectionColor = Color.FromArgb(163, 21, 21); // Red
                            }
                            else
                            {
                                _txtYaml.SelectionColor = Color.Black;
                            }
                        }
                    }
                }
                // List items (lines starting with -)
                else if (trimmedLine.StartsWith("-"))
                {
                    _txtYaml.Select(position + line.IndexOf('-'), 1);
                    _txtYaml.SelectionColor = Color.FromArgb(215, 58, 73); // Pink/Red
                    _txtYaml.SelectionFont = new Font(_txtYaml.Font, FontStyle.Bold);
                }

                position += line.Length + 1; // +1 for newline
            }

            // Reset selection and scroll to top
            _txtYaml.Select(0, 0);
            _txtYaml.SelectionColor = _txtYaml.ForeColor;
            _txtYaml.SelectionStart = 0;
            _txtYaml.ScrollToCaret();
        }

        private async void BtnApply_Click(object? sender, EventArgs e)
        {
            if (_selectedTemplate == null || string.IsNullOrWhiteSpace(_txtYaml.Text))
            {
                MessageBox.Show("Please select a template and version first.", "No Template Selected",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_controller == null)
            {
                MessageBox.Show("Controller not initialized. Please restart the application.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                // Get available namespaces - use a simple default list
                var namespaceArray = new[] { "default", "kube-system", "kube-public", "kube-node-lease" };

                if (namespaceArray.Length == 0)
                {
                    MessageBox.Show("No namespaces found. Please ensure cluster connection is active.", "No Namespaces",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Open variable input dialog
                using var varDialog = new VariableInputDialog(_selectedTemplate.Name ?? "Template", _txtYaml.Text, namespaceArray);
                if (varDialog.ShowDialog(this) == DialogResult.OK)
                {
                    // Perform variable substitution
                    string yamlToApply = _txtYaml.Text;
                    foreach (var kvp in varDialog.VariableValues)
                    {
                        yamlToApply = yamlToApply.Replace($"{{{{{kvp.Key}}}}}", kvp.Value);
                    }

                    // Show preview if requested
                    if (varDialog.ShowPreview)
                    {
                        var previewDialog = new Form
                        {
                            Text = $"Preview: {_selectedTemplate.Name}",
                            Size = new Size(800, 600),
                            StartPosition = FormStartPosition.CenterParent,
                            MaximizeBox = true,
                            MinimizeBox = false
                        };

                        var txtPreview = new RichTextBox
                        {
                            Dock = DockStyle.Fill,
                            Font = new Font("Consolas", 14F),
                            Text = yamlToApply,
                            ReadOnly = true,
                            WordWrap = false,
                            ScrollBars = RichTextBoxScrollBars.Both
                        };

                        var btnPanel = new Panel
                        {
                            Dock = DockStyle.Bottom,
                            Height = 60,
                            Padding = new Padding(15)
                        };

                        var btnConfirm = new Button
                        {
                            Text = "✅ Apply",
                            Location = new Point(520, 15),
                            Size = new Size(120, 35),
                            DialogResult = DialogResult.OK
                        };

                        var btnEdit = new Button
                        {
                            Text = "✏️ Edit",
                            Location = new Point(390, 15),
                            Size = new Size(120, 35),
                            DialogResult = DialogResult.Retry
                        };

                        var btnCancel = new Button
                        {
                            Text = "Cancel",
                            Location = new Point(650, 15),
                            Size = new Size(120, 35),
                            DialogResult = DialogResult.Cancel
                        };

                        btnPanel.Controls.AddRange(new Control[] { btnConfirm, btnEdit, btnCancel });
                        previewDialog.Controls.Add(txtPreview);
                        previewDialog.Controls.Add(btnPanel);

                        var previewResult = previewDialog.ShowDialog(this);
                        if (previewResult == DialogResult.Retry)
                        {
                            // User wants to edit - put back in text box and cancel
                            MessageBox.Show("Edit mode not yet implemented. Please cancel and edit the template directly.",
                                "Coming Soon", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                        else if (previewResult != DialogResult.OK)
                        {
                            return; // User cancelled
                        }
                    }

                    // Apply the YAML to cluster
                    try
                    {
                        await _controller.ApplyYamlAsync(yamlToApply).ConfigureAwait(false);
                        
                        MessageBox.Show(
                            $"✓ Template '{_selectedTemplate.Name ?? "Template"}' applied successfully!",
                            "Success",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }
                    catch (Exception applyEx)
                    {
                        MessageBox.Show(
                            $"Failed to apply template:\n{applyEx.Message}\n\nPlease check YAML syntax and cluster connection.",
                            "Apply Failed",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error applying template: {ex.Message}\n\nStack trace:\n{ex.StackTrace}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void BtnNew_Click(object? sender, EventArgs e)
        {
            using var editor = new TemplateEditorDialog();
            if (editor.ShowDialog(this) == DialogResult.OK)
            {
                // Get current user ID from session
                var currentUser = _sessionService.CurrentUser;
                if (currentUser == null)
                {
                    MessageBox.Show("Session expired. Please login again.", "Authentication Required",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                try
                {
                    var templateId = await _templateService.CreateTemplateAsync(
                        editor.TemplateName,
                        editor.Description,
                        editor.Category,
                        editor.YamlContent,
                        currentUser.UserId,
                        editor.Tags,
                        editor.IsPublic).ConfigureAwait(false);

                    if (templateId > 0)
                    {
                        MessageBox.Show(
                            $"✓ Template '{editor.TemplateName}' created successfully!",
                            "Success",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);

                        // Refresh template list
                        await LoadTemplatesAsync().ConfigureAwait(false);

                        // Select the newly created template on UI thread
                        if (_gridTemplates.InvokeRequired)
                        {
                            _gridTemplates.Invoke(() =>
                            {
                                for (int i = 0; i < _gridTemplates.Rows.Count; i++)
                                {
                                    if ((int)_gridTemplates.Rows[i].Cells["Id"].Value == templateId)
                                    {
                                        _gridTemplates.Rows[i].Selected = true;
                                        _gridTemplates.FirstDisplayedScrollingRowIndex = i;
                                        break;
                                    }
                                }
                            });
                        }
                        else
                        {
                            for (int i = 0; i < _gridTemplates.Rows.Count; i++)
                            {
                                if ((int)_gridTemplates.Rows[i].Cells["Id"].Value == templateId)
                                {
                                    _gridTemplates.Rows[i].Selected = true;
                                    _gridTemplates.FirstDisplayedScrollingRowIndex = i;
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Failed to create template. Please check logs for details.", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error creating template: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private async void BtnEdit_Click(object? sender, EventArgs e)
        {
            if (_selectedTemplate == null)
            {
                MessageBox.Show("Please select a template first.", "No Template Selected",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using var editor = new TemplateEditorDialog(
                _selectedTemplate.Name,
                _selectedTemplate.Category,
                _selectedTemplate.Description,
                _selectedTemplate.Tags,
                _selectedTemplate.IsPublic,
                _txtYaml.Text);

            if (editor.ShowDialog(this) == DialogResult.OK)
            {
                // Get current user ID from session
                var currentUser = _sessionService.CurrentUser;
                if (currentUser == null)
                {
                    MessageBox.Show("Session expired. Please login again.", "Authentication Required",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                try
                {
                    var success = await _templateService.UpdateTemplateAsync(
                        _selectedTemplate.Id,
                        editor.TemplateName,
                        editor.Description,
                        editor.Category,
                        editor.YamlContent,
                        currentUser.UserId,
                        editor.Tags,
                        editor.IsPublic).ConfigureAwait(false);

                    if (success)
                    {
                        MessageBox.Show(
                            $"✓ Template '{editor.TemplateName}' updated successfully!",
                            "Success",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);

                        // Refresh template list and reselect
                        var updatedTemplateId = _selectedTemplate.Id;
                        await LoadTemplatesAsync().ConfigureAwait(true);
                        
                        // Reselect the updated template on UI thread
                        if (_gridTemplates.InvokeRequired)
                        {
                            _gridTemplates.Invoke(new Action(() =>
                            {
                                foreach (DataGridViewRow row in _gridTemplates.Rows)
                                {
                                    if (row.DataBoundItem is Template t && t.Id == updatedTemplateId)
                                    {
                                        _gridTemplates.ClearSelection();
                                        row.Selected = true;
                                        if (row.Index >= 0 && row.Index < _gridTemplates.Rows.Count)
                                        {
                                            _gridTemplates.FirstDisplayedScrollingRowIndex = row.Index;
                                        }
                                        break;
                                    }
                                }
                            }));
                        }
                        else
                        {
                            foreach (DataGridViewRow row in _gridTemplates.Rows)
                            {
                                if (row.DataBoundItem is Template t && t.Id == updatedTemplateId)
                                {
                                    _gridTemplates.ClearSelection();
                                    row.Selected = true;
                                    if (row.Index >= 0 && row.Index < _gridTemplates.Rows.Count)
                                    {
                                        _gridTemplates.FirstDisplayedScrollingRowIndex = row.Index;
                                    }
                                    break;
                                }
                            }
                        }

                        // Re-select the updated template on UI thread
                        if (_gridTemplates.InvokeRequired)
                        {
                            _gridTemplates.Invoke(() =>
                            {
                                for (int i = 0; i < _gridTemplates.Rows.Count; i++)
                                {
                                    if ((int)_gridTemplates.Rows[i].Cells["Id"].Value == _selectedTemplate.Id)
                                    {
                                        _gridTemplates.Rows[i].Selected = true;
                                        _gridTemplates.FirstDisplayedScrollingRowIndex = i;
                                        break;
                                    }
                                }
                            });
                        }
                        else
                        {
                            for (int i = 0; i < _gridTemplates.Rows.Count; i++)
                            {
                                if ((int)_gridTemplates.Rows[i].Cells["Id"].Value == _selectedTemplate.Id)
                                {
                                    _gridTemplates.Rows[i].Selected = true;
                                    _gridTemplates.FirstDisplayedScrollingRowIndex = i;
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Failed to update template.", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error updating template: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private async void BtnDelete_Click(object? sender, EventArgs e)
        {
            if (_selectedTemplate == null)
            {
                MessageBox.Show("Please select a template first.", "No Template Selected",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show(
                $"Are you sure you want to delete template '{_selectedTemplate.Name}'?\n\n" +
                "This will delete all versions of this template.\n" +
                "⚠️ This action cannot be undone!",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2);

            if (result == DialogResult.Yes)
            {
                try
                {
                    var success = await _templateService.DeleteTemplateAsync(_selectedTemplate.Id).ConfigureAwait(false);

                    if (success)
                    {
                        MessageBox.Show(
                            $"✓ Template '{_selectedTemplate.Name}' deleted successfully.",
                            "Deleted",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);

                        // Clear selection and refresh
                        _selectedTemplate = null;
                        _txtYaml.Clear();
                        _cmbVersion.Items.Clear();
                        _btnApply.Enabled = false;
                        _btnEdit.Enabled = false;
                        _btnDelete.Enabled = false;

                        await LoadTemplatesAsync().ConfigureAwait(true);
                    }
                    else
                    {
                        MessageBox.Show("Failed to delete template.", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting template: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
