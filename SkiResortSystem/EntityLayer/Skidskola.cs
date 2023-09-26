using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer
{
    public class Skidskola
    {
        public string SkidskolaID { get; set; }
        
        //SKA VARA BERÄKNAT
        public int AntalDeltagare { get; set; }
        public string Lärare { get; set; }
        public List<Privatlektion>? Privatlektion { get; set; }
        public List<Grupplektion>? Grupplektion { get; set; }
        public Skidskola(string skidskolaID, int antaldeltagare, string lärare, List<Privatlektion>? privatlektion, List<Grupplektion>? grupplektion)
        {
           SkidskolaID = skidskolaID;
           AntalDeltagare = antaldeltagare;
           Lärare = lärare;
           Privatlektion = privatlektion;
           Grupplektion = grupplektion;
        }

    }
}
