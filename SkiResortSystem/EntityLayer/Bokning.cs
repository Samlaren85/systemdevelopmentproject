﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer
{
    public class Bokning
    {
        private static int _antalBokningar = 0;
        [Key]
        public string BokningsID { get; set; }
        public float UtnyttjadKredit { get; set; }
        public DateTime Ankomsttid { get; set; }
        public DateTime Avresetid { get; set; }
        public Användare AnvändareID { get; set; }
        public Kund KundID { get; set; }

        [ForeignKey("FacilitetsID")]
        public List<Facilitet> FacilitetID { get; set; }
       
        [ForeignKey("UtrustningsID")]
        public List<Utrustning> UtrustningID { get; set; }

        [ForeignKey("AktivitetsID")]
        public List<Aktivitet> AktivitetID{ get; set; }

        public Bokning()
        {

        }
        public Bokning(DateTime ankomsttid, DateTime avresetid, Användare användareID, Kund kundID, List<Facilitet> facilitetID, List<Utrustning> utrustningID, List<Aktivitet> aktivitetID)
        {
            _antalBokningar++;
            BokningsID = "B" + _antalBokningar.ToString("000000");
            UtnyttjadKredit = 0;
            Ankomsttid = ankomsttid;
            Avresetid = avresetid;
            AnvändareID = användareID;
            KundID = kundID;
            FacilitetID = facilitetID;
            UtrustningID = utrustningID;
            AktivitetID = aktivitetID;
        }

        public override string ToString()
        {
            string customerName = string.Empty;
            if (KundID.Företagskund != null) customerName = KundID.Företagskund.Företagsnamn;
            else if (KundID.Privatkund != null) customerName = KundID.Privatkund.Namn();
            return $"{BokningsID} ({customerName})";
        }
    }
}
