using System.ComponentModel.DataAnnotations;

namespace EntityLayer
{
    public class Privatlektion
    {
        [Key]
        public string PrivatlektionId { get; set; }
        public int MaxAntalDeltagare { get; set; }
        public float Timpris { get; set; }
        public Privatlektion()
        {
            
        }
        public Privatlektion(int maxantaldeltagare, float timpris)
        {
            MaxAntalDeltagare = maxantaldeltagare;
            Timpris = timpris;
        }
    }
}
