using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer
{
    public class Utrustningsbokning
    {
        [Key]
        public string UtrustningsbokningsID { get; set; }
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
        [ForeignKey(nameof(Bokning))]
        public Bokning Bokning { get; set; }
        [ForeignKey(nameof(Utrustning))]
        public Utrustning Utrustning { get; set; }
        public DateTime Hämtasut {  get; set; }
        public DateTime Lämnasin { get; set; }
        public Utrustningsbokning() { }
    }
}
