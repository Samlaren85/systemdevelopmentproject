using BusinessLayer;
using EntityLayer;
using SkiResortSystem.Commands;
using SkiResortSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SkiResortSystem.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private Användare användarID;
        public Användare AnvändarID
        {
            get { return användarID; }
            set { användarID = value; OnPropertyChanged(); }
        }

        private Kund kundID;
        public Kund KundID
        {
            get { return kundID; }
            set { kundID = value; OnPropertyChanged(); }
        }

        private List<Facilitet> facilitetsID;
        public List<Facilitet> FacilitetsID
        {
            get { return facilitetsID; }
            set { facilitetsID = value; OnPropertyChanged(); }
        }

        private List<Utrustning> utrustningID;
        public List<Utrustning> UtrustningID
        {
            get { return utrustningID; }
            set { utrustningID = value; OnPropertyChanged(); }
        }

        private List<Aktivitet> aktivitetID;
        public List<Aktivitet> AktivitetID
        {
            get { return aktivitetID; }
            set { aktivitetID = value; OnPropertyChanged(); }
        }

        private string errorMessage;
        public string ErrorMessage
        {
            get { return errorMessage; }
            set
            {
                errorMessage = value;
                OnPropertyChanged();
            }
        }
        private string errorMessage2;
        public string ErrorMessage2
        {
            get { return errorMessage2; }
            set
            {
                errorMessage2 = value;
                OnPropertyChanged();
            }
        }
        private string errorMessage3;
        public string ErrorMessage3
        {
            get { return errorMessage3; }
            set
            {
                errorMessage3 = value;
                OnPropertyChanged();
            }
        }

        private ICommand doubleClickBookingCommand = null!;
        public ICommand DoubleClickBookingCommand =>
            doubleClickBookingCommand ??= doubleClickBookingCommand = new RelayCommand(() =>
            {
                if (SelectedCustomer != null)
                {
                    FacilitetsSökning = new List<Facilitet>();
                    BookingOverviewViewModel bokningsöversikt = new BookingOverviewViewModel(SelectedCustomer, SelectedFacility, Avresetid, Ankomsttid, antalPersonerTillBoende, Facilitetspriset);
                    windowService.ShowDialog(bokningsöversikt);
                }
            });
        private ICommand doubleClickBookingCommandÄndra = null!;
        public ICommand DoubleClickBookingCommandÄndra =>
            doubleClickBookingCommandÄndra ??= doubleClickBookingCommandÄndra = new RelayCommand(() =>
            {
                if (ÄndraBokning != null)
                {
                    BookingOverviewViewModel bokningsöversikt = new BookingOverviewViewModel(ÄndraBokning);
                    BookingResults = new List<Bokning>();
                    SearchBooking = string.Empty;
                    windowService.ShowDialog(bokningsöversikt);
                }
            });

        private Bokning ändraBokning;
        public Bokning ÄndraBokning
        {
            get { return ändraBokning; }
            set { ändraBokning = value; OnPropertyChanged(); }
        }


        private string searchBooking;
        public string SearchBooking
        {
            get { return searchBooking; }
            set
            {
                if (value != null)
                {
                    searchBooking = value;
                    BookingResults = SearchBookings(searchBooking, AnkomsttidÄndra, AvresetidÄndra); if (searchBooking == string.Empty) { BookingResults = new List<Bokning>(); }
                    OnPropertyChanged(SearchBooking);
                }
            }
        }

        private Bokning selectedBooking;
        public Bokning SelectedBooking
        {
            get { return selectedBooking; }
            set
            {
                if (value == selectedBooking) return;
                if (value != null)
                {
                    selectedBooking = value;
                    SearchBooking = SelectedBooking.ToString().Split(" (")[0];
                }
                OnPropertyChanged();
            }
        }

        private IList<Bokning> bookingResults;
        public IList<Bokning> BookingResults
        {
            get { return bookingResults; }
            set
            {
                bookingResults = value;
                OnPropertyChanged();
            }
        }
        private string noBookingResult;
        public string NoBookingResult
        {
            get { return noBookingResult; }
            set
            {
                noBookingResult = value;
                OnPropertyChanged();
            }
        }

        public IList<Bokning> SearchBookings(string searchstring, DateTime? from, DateTime? to)
        {
            BookingController bc = new BookingController();
            IList<Bokning> results = new List<Bokning>();
            if ((from != null && to != null) && !(from == DateTime.Today && to == DateTime.Today && searchstring != null))
            {
                NoBookingResult = string.Empty;

                try
                {
                    results = bc.FindMasterBooking(searchstring, from, to);
                }
                catch (Exception ex)
                {
                    results = new List<Bokning>();
                    NoBookingResult = "Ingen bokning hittades";

                }
            }
            else if (searchstring != null)
            {
                NoBookingResult = string.Empty;

                try
                {
                    results = bc.FindMasterBooking(searchstring);
                    if(results.Count() == 0)
                    {
                        NoBookingResult = "Ingen bokning hittades";

                    }
                }
                catch (Exception ex)
                {
                    results = new List<Bokning>();
                    NoBookingResult = "Ingen bokning hittades";

                }
            }
            return results;
        }

        private ICommand createBooking = null!;

        public ICommand CreateBooking => createBooking ??= createBooking = new RelayCommand(() =>
        {
            BookingController bc = new BookingController();
            bc.CreateBokning(ankomsttid, avresetid, AnvändarID, KundID, FacilitetsID, AntalPersonerTillBoende);
        });

        private Facilitet selectedFacility;
        public Facilitet SelectedFacility
        {
            get { return selectedFacility; }
            set
            {
                if (value != null)
                {
                    selectedFacility = value; Facilitetspriset = value.Facilitetspris * ((100 - SelectedCustomer.Rabatt) / 100);
                    OnPropertyChanged();
                }
            }
        }

        private float facilitetspriset;
        public float Facilitetspriset
        {
            get { return facilitetspriset; }
            set
            {
                facilitetspriset = value;
                OnPropertyChanged();
            }
        }
        private DateTime ankomsttid = DateTime.Today;
        public DateTime Ankomsttid
        {
            get { return ankomsttid; }
            set
            {
                ankomsttid = value;
                if (ankomsttid > avresetid) Avresetid = ankomsttid;
                OnPropertyChanged();
            }
        }
        private DateTime avresetid = DateTime.Today;
        public DateTime Avresetid
        {
            get { return avresetid; }
            set
            {
                if (value < ankomsttid) avresetid = Ankomsttid;
                else avresetid = value;
                OnPropertyChanged();
            }
        }
        private DateTime ankomsttidÄndra = DateTime.Today;
        public DateTime AnkomsttidÄndra
        {
            get { return ankomsttidÄndra; }
            set
            {
                ankomsttidÄndra = value;
                if (ankomsttidÄndra > avresetidÄndra) AvresetidÄndra = ankomsttidÄndra;
                OnPropertyChanged();
            }
        }
        private DateTime avresetidÄndra = DateTime.Today;
        public DateTime AvresetidÄndra
        {
            get { return avresetidÄndra; }
            set
            {
                if (value < ankomsttidÄndra) avresetidÄndra = AnkomsttidÄndra;
                else avresetidÄndra = value;
                OnPropertyChanged();
            }
        }

        private bool lägenhetradiobutton;
        public bool Lägenhetradiobutton
        {
            get { return lägenhetradiobutton; }
            set { lägenhetradiobutton = value; OnPropertyChanged(); }
        }

        private bool campingradiobutton;
        public bool Campingradiobutton
        {
            get { return campingradiobutton; }
            set { campingradiobutton = value; OnPropertyChanged(); }
        }
        private bool konferensradiobutton;
        public bool Konferensradiobutton
        {
            get { return konferensradiobutton; }
            set { konferensradiobutton = value; OnPropertyChanged(); }
        }

        private string antalPersonerTillBoende;
        public string AntalPersonerTillBoende
        {
            get { return antalPersonerTillBoende; }
            set { antalPersonerTillBoende = value; ErrorMessage2 = string.Empty; OnPropertyChanged(); }
        }

        private List<Facilitet> facilitetsSökning;
        public List<Facilitet> FacilitetsSökning
        {
            get { return facilitetsSökning; }
            set { facilitetsSökning = value; OnPropertyChanged(); }
        }

        private ICommand sökLedigaFaciliteter = null!;

        public ICommand SökLedigaFaciliteter => sökLedigaFaciliteter ??= sökLedigaFaciliteter = new RelayCommand(() =>
        {
            
            FacilitetsSökning = new List<Facilitet>(); 
            AccommodationController ac = new AccommodationController();
            ErrorMessage2 = string.Empty;
            ErrorMessage3 = string.Empty;
            if (Lägenhetradiobutton)
            {
                if (SelectedCustomer == null)
                {
                    ErrorMessage = "Du behöver välja kund!";


                }
                if ((Ankomsttid.DayOfWeek != DayOfWeek.Friday && Ankomsttid.DayOfWeek != DayOfWeek.Sunday) &&
                    (Avresetid.DayOfWeek != DayOfWeek.Friday && Avresetid.DayOfWeek != DayOfWeek.Sunday))
                {
                    ErrorMessage2 = "Ankomst- och avresetid måste vara en fredag eller en söndag";
                }
                else if (Ankomsttid.DayOfWeek == DayOfWeek.Friday && Avresetid.DayOfWeek != DayOfWeek.Sunday)
                {
                    ErrorMessage2 = "För vald ankomsttid måste avresetid vara en söndag";
                }
                else if (Ankomsttid.DayOfWeek == DayOfWeek.Sunday && Avresetid.DayOfWeek != DayOfWeek.Friday && Avresetid.DayOfWeek != DayOfWeek.Sunday)
                {
                    ErrorMessage2 = "För vald ankomsttid måste avresetid vara en fredag eller en söndag";
                }
                else if (Avresetid.DayOfWeek == DayOfWeek.Friday && Ankomsttid.DayOfWeek != DayOfWeek.Sunday)
                {
                    ErrorMessage2 = "För vald avresetid måste ankomsttid vara en söndag";
                }
                else if (Avresetid.DayOfWeek == DayOfWeek.Sunday && Ankomsttid.DayOfWeek != DayOfWeek.Friday && Ankomsttid.DayOfWeek != DayOfWeek.Sunday)
                {
                    ErrorMessage2 = "För vald avresetid måste ankomst vara en fredag eller en söndag";
                }
                else
                {
                    bool success = int.TryParse(antalPersonerTillBoende, out int x);
                    if (success)
                    {
                        FacilitetsSökning = ac.FindLedigaLägenheter(x, Ankomsttid, Avresetid);
                        if (FacilitetsSökning.Count() < 1)
                        {
                            ErrorMessage2 = "Hittade inga tillgängliga faciliteter på din sökning";
                        }
                    }
                    else
                    {
                        ErrorMessage2 = string.Empty;
                        ErrorMessage2 = "Du behöver lägga till antal kunder";
                    }
                }

            }

            if (Konferensradiobutton)
            {
                bool success = int.TryParse(antalPersonerTillBoende, out int x);
                if (success)
                {
                    FacilitetsSökning = ac.FindLedigaKonferens(x, Ankomsttid, Avresetid);
                    if (FacilitetsSökning.Count() < 1)
                    {
                        ErrorMessage2 = "Hittade inga tillgängliga faciliteter på din sökning";
                    }
                }
                else
                {
                    ErrorMessage2 = string.Empty;
                    ErrorMessage2 = "Du behöver lägga till antal kunder";
                }
            }

            if (Campingradiobutton)
            {
                bool success = int.TryParse(antalPersonerTillBoende, out int x);
                if (success)
                {
                    FacilitetsSökning = ac.FindLedigaCamping(x, Ankomsttid, Avresetid);
                    if (FacilitetsSökning.Count() < 1)
                    {
                        ErrorMessage2 = "Hittade inga tillgängliga faciliteter på din sökning";
                    }

                }
                else
                {
                    ErrorMessage2 = string.Empty;
                    ErrorMessage2 = "Du behöver lägga till antal kunder";
                }
            }
            if (Campingradiobutton == false && Konferensradiobutton == false && Lägenhetradiobutton == false)
            {
                ErrorMessage3 = "Du behöver välja facilitetstyp";
            }
        });
    }
}
