namespace K8sManager.Views
{
    partial class TerminalDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            this._txtCommand = new MaterialSkin.Controls.MaterialTextBox2();
            this._btnExecute = new MaterialSkin.Controls.MaterialButton();
            this._txtOutput = new System.Windows.Forms.TextBox();
            this._panel = new System.Windows.Forms.Panel();
            this._panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // _txtCommand
            // 
            this._txtCommand.AnimateReadOnly = false;
            this._txtCommand.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this._txtCommand.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this._txtCommand.Depth = 0;
            this._txtCommand.Dock = System.Windows.Forms.DockStyle.Top;
            this._txtCommand.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this._txtCommand.HideSelection = true;
            this._txtCommand.Hint = "Enter command...";
            this._txtCommand.LeadingIcon = null;
            this._txtCommand.Location = new System.Drawing.Point(10, 10);
            this._txtCommand.MaxLength = 32767;
            this._txtCommand.MouseState = MaterialSkin.MouseState.OUT;
            this._txtCommand.Name = "_txtCommand";
            this._txtCommand.PasswordChar = '\0';
            this._txtCommand.PrefixSuffixText = null;
            this._txtCommand.ReadOnly = false;
            this._txtCommand.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._txtCommand.SelectedText = "";
            this._txtCommand.SelectionLength = 0;
            this._txtCommand.SelectionStart = 0;
            this._txtCommand.ShortcutsEnabled = true;
            this._txtCommand.Size = new System.Drawing.Size(880, 36);
            this._txtCommand.TabIndex = 0;
            this._txtCommand.TabStop = false;
            this._txtCommand.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this._txtCommand.TrailingIcon = null;
            this._txtCommand.UseSystemPasswordChar = false;
            this._txtCommand.UseTallSize = false;
            // 
            // _btnExecute
            // 
            this._btnExecute.AutoSize = false;
            this._btnExecute.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._btnExecute.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this._btnExecute.Depth = 0;
            this._btnExecute.Dock = System.Windows.Forms.DockStyle.Top;
            this._btnExecute.HighEmphasis = true;
            this._btnExecute.Icon = null;
            this._btnExecute.Location = new System.Drawing.Point(10, 46);
            this._btnExecute.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this._btnExecute.MouseState = MaterialSkin.MouseState.HOVER;
            this._btnExecute.Name = "_btnExecute";
            this._btnExecute.NoAccentTextColor = System.Drawing.Color.Empty;
            this._btnExecute.Size = new System.Drawing.Size(880, 36);
            this._btnExecute.TabIndex = 1;
            this._btnExecute.Text = "Execute";
            this._btnExecute.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this._btnExecute.UseAccentColor = false;
            this._btnExecute.UseVisualStyleBackColor = true;
            // 
            // _txtOutput
            // 
            this._txtOutput.BackColor = System.Drawing.Color.Black;
            this._txtOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this._txtOutput.Font = new System.Drawing.Font("Consolas", 10F);
            this._txtOutput.ForeColor = System.Drawing.Color.LightGreen;
            this._txtOutput.Location = new System.Drawing.Point(0, 90);
            this._txtOutput.Multiline = true;
            this._txtOutput.Name = "_txtOutput";
            this._txtOutput.ReadOnly = true;
            this._txtOutput.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this._txtOutput.Size = new System.Drawing.Size(900, 510);
            this._txtOutput.TabIndex = 2;
            // 
            // _panel
            // 
            this._panel.Controls.Add(this._btnExecute);
            this._panel.Controls.Add(this._txtCommand);
            this._panel.Dock = System.Windows.Forms.DockStyle.Top;
            this._panel.Location = new System.Drawing.Point(0, 0);
            this._panel.Name = "_panel";
            this._panel.Padding = new System.Windows.Forms.Padding(10);
            this._panel.Size = new System.Drawing.Size(900, 90);
            this._panel.TabIndex = 3;
            // 
            // TerminalDialog
            // 
            this.ClientSize = new System.Drawing.Size(900, 600);
            this.Controls.Add(this._txtOutput);
            this.Controls.Add(this._panel);
            this.Name = "TerminalDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this._panel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion
    }
}
