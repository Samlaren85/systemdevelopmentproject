using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer
{
    public class Utrustning
    {
        public string UtrustningsID { get; set; }
        public string UtrustningsBenämning { get; set; }
        public float Pris { get; set; }
        public int Antal { get; set; }
        public List<Paket> Paket { get; set; }
        public Utrustningsstorlek Storlek { get; set; }
        public Utrustning(string utrustningsID, string utrustningsBenämning, float pris, int antal, List<Paket> paket, Utrustningsstorlek storlek)
        {
            UtrustningsID = utrustningsID;
            UtrustningsBenämning = utrustningsBenämning;
            Pris = pris;
            Antal = antal;
            Paket = paket;
            Storlek = storlek;
        }
    }
}
