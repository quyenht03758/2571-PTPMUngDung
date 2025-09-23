using Exercise3.Controllers;
using Exercise3.Data;
using Exercise3.Images;
using Exercise3.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Exercise3.Views
{
    public partial class FrmGameBoard : Form
    {
        private SettingsController _settingsCtl;
        private GameController _game;

        private IImageProvider _imageProvider;
        private readonly Timer _clock = new Timer() { Interval = 1000 };

        private Image _imgBack;
        private readonly Dictionary<string, Image> _cache = new Dictionary<string, Image>();
        private MemoryCard _first;
        private bool _busy;

        public FrmGameBoard()
        {
            InitializeComponent();

            _settingsCtl = new SettingsController(new SettingsRepository());
            var settings = _settingsCtl.GetCurrent();

            var imagesRoot = Path.Combine(Application.StartupPath, "images");
            _imageProvider = new FolderImageProvider(imagesRoot);

            _game = new GameController(_imageProvider, new ScoreRepository(), settings);

            var backPath = _imageProvider.GetBackImagePath();
            if (!File.Exists(backPath)) throw new FileNotFoundException("Back image not found", backPath);
            _imgBack = Image.FromFile(backPath);

            _clock.Tick += (s, e) =>
            {
                _game.Tick();
                lblTimeValue.Text = TimeSpan.FromSeconds(_game.ElapsedSec).ToString(@"hh\:mm\:ss");
            };
        }

        private void FrmGameBoard_Load(object sender, EventArgs e)
        {
            lblScoreValue.Text = "0";
            lblLevelValue.Text = _game.Settings.Rows + "x" + _game.Settings.Cols;
            lblTimeValue.Text = "00:00:00";

            LoadTopScores();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            StartGame();
        }

        private void StartGame()
        {
            _first = null; _busy = false;
            pnlBoard.Controls.Clear();
            _clock.Stop();
            lblTimeValue.Text = "00:00:00";

            var deck = _game.StartNew();
            lblScoreValue.Text = "0";
            lblLevelValue.Text = _game.Settings.Rows + "x" + _game.Settings.Cols;
            _clock.Start();

            int rows = _game.Settings.Rows, cols = _game.Settings.Cols;
            int size = 100, gap = 8, off = 15;

            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                {
                    int idx = i * cols + j;
                    var card = deck[idx];
                    var b = new Button
                    {
                        Tag = card,
                        Size = new Size(size, size),
                        Location = new Point(off + j * (size + gap), off + i * (size + gap)),
                        FlatStyle = FlatStyle.Standard,
                        BackgroundImageLayout = ImageLayout.Stretch
                    };
                    PaintBack(b);
                    b.Click += async delegate { await OnCardClick(b); };
                    pnlBoard.Controls.Add(b);
                }
        }

        private async Task OnCardClick(Button b)
        {
            if (_busy) return;
            var card = (MemoryCard)b.Tag;
            if (card.IsMatched || card.IsFlipped) return;

            await Flip(b, card, true);
            if (_first == null) { _first = card; return; }

            _busy = true;
            bool ok = _game.TryMatch(_first, card);
            lblScoreValue.Text = _game.Score.ToString();

            if (ok)
            {
                await Flash(b);
                var a = FindButtonByCard(_first);
                if (a != null) await Flash(a);
            }
            else
            {
                await Task.Delay(_game.Settings.MismatchDelayMs);
                await Flip(b, card, false);
                var a = FindButtonByCard(_first);
                if (a != null) await Flip(a, _first, false);
            }

            _first = null;
            _busy = false;

            if (AllMatched())
            {
                _clock.Stop();
                var player = PromptPlayerName();
                _game.SaveScore(player);
                LoadTopScores();
                MessageBox.Show("🎉 Completed! Score saved.", "Memory Game");
            }
        }

        private Button FindButtonByCard(MemoryCard c)
        {
            foreach (Control ctl in pnlBoard.Controls)
            {
                var b = ctl as Button;
                if (b != null && object.ReferenceEquals(b.Tag, c)) return b;
            }
            return null;
        }

        private void PaintBack(Button b) { b.BackgroundImage = _imgBack; b.Text = ""; }

        private void PaintFront(Button b, MemoryCard c)
        {
            Image img;
            if (!_cache.TryGetValue(c.ImagePath, out img))
            {
                img = Image.FromFile(c.ImagePath);
                _cache[c.ImagePath] = img;
            }
            b.BackgroundImage = img;
            b.Text = "";
        }

        private async Task Flip(Button b, MemoryCard c, bool front)
        {
            int steps = 5, delay = Math.Max(20, _game.Settings.FlipMs / (steps * 2));
            for (int i = 0; i < steps; i++) { b.Width -= 6; await Task.Delay(delay); }
            if (front) { PaintFront(b, c); c.IsFlipped = true; } else { PaintBack(b); c.IsFlipped = false; }
            for (int i = 0; i < steps; i++) { b.Width += 6; await Task.Delay(delay); }
        }

        private async Task Flash(Button b)
        {
            var old = b.BackColor;
            for (int i = 0; i < 2; i++)
            {
                b.BackColor = Color.LightGoldenrodYellow; await Task.Delay(90);
                b.BackColor = old; await Task.Delay(90);
            }
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            using (var f = new FrmSettings(_settingsCtl))
            {
                if (f.ShowDialog(this) == DialogResult.OK)
                {
                    var s = _settingsCtl.GetCurrent();
                    _game.ApplySettings(s);
                    lblLevelValue.Text = s.Rows + "x" + s.Cols;
                }
            }
        }

        private void btnTopScores_Click(object sender, EventArgs e)
        {
            LoadTopScores();
        }

        private void LoadTopScores()
        {
            lvTopScores.Items.Clear();

            using (var cn = SqlHelper.Open())
            using (var cmd = new System.Data.SqlClient.SqlCommand(
                "SELECT TOP 10 Player, Score, DurationSec, PlayedAt " +
                "FROM dbo.Scores ORDER BY Score DESC, PlayedAt ASC", cn))
            using (var rd = cmd.ExecuteReader())
            {
                while (rd.Read())
                {
                    var item = new ListViewItem(rd["Player"].ToString());
                    item.SubItems.Add(rd["Score"].ToString());
                    item.SubItems.Add(rd["DurationSec"].ToString() + "s");
                    lvTopScores.Items.Add(item);
                }
            }
        }

        // helper: kiểm tra tất cả matched
        private bool AllMatched()
        {
            foreach (Control ctl in pnlBoard.Controls)
            {
                var btn = ctl as Button;
                if (btn == null) continue;
                var c = btn.Tag as MemoryCard;
                if (c != null && !c.IsMatched) return false;
            }
            return true;
        }

        // helper: popup nhập tên
        private string PromptPlayerName()
        {
            using (var dlg = new Form())
            {
                dlg.Text = "Your Name";
                var tb = new TextBox() { Left = 10, Top = 10, Width = 200, Text = "Player" };
                var ok = new Button() { Left = 220, Top = 8, Width = 60, Text = "OK", DialogResult = DialogResult.OK };
                dlg.AcceptButton = ok;
                dlg.ClientSize = new Size(300, 45);
                dlg.Controls.Add(tb);
                dlg.Controls.Add(ok);
                return dlg.ShowDialog(this) == DialogResult.OK ? tb.Text : "Player";
            }
        }
    }
}
