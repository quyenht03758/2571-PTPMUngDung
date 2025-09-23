using System;
using System.Windows.Forms;

namespace Exercise1
{
    public partial class FrmMainApp : Form
    {
        public FrmMainApp()
        {
            InitializeComponent();

            this.ContextMenuStrip = cmsReports;
        }
        private void openReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var f = new Form();
            f.Text = "Report Viewer";
            f.MdiParent = this;
            f.Show();
        }

        // (Tùy chọn) Exit -> đóng app
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FrmMainApp_Load(object sender, EventArgs e)
        {

        }
    }
}
