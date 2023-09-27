using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer
{
    public class Behörighet
    {
        private static int _antalBehörigheter = 0;
        [Key]
        public string BehörighetID { get; set; }
        public string Behörighetstyp { get; set; }
        public Behörighet()
        {
            
        }
        public Behörighet(string behörighetstyp)
        {
            _antalBehörigheter++;
            BehörighetID = "BH" + _antalBehörigheter.ToString("000");
            Behörighetstyp = behörighetstyp;
        }
    }
   
}
