using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer
{
    public class Facilitet
    {
        public string FacilitetID { get; set; }
        
        //FACILITETSPRIS SKA GÖRAS TIDSBESTÄMT
        public float Facilitetspris { get; set; }
        public Konferanssal? KonferansID { get; set; }
        public Lägenhet? LägenhetsID { get; set; }
        public Campingplats? CampingID { get; set; }
        public Facilitet()
        {
            
        }
        public Facilitet(string facilitetID, float facilitetspris, Konferanssal konferansID, Lägenhet lägenhetsID, Campingplats campingID)
        {
            FacilitetID = facilitetID;
            Facilitetspris = facilitetspris;
            KonferansID = konferansID;
            LägenhetsID = lägenhetsID;
            CampingID = campingID;
        }
    }
}
