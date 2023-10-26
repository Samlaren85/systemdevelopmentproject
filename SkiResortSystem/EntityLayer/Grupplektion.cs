using System.ComponentModel.DataAnnotations;

namespace EntityLayer
{
    public class Grupplektion
    {
        [Key]
        public string GrupplektionId { get; set; }
        public string Svårighetsgrad { get; set; }
        public float Lektionspris { get; set; }
        public int MaxAntalDeltagare { get; set; }
        public Grupplektion()
        {
            
        }
        public Grupplektion(string svårighetsgrad, float lektionspris, int maxantaldeltagare)
        {
            Svårighetsgrad = svårighetsgrad;
            Lektionspris = lektionspris;
            MaxAntalDeltagare = maxantaldeltagare;
        }
    }
}
