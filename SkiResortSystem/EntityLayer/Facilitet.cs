using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer
{
    public class Facilitet
    {
        private static int _antalFaciliteter;
        [Key]
        private string facilitetID;
        public string FacilitetID
        {
            get
            {
                if (LägenhetsID != null) return LägenhetsID.LägenhetID;
                else if (CampingID != null) return CampingID.CampingID;
                else return KonferensID.KonferensID;
            }
            set
            { facilitetID = value; }
        }



        public List<Bokning>? BokningsRef { get; set; }
        //FACILITETSPRIS SKA GÖRAS TIDSBESTÄMT
        public float Facilitetspris { get; set; }
        public Konferenssal? KonferensID { get; set; }
        public Lägenhet? LägenhetsID { get; set; }
        public Campingplats? CampingID { get; set; }
        public Facilitet()
        {
            
        }
        public Facilitet(float facilitetspris, Konferenssal? konferansID, Lägenhet? lägenhetsID, Campingplats? campingID)
        {
            _antalFaciliteter++;
            FacilitetID = "F" + _antalFaciliteter.ToString("000000");
            Facilitetspris = facilitetspris;
            KonferensID = konferansID;
            LägenhetsID = lägenhetsID;
            CampingID = campingID;
        }


        public override string ToString()
        {
            if(LägenhetsID != null)
            {
                return  LägenhetsID.LägenhetBenämning + "\n" + LägenhetsID.Lägenhetstorlek + ", " + LägenhetsID.Bäddar;
            }
            if (CampingID != null)
            {
                return CampingID.CampingBenämning + "\n" + CampingID.CampingStorlek;
            }
            if(KonferensID != null)
            {
                return KonferensID.KonferensBenämning;
            }
            else
            {
                return "Databasen saknar information om facilitet.";
            }
        }
    }
}
