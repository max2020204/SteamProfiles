using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Linq;
using System.Resources;
using System.Windows.Forms;

namespace SteamProfiles
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new SteamProfiles());
        }
    }
}
