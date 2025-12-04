namespace K8sManager.Views
{
    partial class ScaleDialog
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
            this.components = new System.ComponentModel.Container();
            this._num = new System.Windows.Forms.NumericUpDown();
            this._ok = new MaterialSkin.Controls.MaterialButton();
            this._cancel = new MaterialSkin.Controls.MaterialButton();
            this._label = new MaterialSkin.Controls.MaterialLabel();
            this._bottom = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this._num)).BeginInit();
            this._bottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // _num
            // 
            this._num.Dock = System.Windows.Forms.DockStyle.Top;
            this._num.Font = new System.Drawing.Font("Segoe UI", 10F);
            this._num.Location = new System.Drawing.Point(0, 64);
            this._num.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            this._num.Minimum = new decimal(new int[] { 0, 0, 0, 0 });
            this._num.Name = "_num";
            this._num.Size = new System.Drawing.Size(360, 42);
            this._num.TabIndex = 0;
            this._num.Value = new decimal(new int[] { 2, 0, 0, 0 });
            // 
            // _ok
            // 
            this._ok.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._ok.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this._ok.Depth = 0;
            this._ok.Dock = System.Windows.Forms.DockStyle.Right;
            this._ok.HighEmphasis = true;
            this._ok.Icon = null;
            this._ok.Location = new System.Drawing.Point(260, 12);
            this._ok.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this._ok.MouseState = MaterialSkin.MouseState.HOVER;
            this._ok.Name = "_ok";
            this._ok.NoAccentTextColor = System.Drawing.Color.Empty;
            this._ok.Size = new System.Drawing.Size(88, 36);
            this._ok.TabIndex = 1;
            this._ok.Text = "Apply";
            this._ok.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this._ok.UseAccentColor = false;
            this._ok.UseVisualStyleBackColor = true;
            this._ok.Click += new System.EventHandler(this.BtnOk_Click);
            // 
            // _cancel
            // 
            this._cancel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._cancel.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this._cancel.Depth = 0;
            this._cancel.Dock = System.Windows.Forms.DockStyle.Right;
            this._cancel.HighEmphasis = true;
            this._cancel.Icon = null;
            this._cancel.Location = new System.Drawing.Point(160, 12);
            this._cancel.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this._cancel.MouseState = MaterialSkin.MouseState.HOVER;
            this._cancel.Name = "_cancel";
            this._cancel.NoAccentTextColor = System.Drawing.Color.Empty;
            this._cancel.Size = new System.Drawing.Size(100, 36);
            this._cancel.TabIndex = 2;
            this._cancel.Text = "Cancel";
            this._cancel.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Text;
            this._cancel.UseAccentColor = false;
            this._cancel.UseVisualStyleBackColor = true;
            this._cancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // _label
            // 
            this._label.AutoSize = true;
            this._label.Depth = 0;
            this._label.Dock = System.Windows.Forms.DockStyle.Top;
            this._label.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this._label.Location = new System.Drawing.Point(0, 0);
            this._label.MouseState = MaterialSkin.MouseState.HOVER;
            this._label.Name = "_label";
            this._label.Padding = new System.Windows.Forms.Padding(12, 12, 12, 4);
            this._label.Size = new System.Drawing.Size(81, 35);
            this._label.TabIndex = 3;
            this._label.Text = "Replicas";
            // 
            // _bottom
            // 
            this._bottom.Controls.Add(this._cancel);
            this._bottom.Controls.Add(this._ok);
            this._bottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._bottom.Location = new System.Drawing.Point(0, 120);
            this._bottom.Name = "_bottom";
            this._bottom.Padding = new System.Windows.Forms.Padding(12);
            this._bottom.Size = new System.Drawing.Size(360, 60);
            this._bottom.TabIndex = 4;
            // 
            // ScaleDialog
            // 
            this.ClientSize = new System.Drawing.Size(360, 180);
            this.Controls.Add(this._num);
            this.Controls.Add(this._label);
            this.Controls.Add(this._bottom);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(360, 180);
            this.Name = "ScaleDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Scale";
            ((System.ComponentModel.ISupportInitialize)(this._num)).EndInit();
            this._bottom.ResumeLayout(false);
            this._bottom.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion
    }
}
