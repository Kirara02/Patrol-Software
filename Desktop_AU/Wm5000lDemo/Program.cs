using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using dotenv.net;

namespace Wm5000AEDemo
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            DotEnv.Load();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new SyncForm());
            //Application.Run(new frmMain());

        }
    }
}
