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

        private void button1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(textBox1.Text) && !string.IsNullOrWhiteSpace(textBox2.Text) && !string.IsNullOrWhiteSpace(textBox3.Text))
            {
                using (RegistryKey key = Registry.CurrentUser.CreateSubKey($@"Software\SteamProfiles\{textBox1.Text}"))
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

        private void AddProfile_FormClosing(object sender, FormClosingEventArgs e)
        {
            new Form1().Updates();
        }
    }
}
