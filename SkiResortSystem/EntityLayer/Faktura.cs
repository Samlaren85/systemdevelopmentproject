﻿using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace EntityLayer
{
    public class Faktura : IComparable<Faktura>
    {
        [Key]
        public string FakturaId { get; set; }
        private Status fakturastatus;
        public Status Fakturastatus
        {
            get { return fakturastatus; }
            set
            {
                if (value == Status.Obetald || value == Status.Betald || value == Status.Makulerad)
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
            Fakturadatum = fakturadatum;
            Förfallodatum = Fakturadatum.AddDays(30);
            Totalpris = totalpris; 
            Moms = moms;
            Bokningsref = bokning;
            Fakturastatus = Status.Obetald;
        }

      /// <summary>
      /// Metoden används för att jämföra de två fakturor som genereras mot kund i fakturaskeendet, för att lägga återbetalningssyddet
      /// på rätt faktura av de två (faktura1) här benämnds thisFakturaID.
      /// </summary>
      /// <param name="obj"></param>
      /// <returns></returns>
      /// <exception cref="ArgumentException"></exception>
        public int CompareTo(Faktura? obj)
        {
            int result = 0;
            int thisFakturaID = ExtractNumericPart(obj.FakturaId);
            int otherFakturaID = ExtractNumericPart(FakturaId);
            if (obj != null)
            {
                if (thisFakturaID > otherFakturaID)
                {
                    return result = 1;
                }
                else if (thisFakturaID < otherFakturaID)
                {
                   return result = -1;
                }
                else
                {
                    return result = 0;
                }
            }
            else
            {
                throw new ArgumentException("Felaktigt format för FakturaID");
            }
        }

        /// <summary>
        /// Hämtar ut siffror ur en sträng.
        /// </summary>
        /// <param name="fakturaID"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        private int ExtractNumericPart(string fakturaID)
        {
            // Tar fram den numeriska delen utav en sträng
            Match match = Regex.Match(fakturaID, @"\d+");

            if (match.Success)
            {
                // Parsar sifferdelen till en integer
                return int.Parse(match.Value);
            }

            // If no numeric part found, handle accordingly
            throw new ArgumentException("Invalid FakturaID format");
        }
    }
}
