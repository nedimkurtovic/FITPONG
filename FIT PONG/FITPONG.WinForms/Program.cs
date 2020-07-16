using FIT_PONG.WinForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FIT_PONG.WinForms
{
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
            //var frmLog = new frmLogin();
            //Application.Run(frmLog);
            //if (frmLog.UspjesnoPrijavljen)
            //{
                Application.Run(new frmMain());
            //}
        }
    }
}
