using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Week67
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (!TestConnection())
            {
                MessageBox.Show("Database connection failed. Application will exit.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return; // stop app
            }

            Application.Run(new MainForm());
        }

        static bool TestConnection()
        {
            try
            {
                using (var cn = Db.Open()) { }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
