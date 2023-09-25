using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer
{
    public class Kund
    {
        public string KundID { get; set; }
        public float Rabatt { get; set; }
        public float Kreditgräns { get; set; }
        public string Gatuadress { get; set; }
        public int Postnummer { get; set; }
        public string Ort { get; set; }
        public string Telefonnummer { get; set; }

    }
}
