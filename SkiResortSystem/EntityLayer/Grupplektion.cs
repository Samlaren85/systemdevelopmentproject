using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer
{
    public class Grupplektion
    {
        public string GrupplektionID { get; set; }
        public string Svårighetsgrad { get; set; }
        public float Lektionspris { get; set; }
        public int MaxAntalDeltagare { get; set; }
    }
}
