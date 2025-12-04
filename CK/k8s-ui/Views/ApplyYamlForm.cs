using System.Diagnostics;
using MaterialSkin.Controls;

namespace K8sManager.Views
{
    public class ApplyYamlForm : MaterialForm
    {
        private readonly RichTextBox _rt = new RichTextBox() { Dock = DockStyle.Fill, Font = new System.Drawing.Font("Consolas", 10f) };
        public ApplyYamlForm()
        {
            Text = "Apply YAML";
            Width = 800; Height = 600;

            var tool = new ToolStrip() { GripStyle = ToolStripGripStyle.Hidden };
            var open = new ToolStripButton("Open…", null, (s, e) => OpenFile());
            var save = new ToolStripButton("Save…", null, (s, e) => SaveFile());
            var apply = new ToolStripButton("Apply", null, (s, e) => Apply());
            tool.Items.Add(open); tool.Items.Add(save); tool.Items.Add(apply);

            Controls.Add(_rt);
            Controls.Add(tool);
            tool.Dock = DockStyle.Top;
        }

        private void OpenFile()
        {
            using var ofd = new OpenFileDialog() { Filter = "YAML|*.yml;*.yaml|All|*.*" };
            if (ofd.ShowDialog(this) == DialogResult.OK)
                _rt.Text = File.ReadAllText(ofd.FileName);
        }

        private void SaveFile()
        {
            using var sfd = new SaveFileDialog() { Filter = "YAML|*.yml;*.yaml", FileName = "resource.yaml" };
            if (sfd.ShowDialog(this) == DialogResult.OK)
                File.WriteAllText(sfd.FileName, _rt.Text);
        }

        private void Apply()
        {
            var temp = Path.GetTempFileName() + ".yaml";
            File.WriteAllText(temp, _rt.Text);
            var psi = new ProcessStartInfo("kubectl", $"apply -f \"{temp}\"")
            {
                UseShellExecute = false,
                CreateNoWindow = true,
            };
            Process.Start(psi);
        }
    }
}
