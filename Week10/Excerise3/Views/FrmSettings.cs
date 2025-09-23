using Exercise3.Controllers;
using Exercise3.Models;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Exercise3.Views
{
    public partial class FrmSettings : Form
    {
        private readonly SettingsController _controller;
        private GameSettings _settings;

        // Use ValueTuple explicitly for C# 7.3
        private readonly System.ValueTuple<int, int>[] _presets = new System.ValueTuple<int, int>[]
        {
            new System.ValueTuple<int,int>(4,3),
            new System.ValueTuple<int,int>(4,4),
            new System.ValueTuple<int,int>(5,4),
            new System.ValueTuple<int,int>(6,5),
            new System.ValueTuple<int,int>(6,6),
            new System.ValueTuple<int,int>(7,6),
            new System.ValueTuple<int,int>(8,6),
            new System.ValueTuple<int,int>(9,6),
            new System.ValueTuple<int,int>(10,7),
            new System.ValueTuple<int,int>(10,8),
            new System.ValueTuple<int,int>(10,9),
            new System.ValueTuple<int,int>(12,10),
            new System.ValueTuple<int,int>(14,10)
        };

        private readonly Color _selectedBack = Color.MediumPurple;
        private readonly Color _selectedFore = Color.White;
        private readonly Color _normalBack = Color.White;
        private readonly Color _normalFore = Color.MediumPurple;

        public FrmSettings(SettingsController controller)
        {
            if (controller == null) throw new ArgumentNullException("controller");
            _controller = controller;
            InitializeComponent();
        }

        private void FrmSettings_Load(object sender, EventArgs e)
        {
            _settings = _controller.GetCurrent();
            CreateDifficultyButtons();
            HighlightCurrent();
            UpdateCurrentLabel();
        }

        private void CreateDifficultyButtons()
        {
            flpDifficulties.Controls.Clear();

            foreach (var t in _presets)
            {
                int r = t.Item1;
                int c = t.Item2;

                var btn = new Button
                {
                    AutoSize = false,
                    Width = 120,
                    Height = 48,
                    Margin = new Padding(10),
                    FlatStyle = FlatStyle.Flat,
                    Text = r + "x" + c,
                    Tag = new System.ValueTuple<int, int>(r, c),
                    BackColor = _normalBack,
                    ForeColor = _normalFore,
                    Font = new Font("Segoe UI", 12f, FontStyle.Bold)
                };
                btn.FlatAppearance.BorderSize = 0;
                btn.Click += Difficulty_Click;
                flpDifficulties.Controls.Add(btn);
            }
        }

        private void Difficulty_Click(object sender, EventArgs e)
        {
            var b = sender as Button;
            if (b == null) return;

            var t = (System.ValueTuple<int, int>)b.Tag;
            int rows = t.Item1;
            int cols = t.Item2;

            try
            {
                _settings = _controller.SelectPreset(rows, cols);
                HighlightCurrent();
                UpdateCurrentLabel();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Invalid size", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void HighlightCurrent()
        {
            foreach (Control ctl in flpDifficulties.Controls)
            {
                var b = ctl as Button;
                if (b == null) continue;

                var t = (System.ValueTuple<int, int>)b.Tag;
                bool isSelected = (t.Item1 == _settings.Rows && t.Item2 == _settings.Cols);
                b.BackColor = isSelected ? _selectedBack : _normalBack;
                b.ForeColor = isSelected ? _selectedFore : _normalFore;
            }
        }

        private void UpdateCurrentLabel()
        {
            lblCurrent.Text = "Current: " + _settings.Rows + "x" + _settings.Cols + "  •  Pairs: " + _settings.Pairs;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
