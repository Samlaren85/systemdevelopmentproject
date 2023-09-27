using DataLayer;
using EntityLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    /// <summary>
    /// Kontrollerklassen för boendemodulen. Hanterar bokningsförfarandet med avseende på boende-/konferensbokning.
    /// Facilititesbokningar är att anse som bokningshuvud och agerar bärare för övriga typer av bokningar.
    /// </summary>
    public class AccommodationController
    {
        public UnitOfWork unitOfWork;

        /// <summary>
        /// AddKonferenssal-metoden används för att lägga till en ny bokningsbar konferenssal i systemet. 
        /// </summary>
        public Facilitet AddKonferenssal(string konferensBenämning, float facilitetspris)
        {
            Konferenssal konferenssal = new Konferenssal() { konferensBenämning };
            konferenssal.konferensBenämning = unitOfWork.KonferensRepository.Add(konferenssal);
            Facilitet facilitet = new Facilitet() { facilitetspris, konferenssal};
            unitOfWork.Save();
            return facilitet;
        }

        /// <summary>
        /// AddCampingPlats-metoden används för att lägga till en ny bokningsbar campingplats i systemet. 
        /// </summary>
        public Facilitet AddCampingPlats(string campingBenämning, double campingStorlek, double facilitetspris)
        {
            CampingPlats campingplats = new campingPlats() { campingBenämning, campingStorlek };
            unitOfWork.CampingRepository.Add(campingplats);
            Facilitet facilitet = new Facilitet() { facilitetspris, campingplats };
            unitOfWork.FacilitetRepository.Add(facilitet);
            unitOfWork.Save();
            return facilitet;
        }
        
        /// <summary>
        /// AddLägenhet-metoden används för att lägga till en ny bokningsbar lägenhet i systemet. 
        /// </summary>
        /// <param name="facilitetspris"></param>
        /// <param name="lägenhetsbenämning"></param>
        /// <param name="bäddar"></param>
        /// <param name="lägenhetsstorlek"></param>
        /// <returns></returns>
        public Facilitet AddLägenhet(double facilitetspris, string lägenhetsbenämning, int bäddar, double lägenhetsstorlek)
        {
            Lägenhet lägenhet = new Lägenhet() { lägenhetsbenämning, bäddar, lägenhetsstorlek };
            unitOfWork.LägenhetRepository.Add(lägenhet);
            Facilitet facilitet = new Facilitet() { facilitetspris, lägenhet };
            unitOfWork.FacilitetRepository.Add(facilitet);
            unitOfWork.Save();
            return facilitet;
        }
 #region Boknings metoder
        public Bokning BokaBoende(Kund nyKund, Användare aktivAnvändare, Ankomstdatum ankDatum, Facilitet boende, double utnyttjadKredit) //Ska returtyp vara Facilitetsbokning?
        {
            
            Bokning NyBoendeBokning = new Bokning()
            {
                nyKund,
                aktivAnvändare,
                ankDatum,
                utnyttjadKredit,
                boende, //Ska kunna bli lägenhet/camping
            };
            
            unitOfWork.BokningRepository.Add(NyBoendeBokning);
            return NyBoendeBokning;
        }

        public Bokning BokaKonferens(Kund nyKund, Användare aktivAnvändare, Ankomstdatum ankDatum, Facilitet konferenssal, double utnyttjadKredit) //Ska returtyp vara Facilitetsbokning?
        {
            Bokning NyKonferensBokning = new Bokning()
            {
                nyKund,
                aktivAnvändare,
                ankDatum,
                utnyttjadKredit,
                konferenssal, //Ska styras för att alltid motsvara en konferenssal
            };

            unitOfWork.BokningRepository.Add(NyKonferensBokning);
            return NyKonferensBokning;
        }
 #endregion

 #region Metoder för sök
            /// <summary>
            /// Metoden kan användas för att söka fram ett enskilt boende
            /// </summary>
            /// <param name="sökTerm"></param>
            /// <returns></returns>
            public Bokning FindBoende(string sökTerm) // input string == Namn, yyyy-mm-dd, BokningID ??? Detta format vi tänkt oss enl. UC?
        {
            return unitOfWork.BokningsRepository.FirstOrDefault(b => (b.BokningsID == sökTerm  
                                                               || b.KundID.Privatkund.Namn().Contains(sökTerm, StringComparison.OrdinalIgnoreCase)
                                                               || b.Ankomstdatum.Contains(sökTerm, StringComparison.OrdinalIgnoreCase)));
        }

        /// <summary>
        /// En LINQ-sökning för att söka upp kundens boendebokning.
        /// Metoden kontrollerar om namnet, datumet eller bokningsnummret som systemanvändaren anger finns med i listan över bokningar
        /// Metoden hanterar utsökning av namn oberoende av om kunden är privat-/företagskund
        /// </summary>
        /// <returns></returns>
        public IList<Bokning> FindBoenden(string sökTerm, DateTime ankomst, DateTime avresa, string facilitetsTyp) // input string == Namn, yyyy-mm-dd, BokningID ??? Detta format vi tänkt oss enl. UC?
        {
            IList<Bokning> bokningar = unitOfWork.BokningsRepository.Find(b => ((b.BokningsID.Contains(sökTerm, StringComparison.OrdinalIgnoreCase) || (b.KundID != null &&
                                                            (b.KundID.Privatkund.Namn().Contains(sökTerm, StringComparison.OrdinalIgnoreCase) ||
                                                            b.KundID.Företagskund.Företagsnamn.Contains(sökTerm, StringComparison.OrdinalIgnoreCase)))) &&
                                                            (b.Ankomsttid.ToShortDateString == ankomst.ToShortDateString || b.Avresetid.ToShortDateString == avresa.ToShortDateString)
                                                            ), x => x.KundID, x => x.FacilitetID);
            List<Bokning> inaktuellBokningar = new List<Bokning>();
            foreach (Bokning bokning in bokningar)
            {
                foreach(Facilitet facilitet in bokning.FacilitetID)
                {
                    if (facilitet.KonferansID == null && facilitetsTyp.Equals("Konferenssal")) inaktuellBokningar.Add(bokning);
                    if (facilitet.CampingID == null && facilitetsTyp.Equals("Campingplats")) inaktuellBokningar.Add(bokning);
                    if (facilitet.LägenhetsID == null && facilitetsTyp.Equals("Lägenhet")) inaktuellBokningar.Add(bokning);
                }
            }
            return bokningar.Except(inaktuellBokningar).ToList(); //Vad händer om första sökkriteriet blir null?!?
        }

        /// <summary>
        /// Metoden presenterar lediga faciliteter för användaren baserat på val 1.Konferens 2.Lägenhet 3.Camping
        /// </summary>
        /// <param name="facilitetsval"></param>
        /// <returns></returns>
        public static List<Facilitet> FindLedigaFaciliteter(Facilitet facilitetsval)
        {
                    return facilitetsval.Where(facilitetsval =>
                            facilitetsval.konferensID != null && (facilitetsval.Facilitetbokning.Bokning.BokningID != null) ||
                            facilitetsval.lägenhetsID != null && (facilitetsval.Facilitetbokning.Bokning.BokningID != null) ||
                            facilitetsval.campingID != null && (facilitetsval.Facilitetbokning.Bokning.BokningID != null)
                            ).ToList();
        }

        /// <summary>
        /// Metoden hämtar och presenterar en lista för användaren med lägenheter baserat på antal personer som bokningen avser
        /// </summary>
        /// <param name="antalPersoner"></param>
        /// <returns></returns>
        public static List<Facilitet> FindLedigaLägenheter(Lägenhet lägenhet, int antalPersoner) //Endast påbörjad! behöver hjälp
        {
            if (antalPersoner <= 4)
            {
                return lägenhet.Where(antalPersoner =>
                lägenhet.bäddar <= 4 && Bokning.BokningID == null).ToList();
            }
            else
            {
                return lägenhet.Where(antalPersoner =>
                lägenhet.bäddar >= 4 && Bokning.BokningID == null).ToList();
            }
        }
 #endregion
        /// <summary>
        /// Konstruktor för boendemodulen
        /// </summary>
        public AccommodationController() 
        {
            unitOfWork = new UnitOfWork();
            
        }

    }
}
