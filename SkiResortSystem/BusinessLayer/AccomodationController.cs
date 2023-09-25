using DataLayer;
using EntityLayer;
using System;
using System.Collections.Generic;
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
        public int FacilitetPris { get; set; }

        /// <summary>
        /// AddKonferens´sal-metoden används för att lägga till en ny bokningsbar konferenssal i systemet. 
        /// </summary>
        public Facilitet AddKonferenssal(string konferensBenämning, double facilitetspris)
        {
            Konferenssal konferenssal = new Konferenssal() { konferensBenämning };
            konferenssal.konferensBenämning = unitOfWork.KonferensRepository.Add(konferenssal);
            Facilitet facilitet = new Facilitet() { facilitetspris, konferenssal };
            return facilitet;
        }

        /// <summary>
        /// AddCampingPlats-metoden används för att lägga till en ny bokningsbar campingplats i systemet. 
        /// </summary>
        public Facilitet AddCampingPlats(string campingBenämning, double campingStorlek, double facilitetspris)
        {
            CampingPlats campingPlats = new campingPlats() { campingBenämning, campingStorlek };
            unitOfWork.CampingRepository.Add(campingPlats);
            Facilitet facilitet = new Facilitet() { facilitetspris, campingPlats };
            unitOfWork.FacilitetRepository.Add(facilitet);
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
        public Facilitet AddLägenhet(double facilitetspris, string lägenhetsbenämning, int bäddar, double lägenhetsstorlek)
        {
            Lägenhet lägenhet = new Lägenhet() { lägenhetsbenämning, bäddar, lägenhetsstorlek };
            unitOfWork.LägenhetRepository.Add(lägenhet);
            Facilitet facilitet = new Facilitet() { facilitetspris, lägenhet };
            unitOfWork.FacilitetRepository.Add(facilitet);
            return facilitet;
        }
        
        /// <summary>
        /// Konstruktor för boendemodulen
        /// </summary>
        public AccommodationController() 
        {
            unitOfWork = new UnitOfWork();
            
        }

    }
}
