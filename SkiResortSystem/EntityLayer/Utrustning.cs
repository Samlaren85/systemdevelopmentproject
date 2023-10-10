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
        [Key]
        public string UtrustningsID { get; set; }
        public string UtrustningsBenämning { get; set; }
        public float Pris { get; set; }
        public string Storlek { get; set; }
        public List<Utrustningsbokning>? BokningsRef { get; set; }
        public Utrustning()
        {
            
        }
        public Utrustning(string utrustningsID, string utrustningsBenämning, float pris, string storlek)
        {
            UtrustningsID = utrustningsID;
            UtrustningsBenämning = utrustningsBenämning;
            Pris = pris;
            Storlek = storlek;
        }
    }
}
