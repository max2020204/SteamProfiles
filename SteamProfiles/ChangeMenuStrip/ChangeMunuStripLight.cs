using SteamProfiles.ChangeMenuStrip;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SteamProfiles
{
    class ChangeMunuStripLight : ToolStripProfessionalRenderer
    {
        public ChangeMunuStripLight() : base(new ChangeHoverColorLight()) 
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
