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


        public Utrustning FindUtrustning(string? typ, string storlek)
        {
            return unitOfWork.UtrustningRepository.FirstOrDefault(u => (u.UtrustningsBenämning.Contains(typ) && u.Storlek.Equals(storlek)));
        }
        public IList<Utrustning> FindUtrustning(string? typ)
        {
            return unitOfWork.UtrustningRepository.Find(u => (typ == null || typ == string.Empty || u.UtrustningsBenämning.Contains(typ)));
        }

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

        public IList<Utrustningsbokning> FindUtrustningsbokningar(DateTime? fetchDate, DateTime? returnDate, string typAvUtrustning = null!)
        {
            return unitOfWork.UtrustningsbokningsRepository.Find(u => ((fetchDate == null || u.Lämnasin <= fetchDate) && (returnDate == null || u.Hämtasut >= returnDate) || (typAvUtrustning == null || typAvUtrustning == string.Empty || u.Utrustning.UtrustningsBenämning.Contains(typAvUtrustning))));
        }
        public Utrustningsbokning FindUtrustningsbokningar(Utrustningsbokning utrustningsbokning)
        {
            return unitOfWork.UtrustningsbokningsRepository.FirstOrDefault(u => u.Equals(utrustningsbokning));
        }

        public Utrustningsbokning CreateUtrustningsbokning(Utrustning utrustning, DateTime frånDatum, DateTime tillDatum, Bokning? selectedEquipmentOrder)
        {
            return new Utrustningsbokning(Status.Kommande, frånDatum, tillDatum, selectedEquipmentOrder, utrustning);
        }

        public void SaveEquipmentBooking(List<Utrustningsbokning> utrustningsbokningar)
        {
           foreach (Utrustningsbokning utr in utrustningsbokningar)
            {
                if (unitOfWork.UtrustningsbokningsRepository.FirstOrDefault(ub => ub.Equals(utr)) != null) unitOfWork.UtrustningsbokningsRepository.Update(utr);
                else unitOfWork.UtrustningsbokningsRepository.Add(utr);
            }
            unitOfWork.Save();
        }

        public bool RemoveEquipmentBooking(Utrustningsbokning utrustningsbokning)
        {
            bool done = unitOfWork.UtrustningsbokningsRepository.Remove(utrustningsbokning);
            if (done) { unitOfWork.Save(); }
            return done;
        }
    }
}
