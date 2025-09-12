using System;
using System.Drawing;
using System.Windows.Forms;

namespace Exercise1
{
    public class Form1 : Form
    {
        // Top-level layout
        private TableLayoutPanel root;
        private Panel card;

        // Card layout/content
        private TableLayoutPanel tl;
        private PictureBox picLogo;
        private Label lblHeader;

        private Label lblUser;
        private TextBox txtUser;
        private Label lblPass;
        private TextBox txtPass;

        private CheckBox chkShow;
        private CheckBox chkRemember;
        private Label lblCaps;

        private FlowLayoutPanel pnlButtons;
        private Button btnLogin;
        private Button btnCancel;

        private ErrorProvider errorProvider;
        private System.ComponentModel.IContainer components;
        private FlowLayoutPanel optionsPanel;
        private ToolTip tip;

        public Form1()
        {
            InitializeComponent();

            // ===== All event hookups & runtime logic go here (Designer-safe) =====
            this.KeyPreview = true;
            this.KeyDown += Form1_KeyDown;
            this.Shown += (s, e) => UpdateCapsLock();

            card.Paint += Card_PaintBorder;
            txtPass.KeyUp += (s, e) => UpdateCapsLock();
            chkShow.CheckedChanged += (s, e) => txtPass.UseSystemPasswordChar = !chkShow.Checked;
            btnLogin.Click += BtnLogin_Click;
            btnCancel.Click += (s, e) => this.Close();

            this.AcceptButton = btnLogin;
            this.CancelButton = btnCancel;

            errorProvider.ContainerControl = this; // required for ErrorProvider
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.tip = new System.Windows.Forms.ToolTip(this.components);
            this.txtUser = new System.Windows.Forms.TextBox();
            this.txtPass = new System.Windows.Forms.TextBox();
            this.chkRemember = new System.Windows.Forms.CheckBox();
            this.root = new System.Windows.Forms.TableLayoutPanel();
            this.card = new System.Windows.Forms.Panel();
            this.tl = new System.Windows.Forms.TableLayoutPanel();
            this.picLogo = new System.Windows.Forms.PictureBox();
            this.lblHeader = new System.Windows.Forms.Label();
            this.lblUser = new System.Windows.Forms.Label();
            this.lblPass = new System.Windows.Forms.Label();
            this.optionsPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.chkShow = new System.Windows.Forms.CheckBox();
            this.lblCaps = new System.Windows.Forms.Label();
            this.pnlButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.btnLogin = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.root.SuspendLayout();
            this.card.SuspendLayout();
            this.tl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).BeginInit();
            this.optionsPanel.SuspendLayout();
            this.pnlButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // errorProvider
            // 
            this.errorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this.errorProvider.ContainerControl = this;
            // 
            // tip
            // 
            this.tip.AutoPopDelay = 5000;
            this.tip.InitialDelay = 200;
            this.tip.IsBalloon = true;
            this.tip.ReshowDelay = 150;
            // 
            // txtUser
            // 
            this.tl.SetColumnSpan(this.txtUser, 2);
            this.txtUser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtUser.Location = new System.Drawing.Point(11, 108);
            this.txtUser.Name = "txtUser";
            this.txtUser.Size = new System.Drawing.Size(572, 30);
            this.txtUser.TabIndex = 3;
            this.tip.SetToolTip(this.txtUser, "Enter your username (e.g., admin)");
            // 
            // txtPass
            // 
            this.tl.SetColumnSpan(this.txtPass, 2);
            this.txtPass.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtPass.Location = new System.Drawing.Point(11, 177);
            this.txtPass.Name = "txtPass";
            this.txtPass.Size = new System.Drawing.Size(572, 30);
            this.txtPass.TabIndex = 5;
            this.tip.SetToolTip(this.txtPass, "Enter your password (e.g., admin)");
            this.txtPass.UseSystemPasswordChar = true;
            // 
            // chkRemember
            // 
            this.chkRemember.AutoSize = true;
            this.chkRemember.Location = new System.Drawing.Point(158, 3);
            this.chkRemember.Name = "chkRemember";
            this.chkRemember.Size = new System.Drawing.Size(144, 27);
            this.chkRemember.TabIndex = 1;
            this.chkRemember.Text = "Remember me";
            this.tip.SetToolTip(this.chkRemember, "Sample UI only (no persistence in this demo).");
            // 
            // root
            // 
            this.root.BackColor = System.Drawing.Color.Transparent;
            this.root.ColumnCount = 3;
            this.root.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.root.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.root.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.root.Controls.Add(this.card, 1, 1);
            this.root.Dock = System.Windows.Forms.DockStyle.Fill;
            this.root.Location = new System.Drawing.Point(0, 0);
            this.root.Name = "root";
            this.root.RowCount = 3;
            this.root.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.root.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.root.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.root.Size = new System.Drawing.Size(1068, 540);
            this.root.TabIndex = 0;
            // 
            // card
            // 
            this.card.BackColor = System.Drawing.Color.White;
            this.card.Controls.Add(this.tl);
            this.card.Dock = System.Windows.Forms.DockStyle.Fill;
            this.card.Location = new System.Drawing.Point(216, 84);
            this.card.Name = "card";
            this.card.Padding = new System.Windows.Forms.Padding(20);
            this.card.Size = new System.Drawing.Size(634, 372);
            this.card.TabIndex = 0;
            // 
            // tl
            // 
            this.tl.ColumnCount = 2;
            this.tl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 64F));
            this.tl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tl.Controls.Add(this.picLogo, 0, 0);
            this.tl.Controls.Add(this.lblUser, 0, 2);
            this.tl.Controls.Add(this.txtUser, 0, 3);
            this.tl.Controls.Add(this.lblPass, 0, 5);
            this.tl.Controls.Add(this.txtPass, 0, 6);
            this.tl.Controls.Add(this.optionsPanel, 0, 7);
            this.tl.Controls.Add(this.lblCaps, 0, 8);
            this.tl.Controls.Add(this.lblHeader, 1, 0);
            this.tl.Controls.Add(this.pnlButtons, 0, 9);
            this.tl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tl.Location = new System.Drawing.Point(20, 20);
            this.tl.Name = "tl";
            this.tl.Padding = new System.Windows.Forms.Padding(8);
            this.tl.RowCount = 11;
            this.tl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 64F));
            this.tl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tl.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tl.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tl.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tl.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tl.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tl.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tl.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tl.Size = new System.Drawing.Size(594, 332);
            this.tl.TabIndex = 0;
            // 
            // picLogo
            // 
            this.picLogo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(244)))), ((int)(((byte)(246)))));
            this.picLogo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picLogo.Location = new System.Drawing.Point(11, 11);
            this.picLogo.Name = "picLogo";
            this.picLogo.Size = new System.Drawing.Size(58, 58);
            this.picLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picLogo.TabIndex = 0;
            this.picLogo.TabStop = false;
            // 
            // lblHeader
            // 
            this.lblHeader.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblHeader.AutoSize = true;
            this.lblHeader.Font = new System.Drawing.Font("Segoe UI Semibold", 16F, System.Drawing.FontStyle.Bold);
            this.lblHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.lblHeader.Location = new System.Drawing.Point(75, 35);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(194, 37);
            this.lblHeader.TabIndex = 1;
            this.lblHeader.Text = "Welcome back";
            // 
            // lblUser
            // 
            this.lblUser.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblUser.AutoSize = true;
            this.tl.SetColumnSpan(this.lblUser, 2);
            this.lblUser.Location = new System.Drawing.Point(11, 82);
            this.lblUser.Name = "lblUser";
            this.lblUser.Size = new System.Drawing.Size(87, 23);
            this.lblUser.TabIndex = 2;
            this.lblUser.Text = "Username";
            // 
            // lblPass
            // 
            this.lblPass.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblPass.AutoSize = true;
            this.tl.SetColumnSpan(this.lblPass, 2);
            this.lblPass.Location = new System.Drawing.Point(11, 151);
            this.lblPass.Name = "lblPass";
            this.lblPass.Size = new System.Drawing.Size(80, 23);
            this.lblPass.TabIndex = 4;
            this.lblPass.Text = "Password";
            // 
            // optionsPanel
            // 
            this.optionsPanel.AutoSize = true;
            this.tl.SetColumnSpan(this.optionsPanel, 2);
            this.optionsPanel.Controls.Add(this.chkShow);
            this.optionsPanel.Controls.Add(this.chkRemember);
            this.optionsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.optionsPanel.Location = new System.Drawing.Point(11, 213);
            this.optionsPanel.Name = "optionsPanel";
            this.optionsPanel.Size = new System.Drawing.Size(572, 33);
            this.optionsPanel.TabIndex = 6;
            this.optionsPanel.WrapContents = false;
            // 
            // chkShow
            // 
            this.chkShow.AutoSize = true;
            this.chkShow.Location = new System.Drawing.Point(3, 3);
            this.chkShow.Name = "chkShow";
            this.chkShow.Size = new System.Drawing.Size(149, 27);
            this.chkShow.TabIndex = 0;
            this.chkShow.Text = "Show password";
            // 
            // lblCaps
            // 
            this.lblCaps.AutoSize = true;
            this.tl.SetColumnSpan(this.lblCaps, 2);
            this.lblCaps.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCaps.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(185)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
            this.lblCaps.Location = new System.Drawing.Point(11, 249);
            this.lblCaps.Name = "lblCaps";
            this.lblCaps.Size = new System.Drawing.Size(572, 23);
            this.lblCaps.TabIndex = 7;
            // 
            // pnlButtons
            // 
            this.pnlButtons.AutoSize = true;
            this.tl.SetColumnSpan(this.pnlButtons, 2);
            this.pnlButtons.Controls.Add(this.btnLogin);
            this.pnlButtons.Controls.Add(this.btnCancel);
            this.pnlButtons.Location = new System.Drawing.Point(11, 275);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Size = new System.Drawing.Size(155, 39);
            this.pnlButtons.TabIndex = 8;
            this.pnlButtons.WrapContents = false;
            // 
            // btnLogin
            // 
            this.btnLogin.AutoSize = true;
            this.btnLogin.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(99)))), ((int)(((byte)(235)))));
            this.btnLogin.FlatAppearance.BorderSize = 0;
            this.btnLogin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogin.ForeColor = System.Drawing.Color.White;
            this.btnLogin.Location = new System.Drawing.Point(3, 3);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(72, 33);
            this.btnLogin.TabIndex = 0;
            this.btnLogin.Text = "Sign in";
            this.btnLogin.UseVisualStyleBackColor = false;
            // 
            // btnCancel
            // 
            this.btnCancel.AutoSize = true;
            this.btnCancel.Location = new System.Drawing.Point(81, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(71, 33);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            // 
            // Form1
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ClientSize = new System.Drawing.Size(1068, 540);
            this.Controls.Add(this.root);
            this.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(560, 420);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Sign in";
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.root.ResumeLayout(false);
            this.card.ResumeLayout(false);
            this.tl.ResumeLayout(false);
            this.tl.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).EndInit();
            this.optionsPanel.ResumeLayout(false);
            this.optionsPanel.PerformLayout();
            this.pnlButtons.ResumeLayout(false);
            this.pnlButtons.PerformLayout();
            this.ResumeLayout(false);

        }

        // ===== Event handlers (all outside InitializeComponent) =====
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) this.Close();
        }

        private void Card_PaintBorder(object sender, PaintEventArgs e)
        {
            using (var pen = new Pen(Color.FromArgb(225, 229, 234)))
            {
                e.Graphics.DrawRectangle(pen, 0, 0, card.Width - 1, card.Height - 1);
            }
        }

        private void UpdateCapsLock()
        {
            lblCaps.Text = Control.IsKeyLocked(Keys.CapsLock) ? "Caps Lock is ON" : string.Empty;
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            // Basic validation
            errorProvider.SetError(txtUser, string.IsNullOrWhiteSpace(txtUser.Text) ? "Username is required" : "");
            errorProvider.SetError(txtPass, string.IsNullOrWhiteSpace(txtPass.Text) ? "Password is required" : "");
            if (!string.IsNullOrEmpty(errorProvider.GetError(txtUser)) ||
                !string.IsNullOrEmpty(errorProvider.GetError(txtPass)))
                return;

            var user = txtUser.Text.Trim();
            var pass = txtPass.Text;

            if (user == "admin" && pass == "admin")
            {
                MessageBox.Show("Welcome to admin", "Login",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Sorry, your username or password is wrong!", "Login",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
