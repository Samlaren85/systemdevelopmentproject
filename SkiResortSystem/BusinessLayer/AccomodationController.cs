using DataLayer;
using EntityLayer;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design;
using System.Data;
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
        public string column1 = null;
        public string column2 = null;
        public string column3 = null;
        public string column4 = null;
        public string column5 = null;
        public string column6 = null;
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
        /// <summary>
        /// Metoden FindBokadeFaciliteter används i syfte att finna faciliteter av en vald typ, oavsett bokningsstatus.
        /// </summary>
        /// <param name="sökTerm"></param>
        /// <returns></returns>
        public List<Facilitet> FindBokadeFaciliteter(string sökTerm, DateTime franDatum, DateTime tillDatum)
        {
            IList<Facilitet> allaFaciliteter = unitOfWork.FacilitetRepository.Find(f => true);
            IList<Bokning> allaBokningar = unitOfWork.BokningsRepository.Find(b => b.Ankomsttid >= franDatum);

            IList<Facilitet> inaktuellaFaciliteter = new List<Facilitet>();
            foreach (Facilitet f in allaFaciliteter)
            {
                if (f.KonferensID == null && sökTerm.Equals("Konferenssal")) inaktuellaFaciliteter.Add(f);
                if (f.CampingID == null && sökTerm.Equals("Campingplats")) inaktuellaFaciliteter.Add(f);
                if (f.LägenhetsID == null && sökTerm.Equals("Lägenhet")) inaktuellaFaciliteter.Add(f);
            }
            List<Facilitet> söktaFaciliteter = allaFaciliteter.Except(inaktuellaFaciliteter).ToList();

            IList<Facilitet> inaktuellaBokningar = new List<Facilitet>();
            foreach (Bokning b in allaBokningar)
            {
                if (b.Avresetid > tillDatum)
                {
                    foreach(Facilitet f in b.FacilitetID)
                    {
                        inaktuellaBokningar.Add(f);
                    }
                }
                
            }
            return söktaFaciliteter.Except(inaktuellaBokningar).ToList(); //Vad händer om första sökkriteriet blir null?!? ta bort efter testkörning
        }
        public List<T> KontrolleraBeläggning<T>(string sökTerm) //Starta här nästa stittning!
        {
            List<T>myList = new List<T>();
            FindBoende(sökTerm); // Här ska vi få till i vyn 6-Beläggning så att Lägenheter samt konferenssalar splittas till 4 olika kolummner (2st/typ)
            /*FindUtrustning(sökTerm);*/ //Här ska varje enskild typ visas på separat rad (vertikalt) KOMMER ATT SKAPAS I Utrustnings BL09/BL20 sprint2-ish
            /*FindAktiviteter(sökTerm);*/ //Här ska varje aktivitet splittas på enskild rad. Likt Utrustning KOMMER ATT SKAPAS I BL07/BL08 sprint2-ish
            return myList;
        }
 #endregion
        
        public List<string> FetchColumnDataVisaBeläggning<T>(string sökTerm, DateTime franDatum, DateTime tillDatum, string facilitetsTyp, bool lägenhet, bool konferens, bool camping)
        {
            List<Facilitet> inaktuellaFaciliteter = new List<Facilitet>(); // Används som hjälp för filtrering
            IList<Bokning> aktuellaBokningar = FindBoenden(sökTerm, franDatum, tillDatum, facilitetsTyp);
            IList<Facilitet> dataColumn2 = FindLedigaFaciliteter(facilitetsTyp, 4); // Data för LGH1
            IList<Facilitet> dataColumn3 = FindLedigaFaciliteter(facilitetsTyp, 5); // Data för LGH2
            IList<Facilitet> dataColumn4 = new List<Facilitet>();
            IList<Facilitet> dataColumn5 = FindLedigaFaciliteter(facilitetsTyp); // Data för konf1
            IList<Facilitet> dataColumn6 = FindLedigaFaciliteter(facilitetsTyp); // Data för konf2

            if (camping == true)
            {
                dataColumn4 = FindLedigaFaciliteter(sökTerm); // Data för Camping
            }

            if (konferens == true)
            {
                foreach (Facilitet facilitet in dataColumn5)
                {
                    if (facilitet.KonferensID.KonferensBenämning.Equals("Liten"))
                    {
                        inaktuellaFaciliteter.Add(facilitet);
                    }
                    dataColumn5.Except(inaktuellaFaciliteter).ToList();
                    inaktuellaFaciliteter.Clear();
                }

                foreach (Facilitet facilitet in dataColumn6)
                {
                    if (facilitet.KonferensID.KonferensBenämning == "Stor")
                    {
                        inaktuellaFaciliteter.Add(facilitet);
                    }
                    dataColumn6.Except(inaktuellaFaciliteter).ToList();
                    inaktuellaFaciliteter.Clear();
                }
            }
            // Koden nedan hämtar datumen och parsar dessa till en gemensam variabel
            string textFranDatum = null;
           
            franDatum = DateTime.Parse(textFranDatum);

            string dataColumn1 = $"{textFranDatum}";

            List<string> column1List = new List<string> {dataColumn1}; //Datum

            TimeSpan dateDifference = tillDatum - franDatum;
            int periodSlutdatum = (int)dateDifference.TotalDays;
          
            for (int i = 0; i < periodSlutdatum; i++)
            {
                column1List.Add(franDatum.AddDays(i).ToShortDateString());
            }

            IList<string> column2List = new List<string>(); //LGH1
            IList<string> column3List = new List<string>(); // LGH2 
            IList<string> column4List = new List<string>(); //Camping
            IList<string> column5List = new List<string>(); //Konf1
            IList<string> column6List = new List<string>(); //Konf2

            // Denna foreach-loop används för att lägga samtliga listor som ska visas i tabellen inom boendemodulen/Visa beläggning i en gemensam lista.
            foreach (string datum in column1List)
            {

                column2List.Add(dataColumn2.Count(f => (f.BokningsRef.Ankomsttid <= DateTime.Parse(datum) || f.BokningsRef.Avresetid >= DateTime.Parse(datum))).ToString());
                column3List.Add(dataColumn3.Count(f => (f.BokningsRef.Ankomsttid <= DateTime.Parse(datum) || f.BokningsRef.Avresetid >= DateTime.Parse(datum))).ToString());
                column4List.Add(dataColumn4.Count(f => (f.BokningsRef.Ankomsttid <= DateTime.Parse(datum) || f.BokningsRef.Avresetid >= DateTime.Parse(datum))).ToString());
                column5List.Add(dataColumn5.Count(f => (f.BokningsRef.Ankomsttid <= DateTime.Parse(datum) || f.BokningsRef.Avresetid >= DateTime.Parse(datum))).ToString());
                column6List.Add(dataColumn6.Count(f => (f.BokningsRef.Ankomsttid <= DateTime.Parse(datum) || f.BokningsRef.Avresetid >= DateTime.Parse(datum))).ToString());
            }

            // columnData är det gemensamma lista som används för att hämta och presentera data i visa beläggnings fliken(boendemodulen)
            List<string> columnData = new List<string>
            {
                column1List.Count().ToString(),
                column2List.Count().ToString(),
                column3List.Count().ToString(),
                column4List.Count().ToString(),
                column5List.Count().ToString(),
                column6List.Count().ToString()
            };

            return columnData; //Hur ska denna faktiskt se ut?
            
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
