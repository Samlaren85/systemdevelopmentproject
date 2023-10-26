using System.ComponentModel.DataAnnotations;

namespace EntityLayer
{
    public class Privatkund
    {
        [Key]
        public string PersonnummerId { get; set; }
        public string Förnamn { get; set; }
        public string Efternamn { get; set; }
        public Privatkund()
        {
            
        }
        public Privatkund(string personnummerID, string förnamn, string efternamn)
        {
            PersonnummerId = personnummerID;
            Förnamn = förnamn;
            Efternamn = efternamn;
        }
        public string Namn()
        {
            return Förnamn + " " + Efternamn;
        }
    }
}
