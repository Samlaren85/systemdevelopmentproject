using System.ComponentModel.DataAnnotations;

namespace EntityLayer
{
    public class Skidskola
    {
        [Key]
        public string SkidskolaId { get; set; }
        
        public int AntalDeltagare { get; set; }
        public string Lärare { get; set; }
        public DateTime VaraktighetFrån { get; set; }
        public DateTime VaraktighetTill { get; set; }
        public string Typ
        {
            get
            {
                if (Privatlektion != null) return "Privatlektion";
                else if (Grupplektion != null) return $"Grupplektion {Grupplektion.Svårighetsgrad}";
                return "Kunde inte hitta någon typ av Skidskola";
            }
        }
        public float Pris
        {
            get
            {
                if (Privatlektion != null) return Privatlektion.Timpris;
                else if (Grupplektion != null) return Grupplektion.Lektionspris;
                return 0;
            }
        }
        public Privatlektion? Privatlektion { get; set; }
        public Grupplektion? Grupplektion { get; set; }
        public Skidskola()
        {
            
        }
        public Skidskola(int antaldeltagare, string lärare, Privatlektion? privatlektion, Grupplektion? grupplektion, DateTime från, DateTime till)
        {
           AntalDeltagare = antaldeltagare;
           Lärare = lärare;
           Privatlektion = privatlektion;
           Grupplektion = grupplektion;
            VaraktighetFrån = från;
            VaraktighetTill = till;
        }

    }
}
