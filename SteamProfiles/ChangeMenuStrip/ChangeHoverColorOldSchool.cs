using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SteamProfiles.ChangeMenuStrip
{
    class ChangeHoverColorOldSchool : ProfessionalColorTable
    {
        public override Color MenuItemSelected
        {
            get { return ColorTranslator.FromHtml("#000064"); }
        }

        public override Color MenuItemBorder
        {
            get { return ColorTranslator.FromHtml("#00006e"); }
        }


    }
}
