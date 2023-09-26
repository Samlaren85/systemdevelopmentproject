using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer
{
    public class Lägenhet
    {
        [Key]
        public string LägenhetID { get; set; }
        public string LägenhetBenämning { get; set; }
        public string Bäddar { get; set; }
        public string Lägenhetstorlek { get; set; }
        public Lägenhet()
        {
            
        }
        public Lägenhet(string lägenhetID, string lägenhetbenämning, string bäddar, string lägenhetstorlek)
        {
            LägenhetID = lägenhetID;
            LägenhetBenämning = lägenhetbenämning;
            Bäddar = bäddar;
            Lägenhetstorlek = lägenhetstorlek;
        }
    }
}
