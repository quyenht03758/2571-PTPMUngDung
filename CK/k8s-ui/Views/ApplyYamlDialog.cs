using K8sManager.Services;
using K8sManager.Models.Entities;
using System.ComponentModel;
using MaterialSkin.Controls;
using System.Diagnostics;

namespace K8sManager.Views
{
    [DesignerCategory("Form")]
    public partial class ApplyYamlDialog : MaterialForm
    {
        private readonly K8sExplorerService? _k8sService;
        private readonly ApiAuditService? _auditService;

        public ApplyYamlDialog(K8sExplorerService? k8sService, ApiAuditService? auditService)
        {
            InitializeComponent();
            
            _k8sService = k8sService;
            _auditService = auditService;
            
            // Set placeholder YAML
            _rtYaml.Text = @"# Example YAML - replace with your configuration
apiVersion: v1
kind: Pod
metadata:
  name: my-pod
  namespace: default
  labels:
    app: my-app
spec:
  containers:
  - name: nginx
    image: nginx:latest
    ports:
    - containerPort: 80
";
        }

        private void BtnApply_Click(object? sender, EventArgs e)
        {
            _ = BtnApply_ClickAsync();
        }

        private void BtnCancel_Click(object? sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void BtnOpen_Click(object? sender, EventArgs e)
        {
            using var ofd = new OpenFileDialog
            {
                Filter = "YAML files (*.yaml;*.yml)|*.yaml;*.yml|All files (*.*)|*.*",
                Title = "Open YAML File"
            };

            if (ofd.ShowDialog(this) == DialogResult.OK)
            {
                try
                {
                    _rtYaml.Text = File.ReadAllText(ofd.FileName);
                    _txtStatus.Text = $"✅ Loaded file: {Path.GetFileName(ofd.FileName)}";
                    _txtStatus.ForeColor = Color.Green;
                }
                catch (Exception ex)
                {
                    _txtStatus.Text = $"❌ Failed to load file: {ex.Message}";
                    _txtStatus.ForeColor = Color.Red;
                }
            }
        }

        private void BtnSave_Click(object? sender, EventArgs e)
        {
            using var sfd = new SaveFileDialog
            {
                Filter = "YAML files (*.yaml)|*.yaml|All files (*.*)|*.*",
                FileName = "resource.yaml",
                Title = "Save YAML File"
            };

            if (sfd.ShowDialog(this) == DialogResult.OK)
            {
                try
                {
                    File.WriteAllText(sfd.FileName, _rtYaml.Text);
                    _txtStatus.Text = $"✅ Saved to: {Path.GetFileName(sfd.FileName)}";
                    _txtStatus.ForeColor = Color.Green;
                }
                catch (Exception ex)
                {
                    _txtStatus.Text = $"❌ Failed to save file: {ex.Message}";
                    _txtStatus.ForeColor = Color.Red;
                }
            }
        }

        private async Task BtnApply_ClickAsync()
        {
            if (string.IsNullOrWhiteSpace(_rtYaml.Text))
            {
                MessageBox.Show(this,
                    "Please enter YAML configuration to apply.",
                    "Empty YAML",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            if (_k8sService == null)
            {
                MessageBox.Show(this,
                    "Kubernetes service is not initialized.\n\nPlease restart the application.",
                    "Service Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            // Confirm before applying
            var confirmResult = MessageBox.Show(this,
                "Are you sure you want to apply this YAML configuration?\n\n" +
                "This will create or update resources in the cluster.",
                "Confirm Apply",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirmResult != DialogResult.Yes)
                return;

            try
            {
                _btnApply.Enabled = false;
                _txtStatus.Text = "⏳ Applying YAML configuration...";
                _txtStatus.ForeColor = Color.Blue;
                Application.DoEvents();

                var yaml = _rtYaml.Text;
                var success = await _k8sService.ApplyYamlAsync(yaml).ConfigureAwait(true);

                if (success)
                {
                    _txtStatus.Text = "✅ YAML applied successfully! Resource created/updated in the cluster.";
                    _txtStatus.ForeColor = Color.Green;

                    // Log audit
                    if (_auditService != null)
                    {
                        try
                        {
                            await _auditService.LogAsync(
                                userId: 1,
                                action: "ApplyYAML",
                                resourceKind: "YAML",
                                resourceName: "configuration",
                                resourceNamespace: "multiple",
                                success: true,
                                errorMessage: null,
                                details: yaml.Length > 500 ? yaml.Substring(0, 500) + "..." : yaml);
                        }
                        catch
                        {
                            // Ignore audit logging errors
                        }
                    }

                    // Auto-close after 1.5 seconds
                    await Task.Delay(1500);
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    _txtStatus.Text = "❌ Failed to apply YAML. Check the format and try again.";
                    _txtStatus.ForeColor = Color.Red;
                }
            }
            catch (Exception ex)
            {
                _txtStatus.Text = $"❌ Error applying YAML: {ex.Message}";
                _txtStatus.ForeColor = Color.Red;

                MessageBox.Show(this,
                    $"Failed to apply YAML configuration:\n\n{ex.Message}\n\n" +
                    "Please check your YAML syntax and ensure you have permissions to create/update the resource.",
                    "Apply Failed",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                // Log failed audit
                if (_auditService != null)
                {
                    try
                    {
                        await _auditService.LogAsync(
                            userId: 1,
                            action: "ApplyYAML",
                            resourceKind: "YAML",
                            resourceName: "configuration",
                            resourceNamespace: "multiple",
                            success: false,
                            errorMessage: ex.Message,
                            details: null);
                    }
                    catch
                    {
                        // Ignore audit logging errors
                    }
                }
            }
            finally
            {
                _btnApply.Enabled = true;
            }
        }
    }
}
