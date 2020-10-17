using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SteamProfiles
{
    public static class SelectLanguage
    {
        public static void Lang()
        {
            using RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\SteamProfiles");
            if (key.GetValue("Language").ToString() == "Русский")
            {
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("ru");
            }
            else if (key.GetValue("Language").ToString() == "English")
            {
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en");
            }
            else
            {
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en");
            }
        }
    }
}
