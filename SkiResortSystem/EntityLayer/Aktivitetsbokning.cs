using System.ComponentModel.DataAnnotations;

namespace EntityLayer
{
    public class Aktivitetsbokning
    {
        [Key]
        public int AktivitetsbokningsId { get; set; }
        public Status AktivitetsStatus { get; set; }
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
            AktivitetsStatus = Status.Kommande;
        }
    }
}
