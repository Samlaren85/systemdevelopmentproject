using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer
{
    public class Företagskund
    {
        [Key]
        public string OrganisationsnummerID { get; set; }
        public string Företagsnamn { get; set; }
        public string Kontaktperson { get; set; }
        public string Besöksaddress { get; set; }
        public Företagskund()
        {
            
        }
        public Företagskund(string organisationsnummerID, string företagsnamn, string kontaktperson, string besöksaddress)
        {
            OrganisationsnummerID = organisationsnummerID;
            Företagsnamn = företagsnamn;
            Kontaktperson = kontaktperson;
            Besöksaddress = besöksaddress;
        }
    }
}
