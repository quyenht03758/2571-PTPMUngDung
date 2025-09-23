using System.Drawing;
using System.Windows.Forms;

namespace Exercise3.Views
{
    partial class FrmGameBoard
    {
        private Panel pnlBoard;
        private Panel pnlInfo;
        private Label lblScoreText;
        private Label lblScoreValue;
        private Label lblLevelText;
        private Label lblLevelValue;
        private Label lblTimeText;
        private Label lblTimeValue;
        private Button btnStart;
        private Button btnSettings;
        private Button btnTopScores;
        private ListView lvTopScores;

        private void InitializeComponent()
        {
            this.pnlBoard = new System.Windows.Forms.Panel();
            this.pnlInfo = new System.Windows.Forms.Panel();
            this.lblScoreText = new System.Windows.Forms.Label();
            this.lblScoreValue = new System.Windows.Forms.Label();
            this.lblLevelText = new System.Windows.Forms.Label();
            this.lblLevelValue = new System.Windows.Forms.Label();
            this.lblTimeText = new System.Windows.Forms.Label();
            this.lblTimeValue = new System.Windows.Forms.Label();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnSettings = new System.Windows.Forms.Button();
            this.btnTopScores = new System.Windows.Forms.Button();
            this.lvTopScores = new System.Windows.Forms.ListView();
            this.SuspendLayout();
            // 
            // pnlBoard
            // 
            this.pnlBoard.Location = new System.Drawing.Point(12, 12);
            this.pnlBoard.Name = "pnlBoard";
            this.pnlBoard.Size = new System.Drawing.Size(800, 800);
            this.pnlBoard.TabIndex = 0;
            // 
            // pnlInfo
            // 
            this.pnlInfo.Location = new System.Drawing.Point(830, 12);
            this.pnlInfo.Name = "pnlInfo";
            this.pnlInfo.Size = new System.Drawing.Size(240, 800);
            this.pnlInfo.TabIndex = 1;
            // 
            // lblScoreText
            // 
            this.lblScoreText.AutoSize = true;
            this.lblScoreText.Location = new System.Drawing.Point(10, 20);
            this.lblScoreText.Text = "Score:";
            // 
            // lblScoreValue
            // 
            this.lblScoreValue.AutoSize = true;
            this.lblScoreValue.Location = new System.Drawing.Point(100, 20);
            this.lblScoreValue.Text = "0";
            // 
            // lblLevelText
            // 
            this.lblLevelText.AutoSize = true;
            this.lblLevelText.Location = new System.Drawing.Point(10, 50);
            this.lblLevelText.Text = "Level:";
            // 
            // lblLevelValue
            // 
            this.lblLevelValue.AutoSize = true;
            this.lblLevelValue.Location = new System.Drawing.Point(100, 50);
            this.lblLevelValue.Text = "6x6";
            // 
            // lblTimeText
            // 
            this.lblTimeText.AutoSize = true;
            this.lblTimeText.Location = new System.Drawing.Point(10, 80);
            this.lblTimeText.Text = "Time:";
            // 
            // lblTimeValue
            // 
            this.lblTimeValue.AutoSize = true;
            this.lblTimeValue.Location = new System.Drawing.Point(100, 80);
            this.lblTimeValue.Text = "00:00:00";
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(10, 120);
            this.btnStart.Size = new System.Drawing.Size(75, 25);
            this.btnStart.Text = "Start";
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnSettings
            // 
            this.btnSettings.Location = new System.Drawing.Point(100, 120);
            this.btnSettings.Size = new System.Drawing.Size(75, 25);
            this.btnSettings.Text = "Settings";
            this.btnSettings.Click += new System.EventHandler(this.btnSettings_Click);
            // 
            // btnTopScores
            // 
            this.btnTopScores.Location = new System.Drawing.Point(10, 160);
            this.btnTopScores.Size = new System.Drawing.Size(165, 25);
            this.btnTopScores.Text = "Top Scores";
            this.btnTopScores.Click += new System.EventHandler(this.btnTopScores_Click);
            // 
            // lvTopScores
            // 
            this.lvTopScores.Location = new System.Drawing.Point(10, 200);
            this.lvTopScores.Size = new System.Drawing.Size(220, 580);
            this.lvTopScores.View = View.Details;
            this.lvTopScores.FullRowSelect = true;
            this.lvTopScores.GridLines = true;
            this.lvTopScores.Columns.Add("Player", 70);
            this.lvTopScores.Columns.Add("Score", 60);
            this.lvTopScores.Columns.Add("Time", 70);
            // 
            // pnlInfo Controls
            // 
            this.pnlInfo.Controls.Add(this.lblScoreText);
            this.pnlInfo.Controls.Add(this.lblScoreValue);
            this.pnlInfo.Controls.Add(this.lblLevelText);
            this.pnlInfo.Controls.Add(this.lblLevelValue);
            this.pnlInfo.Controls.Add(this.lblTimeText);
            this.pnlInfo.Controls.Add(this.lblTimeValue);
            this.pnlInfo.Controls.Add(this.btnStart);
            this.pnlInfo.Controls.Add(this.btnSettings);
            this.pnlInfo.Controls.Add(this.btnTopScores);
            this.pnlInfo.Controls.Add(this.lvTopScores);
            // 
            // FrmGameBoard
            // 
            this.ClientSize = new System.Drawing.Size(1100, 840);
            this.Controls.Add(this.pnlBoard);
            this.Controls.Add(this.pnlInfo);
            this.Name = "FrmGameBoard";
            this.Text = "Memory Game";
            this.Load += new System.EventHandler(this.FrmGameBoard_Load);
            this.ResumeLayout(false);
        }
    }
}
