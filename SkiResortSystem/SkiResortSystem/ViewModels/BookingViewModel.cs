﻿using BusinessLayer;
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
                    BookingOverviewViewModel bokningsöversikt = new BookingOverviewViewModel(SelectedCustomer, SelectedFacility, AvresetidMedTid, AnkomsttidMedTid, antalPersonerTillBoende, Facilitetspriset);
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
            if ((from != null && to != null) && !(searchstring != null))
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
        private DateTime ankomsttidÄndra = DateTime.Today;
        public DateTime AnkomsttidÄndra
        {
            get { return ankomsttidÄndra; }
            set
            {
                ankomsttidÄndra = value;
                if (value != null) BookingResults = SearchBookings(searchBooking, AnkomsttidÄndra, AvresetidÄndra);
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
                if (value != null) BookingResults = SearchBookings(searchBooking, AnkomsttidÄndra, AvresetidÄndra);
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
            if (Konferensradiobutton == true && SelectedTimeFrån > SelectedTimeTill)
            {
                ErrorMessage2 = "Tiden från måste varje tidigare än tiden till!";
            }
            else if (Konferensradiobutton == false)
            {
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
                else if(bokningsLängd.Days > 6 && Ankomsttid.DayOfWeek == DayOfWeek.Sunday && Avresetid.DayOfWeek == DayOfWeek.Friday)
                {
                    ErrorMessage2 = "Bokning Kortvecka får vara högst 6 dagar / 5 nätter";
                }
                else if (bokningsLängd.Days > 2 && Ankomsttid.DayOfWeek == DayOfWeek.Friday && Avresetid.DayOfWeek == DayOfWeek.Sunday)
                {
                    ErrorMessage2 = "Bokning Weekend får vara högst 3 dagar / 2 nätter";
                }
                else if (bokningsLängd.Days > 7 && Ankomsttid.DayOfWeek == DayOfWeek.Sunday && Avresetid.DayOfWeek == DayOfWeek.Sunday)
                {
                    ErrorMessage2 = "Bokning Vecka får vara högst 8 dagar / 7 nätter";
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
                        }
                        else
                        {
                            ErrorMessage2 = string.Empty;
                            ErrorMessage2 = "Du behöver lägga till antal kunder";
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
                }
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
                    }
                    else
                    {
                        ErrorMessage2 = string.Empty;
                        ErrorMessage2 = "Du behöver lägga till antal kunder";
                    }
                }
                if (Konferensradiobutton)
                {
                    bool success = int.TryParse(antalPersonerTillBoende, out int x);
                    DateTime datum = Ankomsttid.Date;
                    TimeSpan tid = SelectedTimeFrån.TimeOfDay;
                    DateTime AnkomsttidMedTid = datum + tid;
                    DateTime datum2 = Avresetid.Date;
                    TimeSpan tid2 = SelectedTimeTill.TimeOfDay;
                    DateTime AvresetidMedTid = datum2 + tid2;
                    if (success)
                    {
                        FacilitetsSökning = ac.FindLedigaKonferens(x, AnkomsttidMedTid, AvresetidMedTid);
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
            }
            if (Campingradiobutton == false && Konferensradiobutton == false && Lägenhetradiobutton == false)
            {
                ErrorMessage3 = "Du behöver välja facilitetstyp";
            }
        });
    }
}
