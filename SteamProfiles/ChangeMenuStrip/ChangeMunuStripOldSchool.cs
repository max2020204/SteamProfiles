using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SteamProfiles.ChangeMenuStrip
{
    class ChangeMunuStripOldSchool : ToolStripProfessionalRenderer
    {
        public ChangeMunuStripOldSchool() : base(new ChangeHoverColorOldSchool())
        {

        }
        protected override void OnRenderArrow(ToolStripArrowRenderEventArgs e)
        {
            var tsMenuItem = e.Item as ToolStripMenuItem;
            if (tsMenuItem != null)
                e.ArrowColor = Color.White;
            base.OnRenderArrow(e);
        }
    }
}
