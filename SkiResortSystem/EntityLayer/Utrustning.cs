using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer
{
    public class Utrustning
    {
        private static int _antalUtrustning = 0;
        [Key]
        public string UtrustningsID { get; set; }
        public string UtrustningsBenämning { get; set; }
        public float Pris { get; set; }
        public int Antal { get; set; }
        public List<Paket> Paket { get; set; }
        public Utrustningsstorlek Storlek { get; set; }
        public Utrustning()
        {
            
        }
        public Utrustning(string utrustningsBenämning, float pris, int antal, List<Paket> paket, Utrustningsstorlek storlek)
        {
            _antalUtrustning++;
            UtrustningsID = "UTR" + _antalUtrustning.ToString("000000000");
            UtrustningsBenämning = utrustningsBenämning;
            Pris = pris;
            Antal = antal;
            Paket = paket;
            Storlek = storlek;
        }
    }
}
