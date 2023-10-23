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
        /// CreateFaktura: skapar upp en faktura utifrån en befintlig boendebokning.
        /// </summary>
        /// <param name="kundensBokning"></param>
        /// <returns></returns>
        public Faktura CreateFaktura(Bokning kundensBokning)
        {
            DateTime fakturadatum = DateTime.Today;
            float pris = 0;

            
                
                    if (kundensBokning.FacilitetID != null)
                    {
                        foreach (Facilitet f in kundensBokning.FacilitetID)
                        {
                                pris += f.Facilitetspris;
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
            int avbeställningsskydd = 0;
            if (kundensBokning.Återbetalningsskydd.Equals(true))
            {
                avbeställningsskydd = 300;
            }
            float totalpris = (pris+moms+avbeställningsskydd);//ska hämta priset för allt som tillhör fakturan
            Faktura faktura= new Faktura(fakturadatum, totalpris, moms, kundensBokning);
            
            unitOfWork.FakturaRepository.Add(faktura);
            unitOfWork.Save();
            kundensBokning.Betalningsstatus = Status.Obetald;
            return faktura;
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
