using System.ComponentModel.DataAnnotations;

namespace EntityLayer
{
    public class Företagskund
    {
        [Key]
        public string OrganisationsnummerId { get; set; }
        public string Företagsnamn { get; set; }
        public string Kontaktperson { get; set; }
        public string Besöksadress { get; set; }
        public string Besökspostnummer { get; set; }
        public string Besöksort { get; set; }
        public Företagskund()
        {
            
        }
        public Företagskund(string organisationsnummerID, string företagsnamn, string kontaktperson, string besöksadress, string besökspostnummer, string besöksort)
        {
            OrganisationsnummerId = organisationsnummerID;
            Företagsnamn = företagsnamn;
            Kontaktperson = kontaktperson;
            Besöksadress = besöksadress;
            Besökspostnummer = besökspostnummer;
            Besöksort = besöksort;
        }
    }
}
