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
        private static int _antalFakturor = 0;
        [Key]
        public string FakturaID { get; set; }

        public DateTime Fakturadatum { get; set; }
        public float Totalpris { get; set; }
        public float Moms { get; set; }
        public Faktura()
        {
            
        }
        public Faktura(DateTime fakturadatum, float totalpris, float moms)
        {
            _antalFakturor++;
            FakturaID = "FAKT" + _antalFakturor.ToString("000000");
            Fakturadatum = fakturadatum; 
            Totalpris = totalpris; 
            Moms = moms;
        }
    }
}
