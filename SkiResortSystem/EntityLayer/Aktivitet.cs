using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer
{
    public class Aktivitet
    {
        public string AktivitetsID { get; set; }
        public Skidskola Skidskola { get; set; }
        public bool Vintersäsong { get; set; }
        public Aktivitet()
        {
            
        }
        public Aktivitet(string aktivitetsID, Skidskola skidskola, bool vintersäsong)
        {
            AktivitetsID = aktivitetsID;
            Skidskola = skidskola;
            Vintersäsong = vintersäsong;
        }
    }
}
