using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer
{
    public class Kund
    {
        private static int _kundAntal = 0;
        public string KundID { get; set; }
        public float Rabatt { get; set; }
        public float Kreditgräns { get; set; }
        public string Gatuadress { get; set; }
        public int Postnummer { get; set; }
        public string Ort { get; set; }
        public string Telefonnummer { get; set; }
        public Företagskund? Företagskund { get; set; }
        public Privatkund? Privatkund { get; set; }

        public Kund()
        {

        }
        public Kund(float rabatt, float kreditgräns, string gatuadress, int postnummer, string ort, string telefonnummer, Företagskund? företagskund, Privatkund? privatkund)
        {
            _kundAntal++;
            KundID = "K" + _kundAntal.ToString("000000");
            Rabatt = rabatt;
            Kreditgräns = kreditgräns;
            Gatuadress = gatuadress;
            Postnummer = postnummer;
            Ort = ort;
            Telefonnummer = telefonnummer;
            Företagskund = företagskund;
            Privatkund = privatkund;
        }
        public string Adress(string adress)
        {
            return Gatuadress + Postnummer + Ort;
        }
    }
}
