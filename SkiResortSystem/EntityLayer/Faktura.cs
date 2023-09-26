using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer
{
    public class Faktura
    {
        [Key]
        public string FakturaID { get; set; }

        // VILKEN TYP AV DATUM MAN SKA HA??
        public DateTime Fakturadatum { get; set; }
        public float Totalpris { get; set; }
        public float Moms { get; set; }
        public Faktura()
        {
            
        }
        public Faktura(string fakturaID, DateTime fakturadatum, float totalpris, float moms)
        {
            FakturaID = fakturaID;
            Fakturadatum = fakturadatum; 
            Totalpris = totalpris; 
            Moms = moms;
        }
    }
}
