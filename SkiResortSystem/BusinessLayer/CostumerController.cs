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
        /// <summary>
        /// Lägger till en privatkund i databasen
        /// </summary>
        /// <param name="personalNumber"></param>
        /// <param name="firstname"></param>
        /// <param name="lastname"></param>
        /// <param name="streetAdress"></param>
        /// <param name="postalCode"></param>
        /// <param name="city"></param>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        public Kund AddPrivateCostumer(string personalNumber, string firstname, string lastname, string streetAdress, int postalCode, string city, string phoneNumber)
        {

            Privatkund privateCostumer = new Privatkund(personalNumber, firstname, lastname);
            unitOfWork.PrivatkundRepository.Add(privateCostumer);
            Kund costumer = new Kund(0, 12000, streetAdress, postalCode, city, phoneNumber, null, privateCostumer);
            unitOfWork.KundRepository.Add(costumer);
            unitOfWork.Save();
            return costumer;
        }

        /// <summary>
        /// Lägger till en företagskund i databasen
        /// </summary>
        /// <param name="organistaionNumber"></param>
        /// <param name="organisationName"></param>
        /// <param name="contact"></param>
        /// <param name="streetAdress"></param>
        /// <param name="postalCode"></param>
        /// <param name="city"></param>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        public Kund AddCompanyCostumer(string organistaionNumber, string organisationName,string contact, string streetAdress, int postalCode, string city, string phoneNumber)
        {
            Företagskund companyCostumer = new Företagskund(organistaionNumber,organisationName,contact);
            unitOfWork.FöretagskundRepository.Add(companyCostumer);
            Kund costumer = new Kund(0, 12000, streetAdress, postalCode, city, phoneNumber, companyCostumer, null);
            unitOfWork.KundRepository.Add(costumer);
            unitOfWork.Save();
            return costumer;
        }
        /// <summary>
        /// Söker specifik kund i databasen
        /// </summary>
        /// <param name="costumerIdentifier"></param>
        /// <returns></returns>
        public Kund FindCostumer(string costumerIdentifier)
        {
            return unitOfWork.KundRepository.FirstOrDefault(c => (c.KundID == costumerIdentifier 
                                                            || c.Privatkund.Namn().Contains(costumerIdentifier) 
                                                            || c.Företagskund.Företagsnamn.Contains(costumerIdentifier)),
                                                            x => x.Privatkund, x => x.Företagskund);
        }
        /// <summary>
        /// Ändrar en vald kund i databasen om den hittas
        /// </summary>
        /// <param name="costumer"></param>
        /// <returns></returns>
        public bool ChangeCostumer(Kund costumer)
        {
            bool done = unitOfWork.KundRepository.Update(costumer);
            if (done) unitOfWork.Save();
            return done;
        }
        /// <summary>
        /// Tar bort en kund ur databasen
        /// </summary>
        /// <param name="costumer"></param>
        /// <returns></returns>
        public bool RemoveCostumer(Kund costumer)
        {
            bool done = unitOfWork.KundRepository.Remove(costumer);
            if (done) unitOfWork.Save();
            return done;
        }
    }
}
