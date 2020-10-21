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
                    user = textBox1.Text.Replace(" ","");
                if (textBox2.Text.Contains(' '))
                    login = textBox2.Text.Replace(" ", "");
                if (textBox3.Text.Contains(' '))
                    pass = textBox3.Text.Replace(" ", "");
                using (RegistryKey key = Registry.CurrentUser.CreateSubKey($@"Software\SteamProfiles\{login}"))
                {
                    user = Encriptor.Encypter(user);
                    login = Encriptor.Encypter(login);
                    pass = Encriptor.Encypter(pass);
                    if (user != "")
                        key.SetValue("UserName", user);
                    else
                        MessageBox.Show(UserNameError);
                    if (login != "")
                        key.SetValue("Login", login);
                    else
                        MessageBox.Show(LoginError);
                    if (pass != "")
                        key.SetValue("Password", pass);
                    else
                        MessageBox.Show(PasswordError);
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
                if (key.GetValue("Mode")?.ToString() == "Dark")
                {
                    GetAllControls.ThemeChange(mode: true, this, Color.FromArgb(45, 45, 45), Color.FromArgb(55, 55, 55));
                    button1.BackColor = Color.FromArgb(28, 28, 28);
                }
                else if (key.GetValue("Mode")?.ToString() == "Light")
                {
                    GetAllControls.ThemeChange(mode: true, this, Color.FromArgb(0, 0, 80), Color.FromArgb(0, 0, 75));
                    button1.BackColor = Color.FromArgb(0, 0, 50);
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
