using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SteamProfiles.Forms
{
    public partial class AddProfile : Form
    {
        public AddProfile()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(textBox1.Text) && !string.IsNullOrWhiteSpace(textBox2.Text) && !string.IsNullOrWhiteSpace(textBox3.Text))
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey($@"Software\SteamProfiles\{textBox1.Text}", true))
                {
                    key.SetValue("UserName", Encriptor.Encypter(textBox1.Text));
                    key.SetValue("Login", Encriptor.Encypter(textBox2.Text));
                    key.SetValue("Password", Encriptor.Encypter(textBox3.Text));
                }
                textBox1.Clear();
                textBox2.Clear();
                textBox3.Clear();
                MessageBox.Show("Added Successfully");
            }
            else
            {
                MessageBox.Show("Fill the fields");
            }

        }

        private void AddProfile_Load(object sender, EventArgs e)
        {

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
                    GetAllControls.ThemeChange(mode: true, this, Color.FromArgb(0, 0, 80), Color.FromArgb(0, 0, 75));
                    button1.BackColor = Color.FromArgb(0, 0, 50);
                }
            }
        }
    }
}
