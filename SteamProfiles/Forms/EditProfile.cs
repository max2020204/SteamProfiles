using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SteamProfiles.Forms
{
    public partial class EditProfile : Form
    {
        ResourceManager res;
        private string Done, LoginError;
        public EditProfile()
        {
            SelectLanguage.Lang();
            InitializeComponent();
        }
        void switch_language()
        {
            Done = res.GetString("Done");
            LoginError = res.GetString("LoginError");

        }
        private void Button1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(textBox1.Text))
            {
                if (!string.IsNullOrWhiteSpace(textBox2.Text) && string.IsNullOrWhiteSpace(textBox3.Text))
                {
                    using RegistryKey reg = Registry.CurrentUser.OpenSubKey($@"SOFTWARE\SteamProfiles\{textBox1.Text}", true);
                    if (reg != null)
                    {
                        Registry.CurrentUser.OpenSubKey($@"SOFTWARE\SteamProfiles\{textBox1.Text}", true).SetValue("UserName", Encriptor.Encypter(textBox2.Text));
                        MessageBox.Show(Done);
                    }
                }
                if (!string.IsNullOrWhiteSpace(textBox2.Text) && !string.IsNullOrWhiteSpace(textBox3.Text))
                {
                    using RegistryKey reg = Registry.CurrentUser.OpenSubKey($@"SOFTWARE\SteamProfiles\{textBox1.Text}", true);
                    if (reg != null)
                    {
                        Registry.CurrentUser.OpenSubKey($@"SOFTWARE\SteamProfiles\{textBox2.Text}", true).SetValue("UserName", Encriptor.Encypter(textBox2.Text));
                        Registry.CurrentUser.OpenSubKey($@"SOFTWARE\SteamProfiles\{textBox2.Text}", true).SetValue("Password", Encriptor.Encypter(textBox3.Text));
                        MessageBox.Show(Done);

                    }
                }
                if (!string.IsNullOrWhiteSpace(textBox3.Text) && string.IsNullOrWhiteSpace(textBox2.Text))
                {
                    using RegistryKey reg = Registry.CurrentUser.OpenSubKey($@"SOFTWARE\SteamProfiles\{textBox1.Text}", true);
                    if (reg != null)
                    {
                        reg.SetValue("Password", Encriptor.Encypter(textBox3.Text));
                        MessageBox.Show(Done);
                    }
                }
            }
            else
            {
                MessageBox.Show(LoginError);
            }
        }

        private void EditProfile_Load(object sender, EventArgs e)
        {
            res = new ResourceManager("SteamProfiles.Resource.Edit.Res", typeof(Settings).Assembly);
            switch_language();
            using RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\SteamProfiles", true);
            if (key != null)
            {
                switch (key.GetValue("Mode")?.ToString())
                {
                    case "Dark":
                        Themes.ThemeChange(mode: true, this, Color.FromArgb(45, 45, 45), Color.FromArgb(28, 28, 28), Color.FromArgb(55, 55, 55));
                        Themes.ChangeForeColor(true, this, Color.White);
                        break;
                    case "Light":
                        Themes.ThemeChange(mode: true, form: this, backcolor: Color.FromArgb(189, 204, 212), buttoncolor: Color.FromArgb(166, 177, 183), MouseDownBackColor: Color.FromArgb(55, 55, 55));
                        Themes.ChangeForeColor(true, this, Color.Black);
                        break;
                    case "OldSchool":
                        Themes.ThemeChange(mode: true, this, Color.FromArgb(0, 0, 80), Color.FromArgb(0, 0, 50), Color.FromArgb(0, 0, 75));
                        Themes.ChangeForeColor(true, this, Color.White);
                        break;
                }
            }
        }
    }
}
