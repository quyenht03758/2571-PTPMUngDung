using System.Drawing;
using System.Windows.Forms;

namespace Exercise2
{
    partial class FrmGameBoard
    {
        private System.ComponentModel.IContainer components = null;
        private Panel pnlBoard;
        private Panel pnlInfo;
        private Label lblScoreText;
        private Label lblScoreValue;
        private Label lblLevelText;
        private Label lblLevelValue;
        private Label lblNextText;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            this.pnlBoard = new System.Windows.Forms.Panel();
            this.pnlInfo = new System.Windows.Forms.Panel();
            this.lblScoreText = new System.Windows.Forms.Label();
            this.lblScoreValue = new System.Windows.Forms.Label();
            this.lblLevelText = new System.Windows.Forms.Label();
            this.lblLevelValue = new System.Windows.Forms.Label();
            this.lblNextText = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // FrmGameBoard (Form)
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Text = "FrmGameBoard";
            this.ClientSize = new System.Drawing.Size(960, 560);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.FrmGameBoard_Load);
            // 
            // pnlBoard (left: 9x9 grid)
            // 
            this.pnlBoard.BackColor = SystemColors.Control;
            this.pnlBoard.Location = new System.Drawing.Point(16, 16);
            this.pnlBoard.Name = "pnlBoard";
            this.pnlBoard.Size = new System.Drawing.Size(560, 520); // recalculated in BuildBoard()
            this.pnlBoard.BorderStyle = BorderStyle.None;
            // 
            // pnlInfo (right: Score/Level/Next)
            // 
            this.pnlInfo.BackColor = SystemColors.Control;
            this.pnlInfo.Location = new System.Drawing.Point(600, 16);
            this.pnlInfo.Name = "pnlInfo";
            this.pnlInfo.Size = new System.Drawing.Size(340, 520);
            this.pnlInfo.BorderStyle = BorderStyle.None;
            // 
            // lblScoreText
            // 
            this.lblScoreText.AutoSize = true;
            this.lblScoreText.Font = new Font("Times New Roman", 18F, FontStyle.Bold, GraphicsUnit.Point);
            this.lblScoreText.Location = new Point(20, 40);
            this.lblScoreText.Name = "lblScoreText";
            this.lblScoreText.Size = new Size(92, 26);
            this.lblScoreText.Text = "Score:";
            // 
            // lblScoreValue
            // 
            this.lblScoreValue.AutoSize = true;
            this.lblScoreValue.Font = new Font("Times New Roman", 18F, FontStyle.Bold, GraphicsUnit.Point);
            this.lblScoreValue.ForeColor = Color.RoyalBlue;
            this.lblScoreValue.Location = new Point(140, 40);
            this.lblScoreValue.Name = "lblScoreValue";
            this.lblScoreValue.Size = new Size(54, 26);
            this.lblScoreValue.Text = "180";
            // 
            // lblLevelText
            // 
            this.lblLevelText.AutoSize = true;
            this.lblLevelText.Font = new Font("Times New Roman", 18F, FontStyle.Bold, GraphicsUnit.Point);
            this.lblLevelText.Location = new Point(20, 100);
            this.lblLevelText.Name = "lblLevelText";
            this.lblLevelText.Size = new Size(79, 26);
            this.lblLevelText.Text = "Level:";
            // 
            // lblLevelValue
            // 
            this.lblLevelValue.AutoSize = true;
            this.lblLevelValue.Font = new Font("Times New Roman", 18F, FontStyle.Bold, GraphicsUnit.Point);
            this.lblLevelValue.ForeColor = Color.RoyalBlue;
            this.lblLevelValue.Location = new Point(140, 100);
            this.lblLevelValue.Name = "lblLevelValue";
            this.lblLevelValue.Size = new Size(24, 26);
            this.lblLevelValue.Text = "1";
            // 
            // lblNextText
            // 
            this.lblNextText.AutoSize = true;
            this.lblNextText.Font = new Font("Times New Roman", 18F, FontStyle.Bold, GraphicsUnit.Point);
            this.lblNextText.Location = new Point(20, 160);
            this.lblNextText.Name = "lblNextText";
            this.lblNextText.Size = new Size(71, 26);
            this.lblNextText.Text = "Next:";
            // 
            // Add labels into pnlInfo
            // 
            this.pnlInfo.Controls.Add(this.lblScoreText);
            this.pnlInfo.Controls.Add(this.lblScoreValue);
            this.pnlInfo.Controls.Add(this.lblLevelText);
            this.pnlInfo.Controls.Add(this.lblLevelValue);
            this.pnlInfo.Controls.Add(this.lblNextText);
            // 
            // Add panels to Form
            // 
            this.Controls.Add(this.pnlBoard);
            this.Controls.Add(this.pnlInfo);
            this.ResumeLayout(false);
        }
    }
}
