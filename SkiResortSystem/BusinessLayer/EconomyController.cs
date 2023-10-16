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
    public class EconomyController
    {
        public UnitOfWork unitOfWork;

        public EconomyController()
        {
            unitOfWork = new UnitOfWork();
        }

        public Faktura FindFaktura(Bokning kundbokning)
        {
            return unitOfWork.FakturaRepository.FirstOrDefault(f => (f.Bokningsref.Equals(kundbokning.BokningsID)));
        }

        public List<Faktura> FetchBillableBills(IList<Bokning> Lista)
        {
            List<Faktura> billableBills = new List<Faktura>();
            foreach (Bokning b in Lista)
            {
              foreach (Faktura f in b.Fakturaref)
              {
                    if (f.Förfallodatum == null && b.Ankomsttid >= DateTime.Today)
                    {
                        billableBills.Add(f);
                    }
              }
                
            }
            return billableBills;
        }
        public Faktura CreateFaktura(Bokning kundensBokning)
        {
            DateTime fakturadatum = DateTime.Today;
            float pris = 0;

            if (pris == 0)
            {
                foreach (Facilitet f in kundensBokning.FacilitetID)
                {
                    if (kundensBokning.FacilitetID != null)
                    {
                        pris += f.Facilitetspris;
                    }
                };
                foreach (Aktivitetsbokning a in kundensBokning.AktivitetRef)
                {
                    if (a.AktivitetsbokningsID.Equals(kundensBokning.AktivitetRef.FirstOrDefault(a)))
                    {
                        pris += a.TotalPris;
                    }
                }
                foreach (Utrustningsbokning u in kundensBokning.UtrustningRef)
                {
                    if (u.UtrustningsbokningsID.Equals(kundensBokning.UtrustningRef.FirstOrDefault(u)))
                    {
                        pris += u.Utrustning.Pris;
                    }
                    
                }
            }
            
            float totalpris = 0 ;//ska hämta priset för allt som tillhör fakturan
            float moms = (float)(0.2 * pris);
            Faktura faktura= new Faktura(fakturadatum, totalpris, moms);
            unitOfWork.FakturaRepository.Add(faktura);
            kundensBokning.Fakturaref.Add(faktura);
            unitOfWork.Save();
            return faktura;
        }
    }
}
