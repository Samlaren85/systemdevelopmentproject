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

        public Bokning CreateBokning(DateTime ankomsttid, DateTime avresetid, Användare användarID, Kund kundID, List<Facilitet> facilitetsID, string antalpersoner)
        {
            Bokning bokning = new Bokning(ankomsttid, avresetid, användarID, kundID, facilitetsID, antalpersoner);
            return bokning;
        }
        public void SparaBokning(Bokning bokning)
        {
            unitOfWork.BokningsRepository.Add(bokning);
            unitOfWork.Save();
        }

        public void UppdateraBokning(Bokning bokning)
        {
            unitOfWork.BokningsRepository.Update(bokning);
            unitOfWork.Save();

        }

        public IList<Bokning> FindMasterBooking(string searchString, DateTime? Ankomst, DateTime? Avresa)
        {
            return unitOfWork.BokningsRepository.Find(b =>
                    ((b.BokningsID.Contains(searchString, StringComparison.OrdinalIgnoreCase) ||
                    (b.KundID.Privatkund != null && b.KundID.Privatkund.Namn().Contains(searchString, StringComparison.OrdinalIgnoreCase)) ||
                    (b.KundID.Företagskund != null && b.KundID.Företagskund.Företagsnamn.Contains(searchString, StringComparison.OrdinalIgnoreCase))) &&
                    (b.Ankomsttid >= Ankomst && b.Avresetid <= Avresa)) &&
                    (b.Bokningsstatus != Status.Makulerad),
                    x => x.KundID, x => x.KundID.Privatkund, x => x.KundID.Företagskund);
        }

        public IList<Bokning> FindMasterBooking(string searchString)
        {
            return unitOfWork.BokningsRepository.Find(b => 
                    (b.BokningsID.Contains(searchString, StringComparison.OrdinalIgnoreCase) ||
                    (b.KundID.Privatkund != null && b.KundID.Privatkund.Namn().Contains(searchString, StringComparison.OrdinalIgnoreCase)) ||
                    (b.KundID.Företagskund != null && b.KundID.Företagskund.Företagsnamn.Contains(searchString, StringComparison.OrdinalIgnoreCase)) &&
                    (b.Bokningsstatus != Status.Makulerad)),
                    x => x.KundID, x => x.KundID.Privatkund, x => x.KundID.Företagskund);
        }

        public void RemoveBokning(Bokning bokning)
        {
            bool Done = unitOfWork.BokningsRepository.Remove(bokning);
            if (Done)
            {
                unitOfWork.Save();
            }
        }
    }
}
