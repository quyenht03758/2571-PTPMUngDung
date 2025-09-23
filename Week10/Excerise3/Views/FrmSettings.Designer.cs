using System.Drawing;
using System.Windows.Forms;

namespace Exercise3.Views
{
    partial class FrmSettings
    {
        private System.ComponentModel.IContainer components = null;
        private Label lblTitle;
        private FlowLayoutPanel flpDifficulties;
        private Label lblCurrent;
        private Button btnClose;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();

            this.lblTitle = new Label();
            this.flpDifficulties = new FlowLayoutPanel();
            this.lblCurrent = new Label();
            this.btnClose = new Button();

            // Form
            this.Text = "Settings • Difficulty";
            this.StartPosition = FormStartPosition.CenterParent;
            this.ClientSize = new Size(920, 260);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Load += new System.EventHandler(this.FrmSettings_Load);

            // lblTitle
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new Font("Segoe UI", 14f, FontStyle.Bold);
            this.lblTitle.Location = new Point(18, 16);
            this.lblTitle.Text = "Difficulty";

            // flpDifficulties
            this.flpDifficulties.Location = new Point(20, 56);
            this.flpDifficulties.Size = new Size(880, 120);
            this.flpDifficulties.AutoScroll = true;
            this.flpDifficulties.WrapContents = true;
            this.flpDifficulties.FlowDirection = FlowDirection.LeftToRight;

            // lblCurrent
            this.lblCurrent.AutoSize = true;
            this.lblCurrent.Font = new Font("Segoe UI", 10.5f, FontStyle.Regular);
            this.lblCurrent.Location = new Point(20, 190);
            this.lblCurrent.Text = "Current: -";

            // btnClose
            this.btnClose.Text = "Close";
            this.btnClose.Size = new Size(100, 36);
            this.btnClose.Location = new Point(800, 196);
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);

            // Add controls
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.flpDifficulties);
            this.Controls.Add(this.lblCurrent);
            this.Controls.Add(this.btnClose);
        }
    }
}
