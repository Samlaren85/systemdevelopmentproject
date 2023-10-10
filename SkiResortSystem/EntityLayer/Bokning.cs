using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer
{
    public class Bokning
    {
        private static int _antalBokningar = 0;
        [Key]
        public string BokningsID { get; set; }
        private Status bokningsstatus;
        public Status Bokningsstatus
        {
            get { return bokningsstatus; }
            set
            {
                if (value == Status.Kommande || (value >= Status.Incheckad && value < Status.Utlämnad) || value == Status.Makulerad)
                {
                    bokningsstatus = value;
                }
                else
                {
                    throw new ArgumentException("Otillåten status");
                }
            }
        }
        public float UtnyttjadKredit { get; set; }
        public bool Återbetalningsskydd { get; set; }
        public DateTime Ankomsttid { get; set; }
        public DateTime Avresetid { get; set; }
        public Användare AnvändareID { get; set; }
        public Kund KundID { get; set; }

        public ICollection<Facilitet> FacilitetID { get; set; }
        
        public ICollection<Utrustningsbokning>? UtrustningRef { get; set; }

        public ICollection<Aktivitet>? AktivitetID{ get; set; }

        public Bokning()
        {

        }
        public Bokning(DateTime ankomsttid, DateTime avresetid, Användare användareID, Kund kundID, List<Facilitet> facilitetID, bool återbetalningsskydd = false)
        {
            _antalBokningar++;
            BokningsID = "B" + _antalBokningar.ToString("000000");
            UtnyttjadKredit = 0;
            Ankomsttid = ankomsttid;
            Avresetid = avresetid;
            AnvändareID = användareID;
            KundID = kundID;
            FacilitetID = facilitetID;
            UtrustningRef = null;
            AktivitetID = null;
            Återbetalningsskydd = återbetalningsskydd;

        }

        public override string ToString()
        {
            string customerName = string.Empty;
            if (KundID.Företagskund != null) customerName = KundID.Företagskund.Företagsnamn;
            else if (KundID.Privatkund != null) customerName = KundID.Privatkund.Namn();
            return $"{BokningsID} ({customerName})";
        }
    }
}
