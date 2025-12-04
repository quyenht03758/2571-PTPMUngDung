namespace K8sManager.Views
{
    partial class SimpleLoginView
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.TextBox _usernameTextBox;
        private System.Windows.Forms.TextBox _passwordTextBox;
        private System.Windows.Forms.Button _loginButton;
        private System.Windows.Forms.Label _titleLabel;
        private System.Windows.Forms.Label _usernameLabel;
        private System.Windows.Forms.Label _passwordLabel;
        private System.Windows.Forms.Label _statusLabel;
        private System.Windows.Forms.CheckBox _rememberMeCheckBox;
        private System.Windows.Forms.PictureBox _logoPictureBox;
        private System.Windows.Forms.Panel _leftPanel;
        private System.Windows.Forms.Panel _rightPanel;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this._leftPanel = new System.Windows.Forms.Panel();
            this._logoPictureBox = new System.Windows.Forms.PictureBox();
            this._rightPanel = new System.Windows.Forms.Panel();
            this._titleLabel = new System.Windows.Forms.Label();
            this._usernameLabel = new System.Windows.Forms.Label();
            this._usernameTextBox = new System.Windows.Forms.TextBox();
            this._passwordLabel = new System.Windows.Forms.Label();
            this._passwordTextBox = new System.Windows.Forms.TextBox();
            this._rememberMeCheckBox = new System.Windows.Forms.CheckBox();
            this._loginButton = new System.Windows.Forms.Button();
            this._statusLabel = new System.Windows.Forms.Label();
            this._leftPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._logoPictureBox)).BeginInit();
            this._rightPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // _leftPanel
            // 
            this._leftPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(115)))), ((int)(((byte)(220)))));
            this._leftPanel.Controls.Add(this._logoPictureBox);
            this._leftPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this._leftPanel.Location = new System.Drawing.Point(0, 0);
            this._leftPanel.Name = "_leftPanel";
            this._leftPanel.Size = new System.Drawing.Size(300, 450);
            this._leftPanel.TabIndex = 0;
            // 
            // _logoPictureBox
            // 
            this._logoPictureBox.BackColor = System.Drawing.Color.Transparent;
            this._logoPictureBox.Location = new System.Drawing.Point(50, 125);
            this._logoPictureBox.Name = "_logoPictureBox";
            this._logoPictureBox.Size = new System.Drawing.Size(200, 200);
            this._logoPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this._logoPictureBox.TabIndex = 0;
            this._logoPictureBox.TabStop = false;
            // 
            // _rightPanel
            // 
            this._rightPanel.BackColor = System.Drawing.Color.White;
            this._rightPanel.Controls.Add(this._titleLabel);
            this._rightPanel.Controls.Add(this._usernameLabel);
            this._rightPanel.Controls.Add(this._usernameTextBox);
            this._rightPanel.Controls.Add(this._passwordLabel);
            this._rightPanel.Controls.Add(this._passwordTextBox);
            this._rightPanel.Controls.Add(this._rememberMeCheckBox);
            this._rightPanel.Controls.Add(this._loginButton);
            this._rightPanel.Controls.Add(this._statusLabel);
            this._rightPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._rightPanel.Location = new System.Drawing.Point(300, 0);
            this._rightPanel.Name = "_rightPanel";
            this._rightPanel.Size = new System.Drawing.Size(400, 450);
            this._rightPanel.TabIndex = 1;
            // 
            // _titleLabel
            // 
            this._titleLabel.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this._titleLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(115)))), ((int)(((byte)(220)))));
            this._titleLabel.Location = new System.Drawing.Point(30, 60);
            this._titleLabel.Name = "_titleLabel";
            this._titleLabel.Size = new System.Drawing.Size(340, 40);
            this._titleLabel.TabIndex = 0;
            this._titleLabel.Text = "Kubernetes Manager";
            this._titleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // _usernameLabel
            // 
            this._usernameLabel.AutoSize = true;
            this._usernameLabel.Font = new System.Drawing.Font("Segoe UI", 9F);
            this._usernameLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this._usernameLabel.Location = new System.Drawing.Point(50, 130);
            this._usernameLabel.Name = "_usernameLabel";
            this._usernameLabel.Size = new System.Drawing.Size(63, 15);
            this._usernameLabel.TabIndex = 1;
            this._usernameLabel.Text = "Username:";
            // 
            // _usernameTextBox
            // 
            this._usernameTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._usernameTextBox.Font = new System.Drawing.Font("Segoe UI", 11F);
            this._usernameTextBox.Location = new System.Drawing.Point(50, 150);
            this._usernameTextBox.Name = "_usernameTextBox";
            this._usernameTextBox.Size = new System.Drawing.Size(300, 27);
            this._usernameTextBox.TabIndex = 2;
            // 
            // _passwordLabel
            // 
            this._passwordLabel.AutoSize = true;
            this._passwordLabel.Font = new System.Drawing.Font("Segoe UI", 9F);
            this._passwordLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this._passwordLabel.Location = new System.Drawing.Point(50, 195);
            this._passwordLabel.Name = "_passwordLabel";
            this._passwordLabel.Size = new System.Drawing.Size(60, 15);
            this._passwordLabel.TabIndex = 3;
            this._passwordLabel.Text = "Password:";
            // 
            // _passwordTextBox
            // 
            this._passwordTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._passwordTextBox.Font = new System.Drawing.Font("Segoe UI", 11F);
            this._passwordTextBox.Location = new System.Drawing.Point(50, 215);
            this._passwordTextBox.Name = "_passwordTextBox";
            this._passwordTextBox.Size = new System.Drawing.Size(300, 27);
            this._passwordTextBox.TabIndex = 4;
            this._passwordTextBox.UseSystemPasswordChar = true;
            // 
            // _rememberMeCheckBox
            // 
            this._rememberMeCheckBox.AutoSize = true;
            this._rememberMeCheckBox.Font = new System.Drawing.Font("Segoe UI", 9F);
            this._rememberMeCheckBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this._rememberMeCheckBox.Location = new System.Drawing.Point(50, 260);
            this._rememberMeCheckBox.Name = "_rememberMeCheckBox";
            this._rememberMeCheckBox.Size = new System.Drawing.Size(104, 19);
            this._rememberMeCheckBox.TabIndex = 5;
            this._rememberMeCheckBox.Text = "Remember Me";
            this._rememberMeCheckBox.UseVisualStyleBackColor = true;
            // 
            // _loginButton
            // 
            this._loginButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(115)))), ((int)(((byte)(220)))));
            this._loginButton.FlatAppearance.BorderSize = 0;
            this._loginButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._loginButton.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this._loginButton.ForeColor = System.Drawing.Color.White;
            this._loginButton.Location = new System.Drawing.Point(50, 300);
            this._loginButton.Name = "_loginButton";
            this._loginButton.Size = new System.Drawing.Size(300, 42);
            this._loginButton.TabIndex = 6;
            this._loginButton.Text = "Login";
            this._loginButton.UseVisualStyleBackColor = false;
            this._loginButton.Click += new System.EventHandler(this.LoginButton_Click);
            // 
            // _statusLabel
            // 
            this._statusLabel.Font = new System.Drawing.Font("Segoe UI", 9F);
            this._statusLabel.ForeColor = System.Drawing.Color.Red;
            this._statusLabel.Location = new System.Drawing.Point(50, 360);
            this._statusLabel.Name = "_statusLabel";
            this._statusLabel.Size = new System.Drawing.Size(300, 20);
            this._statusLabel.TabIndex = 7;
            this._statusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SimpleLoginView
            // 
            this.AcceptButton = this._loginButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(700, 450);
            this.Controls.Add(this._rightPanel);
            this.Controls.Add(this._leftPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SimpleLoginView";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Kubernetes Manager - Login";
            this._leftPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._logoPictureBox)).EndInit();
            this._rightPanel.ResumeLayout(false);
            this._rightPanel.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion
    }
}
