namespace Exercise1
{
    partial class FrmMainApp
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.MenuStrip menuMain;
        private System.Windows.Forms.ToolStripMenuItem systemToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem managementEmployeesToolStripMenuItem;
        private System.Windows.Forms.ToolStrip toolMain;
        private System.Windows.Forms.ToolStripDropDownButton btnReports;
        private System.Windows.Forms.ContextMenuStrip cmsReports;
        private System.Windows.Forms.ToolStripMenuItem openReportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.menuMain = new System.Windows.Forms.MenuStrip();
            this.systemToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.managementEmployeesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolMain = new System.Windows.Forms.ToolStrip();
            this.btnReports = new System.Windows.Forms.ToolStripDropDownButton();
            this.cmsReports = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.openReportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMain.SuspendLayout();
            this.toolMain.SuspendLayout();
            this.cmsReports.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuMain
            // 
            this.menuMain.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.systemToolStripMenuItem,
            this.managementEmployeesToolStripMenuItem});
            this.menuMain.Location = new System.Drawing.Point(0, 0);
            this.menuMain.Name = "menuMain";
            this.menuMain.Size = new System.Drawing.Size(1024, 28);
            this.menuMain.TabIndex = 0;
            this.menuMain.Text = "menuMain";
            // 
            // systemToolStripMenuItem
            // 
            this.systemToolStripMenuItem.Name = "systemToolStripMenuItem";
            this.systemToolStripMenuItem.Size = new System.Drawing.Size(70, 24);
            this.systemToolStripMenuItem.Text = "System";
            // 
            // managementEmployeesToolStripMenuItem
            // 
            this.managementEmployeesToolStripMenuItem.Name = "managementEmployeesToolStripMenuItem";
            this.managementEmployeesToolStripMenuItem.Size = new System.Drawing.Size(183, 24);
            this.managementEmployeesToolStripMenuItem.Text = "ManagementEmployees";
            // 
            // toolMain
            // 
            this.toolMain.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolMain.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnReports});
            this.toolMain.Location = new System.Drawing.Point(0, 28);
            this.toolMain.Name = "toolMain";
            this.toolMain.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolMain.Size = new System.Drawing.Size(1024, 27);
            this.toolMain.TabIndex = 1;
            this.toolMain.Text = "toolMain";
            // 
            // btnReports
            // 
            this.btnReports.DropDown = this.cmsReports;
            this.btnReports.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnReports.Name = "btnReports";
            this.btnReports.Size = new System.Drawing.Size(74, 24);
            this.btnReports.Text = "Reports";
            // 
            // cmsReports
            // 
            this.cmsReports.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.cmsReports.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openReportToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.cmsReports.Name = "cmsReports";
            this.cmsReports.OwnerItem = this.btnReports;
            this.cmsReports.Size = new System.Drawing.Size(164, 52);
            // 
            // openReportToolStripMenuItem
            // 
            this.openReportToolStripMenuItem.Name = "openReportToolStripMenuItem";
            this.openReportToolStripMenuItem.Size = new System.Drawing.Size(163, 24);
            this.openReportToolStripMenuItem.Text = "Open Report";
            this.openReportToolStripMenuItem.Click += new System.EventHandler(this.openReportToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(163, 24);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // FrmMainApp
            // 
            this.ClientSize = new System.Drawing.Size(1024, 600);
            this.Controls.Add(this.toolMain);
            this.Controls.Add(this.menuMain);
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuMain;
            this.Name = "FrmMainApp";
            this.Text = "FrmMainApp";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.FrmMainApp_Load);
            this.menuMain.ResumeLayout(false);
            this.menuMain.PerformLayout();
            this.toolMain.ResumeLayout(false);
            this.toolMain.PerformLayout();
            this.cmsReports.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
