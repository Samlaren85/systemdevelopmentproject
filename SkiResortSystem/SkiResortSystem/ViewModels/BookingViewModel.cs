using BusinessLayer;
using EntityLayer;
using SkiResortSystem.Commands;
using SkiResortSystem.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
        /// <summary>
        /// NÄr man väljer en facilitet så öppnas bokningsöversikten för att skapa upp en ny bokning.
        /// </summary>
        private ICommand doubleClickBookingCommand = null!;
        public ICommand DoubleClickBookingCommand =>
            doubleClickBookingCommand ??= doubleClickBookingCommand = new RelayCommand(() =>
            {
                if (SelectedCustomer != null)
                {
                    DateTime datum = Ankomsttid.Date;
                    TimeSpan tid = SelectedTimeFrån.TimeOfDay;
                    DateTime AnkomsttidMedTid = datum + tid;
                    DateTime datum2 = Avresetid.Date;
                    TimeSpan tid2 = SelectedTimeTill.TimeOfDay;
                    DateTime AvresetidMedTid = datum2 + tid2;
                    FacilitetsSökning = new List<Facilitet>();
                    BookingOverviewViewModel bokningsöversikt = new BookingOverviewViewModel(SelectedCustomer, SelectedFacility, AvresetidMedTid, AnkomsttidMedTid, antalPersonerTillBoende);
                    windowService.ShowDialog(bokningsöversikt);
                }
            });

        /// <summary>
        /// Öppnar upp bokningsöversikten för hantering/ändring av befintlig bokning, kan tas bort, 
        /// </summary>
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

        private Visibility konferensReadOnly = Visibility.Collapsed;
        public Visibility KonferensReadOnly
        {
            get { return konferensReadOnly; }
            set
            {
                konferensReadOnly = value;
                OnPropertyChanged();
            }
        }

        private DateTime _selectedTimeFrån;

        public DateTime SelectedTimeFrån
        {
            get { return _selectedTimeFrån; }
            set
            {
                if (_selectedTimeFrån != value)
                {
                    _selectedTimeFrån = value;
                    OnPropertyChanged(nameof(SelectedTimeFrån));
                }
            }
        }

        private DateTime _selectedTimeTill;

        public DateTime SelectedTimeTill
        {
            get { return _selectedTimeTill; }
            set
            {
                if (_selectedTimeTill != value)
                {
                    _selectedTimeTill = value;
                    OnPropertyChanged(nameof(SelectedTimeTill));
                }
            }
        }

        public IList<Bokning> SearchBookings(string searchstring, DateTime? from, DateTime? to)
        {
            BookingController bc = new BookingController();
            IList<Bokning> results = new List<Bokning>();
            if ((from != null || to != null))
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
                    selectedFacility = value; Facilitetspriset = value.FacilitetsPris.Pris * ((100 - SelectedCustomer.Rabatt) / 100);
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

        private bool checkinCheckoutSelected;
        public bool CheckinCheckoutSelected
        {
            get { return  checkinCheckoutSelected; } 
            set
            {
                checkinCheckoutSelected = value;
                if (value) BookingResults = SearchBookings(searchBooking, AnkomsttidÄndra, AvresetidÄndra);
                OnPropertyChanged(nameof(BookingResults));
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
        private DateTime? ankomsttidÄndra;
        public DateTime? AnkomsttidÄndra
        {
            get { return ankomsttidÄndra; }
            set
            {
                ankomsttidÄndra = value;
                if (ankomsttidÄndra > avresetidÄndra) AvresetidÄndra = ankomsttidÄndra;
                BookingResults = SearchBookings(searchBooking, AnkomsttidÄndra, AvresetidÄndra);
                OnPropertyChanged();
            }
        }
        private DateTime? avresetidÄndra;
        public DateTime? AvresetidÄndra
        {
            get { return avresetidÄndra; }
            set
            {
                if (value < ankomsttidÄndra) avresetidÄndra = AnkomsttidÄndra;
                else avresetidÄndra = value;
                BookingResults = SearchBookings(searchBooking, AnkomsttidÄndra, AvresetidÄndra);
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
            set { konferensradiobutton = value; if (value == true) { KonferensReadOnly = Visibility.Visible; }
                if (value == false) { KonferensReadOnly = Visibility.Collapsed; }
                OnPropertyChanged(); }
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
            TimeSpan bokningsLängd = Avresetid - Ankomsttid;

            if (SelectedCustomer == null)
            {
                ErrorMessage = "Du behöver välja kund!";
            }
            if (Konferensradiobutton == true && (SelectedTimeFrån > SelectedTimeTill && Ankomsttid == Avresetid))
            {
                ErrorMessage2 = "Tiden från måste vara tidigare än tiden till!";
            }
            else if (Konferensradiobutton == false)
            {
                if (Ankomsttid < DateTime.Today || Avresetid < DateTime.Today)
                {
                    ErrorMessage2 = "Ankomst- och avresedatum måst vara senare än dagens datum";
                }
                else if (((Ankomsttid.DayOfWeek != DayOfWeek.Friday && Ankomsttid.DayOfWeek != DayOfWeek.Sunday) ||
                         (Avresetid.DayOfWeek != DayOfWeek.Friday && Avresetid.DayOfWeek != DayOfWeek.Sunday)) && Lägenhetradiobutton)
                {
                    ErrorMessage2 = "Ankomst- och avresedatum måste vara en fredag eller en söndag";
                }
                else if (Ankomsttid.DayOfWeek == DayOfWeek.Sunday &&
                         Avresetid.DayOfWeek != DayOfWeek.Sunday && Campingradiobutton)
                {
                    ErrorMessage2 = "För veckobokning måste Ankomst- och avresedatum måste vara en söndag";
                }
                else if ((bokningsLängd.Days > 1 && bokningsLängd.Days < 7) && Campingradiobutton)
                {
                    ErrorMessage2 = "Campingbokning får vara dygn eller veckobokning";
                }
                else if (Ankomsttid.DayOfWeek == DayOfWeek.Friday && Avresetid.DayOfWeek != DayOfWeek.Sunday && Lägenhetradiobutton)
                {
                    ErrorMessage2 = "För vald ankomstdatum måste avresedatum vara en söndag";
                }
                else if (Ankomsttid.DayOfWeek == DayOfWeek.Sunday && (Avresetid.DayOfWeek != DayOfWeek.Friday && Avresetid.DayOfWeek != DayOfWeek.Sunday) && Lägenhetradiobutton)
                {
                    ErrorMessage2 = "För vald ankomstdatum måste avresedatum vara en fredag eller en söndag";
                }
                else if (Ankomsttid.DayOfWeek == DayOfWeek.Sunday && Avresetid.DayOfWeek == DayOfWeek.Sunday && bokningsLängd.Days != 7 && Lägenhetradiobutton)
                {
                    ErrorMessage2 = "Bokning Vecka måste vara exakt 7 nätter (söndag till söndag)";
                }
                else if((bokningsLängd.Days > 6 && Ankomsttid.DayOfWeek == DayOfWeek.Sunday && Avresetid.DayOfWeek == DayOfWeek.Friday) && Lägenhetradiobutton)
                {
                    ErrorMessage2 = "Bokning Kortvecka får vara högst 5 nätter";
                }
                else if ((bokningsLängd.Days > 2 && Ankomsttid.DayOfWeek == DayOfWeek.Friday && Avresetid.DayOfWeek == DayOfWeek.Sunday) && Lägenhetradiobutton)
                {
                    ErrorMessage2 = "Bokning Weekend får vara högst 2 nätter";
                }
                else if (bokningsLängd.Days > 7 && Ankomsttid.DayOfWeek == DayOfWeek.Sunday && Avresetid.DayOfWeek == DayOfWeek.Sunday)
                {
                    ErrorMessage2 = "Bokning Vecka får vara högst 7 nätter";
                }
                else
                {
                    if (Lägenhetradiobutton)
                    {
                        bool success = int.TryParse(antalPersonerTillBoende, out int x);
                        if (success)
                        {
                            FacilitetsSökning = ac.FindLedigaLägenheter(x, Ankomsttid, Avresetid);
                            if (FacilitetsSökning.Count() < 1)
                            {
                                ErrorMessage2 = "Hittade inga tillgängliga faciliteter på din sökning";
                            }
                            else
                            {
                                TimeSpan t = Avresetid - Ankomsttid;
                                foreach (Facilitet f in FacilitetsSökning)
                                {
                                    f.TotalprisFörPresentationIBoendeModul = f.FacilitetsPris.Pris * t.Days;
                                }
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
                            else
                            {
                                TimeSpan t = Avresetid - Ankomsttid;
                                TimeSpan h = SelectedTimeTill - SelectedTimeFrån;
                                if (t.Days == 0 && h.Hours <= 5)
                                {
                                    foreach (Facilitet f in FacilitetsSökning)
                                    {
                                        f.TotalprisFörPresentationIBoendeModul = f.FacilitetsPris.Pris * h.Hours;
                                    }
                                }
                                else if (t.Days == 1)
                                {
                                    foreach (Facilitet f in FacilitetsSökning)
                                    {
                                        f.TotalprisFörPresentationIBoendeModul = f.FacilitetsPris.Pris;
                                    }
                                }
                                else
                                {
                                    foreach (Facilitet f in FacilitetsSökning)
                                    {
                                        f.TotalprisFörPresentationIBoendeModul = f.FacilitetsPris.Pris * t.Days;
                                    }
                                }
                            }
                        }
                        else
                        {
                            ErrorMessage2 = string.Empty;
                            ErrorMessage2 = "Du behöver lägga till antal kunder";
                        }
                    }
                }
            }
            else
            {
                //if (Lägenhetradiobutton)
                //{
                //    bool success = int.TryParse(antalPersonerTillBoende, out int x);
                //    if (success)
                //    {
                //        FacilitetsSökning = ac.FindLedigaLägenheter(x, Ankomsttid, Avresetid);
                //        if (FacilitetsSökning.Count() < 1)
                //        {
                //            ErrorMessage2 = "Hittade inga tillgängliga faciliteter på din sökning";
                //        }
                //    }
                //    else
                //    {
                //        ErrorMessage2 = string.Empty;
                //        ErrorMessage2 = "Du behöver lägga till antal kunder";
                //    }
                //}
                if (Konferensradiobutton)
                {
                    bool success = int.TryParse(antalPersonerTillBoende, out int x);
                    DateTime datum = Ankomsttid.Date;
                    TimeSpan tid = SelectedTimeFrån.TimeOfDay;
                    DateTime AnkomsttidMedTid = datum + tid;
                    DateTime datum2 = Avresetid.Date;
                    TimeSpan tid2 = SelectedTimeTill.TimeOfDay;
                    DateTime AvresetidMedTid = datum2 + tid2;
                    if (bokningsLängd.Days > 7)
                    {
                        ErrorMessage2 = "Konferensbokning får vara högst 1 vecka";
                    }
                    else{
                        if (success)
                        {
                            FacilitetsSökning = ac.FindLedigaKonferens(x, AnkomsttidMedTid, AvresetidMedTid);
                            if (FacilitetsSökning.Count() < 1)
                            {
                                ErrorMessage2 = "Hittade inga tillgängliga faciliteter på din sökning";
                            }
                            else
                            {
                                TimeSpan t = Avresetid - Ankomsttid;
                                TimeSpan h = SelectedTimeTill - SelectedTimeFrån;
                                if (t.Days == 0 && h.Hours <= 5)
                                {
                                    foreach (Facilitet f in FacilitetsSökning)
                                    {
                                        f.TotalprisFörPresentationIBoendeModul = f.FacilitetsPris.Pris * h.Hours;
                                    }
                                }
                                else if (t.Days == 0 && h.Hours > 6)
                                {
                                    foreach (Facilitet f in FacilitetsSökning)
                                    {
                                        f.TotalprisFörPresentationIBoendeModul = f.FacilitetsPris.Pris;
                                    }
                                }
                                else
                                {
                                    foreach (Facilitet f in FacilitetsSökning)
                                    {
                                        f.TotalprisFörPresentationIBoendeModul = f.FacilitetsPris.Pris * t.Days;
                                    }
                                }
                            }
                        }
                        else
                        {
                            ErrorMessage2 = string.Empty;
                            ErrorMessage2 = "Du behöver lägga till antal kunder";
                        }
                    }
                    
                }
                //if (Campingradiobutton)
                //{
                //    bool success = int.TryParse(antalPersonerTillBoende, out int x);
                //    if (success)
                //    {
                //        FacilitetsSökning = ac.FindLedigaCamping(x, Ankomsttid, Avresetid);
                //        if (FacilitetsSökning.Count() < 1)
                //        {
                //            ErrorMessage2 = "Hittade inga tillgängliga faciliteter på din sökning";
                //        }

                //    }
                //    else
                //    {
                //        ErrorMessage2 = string.Empty;
                //        ErrorMessage2 = "Du behöver lägga till antal kunder";
                //    }
                //}
            }
            if (Campingradiobutton == false && Konferensradiobutton == false && Lägenhetradiobutton == false)
            {
                ErrorMessage3 = "Du behöver välja facilitetstyp";
            }
        });

        #region Prislist tabben

        private int prislistIndex;
        public int PrislistIndex
        {
            get { return prislistIndex; }
            set
            {
                prislistIndex = value;
                switch (prislistIndex)
                {
                    case 1:
                        PriceImgSource = "/Views/Images/Prislista boende.png";
                        break;
                    case 2:
                        PriceImgSource = "/Views/Images/Prislista konferens.png";
                        break;
                    case 3:
                        PriceImgSource = "/Views/Images/Priser aktivitet.png";
                        break;
                    case 4:
                        PriceImgSource = "/Views/Images/Prislista utrustning.png";
                        break;
                     default:
                        PriceImgSource = "/Views/Images/Logo.png";
                        break;
                }
                OnPropertyChanged();
            }
        }

        private string priceImgSource;
        public string PriceImgSource
        {
            get { return priceImgSource; }
            set
            {
                priceImgSource = value;
                OnPropertyChanged();
            }
        }
        #endregion
    }
}
