using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer
{
    public class Utrustningsstorlek
    {
        public string StorleksID { get; set; }
        public string Storlek { get; set; }
        public Utrustningsstorlek(string storleksid, string storlek)
        {
            StorleksID = storleksid;
            Storlek = storlek;
        }
    }
}
