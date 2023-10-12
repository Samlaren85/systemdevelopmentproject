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
        //Properties för företagskunder
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
        private string searchText;
        public string SearchText
        {
            get { return searchText; }
            set
            {
                searchText = value;
                SearchCustomers();
                if (searchText == string.Empty)
                {
                    SearchResults = new List<Kund>();
                }
                if (SearchResults.Count > 0)
                {
                    ÖppnaDropDown = true;
                }
                if (SearchResults.Count <= 0 || SearchText == "")
                {
                    ÖppnaDropDown = false;
                }
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
                if (selectedCustomer == value) return;


                if (value != null)
                {
                    selectedCustomer = value;

                    SearchText = selectedCustomer.ToString().Split(" (")[0];
                }
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
                if (searchResults.Count < 1)
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
        private ICommand customerPrivateOverview = null!;
        public ICommand CustomerPrivateOverview =>
            customerPrivateOverview ??= customerPrivateOverview = new RelayCommand(() =>
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
    }
}
