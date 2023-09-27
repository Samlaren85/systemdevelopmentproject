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
        private static int _antalFaciliteter;
        [Key]
        public string FacilitetID { get; set; }
        
        //FACILITETSPRIS SKA GÖRAS TIDSBESTÄMT
        public float Facilitetspris { get; set; }
        public Konferenssal? KonferansID { get; set; }
        public Lägenhet? LägenhetsID { get; set; }
        public Campingplats? CampingID { get; set; }
        public Facilitet()
        {
            
        }
        public Facilitet(float facilitetspris, Konferenssal konferansID, Lägenhet lägenhetsID, Campingplats campingID)
        {
            _antalFaciliteter++;
            FacilitetID = "F" + _antalFaciliteter.ToString("000000");
            Facilitetspris = facilitetspris;
            KonferansID = konferansID;
            LägenhetsID = lägenhetsID;
            CampingID = campingID;
        }
    }
}
