using System.ComponentModel.DataAnnotations;

namespace EntityLayer
{
    public class Lägenhet
    {
        [Key]
        public string LägenhetId { get; set; }
        public string LägenhetBenämning { get; set; }
        public int Bäddar { get; set; }
        public string Lägenhetstorlek { get; set; }
        public Lägenhet()
        {
            
        }
        public Lägenhet(string lägenhetbenämning, int bäddar, string lägenhetstorlek)
        {
            LägenhetBenämning = lägenhetbenämning;
            Bäddar = bäddar;
            Lägenhetstorlek = lägenhetstorlek;
        }
    }
}
