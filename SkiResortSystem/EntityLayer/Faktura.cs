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
                if (value == Status.Obetald || value == Status.Makulerad)
                {
                    fakturastatus = value;
                }
                else
                {
                    throw new ArgumentException("Otillåten status");
                }
            }
        }
        public DateTime Förfallodatum {  get; set; }
        public Bokning Bokningsref { get; set; }

        public DateTime Fakturadatum { get; set; }
        public float Totalpris { get; set; }
        public float Moms { get; set; }
        public Faktura()
        {
            
        }
        public Faktura(DateTime fakturadatum, float totalpris, float moms, Bokning bokning)
        {
            _antalFakturor++;
            FakturaID = "FAKT" + _antalFakturor.ToString("000000");
            Fakturadatum = fakturadatum;
            Förfallodatum = Fakturadatum.AddDays(30);
            Totalpris = totalpris; 
            Moms = moms;
            Bokningsref = bokning;
            Fakturastatus = Status.Obetald;
        }
    }
}
