using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer
{
    public class Aktivitetsbokning
    {
        [Key]
        public int AktivitetsbokningsID { get; set; }
        public Bokning Bokningsref { get; set; }
        public Aktivitet Aktivitetsref { get; set; }
        public float TotalPris => Aktivitetsref.Skidskola.Pris * Antal;
        public int Antal { get; set; }
        public Aktivitetsbokning()
        {
              
        }
        public Aktivitetsbokning(Bokning bokning, Aktivitet aktivitet, int antal)
        {
            Bokningsref = bokning;
            Aktivitetsref = aktivitet;
            Antal = antal;
        }
    }
}
