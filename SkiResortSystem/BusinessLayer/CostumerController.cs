using DataLayer;
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
        public Kund AddPrivateCostumer(string firstname, string lastname, string streetAdress, string postalCode, string city, string phoneNumber)
        {

            PrivatKund privateCostumer = new PrivatKund() { firstname, lastname };
            unitOfWork.PrivatKund.Add(privateCostumer);
            Kund costumer = new Kund() { privatkund = privateCostumer, gatuadress = streetAdress, postnummer = postalCode, stad = city, telefonnummer = phoneNumber };
            unitOfWork.KundRepository.Add(costumer);
            unitOfWork.Save();
            return costumer;
        }

        public Kund AddCompanyCostumer(string organisationName,string contact, string streetAdress, string postalCode, string city, string phoneNumber)
        {
            FöretagsKund companyCostumer = new FöretagsKund() { organisationName, contact };
            unitOfWork.FöretagsKund.Add(companyCostumer);
            Kund costumer = new Kund() { företagskund = companyCostumer, gatuadress = streetAdress, postnummer = postalCode, stad = city, telefonnummer = phoneNumber };
            unitOfWork.KundRepository.Add(costumer);
            unitOfWork.Save();
            return costumer;
        }
        public Kund FindCostumer(string costumerIdentifier)
        {
            return unitOfWork.KundRepository.Find(c => c.KundId == costumerIdentifier, PrivatKund, FöretagsKund);
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
