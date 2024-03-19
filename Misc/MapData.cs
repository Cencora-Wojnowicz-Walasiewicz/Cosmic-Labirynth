using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cosmic_Labirynth.Misc
{
    public class MapData
    {
        // Klasa do zczytywania danych mapy z pliku JSON
        public string tilesetName {  get; set; }
        public int mapCols { get; set; }
        public int mapRows { get; set; }
        public int tileCols { get; set; }
        public int tileRows { get; set; }
        public int[] tiles1 { get; set; }
        public int[] tiles2 { get; set; }
    }
}
