using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer
{
    public class Aktivitet
    {
        private static int _antalAktiviteter = 0;
        [Key]
        public string AktivitetsID { get; set; }
        public Skidskola Skidskola { get; set; }
        public bool Vintersäsong { get; set; }
        public Aktivitet()
        {
            
        }
        public Aktivitet(Skidskola skidskola, bool vintersäsong)
        {
            _antalAktiviteter++;
            AktivitetsID = "AKT" + _antalAktiviteter.ToString("000");
            Skidskola = skidskola;
            Vintersäsong = vintersäsong;
        }
    }
}
