using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer
{
    public class Utrustningsstorlek
    {
        private static int _antalUtrustningsstorlekar = 0;
        [Key]
        public string StorleksID { get; set; }
        public string Storlek { get; set; }
        public Utrustningsstorlek()
        {
            
        }
        public Utrustningsstorlek(string storlek)
        {
            _antalUtrustningsstorlekar++;
            StorleksID = "STL" + _antalUtrustningsstorlekar.ToString("000");
            Storlek = storlek;
        }
    }
}
