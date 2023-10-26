using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntityLayer
{
    public class Facilitet
    {
        private string facilitetId;
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string FacilitetId
        {
            get
            {
                if (LägenhetsID != null) return LägenhetsID.LägenhetId;
                else if (CampingID != null) return CampingID.CampingId;
                else return KonferensID.KonferensId;
            }
            set
            { facilitetId = value; }
        }

        [NotMapped]
        public float TotalprisFörPresentationIBoendeModul { get; set; }

        public List<Bokning>? BokningsRef { get; set; }
        //FACILITETSPRIS SKA GÖRAS TIDSBESTÄMT
        public int? FacilitetsPrisID { get; set; }

        public virtual FacilitetsPris? FacilitetsPris { get; set; }
        public Konferenssal? KonferensID { get; set; }
        public Lägenhet? LägenhetsID { get; set; }
        public Campingplats? CampingID { get; set; }
        public Facilitet()
        {
            
        }
        public Facilitet( Konferenssal? konferansID, Lägenhet? lägenhetsID, Campingplats? campingID)
        {
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
        public string Typ
        {
            get
            {
                if (LägenhetsID != null)
                {
                    return LägenhetsID.LägenhetBenämning;
                }
                if (CampingID != null)
                {
                    return CampingID.CampingBenämning;
                }
                if (KonferensID != null)
                {
                    return KonferensID.KonferensBenämning;
                }
                else
                {
                    return "Saknar facilitet";
                }
            }
        }
    }
}
