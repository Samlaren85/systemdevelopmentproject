using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer
{
    public class Konferenssal
    {
        [Key]
        public string KonferensID { get; set; }
        public string KonferensBenämning { get; set; }
        public Konferenssal()
        {
            
        }
        public Konferenssal(string konferensID, string konferensBenämning)
        {
            KonferensID = konferensID;
            KonferensBenämning = konferensBenämning;
        }
    }
}
