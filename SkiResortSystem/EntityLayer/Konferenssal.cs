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
        private static int _antalKonferenssalar = 0;
        [Key]
        public string KonferensID { get; set; }
        public string KonferensBenämning { get; set; }
        public Konferenssal()
        {
            
        }
        public Konferenssal(string konferensBenämning)
        {
            _antalKonferenssalar++;
            KonferensID = "KONF" + _antalKonferenssalar.ToString("000");
            KonferensBenämning = konferensBenämning;
        }
    }
}
