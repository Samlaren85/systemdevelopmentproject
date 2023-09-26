using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer
{
    public class Paket
    {
        public string PaketID { get; set; }
        public string Paketbenämning { get; set; }
        public float Pris { get; set; }
        public Paket()
        {
            
        }
        public Paket(string paketID, string paketbenämning, float pris)
        {
            PaketID = paketID;
            Paketbenämning = paketbenämning;
            Pris = pris;
        }
    }
}
