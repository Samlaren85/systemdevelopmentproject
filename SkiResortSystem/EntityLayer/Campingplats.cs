using System.ComponentModel.DataAnnotations;

namespace EntityLayer
{
    public class Campingplats
    {
        private static int _antalCampingplatser = 0;
        [Key]
        public string CampingID { get; set; }
        public string CampingBenämning { get; set; }
        public string CampingStorlek { get; set; }
        public Campingplats()
        {
            
        }
        public Campingplats(string campingBenämning, string campingStorlek)
        {
            _antalCampingplatser++;
            CampingID = "CAMP" + _antalCampingplatser.ToString("0000");
            CampingBenämning = campingBenämning;
            CampingStorlek = campingStorlek;
        }
    }
}
