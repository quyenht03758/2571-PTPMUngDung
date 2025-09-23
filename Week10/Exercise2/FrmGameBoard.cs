using System;
using System.Drawing;
using System.Windows.Forms;

namespace Exercise2
{
    public partial class FrmGameBoard : Form
    {
        private const int Rows = 9;
        private const int Cols = 9;
        private Button[,] _buttons = new Button[Rows, Cols];

        // Layout settings
        private const int CellSize = 48;   // button size (px)
        private const int Gap = 8;         // space between buttons
        private const int OffsetX = 20;    // left margin
        private const int OffsetY = 20;    // top margin

        public FrmGameBoard()
        {
            InitializeComponent();
        }

        private void FrmGameBoard_Load(object sender, EventArgs e)
        {
            BuildBoard();

            // Demo values to match the screenshot
            lblScoreValue.Text = "180";
            lblLevelValue.Text = "1";
            lblNextText.Text = "Next:";
        }

        private void BuildBoard()
        {
            // Calculate board size
            int boardWidth = OffsetX + Cols * (CellSize + Gap);
            int boardHeight = OffsetY + Rows * (CellSize + Gap);
            pnlBoard.Size = new Size(boardWidth + OffsetX, boardHeight + OffsetY);

            // Create a 9x9 grid of buttons
            for (int r = 0; r < Rows; r++)
            {
                for (int c = 0; c < Cols; c++)
                {
                    var btn = new Button
                    {
                        Name = $"btn_{r}_{c}",
                        Text = $"{r}-{c}",
                        Font = new Font("Segoe UI", 9f, FontStyle.Regular),
                        Size = new Size(CellSize, CellSize),
                        Location = new Point(
                            OffsetX + c * (CellSize + Gap),
                            OffsetY + r * (CellSize + Gap)
                        ),
                        FlatStyle = FlatStyle.Standard,
                        TabStop = false
                    };

                    // Optional: click to test
                    // btn.Click += (s, e) => MessageBox.Show(btn.Text);

                    pnlBoard.Controls.Add(btn);
                    _buttons[r, c] = btn;
                }
            }
        }
    }
}
