using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SteamProfiles.Forms
{
    public partial class About : Form
    {
        ResourceManager res;
        string wallet;
        public About()
        {
            InitializeComponent();
        }

        private void About_Load(object sender, EventArgs e)
        {
            SelectLanguage.Lang();
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            string version = fvi.ProductVersion;
            label3.Text += " " + version;
            res = new ResourceManager("SteamProfiles.Resource.About.Res", typeof(Settings).Assembly);
            wallet = res.GetString("Wallet");
            using RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\SteamProfiles", true);
            if (key != null)
            {
                switch (key.GetValue("Mode")?.ToString())
                {
                    case "Dark":
                        Themes.ThemeChange(mode: true, this, Color.FromArgb(45, 45, 45), Color.FromArgb(28, 28, 28), Color.FromArgb(55, 55, 55));
                        Themes.ChangeForeColor(true, this, Color.White);
                        linkLabel1.LinkColor = Color.White;
                        linkLabel2.LinkColor = Color.White;
                        linkLabel3.LinkColor = Color.White;
                        linkLabel4.LinkColor = Color.White;
                        break;
                    case "Light":
                        Themes.ThemeChange(mode: true, form: this, backcolor: Color.FromArgb(189, 204, 212), buttoncolor: Color.FromArgb(166, 177, 183),MouseDownBackColor: Color.FromArgb(55, 55, 55));
                        Themes.ChangeForeColor(true, this, Color.Black);
                        break;
                    case "OldSchool":
                        Themes.ThemeChange(mode: true, this, Color.FromArgb(0, 0, 80), Color.FromArgb(0, 0, 50), Color.FromArgb(0, 0, 75));
                        Themes.ChangeForeColor(true, this, Color.White);
                        linkLabel1.LinkColor = Color.White;
                        linkLabel2.LinkColor = Color.White;
                        linkLabel3.LinkColor = Color.White;
                        linkLabel4.LinkColor = Color.White;
                        break;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Process.Start("https://boosty.to/blackdream");
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Clipboard.SetText(linkLabel1.Text);
            MessageBox.Show(wallet);
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Clipboard.SetText(linkLabel2.Text);
            MessageBox.Show(wallet);
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Clipboard.SetText(linkLabel3.Text);
            MessageBox.Show(wallet);
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Clipboard.SetText(linkLabel4.Text);
            MessageBox.Show(wallet);
        }
    }
}
