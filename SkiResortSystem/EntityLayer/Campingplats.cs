using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer
{
    public class Campingplats
    {
        [Key]
        public string CampingID { get; set; }
        public string CampingBenämning { get; set; }
        public string CampingStorlek { get; set; }
        public Campingplats()
        {
            
        }
        public Campingplats(string campingID, string campingBenämning, string campingStorlek)
        {
            CampingID = campingID;
            CampingBenämning = campingBenämning;
            CampingStorlek = campingStorlek;
        }
    }
}
