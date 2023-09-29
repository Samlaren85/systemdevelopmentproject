using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer
{
    public class Skidskola
    {
        private static int _antalSkidskolor = 0;
        [Key]
        public string SkidskolaID { get; set; }
        
        public int AntalDeltagare { get; set; }
        public string Lärare { get; set; }
        public List<Privatlektion>? Privatlektion { get; set; }
        public List<Grupplektion>? Grupplektion { get; set; }
        public Skidskola()
        {
            
        }
        public Skidskola(int antaldeltagare, string lärare, List<Privatlektion>? privatlektion, List<Grupplektion>? grupplektion)
        {
           _antalSkidskolor++;
           SkidskolaID = "SKID" + _antalSkidskolor.ToString("00000");
           AntalDeltagare = antaldeltagare;
           Lärare = lärare;
           Privatlektion = privatlektion;
           Grupplektion = grupplektion;
        }

    }
}
