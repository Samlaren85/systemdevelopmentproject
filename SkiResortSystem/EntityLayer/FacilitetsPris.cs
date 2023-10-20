using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer
{
   
    public class FacilitetsPris
    {
        [Key]
        public int FacilitetsPrisID { get; set; }
        public string FacilitetTyp { get; set; }
        public string BokningTyp { get; set; }
        public int Vecka { get; set; }
        public float Pris { get; set; }
        
    }
}
