using MaterialSkin;
using System.ComponentModel;
using MaterialSkin.Controls;

namespace K8sManager.Views
{
    [DesignerCategory("Form")]
    public partial class ScaleDialog : MaterialForm
    {
        private NumericUpDown _num = null!;
        private MaterialButton _ok = null!;
        private MaterialButton _cancel = null!;
        private MaterialLabel _label = null!;
        private Panel _bottom = null!;

        public int Replicas => (int)_num.Value;

        public ScaleDialog(string name, int currentReplicas = 2)
        {
            InitializeComponent();
            
            _num.Value = currentReplicas;
            Text = $"Scale - {name}";
        }

        private void BtnOk_Click(object? sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void BtnCancel_Click(object? sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
