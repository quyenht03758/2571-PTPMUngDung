using MaterialSkin;
using System.ComponentModel;
using MaterialSkin.Controls;

namespace K8sManager.Views
{
    /// <summary>
    /// Dialog for creating or editing YAML templates
    /// </summary>
    [DesignerCategory("Form")]

    public partial class TemplateEditorDialog : MaterialForm
    {
        private MaterialTextBox2 _txtName = null!;
        private MaterialComboBox _cmbCategory = null!;
        private MaterialMultiLineTextBox2 _txtDescription = null!;
        private MaterialTextBox2 _txtTags = null!;
        private MaterialCheckbox _chkPublic = null!;
        private RichTextBox _txtYaml = null!;
        private MaterialButton _btnSave = null!;
        private MaterialButton _btnCancel = null!;
        private Label _lblHelp = null!;

        public string TemplateName => _txtName.Text;
        public string Category => _cmbCategory.SelectedItem?.ToString() ?? "Other";
        public string Description => _txtDescription.Text;
        public string Tags => _txtTags.Text;
        public bool IsPublic => _chkPublic.Checked;
        public string YamlContent => _txtYaml.Text;

        public TemplateEditorDialog(string? name = null, string? category = null,
            string? description = null, string? tags = null, bool isPublic = false,
            string? yamlContent = null)
        {
            InitializeComponent();
            InitializeFormControls();

            if (!string.IsNullOrEmpty(name))
            {
                _txtName.Text = name;
                _cmbCategory.SelectedItem = category ?? "Other";
                _txtDescription.Text = description ?? "";
                _txtTags.Text = tags ?? "";
                _chkPublic.Checked = isPublic;
                _txtYaml.Text = yamlContent ?? "";
                Text = $"✏️ Edit Template: {name}";
            }
        }

        private void InitializeFormControls()
        {
            // MaterialSkin setup
            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            materialSkinManager.ColorScheme = new ColorScheme(
                Primary.Blue600, Primary.Blue700,
                Primary.Blue200, Accent.LightBlue200,
                TextShade.WHITE
            );

            Text = "📝 Create Template";
            Size = new Size(900, 750);
            StartPosition = FormStartPosition.CenterParent;
            MaximizeBox = false;
            MinimizeBox = false;
            Padding = new Padding(5, 70, 5, 5);

            // Main layout panel
            var mainPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 7,
                ColumnCount = 2,
                Padding = new Padding(20, 10, 20, 10)
            };

            mainPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 60F)); // Name
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 60F)); // Category
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 100F)); // Description
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 60F)); // Tags
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F)); // Public checkbox
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F)); // YAML editor
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 70F)); // Buttons

            mainPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120F));
            mainPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));

            // Row 0: Template Name
            var lblName = new MaterialLabel
            {
                Text = "Name:",
                AutoSize = false,
                Size = new Size(110, 40),
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold)
            };

            _txtName = new MaterialTextBox2
            {
                Hint = "Enter template name",
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 10F)
            };

            mainPanel.Controls.Add(lblName, 0, 0);
            mainPanel.Controls.Add(_txtName, 1, 0);

            // Row 1: Category
            var lblCategory = new MaterialLabel
            {
                Text = "Category:",
                AutoSize = false,
                Size = new Size(110, 40),
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold)
            };

            _cmbCategory = new MaterialComboBox
            {
                Dock = DockStyle.Fill,
                Hint = "Select category",
                Font = new Font("Segoe UI", 10F)
            };
            _cmbCategory.Items.AddRange(new object[]
            {
                "Workload",
                "Network",
                "Storage",
                "Config",
                "Other"
            });
            _cmbCategory.SelectedIndex = 0;

            mainPanel.Controls.Add(lblCategory, 0, 1);
            mainPanel.Controls.Add(_cmbCategory, 1, 1);

            // Row 2: Description
            var lblDescription = new MaterialLabel
            {
                Text = "Description:",
                AutoSize = false,
                Size = new Size(110, 40),
                TextAlign = ContentAlignment.TopLeft,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold)
            };

            _txtDescription = new MaterialMultiLineTextBox2
            {
                Hint = "Template description (optional)",
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 10F)
            };

            mainPanel.Controls.Add(lblDescription, 0, 2);
            mainPanel.Controls.Add(_txtDescription, 1, 2);

            // Row 3: Tags
            var lblTags = new MaterialLabel
            {
                Text = "Tags:",
                AutoSize = false,
                Size = new Size(110, 40),
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold)
            };

            _txtTags = new MaterialTextBox2
            {
                Hint = "Comma-separated tags (e.g., deployment, web, nginx)",
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 10F)
            };

            mainPanel.Controls.Add(lblTags, 0, 3);
            mainPanel.Controls.Add(_txtTags, 1, 3);

            // Row 4: Public checkbox
            var lblPublic = new MaterialLabel
            {
                Text = "Visibility:",
                AutoSize = false,
                Size = new Size(110, 30),
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold)
            };

            _chkPublic = new MaterialCheckbox
            {
                Text = "Make this template public (visible to all users)",
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 9F)
            };

            mainPanel.Controls.Add(lblPublic, 0, 4);
            mainPanel.Controls.Add(_chkPublic, 1, 4);

            // Row 5: YAML Editor
            var yamlPanel = new Panel
            {
                Dock = DockStyle.Fill
            };
            mainPanel.SetColumnSpan(yamlPanel, 2);

            var lblYaml = new MaterialLabel
            {
                Text = "YAML Content:",
                AutoSize = true,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Dock = DockStyle.Top,
                Height = 30
            };

            _lblHelp = new Label
            {
                Text = "💡 Tip: Use {{VARIABLE_NAME}} for placeholders (e.g., {{APP_NAME}}, {{NAMESPACE}})",
                Dock = DockStyle.Top,
                Height = 25,
                Font = new Font("Segoe UI", 8.5F, FontStyle.Italic),
                ForeColor = Color.FromArgb(100, 100, 100),
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(0, 5, 0, 0)
            };

            _txtYaml = new RichTextBox
            {
                Dock = DockStyle.Fill,
                Font = new Font("Consolas", 10F),
                WordWrap = false,
                AcceptsTab = true,
                ScrollBars = RichTextBoxScrollBars.Both,
                BorderStyle = BorderStyle.FixedSingle
            };

            yamlPanel.Controls.Add(_txtYaml);
            yamlPanel.Controls.Add(_lblHelp);
            yamlPanel.Controls.Add(lblYaml);

            mainPanel.Controls.Add(yamlPanel, 0, 5);

            // Row 6: Buttons
            var buttonPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.RightToLeft,
                Padding = new Padding(0, 15, 0, 0)
            };
            mainPanel.SetColumnSpan(buttonPanel, 2);

            _btnSave = new MaterialButton
            {
                Text = "💾 Save Template",
                Size = new Size(170, 40),
                Type = MaterialButton.MaterialButtonType.Contained,
                UseAccentColor = false
            };
            _btnSave.Click += BtnSave_Click;

            _btnCancel = new MaterialButton
            {
                Text = "Cancel",
                Size = new Size(120, 40),
                Type = MaterialButton.MaterialButtonType.Outlined,
                Margin = new Padding(0, 0, 10, 0)
            };
            _btnCancel.Click += (s, e) => DialogResult = DialogResult.Cancel;

            buttonPanel.Controls.Add(_btnSave);
            buttonPanel.Controls.Add(_btnCancel);

            mainPanel.Controls.Add(buttonPanel, 0, 6);

            Controls.Add(mainPanel);
        }

        private void BtnSave_Click(object? sender, EventArgs e)
        {
            // Validation
            if (string.IsNullOrWhiteSpace(_txtName.Text))
            {
                MessageBox.Show("Please enter a template name.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _txtName.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(_txtYaml.Text))
            {
                MessageBox.Show("Please enter YAML content.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _txtYaml.Focus();
                return;
            }

            // Basic YAML validation - check if it starts with valid YAML
            var yaml = _txtYaml.Text.Trim();
            if (!yaml.Contains("apiVersion:") && !yaml.Contains("kind:"))
            {
                var result = MessageBox.Show(
                    "The YAML content doesn't appear to contain standard Kubernetes resource fields (apiVersion, kind).\n\n" +
                    "Do you want to save it anyway?",
                    "YAML Validation Warning",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (result != DialogResult.Yes)
                {
                    return;
                }
            }

            DialogResult = DialogResult.OK;
        }
    }
}
