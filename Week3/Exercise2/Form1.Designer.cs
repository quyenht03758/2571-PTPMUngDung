using System.Windows.Forms;

namespace Exercise2
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        // Controls declared here so Designer can find them
        private PictureBox picPreview;
        private TextBox txtPath;
        private Button btnBrowse;
        private Button btnLoadFromPath;
        private Button btnSaveAs;
        private Button btnClear;
        private Label lblHint;
       
      
   

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.picPreview = new System.Windows.Forms.PictureBox();
            this.txtPath = new System.Windows.Forms.TextBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.btnLoadFromPath = new System.Windows.Forms.Button();
            this.btnSaveAs = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.lblHint = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.picPreview)).BeginInit();
            this.SuspendLayout();
            // 
            // picPreview
            // 
            this.picPreview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picPreview.Location = new System.Drawing.Point(20, 20);
            this.picPreview.Name = "picPreview";
            this.picPreview.Size = new System.Drawing.Size(640, 520);
            this.picPreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picPreview.TabIndex = 0;
            this.picPreview.TabStop = false;
            // 
            // txtPath
            // 
            this.txtPath.Location = new System.Drawing.Point(20, 560);
            this.txtPath.Name = "txtPath";
            this.txtPath.Size = new System.Drawing.Size(520, 30);
            this.txtPath.TabIndex = 1;
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(550, 557);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(90, 30);
            this.btnBrowse.TabIndex = 2;
            this.btnBrowse.Text = "Browse...";
            this.btnBrowse.UseVisualStyleBackColor = true;
            // 
            // btnLoadFromPath
            // 
            this.btnLoadFromPath.Location = new System.Drawing.Point(650, 557);
            this.btnLoadFromPath.Name = "btnLoadFromPath";
            this.btnLoadFromPath.Size = new System.Drawing.Size(120, 30);
            this.btnLoadFromPath.TabIndex = 3;
            this.btnLoadFromPath.Text = "Load from Path";
            this.btnLoadFromPath.UseVisualStyleBackColor = true;
            // 
            // btnSaveAs
            // 
            this.btnSaveAs.Location = new System.Drawing.Point(780, 557);
            this.btnSaveAs.Name = "btnSaveAs";
            this.btnSaveAs.Size = new System.Drawing.Size(100, 30);
            this.btnSaveAs.TabIndex = 4;
            this.btnSaveAs.Text = "Save As...";
            this.btnSaveAs.UseVisualStyleBackColor = true;
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(780, 20);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(100, 30);
            this.btnClear.TabIndex = 5;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            // 
            // lblHint
            // 
            this.lblHint.AutoSize = true;
            this.lblHint.Location = new System.Drawing.Point(680, 70);
            this.lblHint.Name = "lblHint";
            this.lblHint.Size = new System.Drawing.Size(318, 69);
            this.lblHint.TabIndex = 6;
            this.lblHint.Text = "Tips:\r\n- Drag & drop an image here\r\n- Ctrl+O: Open | Ctrl+S: Save | Del: Clear";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 23F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(920, 620);
            this.Controls.Add(this.picPreview);
            this.Controls.Add(this.txtPath);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.btnLoadFromPath);
            this.Controls.Add(this.btnSaveAs);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.lblHint);
            this.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Image Open/Save Demo";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picPreview)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_currentImage != null)
                {
                    _currentImage.Dispose();
                    _currentImage = null;
                }
                components?.Dispose();
            }
            base.Dispose(disposing);
        }
       
    }
}

