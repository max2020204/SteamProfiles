using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SteamProfiles
{
    public static class GetAllControls
    {
        public static List<Control> GetControls(Control form)
        {
            var controlList = new List<Control>();

            foreach (Control childControl in form.Controls)
            {
                // Recurse child controls.
                controlList.AddRange(GetControls(childControl));
                controlList.Add(childControl);
            }
            return controlList;
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
            else
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
    }
}
