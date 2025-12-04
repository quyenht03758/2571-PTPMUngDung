namespace K8sManager.Views
{
    partial class LoginView
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this._titleLabel = new MaterialSkin.Controls.MaterialLabel();
            this._usernameTextBox = new MaterialSkin.Controls.MaterialTextBox2();
            this._passwordTextBox = new MaterialSkin.Controls.MaterialTextBox2();
            this._statusLabel = new MaterialSkin.Controls.MaterialLabel();
            this._loginButton = new MaterialSkin.Controls.MaterialButton();
            this._exitButton = new MaterialSkin.Controls.MaterialButton();
            this.SuspendLayout();
            // 
            // _titleLabel
            // 
            this._titleLabel.AutoSize = true;
            this._titleLabel.Depth = 0;
            this._titleLabel.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this._titleLabel.FontType = MaterialSkin.MaterialSkinManager.fontType.H5;
            this._titleLabel.Location = new System.Drawing.Point(50, 80);
            this._titleLabel.MouseState = MaterialSkin.MouseState.HOVER;
            this._titleLabel.Name = "_titleLabel";
            this._titleLabel.Size = new System.Drawing.Size(285, 37);
            this._titleLabel.TabIndex = 0;
            this._titleLabel.Text = "Kubernetes Manager";
            // 
            // _usernameTextBox
            // 
            this._usernameTextBox.AnimateReadOnly = false;
            this._usernameTextBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this._usernameTextBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this._usernameTextBox.Depth = 0;
            this._usernameTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this._usernameTextBox.HideSelection = true;
            this._usernameTextBox.Hint = "Tên đăng nhập";
            this._usernameTextBox.LeadingIcon = null;
            this._usernameTextBox.Location = new System.Drawing.Point(50, 150);
            this._usernameTextBox.MaxLength = 32767;
            this._usernameTextBox.MouseState = MaterialSkin.MouseState.OUT;
            this._usernameTextBox.Name = "_usernameTextBox";
            this._usernameTextBox.PasswordChar = '\0';
            this._usernameTextBox.PrefixSuffixText = null;
            this._usernameTextBox.ReadOnly = false;
            this._usernameTextBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._usernameTextBox.SelectedText = "";
            this._usernameTextBox.SelectionLength = 0;
            this._usernameTextBox.SelectionStart = 0;
            this._usernameTextBox.ShortcutsEnabled = true;
            this._usernameTextBox.Size = new System.Drawing.Size(300, 50);
            this._usernameTextBox.TabIndex = 0;
            this._usernameTextBox.TabStop = false;
            this._usernameTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this._usernameTextBox.TrailingIcon = null;
            this._usernameTextBox.UseSystemPasswordChar = false;
            // 
            // _passwordTextBox
            // 
            this._passwordTextBox.AnimateReadOnly = false;
            this._passwordTextBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this._passwordTextBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this._passwordTextBox.Depth = 0;
            this._passwordTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this._passwordTextBox.HideSelection = true;
            this._passwordTextBox.Hint = "Mật khẩu";
            this._passwordTextBox.LeadingIcon = null;
            this._passwordTextBox.Location = new System.Drawing.Point(50, 210);
            this._passwordTextBox.MaxLength = 32767;
            this._passwordTextBox.MouseState = MaterialSkin.MouseState.OUT;
            this._passwordTextBox.Name = "_passwordTextBox";
            this._passwordTextBox.PasswordChar = '\0';
            this._passwordTextBox.PrefixSuffixText = null;
            this._passwordTextBox.ReadOnly = false;
            this._passwordTextBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._passwordTextBox.SelectedText = "";
            this._passwordTextBox.SelectionLength = 0;
            this._passwordTextBox.SelectionStart = 0;
            this._passwordTextBox.ShortcutsEnabled = true;
            this._passwordTextBox.Size = new System.Drawing.Size(300, 50);
            this._passwordTextBox.TabIndex = 1;
            this._passwordTextBox.TabStop = false;
            this._passwordTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this._passwordTextBox.TrailingIcon = null;
            this._passwordTextBox.UseSystemPasswordChar = true;
            // 
            // _statusLabel
            // 
            this._statusLabel.AutoSize = true;
            this._statusLabel.Depth = 0;
            this._statusLabel.Font = new System.Drawing.Font("Roboto", 11F);
            this._statusLabel.FontType = MaterialSkin.MaterialSkinManager.fontType.Subtitle2;
            this._statusLabel.ForeColor = System.Drawing.Color.Red;
            this._statusLabel.Location = new System.Drawing.Point(50, 270);
            this._statusLabel.MouseState = MaterialSkin.MouseState.HOVER;
            this._statusLabel.Name = "_statusLabel";
            this._statusLabel.Size = new System.Drawing.Size(0, 19);
            this._statusLabel.TabIndex = 3;
            this._statusLabel.Visible = false;
            // 
            // _loginButton
            // 
            this._loginButton.AutoSize = false;
            this._loginButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._loginButton.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this._loginButton.Depth = 0;
            this._loginButton.HighEmphasis = true;
            this._loginButton.Icon = null;
            this._loginButton.Location = new System.Drawing.Point(50, 300);
            this._loginButton.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this._loginButton.MouseState = MaterialSkin.MouseState.HOVER;
            this._loginButton.Name = "_loginButton";
            this._loginButton.NoAccentTextColor = System.Drawing.Color.Empty;
            this._loginButton.Size = new System.Drawing.Size(140, 36);
            this._loginButton.TabIndex = 2;
            this._loginButton.Text = "ĐĂNG NHẬP";
            this._loginButton.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this._loginButton.UseAccentColor = false;
            this._loginButton.UseVisualStyleBackColor = true;
            // 
            // _exitButton
            // 
            this._exitButton.AutoSize = false;
            this._exitButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._exitButton.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this._exitButton.Depth = 0;
            this._exitButton.HighEmphasis = true;
            this._exitButton.Icon = null;
            this._exitButton.Location = new System.Drawing.Point(210, 300);
            this._exitButton.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this._exitButton.MouseState = MaterialSkin.MouseState.HOVER;
            this._exitButton.Name = "_exitButton";
            this._exitButton.NoAccentTextColor = System.Drawing.Color.Empty;
            this._exitButton.Size = new System.Drawing.Size(140, 36);
            this._exitButton.TabIndex = 3;
            this._exitButton.Text = "THOÁT";
            this._exitButton.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Outlined;
            this._exitButton.UseAccentColor = false;
            this._exitButton.UseVisualStyleBackColor = true;
            // 
            // LoginView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(400, 380);
            this.Controls.Add(this._titleLabel);
            this.Controls.Add(this._usernameTextBox);
            this.Controls.Add(this._passwordTextBox);
            this.Controls.Add(this._statusLabel);
            this.Controls.Add(this._loginButton);
            this.Controls.Add(this._exitButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LoginView";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "K8s Manager - Đăng nhập";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
