using System;
using System.Windows.Forms;

namespace ClientApplication
{
    /// <summary>
    /// Main entry point for the Client application.
    /// </summary>
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FrmProjectManager());
        }
    }
}
