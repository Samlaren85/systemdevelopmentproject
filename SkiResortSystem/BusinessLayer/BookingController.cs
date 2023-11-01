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

        /// <summary>
        /// Skapar en bokning med facilitet. Väntar med att spara till databasen för att avbeställningskydd kan väljas i bokningsöversikten. Man kan välja att sedan spara eller ta bort bokningen.
        /// </summary>
        /// <param name="ankomsttid"></param>
        /// <param name="avresetid"></param>
        /// <param name="användarID"></param>
        /// <param name="kundID"></param>
        /// <param name="facilitetsID"></param>
        /// <param name="antalpersoner"></param>
        /// <returns></returns>
        public Bokning CreateBokning(DateTime ankomsttid, DateTime avresetid, Användare användarID, Kund kundID, List<Facilitet> facilitetsID, string antalpersoner)
        {
            Bokning bokning = new Bokning(ankomsttid, avresetid, användarID, kundID, facilitetsID, antalpersoner);
            return bokning;
        }

        /// <summary>
        /// Sparar ner bokning till databasen.
        /// </summary>
        /// <param name="bokning"></param>
        public void SparaBokning(Bokning bokning)
        {
            unitOfWork.BokningsRepository.Add(bokning);
            unitOfWork.Save();
        }

        /// <summary>
        /// Uppdaterar bokning till databasen, används när bokning ska ändras eller "tas bort"- då läggs status makulerad till.
        /// </summary>
        /// <param name="bokning"></param>
        public void UppdateraBokning(Bokning bokning)
        {
            unitOfWork.BokningsRepository.Update(bokning);
            unitOfWork.Save();

        }

        /// <summary>
        /// Metoder för sökning av bokningar nedan med olika inparametrar beroende på om man vill hitta alla eller specifika datum etc.
        /// </summary>
        /// <param name="searchString"></param>
        /// <param name="Ankomst"></param>
        /// <param name="Avresa"></param>
        /// <returns></returns>
        public IList<Bokning> FindMasterBooking(string searchString, DateTime? Ankomst, DateTime? Avresa)
        {
            return unitOfWork.BokningsRepository.Find(b =>
                    (searchString == null || (b.Bokningsnummer.Contains(searchString, StringComparison.OrdinalIgnoreCase) ||
                    (b.KundID.Privatkund != null && b.KundID.Privatkund.Namn().Contains(searchString, StringComparison.OrdinalIgnoreCase)) ||
                    (b.KundID.Företagskund != null && b.KundID.Företagskund.Företagsnamn.Contains(searchString, StringComparison.OrdinalIgnoreCase)))) &&
                    ((Ankomst == null || b.Ankomsttid == Ankomst) && (Avresa == null || b.Avresetid == Avresa)) &&
                    (b.Bokningsstatus != Status.Makulerad),
                    x => x.KundID, x => x.KundID.Privatkund, x => x.KundID.Företagskund);
        }

        public IList<Bokning> FindMasterBooking(string searchString)
        {
            return unitOfWork.BokningsRepository.Find(b =>
                    (b.Bokningsnummer.Contains(searchString, StringComparison.OrdinalIgnoreCase) ||
                    (b.KundID.Privatkund != null && b.KundID.Privatkund.Namn().Contains(searchString, StringComparison.OrdinalIgnoreCase)) ||
                    (b.KundID.Företagskund != null && b.KundID.Företagskund.Företagsnamn.Contains(searchString, StringComparison.OrdinalIgnoreCase))) &&
                    (b.Bokningsstatus != Status.Makulerad),
                    x => x.KundID, x => x.KundID.Privatkund, x => x.KundID.Företagskund, f => f.Fakturaref);
        }
        public IList<Bokning> FindMasterBooking()
        {
            return unitOfWork.BokningsRepository.Find(b =>
                    b.Betalningsstatus.Equals(Status.Ofakturerad) && b.Bokningsstatus.Equals(Status.Kommande));
        }


        
    }
}
