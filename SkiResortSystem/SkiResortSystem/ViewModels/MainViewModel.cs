using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using BusinessLayer;
using DataLayer;
using EntityLayer;
using Microsoft.EntityFrameworkCore.Diagnostics;
using SkiResortSystem.Commands;
using SkiResortSystem.Models;
using SkiResortSystem.Services;
using SkiResortSystem.Views;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SkiResortSystem.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        WindowService windowService = new WindowService();

        private string loggedInUser = "#NA";
        public string LoggedInUser
        {
            get { return loggedInUser; }
            set { loggedInUser = value; OnPropertyChanged(); }
        }

        //private Visibility mainVisibility = Visibility.Visible;
       /* public Visibility MainVisibility
        {
            get { return mainVisibility; }
            set { mainVisibility = value; OnPropertyChanged(); }
        } */

        /// <summary>
        /// Döljer Main View och öppnar upp löserordsrutan!
        /// </summary>
        public MainViewModel()
        {
            LogIn();
        }
       
        public void LogIn()
        {
           // MainVisibility = Visibility.Hidden;
            LoginViewModel logIn = new LoginViewModel();
            windowService.ShowDialog(logIn);
            if (SessionController.LoggedIn != null)
            {
                LoggedInUser = $"{SessionController.LoggedIn.UserID}";
              //  MainVisibility = Visibility.Visible;
            }
            else ExitCommand.Execute(null);
        }

        //Properties för att binda till mainwindow skapa privatkunder 

        private string personnummerID;
        public string PersonnummerID
        {
            get { return personnummerID; }
            set { personnummerID = value; OnPropertyChanged(); }
        }

        private string förnamn;
        public string Förnamn
        {
            get { return förnamn; }
            set { förnamn = value; OnPropertyChanged(); }
        }

        private string efternamn;
        public string Efternamn
        {
            get { return efternamn; }
            set { efternamn = value; OnPropertyChanged(); }
        }

        private string gatuadressPrivat;
        public string GatuadressPrivat
        {
            get { return gatuadressPrivat; }
            set { gatuadressPrivat = value; OnPropertyChanged(); }
        }

        private string postnummerPrivat;
        public string PostnummerPrivat
        {
            get { return postnummerPrivat; }
            set { postnummerPrivat = value; OnPropertyChanged(); }
        }

        private string ortPrivat;
        public string OrtPrivat
        {
            get { return ortPrivat; }
            set { ortPrivat = value; OnPropertyChanged(); }
        }

        private string telefonnummerPrivat;
        public string TelefonnummerPrivat
        {
            get { return telefonnummerPrivat; }
            set { telefonnummerPrivat = value; OnPropertyChanged(); }
        }
		
		private string epost;
		public string Epost
		{
			get { return epost; }
			set { epost = value; OnPropertyChanged(); }
		}

        private ICommand createPrivateCustomer = null!;

        public ICommand CreatePrivateCustomer => createPrivateCustomer ??= new RelayCommand(() =>
        {
            CustomerController cc = new CustomerController();
            cc.AddPrivateCustomer(PersonnummerID, Förnamn, Efternamn, GatuadressPrivat, PostnummerPrivat, OrtPrivat, TelefonnummerPrivat, epost);
        });

        //Properties för att binda till mainwindow skapa företagskunderkunder 

        private string organisationsnummerID;
        public string OrganisationsnummerID
        {
            get { return organisationsnummerID; }
            set { organisationsnummerID = value; OnPropertyChanged(); }
        }

        private string företagsnamn;
        public string Företagsnamn
        {
            get { return företagsnamn; }
            set { företagsnamn = value; OnPropertyChanged(); }
        }

        private string kontaktperson;
        public string Kontaktperson
        {
            get { return kontaktperson; }
            set { kontaktperson = value; OnPropertyChanged(); }
        }

        private string besöksaddress;
        public string Besöksaddress
        {
            get { return besöksaddress; }
            set { besöksaddress = value; OnPropertyChanged(); }
        }

        private string besökspostnummer;
        public string Besökspostnummer
        {
            get { return besökspostnummer; }
            set { besökspostnummer = value; OnPropertyChanged(); }
        }
        private string besöksort;
        public string Besöksort
        {
            get { return besöksort; }
            set { besöksort = value; OnPropertyChanged(); }
        }

        private string gatuadressFöretag;
        public string GatuadressFöretag
        {
            get { return gatuadressFöretag; }
            set { gatuadressFöretag = value; OnPropertyChanged(); }
        }

        private string postnummerFöretag;
        public string PostnummerFöretag
        {
            get { return postnummerFöretag; }
            set { postnummerFöretag = value; OnPropertyChanged(); }
        }

        private string ortFöretag;
        public string OrtFöretag
        {
            get { return ortFöretag; }
            set { ortFöretag = value; OnPropertyChanged(); }
        }

        private string telefonnummerFöretag;
        public string TelefonnummerFöretag
        {
            get { return telefonnummerFöretag; }
            set { telefonnummerFöretag = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Används för att markera kund i kundlistan, vid dubbelklick öppnas en kundöversikt
        /// </summary>
        private Kund selectCustomer;
        public Kund SelectCustomer
        {
            get { return selectCustomer; }
            set
            {
                selectCustomer = value;
            }
        }


        private ICommand doubleClickCustomerCommand = null!;
        public ICommand DoubleClickCustomerCommand =>
            doubleClickCustomerCommand ??= doubleClickCustomerCommand = new RelayCommand(() =>
            {
                if (selectCustomer.Privatkund != null)
                {
                    CustomerOverviewPrivateViewModel kundöversikt = new CustomerOverviewPrivateViewModel(SelectCustomer);
                    windowService.ShowDialog(kundöversikt);
                }
                else if (selectCustomer.Företagskund != null)
                {
                    CustomerOverviewCompanyViewModel kundöversikt = new CustomerOverviewCompanyViewModel(SelectCustomer);
                    windowService.ShowDialog(kundöversikt);
                }
            });

       

        private ICommand createBusinessCustomer = null!;

        public ICommand CreateBusinessCustomer => createBusinessCustomer ??= createBusinessCustomer = new RelayCommand(() =>
        {
            CustomerController cc = new CustomerController();
            cc.AddCompanyCustomer(OrganisationsnummerID, Företagsnamn, Kontaktperson, Besöksaddress, Besökspostnummer, Besöksort, GatuadressFöretag, PostnummerFöretag, OrtFöretag, TelefonnummerFöretag, epost);
        });


        /// <summary>
        /// Nedan för att söka efter kund ifrån boende 
        /// </summary>
        ///

        

        private string searchText;
        public string SearchText
        {
            get { return searchText; }
            set
            {
                searchText = value;
                SearchCustomers();
                OnPropertyChanged(); 
            }
        }

        private List<Kund> searchResults = new List<Kund>();
        public List<Kund> SearchResults
        {
            get { return searchResults; }
            set
            {
                searchResults = value;
                OnPropertyChanged();
            }
        }

        private Kund selectedCustomer;
        public Kund SelectedCustomer
        {
            get { return selectedCustomer; }
            set
            {
                if (selectedCustomer == value) return; // Lägg till den här raden för att undvika oändlig loop


                if (value != null)
                {
                    selectedCustomer = value;

                    SearchText = selectedCustomer.ToString().Split(" (")[0];
                }
                OnPropertyChanged(); 
            }
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

        private void SearchCustomers()
        {
            CustomerController cc = new CustomerController();
            try
            {
                ErrorMessage = string.Empty;
                
                SearchResults = cc.GetSearchResults(SearchText);
                if(searchResults .Count < 1)
                {
                    ErrorMessage = "Ingen kund hittades";

                }

            }
            catch (Exception ex)
            {
                ErrorMessage = "Ingen kund hittades, " + ex.Message;
                SearchResults = new List<Kund>(); 
            }
        }

        //prop som ska till bokning

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

        private DateTime ankomsttid = DateTime.Today;
        public DateTime Ankomsttid
        {
            get { return ankomsttid; }
            set { 
                ankomsttid = value;
                if (ankomsttid > avresetid) Avresetid = ankomsttid;
                OnPropertyChanged(); 
            }
        }
        private DateTime avresetid = DateTime.Today;
        public DateTime Avresetid
        {
            get { return  avresetid; }
            set {
                if (value < ankomsttid) avresetid = Ankomsttid;
                else avresetid = value;
                OnPropertyChanged(); }
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
            AccommodationController ac = new AccommodationController();
           
            if (Lägenhetradiobutton)
            {
                if (SelectedCustomer == null)
                {
                    ErrorMessage = "Du behöver välja kund!";
                   

                }
                else
                {
                    bool success = int.TryParse(antalPersonerTillBoende, out int x);
                    if (success)
                    {
                        FacilitetsSökning = ac.FindLedigaLägenheter(x, Ankomsttid, Avresetid);
                        foreach (Facilitet f in FacilitetsSökning)
                        {
                            Facilitetspris = f.Facilitetspris * SelectedCustomer.Rabatt;
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
                    foreach (Facilitet f in FacilitetsSökning)
                    {
                        Facilitetspris = f.Facilitetspris * SelectedCustomer.Rabatt;
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
                    foreach(Facilitet f in FacilitetsSökning)
                    {
                        Facilitetspris = f.Facilitetspris * SelectedCustomer.Rabatt;
                    }
                }
                else
                {
                    ErrorMessage2 = string.Empty;
                    ErrorMessage2 = "Du behöver lägga till antal kunder";
                }
            }
            if(Campingradiobutton == false && Konferensradiobutton == false && Lägenhetradiobutton == false) 
            {
                ErrorMessage3 = "Du behöver välja facilitetstyp";
            }
        });

        private IList<List<string>> visaBeläggning;
        public IList<List<string>> VisaBeläggning
        {
            get { return visaBeläggning; }
            set { visaBeläggning = value; OnPropertyChanged(); }
        }

        private DateTime beläggningankomsttid = DateTime.Today;
        public DateTime BeläggningAnkomsttid
        {
            get { return beläggningankomsttid; }
            set
            {
                beläggningankomsttid = value;
                BeläggningDatumperiod = beläggningankomsttid.AddDays(7);
                OnPropertyChanged();
            }
        }

        private DateTime beläggningdatumperiod = DateTime.Today;
        public DateTime BeläggningDatumperiod
        {
            get { return beläggningdatumperiod; }
            set
            {
                beläggningdatumperiod= value;
                OnPropertyChanged();
            }
        }

        private float facilitetspris;
        public float Facilitetspris
        {
            get { return facilitetspris; }
            set
            {
                facilitetspris = value;
                OnPropertyChanged();
            }
        }

        private bool boendekonferensbeläggningradiobutton;
        public bool BoendeKonferensbeläggningradiobutton
        {
            get { return boendekonferensbeläggningradiobutton; }
            set { boendekonferensbeläggningradiobutton = value; VisaBeläggningen.Execute(true); OnPropertyChanged(); }
        }

        private bool utrustningbeläggningradiobutton;
        public bool UtrustningBeläggningradiobutton
        {
            get { return utrustningbeläggningradiobutton; }
            set { utrustningbeläggningradiobutton = value; OnPropertyChanged(); }
        }

        private bool aktivitetbeläggningradiobutton;
        public bool Aktivitetbeläggningradiobutton
        {
            get { return aktivitetbeläggningradiobutton; }
            set { aktivitetbeläggningradiobutton = value; OnPropertyChanged(); }
        }

        private ICommand visaBeläggningen = null!;
        public ICommand VisaBeläggningen => visaBeläggningen ??= visaBeläggningen = new RelayCommand(() =>
        {
            AccommodationController ac = new AccommodationController();

            if (BoendeKonferensbeläggningradiobutton)
            {
                VisaBeläggning = ac.VisaBeläggningen(BeläggningAnkomsttid, BeläggningDatumperiod, true, false, false);
            }


            if (UtrustningBeläggningradiobutton)
            {
                {
                    VisaBeläggning = ac.VisaBeläggningen(BeläggningAnkomsttid, BeläggningDatumperiod, false, true, false);
                }
            }

            if (Aktivitetbeläggningradiobutton)
            {
                {
                    VisaBeläggning = ac.VisaBeläggningen(BeläggningAnkomsttid, BeläggningDatumperiod, false, false, true);
                }
            }
        });

        private ICommand createBooking = null!;

        public ICommand CreateBooking => createBooking ??= createBooking = new RelayCommand(() =>
        {
            BookingController bc = new BookingController();
            bc.CreateBokning(ankomsttid, avresetid, AnvändarID, KundID, FacilitetsID, UtrustningID, AktivitetID);
        });


        private ICommand customerPriveteOverview = null!;
        public ICommand CustomerPrivateOverview =>
            customerPriveteOverview ??= customerPriveteOverview = new RelayCommand(() =>
            {
                CustomerOverviewPrivateViewModel kundöversikt = new CustomerOverviewPrivateViewModel();
                windowService.ShowDialog(kundöversikt);
            }
            );

        private ICommand customerCompanyOverview = null!;
        public ICommand CustomerCompanyOverview =>
            customerCompanyOverview ??= customerCompanyOverview = new RelayCommand(() =>
            {
                CustomerOverviewCompanyViewModel kundöversikt = new CustomerOverviewCompanyViewModel();
                windowService.ShowDialog(kundöversikt);
            }
            );


        private Facilitet selectedFacility;
        public Facilitet SelectedFacility
        {
            get { return selectedFacility; }
            set
            {
                selectedFacility = value;
                OnPropertyChanged();
            }
        }

        private ICommand doubleClickBookingCommand = null!;
        public ICommand DoubleClickBookingCommand =>
            doubleClickBookingCommand ??= doubleClickBookingCommand = new RelayCommand(() =>
            {
                if (SelectedCustomer != null)
                {
                    int.TryParse(antalPersonerTillBoende, out int antalpersoner);
                    BookingOverviewViewModel bokningsöversikt = new BookingOverviewViewModel(SelectedCustomer, SelectedFacility, Avresetid, Ankomsttid, antalpersoner, Facilitetspris);
                    windowService.ShowDialog(bokningsöversikt);
                }
            });

        #region AktivitetsModulen

        private string searchBooking;
        public string SearchBooking
        {
            get { return searchBooking; }
            set
            {
                searchBooking = value;
                SearchBookings(); 
                OnPropertyChanged();
            }
        }

        private Bokning selectedBooking;
        public Bokning SelectedBooking
        {
            get { return selectedBooking; }
            set
            {
                if (value != null)
                {
                    selectedBooking = value;
                    SearchBooking = SelectedBooking.ToString().Split(" (")[0];
                    OnPropertyChanged();
                }
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

        public void SearchBookings()
        {
            BookingController bc = new BookingController();
            try
            {
                NoBookingResult = string.Empty;

                BookingResults = bc.FindMasterBooking(SearchBooking);
                if (searchResults.Count < 1)
                {
                    NoBookingResult = "Ingen bokning hittades";
                }
               
            }
            catch (Exception ex)
            {
                NoBookingResult = "Ingen bokning hittades, " + ex.Message;
                SearchResults = new List<Kund>();
            }
        }

        public void SearchActivities()
        {
            //Inte implementerat än!
        }

        private IList<Aktivitet> aktivitetsSökning;
        public IList<Aktivitet> AktivitetsSökning
        {
            get { return aktivitetsSökning; }
            set
            {
                aktivitetsSökning= value;
                OnPropertyChanged();
            }
        }

        #endregion

        /// <summary>
        /// Logga ut och stänga ner nedan 
        /// </summary>
        private ICommand logOut = null!;
        
        public ICommand LogOut =>
            logOut ??= logOut = new RelayCommand(() =>
                {
                    SessionController.Terminate();
                    LogIn();
                });   

        private ICommand exitCommand = null!;
        public ICommand ExitCommand =>
            exitCommand ??= exitCommand = new RelayCommand<ICloseable>((closeable) =>
                {
                    Application.Current.Shutdown();
                });   
    }
}
