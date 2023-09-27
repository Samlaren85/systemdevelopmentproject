﻿using DataLayer;
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
        public Kund AddPrivateCustomer(string personalNumber, string firstname, string lastname, string streetAdress, int postalCode, string city, string phoneNumber)
        {

            Privatkund privateCustomer = new Privatkund(personalNumber, firstname, lastname);
            unitOfWork.PrivatkundRepository.Add(privateCustomer);
            Kund customer = new Kund(0, 12000, streetAdress, postalCode, city, phoneNumber, null, privateCustomer);
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
        public Kund AddCompanyCustomer(string organistaionNumber, string organisationName,string contact, string streetAdress, int postalCode, string city, string phoneNumber)
        {
            Företagskund companyCustomer = new Företagskund(organistaionNumber,organisationName,contact);
            unitOfWork.FöretagskundRepository.Add(companyCustomer);
            Kund customer = new Kund(0, 12000, streetAdress, postalCode, city, phoneNumber, companyCustomer, null);
            unitOfWork.KundRepository.Add(customer);
            unitOfWork.Save();
            return customer;
        }
        /// <summary>
        /// Söker specifik kund i databasen
        /// </summary>
        /// <param name="customerIdentifier"></param>
        /// <returns></returns>
        public Kund FindCustomer(string customerIdentifier)
        {
            return unitOfWork.KundRepository.FirstOrDefault(c => (c.KundID == customerIdentifier 
                                                            || c.Privatkund.Namn().Contains(customerIdentifier) 
                                                            || c.Företagskund.Företagsnamn.Contains(customerIdentifier)),
                                                            x => x.Privatkund, x => x.Företagskund);
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
        /// Tar bort en kund ur databasen
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        public bool RemoveCustomer(Kund customer)
        {
            bool done = unitOfWork.KundRepository.Remove(customer);
            if (done) unitOfWork.Save();
            return done;
        }
    }
}