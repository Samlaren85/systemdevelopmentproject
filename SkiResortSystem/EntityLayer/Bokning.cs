using System.ComponentModel.DataAnnotations;

namespace EntityLayer
{
    public class Bokning
    {
        [Key]
        public string BokningsId { get; set; }
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
        public string Bokningsnummer { get; set; }
        private Status betalningsstatus;
        public Status Betalningsstatus
        {
            get { return betalningsstatus; }
            set
            { 
            
                if ((value >= Status.Obetald && value <= Status.Ofakturerad) || value == Status.Makulerad)
                {
                    betalningsstatus = value;
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
        public string AntalPersoner { get; set; }
        public float Totalpris {  get; set; }
        public Användare AnvändareID { get; set; }
        public Kund KundID { get; set; }

        public ICollection<Facilitet> FacilitetID { get; set; }

        public ICollection<Utrustningsbokning>? UtrustningRef { get; set; }

        public ICollection<Aktivitetsbokning>? AktivitetRef{ get; set; }

        public ICollection<Faktura>? Fakturaref { get; set; }

        public Bokning()
        {

        }
        public Bokning(DateTime ankomsttid, DateTime avresetid, Användare användareID, Kund kundID, List<Facilitet> facilitetID, string antalPersoner, bool återbetalningsskydd = false)
        {
            UtnyttjadKredit = 0;
            Ankomsttid = ankomsttid;
            Avresetid = avresetid;
            AnvändareID = användareID;
            KundID = kundID;
            FacilitetID = facilitetID;
            AntalPersoner = antalPersoner;
            UtrustningRef = null;
            AktivitetRef = null;
            Fakturaref = null;
            Återbetalningsskydd = återbetalningsskydd;
            Bokningsstatus = Status.Kommande;
            Betalningsstatus = Status.Ofakturerad;
        }

        public override string ToString()
        {
            string customerName = string.Empty;
            if (KundID.Företagskund != null) customerName = KundID.Företagskund.Företagsnamn;
            else if (KundID.Privatkund != null) customerName = KundID.Privatkund.Namn();
            return $"{Bokningsnummer} ({customerName})";
        }
    }
}
