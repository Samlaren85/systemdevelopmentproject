using DataLayer;
using EntityLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class CostumerController
    {
        public UnitOfWork unitOfWork;

        public CostumerController()
        {
            unitOfWork = new UnitOfWork();
        }
        public Kund AddPrivateCostumer(string firstname, string lastname, string streetAdress, int postalCode, string city, string phoneNumber)
        {

            Privatkund privateCostumer = new Privatkund() { Förnamn = firstname, Efternamn = lastname };
            unitOfWork.PrivatkundRepository.Add(privateCostumer);
            Kund costumer = new Kund() { Privatkund = privateCostumer, Gatuadress = streetAdress, Postnummer = postalCode, Ort = city, Telefonnummer = phoneNumber, Rabatt = 0, Kreditgräns = 12000 };
            unitOfWork.KundRepository.Add(costumer);
            unitOfWork.Save();
            return costumer;
        }

        public Kund AddCompanyCostumer(string organisationName,string contact, string streetAdress, int postalCode, string city, string phoneNumber)
        {
            Företagskund companyCostumer = new Företagskund() { Företagsnamn = organisationName, Kontaktperson = contact };
            unitOfWork.FöretagskundRepository.Add(companyCostumer);
            Kund costumer = new Kund() { företagskund = companyCostumer, Gatuadress = streetAdress, Postnummer = postalCode, Ort = city, Telefonnummer = phoneNumber, Rabatt = 0, Kreditgräns = 12000 };
            unitOfWork.KundRepository.Add(costumer);
            unitOfWork.Save();
            return costumer;
        }
        public Kund FindCostumer(string costumerIdentifier)
        {
            return unitOfWork.KundRepository.FirstOrDefault(c => (c.KundId == costumerIdentifier || c.Namn().Contains(costumerIdentifier)), PrivatKund, FöretagsKund);
        }
        public bool ChangeCostumer(Kund costumer)
        {
            bool done = unitOfWork.KundRepository.Update(costumer);
            if (done) unitOfWork.Save();
            return done;
        }
        public bool RemoveCostumer(Kund costumer)
        {
            bool done = unitOfWork.KundRepository.Remove(costumer);
            if (done) unitOfWork.Save();
            return done;
        }
    }
}
