using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace sample_CSharp2008
{
    static class Program
    {
        /// <summary>
        /// Main entry point
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new DeviceSelect());
            Application.Run(new EventPatrolForm());
        }
    }
}
