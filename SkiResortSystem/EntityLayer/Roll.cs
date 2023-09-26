using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer
{
    public class Roll
    {
        [Key]
        public string RollID { get; set; }
        public string Rolltyp { get; set; }
        public List<Behörighet> BehörighetID { get; set; }
        public Roll()
        {
            
        }
        public Roll(string rollID, string rolltyp, List<Behörighet> behörighetID)
        { 
            RollID = rollID;
            Rolltyp = rolltyp;
            BehörighetID = behörighetID;
        }
    }
}
