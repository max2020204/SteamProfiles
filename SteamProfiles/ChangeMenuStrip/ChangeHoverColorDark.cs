using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SteamProfiles.ChangeMenuStrip
{
    class ChangeHoverColorDark : ProfessionalColorTable
    {
        public override Color MenuItemSelected
        {
            get { return ColorTranslator.FromHtml("#1c1c1c"); }
        }

        public override Color MenuItemBorder
        {
            get { return ColorTranslator.FromHtml("#232323"); }
        }

    }
}
