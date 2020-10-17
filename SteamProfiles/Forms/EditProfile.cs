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
                if (key.GetValue("Mode")?.ToString() == "Dark")
                {
                    GetAllControls.ThemeChange(mode: true, this, Color.FromArgb(45, 45, 45), Color.FromArgb(55, 55, 55));
                    button1.BackColor = Color.FromArgb(28, 28, 28);
                }
                else if (key.GetValue("Mode")?.ToString() == "Light")
                {
                    GetAllControls.ThemeChange(mode: true, this, Color.FromArgb(0, 0, 50), Color.FromArgb(0, 0, 75));
                    button1.BackColor = Color.FromArgb(0, 0, 50);
                }
            }
        }
    }
}
