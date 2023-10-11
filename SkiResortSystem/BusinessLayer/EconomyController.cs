using DataLayer;
using EntityLayer;
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
            return unitOfWork.FakturaRepository.FirstOrDefault(f => (f.BokningsID.Equals(kundbokning.BokningsID)));
        }
    }
}
