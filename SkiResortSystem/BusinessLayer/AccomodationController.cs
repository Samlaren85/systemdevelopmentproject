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
        #region Metoder för att lägga till logiobjekt

        
        /// <summary>
        /// AddKonferenssal-metoden används för att lägga till en ny bokningsbar konferenssal i systemet. 
        /// </summary>
        public Facilitet AddKonferenssal(string konferensBenämning, float facilitetspris)
        {
            Konferenssal konferenssal = new Konferenssal(konferensBenämning);
            unitOfWork.KonferenssalRepository.Add(konferenssal);
            Facilitet facilitet = new Facilitet(facilitetspris, konferenssal, null, null);
            unitOfWork.Save();
            return facilitet;
        }

        /// <summary>
        /// AddCampingPlats-metoden används för att lägga till en ny bokningsbar campingplats i systemet. 
        /// </summary>
        public Facilitet AddCampingPlats(string campingBenämning, string campingStorlek, float facilitetspris)
        {
            Campingplats campingplats = new Campingplats() { CampingBenämning = campingBenämning, CampingStorlek = campingStorlek };
            unitOfWork.CampingplatsRepository.Add(campingplats);
            Facilitet facilitet = new Facilitet() { Facilitetspris = facilitetspris, CampingID = campingplats };
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
        public Facilitet AddLägenhet(float facilitetspris, string lägenhetsbenämning, int bäddar, string lägenhetsstorlek)
        {
            Lägenhet lägenhet = new Lägenhet() {LägenhetBenämning = lägenhetsbenämning, Bäddar = bäddar, Lägenhetstorlek = lägenhetsstorlek };
            unitOfWork.LägenhetRepository.Add(lägenhet);
            Facilitet facilitet = new Facilitet() {Facilitetspris = facilitetspris, LägenhetsID = lägenhet };
            unitOfWork.FacilitetRepository.Add(facilitet);
            unitOfWork.Save();
            return facilitet;
        }
        #endregion
        #region Metoder för sök
        /// <summary>
        /// Metoden kan användas för att söka fram ett specifikt boende och presentera detaljer för användaren.
        /// </summary>
        /// <param name="sökTerm"></param>
        /// <returns></returns>
        public Bokning FindBoende(string sökTerm)
        {
            return unitOfWork.BokningsRepository.FirstOrDefault(b => (b.BokningsID == sökTerm  
                                                               || b.KundID.Privatkund.Namn().Contains(sökTerm, StringComparison.OrdinalIgnoreCase)
                                                               || b.KundID.Företagskund.Företagsnamn.Contains(sökTerm, StringComparison.OrdinalIgnoreCase)
                                                               || b.Ankomsttid.ToShortDateString().Contains(sökTerm, StringComparison.OrdinalIgnoreCase)), x => x.KundID, x => x.KundID.Privatkund, x => x.KundID.Företagskund);
        }

        /// <summary>
        /// En LINQ-sökning för att söka upp kundens boendebokning.
        /// Metoden kontrollerar om namnet, datumet eller bokningsnummret som systemanvändaren anger finns med i listan över bokningar
        /// Metoden hanterar utsökning av namn oberoende av om kunden är privat-/företagskund
        /// </summary>
        /// <returns></returns>
        public IList<Bokning> FindBoenden(string sökTerm, DateTime ankomst, DateTime avresa, string facilitetsTyp) 
        {
            IList<Bokning> bokningar = unitOfWork.BokningsRepository.Find(b => ((b.BokningsID.Contains(sökTerm, StringComparison.OrdinalIgnoreCase) || (b.KundID != null &&
                                                            (b.KundID.Privatkund.Namn().Contains(sökTerm, StringComparison.OrdinalIgnoreCase) ||
                                                            b.KundID.Företagskund.Företagsnamn.Contains(sökTerm, StringComparison.OrdinalIgnoreCase)))) &&
                                                            (b.Ankomsttid.ToShortDateString == ankomst.ToShortDateString || b.Avresetid.ToShortDateString == avresa.ToShortDateString)
                                                            ), x => x.KundID, x => x.KundID.Privatkund, x => x.KundID.Företagskund);
            List<Bokning> inaktuellBokningar = new List<Bokning>();
            foreach (Bokning bokning in bokningar)
            {
                foreach(Facilitet facilitet in bokning.FacilitetID)
                {
                    if (facilitet.KonferensID == null && facilitetsTyp.Equals("Konferenssal")) inaktuellBokningar.Add(bokning);
                    if (facilitet.CampingID == null && facilitetsTyp.Equals("Campingplats")) inaktuellBokningar.Add(bokning);
                    if (facilitet.LägenhetsID == null && facilitetsTyp.Equals("Lägenhet")) inaktuellBokningar.Add(bokning);
                }
            }
            return bokningar.Except(inaktuellBokningar).ToList(); //Vad händer om första sökkriteriet blir null?!? Ta bort efter testkörning
        }

        /// <summary>
        /// Metoden presenterar lediga faciliteter för användaren baserat på val Konferens, Lägenhet, Camping
        /// </summary>
        /// <param name="facilitetsval"></param>
        /// <returns></returns>
        public List<Facilitet> FindLedigaFaciliteter(string sökTerm)
        {
            IList<Facilitet> allaFaciliteter = unitOfWork.FacilitetRepository.Find(f => true);
            IList<Bokning> allabokningar = unitOfWork.BokningsRepository.Find(b => b.Ankomsttid >= DateTime.Now);

            IList<Facilitet> inaktuellaFaciliteter = new List<Facilitet>();
            foreach (Facilitet f in allaFaciliteter)
            {
                    if (f.KonferensID == null && sökTerm.Equals("Konferenssal")) inaktuellaFaciliteter.Add(f);
                    if (f.CampingID == null && sökTerm.Equals("Campingplats")) inaktuellaFaciliteter.Add(f);
                    if (f.LägenhetsID == null && sökTerm.Equals("Lägenhet")) inaktuellaFaciliteter.Add(f);
            }

            IList<Facilitet> inaktuellaBokningar = new List<Facilitet>();
            foreach (Bokning b in allabokningar) 
            {
                foreach (Facilitet f in b.FacilitetID)
                {
                    inaktuellaBokningar.Add(f);
                }
            }
            List<Facilitet> söktaFaciliteter = allaFaciliteter.Except(inaktuellaFaciliteter).ToList();
            
            return söktaFaciliteter.Except(inaktuellaBokningar).ToList(); //Vad händer om första sökkriteriet blir null?!? ta bort efter testkörning
        }
        public List<Facilitet> FindLedigaFaciliteter(string sökTerm, int antalPersoner)
        {
            List<Facilitet> faciliteter = FindLedigaFaciliteter(sökTerm);
            List<Facilitet> inaktuellaFaciliteter = new List<Facilitet>();

            foreach (Facilitet facilitet in faciliteter)
            {
                if (facilitet.LägenhetsID.Bäddar >= antalPersoner)
                {
                    inaktuellaFaciliteter.Add(facilitet);
                }
            }
            return faciliteter.Except(inaktuellaFaciliteter).ToList();
        }

        public List<T> KontrolleraBeläggning<T>(string sökTerm) //Starta här nästa stittning!
        {
            List<T>myList = new List<T>();
            FindBoende(sökTerm); // Här ska vi få till i vyn 6-Beläggning så att Lägenheter samt konferenssalar splittas till 4 olika kolummner (2st/typ)
            /*FindUtrustning(sökTerm);*/ //Här ska varje enskild typ visas på separat rad (vertikalt) KOMMER ATT SKAPAS I Utrustnings BL09/BL20 sprint2-ish
            /*FindAktiviteter(sökTerm);*/ //Här ska varje aktivitet splittas på enskild rad. Likt Utrustning KOMMER ATT SKAPAS I BL07/BL08 sprint2-ish
            return myList;
        }

        public List<Facilitet> FindLedigaFaciliteterFörBokning(string sökTerm, int antalPersoner, DateTime ankomst, DateTime avrese)
        {
            IList<Bokning> bokningar = unitOfWork.BokningsRepository.Find(b => b.Ankomsttid < avrese && b.Avresetid > ankomst, X => X.FacilitetID);
            IList<Facilitet> Faciliteter = unitOfWork.FacilitetRepository.Find(b => b.FacilitetID.Contains(sökTerm, StringComparison.OrdinalIgnoreCase) && antalPersoner <= b.LägenhetsID.Bäddar, x => x.LägenhetsID);
            List<Facilitet> inaktuellFaciliteter = new List<Facilitet>();
            foreach (Bokning b in bokningar)
            {
                foreach (Facilitet facilitet in b.FacilitetID)
                {
                    inaktuellFaciliteter.Add(facilitet);
                }
            }
            return Faciliteter.Except(inaktuellFaciliteter).ToList();
        }

        public List<Facilitet> FindLedigaLägenheter(int antalpersoner, DateTime ankomst, DateTime avrese)
        {      
                return FindLedigaFaciliteterFörBokning("LGH", antalpersoner,  ankomst,  avrese);          
        }
        public List<Facilitet> FindLedigaCamping(int antalpersoner, DateTime ankomst, DateTime avrese)
        {
            return FindLedigaFaciliteterFörBokning("CAMP", antalpersoner,  ankomst,  avrese);
        }
        public List<Facilitet> FindLedigaKonferens(int antalpersoner, DateTime ankomst, DateTime avrese)
        {
            return FindLedigaFaciliteterFörBokning("KONF", antalpersoner,  ankomst,  avrese);
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
