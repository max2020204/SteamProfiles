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
    public partial class AddProfile : Form
    {
        ResourceManager res;
        private string UserNameError, LoginError, PasswordError, Success, FieldsError, SteamSettings;

        public AddProfile()
        {
            SelectLanguage.Lang();
            InitializeComponent();
        }
        void Switch_language()
        {
            UserNameError = res.GetString("UserNameError");
            LoginError = res.GetString("LoginError");
            PasswordError = res.GetString("PasswordError");
            Success = res.GetString("Success");
            FieldsError = res.GetString("FieldsError");
            SteamSettings = res.GetString("SteamSettings");
        }
        private void Button1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(textBox1.Text) && !string.IsNullOrWhiteSpace(textBox2.Text) && !string.IsNullOrWhiteSpace(textBox3.Text))
            {
                string user = textBox1.Text, login = textBox2.Text, pass = textBox3.Text;
                if (textBox1.Text.Contains(' '))
                    user = textBox1.Text.Replace(" ", "");
                if (textBox2.Text.Contains(' '))
                    login = textBox2.Text.Replace(" ", "");
                if (textBox3.Text.Contains(' '))
                    pass = textBox3.Text.Replace(" ", "");
                using (RegistryKey key = Registry.CurrentUser.CreateSubKey($@"Software\SteamProfiles\{login}"))
                {
                    user = Encriptor.Encypter(user);
                    login = Encriptor.Encypter(login);
                    pass = Encriptor.Encypter(pass);
                    key.SetValue("UserName", user);

                    key.SetValue("Login", login);

                    key.SetValue("Password", pass);

                }
                textBox1.Clear();
                textBox2.Clear();
                textBox3.Clear();
                MessageBox.Show(Success);
            }
            else
            {
                MessageBox.Show(FieldsError);
            }

        }
        private void AddProfile_Load(object sender, EventArgs e)
        {
            res = new ResourceManager("SteamProfiles.Resource.Add.Res", typeof(Settings).Assembly);
            Switch_language();
            using RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\SteamProfiles", true);
            if (key != null)
            {
                if (key?.GetValue("SteamPath") == null)
                {
                    Close();
                    SteamPath();
                }
                switch (key.GetValue("Mode")?.ToString())
                {
                    case "Dark":
                        Themes.ThemeChange(mode: true, this, Color.FromArgb(45, 45, 45), Color.FromArgb(35, 35, 35), Color.FromArgb(55, 55, 55));
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

        private void SteamPath()
        {
            using RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\SteamProfiles", true);
            if (key != null)
            {
                if (key.GetValue("SteamPath") == null)
                {
                    MessageBox.Show(SteamSettings);
                    Settings s = new Settings();
                    s.TopMost = true;
                    s.Show();
                }
            }
        }
    }
}