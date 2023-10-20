using EntityLayer;

namespace DataLayer
{
    public class UnitOfWork
    {
        protected AppDbContext appDbContext { get; }
        public Repository<Kund> KundRepository { get; private set; }
        public Repository<Aktivitet> AktivitetRepository { get; private set; }
        public Repository<Aktivitetsbokning> AktivitetsbokningsRepository { get; private set; }
        public Repository<Användare> AnvändarRepository { get; private set; }
        public Repository<Behörighet> BehörighetsRepository { get; private set; }
        public Repository<Bokning> BokningsRepository { get; private set; }
        public Repository<Campingplats> CampingplatsRepository { get; private set; }
        public Repository<Facilitet> FacilitetRepository { get; private set; }
        public Repository<Faktura> FakturaRepository { get; private set; }
        public Repository<Företagskund> FöretagskundRepository { get; private set; }
        public Repository<Grupplektion> GrupplektionRepository { get; private set; }
        public Repository<Konferenssal> KonferenssalRepository { get; private set; }
        public Repository<Lägenhet> LägenhetRepository { get; private set; }
        public Repository<Privatkund> PrivatkundRepository { get; private set; }
        public Repository<Privatlektion> PrivatlektionRepository { get; private set; }
        public Repository<Roll> RollRepository { get; private set; }
        public Repository<Skidskola> SkidskolaRepository { get; private set; }
        public Repository<Utrustning> UtrustningRepository { get; private set; }
        public Repository<Utrustningsbokning> UtrustningsbokningsRepository { get; private set; }
        public Repository<FacilitetsPris> FacilitetsprisRepository { get; private set; }


        public UnitOfWork()
        {
            appDbContext = AppDbContext.Instantiate();
            KundRepository = new Repository<Kund>(appDbContext);
            AktivitetRepository = new Repository<Aktivitet>(appDbContext);
            AktivitetsbokningsRepository = new Repository<Aktivitetsbokning>(appDbContext);
            AnvändarRepository = new Repository<Användare>(appDbContext);
            BehörighetsRepository = new Repository<Behörighet>(appDbContext);
            BokningsRepository = new Repository<Bokning>(appDbContext);
            CampingplatsRepository = new Repository<Campingplats>(appDbContext);
            FacilitetRepository = new Repository<Facilitet>(appDbContext);
            FakturaRepository = new Repository<Faktura>(appDbContext);
            FöretagskundRepository = new Repository<Företagskund>(appDbContext);
            GrupplektionRepository = new Repository<Grupplektion>(appDbContext);
            KonferenssalRepository = new Repository<Konferenssal>(appDbContext);
            LägenhetRepository = new Repository<Lägenhet>(appDbContext);
            PrivatkundRepository = new Repository<Privatkund>(appDbContext);
            PrivatlektionRepository = new Repository<Privatlektion>(appDbContext);
            RollRepository = new Repository<Roll>(appDbContext);
            SkidskolaRepository = new Repository<Skidskola>(appDbContext);
            UtrustningRepository = new Repository<Utrustning>(appDbContext);
            UtrustningsbokningsRepository = new Repository<Utrustningsbokning>(appDbContext);
            FacilitetsprisRepository = new Repository<FacilitetsPris> (appDbContext);
        }
        public void Save()
        {
            appDbContext.SaveChanges();
        }
    }
}
