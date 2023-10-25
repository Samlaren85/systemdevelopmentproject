using System.ComponentModel.DataAnnotations;

namespace EntityLayer
{
    public class Privatlektion
    {
        private static int _antalPrivatlektioner = 0;
        [Key]
        public string PrivatlektionID { get; set; }
        public int MaxAntalDeltagare { get; set; }
        public float Timpris { get; set; }
        public Privatlektion()
        {
            
        }
        public Privatlektion(int maxantaldeltagare, float timpris)
        {
            _antalPrivatlektioner++;
            PrivatlektionID = "PRIV" + _antalPrivatlektioner.ToString("0000");
            MaxAntalDeltagare = maxantaldeltagare;
            Timpris = timpris;
        }
    }
}
