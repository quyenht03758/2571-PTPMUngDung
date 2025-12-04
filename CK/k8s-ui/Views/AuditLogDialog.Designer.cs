namespace K8sManager.Views
{
    partial class AuditLogDialog
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
            this.components = new System.ComponentModel.Container();
            this.SuspendLayout();
            this.ClientSize = new System.Drawing.Size(900, 600);
            this.Name = "AuditLogDialog";
            this.Text = "AuditLogDialog";
            this.ResumeLayout(false);
        }
    }
}
