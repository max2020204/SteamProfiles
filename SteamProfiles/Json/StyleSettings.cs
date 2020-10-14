using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteamProfiles.Json
{
    public class StyleSettings
    {
        public HeaderTextColor HeaderTextColor { get; set; }
        public Font HeaderCellSize { get; set; }
        public CellTextColor CellTextColor { get; set; }
        public Font CellSize { get; set; }
    }
    public class HeaderTextColor
    {
        public byte HeaderTextColorR { get; set; }
        public byte HeaderTextColorG { get; set; }
        public byte HeaderTextColorB { get; set; }
    }
    public class CellTextColor
    {
        public byte CellTextColorR { get; set; }
        public byte CellTextColorG { get; set; }
        public byte CellTextColorB { get; set; }
    }
}
