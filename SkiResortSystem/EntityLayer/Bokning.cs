using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public float UtnyttjadKredit { get; set; }
        public DateTime Ankomsttid { get; set; }
        public DateTime Avresetid { get; set; }
        public Användare AnvändareID { get; set; }
        public Kund KundID { get; set; }
        public List<Facilitet> FacilitetID { get; set; }
        public List<Utrustning> UtrustningID { get; set; }
        public List<Aktivitet> AktivitetID{ get; set; }

        public Bokning()
        {

        }
        public Bokning(Användare användareID, Kund kundID, List<Facilitet> facilitetID, List<Utrustning> utrustningID, List<Aktivitet> aktivitetID)
        {
            _antalBokningar++;
            BokningsID = "B" + _antalBokningar.ToString("000000");
            UtnyttjadKredit = 0;
            AnvändareID = användareID;
            KundID = kundID;
            FacilitetID = facilitetID;
            UtrustningID = utrustningID;
            AktivitetID = aktivitetID;
        }
    }
}
