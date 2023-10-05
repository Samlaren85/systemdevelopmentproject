using BusinessLayer;
using EntityLayer;
using SkiResortSystem.Models;
using SkiResortSystem.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;

namespace SkiResortSystem.ViewModels
{
    class BookingOverviewViewModel : ObservableObject
    {


        private Kund kund;
        public Kund Kund
        {
            get { return kund; }
            set { kund = value; OnPropertyChanged(); }
        }

        private int antalNätter;
        public int AntalNätter
        {
            get { return antalNätter; }
            set { antalNätter = Avresa.Day-Ankomst.Day; OnPropertyChanged(); }
        }

        private float utnyttjadKredit;
        public float UtnyttjadKredit
        {
            get { return utnyttjadKredit; }
            set { utnyttjadKredit = value; OnPropertyChanged(); }
        }

        private string facilitetstyp;
        public string Facilitetstyp
        {
            get { return facilitetstyp; }
            set { facilitetstyp = value; OnPropertyChanged(); }
        }

        private string bokningsnummer;
        public string Bokningsnummer
        {
            get { return bokningsnummer; }
            set { bokningsnummer = value; OnPropertyChanged(); }
        }

        private int antalPersoner;
        public int AntalPersoner
        {
            get { return antalPersoner; }
            set { antalPersoner = value; OnPropertyChanged(); }
        }

        private float prisPerNatt;
        public float PrisPerNatt
        {
            get { return prisPerNatt; }
            set { prisPerNatt = value; OnPropertyChanged(); }
        }

        private float totalpris;
        public float Totalpris
        {
            get { return totalpris; }
            set { totalpris = value; OnPropertyChanged(); }
        }

        private DateTime ankomst;
        public DateTime Ankomst
        {
            get { return ankomst; }
            set {
                ankomst = value;
                if (ankomst > avresa) Avresa = ankomst;
                OnPropertyChanged();
            }
        }

        private DateTime avresa;
        public DateTime Avresa
        {
            get { return avresa; }
            set {
                if (value < ankomst) avresa = Ankomst;
                else avresa = value;
                OnPropertyChanged();
            }
        }
        

        public Bokning SkapaBokning(DateTime ankomsttid, DateTime avresetid, Användare användareID, Kund kundID, List<Facilitet> facilitetID, List<Utrustning> utrustningID, List<Aktivitet> aktivitetID)
        {
            BookingController bc = new BookingController();
            Bokning b = bc.CreateBokning(ankomsttid, avresetid, användareID, kundID, facilitetID, null, null);
            return b;
        }


        public BookingOverviewViewModel()
        {

        }
        public BookingOverviewViewModel(Kund ValdKund, Facilitet Valdfacilitet, DateTime Valdavresetid, DateTime Valdankomsttid, int ValdaAntalPersoner)
        {
            Kund = ValdKund;
            AntalPersoner = ValdaAntalPersoner;
            Ankomst = Valdankomsttid;
            Avresa = Valdavresetid;
          

            if (Valdfacilitet.LägenhetsID != null)
            {
                Facilitetstyp = "Lägenhet, " + Valdfacilitet.LägenhetsID.LägenhetBenämning;
            }
            if (Valdfacilitet.CampingID != null)
            {
                Facilitetstyp = "Campingplats, " + Valdfacilitet.CampingID.CampingBenämning;
            }
            if (Valdfacilitet.KonferensID != null)
            {
                Facilitetstyp = "Konferenssal, " + Valdfacilitet.KonferensID.KonferensBenämning;
            }
            List<Facilitet> BokadFacilitet = new List<Facilitet>
            {
                Valdfacilitet
            };
            Bokning b = SkapaBokning(Ankomst, Avresa, SessionController.LoggedIn, ValdKund, BokadFacilitet, null, null);
            Bokningsnummer = b.BokningsID;

        }
    }
}
