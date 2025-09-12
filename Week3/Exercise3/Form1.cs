using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Exercise3
{
    public partial class Form1 : Form
    {
        // data: Category -> List of (name, image)
        private Dictionary<string, List<(string name, Image img)>> data =
            new Dictionary<string, List<(string, Image)>>();

        public Form1()
        {
            InitializeComponent();
            WireEvents();
            InitializeData();     // <- nạp dữ liệu
            ApplyView(View.LargeIcon);
        }

        private void WireEvents()
        {
            listBoxCategories.SelectedIndexChanged += listBoxCategories_SelectedIndexChanged;

            radioList.CheckedChanged += radioList_CheckedChanged;
            radioTile.CheckedChanged += radioTile_CheckedChanged;
            radioLarge.CheckedChanged += radioLarge_CheckedChanged;
            comboView.SelectedIndexChanged += comboView_SelectedIndexChanged;
        }

        // ---------- LOAD DATA ----------
        private void InitializeData()
        {
            // 1) Tạo danh mục bên trái
            listBoxCategories.Items.Clear();
            listBoxCategories.Items.Add("View All");
            listBoxCategories.Items.Add("Vegetables");
            listBoxCategories.Items.Add("Fruits");
            listBoxCategories.Items.Add("Food and drinks");
            listBoxCategories.SelectedIndex = 0;

            // 2) Chuẩn bị ảnh (từ thư mục images/; nếu không có -> tạo placeholder)
            string imgDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "images");
            Directory.CreateDirectory(imgDir);
            var imgVegetable = LoadOrPlaceholder(Path.Combine(imgDir, "cabbage.png"), Color.SeaGreen, "veg");
            var imgFruit = LoadOrPlaceholder(Path.Combine(imgDir, "eggplant.png"), Color.MediumPurple, "fruit");
            var imgFood = LoadOrPlaceholder(Path.Combine(imgDir, "strawberry.png"), Color.IndianRed, "food");

            // 3) Tạo dữ liệu mẫu
            data.Clear();
            data["Vegetables"] = new List<(string, Image)>
            {
                ("vegetable 1", imgVegetable),
                ("vegetable 2", imgVegetable),
                ("vegetable 3", imgVegetable),
            };

            data["Fruits"] = new List<(string, Image)>
            {
                ("fruit 1", imgFruit),
                ("fruit 2", imgFruit),
                ("fruit 3", imgFruit),
            };

            data["Food and drinks"] = new List<(string, Image)>
            {
                ("food 1", imgFood),
                ("food 2", imgFood),
            };

            // 4) Hiển thị ban đầu
            ShowItems("View All");
        }

        private Image LoadOrPlaceholder(string path, Color color, string text)
        {
            try
            {
                if (File.Exists(path))
                    return Image.FromFile(path);
            }
            catch { /* ignore and fallback to placeholder */ }

            // placeholder 64x64
            var bmp = new Bitmap(64, 64);
            using (var g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.White);
                using (var br = new SolidBrush(color))
                    g.FillEllipse(br, 2, 2, 60, 60);
                using (var pen = new Pen(Color.FromArgb(60, 0, 0, 0)))
                    g.DrawEllipse(pen, 2, 2, 60, 60);
                using (var f = new Font("Segoe UI", 8, FontStyle.Bold))
                using (var br2 = new SolidBrush(Color.White))
                using (var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
                    g.DrawString(text, f, br2, new RectangleF(0, 0, 64, 64), sf);
            }
            return bmp;
        }

        // ---------- RENDER ----------
        private void ShowItems(string category)
        {
            listView1.BeginUpdate();

            listView1.Items.Clear();
            listView1.Groups.Clear();
            imageListLarge.Images.Clear();
            imageListSmall.Images.Clear();

            listView1.LargeImageList = imageListLarge;
            listView1.SmallImageList = imageListSmall;

            int imgIndex = 0;

            foreach (var kv in data)
            {
                if (category != "View All" && kv.Key != category) continue;

                var group = new ListViewGroup(kv.Key, kv.Key);
                listView1.Groups.Add(group);

                foreach (var (name, img) in kv.Value)
                {
                    imageListLarge.Images.Add(img);
                    imageListSmall.Images.Add(img);

                    var item = new ListViewItem(name, imgIndex) { Group = group };
                    listView1.Items.Add(item);
                    imgIndex++;
                }
            }

            listView1.EndUpdate();
        }

        // ---------- EVENTS ----------
        private void listBoxCategories_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxCategories.SelectedItem == null) return;
            var selected = listBoxCategories.SelectedItem.ToString();
            ShowItems(selected);
        }

        private void radioList_CheckedChanged(object sender, EventArgs e)
        {
            if (radioList.Checked) ApplyView(View.List);
        }

        private void radioTile_CheckedChanged(object sender, EventArgs e)
        {
            if (radioTile.Checked) ApplyView(View.Tile);
        }

        private void radioLarge_CheckedChanged(object sender, EventArgs e)
        {
            if (radioLarge.Checked) ApplyView(View.LargeIcon);
        }

        private void comboView_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboView.SelectedItem?.ToString())
            {
                case "LargeIcon": ApplyView(View.LargeIcon); break;
                case "SmallIcon": ApplyView(View.SmallIcon); break;
                case "Tile": ApplyView(View.Tile); break;
                case "List": ApplyView(View.List); break;
                case "Details": ApplyView(View.Details); break;
            }
        }

        private void ApplyView(View view)
        {
            listView1.View = view;
            // chỉnh kích thước icon lớn/nhỏ cho đẹp
            if (view == View.LargeIcon || view == View.Tile)
                imageListLarge.ImageSize = new Size(64, 64);
            else if (view == View.SmallIcon || view == View.List || view == View.Details)
                imageListSmall.ImageSize = new Size(32, 32);
        }
        // Add this method to your Form1 class to fix CS1061
        private void listView1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            // You can add your logic here if needed, or leave it empty if not used.
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
