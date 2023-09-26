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
        [Key]
        public string PrivatlektionID { get; set; }
        public string MaxAntalDeltagare { get; set; }
        public float Timpris { get; set; }
        public Privatlektion()
        {
            
        }
        public Privatlektion(string privatlektionID, string maxantaldeltagare, float timpris)
        {
            PrivatlektionID = privatlektionID;
            MaxAntalDeltagare = maxantaldeltagare;
            Timpris = timpris;
        }
    }
}
