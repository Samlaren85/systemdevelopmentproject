using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using DataLayer;
using EntityLayer;


namespace BusinessLayer
{
    public class BookingController
    {
        public UnitOfWork unitOfWork;

        public BookingController()
        {
            unitOfWork = new UnitOfWork();
        }

        public Bokning CreateBokning(DateTime ankomsttid, DateTime avresetid, Användare användarID, Kund kundID, List<Facilitet> facilitetsID, List<Utrustning> utrustningID, List<Aktivitet> AktivitetID)
        {
            Bokning bokning = new Bokning(ankomsttid, avresetid, användarID, kundID, facilitetsID, utrustningID, AktivitetID);
            return bokning;
        }
        public void SparaBokning(Bokning bokning)
        {
            unitOfWork.BokningsRepository.Add(bokning);
            unitOfWork.Save();
        }
      

        public IList<Bokning> FindMasterBooking(string searchString)
        {
            return unitOfWork.BokningsRepository.Find(b => b.BokningsID.Contains(searchString) || b.KundID.Privatkund.Namn().Contains(searchString) || b.KundID.Företagskund.Företagsnamn.Contains(searchString), x => x.KundID, x => x.KundID.Företagskund, x => x.KundID.Företagskund);
        }

    }
}
