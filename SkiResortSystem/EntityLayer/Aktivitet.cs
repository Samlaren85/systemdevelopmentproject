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

        //vintersäsong som property??
        public Aktivitet(string aktivitetsID, Skidskola skidskola)
        {
            AktivitetsID = aktivitetsID;
            Skidskola = skidskola;
        }
    }
}
