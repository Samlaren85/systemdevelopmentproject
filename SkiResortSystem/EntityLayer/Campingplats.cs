using System.ComponentModel.DataAnnotations;

namespace EntityLayer
{
    public class Campingplats
    {
        [Key]
        public string CampingId { get; set; }
        public string CampingBenämning { get; set; }
        public string CampingStorlek { get; set; }
        public Campingplats()
        {
            
        }
        public Campingplats(string campingBenämning, string campingStorlek)
        {
            CampingBenämning = campingBenämning;
            CampingStorlek = campingStorlek;
        }
    }
}
