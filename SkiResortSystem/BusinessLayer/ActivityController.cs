using DataLayer;
using EntityLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class ActivityController
    {
        UnitOfWork unitOfWork;
        public ActivityController() 
        { 
            unitOfWork = new UnitOfWork();
        }

        public IList<Aktivitet> FindSkiSchool(DateTime from, DateTime to)
        {
            return unitOfWork.AktivitetRepository.Find(a => a.Skidskola.VaraktighetFrån >= from && a.Skidskola.VaraktighetTill <= to, x => x.Skidskola, x => x.Skidskola.Privatlektion, x => x.Skidskola.Grupplektion);
        }
    }
}
