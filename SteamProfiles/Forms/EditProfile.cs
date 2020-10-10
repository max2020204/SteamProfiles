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
    public partial class EditProfile : Form
    {
        public EditProfile()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(textBox1.Text))
            {
                if (!string.IsNullOrWhiteSpace(textBox2.Text) && string.IsNullOrWhiteSpace(textBox3.Text))
                {
                    using (RegistryKey reg = Registry.CurrentUser.OpenSubKey($@"SOFTWARE\SteamProfiles\{textBox1.Text}", true))
                    {
                        if (reg != null)
                        {
                            if (Registry.CurrentUser.OpenSubKey($@"SOFTWARE\SteamProfiles\{textBox2.Text}") == null)
                            {
                                string login = Encriptor.Decypter(reg.GetValue("Login").ToString());
                                string password = Encriptor.Decypter(reg.GetValue("Password").ToString());
                                Registry.CurrentUser.DeleteSubKey($@"SOFTWARE\SteamProfiles\{textBox1.Text}");
                                Registry.CurrentUser.CreateSubKey($@"SOFTWARE\SteamProfiles\{textBox2.Text}");
                                Registry.CurrentUser.OpenSubKey($@"SOFTWARE\SteamProfiles\{textBox2.Text}", true).SetValue("UserName", Encriptor.Encypter(textBox2.Text));
                                Registry.CurrentUser.OpenSubKey($@"SOFTWARE\SteamProfiles\{textBox2.Text}", true).SetValue("Login", Encriptor.Encypter(login));
                                Registry.CurrentUser.OpenSubKey($@"SOFTWARE\SteamProfiles\{textBox2.Text}", true).SetValue("Password", Encriptor.Encypter(password));
                                MessageBox.Show("Done");
                            }
                        }
                    }
                }
                if (!string.IsNullOrWhiteSpace(textBox2.Text) && !string.IsNullOrWhiteSpace(textBox3.Text))
                {
                    using (RegistryKey reg = Registry.CurrentUser.OpenSubKey($@"SOFTWARE\SteamProfiles\{textBox1.Text}", true))
                    {
                        if (reg != null)
                        {
                            string login = Encriptor.Decypter(reg.GetValue("Login").ToString());
                            Registry.CurrentUser.DeleteSubKey($@"SOFTWARE\SteamProfiles\{textBox1.Text}");
                            Registry.CurrentUser.CreateSubKey($@"SOFTWARE\SteamProfiles\{textBox2.Text}");
                            Registry.CurrentUser.OpenSubKey($@"SOFTWARE\SteamProfiles\{textBox2.Text}", true).SetValue("UserName", Encriptor.Encypter(textBox2.Text));
                            Registry.CurrentUser.OpenSubKey($@"SOFTWARE\SteamProfiles\{textBox2.Text}", true).SetValue("Login", Encriptor.Encypter(login));
                            Registry.CurrentUser.OpenSubKey($@"SOFTWARE\SteamProfiles\{textBox2.Text}", true).SetValue("Password", Encriptor.Encypter(textBox3.Text));
                            MessageBox.Show("Done");

                        }
                    }
                }
                if (!string.IsNullOrWhiteSpace(textBox3.Text) && string.IsNullOrWhiteSpace(textBox2.Text))
                {
                    using (RegistryKey reg = Registry.CurrentUser.OpenSubKey($@"SOFTWARE\SteamProfiles\{textBox1.Text}", true))
                    {
                        if (reg != null)
                        {
                            reg.DeleteValue("Password");
                            reg.SetValue("Password", Encriptor.Encypter(textBox3.Text));
                            MessageBox.Show("Done");
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Fill your username");
            }
        }
    }
}
