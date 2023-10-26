using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntityLayer
{
    public class Kund
    {
        private string kundId;
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string KundId 
        { 
            get
            {
                if (Privatkund != null) return Privatkund.PersonnummerId;
                else return Företagskund.OrganisationsnummerId;
            }
            set { kundId = value; }
        }
        public float Rabatt { get; set; }
        public float Kreditgräns { get; set; }
        public string Gatuadress { get; set; }
        public string Postnummer { get; set; }
        public string Ort { get; set; }
        public string Telefonnummer { get; set; }
        public string Epost { get; set; }
        public string Namn
        {
            get 
            { 
                if (Privatkund != null) return Privatkund.Namn() + " (Privatkund)";
                else return Företagskund.Företagsnamn + " (Företagskund)";
            }
        }
        public Företagskund? Företagskund { get; set; }
        public Privatkund? Privatkund { get; set; }
        public ICollection<Bokning> BokningsRef { get; set; }
        public Kund()
        {
            
        }
        public Kund(float rabatt, float kreditgräns, string gatuadress, string postnummer, string ort, string telefonnummer, string epost, Företagskund? företagskund, Privatkund? privatkund)
        {
            Rabatt = rabatt;
            Kreditgräns = kreditgräns;
            Gatuadress = gatuadress;
            Postnummer = postnummer;
            Ort = ort;
            Telefonnummer = telefonnummer;
            Epost = epost;
            Företagskund = företagskund;
            Privatkund = privatkund;
        }
        public string Adress(string adress)
        {
            return Gatuadress + Postnummer + Ort;
        }
        public override string ToString()
        {
            string kundtext;
            if(Privatkund != null)
            {
                kundtext = Privatkund.Namn() + " (Privatkund)";
            }
            else
            {
                kundtext = Företagskund.Företagsnamn + " (Företagskund)";
            }
            return kundtext.ToString();
        }

        public string Number()
        {
            string kundnummer;
            if (Privatkund != null)
            {
                kundnummer = Privatkund.PersonnummerId;
            }
            else
            {
                kundnummer = Företagskund.OrganisationsnummerId;
            }
            return kundnummer.ToString();
        }
    }
}
