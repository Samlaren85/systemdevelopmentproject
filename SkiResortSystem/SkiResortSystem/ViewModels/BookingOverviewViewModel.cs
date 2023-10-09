using BusinessLayer;
using EntityLayer;
using SkiResortSystem.Commands;
using SkiResortSystem.Models;
using SkiResortSystem.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SkiResortSystem.ViewModels
{
    class BookingOverviewViewModel : ObservableObject
    {
        private string kundPresentation;
        public string KundPresentation
        {
            get { return kundPresentation; }
            set { kundPresentation = value; OnPropertyChanged(); }
        }




        private Kund kund;
        public Kund Kund
        {
            get { return kund; }
            set { 
                kund = value;
                kundPresentation = kund.ToString().Split(" (")[0];
                OnPropertyChanged(); }
        }

       

        private int antalNätter;
        public int AntalNätter
        {
            get { return antalNätter; }
            set { antalNätter = value; OnPropertyChanged(); }
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

        private Bokning bokning;
        public Bokning Bokning
        {
            get { return bokning; }
            set { bokning = value; OnPropertyChanged(); }
        }

        private bool avbetalningsskydd;
        public bool Avbetalningsskydd
        {
            get { return avbetalningsskydd; }
            set { avbetalningsskydd = value; OnPropertyChanged(); }
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

        private ICommand saveCustomer = null!;

        public ICommand SaveCustomer => saveCustomer ??= saveCustomer = new RelayCommand<ICloseable>((view) =>
        {
            BookingController bc = new BookingController();
            Bokning.Återbetalningsskydd = Avbetalningsskydd;
            bc.SparaBokning(Bokning);
            MessageBoxResult respons = MessageBox.Show($"Bokning {Bokning.BokningsID} är nu sparad i systemet!");
            CloseCommand.Execute(view);

        });

        private ICommand closeCommand = null!;

        public ICommand CloseCommand => closeCommand ??= closeCommand = new RelayCommand<ICloseable>((view) =>
        {
            view.Close();

        });


        public BookingOverviewViewModel()
        {

        }
        public BookingOverviewViewModel(Kund ValdKund, Facilitet Valdfacilitet, DateTime Valdavresetid, DateTime Valdankomsttid, int ValdaAntalPersoner,float Facilitetspris)
        { 
            Kund = ValdKund;
            AntalPersoner = ValdaAntalPersoner;
            Ankomst = Valdankomsttid;
            Avresa = Valdavresetid;
            TimeSpan tidsspann = Avresa - Ankomst;
            AntalNätter = tidsspann.Days;
            Totalpris = Facilitetspris;
            if(AntalNätter == 0)
            {
                PrisPerNatt = 0;
            }
            else
            {
                PrisPerNatt = Totalpris / AntalNätter;
                PrisPerNatt = (float)Math.Round(PrisPerNatt, 2);
            }


            if (Valdfacilitet.LägenhetsID != null)
            {
                Facilitetstyp = "Lägenhet, " + Valdfacilitet.LägenhetsID.Lägenhetstorlek;
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
            Bokning = SkapaBokning(Ankomst, Avresa, SessionController.LoggedIn, ValdKund, BokadFacilitet, null, null);
            Bokningsnummer = Bokning.BokningsID;

        }
    }
}
