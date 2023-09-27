using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer
{
    public class Grupplektion
    {
        private static int _antalGrupplektioner = 0;
        [Key]
        public string GrupplektionID { get; set; }
        public string Svårighetsgrad { get; set; }
        public float Lektionspris { get; set; }
        public int MaxAntalDeltagare { get; set; }
        public Grupplektion()
        {
            
        }
        public Grupplektion(string svårighetsgrad, float lektionspris, int maxantaldeltagare)
        {
            _antalGrupplektioner++;
            GrupplektionID = "GRUPP" + _antalGrupplektioner.ToString("000000");
            Svårighetsgrad = svårighetsgrad;
            Lektionspris = lektionspris;
            MaxAntalDeltagare = maxantaldeltagare;
        }
    }
}
