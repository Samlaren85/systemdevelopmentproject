using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer
{
    public class Aktivitet
    {
        private static int _antalAktiviteter = 0;
        [Key]
        public string AktivitetsID { get; set; }
        private Status aktivitetsstatus;
        public Status Aktivitetsstatus
        {
            get { return aktivitetsstatus; }
            set
            {
                if (value == Status.Kommande || value == Status.Genomförd || value == Status.Makulerad)
                {
                    aktivitetsstatus = value;
                }
                else
                {
                    throw new ArgumentException("Otillåten status");
                }
            }
        }
        public Skidskola Skidskola { get; set; }
        public bool Vintersäsong { get; set; }
        public ICollection<Bokning>? BokningsRef { get; set; }
        public Aktivitet()
        {
            
        }
        public Aktivitet(Skidskola skidskola, bool vintersäsong)
        {
            _antalAktiviteter++;
            AktivitetsID = "AKT" + _antalAktiviteter.ToString("000");
            Skidskola = skidskola;
            Vintersäsong = vintersäsong;
        }
    }
}
