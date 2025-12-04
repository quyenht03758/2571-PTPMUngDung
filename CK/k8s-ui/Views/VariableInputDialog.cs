using System.Text.RegularExpressions;
using System.ComponentModel;
using MaterialSkin;
using MaterialSkin.Controls;

namespace K8sManager.Views
{
    /// <summary>
    /// Dialog for inputting template variables before applying
    /// </summary>
    [DesignerCategory("Form")]

    public partial class VariableInputDialog : MaterialForm
    {
        private readonly Dictionary<string, Control> _variableInputs = new();
        private TableLayoutPanel _inputPanel = null!;
        private MaterialCheckbox _chkPreview = null!;
        private MaterialButton _btnApply = null!;
        private MaterialButton _btnCancel = null!;

        public Dictionary<string, string> VariableValues { get; private set; } = new();
        public bool ShowPreview => _chkPreview.Checked;

        public VariableInputDialog(string templateName, string yamlContent, string[] namespaces)
        {
            InitializeComponent();
            InitializeControls(templateName);
            ExtractAndCreateInputs(yamlContent ?? "", namespaces ?? new[] { "default" });
        }

        private void InitializeControls(string templateName)
        {
            // MaterialSkin setup
            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            materialSkinManager.ColorScheme = new ColorScheme(
                Primary.Green600, Primary.Green700,
                Primary.Green200, Accent.LightGreen200,
                TextShade.WHITE
            );

            Text = $"Apply Template: {templateName}";
            Size = new Size(600, 500);
            StartPosition = FormStartPosition.CenterParent;
            MaximizeBox = false;
            MinimizeBox = false;
            Padding = new Padding(5, 70, 5, 5);

            var mainPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20)
            };

            // Title label
            var lblTitle = new MaterialLabel
            {
                Text = "Provide values for template variables:",
                Dock = DockStyle.Top,
                Height = 35,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold)
            };

            // Scrollable panel for inputs
            var scrollPanel = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                Padding = new Padding(0, 10, 0, 10)
            };

            _inputPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                ColumnCount = 2,
                Padding = new Padding(5)
            };
            _inputPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 180F));
            _inputPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));

            scrollPanel.Controls.Add(_inputPanel);

            // Bottom panel
            var bottomPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 100,
                Padding = new Padding(0, 10, 0, 0)
            };

            _chkPreview = new MaterialCheckbox
            {
                Text = "☑ Show preview before applying",
                Dock = DockStyle.Top,
                Height = 30,
                Checked = true,
                Font = new Font("Segoe UI", 9.5F)
            };

            var buttonPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom,
                FlowDirection = FlowDirection.RightToLeft,
                Height = 50,
                Padding = new Padding(0, 10, 0, 0)
            };

            _btnApply = new MaterialButton
            {
                Text = "✅ Apply",
                Size = new Size(140, 40),
                Type = MaterialButton.MaterialButtonType.Contained,
                UseAccentColor = false
            };
            _btnApply.Click += BtnApply_Click;

            _btnCancel = new MaterialButton
            {
                Text = "Cancel",
                Size = new Size(110, 40),
                Type = MaterialButton.MaterialButtonType.Outlined,
                Margin = new Padding(0, 0, 10, 0)
            };
            _btnCancel.Click += (s, e) => DialogResult = DialogResult.Cancel;

            buttonPanel.Controls.Add(_btnApply);
            buttonPanel.Controls.Add(_btnCancel);

            bottomPanel.Controls.Add(buttonPanel);
            bottomPanel.Controls.Add(_chkPreview);

            mainPanel.Controls.Add(scrollPanel);
            mainPanel.Controls.Add(lblTitle);
            mainPanel.Controls.Add(bottomPanel);

            Controls.Add(mainPanel);
        }

        private void ExtractAndCreateInputs(string yamlContent, string[] namespaces)
        {
            if (string.IsNullOrWhiteSpace(yamlContent))
            {
                yamlContent = "";
            }

            if (namespaces == null || namespaces.Length == 0)
            {
                namespaces = new[] { "default" };
            }

            // Extract all {{VARIABLE}} patterns
            var regex = new Regex(@"\{\{(\w+)\}\}", RegexOptions.Compiled);
            var matches = regex.Matches(yamlContent);
            var variables = matches.Cast<Match>()
                .Select(m => m.Groups[1].Value)
                .Distinct()
                .OrderBy(v => v)
                .ToList();

            if (variables.Count == 0)
            {
                var lblNoVars = new Label
                {
                    Text = "This template has no variables to configure.",
                    Dock = DockStyle.Top,
                    Height = 30,
                    Font = new Font("Segoe UI", 10F, FontStyle.Italic),
                    ForeColor = Color.Gray,
                    TextAlign = ContentAlignment.MiddleCenter
                };
                _inputPanel.Controls.Add(lblNoVars);
                _inputPanel.SetColumnSpan(lblNoVars, 2);
                return;
            }

            _inputPanel.RowCount = variables.Count;

            for (int i = 0; i < variables.Count; i++)
            {
                var varName = variables[i];
                _inputPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));

                var lblVar = new Label
                {
                    Text = $"{varName}:",
                    AutoSize = false,
                    Size = new Size(170, 35),
                    TextAlign = ContentAlignment.MiddleLeft,
                    Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                    Margin = new Padding(0, 5, 10, 5)
                };

                TextBox txtValue;

                // Special handling for NAMESPACE variable - use ComboBox
                if (varName.Equals("NAMESPACE", StringComparison.OrdinalIgnoreCase))
                {
                    var cmbNamespace = new ComboBox
                    {
                        Dock = DockStyle.Fill,
                        DropDownStyle = ComboBoxStyle.DropDown,
                        Font = new Font("Segoe UI", 10F),
                        Margin = new Padding(0, 5, 0, 5)
                    };

                    // Filter out null elements before adding
                    var validNamespaces = namespaces.Where(ns => !string.IsNullOrWhiteSpace(ns)).ToArray();
                    if (validNamespaces.Length > 0)
                    {
                        cmbNamespace.Items.AddRange(validNamespaces);
                        cmbNamespace.SelectedIndex = 0;
                    }
                    else
                    {
                        cmbNamespace.Items.Add("default");
                        cmbNamespace.SelectedIndex = 0;
                    }

                    _inputPanel.Controls.Add(lblVar, 0, i);
                    _inputPanel.Controls.Add(cmbNamespace, 1, i);

                    // Store reference for retrieval
                    _variableInputs[varName] = cmbNamespace;
                }
                else
                {
                    txtValue = new TextBox
                    {
                        Dock = DockStyle.Fill,
                        Font = new Font("Segoe UI", 10F),
                        Margin = new Padding(0, 5, 0, 5)
                    };

                    // Set default values for common variables
                    if (varName.Equals("ENV", StringComparison.OrdinalIgnoreCase))
                        txtValue.Text = "production";
                    else if (varName.Contains("PORT"))
                        txtValue.Text = "8080";
                    else if (varName.Contains("REPLICAS"))
                        txtValue.Text = "3";

                    _inputPanel.Controls.Add(lblVar, 0, i);
                    _inputPanel.Controls.Add(txtValue, 1, i);

                    _variableInputs[varName] = txtValue;
                }
            }
        }

        private void BtnApply_Click(object? sender, EventArgs e)
        {
            // Collect all variable values
            VariableValues = new Dictionary<string, string>();
            var missingValues = new List<string>();

            foreach (var kvp in _variableInputs)
            {
                var value = kvp.Value is ComboBox cmb ? cmb.Text : kvp.Value.Text;

                if (string.IsNullOrWhiteSpace(value))
                {
                    missingValues.Add(kvp.Key);
                }
                else
                {
                    VariableValues[kvp.Key] = value.Trim();
                }
            }

            if (missingValues.Count > 0)
            {
                MessageBox.Show(
                    $"Please provide values for the following variables:\n\n{string.Join("\n", missingValues)}",
                    "Missing Values",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            DialogResult = DialogResult.OK;
        }
    }
}
