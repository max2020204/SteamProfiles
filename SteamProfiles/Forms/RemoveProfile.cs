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
    public partial class RemoveProfile : Form
    {
        public RemoveProfile()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(textBox1.Text))
            {
                DialogResult dialogResult = MessageBox.Show($"Are you sure you want delete {textBox1.Text}", $"Deleting {textBox1.Text}", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    try
                    {
                        using (RegistryKey remove = Registry.CurrentUser.OpenSubKey($@"Software\SteamProfiles\{textBox1.Text}",true))
                        {
                            if (remove != null)
                            {
                                Registry.CurrentUser.DeleteSubKey($@"Software\SteamProfiles\{textBox1.Text}");
                                new Form1().Updates();
                                MessageBox.Show("Removed Successfully");

                            }
                            else
                            {
                                MessageBox.Show("User not found");
                            }
                        }


                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }
    }
}
