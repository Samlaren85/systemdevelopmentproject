using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer
{
    public class Privatkund
    {
        public string PersonnummerID { get; set; }
        public string Förnamn { get; set; }
        public string Efternamn { get; set; }
        public Privatkund()
        {
            
        }
        public Privatkund(string personnummerID, string förnamn, string efternamn)
        {
            PersonnummerID = personnummerID;
            Förnamn = förnamn;
            Efternamn = efternamn;
        }
        public string Namn()
        {
            return Förnamn + Efternamn;
        }
    }
}
