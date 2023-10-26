using System.ComponentModel.DataAnnotations;

namespace EntityLayer
{
   
    public class FacilitetsPris
    {
        [Key]
        public int FacilitetsPrisId { get; set; }
        public string FacilitetTyp { get; set; }
        public string BokningTyp { get; set; }
        public int Vecka { get; set; }
        public float Pris { get; set; }
        
    }
}
