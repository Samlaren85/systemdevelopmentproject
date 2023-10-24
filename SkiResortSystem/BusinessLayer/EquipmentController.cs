using DataLayer;
using EntityLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class EquipmentController
    {
        private UnitOfWork unitOfWork;
        public EquipmentController()
        {
            unitOfWork = new UnitOfWork();
        }


        /// <summary>
        /// Söker upp utrustning via typ och storlek
        /// </summary>
        /// <param name="typ"></param>
        /// <param name="storlek"></param>
        /// <returns></returns>
        public Utrustning FindUtrustning(string? typ, string storlek)
        {
            return unitOfWork.UtrustningRepository.FirstOrDefault(u => (u.UtrustningsBenämning.Contains(typ) && u.Storlek.Equals(storlek)));
        }
        /// <summary>
        /// Söker upp utrustning via typ i databasen
        /// </summary>
        /// <param name="typ"></param>
        /// <returns></returns>
        public IList<Utrustning> FindUtrustning(string? typ)
        {
            return unitOfWork.UtrustningRepository.Find(u => (typ == null || typ == string.Empty || u.UtrustningsBenämning.Contains(typ)));
        }

        /// <summary>
        /// Söker upp tillgänglig utrustning i databasen
        /// </summary>
        /// <param name="fetchDate"></param>
        /// <param name="returnDate"></param>
        /// <param name="typ"></param>
        /// <returns></returns>
        public IList<Utrustning> FindTillgängligUtrustning(DateTime? fetchDate, DateTime? returnDate, string? typ)
        {
            IList<Utrustning> result = new List<Utrustning>();
            foreach (Utrustning u in FindUtrustning(typ))
            {
                bool found = false; 
                foreach (Utrustningsbokning ub in FindUtrustningsbokningar(fetchDate, returnDate))
                { 
                    if (u.Equals(ub.Utrustning))
                    {
                        found = true; 
                        break;
                    }
                }
                if (!found) result.Add(u);
            }
            return result;
        }

        /// <summary>
        /// Söker upp ej makulerade utrustningsbokningar
        /// </summary>
        /// <param name="fetchDate"></param>
        /// <param name="returnDate"></param>
        /// <param name="typAvUtrustning"></param>
        /// <returns></returns>
        public IList<Utrustningsbokning> FindUtrustningsbokningar(DateTime? fetchDate, DateTime? returnDate, string typAvUtrustning = null!)
        {
            return unitOfWork.UtrustningsbokningsRepository.Find(u => ((fetchDate == null || u.Hämtasut == fetchDate) && (returnDate == null || u.Lämnasin == returnDate) && (typAvUtrustning == null || typAvUtrustning == string.Empty || u.Utrustning.UtrustningsBenämning.Contains(typAvUtrustning))) && u.Utrustningsstatus != Status.Makulerad);
        }
       
        /// <summary>
        /// Söker upp ej makulerad utrustningsbokning
        /// </summary>
        /// <param name="utrustningsbokning"></param>
        /// <returns></returns>
        public Utrustningsbokning FindUtrustningsbokningar(Utrustningsbokning utrustningsbokning)
        {
            return unitOfWork.UtrustningsbokningsRepository.FirstOrDefault(u => u.Equals(utrustningsbokning) && u.Utrustningsstatus!= Status.Makulerad);
        }

        /// <summary>
        /// Skapar ett utrustningsboknings objekt utan att spara det till databasen
        /// </summary>
        /// <param name="utrustning"></param>
        /// <param name="frånDatum"></param>
        /// <param name="tillDatum"></param>
        /// <param name="selectedEquipmentOrder"></param>
        /// <returns></returns>
        public Utrustningsbokning CreateUtrustningsbokning(Utrustning utrustning, DateTime frånDatum, DateTime tillDatum, Bokning? selectedEquipmentOrder)
        {
            return new Utrustningsbokning(Status.Kommande, frånDatum, tillDatum, selectedEquipmentOrder, utrustning);
        }

        /// <summary>
        /// Uppdaterar befintliga objekt eller sparar nyupplagda objekt till databasen
        /// </summary>
        /// <param name="utrustningsbokningar"></param>
        public void SaveEquipmentBooking(List<Utrustningsbokning> utrustningsbokningar)
        {
           foreach (Utrustningsbokning utr in utrustningsbokningar)
            {
                if (unitOfWork.UtrustningsbokningsRepository.FirstOrDefault(ub => ub.Equals(utr)) != null) unitOfWork.UtrustningsbokningsRepository.Update(utr);
                else unitOfWork.UtrustningsbokningsRepository.Add(utr);
            }
            unitOfWork.Save();
        }

        /// <summary>
        /// Sätter utrustningsbokningens status till makulerad
        /// </summary>
        /// <param name="utrustningsbokning"></param>
        /// <returns></returns>
        public bool RemoveEquipmentBooking(Utrustningsbokning utrustningsbokning)
        {
            unitOfWork.UtrustningsbokningsRepository.FirstOrDefault(u => u.Equals(utrustningsbokning)).Utrustningsstatus = Status.Makulerad;
            unitOfWork.Save();
            return true;
        }

        public IList<List<string>> VisaBeläggningen(DateTime franDatum, DateTime tillDatum)
        {
            List<Utrustning> inaktuellaFaciliteter = new List<Utrustning>(); // Används som hjälp för filtrering
            IList<Utrustning> dataColumn2 = new List<Utrustning>();
            IList<Utrustning> dataColumn3 = new List<Utrustning>();
            IList<Utrustning> dataColumn4 = new List<Utrustning>();
            IList<Utrustning> dataColumn5 = new List<Utrustning>();
            IList<Utrustning> dataColumn6 = new List<Utrustning>();

            TimeSpan dateDifference = tillDatum - franDatum;
            int periodSlutdatum = (int)dateDifference.TotalDays;

            string utrustningsTyp = "Alpinskidor";
            dataColumn2 = FindUtrustning(utrustningsTyp); 
            utrustningsTyp = "Längdskidor";
            dataColumn3 = FindUtrustning(utrustningsTyp); 
            utrustningsTyp = "Snowboard";
            dataColumn4 = FindUtrustning(utrustningsTyp);
            utrustningsTyp = "Skoter";
            dataColumn5 = FindUtrustning(utrustningsTyp); 

            dataColumn4 = dataColumn4.Except(FindUtrustning("Snowboardboots")).ToList();

            List<string> DatumColumnList1 = new List<string>();

            for (int i = 0; i < periodSlutdatum; i++)
            {
                DatumColumnList1.Add(franDatum.AddDays(i).ToShortDateString());
            }

            List<string> AlpinColumnList2 = new List<string>();
            List<string> LängdColumnList3 = new List<string>();
            List<string> SnowboardColumnList4 = new List<string>();
            List<string> SkoterColumnList5 = new List<string>();


            // Denna foreach-loop används för att lägga samtliga listor som ska visas i tabellen inom boendemodulen/Visa beläggning i en gemensam lista.
            foreach (string datum in DatumColumnList1)
            {
                AlpinColumnList2.Add(Räkneverk(dataColumn2, datum).ToString());
                LängdColumnList3.Add(Räkneverk(dataColumn3, datum).ToString());
                SnowboardColumnList4.Add(Räkneverk(dataColumn4, datum).ToString());
                SkoterColumnList5.Add(Räkneverk(dataColumn5, datum).ToString());
            }

            // columnData är det gemensamma lista som används för att hämta och presentera data i visa beläggnings fliken(boendemodulen)
            IList<List<string>> columnData = new List<List<string>>
            {
                DatumColumnList1,
                AlpinColumnList2,
                LängdColumnList3,
                SnowboardColumnList4,
                SkoterColumnList5
            };


            return columnData; //Hur ska denna faktiskt se ut?

        }
        /// <summary>
        /// Om BokningsRef == null VID datum så läggs objektet till på samtliga platser(uppräkningen sker alltså) annars tas inte uppräkning med på de platser där värdet är annat än NULL
        /// </summary>
        /// <param name="Lista"></param>
        /// <param name="datum"></param>
        /// <returns></returns>
        public int Räkneverk(IList<Utrustning> Lista, string datum)
        {
            int antal = 0;
            foreach (Utrustning u in Lista)
            {
                if (u.BokningsRef != null)
                {
                    bool bokad = false;
                    foreach (Utrustningsbokning b in u.BokningsRef)
                    {
                        if (b.Hämtasut <= DateTime.Parse(datum) && b.Lämnasin >= DateTime.Parse(datum))
                            bokad = true;
                    }
                    if (!bokad) antal++;
                }
                else antal++;
                
                
            }
            return antal;
        }
    }
}
