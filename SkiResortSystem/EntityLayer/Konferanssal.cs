using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer
{
    public class Konferanssal
    {
        [Key]
        public string KonferansID { get; set; }
        public string KonferansBenämning { get; set; }
        public Konferanssal()
        {
            
        }
        public Konferanssal(string konferansID, string konferansBenämning)
        {
            KonferansID = konferansID;
            KonferansBenämning = konferansBenämning;
        }
    }
}
