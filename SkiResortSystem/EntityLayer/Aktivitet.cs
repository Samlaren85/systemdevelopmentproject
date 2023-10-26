using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace EntityLayer
{
    public class Aktivitet
    {
        [Key]
        public string AktivitetsId { get; set; }
        private Status aktivitetsstatus;
        public Status Aktivitetsstatus
        {
            get { return aktivitetsstatus; }
            set
            {
                if (value == Status.Kommande || value == Status.Genomförd || value == Status.Makulerad)
                {
                    aktivitetsstatus = value;
                }
                else
                {
                    throw new ArgumentException("Otillåten status");
                }
            }
        }
        public string Typ
        {
            get
            {
                if (Skidskola != null)
                {
                    return Skidskola.Typ;
                }
                return "Ingen typ hittad av aktiviteten";
            }
        }
        
        public string Varaktighet
        {
            get
            {
                if (Skidskola != null)
                {
                    Calendar calender = CultureInfo.CurrentCulture.Calendar;
                    if (Skidskola.Grupplektion != null) return $"v.{calender.GetWeekOfYear(Skidskola.VaraktighetFrån, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday)} {Skidskola.VaraktighetFrån.DayOfWeek}-{Skidskola.VaraktighetTill.DayOfWeek}";
                    else if (Skidskola.Privatlektion != null) return $"v.{calender.GetWeekOfYear(Skidskola.VaraktighetFrån, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday)} {Skidskola.VaraktighetFrån.DayOfWeek} ({Skidskola.VaraktighetFrån.TimeOfDay})";
                }
                return "Ingen varaktighet hittad av aktiviteten";
            }
        }
        public int AntalPlatserKvar
        {
            get
            {
                if (Skidskola != null)
                {
                    if (Skidskola.Grupplektion != null) return Skidskola.Grupplektion.MaxAntalDeltagare - Skidskola.AntalDeltagare;
                    else if (Skidskola.Privatlektion != null) return Skidskola.Privatlektion.MaxAntalDeltagare - Skidskola.AntalDeltagare;
                }
                return 0;
            }
        }
        public Skidskola Skidskola { get; set; }
        public bool Vintersäsong { get; set; }
        public ICollection<Aktivitetsbokning>? BokningsRef { get; set; }
        public Aktivitet()
        {
            
        }
        public Aktivitet(Skidskola skidskola, bool vintersäsong)
        {
            Skidskola = skidskola;
            Vintersäsong = vintersäsong;
        }
    }
}
