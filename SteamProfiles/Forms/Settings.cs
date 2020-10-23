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
        Dictionary<string, string> lang = new Dictionary<string, string>();
        bool start = false;
        ResourceManager res;
        string AutoFindError, AutoFindResult, SteamPath,
        RestartRequired, RestartText, LastVersion, NewVersion, 
        NewVersionAsk,CurrentVersion, End,Dark,Light,OldSchool;
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
            SteamPath = res.GetString("SteamPath");
            RestartRequired = res.GetString("RestartRequired");
            RestartText = res.GetString("RestartText");
            LastVersion = res.GetString("LastVersion");
            NewVersion = res.GetString("NewVersion");
            NewVersionAsk = res.GetString("NewVersionAsk");
            CurrentVersion = res.GetString("CurrentVersion");
            End = res.GetString("End");
            Dark = res.GetString("Dark");
            Light = res.GetString("Light");
            OldSchool = res.GetString("OldSchool");
            lang.Add(Dark, "Dark");
            lang.Add(Light, "Light");
            lang.Add(OldSchool, "OldSchool");
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

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\SteamProfiles", true))
            {
                if (key != null)
                {
                    foreach (var item in lang)
                    {
                        if (item.Key == Theme.SelectedItem.ToString())
                        {
                            switch (item.Value)
                            {
                                case "Dark":
                                    key.SetValue("Mode", "Dark");
                                    Theme.SelectedItem = "Dark";
                                    ThemeMode();
                                    break;
                                case "Light":
                                    key.SetValue("Mode", "Light");
                                    Theme.SelectedItem = "Light";
                                    ThemeMode();
                                    break;
                                case "OldSchool":
                                    key.SetValue("Mode", "OldSchool");
                                    Theme.SelectedItem = "OldSchool";
                                    ThemeMode();
                                    break;
                            }
                        }
                    }
                   
                }
            }
        }



        void AdditemsToTheme()
        {
            Theme.Items.Add(Dark);
            Theme.Items.Add(Light);
            Theme.Items.Add(OldSchool);
        }
        void ComboboxSlection()
        {
            
           
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\SteamProfiles"))
            {
                textBox1.Text = key?.GetValue("SteamPath")?.ToString();
                comboBox2.SelectedItem = key?.GetValue("Language")?.ToString();
                foreach (var item in lang)
                {
                    if (item.Value == key.GetValue("Mode").ToString())
                    {
                        Theme.SelectedItem = item.Key;
                        break;
                    }
                }      
            }
        }
        private void Settings_Load(object sender, EventArgs e)
        {
            res = new ResourceManager("SteamProfiles.Resource.Settings.Res", typeof(Settings).Assembly);
            Switch_language();
            ThemeMode();
            AdditemsToTheme();
            if (!Check())
            {
                RegisterInStartup(checkBox1.Checked);
            }
            foreach (string drive in Directory.GetLogicalDrives())
            {
                comboBox1.Items.Add(drive);
            }
            comboBox1.SelectedItem = comboBox1.Items[0];
            ComboboxSlection();
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
            switch (key.GetValue("Mode")?.ToString())
            {
                case "Dark":
                    Themes.ThemeChange(mode: true, this, Color.FromArgb(45, 45, 45), Color.FromArgb(45, 45, 45), Color.FromArgb(55, 55, 55));
                    BackColor = Color.FromArgb(45, 45, 45);
                    Themes.ChangeForeColor(true, this, Color.White);
                    break;
                case "Light":
                    Themes.ThemeChange(mode: true, form: this, backcolor: Color.FromArgb(189, 204, 212), buttoncolor: Color.FromArgb(189, 204, 212), MouseDownBackColor: Color.FromArgb(55, 55, 55));
                    BackColor = Color.FromArgb(189, 204, 212);
                    Themes.ChangeForeColor(true, this, Color.Black);
                    break;
                case "OldSchool":
                    Themes.ThemeChange(mode: true, this, Color.FromArgb(0, 0, 80), Color.FromArgb(0, 0, 80), Color.FromArgb(0, 0, 75));
                    BackColor = Color.FromArgb(0, 0, 80);
                    Themes.ChangeForeColor(true, this, Color.White);
                    break;
                default:
                    key.SetValue("Mode", "Light");
                    break;
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




