using DataLayer;
using EntityLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class CustomerController
    {
        public UnitOfWork unitOfWork;

        public CustomerController()
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
        public Kund AddPrivateCustomer(string personalNumber, string firstname, string lastname, string streetAdress, string postalCode, string city, string phoneNumber, string epost)
        {

            Privatkund privateCustomer = new Privatkund(personalNumber, firstname, lastname);
            unitOfWork.PrivatkundRepository.Add(privateCustomer);
            Kund customer = new Kund(0, 12000, streetAdress, postalCode, city, phoneNumber, epost, null, privateCustomer);
            unitOfWork.KundRepository.Add(customer);
            unitOfWork.Save();
            return customer;
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
        public Kund AddCompanyCustomer(string organistaionNumber, string organisationName,string contact, string visitAdress, string visitPostalCode, string visitCity, string streetAdress, string postalCode, string city, string phoneNumber, string epost)
        {
            Företagskund companyCustomer = new Företagskund(organistaionNumber,organisationName,contact, visitAdress, visitPostalCode, visitCity);
            unitOfWork.FöretagskundRepository.Add(companyCustomer);
            Kund customer = new Kund(0, 12000, streetAdress, postalCode, city, phoneNumber, epost, companyCustomer, null);
            unitOfWork.KundRepository.Add(customer);
            unitOfWork.Save();
            return customer;
        }
 
        /// <summary>
        /// Ändrar en vald kund i databasen om den hittas
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        public bool ChangeCustomer(Kund customer)
        {
            bool done = unitOfWork.KundRepository.Update(customer);
            if (done) unitOfWork.Save();
            return done;
        }
        /// <summary>
        /// Tar bort en kund ur databasen om kunden inte har obetalda fakturor eller kommande bokningar.
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        public bool RemoveCustomer(Kund customer)
        {
            bool go = true;
            bool done = false;
            if (customer.BokningsRef != null)
            {
                foreach (Bokning b in customer.BokningsRef)
                {
                    if (b.Bokningsstatus != Status.Makulerad)
                    {
                        foreach (Faktura f in b.Fakturaref)
                        {
                            if (f.Fakturastatus == Status.Obetald)
                            {
                                go = false;
                                throw new Exception("Kunden har obetalda fakturor,\nkontrollera med ekonomi innan kunden kan tas bort!");
                            }
                        }
                        if (b.Bokningsstatus == Status.Kommande)
                        {
                            go = false;
                            throw new Exception("Kunden har kommande bokningar,\n makulera dessa innan kunden kan tas bort");
                        }
                    }
                }
            }
            if (go)
            { 
                foreach (Bokning b in customer.BokningsRef)
                {
                    b.KundID = unitOfWork.KundRepository.FirstOrDefault(k => k.KundID == "00000000-0000");
                }
                done = unitOfWork.KundRepository.Remove(customer);
                if (done) unitOfWork.Save();
            }
            return done;
        }


        /// <summary>
        /// Söker kund i databasen
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <returns></returns>
        public List<Kund> SearchCustomers(string searchTerm)
        {

            List<Kund> x = unitOfWork.KundRepository
                .Find(c =>
                    !c.KundID.Equals("00000000-0000") && (
                    c.KundID.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    (c.Privatkund!= null && c.Privatkund.Namn().Contains(searchTerm, StringComparison.OrdinalIgnoreCase)) ||
                    (c.Företagskund != null && c.Företagskund.Företagsnamn.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))), x => x.Privatkund, x => x.Företagskund)
                .ToList();

            return x;
        }
        public List<Kund> GetSearchResults(string searchTerm)
        {
            return SearchCustomers(searchTerm);
        }


    }
}
