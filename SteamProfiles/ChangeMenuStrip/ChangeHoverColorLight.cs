﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SteamProfiles.ChangeMenuStrip
{
    class ChangeHoverColorLight : ProfessionalColorTable
    {
        public override Color MenuItemSelected
        {
            get { return ColorTranslator.FromHtml("#9EA9AE"); }
        }

        public override Color MenuItemBorder
        {
            get { return ColorTranslator.FromHtml("#97A2A6"); }
        }
        

    }
}
