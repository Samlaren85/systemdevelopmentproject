using System.ComponentModel.DataAnnotations;

namespace EntityLayer
{
    public class Utrustningsbokning
    {
        [Key]
        public int UtrustningsbokningsId { get; set; }
        private Status utrustningsstatus;
        public Status Utrustningsstatus {
           get { return utrustningsstatus; }
           set 
           {
                if (value == Status.Kommande || (value >= Status.Utlämnad && value < Status.Genomförd) || value == Status.Makulerad)
                {
                    utrustningsstatus = value;
                }
                else
                {
                    throw new ArgumentException("Otillåten status");
                }
            } 
        }
        public DateTime Hämtasut {  get; set; }
        public DateTime Lämnasin { get; set; }
        public Bokning Bokning { get; set; }
        public Utrustning Utrustning { get; set; }
        public string? Hylla {  get; set; }
        private float totalPris;
        public float TotalPris 
        { 
            get { return totalPris; } 
            set 
            {
                TimeSpan bokningstid = Lämnasin - Hämtasut;
                totalPris = (float)(Utrustning.Pris*bokningstid.TotalDays) ;
            } 
        }

        public Utrustningsbokning()
        {
        }
        public Utrustningsbokning(Status status, DateTime hämta, DateTime lämna, Bokning bokning, Utrustning utrustning) 
        {
            Utrustningsstatus = status;
            Hämtasut = hämta;
            Lämnasin = lämna;
            Bokning = bokning;
            Utrustning = utrustning;
            TotalPris = totalPris;
        }
    }
}
