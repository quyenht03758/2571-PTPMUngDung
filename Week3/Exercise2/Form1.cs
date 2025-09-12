using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace Exercise2
{
    public partial class Form1 : Form
    {
        private Image _currentImage;
        public Form1()
        {
            InitializeComponent();

            // ---- Event wiring (Designer-safe; never inside InitializeComponent) ----
            btnBrowse.Click += BtnBrowse_Click;
            btnLoadFromPath.Click += BtnLoadFromPath_Click;
            btnSaveAs.Click += BtnSaveAs_Click;
            btnClear.Click += BtnClear_Click;

            picPreview.AllowDrop = true;
            picPreview.DragEnter += PicPreview_DragEnter;
            picPreview.DragDrop += PicPreview_DragDrop;

            KeyPreview = true;
            KeyDown += ImageForm_KeyDown;
        }

        // ---------- UI Events ----------
        private void BtnBrowse_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog
            {
                Title = "Select an image",
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif;*.tif;*.tiff"
            })
            {
                if (ofd.ShowDialog(this) == DialogResult.OK)
                {
                    txtPath.Text = ofd.FileName;
                    LoadImageSafe(ofd.FileName);
                }
            }
        }

        private void BtnLoadFromPath_Click(object sender, EventArgs e)
        {
            var path = txtPath.Text.Trim();
            if (string.IsNullOrEmpty(path))
            {
                MessageBox.Show("Please enter an image file path.", "Load",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            LoadImageSafe(path);
        }

        private void BtnSaveAs_Click(object sender, EventArgs e)
        {
            if (picPreview.Image == null)
            {
                MessageBox.Show("No image to save.", "Save",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (var sfd = new SaveFileDialog
            {
                Title = "Save image as",
                Filter = "PNG Image|*.png|JPEG Image|*.jpg|Bitmap Image|*.bmp|TIFF Image|*.tif",
                FileName = "output"
            })
            {
                if (sfd.ShowDialog(this) == DialogResult.OK)
                {
                    try
                    {
                        var fmt = GetImageFormatByExtension(Path.GetExtension(sfd.FileName));
                        picPreview.Image.Save(sfd.FileName, fmt);
                        MessageBox.Show("Saved successfully!", "Save",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Failed to save image:\n" + ex.Message, "Save Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            SetImage(null);
            txtPath.Clear();
            lblHint.Text = "Drop an image here, or use Browse / Load from Path.";
        }

        private void PicPreview_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.None;
            if (e.Data?.GetDataPresent(DataFormats.FileDrop) == true)
            {
                var files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files != null && files.Length > 0 && IsSupportedImage(files[0]))
                    e.Effect = DragDropEffects.Copy;
            }
        }

        private void PicPreview_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data?.GetDataPresent(DataFormats.FileDrop) != true) return;
            var files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files == null || files.Length == 0) return;

            var path = files[0];
            if (IsSupportedImage(path))
            {
                txtPath.Text = path;
                LoadImageSafe(path);
            }
        }

        private void ImageForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.O) BtnBrowse_Click(sender, EventArgs.Empty);
            else if (e.Control && e.KeyCode == Keys.S) BtnSaveAs_Click(sender, EventArgs.Empty);
            else if (e.KeyCode == Keys.Delete) BtnClear_Click(sender, EventArgs.Empty);
        }

        // ---------- Helpers ----------
        private void LoadImageSafe(string path)
        {
            try
            {
                if (!File.Exists(path))
                    throw new FileNotFoundException("File not found.", path);

                // Load into memory (avoid locking the source file)
                using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                using (var ms = new MemoryStream())
                {
                    fs.CopyTo(ms);
                    ms.Position = 0;
                    using (var temp = Image.FromStream(ms))
                    {
                        SetImage(new Bitmap(temp)); // clone to detach from stream
                    }
                }

                UpdateHintWithInfo(path);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load image:\n" + ex.Message, "Open Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetImage(Image img)
        {
            if (_currentImage != null)
            {
                picPreview.Image = null;
                _currentImage.Dispose();
                _currentImage = null;
            }
            _currentImage = img;
            picPreview.Image = _currentImage;
        }

        private void UpdateHintWithInfo(string path)
        {
            if (_currentImage == null) { lblHint.Text = ""; return; }
            var sizeBytes = new FileInfo(path).Length;
            lblHint.Text = $"Path: {path}   |   {_currentImage.Width}×{_currentImage.Height}px   |   {FormatBytes(sizeBytes)}";
        }

        private static bool IsSupportedImage(string path)
        {
            var ext = (Path.GetExtension(path) ?? "").ToLowerInvariant();
            return ext == ".jpg" || ext == ".jpeg" || ext == ".png" ||
                   ext == ".bmp" || ext == ".gif" || ext == ".tif" || ext == ".tiff";
        }

        private static ImageFormat GetImageFormatByExtension(string ext)
        {
            switch ((ext ?? "").ToLowerInvariant())
            {
                case ".jpg":
                case ".jpeg": return ImageFormat.Jpeg;
                case ".bmp": return ImageFormat.Bmp;
                case ".tif":
                case ".tiff": return ImageFormat.Tiff;
                default: return ImageFormat.Png;
            }
        }

        private static string FormatBytes(long bytes)
        {
            string[] units = { "B", "KB", "MB", "GB" };
            double val = bytes;
            int i = 0;
            while (val >= 1024 && i < units.Length - 1) { val /= 1024; i++; }
            return $"{val:0.##} {units[i]}";
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            SetImage(null); // dispose if any
            base.OnFormClosed(e);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
