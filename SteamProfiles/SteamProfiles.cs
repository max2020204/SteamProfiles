﻿using Microsoft.Win32;
using Newtonsoft.Json;
using SteamProfiles.ChangeMenuStrip;
using SteamProfiles.Forms;
using SteamProfiles.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Threading;
using System.Windows.Forms;

namespace SteamProfiles
{
    // SteamProfiles Copyright (C) 2020  Maksym Nikitiuk
    public partial class SteamProfiles : Form
    {
        ResourceManager res;
        string SteamSettings, SuccessRemove, Dublicate;
        StyleSettings settings;
        public SteamProfiles()
        {
            Lang();
            Runing();
            InitializeComponent();
        }
        void Runing()
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\SteamProfiles", true))
            {
                if (key.GetValue("temp") == null)
                {
                    if (Process.GetProcesses().Count(x => x.ProcessName == "SteamProfiles") > 1)
                    {
                        MessageBox.Show(Dublicate);
                        Process.GetCurrentProcess().Kill();
                    }
                }
                else
                {
                    key?.DeleteValue("temp");
                }
            }
        }
        void Switch_language()
        {
            SteamSettings = res.GetString("SteamSettings");
            SuccessRemove = res.GetString("SuccessRemove");
            Dublicate = res.GetString("Dublicate");

        }
        void Lang()
        {
            res = new ResourceManager("SteamProfiles.Resource.SteamProfiles.Res", typeof(SteamProfiles).Assembly);
            using (RegistryKey lang = Registry.CurrentUser.OpenSubKey(@"Software\SteamProfiles", true))
            {
                if (lang?.GetValue("Language") != null)
                {
                    if (lang.GetValue("Language").ToString() == "English")
                    {
                        Thread.CurrentThread.CurrentUICulture = new CultureInfo("en");
                        Switch_language();
                    }
                    else if (lang.GetValue("Language").ToString() == "Русский")
                    {
                        Thread.CurrentThread.CurrentUICulture = new CultureInfo("ru");
                        Switch_language();
                    }
                }
                else
                {
                    Registry.CurrentUser.CreateSubKey(@"Software\SteamProfiles").SetValue("Language", "English"); 
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo("en");
                    Switch_language();
                }
            }
        }
        void FlatApperence(Color MouseDownBackColor)
        {
            List<Control> availControls = GetAllControls.GetControls(this);
            foreach (var item in availControls)
            {
                if (item is Button button)
                {
                    button.FlatAppearance.MouseDownBackColor = MouseDownBackColor;
                }
            }
        }
        void ContextMenuTheme(Color BackColor)
        {
            for (int i = 0; i < gridmenustrip.Items.Count; i++)
            {
                gridmenustrip.Items[i].BackColor = BackColor;
                ToolStripMenuItem items = gridmenustrip.Items[i] as ToolStripMenuItem;
                for (int j = 0; j < items.DropDownItems.Count; j++)
                {
                    items.DropDownItems[j].BackColor = BackColor;
                    ToolStripMenuItem DropedItems = items;
                    if (DropedItems.DropDownItems.Count > 0)
                    {
                        for (int k = 0; k < DropedItems.DropDownItems.Count; k++)
                        {
                            DropedItems.DropDownItems[k].BackColor = BackColor;
                        }
                    }
                }
            }
            for (int i = 0; i < notifymenustrip.Items.Count; i++)
            {
                notifymenustrip.Items[i].BackColor = BackColor;
            }
            showToolStripMenuItem.BackColor = BackColor;
            hideToolStripMenuItem.BackColor = BackColor;
        }
        bool ThemeMode()
        {
            using RegistryKey mode = Registry.CurrentUser.OpenSubKey(@"Software\SteamProfiles", true);
            if (mode != null)
            {
                if (mode.GetValue("Mode")?.ToString() == "Dark")
                {
                    panel1.BackColor = Color.FromArgb(28, 28, 28);
                    button3.BackColor = Color.FromArgb(28, 28, 28);
                    pictureBox1.BackColor = Color.FromArgb(28, 28, 28);
                    metroGrid1.BackgroundColor = Color.FromArgb(45, 45, 45);
                    metroGrid1.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(35, 35, 35);
                    metroGrid1.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(45, 45, 45);
                    metroGrid1.DefaultCellStyle.BackColor = Color.FromArgb(45, 45, 45);
                    metroGrid1.DefaultCellStyle.SelectionBackColor = Color.FromArgb(55, 55, 55);
                    metroGrid1.RowHeadersDefaultCellStyle.BackColor = Color.FromArgb(45, 45, 45);
                    metroGrid1.RowHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(55, 55, 55);
                    FlatApperence(Color.FromArgb(55, 55, 55));
                    gridmenustrip.Renderer = new ChangeMunuStripDark();
                    notifymenustrip.Renderer = new ChangeMunuStripDark();
                    ContextMenuTheme(Color.FromArgb(45, 45, 45));
                    return true;
                }
                else if (mode.GetValue("Mode")?.ToString() == "Light")
                {
                    panel1.BackColor = Color.FromArgb(0, 0, 50);
                    button3.BackColor = Color.FromArgb(0, 0, 50);
                    pictureBox1.BackColor = Color.FromArgb(0, 0, 50);
                    metroGrid1.BackgroundColor = Color.FromArgb(0, 0, 80);
                    metroGrid1.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(0, 0, 70);
                    metroGrid1.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(0, 0, 100);
                    metroGrid1.DefaultCellStyle.BackColor = Color.FromArgb(0, 0, 80);
                    metroGrid1.DefaultCellStyle.SelectionBackColor = Color.FromArgb(0, 0, 100);
                    metroGrid1.RowHeadersDefaultCellStyle.BackColor = Color.FromArgb(0, 0, 80);
                    metroGrid1.RowHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(0, 0, 100);
                    FlatApperence(Color.FromArgb(0, 0, 75));
                    gridmenustrip.Renderer = new ChangeMunuStripLight();
                    notifymenustrip.Renderer = new ChangeMunuStripLight();
                    ContextMenuTheme(Color.FromArgb(0, 0, 80));
                    return false;
                }

            }
            return false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (!ThemeMode())
            {
                gridmenustrip.Renderer = new ChangeMunuStripLight();
                notifymenustrip.Renderer = new ChangeMunuStripLight();
            }
            else
            {
                gridmenustrip.Renderer = new ChangeMunuStripDark();
                notifymenustrip.Renderer = new ChangeMunuStripDark();
            }
            foreach (string s in Environment.GetCommandLineArgs())
            {
                MinimizeApp(s);
            }
            Updates();
            notifyIcon1.Text = "SteamProfiles";
            notifyIcon1.Visible = true;
            using RegistryKey style = Registry.CurrentUser.OpenSubKey(@"Software\SteamProfiles", true);
            if (style != null)
            {
                if (style.GetValue("Style") != null)
                {
                    settings = JsonConvert.DeserializeObject<StyleSettings>(style.GetValue("Style").ToString());
                    metroGrid1.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(settings.HeaderTextColor.HeaderTextColorR, settings.HeaderTextColor.HeaderTextColorG, settings.HeaderTextColor.HeaderTextColorB);
                    metroGrid1.ColumnHeadersDefaultCellStyle.Font = settings.HeaderCellSize;
                    metroGrid1.DefaultCellStyle.ForeColor = Color.FromArgb(settings.CellTextColor.CellTextColorR, settings.CellTextColor.CellTextColorG, settings.CellTextColor.CellTextColorB);
                    metroGrid1.DefaultCellStyle.Font = settings.CellSize;
                }
                if (style.GetValue("Mode")?.ToString() == "Dark")
                {
                    panel1.BackColor = Color.FromArgb(28, 28, 28);
                    button3.BackColor = Color.FromArgb(28, 28, 28);
                    pictureBox1.BackColor = Color.FromArgb(28, 28, 28);
                    metroGrid1.BackgroundColor = Color.FromArgb(45, 45, 45);
                    metroGrid1.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(35, 35, 35);
                    metroGrid1.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(45, 45, 45);
                    metroGrid1.DefaultCellStyle.BackColor = Color.FromArgb(45, 45, 45);
                    metroGrid1.DefaultCellStyle.SelectionBackColor = Color.FromArgb(55, 55, 55);
                    metroGrid1.RowHeadersDefaultCellStyle.BackColor = Color.FromArgb(45, 45, 45);
                    metroGrid1.RowHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(55, 55, 55);

                }
                else if (style.GetValue("Mode")?.ToString() == "Light")
                {
                    panel1.BackColor = Color.FromArgb(0, 0, 50);
                    button3.BackColor = Color.FromArgb(0, 0, 50);
                    pictureBox1.BackColor = Color.FromArgb(0, 0, 50);
                    metroGrid1.BackgroundColor = Color.FromArgb(0, 0, 80);
                    metroGrid1.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(0, 0, 70);
                    metroGrid1.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(0, 0, 100);
                    metroGrid1.DefaultCellStyle.BackColor = Color.FromArgb(0, 0, 80);
                    metroGrid1.DefaultCellStyle.SelectionBackColor = Color.FromArgb(0, 0, 100);
                    metroGrid1.RowHeadersDefaultCellStyle.BackColor = Color.FromArgb(0, 0, 80);
                    metroGrid1.RowHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(0, 0, 100);
                }

            }
        }
        public void MinimizeApp(string parameter)
        {
            if (parameter == "-silent")
            {
                this.WindowState = FormWindowState.Minimized;
                notifyIcon1.Visible = true;
                ShowInTaskbar = false;
                ThemeMode();
                Hide();
            }
        }
        void Save()
        {
            var style = new StyleSettings
            {
                HeaderTextColor = new HeaderTextColor
                {
                    HeaderTextColorR = metroGrid1.ColumnHeadersDefaultCellStyle.ForeColor.R,
                    HeaderTextColorG = metroGrid1.ColumnHeadersDefaultCellStyle.ForeColor.G,
                    HeaderTextColorB = metroGrid1.ColumnHeadersDefaultCellStyle.ForeColor.B
                },
                CellTextColor = new CellTextColor
                {
                    CellTextColorR = metroGrid1.DefaultCellStyle.ForeColor.R,
                    CellTextColorG = metroGrid1.DefaultCellStyle.ForeColor.G,
                    CellTextColorB = metroGrid1.DefaultCellStyle.ForeColor.B
                },
                HeaderCellSize = metroGrid1.ColumnHeadersDefaultCellStyle.Font,
                CellSize = metroGrid1.DefaultCellStyle.Font
            };
            using RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\SteamProfiles", true);
            try
            {
                key.SetValue("Style", JsonConvert.SerializeObject(style));
            }
            catch (Exception)
            {


            }
        }
        private void NotifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Show();
            notifyIcon1.Visible = false;
            WindowState = FormWindowState.Normal;
        }
        private void Form1_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                Hide();
                notifyIcon1.Visible = true;
            }
            else if (WindowState == FormWindowState.Normal)
            {
                notifyIcon1.Visible = false;
            }
        }
        void SteamPath()
        {
            using RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\SteamProfiles", true);
            if (key != null)
            {
                if (key.GetValue("SteamPath") == null)
                {
                    MessageBox.Show(SteamSettings);
                    new Settings().Show();
                }
            }
        }
        public void Updates(bool password = false)
        {
            metroGrid1.DataSource = null;
            metroGrid1.Rows.Clear();
            using RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\SteamProfiles\");
            if (key != null)
            {
                string[] subkeys = key.GetSubKeyNames();
                if (subkeys.Length > 0)
                {
                    metroGrid1.Rows.Add(subkeys.Length + 1);
                    for (int i = 0; i < subkeys.Length; i++)
                    {
                        using RegistryKey k = Registry.CurrentUser.OpenSubKey($@"Software\SteamProfiles\{subkeys[i]}");
                        string[] subvalues = k.GetValueNames();
                        if (subvalues.Length > 0)
                        {
                            metroGrid1.Rows[i].Cells["UserName"].Value = Encriptor.Decypter(k.GetValue(subvalues.Where(x => x.Contains("UserName")).FirstOrDefault()).ToString());

                            metroGrid1.Rows[i].Cells["Login"].Value = Encriptor.Decypter(k.GetValue(subvalues.Where(x => x.Contains("Login")).FirstOrDefault()).ToString());
                            if (password)
                            {
                                metroGrid1.Rows[i].Cells["Password"].Value = Encriptor.Decypter(k.GetValue(subvalues.Where(x => x.Contains("Password")).FirstOrDefault()).ToString());
                            }
                            else
                            {
                                metroGrid1.Rows[i].Cells["Password"].Value = new string('*', Encriptor.Decypter(k.GetValue(subvalues.Where(x => x.Contains("Password")).FirstOrDefault()).ToString()).Length);
                            }
                            CreateSubMenu(Encriptor.Decypter(k.GetValue(subvalues.Where(x => x.Contains("Login")).FirstOrDefault()).ToString()),
                                Encriptor.Decypter(k.GetValue(subvalues.Where(x => x.Contains("Password")).FirstOrDefault()).ToString()));
                        }
                    }
                    for (int i = 0; i < metroGrid1.RowCount - 1; i++)
                    {

                        for (int j = 0; j < metroGrid1.ColumnCount; j++)
                        {
                            if (i < 0)
                            {
                                break;
                            }
                            if (metroGrid1.Rows[i].Cells[j].Value == null || metroGrid1.Rows[i].Cells[j].Value.ToString() == "")
                            {
                                metroGrid1.Rows.RemoveAt(i);
                                i--;
                            }
                        }
                    }
                    UpdateValue();
                }
            }
            else
            {
                Registry.CurrentUser.CreateSubKey($@"Software\SteamProfiles");
                SteamPath();
            }
        }
        void UpdateValue()
        {
            for (int i = 0; i < metroGrid1.ColumnCount; i++)
            {
                for (int j = 0; j < metroGrid1.RowCount; j++)
                {
                    metroGrid1.UpdateCellValue(i, j);
                }
            }
        }
        void CreateSubMenu(string login, string password)
        {
            string mode = "";
            string steam = "";
            ToolStripMenuItem ToolStrip = new ToolStripMenuItem(login)
            {
                Text = login
            };
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\SteamProfiles"))
            {
                mode = key?.GetValue("Mode")?.ToString();
                steam = key.GetValue("SteamPath").ToString();
            }
            ToolStrip.Click += (s, a) =>
            {
                ProcessStartInfo start = new ProcessStartInfo
                {
                    WindowStyle = ProcessWindowStyle.Hidden,
                    FileName = "cmd",
                    Arguments = "/c taskkill /F /IM Steam.exe"
                };
                Process.Start(start);
                Thread.Sleep(100);

                Process p = new Process();

                p.StartInfo.FileName = steam;
                p.StartInfo.Arguments = $"-login {login} {password}";
                p.Start();
            };
            if (mode == "Dark")
            {
                ToolStrip.BackColor = Color.FromArgb(45, 45, 45);
            }
            ToolStrip.ForeColor = Color.White;
            ToolStrip.CheckOnClick = true;
            bool check = false;

            for (int i = 0; i < notifymenustrip.Items.Count; i++)
            {
                if (notifymenustrip.Items[i].Text == ToolStrip.Text)
                {
                    check = true;
                }
            }
            if (!check)
            {
                notifymenustrip.Items.Add(ToolStrip);
            }

        }
        private void AddToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            new AddProfile().Show();
        }

        private void EditToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            new EditProfile().Show();
        }

        private void DeleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RemoveRow(metroGrid1);
        }
        void RemoveRow(DataGridView metroGrid1)
        {
            foreach (DataGridViewRow row in metroGrid1.SelectedRows)
            {
                try
                {
                    Registry.CurrentUser.DeleteSubKey($@"Software\SteamProfiles\{row.Cells[1].Value?.ToString().Replace(" ", "")}");
                    MessageBox.Show(SuccessRemove);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                try
                {
                    metroGrid1.Rows.Remove(row);
                }
                catch (Exception)
                {
                }

            }
        }

        private void ToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            metroGrid1.ColumnHeadersDefaultCellStyle.Font = new Font(metroGrid1.ColumnHeadersDefaultCellStyle.Font.FontFamily, 9f);
        }

        private void ToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            metroGrid1.ColumnHeadersDefaultCellStyle.Font = new Font(metroGrid1.ColumnHeadersDefaultCellStyle.Font.FontFamily, 10f);
        }

        private void ToolStripMenuItem4_Click(object sender, EventArgs e)
        {
            metroGrid1.ColumnHeadersDefaultCellStyle.Font = new Font(metroGrid1.ColumnHeadersDefaultCellStyle.Font.FontFamily, 11f);
        }

        private void ToolStripMenuItem5_Click(object sender, EventArgs e)
        {
            metroGrid1.ColumnHeadersDefaultCellStyle.Font = new Font(metroGrid1.ColumnHeadersDefaultCellStyle.Font.FontFamily, 12f);
        }

        private void ToolStripMenuItem6_Click(object sender, EventArgs e)
        {
            metroGrid1.ColumnHeadersDefaultCellStyle.Font = new Font(metroGrid1.ColumnHeadersDefaultCellStyle.Font.FontFamily, 14f);
        }

        private void ToolStripMenuItem7_Click(object sender, EventArgs e)
        {
            metroGrid1.ColumnHeadersDefaultCellStyle.Font = new Font(metroGrid1.ColumnHeadersDefaultCellStyle.Font.FontFamily, 16f);
        }

        private void ToolStripMenuItem8_Click(object sender, EventArgs e)
        {
            metroGrid1.ColumnHeadersDefaultCellStyle.Font = new Font(metroGrid1.ColumnHeadersDefaultCellStyle.Font.FontFamily, 18f);
        }

        private void ToolStripMenuItem9_Click(object sender, EventArgs e)
        {
            metroGrid1.ColumnHeadersDefaultCellStyle.Font = new Font(metroGrid1.ColumnHeadersDefaultCellStyle.Font.FontFamily, 20f);
        }

        private void ToolStripMenuItem10_Click(object sender, EventArgs e)
        {
            metroGrid1.ColumnHeadersDefaultCellStyle.Font = new Font(metroGrid1.ColumnHeadersDefaultCellStyle.Font.FontFamily, 22f);
        }

        private void ToolStripMenuItem11_Click(object sender, EventArgs e)
        {
            metroGrid1.DefaultCellStyle.Font = new Font(metroGrid1.ColumnHeadersDefaultCellStyle.Font.FontFamily, 9f);
        }

        private void ToolStripMenuItem12_Click(object sender, EventArgs e)
        {
            metroGrid1.DefaultCellStyle.Font = new Font(metroGrid1.ColumnHeadersDefaultCellStyle.Font.FontFamily, 10f);
        }

        private void ToolStripMenuItem13_Click(object sender, EventArgs e)
        {
            metroGrid1.DefaultCellStyle.Font = new Font(metroGrid1.ColumnHeadersDefaultCellStyle.Font.FontFamily, 11f);
        }

        private void ToolStripMenuItem14_Click(object sender, EventArgs e)
        {
            metroGrid1.DefaultCellStyle.Font = new Font(metroGrid1.ColumnHeadersDefaultCellStyle.Font.FontFamily, 12f);
        }

        private void ToolStripMenuItem15_Click(object sender, EventArgs e)
        {
            metroGrid1.DefaultCellStyle.Font = new Font(metroGrid1.ColumnHeadersDefaultCellStyle.Font.FontFamily, 14f);
        }

        private void ToolStripMenuItem16_Click(object sender, EventArgs e)
        {
            metroGrid1.DefaultCellStyle.Font = new Font(metroGrid1.ColumnHeadersDefaultCellStyle.Font.FontFamily, 16f);
        }

        private void ToolStripMenuItem17_Click(object sender, EventArgs e)
        {
            metroGrid1.DefaultCellStyle.Font = new Font(metroGrid1.ColumnHeadersDefaultCellStyle.Font.FontFamily, 18f);
        }

        private void ToolStripMenuItem18_Click(object sender, EventArgs e)
        {
            metroGrid1.DefaultCellStyle.Font = new Font(metroGrid1.ColumnHeadersDefaultCellStyle.Font.FontFamily, 20f);
        }

        private void ToolStripMenuItem19_Click(object sender, EventArgs e)
        {
            metroGrid1.DefaultCellStyle.Font = new Font(metroGrid1.ColumnHeadersDefaultCellStyle.Font.FontFamily, 22f);
        }
        private void ShowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Updates(true);
        }

        private void HideToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Updates();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            new AddProfile().Show();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            RemoveRow(metroGrid1);
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            new EditProfile().Show();
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            Updates();
        }

        private void Button6_Click(object sender, EventArgs e)
        {
            new Settings().Show();
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void UpdateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Updates();
        }

        private void WhiteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            metroGrid1.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
        }

        private void OwnColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() != DialogResult.Cancel)
            {
                metroGrid1.ColumnHeadersDefaultCellStyle.ForeColor = colorDialog1.Color;
            }
        }

        private void OwnColorToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() != DialogResult.Cancel)
            {
                metroGrid1.DefaultCellStyle.ForeColor = colorDialog1.Color;
            }
        }

        private void YellowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            metroGrid1.ColumnHeadersDefaultCellStyle.ForeColor = Color.Yellow;
        }
        private void GreenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            metroGrid1.ColumnHeadersDefaultCellStyle.ForeColor = Color.Green;
        }

        private void WhiteToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            metroGrid1.DefaultCellStyle.ForeColor = Color.White;
        }

        private void YellowToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            metroGrid1.DefaultCellStyle.ForeColor = Color.Yellow;
        }
        private void GreenToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            metroGrid1.DefaultCellStyle.ForeColor = Color.Green;
        }

        private void UpdateToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Updates();
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void Open_Click(object sender, EventArgs e)
        {
            Show();
        }

        private void MetroGrid1_ColumnHeadersDefaultCellStyleChanged(object sender, EventArgs e)
        {
            Save();
        }

        private void MetroGrid1_DefaultCellStyleChanged(object sender, EventArgs e)
        {
            Save();
        }

        private void ResetToDefualtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using RegistryKey style = Registry.CurrentUser.OpenSubKey(@"Software\SteamProfiles", true);
            if (style != null)
            {
                if (style.GetValue("Style") != null)
                {
                    Registry.CurrentUser.OpenSubKey(@"Software\SteamProfiles", true).DeleteValue("Style");
                    metroGrid1.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                    metroGrid1.DefaultCellStyle.ForeColor = Color.White;
                    metroGrid1.DefaultCellStyle.Font = new Font(metroGrid1.ColumnHeadersDefaultCellStyle.Font.FontFamily, 11f);
                    metroGrid1.ColumnHeadersDefaultCellStyle.Font = new Font(metroGrid1.ColumnHeadersDefaultCellStyle.Font.FontFamily, 11f);
                }
            }
        }
        private void RedToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            metroGrid1.ColumnHeadersDefaultCellStyle.ForeColor = Color.Red;
        }

        private void RedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            metroGrid1.DefaultCellStyle.ForeColor = Color.Red;
        }

        private void SteamProfiles_Activated(object sender, EventArgs e)
        {
            Updates();
            ThemeMode();
        }

        private void Button4_Click_1(object sender, EventArgs e)
        {
            new About().Show();
        }
    }
}

