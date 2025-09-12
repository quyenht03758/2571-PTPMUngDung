using System.Drawing;
using System.Windows.Forms;

namespace Exercise3
{
    partial class Form1
    {
    
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.ListBox listBoxCategories;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ImageList imageListLarge;
        private System.Windows.Forms.ImageList imageListSmall;
        private System.Windows.Forms.GroupBox groupBoxView;
        private System.Windows.Forms.RadioButton radioList;
        private System.Windows.Forms.RadioButton radioTile;
        private System.Windows.Forms.RadioButton radioLarge;
        private System.Windows.Forms.ComboBox comboView;
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }


        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.listBoxCategories = new System.Windows.Forms.ListBox();
            this.listView1 = new System.Windows.Forms.ListView();
            this.imageListLarge = new System.Windows.Forms.ImageList(this.components);
            this.imageListSmall = new System.Windows.Forms.ImageList(this.components);
            this.groupBoxView = new System.Windows.Forms.GroupBox();
            this.radioList = new System.Windows.Forms.RadioButton();
            this.radioTile = new System.Windows.Forms.RadioButton();
            this.radioLarge = new System.Windows.Forms.RadioButton();
            this.comboView = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.groupBoxView.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(0, 0);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.listBoxCategories);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.listView1);
            this.splitContainer.Panel2.Controls.Add(this.groupBoxView);
            this.splitContainer.Size = new System.Drawing.Size(1000, 600);
            this.splitContainer.SplitterDistance = 180;
            this.splitContainer.TabIndex = 0;
            // 
            // listBoxCategories
            // 
            this.listBoxCategories.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxCategories.FormattingEnabled = true;
            this.listBoxCategories.ItemHeight = 16;
            this.listBoxCategories.Location = new System.Drawing.Point(0, 0);
            this.listBoxCategories.Name = "listBoxCategories";
            this.listBoxCategories.Size = new System.Drawing.Size(180, 600);
            this.listBoxCategories.TabIndex = 0;
            this.listBoxCategories.SelectedIndexChanged += new System.EventHandler(this.listBoxCategories_SelectedIndexChanged);
            // 
            // listView1
            // 
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.HideSelection = false;
            this.listView1.LargeImageList = this.imageListLarge;
            this.listView1.Location = new System.Drawing.Point(0, 60);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(816, 540);
            this.listView1.SmallImageList = this.imageListSmall;
            this.listView1.TabIndex = 1;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged_1);
            // 
            // imageListLarge
            // 
            this.imageListLarge.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.imageListLarge.ImageSize = new System.Drawing.Size(64, 64);
            this.imageListLarge.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // imageListSmall
            // 
            this.imageListSmall.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.imageListSmall.ImageSize = new System.Drawing.Size(32, 32);
            this.imageListSmall.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // groupBoxView
            // 
            this.groupBoxView.Controls.Add(this.radioList);
            this.groupBoxView.Controls.Add(this.radioTile);
            this.groupBoxView.Controls.Add(this.radioLarge);
            this.groupBoxView.Controls.Add(this.comboView);
            this.groupBoxView.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxView.Location = new System.Drawing.Point(0, 0);
            this.groupBoxView.Name = "groupBoxView";
            this.groupBoxView.Size = new System.Drawing.Size(816, 60);
            this.groupBoxView.TabIndex = 2;
            this.groupBoxView.TabStop = false;
            this.groupBoxView.Text = "View Setting";
            // 
            // radioList
            // 
            this.radioList.AutoSize = true;
            this.radioList.Location = new System.Drawing.Point(10, 25);
            this.radioList.Name = "radioList";
            this.radioList.Size = new System.Drawing.Size(48, 20);
            this.radioList.TabIndex = 0;
            this.radioList.Text = "List";
            this.radioList.CheckedChanged += new System.EventHandler(this.radioList_CheckedChanged);
            // 
            // radioTile
            // 
            this.radioTile.AutoSize = true;
            this.radioTile.Location = new System.Drawing.Point(70, 25);
            this.radioTile.Name = "radioTile";
            this.radioTile.Size = new System.Drawing.Size(51, 20);
            this.radioTile.TabIndex = 1;
            this.radioTile.Text = "Tile";
            this.radioTile.CheckedChanged += new System.EventHandler(this.radioTile_CheckedChanged);
            // 
            // radioLarge
            // 
            this.radioLarge.AutoSize = true;
            this.radioLarge.Checked = true;
            this.radioLarge.Location = new System.Drawing.Point(130, 25);
            this.radioLarge.Name = "radioLarge";
            this.radioLarge.Size = new System.Drawing.Size(88, 20);
            this.radioLarge.TabIndex = 2;
            this.radioLarge.TabStop = true;
            this.radioLarge.Text = "LargeIcon";
            this.radioLarge.CheckedChanged += new System.EventHandler(this.radioLarge_CheckedChanged);
            // 
            // comboView
            // 
            this.comboView.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboView.Items.AddRange(new object[] {
            "LargeIcon",
            "SmallIcon",
            "Tile",
            "List",
            "Details"});
            this.comboView.Location = new System.Drawing.Point(220, 22);
            this.comboView.Name = "comboView";
            this.comboView.Size = new System.Drawing.Size(121, 24);
            this.comboView.TabIndex = 3;
            this.comboView.SelectedIndexChanged += new System.EventHandler(this.comboView_SelectedIndexChanged);
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(1000, 600);
            this.Controls.Add(this.splitContainer);
            this.Name = "Form1";
            this.Text = "ListBox & ListView Demo";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.groupBoxView.ResumeLayout(false);
            this.groupBoxView.PerformLayout();
            this.ResumeLayout(false);

        }
    }
}

