using MetroFramework.Controls;
using MetroFramework.Forms;
using Microsoft.Win32;
using Octokit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ToggleSlider;
using Application = System.Windows.Forms.Application;

namespace SteamProfiles.Forms
{
    public partial class Settings : Form
    {
        bool start = false;
        ResourceManager res;
        string AutoFindError, AutoFindResult, ToggleOff, ToggleOn, SteamPath,
            RestartRequired, RestartText, LastVersion, NewVersion, NewVersionAsk,
            CurrentVersion, End;
        string regestyLang;
        readonly string[] standparh = new string[] { @"C:\Program Files (x86)", @"C:\Steam" };
        public Settings()
        {
            SelectLanguage.Lang();
            InitializeComponent();
        }

        void Switch_language()
        {
            AutoFindError = res.GetString("AutoFindError");
            AutoFindResult = res.GetString("AutoFindResult");
            ToggleOff = res.GetString("ToggleOff");
            ToggleOn = res.GetString("ToggleOn");
            SteamPath = res.GetString("SteamPath");
            RestartRequired = res.GetString("RestartRequired");
            RestartText = res.GetString("RestartText");
            LastVersion = res.GetString("LastVersion");
            NewVersion = res.GetString("NewVersion");
            NewVersionAsk = res.GetString("NewVersionAsk");
            CurrentVersion = res.GetString("CurrentVersion");
            End = res.GetString("End");
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

            res = new ResourceManager("SteamProfiles.Resource.Settings.Res", typeof(Settings).Assembly);
            Switch_language();
            ThemeMode();
            if (!Check())
            {
                RegisterInStartup(checkBox1.Checked);
            }
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\SteamProfiles"))
            {
                textBox1.Text = key?.GetValue("SteamPath")?.ToString();
                regestyLang = key?.GetValue("Language")?.ToString();
            }
            foreach (string drive in Directory.GetLogicalDrives())
            {
                comboBox1.Items.Add(drive);
            }
            comboBox1.SelectedItem = comboBox1.Items[0];
            if (regestyLang == "Русский")
            {
                comboBox2.SelectedItem = comboBox2.Items[1];
                start = true;
            }
            else if (regestyLang == "English")
            {
                comboBox2.SelectedItem = comboBox2.Items[0];
                start = true;
            }

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
                registryKey.SetValue("SteamProfiles", System.Windows.Forms.Application.ExecutablePath + " -silent");
            }
            else
            {
                if (registryKey.GetValue("SteamProfiles") != null)
                {
                    registryKey.DeleteValue("SteamProfiles");
                }

            }
        }
        private void Button3_Click(object sender, EventArgs e)
        {
            CheckUpdate();
        }
        async void CheckUpdate()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            string version = fvi.ProductVersion;
            GitHubClient github = new GitHubClient(new ProductHeaderValue("SteamProfiles"));
            var user = await github.Repository.Release.GetAll("max2020204", "SteamProfiles");
            string tag = user[0].TagName;
            switch (tag.CompareTo(version))
            {
                case 0:
                    MessageBox.Show(LastVersion + " " + version);
                    break;
                case 1:
                    DialogResult result = MessageBox.Show($"{NewVersionAsk} {tag} {End}\n{CurrentVersion} {version}", NewVersion, MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        string addres = $@"https://github.com/max2020204/SteamProfiles/releases/download/{tag}/SteamProfiles-{tag}.zip";
                        using (WebClient web = new WebClient())
                        {

                            web.DownloadFile(addres, $"SteamProfiles-{tag}.zip");
                            using (ZipArchive zip = ZipFile.OpenRead($"SteamProfiles-{tag}.zip"))
                            {
                                zip.ExtractToDirectory("SteamProfiles");
                            }
                        }
                        File.Delete($"SteamProfiles-{tag}.zip");
                        using (StreamWriter sw = new StreamWriter("update.bat"))
                        {
                            sw.WriteLine("taskkill /F /IM SteamProfiles.exe");
                            sw.WriteLine(@"Xcopy %cd%\SteamProfiles %cd%  /E /H /C /I");
                            sw.WriteLine("RMDIR SteamProfiles /S /Q");
                            sw.WriteLine("start SteamProfiles.exe");
                        }
                        Process process = Process.Start("update.bat");
                        process.WaitForExit();

                    }
                    break;
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
            bool spath = false;
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                List<string> files = new List<string>();
                metroLabel3.Visible = true;
                button1.Enabled = false;
                button2.Enabled = false;
                if (comboBox1.SelectedItem.ToString() == @"C:\")
                {
                    for (int i = 0; i < standparh.Length; i++)
                    {
                        AddFiles(standparh[i], files);
                        foreach (var item in files)
                        {
                            if (item.Contains("steam.exe"))
                            {
                                DialogResult result = MessageBox.Show($"{AutoFindResult}\n{item}", SteamPath, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                                if (result == DialogResult.Yes)
                                {
                                    RegistrySetPath(item);
                                    textBox1.Text = item;
                                    spath = true;
                                    break;
                                }
                            }
                        }
                    }
                }
                if (spath == false)
                {
                    AddFiles(comboBox1.SelectedItem.ToString(), files);
                    foreach (var item in files)
                    {
                        if (item.Contains("steam.exe"))
                        {
                            DialogResult result = MessageBox.Show($"{AutoFindResult}\n{item}", SteamPath, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (result == DialogResult.Yes)
                            {
                                RegistrySetPath(item);
                                textBox1.Text = item;
                                break;
                            }
                        }
                    }
                }
                button1.Enabled = true;
                button2.Enabled = true;
                metroLabel3.Visible = false;
            }
            else
            {
                MessageBox.Show(AutoFindError);
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
                toggleSliderComponent1.ToggleBarText = ToggleOn;
                toggleSliderComponent1.Checked = true;
                BackColor = Color.FromArgb(28, 28, 28);
            }
            else if (key.GetValue("Mode")?.ToString() == "Light")
            {
                GetAllControls.ThemeChange(mode: true, this, Color.FromArgb(0, 0, 50), Color.FromArgb(0, 0, 75));
                toggleSliderComponent1.ToggleBarText = ToggleOff;
                toggleSliderComponent1.Checked = false;
                BackColor = Color.FromArgb(0, 0, 50);
            }
            else
            {
                key.SetValue("Mode", "Light");
            }
        }
        private void ToggleSliderComponent1_CheckChanged(object sender, EventArgs e)
        {
            using RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\SteamProfiles", true);
            if (toggleSliderComponent1.Checked)
            {
                toggleSliderComponent1.ToggleBarText = ToggleOn;
                GetAllControls.ThemeChange(toggleSliderComponent1.Checked, this, Color.FromArgb(45, 45, 45), Color.FromArgb(55, 55, 55));
                BackColor = Color.FromArgb(45, 45, 45);
                key.SetValue("Mode", "Dark");
            }
            else
            {
                toggleSliderComponent1.ToggleBarText = ToggleOff;
                GetAllControls.ThemeChange(toggleSliderComponent1.Checked, this, Color.FromArgb(0, 0, 50), Color.FromArgb(0, 0, 75));
                BackColor = Color.FromArgb(0, 0, 50);
                key.SetValue("Mode", "Light");
            }
        }

        private void ComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\SteamProfiles", true))
            {
                key.SetValue("Language", comboBox2.SelectedItem);

                if (start)
                {
                    DialogResult result = MessageBox.Show(RestartText, RestartRequired, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        key.SetValue("temp", "1");
                        Application.Restart();
                    }
                }
            }
        }
    }
}




