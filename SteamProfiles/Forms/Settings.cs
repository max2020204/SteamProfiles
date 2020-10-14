using MetroFramework.Controls;
using MetroFramework.Forms;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ToggleSlider;

namespace SteamProfiles.Forms
{
    public partial class Settings : Form
    {
        public Settings()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            using OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Executable files (*.exe)|*.exe"
            };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                RegistrySetPath(openFileDialog.FileName);
            }
            textBox1.Text = openFileDialog.FileName;
        }
        void RegistrySetPath(string path)
        {
            using RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\SteamProfiles", true);
            bool temp = true;
            while (temp)
            {
                if (key != null)
                {
                    if (key.GetValue("SteamPath") == null)
                    {
                        key.SetValue("SteamPath", path);
                    }
                    else
                    {
                        temp = false;
                    }
                }
                else
                {
                    Registry.CurrentUser.CreateSubKey(@"Software\SteamProfiles");
                }
            }
        }

        private void Settings_Load(object sender, EventArgs e)
        {
            ThemeMode();
            if (!Check())
            {
                RegisterInStartup(checkBox1.Checked);
            }
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\SteamProfiles"))
            {
                textBox1.Text = key?.GetValue("SteamPath")?.ToString();
            }
            foreach (string drive in Directory.GetLogicalDrives())
            {
                comboBox1.Items.Add(drive);
            }
            comboBox1.SelectedItem = comboBox1.Items[0];
        }

        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            RegisterInStartup(checkBox1.Checked);
        }
        private void RegisterInStartup(bool isChecked)
        {
            using RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
            if (isChecked)
            {
                registryKey.SetValue("SteamProfiles", Application.ExecutablePath + " -silent");
            }
            else
            {
                if (registryKey.GetValue("SteamProfiles") != null)
                {
                    registryKey.DeleteValue("SteamProfiles");
                }

            }
        }
        bool Check()
        {
            using RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run");
            if (key != null)
            {
                if (key.GetValue("SteamProfiles") != null)
                {
                    checkBox1.Checked = true;
                    return true;
                }
                else
                {
                    checkBox1.Checked = false;
                    return false;
                }
            }
            else
            {
                checkBox1.Checked = false;
                return false;
            }
        }
        private void Button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                List<string> files = new List<string>();
                metroLabel3.Visible = true;
                button1.Enabled = false;
                button2.Enabled = false;
                AddFiles(comboBox1.SelectedItem.ToString(), files);
                foreach (var item in files)
                {
                    if (item.Contains("steam.exe"))
                    {
                        DialogResult result = MessageBox.Show($"This is the right path?\n{item}", "Steam path", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (result == DialogResult.Yes)
                        {
                            RegistrySetPath(item);
                            textBox1.Text = item;
                            button1.Enabled = true;
                            button2.Enabled = true;
                            metroLabel3.Visible = false;
                            break;
                        }
                    }
                }
                button1.Enabled = true;
                button2.Enabled = true;
                metroLabel3.Visible = false;
            }
            else
            {
                MessageBox.Show("Path has already been found");
            }
        }
        private static void AddFiles(string path, IList<string> files)
        {
            var t = Task.Run(() =>
            {
                try
                {
                    Directory.GetFiles(path)
                        .ToList()
                        .ForEach(s => files.Add(s));

                    Directory.GetDirectories(path)
                        .ToList()
                        .ForEach(s => AddFiles(s, files));
                }
                catch (UnauthorizedAccessException)
                {

                }
            });
            t.Wait();
        }
        void ThemeMode()
        {
            using RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\SteamProfiles", true);
            if (key.GetValue("Mode")?.ToString() == "Dark")
            {
                GetAllControls.ThemeChange(mode: true, this, Color.FromArgb(45, 45, 45), Color.FromArgb(55, 55, 55));
                toggleSliderComponent1.ToggleBarText = "On";
                toggleSliderComponent1.Checked = true;
                BackColor = Color.FromArgb(28, 28, 28);
            }
            else if (key.GetValue("Mode")?.ToString() == "Light")
            {
                GetAllControls.ThemeChange(mode: true, this, Color.FromArgb(0, 0, 50), Color.FromArgb(0, 0, 75));
                toggleSliderComponent1.ToggleBarText = "Off";
                toggleSliderComponent1.Checked = false;
                BackColor = Color.FromArgb(0, 0, 50);
            }
        }
        private void ToggleSliderComponent1_CheckChanged(object sender, EventArgs e)
        {
            using RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\SteamProfiles", true);
            if (toggleSliderComponent1.Checked)
            {
                toggleSliderComponent1.ToggleBarText = "On";
                GetAllControls.ThemeChange(toggleSliderComponent1.Checked, this, Color.FromArgb(45, 45, 45), Color.FromArgb(55, 55, 55));
                BackColor = Color.FromArgb(45, 45, 45);
                key.SetValue("Mode", "Dark");
            }
            else
            {
                toggleSliderComponent1.ToggleBarText = "Off";
                GetAllControls.ThemeChange(toggleSliderComponent1.Checked, this, Color.FromArgb(0, 0, 50), Color.FromArgb(0, 0, 75));
                BackColor = Color.FromArgb(0, 0, 50);
                key.SetValue("Mode", "Light");
            }
        }


    }
}



