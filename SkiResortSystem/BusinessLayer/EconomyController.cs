using DataLayer;
using EntityLayer;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    /// <summary>
    /// EconomyController är controller-klass för de metoder som hanteras i systemets ekonomidel.
    /// </summary>
    public class EconomyController
    {
        public UnitOfWork unitOfWork;

        public EconomyController()
        {
            unitOfWork = new UnitOfWork();
        }
        /*
        public Faktura FindFaktura(Bokning kundbokning)
        {
            return unitOfWork.FakturaRepository.FirstOrDefault(f => (f.Bokningsref.Equals(kundbokning.BokningsID)));
        }*/ //Den här metoden används inte eller?! Kan i så fall ta bort det utkommenterade

        /// <summary>
        /// Metoden tar emot en fullständig lista med bokningar som sökts fram och hanterar endast de som i nuläget är ofakturerade.
        /// Resultatet kommer sedan att användas vid skapande av fakturor för ofakturerade bokningar.
        /// </summary>
        /// <param name="Lista"></param>
        /// <returns></returns>
        
        public List<Faktura> HämtaFaktureradeFakturor(IList<Bokning> Lista)
        {
            List<Faktura> faktureradeFakturor = new List<Faktura>();
            foreach (Bokning b in Lista)
            {
                if (b.Betalningsstatus == Status.Obetald || b.Betalningsstatus == Status.Betald)
                {
                    foreach (Faktura f in b.Fakturaref)
                    {
                        if (f.Fakturastatus != Status.Makulerad)
                        {
                            faktureradeFakturor.Add(f);
                        }
                    }
                }
            }
            return faktureradeFakturor;
        }

        /// <summary>
        /// CreateFaktura: skapar upp en faktura utifrån en befintlig boendebokning. Hämtar pris data beroende på kundens bokning.
        /// </summary>
        /// <param name="kundensBokning"></param>
        /// <returns></returns>
        public void CreateFaktura(Bokning kundensBokning)
        {
            DateTime fakturadatum = DateTime.Today;
            float pris = 0;

            
                
                    if (kundensBokning.FacilitetID != null)
                    {
                        foreach (Facilitet f in kundensBokning.FacilitetID)
                        {
                                pris += f.FacilitetsPris.Pris;
                        };
                    }


                    if (kundensBokning.AktivitetRef != null)
                    {
                        foreach (Aktivitetsbokning a in kundensBokning.AktivitetRef)
                        {
                                pris += a.TotalPris;
                        }
                    }

                    if (kundensBokning.UtrustningRef != null)
                    {
                        foreach (Utrustningsbokning u in kundensBokning.UtrustningRef)
                        {
                                pris += u.Utrustning.Pris;
                        }
                    }

            
            float moms = (float)(0.2 * pris);
            float avbeställningsskydd = 0;
            if (kundensBokning.Återbetalningsskydd.Equals(true))
            {
                avbeställningsskydd = 300;
            }
            float totalpris = (pris+moms);
            float prisFaktura1 = (float)(totalpris*0.2) + avbeställningsskydd;
            float momsFaktura1 = (float)(prisFaktura1 * 0.2);

            float prisFaktura2 = (float)(totalpris * 0.8);
            float momsFaktura2 = (float)(prisFaktura2 * 0.2);

            ///
            /// Faktura #1 som avser 20% av totalbeloppet och ska betalas senast 30dagar efter bokningsdatum.
            /// 
            Faktura faktura1 = new Faktura(fakturadatum, prisFaktura1, momsFaktura1, kundensBokning);

            ///
            /// Faktura #2 som avser 80% av totalbeloppet och ska vara betalt senast 30dagar före ankomst.
            /// 
            Faktura faktura2 = new Faktura(fakturadatum, prisFaktura2, momsFaktura2, kundensBokning);

            /// 
            /// If-satsen hanterar fakturornas förfallodatum.
            /// 
            if (kundensBokning.Avresetid >= DateTime.Today.AddDays(-30) && kundensBokning.Avresetid <= DateTime.Today)
            {
                // Bokningstillfället inträffar inom 30dagar och därför sätts förfallodatum till ankomstdagen.
                faktura1.Förfallodatum = kundensBokning.Ankomsttid;
                faktura2.Förfallodatum = kundensBokning.Ankomsttid;
            }
            else
            {
                // Fakturan avser en bokning längre fram i tiden än 30dagar och förfallodatum sätts till 30dagar innan bokningen äger rum.
                faktura1.Förfallodatum = kundensBokning.Avresetid.AddDays(-30);
                faktura2.Förfallodatum = kundensBokning.Avresetid.AddDays(-30);
            }
            

            unitOfWork.FakturaRepository.Add(faktura1);
            unitOfWork.FakturaRepository.Add(faktura2);
            unitOfWork.Save();
            kundensBokning.Betalningsstatus = Status.Obetald;

            /// 
            /// Fakturorna skrivs ut samt läggs i en mapp på datorns hårddisk för evt. senare utskriftsmöjlighet.
            /// 
            PrintController.PrintController.Run(faktura1);
            PrintController.PrintController.Run(faktura2);
        }

        /// <summary>
        /// Metoden uppdaterar databasen med nytt värde.
        /// </summary>
        /// <param name="faktura"></param>
        public void UpdateFaktura(Faktura faktura)
        {
            unitOfWork.FakturaRepository.Update(faktura);
            unitOfWork.Save();

        }
    }
}
