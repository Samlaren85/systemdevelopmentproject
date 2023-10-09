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
        public Facilitet AddKonferenssal(string konferensBenämning, float facilitetspris, int antalPersoner)
        {
            Konferenssal konferenssal = new Konferenssal(konferensBenämning, antalPersoner);
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
                if (facilitet.LägenhetsID != null &&facilitet.LägenhetsID.Bäddar < antalPersoner)
                {
                    inaktuellaFaciliteter.Add(facilitet);
                }
                if (facilitet.KonferensID != null &&facilitet.KonferensID.AntalPersoner < antalPersoner)
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
        
        public IList<List<string>> VisaBeläggningen(DateTime franDatum, DateTime tillDatum, bool boende , bool utrustning, bool aktivitet)
        {
            List<Facilitet> inaktuellaFaciliteter = new List<Facilitet>(); // Används som hjälp för filtrering
            IList<Facilitet> dataColumn2 = new List<Facilitet>();
            IList<Facilitet> dataColumn3 = new List<Facilitet>();
            IList<Facilitet> dataColumn4 = new List<Facilitet>();
            IList<Facilitet> dataColumn5 = new List<Facilitet>();
            IList<Facilitet> dataColumn6 = new List<Facilitet>();

            if (boende == true)
            {
                string facilitetsTyp = "Campingplats";
                dataColumn4 = FindLedigaFaciliteter(facilitetsTyp); // Data för Camping
            }

            if (boende == true)
            {
                string facilitetsTyp = "Lägenhet";
                dataColumn3 = FindLedigaFaciliteter(facilitetsTyp, 6);
                dataColumn2 = FindLedigaFaciliteter(facilitetsTyp, 4);
            }

            if (boende == true)
            {
                string facilitetsTyp = "Konferenssal";
                dataColumn5 = FindLedigaFaciliteter(facilitetsTyp, 50);
                dataColumn6 = FindLedigaFaciliteter(facilitetsTyp, 20);

            }
            
            List<string> DatumColumnList1 = new List<string>();
            
            TimeSpan dateDifference = tillDatum - franDatum;
            int periodSlutdatum = (int)dateDifference.TotalDays;
          
            for (int i = 0; i < periodSlutdatum; i++)
            {
                DatumColumnList1.Add(franDatum.AddDays(i).ToShortDateString());
            }

            List<string> LGH1ColumnList2 = new List<string>(); //LGH1 - liten
            List<string> LGH2ColumnList3 = new List<string>(); // LGH2 - stor
            List<string> CampingColumnList4 = new List<string>(); //Camping
            List<string> Konf1ColumnList5 = new List<string>(); //Konf1 -stor
            List<string> Konf2ColumnList6 = new List<string>(); //Konf2 -liten

            // Denna foreach-loop används för att lägga samtliga listor som ska visas i tabellen inom boendemodulen/Visa beläggning i en gemensam lista.
            foreach (string datum in DatumColumnList1)
            {
                // Om BokningsRef == null VID datum så läggs objektet till på samtliga platser(uppräkningen sker alltså) annars tas inte uppräkning med på de platser där värdet är annat än NULL
                LGH1ColumnList2.Add(dataColumn2.Count(f => (f.BokningsRef == null || (f.BokningsRef.Ankomsttid <= DateTime.Parse(datum) || f.BokningsRef.Avresetid >= DateTime.Parse(datum)))).ToString());
                LGH2ColumnList3.Add(dataColumn3.Count(f => (f.BokningsRef == null || (f.BokningsRef.Ankomsttid <= DateTime.Parse(datum) || f.BokningsRef.Avresetid >= DateTime.Parse(datum)))).ToString());
                CampingColumnList4.Add(dataColumn4.Count(f => (f.BokningsRef == null || (f.BokningsRef.Ankomsttid <= DateTime.Parse(datum) || f.BokningsRef.Avresetid >= DateTime.Parse(datum)))).ToString());
                Konf1ColumnList5.Add(dataColumn5.Count(f => (f.BokningsRef == null || (f.BokningsRef.Ankomsttid <= DateTime.Parse(datum) || f.BokningsRef.Avresetid >= DateTime.Parse(datum)))).ToString());
                Konf2ColumnList6.Add(dataColumn6.Count(f => (f.BokningsRef == null || (f.BokningsRef.Ankomsttid <= DateTime.Parse(datum) || f.BokningsRef.Avresetid >= DateTime.Parse(datum)))).ToString());
            }

            // columnData är det gemensamma lista som används för att hämta och presentera data i visa beläggnings fliken(boendemodulen)
            IList<List<string>> columnData = new List<List<string>>
            {
                DatumColumnList1,
                LGH1ColumnList2,
                LGH2ColumnList3,
                CampingColumnList4,
                Konf1ColumnList5,
                Konf2ColumnList6
            };
            

            return columnData; //Hur ska denna faktiskt se ut?
            
        }
        #region Metoder för att söka fram lediga boenden.
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
