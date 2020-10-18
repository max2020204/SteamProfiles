using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SteamProfiles.Forms
{
    public partial class About : Form
    {
        public About()
        {
            InitializeComponent();
        }

        private void About_Load(object sender, EventArgs e)
        {
            SelectLanguage.Lang();
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            string version = fvi.FileVersion;
            label3.Text += " " + version;
            using RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\SteamProfiles", true);
            if (key != null)
            {
                if (key.GetValue("Mode")?.ToString() == "Dark")
                {
                    GetAllControls.ThemeChange(mode: true, this, Color.FromArgb(45, 45, 45), Color.FromArgb(55, 55, 55));
                    // button1.BackColor = Color.FromArgb(28, 28, 28);
                }
                else if (key.GetValue("Mode")?.ToString() == "Light")
                {
                    GetAllControls.ThemeChange(mode: true, this, Color.FromArgb(0, 0, 80), Color.FromArgb(0, 0, 75));
                   //  button1.BackColor = Color.FromArgb(0, 0, 50);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Process.Start("https://boosty.to/blackdream");
        }
    }
}
