using SteamProfiles.Forms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SteamProfiles
{
    static class Themes
    {
        public static void ThemeChange(bool mode, Control form, Color backcolor, Color buttoncolor, Color MouseDownBackColor)
        {
            if (buttoncolor == null)
            {
                buttoncolor = backcolor;
            }
            List<Control> availControls = GetAllControls.GetControls(form);
            if (mode)
            {
                foreach (var item in availControls)
                {
                    item.BackColor = backcolor;
                    if (item is Button button)
                    {
                        button.BackColor = buttoncolor;
                        button.FlatAppearance.MouseDownBackColor = MouseDownBackColor;
                    }
                }
            }

        }
        public static void ThemeChange(bool mode, Control form, Color backcolor, Color MouseDownBackColor)
        {
            List<Control> availControls = GetAllControls.GetControls(form);
            if (mode)
            {
                foreach (var item in availControls)
                {
                    item.BackColor = backcolor;
                    if (item is Button button)
                    {
                        button.FlatAppearance.MouseDownBackColor = MouseDownBackColor;
                    }
                }
            }

        }
        public static void ChangeForeColor(bool mode, Control form, Color Forecolor)
        {
            List<Control> availControls = GetAllControls.GetControls(form);
            if (mode)
            {
                foreach (var item in availControls)
                {
                    item.ForeColor = Forecolor;
                }
            }

        }
    }
}
