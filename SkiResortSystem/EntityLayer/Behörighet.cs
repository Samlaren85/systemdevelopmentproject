using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer
{
    public class Behörighet
    {
        [Key]
        public string BehörighetID { get; set; }
        public string Behörighetstyp { get; set; }
        public Behörighet()
        {
            
        }
        public Behörighet(string behörighetID, string behörighetstyp)
        {
            BehörighetID = behörighetID;
            Behörighetstyp = behörighetstyp;
        }
    }
   
}
