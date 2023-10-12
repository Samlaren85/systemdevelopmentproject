using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer
{
    public class Faktura
    {
        private static int _antalFakturor = 0;
        [Key]
        public string FakturaID { get; set; }
        private Status fakturastatus;
        public Status Fakturastatus
        {
            get { return fakturastatus; }
            set
            {
                if (value == Status.Obetald || value == Status.Arkiverad)
                {
                    fakturastatus = value;
                }
                else
                {
                    throw new ArgumentException("Otillåten status");
                }
            }
        }
        private DateTime förfallodatum;
        public DateTime Förfallodatum 
        {
            get { return Förfallodatum; }
            set
            {
                    if (Förfallodatum == Fakturadatum.AddDays(20))
                {
                    förfallodatum = value;
                }
                else if (Förfallodatum != Fakturadatum.AddDays(20))
                {
                    foreach (Bokning b in BokningsID)
                    {
                        if (Förfallodatum != b.Avresetid.AddDays(30))
                        {
                            förfallodatum = b.Avresetid.AddDays(30);
                        }
                        
                    }
                }
                else
                {
                    throw new ArgumentException("Felaktigt förfallodatum");
                }
            } 
        }
        public ICollection<Bokning> BokningsID { get; set; }

        public DateTime Fakturadatum { get; set; }
        public float Totalpris { get; set; }
        public float Moms { get; set; }
        public Faktura()
        {
            
        }
        public Faktura(DateTime fakturadatum, float totalpris, float moms)
        {
            _antalFakturor++;
            FakturaID = "FAKT" + _antalFakturor.ToString("000000");
            Fakturadatum = fakturadatum; 
            Totalpris = totalpris; 
            Moms = moms;
        }
    }
}
