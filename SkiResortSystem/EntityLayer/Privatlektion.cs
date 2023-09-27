using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer
{
    public class Privatlektion
    {
        private static int _antalPrivatlektioner = 0;
        [Key]
        public string PrivatlektionID { get; set; }
        public string MaxAntalDeltagare { get; set; }
        public float Timpris { get; set; }
        public Privatlektion()
        {
            
        }
        public Privatlektion(string maxantaldeltagare, float timpris)
        {
            _antalPrivatlektioner++;
            PrivatlektionID = "PRIV" + _antalPrivatlektioner.ToString("0000");
            MaxAntalDeltagare = maxantaldeltagare;
            Timpris = timpris;
        }
    }
}
