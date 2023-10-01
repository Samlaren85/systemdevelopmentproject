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
using SkiResortSystem.Commands;
using SkiResortSystem.Models;
using SkiResortSystem.Services;
using SkiResortSystem.Views;

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

        private ICommand createBusinessCustomer = null!;

        public ICommand CreateBusinessCustomer => createBusinessCustomer ??= createBusinessCustomer = new RelayCommand(() =>
        {
            CustomerController cc = new CustomerController();
            cc.AddCompanyCustomer(OrganisationsnummerID, Företagsnamn, Kontaktperson, Besöksaddress, GatuadressFöretag, PostnummerFöretag, OrtFöretag, TelefonnummerFöretag, epost);
        });


        /// <summary>
        /// Nedan för att söka efter kund ifrån boende 
        /// </summary>

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
                selectedCustomer = value;
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

        private DateTime ankomsttid;
        public DateTime Ankomsttid
        {
            get { return ankomsttid; }
            set { ankomsttid = value; OnPropertyChanged(); }
        }
        private DateTime avresetid;
        public DateTime Avresetid
        {
            get { return  avresetid; }
            set { avresetid = value; OnPropertyChanged(); }
        }

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
