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
        public DateTime VaraktighetFrån { get; set; }
        public DateTime VaraktighetTill { get; set; }
        public string Typ
        {
            get
            {
                if (Privatlektion != null) return "Privatlektion";
                else if (Grupplektion != null) return "Grupplektion";
                return "Kunde inte hitta någon typ av Skidskola";
            }
        }
        public Privatlektion? Privatlektion { get; set; }
        public Grupplektion? Grupplektion { get; set; }
        public Skidskola()
        {
            
        }
        public Skidskola(int antaldeltagare, string lärare, Privatlektion? privatlektion, Grupplektion? grupplektion, DateTime från, DateTime till)
        {
           _antalSkidskolor++;
           SkidskolaID = "SKID" + _antalSkidskolor.ToString("00000");
           AntalDeltagare = antaldeltagare;
           Lärare = lärare;
           Privatlektion = privatlektion;
           Grupplektion = grupplektion;
            VaraktighetFrån = från;
            VaraktighetTill = till;
        }

    }
}
