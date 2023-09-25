using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer
{
    public class Behörighet
    {
        public string BehörighetID { get; set; }
        public string Behörighetstyp { get; set; }
        public Behörighet(string behörighetID, string behörighetstyp)
        {
            BehörighetID = behörighetID;
            Behörighetstyp = behörighetstyp;
        }
    }
   
}
