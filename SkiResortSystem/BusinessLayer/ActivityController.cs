﻿using DataLayer;
using EntityLayer;


namespace BusinessLayer
{
    public class ActivityController
    {
        UnitOfWork unitOfWork;
        public ActivityController() 
        { 
            unitOfWork = new UnitOfWork();
        }

        /// <summary>
        /// Söker efter Skidskolor som ligger inom ett datumspann
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public IList<Aktivitet> FindSkiSchool(DateTime? from, DateTime? to)
        {
            return unitOfWork.AktivitetRepository.Find(a => (from == null || a.Skidskola.VaraktighetFrån.Date >= from) && (to == null || a.Skidskola.VaraktighetTill.Date.AddHours(23).AddMinutes(59) <= to), x => x.Skidskola, x => x.Skidskola.Privatlektion, x => x.Skidskola.Grupplektion);
        }

        /// <summary>
        /// Sparar ändringar på existerande objekt eller lägger till saknade objekt i databasen.
        /// </summary>
        /// <param name="ab"></param>
        /// <exception cref="Exception"></exception>
        public void SaveAktivityBooking(Aktivitetsbokning ab)
        {
           
            if (unitOfWork.AktivitetsbokningsRepository.FirstOrDefault(a => a.Equals(ab)) == null)
            {
                if (ab.Bokningsref.UtnyttjadKredit + ab.TotalPris <= ab.Bokningsref.KundID.Kreditgräns)
                {
                    if (ab.Antal <= ab.Aktivitetsref.AntalPlatserKvar)
                    {
                        unitOfWork.AktivitetsbokningsRepository.Add(ab);
                        ab.Aktivitetsref.Skidskola.AntalDeltagare += ab.Antal;
                        if (ab.Bokningsref.Betalningsstatus != Status.Ofakturerad) ab.Bokningsref.UtnyttjadKredit += ab.Aktivitetsref.Skidskola.Pris;
                        unitOfWork.Save();
                    }
                    else throw new Exception("Antal personer överskrider kursens Deltagargräns");
                }
                else throw new Exception("Beloppet överskrider kundens kreditgräns!");
            }
            else
            {
                Aktivitetsbokning existerandeAktivitetsbokning = unitOfWork.AktivitetsbokningsRepository.FirstOrDefault(a => a.Equals(ab));
                if (ab.Bokningsref.UtnyttjadKredit - existerandeAktivitetsbokning.TotalPris + ab.TotalPris <= ab.Bokningsref.KundID.Kreditgräns)
                {
                    if (ab.Antal - existerandeAktivitetsbokning.Antal <= ab.Aktivitetsref.AntalPlatserKvar)
                    {
                        ab.Aktivitetsref.Skidskola.AntalDeltagare -= existerandeAktivitetsbokning.Antal;
                        ab.Aktivitetsref.Skidskola.AntalDeltagare += ab.Antal;
                        if (ab.Bokningsref.Betalningsstatus != Status.Ofakturerad)
                        {
                            ab.Bokningsref.UtnyttjadKredit -= existerandeAktivitetsbokning.TotalPris;
                            ab.Bokningsref.UtnyttjadKredit += ab.TotalPris;
                        }
                        unitOfWork.AktivitetsbokningsRepository.Update(ab);
                        unitOfWork.Save();
                    }
                    else throw new Exception("Antal personer överskrider kursens Deltagargräns");
                }
                else throw new Exception("Beloppet överskrider kundens kreditgräns!");
            }
        }

        /// <summary>
        /// Tilldelar statusen makulerad till aktivitetsbokningen.
        /// </summary>
        /// <param name="ab"></param>
        /// <returns></returns>
        public bool RemoveAktivityBooking(Aktivitetsbokning ab)
        {
            bool done;
            try
            {
                ab.Aktivitetsref.Skidskola.AntalDeltagare -= ab.Antal;
                if ((ab.Bokningsref.Betalningsstatus == Status.Obetald || ab.Bokningsref.Betalningsstatus == Status.Betald) &&
                    ab.Bokningsref.UtnyttjadKredit > ab.Aktivitetsref.Skidskola.Pris)
                    ab.Bokningsref.UtnyttjadKredit -= ab.TotalPris;
                ab.AktivitetsStatus = Status.Makulerad;
                done = true;
            } catch (Exception ex)
            {
                done = false;
            }
            if (done) unitOfWork.Save();
            return done;
        }

        /// <summary>
        /// Söker alla aktivitetsbokningar som inte är makulerade med de angivna parametrarna.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="typ"></param>
        /// <returns></returns>
        public IList<Aktivitetsbokning> FindBookedActivities (DateTime? from, DateTime? to, string typ)
        {
            return unitOfWork.AktivitetsbokningsRepository.Find(ab => ((from == null || ab.Aktivitetsref.Skidskola.VaraktighetFrån.Equals(from)) &&
                                                                      (to == null || ab.Aktivitetsref.Skidskola.VaraktighetTill.Equals(to)) &&
                                                                      (typ == null || typ == string.Empty || ab.Aktivitetsref.Typ.Equals(typ))) &&
                                                                      ab.AktivitetsStatus != Status.Makulerad, x => x.Aktivitetsref, x => x.Aktivitetsref.Skidskola);
        }

        /// <summary>
        /// Söker efter Skidskolor som ligger inom ett datumspann och av en viss typ
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="typ"></param>
        /// <returns></returns>
        public IList<Aktivitet> FindSkiSchool(DateTime from, DateTime to, string typ)
        {
            return unitOfWork.AktivitetRepository.Find(a => a.Skidskola.VaraktighetFrån >= from && a.Skidskola.VaraktighetTill <= to && a.Typ.Contains(typ), x => x.Skidskola, x => x.Skidskola.Privatlektion, x => x.Skidskola.Grupplektion);
        }

        /// <summary>
        /// Visar beläggningsstatistik för aktiviteter.
        /// </summary>
        /// <param name="franDatum"></param>
        /// <param name="tillDatum"></param>
        /// <returns></returns>
        public IList<List<string>> VisaBeläggningen(DateTime franDatum, DateTime tillDatum)
        {
            List<Aktivitet> inaktuellaFaciliteter = new List<Aktivitet>(); // Används som hjälp för filtrering
            IList<Aktivitet> dataColumn2 = new List<Aktivitet>();
            IList<Aktivitet> dataColumn3 = new List<Aktivitet>();
            IList<Aktivitet> dataColumn4 = new List<Aktivitet>();
            IList<Aktivitet> dataColumn5 = new List<Aktivitet>();
            IList<Aktivitet> dataColumn6 = new List<Aktivitet>();

            TimeSpan dateDifference = tillDatum - franDatum;
            int periodSlutdatum = (int)dateDifference.TotalDays;
            
            string aktivitetsTyp = "Privatlektion";
            dataColumn2 = FindSkiSchool(franDatum, tillDatum, aktivitetsTyp); // Data för Privatlektioner

            aktivitetsTyp = "Grupplektion";
            foreach (Aktivitet a in FindSkiSchool(franDatum, tillDatum, aktivitetsTyp))// Data för Grupplektioner
            {
                if (a.Skidskola.Grupplektion.Svårighetsgrad.Equals("Grön")) dataColumn3.Add(a);
                else if (a.Skidskola.Grupplektion.Svårighetsgrad.Equals("Blå")) dataColumn4.Add(a);
                else if (a.Skidskola.Grupplektion.Svårighetsgrad.Equals("Röd")) dataColumn5.Add(a);
                else if (a.Skidskola.Grupplektion.Svårighetsgrad.Equals("Svart")) dataColumn6.Add(a);
            }

            List<string> DatumColumnList1 = new List<string>();
            
            for (int i = 0; i < periodSlutdatum; i++)
            {
                DatumColumnList1.Add(franDatum.AddDays(i).ToShortDateString());
            }

            List<string> PRIVColumnList2 = new List<string>();
            List<string> GRUPPColumnList3 = new List<string>();
            List<string> GRUPPColumnList4 = new List<string>();
            List<string> GRUPPColumnList5 = new List<string>();
            List<string> GRUPPColumnList6 = new List<string>();


            // Denna foreach-loop används för att lägga samtliga listor som ska visas i tabellen inom boendemodulen/Visa beläggning i en gemensam lista.
            foreach (string datum in DatumColumnList1)
            {
                PRIVColumnList2.Add(Räkneverk(dataColumn2, datum).ToString());
                GRUPPColumnList3.Add(Räkneverk(dataColumn3, datum).ToString());
                GRUPPColumnList4.Add(Räkneverk(dataColumn3, datum).ToString());
                GRUPPColumnList5.Add(Räkneverk(dataColumn3, datum).ToString());
                GRUPPColumnList6.Add(Räkneverk(dataColumn3, datum).ToString());
            }

            // columnData är det gemensamma lista som används för att hämta och presentera data i visa beläggnings fliken(boendemodulen)
            IList<List<string>> columnData = new List<List<string>>
            {
                DatumColumnList1,
                PRIVColumnList2,
                GRUPPColumnList3,
                GRUPPColumnList4,
                GRUPPColumnList5,
                GRUPPColumnList6
            };


            return columnData; //Hur ska denna faktiskt se ut?

        }
        /// <summary>
        /// Om BokningsRef == null VID datum så läggs objektet till på samtliga platser(uppräkningen sker alltså) annars tas inte uppräkning med på de platser där värdet är annat än NULL
        /// </summary>
        /// <param name="Lista"></param>
        /// <param name="datum"></param>
        /// <returns></returns>
        public int Räkneverk(IList<Aktivitet> Lista, string datum)
        {
            int antal = 0;
            foreach (Aktivitet a in Lista)
            {
                if (a.Skidskola.VaraktighetFrån <= DateTime.Parse(datum) && a.Skidskola.VaraktighetTill >= DateTime.Parse(datum))
                antal += a.AntalPlatserKvar;
            }
            return antal;
        }

    }
}
