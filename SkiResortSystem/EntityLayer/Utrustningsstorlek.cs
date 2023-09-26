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
        [Key]
        public string StorleksID { get; set; }
        public string Storlek { get; set; }
        public Utrustningsstorlek()
        {
            
        }
        public Utrustningsstorlek(string storleksid, string storlek)
        {
            StorleksID = storleksid;
            Storlek = storlek;
        }
    }
}
