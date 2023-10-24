using DataLayer;
using EntityLayer;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Globalization;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Transactions;

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
        /// Metoden presenterar lediga faciliteter för användaren baserat på val Konferens, Lägenhet, Camping i fliken "Visa beläggning" i boendemodulen.
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
        
        /// <summary>
        /// Levererar en lista med alla faciliteter i databasen
        /// </summary>
        /// <returns></returns>
        public IList<Facilitet> FindFaciliteter(string searchString)
        {
            IList<Facilitet> list;
            if (searchString.Equals("Boende")) list = unitOfWork.FacilitetRepository.Find(f => f.KonferensID == null, l => l.LägenhetsID, c => c.CampingID);
            else list = unitOfWork.FacilitetRepository.Find(f => f.KonferensID != null, k => k.KonferensID);
            return list;
        }
        public List<Facilitet> FindLedigaFaciliteter(string sökTerm, int antalPersoner)
        {
            List<Facilitet> faciliteter = FindLedigaFaciliteter(sökTerm);
            List<Facilitet> inaktuellaFaciliteter = new List<Facilitet>();

            foreach (Facilitet facilitet in faciliteter)
            { 
                if (facilitet.LägenhetsID != null && facilitet.LägenhetsID.Bäddar < antalPersoner)
                {
                    inaktuellaFaciliteter.Add(facilitet);
                }
                
                if (facilitet.KonferensID != null && facilitet.KonferensID.AntalPersoner < antalPersoner)
                {
                    inaktuellaFaciliteter.Add(facilitet);
                    
                }
            }
            return faciliteter.Except(inaktuellaFaciliteter).ToList();
        }
 
 #endregion
        
        public IList<List<string>> VisaBeläggningen(DateTime franDatum, DateTime tillDatum)
        {
            List<Facilitet> inaktuellaFaciliteter = new List<Facilitet>(); // Används som hjälp för filtrering
            IList<Facilitet> dataColumn2 = new List<Facilitet>();
            IList<Facilitet> dataColumn3 = new List<Facilitet>();
            IList<Facilitet> dataColumn4 = new List<Facilitet>();
            IList<Facilitet> dataColumn5 = new List<Facilitet>();
            IList<Facilitet> dataColumn6 = new List<Facilitet>();

                string facilitetsTyp = "Campingplats";
                dataColumn4 = FindLedigaFaciliteter(facilitetsTyp); // Data för Camping
           
                facilitetsTyp = "Lägenhet";
                dataColumn3 = FindLedigaFaciliteter(facilitetsTyp, 5);
                foreach (Facilitet lägenhet in dataColumn3)
                {
                    if (lägenhet.LägenhetsID != null && lägenhet.LägenhetsID.Bäddar <= 4)
                    {
                        inaktuellaFaciliteter.Add(lägenhet);

                    }
                }
                dataColumn3 = dataColumn3.Except(inaktuellaFaciliteter).ToList();
                inaktuellaFaciliteter.Clear();

                dataColumn2 = FindLedigaFaciliteter(facilitetsTyp, 4);
                foreach (Facilitet lägenhet in dataColumn2)
                {
                    if (lägenhet.LägenhetsID != null && lägenhet.LägenhetsID.Bäddar >= 6)
                    {
                        inaktuellaFaciliteter.Add(lägenhet);

                    }
                }
                dataColumn2 = dataColumn2.Except(inaktuellaFaciliteter).ToList();
                inaktuellaFaciliteter.Clear();

            
                facilitetsTyp = "Konferenssal";
                dataColumn5 = FindLedigaFaciliteter(facilitetsTyp, 50);
                foreach (Facilitet konferenssal in dataColumn5)
                {
                    if (konferenssal.KonferensID != null && konferenssal.KonferensID.AntalPersoner < 50)
                    {
                        inaktuellaFaciliteter.Add(konferenssal);
                    }
                }
                dataColumn5 = dataColumn5.Except(inaktuellaFaciliteter).ToList();
                inaktuellaFaciliteter.Clear();

                dataColumn6 = FindLedigaFaciliteter(facilitetsTyp, 20);
                foreach (Facilitet konferenssal in dataColumn6)
                {
                    if (konferenssal.KonferensID != null && konferenssal.KonferensID.AntalPersoner > 20)
                    {
                        inaktuellaFaciliteter.Add(konferenssal);
                    }
                }
                dataColumn6 = dataColumn6.Except(inaktuellaFaciliteter).ToList();
                inaktuellaFaciliteter.Clear();


            

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
                
                LGH1ColumnList2.Add(Räkneverk(dataColumn2, datum).ToString());
                
                LGH2ColumnList3.Add(Räkneverk(dataColumn3, datum).ToString());
                CampingColumnList4.Add(Räkneverk(dataColumn4, datum).ToString());
                Konf1ColumnList5.Add(Räkneverk(dataColumn5, datum).ToString());
                Konf2ColumnList6.Add(Räkneverk(dataColumn6, datum).ToString());
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
        /// <summary>
        /// Om BokningsRef == null VID datum så läggs objektet till på samtliga platser(uppräkningen sker alltså) annars tas inte uppräkning med på de platser där värdet är annat än NULL
        /// 
        /// </summary>
        /// <param name="Lista"></param>
        /// <param name="datum"></param>
        /// <returns></returns>
        public int Räkneverk(IList<Facilitet>Lista, string datum)
        {
            int antal = 0;
            foreach (Facilitet f in Lista)
            {
                if (f.BokningsRef.IsNullOrEmpty())
                {
                    antal++;
                }
                else
                {
                    foreach (Bokning b in f.BokningsRef)
                    {
                        if (b.Ankomsttid <= DateTime.Parse(datum) || b.Avresetid >= DateTime.Parse(datum))
                        {
                            antal++;
                        }

                    }
                }
            }
            return antal;
        }
        #region Metoder för att söka fram lediga boenden.
        /// <summary>
        /// Hämtar faciliteter utan bokning från databasen inom ett visst tidsspann. Får också med söksträng LGH, CAMP; eller KONF för att sortera på facilitetstyp
        /// </summary>
        /// <param name="sökTerm"></param>
        /// <param name="antalPersoner"></param>
        /// <param name="ankomst"></param>
        /// <param name="avrese"></param>
        /// <returns></returns>
        public List<Facilitet> FindLedigaFaciliteterFörBokning(string sökTerm, int antalPersoner, DateTime ankomst, DateTime avrese)
        {
            IList<Bokning> bokningar = unitOfWork.BokningsRepository.Find(b => b.Ankomsttid < avrese && b.Avresetid > ankomst, X => X.FacilitetID);
            IList<Facilitet> Faciliteter = unitOfWork.FacilitetRepository.Find(b => b.FacilitetID.Contains(sökTerm, StringComparison.OrdinalIgnoreCase) && 
                                ((b.LägenhetsID != null && antalPersoner <= b.LägenhetsID.Bäddar) ||
                                (b.CampingID != null) ||
                                (b.KonferensID != null && antalPersoner <= b.KonferensID.AntalPersoner)), x => x.LägenhetsID, x => x.CampingID, x => x.KonferensID);
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


        /// <summary>
        /// beroende på vilken radiobutton iklickad i bookingviewmodel så anropas en av de 3 nedan metoder som hämtar tillgängliga faciliteter och tilldelar det
        /// korrekta priset för boende/facilitetstyp och vecka.
        /// /// </summary>
        /// <param name="antalpersoner"></param>
        /// <param name="ankomst"></param>
        /// <param name="avrese"></param>
        /// <returns></returns>
        public List<Facilitet> FindLedigaLägenheter(int antalpersoner, DateTime ankomst, DateTime avrese)
        {
            List<Facilitet> Faciliteter = FindLedigaFaciliteterFörBokning("LGH", antalpersoner, ankomst, avrese);
            DateTime date = ankomst;
            int weekNumber = GetIso8601WeekOfYear(date);
            string bokningstyp;
            TimeSpan tidsspann = avrese - ankomst;
            if (tidsspann.Days == 2)
            {
                bokningstyp = "Weekend";
            }
            else if (tidsspann.Days == 5)
            {
                bokningstyp = "Kortvecka";
            }
            else
            {
                bokningstyp = "Vecka";
            }
            if(weekNumber >= 14 && weekNumber <= 22)
            {
                weekNumber = 14;
            }
            if (weekNumber >= 23 && weekNumber <= 50)
            {
                weekNumber = 23;
            }
            foreach (Facilitet f in Faciliteter)
            {
                f.FacilitetsPris = unitOfWork.FacilitetsprisRepository.FirstOrDefault(b => b.FacilitetTyp.Contains("LGH") && b.BokningTyp == bokningstyp && weekNumber == b.Vecka); 
                unitOfWork.Save();
            }
            return Faciliteter;
        }
        /// <summary>
        /// Beskrivning i metoden ovan
        /// </summary>
        /// <param name="antalpersoner"></param>
        /// <param name="ankomst"></param>
        /// <param name="avrese"></param>
        /// <returns></returns>
        public List<Facilitet> FindLedigaCamping(int antalpersoner, DateTime ankomst, DateTime avrese)
        {
            List<Facilitet> Faciliteter = FindLedigaFaciliteterFörBokning("CAMP", antalpersoner, ankomst, avrese);
            DateTime date = ankomst;
            int weekNumber = GetIso8601WeekOfYear(date);
            string bokningstyp;
            TimeSpan tidsspann = avrese - ankomst;
            if(tidsspann.Days == 1)
            {
                bokningstyp = "Dygn";
            }
            else
            {
                bokningstyp = "Vecka";
            }
            if (weekNumber >= 14 && weekNumber <= 22)
            {
                weekNumber = 14;
            }
            if (weekNumber >= 23 && weekNumber <= 50)
            {
                weekNumber = 23;
            }
            foreach (Facilitet f in Faciliteter)
            {
                f.FacilitetsPris = unitOfWork.FacilitetsprisRepository.FirstOrDefault(b => b.FacilitetTyp.Contains("CAMP") && b.BokningTyp == bokningstyp && weekNumber == b.Vecka);
                unitOfWork.Save();
            }
            return Faciliteter;
        }
        /// <summary>
        /// Beskriving 2 metoder upp. 
        /// </summary>
        /// <param name="antalpersoner"></param>
        /// <param name="ankomst"></param>
        /// <param name="avrese"></param>
        /// <returns></returns>
        public List<Facilitet> FindLedigaKonferens(int antalpersoner, DateTime ankomst, DateTime avrese)
        {
            List<Facilitet> Faciliteter = FindLedigaFaciliteterFörBokning("KONF", antalpersoner, ankomst, avrese);
            DateTime date = ankomst;
            int weekNumber = GetIso8601WeekOfYear(date);
            string bokningstyp;
            TimeSpan tidsspann = avrese - ankomst;
            if (tidsspann.Days == 1)
            {
                bokningstyp = "Dygn";
            }
            else if(tidsspann.Hours <= 5)
            {
                bokningstyp = "Tim";
            }
            else
            {
                bokningstyp = "Vecka";
            }
            if (weekNumber >= 14 && weekNumber <= 22)
            {
                weekNumber = 14;
            }
            if (weekNumber >= 23 && weekNumber <= 50)
            {
                weekNumber = 23;
            }
            foreach (Facilitet f in Faciliteter)
            {
                f.FacilitetsPris = unitOfWork.FacilitetsprisRepository.FirstOrDefault(b => b.FacilitetTyp.Contains("pers") && b.BokningTyp == bokningstyp && weekNumber == b.Vecka);
                unitOfWork.Save();
            }
            return Faciliteter;
        }

        /// <summary>
        /// Returnerar veckonummer
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        static int GetIso8601WeekOfYear(DateTime time)
        {
            // Returnerar veckonumret enligt ISO 8601 standarden.
            Calendar calendar = CultureInfo.InvariantCulture.Calendar;
            return calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
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
