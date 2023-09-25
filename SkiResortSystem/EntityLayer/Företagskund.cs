using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer
{
    public class Företagskund
    {
        public string OrganisationsnummerID { get; set; }
        public string Företagsnamn { get; set; }
        public string Kontaktperson { get; set; }
        public Företagskund(string organisationsnummerID, string företagsnamn, string kontaktperson)
        {
            OrganisationsnummerID = organisationsnummerID;
            Företagsnamn = företagsnamn;
            Kontaktperson = kontaktperson;
        }
    }
}
