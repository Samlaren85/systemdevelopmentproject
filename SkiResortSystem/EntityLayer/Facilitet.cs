using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer
{
    public class Facilitet
    {
        [Key]
        public string FacilitetID { get; set; }
        
        //FACILITETSPRIS SKA GÖRAS TIDSBESTÄMT
        public float Facilitetspris { get; set; }
        public Konferenssal? KonferensID { get; set; }
        public Lägenhet? LägenhetsID { get; set; }
        public Campingplats? CampingID { get; set; }
        public Facilitet()
        {
            
        }
        public Facilitet(string facilitetID, float facilitetspris, Konferenssal konferansID, Lägenhet lägenhetsID, Campingplats campingID)
        {
            FacilitetID = facilitetID;
            Facilitetspris = facilitetspris;
            KonferensID = konferansID;
            LägenhetsID = lägenhetsID;
            CampingID = campingID;
        }
    }
}
