using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer
{
    public class Konferanssal
    {
        public string KonferansID { get; set; }
        public string KonferansBenämning { get; set; }
        public Konferanssal(string konferansID, string konferansBenämning)
        {
            KonferansID = konferansID;
            KonferansBenämning = konferansBenämning;
        }
    }
}
