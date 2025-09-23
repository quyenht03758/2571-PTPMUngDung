// Program.cs
using Exercise3.Views;
using System;
using System.Windows.Forms;

namespace Exercise3
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FrmGameBoard());
        }
    }
}
