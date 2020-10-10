using MetroFramework.Forms;
using Microsoft.Win32;
using SteamProfiles.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SteamProfiles
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();

        }
        private void Form1_Load(object sender, EventArgs e)
        {
            foreach (string s in System.Environment.GetCommandLineArgs())
            {
                MinimizeApp(s);
            }
            Updates();
            notifyIcon1.Text = "SteamProfiles";
            notifyIcon1.Visible = true;
        }
        public void MinimizeApp(string parameter)
        {
            if (parameter == "-silent")
            {
                this.WindowState = FormWindowState.Minimized;
                notifyIcon1.Visible = true;
                ShowInTaskbar = false;
                Hide();
            }

        }
        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Show();
            notifyIcon1.Visible = false;
            WindowState = FormWindowState.Normal;
        }
        void SteamPath()
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\SteamProfiles", true))
            {
                if (key != null)
                {
                    if (key.GetValue("SteamPath") == null)
                    {
                        MessageBox.Show("Enter your Steam path");
                        new Settings().Show();
                    }
                }
            }
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
        public void Updates(bool password = false)
        {
            metroGrid1.DataSource = null;
            metroGrid1.Rows.Clear();
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\SteamProfiles\"))
            {
                if (key != null)
                {
                    string[] subkeys = key.GetSubKeyNames();
                    if (subkeys.Length > 0)
                    {

                        metroGrid1.Rows.Add(subkeys.Length + 1);
                        for (int i = 0; i < subkeys.Length; i++)
                        {
                            using (RegistryKey k = Registry.CurrentUser.OpenSubKey($@"Software\SteamProfiles\{subkeys[i]}"))
                            {
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
                                    CreateSubMenu(Encriptor.Decypter(k.GetValue(subvalues.Where(x => x.Contains("UserName")).FirstOrDefault()).ToString()),
                                        Encriptor.Decypter(k.GetValue(subvalues.Where(x => x.Contains("Login")).FirstOrDefault()).ToString()),
                                        Encriptor.Decypter(k.GetValue(subvalues.Where(x => x.Contains("Password")).FirstOrDefault()).ToString()));
                                }
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
                else if (key == null)
                {
                    Registry.CurrentUser.CreateSubKey($@"Software\SteamProfiles");
                    SteamPath();
                }
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
        void CreateSubMenu(string text, string login, string password)
        {

            ToolStripMenuItem ToolStrip = new ToolStripMenuItem(text);
            ToolStrip.Text = text;
            
            ToolStrip.Click += (s,a) =>
            {
                ProcessStartInfo start = new ProcessStartInfo();
                start.WindowStyle = ProcessWindowStyle.Hidden;
                start.FileName = "cmd";
                start.Arguments = "/c taskkill /F /IM Steam.exe";
                Process.Start(start);
                Thread.Sleep(100); 
                string steam = "";
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\SteamProfiles"))
                {
                    steam = key.GetValue("SteamPath").ToString();
                }
                Process p = new Process();
                
                p.StartInfo.FileName = steam;
                p.StartInfo.Arguments = $"-login {login} {password}";
                p.Start();
            };
            ToolStrip.CheckOnClick = true;
            notifymenustrip.Items.Add(ToolStrip);
        }

        private void OpenSteam(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void exitToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Show();
        }
        private void addToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            new AddProfile().Show();
        }

        private void editToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            new EditProfile().Show();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new RemoveProfile().Show();
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            metroGrid1.ColumnHeadersDefaultCellStyle.Font = new Font(metroGrid1.ColumnHeadersDefaultCellStyle.Font.FontFamily, 9f);
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            metroGrid1.ColumnHeadersDefaultCellStyle.Font = new Font(metroGrid1.ColumnHeadersDefaultCellStyle.Font.FontFamily, 10f);
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            metroGrid1.ColumnHeadersDefaultCellStyle.Font = new Font(metroGrid1.ColumnHeadersDefaultCellStyle.Font.FontFamily, 11f);
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            metroGrid1.ColumnHeadersDefaultCellStyle.Font = new Font(metroGrid1.ColumnHeadersDefaultCellStyle.Font.FontFamily, 12f);
        }

        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            metroGrid1.ColumnHeadersDefaultCellStyle.Font = new Font(metroGrid1.ColumnHeadersDefaultCellStyle.Font.FontFamily, 14f);
        }

        private void toolStripMenuItem7_Click(object sender, EventArgs e)
        {
            metroGrid1.ColumnHeadersDefaultCellStyle.Font = new Font(metroGrid1.ColumnHeadersDefaultCellStyle.Font.FontFamily, 16f);
        }

        private void toolStripMenuItem8_Click(object sender, EventArgs e)
        {
            metroGrid1.ColumnHeadersDefaultCellStyle.Font = new Font(metroGrid1.ColumnHeadersDefaultCellStyle.Font.FontFamily, 18f);
        }

        private void toolStripMenuItem9_Click(object sender, EventArgs e)
        {
            metroGrid1.ColumnHeadersDefaultCellStyle.Font = new Font(metroGrid1.ColumnHeadersDefaultCellStyle.Font.FontFamily, 20f);
        }

        private void toolStripMenuItem10_Click(object sender, EventArgs e)
        {
            metroGrid1.ColumnHeadersDefaultCellStyle.Font = new Font(metroGrid1.ColumnHeadersDefaultCellStyle.Font.FontFamily, 22f);
        }

        private void toolStripMenuItem11_Click(object sender, EventArgs e)
        {
            metroGrid1.DefaultCellStyle.Font = new Font(metroGrid1.ColumnHeadersDefaultCellStyle.Font.FontFamily, 9f);
        }

        private void toolStripMenuItem12_Click(object sender, EventArgs e)
        {
            metroGrid1.DefaultCellStyle.Font = new Font(metroGrid1.ColumnHeadersDefaultCellStyle.Font.FontFamily, 10f);
        }

        private void toolStripMenuItem13_Click(object sender, EventArgs e)
        {
            metroGrid1.DefaultCellStyle.Font = new Font(metroGrid1.ColumnHeadersDefaultCellStyle.Font.FontFamily, 11f);
        }

        private void toolStripMenuItem14_Click(object sender, EventArgs e)
        {
            metroGrid1.DefaultCellStyle.Font = new Font(metroGrid1.ColumnHeadersDefaultCellStyle.Font.FontFamily, 12f);
        }

        private void toolStripMenuItem15_Click(object sender, EventArgs e)
        {
            metroGrid1.DefaultCellStyle.Font = new Font(metroGrid1.ColumnHeadersDefaultCellStyle.Font.FontFamily, 14f);
        }

        private void toolStripMenuItem16_Click(object sender, EventArgs e)
        {
            metroGrid1.DefaultCellStyle.Font = new Font(metroGrid1.ColumnHeadersDefaultCellStyle.Font.FontFamily, 16f);
        }

        private void toolStripMenuItem17_Click(object sender, EventArgs e)
        {
            metroGrid1.DefaultCellStyle.Font = new Font(metroGrid1.ColumnHeadersDefaultCellStyle.Font.FontFamily, 18f);
        }

        private void toolStripMenuItem18_Click(object sender, EventArgs e)
        {
            metroGrid1.DefaultCellStyle.Font = new Font(metroGrid1.ColumnHeadersDefaultCellStyle.Font.FontFamily, 20f);
        }

        private void toolStripMenuItem19_Click(object sender, EventArgs e)
        {
            metroGrid1.DefaultCellStyle.Font = new Font(metroGrid1.ColumnHeadersDefaultCellStyle.Font.FontFamily, 22f);
        }
        private void showToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Updates(true);
        }

        private void hideToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Updates();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            new AddProfile().Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            new RemoveProfile().Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {

            new EditProfile().Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Updates();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            new Settings().Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void updateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Updates();
        }

        private void whiteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            metroGrid1.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
        }

        private void ownColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() != DialogResult.Cancel)
            {
                metroGrid1.ColumnHeadersDefaultCellStyle.ForeColor = colorDialog1.Color;
            }
        }

        private void ownColorToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() != DialogResult.Cancel)
            {
                metroGrid1.DefaultCellStyle.ForeColor = colorDialog1.Color;
            }
        }

        private void yellowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            metroGrid1.ColumnHeadersDefaultCellStyle.ForeColor = Color.Yellow;
        }

        private void redToolStripMenuItem_Click(object sender, EventArgs e)
        {
            metroGrid1.ColumnHeadersDefaultCellStyle.ForeColor = Color.Red;
        }

        private void greenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            metroGrid1.ColumnHeadersDefaultCellStyle.ForeColor = Color.Green;
        }

        private void whiteToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            metroGrid1.DefaultCellStyle.ForeColor = Color.White;
        }

        private void yellowToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            metroGrid1.DefaultCellStyle.ForeColor = Color.Yellow;
        }

        private void redToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            metroGrid1.DefaultCellStyle.ForeColor = Color.Red;
        }

        private void greenToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            metroGrid1.DefaultCellStyle.ForeColor = Color.Green;
        }
    }
}

