using DataLayer;
using EntityLayer;
using System;
using System.Collections.Generic;
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
            return unitOfWork.UtrustningsbokningsRepository.Find(u => ((fetchDate == null || u.Lämnasin <= fetchDate) && (returnDate == null || u.Hämtasut >= returnDate) || (typAvUtrustning == null || typAvUtrustning == string.Empty || u.Utrustning.UtrustningsBenämning.Contains(typAvUtrustning))) && u.Utrustningsstatus != Status.Makulerad);
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
    }
}
