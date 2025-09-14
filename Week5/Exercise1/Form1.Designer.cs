namespace Exercise1
{
    partial class Form1
    {

        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.ComboBox cbxDepartments;
       
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
            this.cbxDepartments = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // cbxDepartments
            // 
            this.cbxDepartments.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxDepartments.FormattingEnabled = true;
            this.cbxDepartments.Location = new System.Drawing.Point(30, 30);
            this.cbxDepartments.Name = "cbxDepartments";
            this.cbxDepartments.Size = new System.Drawing.Size(250, 23);
            this.cbxDepartments.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(400, 150);
            this.Controls.Add(this.cbxDepartments);
            this.Name = "Form1";
            this.Text = "Department Binding";
            this.ResumeLayout(false);
        }

        #endregion
    }
}

