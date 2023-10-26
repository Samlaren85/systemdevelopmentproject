using BusinessLayer;
using EntityLayer;
using SkiResortSystem.Commands;
using SkiResortSystem.Models;
using SkiResortSystem.Services;
using System;
using System.Windows;
using System.Windows.Input;

namespace SkiResortSystem.ViewModels
{
    public class CustomerOverviewCompanyViewModel: ObservableObject
    {
        private CustomerController customerController;
        private string rubrik;
        public string Rubrik
        {
            get { return rubrik; }
            set { rubrik = value; OnPropertyChanged(); }
        }
        private bool kundIDReadOnly = true;
        public bool KundIDReadOnly
        {
            get { return kundIDReadOnly; }
            set { kundIDReadOnly = value; OnPropertyChanged(); }
        }
        private bool kreditReadOnly = false;
        public bool KreditReadOnly
        {
            get { return kreditReadOnly; }
            set { kreditReadOnly = value; OnPropertyChanged(); }
        }
        private bool rabattReadOnly = false;
        public bool RabattReadOnly
        {
            get { return rabattReadOnly; }
            set { rabattReadOnly = value; OnPropertyChanged(); }
        }
        private bool removeEnabled = false;
        public bool RemoveEnabled
        {
            get { return removeEnabled; }
            set { removeEnabled = value; OnPropertyChanged(); }
        }
        private bool _isCurrentUserMarketingManager;
        public bool IsCurrentUserMarketingManager
        {
            get { return _isCurrentUserMarketingManager; }
            set
            {
                if (_isCurrentUserMarketingManager != value)
                {
                    _isCurrentUserMarketingManager = value;
                    OnPropertyChanged(nameof(IsCurrentUserMarketingManager));
                }
            }
        }

        private Kund kund;
        public Kund Kund
        {
            get { return kund; }
            set
            {
                kund = value;
                if (value != null)
                {
                    KundIDReadOnly = true;
                    RemoveEnabled = true;
                    Rubrik = "FÖRETAGSKUND";
                }
                else
                {
                    KundIDReadOnly = false;
                    RemoveEnabled = false;
                    Rubrik = "LÄGG TILL NY FÖRETAGSKUND";
                }
                OnPropertyChanged();
            }
        }
        private string företagsnamn;
        public string Företagsnamn
        {
            get { return företagsnamn; }
            set
            {
                företagsnamn = value;
                if (Kund != null) Kund.Företagskund.Företagsnamn = value;
                OnPropertyChanged();
            }
        }
        private string kontakt;
        public string Kontakt
        {
            get { return kontakt; }
            set
            {
                kontakt = value;
                if (Kund != null) Kund.Företagskund.Kontaktperson = value;
                OnPropertyChanged();
            }
        }
        private string organisationsnummer;
        public string Organisationsnummer
        {
            get { return organisationsnummer; }
            set
            {
                organisationsnummer = value; 
                if (Kund != null) Kund.Företagskund.OrganisationsnummerId = value;
                OnPropertyChanged();
            }
        }
        private string besöksadress;
        public string Besöksadress
        {
            get { return besöksadress; }
            set
            {
                besöksadress = value;
                if (Kund != null) Kund.Företagskund.Besöksadress = value;
                OnPropertyChanged();
            }
        }
        private string besökspostnummer;
        public string Besökspostnummer
        {
            get { return besökspostnummer; }
            set
            {
                besökspostnummer = value;
                if (Kund != null) Kund.Företagskund.Besökspostnummer = value;
                OnPropertyChanged();
            }
        }
        private string besöksort;
        public string Besöksort
        {
            get { return besöksort; }
            set
            {
                besöksort = value;
                if (Kund != null) Kund.Företagskund.Besöksort = value;
                OnPropertyChanged();
            }
        }
        private string gatuadress;
        public string Gatuadress
        {
            get { return gatuadress; }
            set
            {
                gatuadress = value;
                if (Kund != null) Kund.Gatuadress = value;
                OnPropertyChanged();
            }
        }
        private string postnummer;
        public string Postnummer
        {
            get { return postnummer; }
            set
            {
                postnummer = value;
                if (Kund != null) Kund.Postnummer = value;
                OnPropertyChanged();
            }
        }
        private string postort;
        public string Postort
        {
            get { return postort; }
            set
            {
                postort = value;
                if (Kund != null) Kund.Ort = value;
                OnPropertyChanged();
            }
        }
        private string telefonnummer;
        public string Telefonnummer
        {
            get { return telefonnummer; }
            set
            {
                telefonnummer = value;
                if (Kund != null) Kund.Telefonnummer = value;
                OnPropertyChanged();
            }
        }
        private string epost;
        public string Epost
        {
            get { return epost; }
            set
            {
                epost = value;
                if (Kund != null) Kund.Epost = value;
                OnPropertyChanged();
            }
        }
        private float kreditgräns;
        public float Kreditgräns
        {
            get { return kreditgräns; }
            set { kreditgräns = value; if (Kund != null) Kund.Kreditgräns = value; OnPropertyChanged(); }
        }
        private float rabatt;
        public float Rabatt
        {
            get { return rabatt; }
            set { rabatt = value; if (Kund != null) Kund.Rabatt = value; OnPropertyChanged(); }
        }
        public CustomerOverviewCompanyViewModel()
        {
            
        }
        public CustomerOverviewCompanyViewModel(bool marketingmanager)
        {
            customerController = new CustomerController();
            KundIDReadOnly = false;
            Rubrik = "LÄGG TILL NY FÖRETAGSKUND";
            IsCurrentUserMarketingManager = marketingmanager;
        }
        public CustomerOverviewCompanyViewModel(Kund laddadKund, bool marketingmanager)
        {
            customerController = new CustomerController();
            Kund = laddadKund;
            Företagsnamn = laddadKund.Företagskund.Företagsnamn;
            Kontakt = laddadKund.Företagskund.Kontaktperson;
            Organisationsnummer = laddadKund.Företagskund.OrganisationsnummerId;
            Besöksadress = laddadKund.Företagskund.Besöksadress;
            Besökspostnummer = laddadKund.Företagskund.Besökspostnummer;
            Besöksort = laddadKund.Företagskund.Besöksort;
            Gatuadress = laddadKund.Gatuadress;
            Postnummer = laddadKund.Postnummer;
            Postort = laddadKund.Ort;
            Telefonnummer = laddadKund.Telefonnummer;
            Epost = laddadKund.Epost;
            Rabatt = laddadKund.Rabatt;
            Kreditgräns = laddadKund.Kreditgräns;
            IsCurrentUserMarketingManager = marketingmanager;
        }

        /// <summary>
        /// Sparar kund
        /// </summary>
        private ICommand saveCustomer = null!;
        public ICommand SaveCustomer =>
            saveCustomer ??= saveCustomer = new RelayCommand<ICloseable>((view) =>
            {
                if (Kund == null)
                {
                    try
                    {
                        Kund = customerController.AddCompanyCustomer(Organisationsnummer, Företagsnamn, Kontakt, Besöksadress, Besökspostnummer, Besöksort, Gatuadress, Postnummer, Postort, Telefonnummer, Epost);
                        MessageBoxResult respons = MessageBox.Show($"Kund {Kund} är sparad i systemet!\nVill du gå tillbaka till kundmodulen?", "Sparad kund", MessageBoxButton.YesNo);
                        if (respons == MessageBoxResult.Yes) CloseCommand.Execute(view);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                else
                {
                    try
                    {
                        customerController.ChangeCustomer(Kund);
                        MessageBoxResult respons = MessageBox.Show($"Kund {Kund} är uppdaterad i systemet!", "Uppdaterad kund");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            });
        /// <summary>
        /// Tar bort kund
        /// </summary>
        private ICommand removeCustomer = null!;
        public ICommand RemoveCustomer =>
            removeCustomer ??= removeCustomer = new RelayCommand<ICloseable>((view) =>
            {
                if (Kund != null)
                {
                    try
                    {
                        string name = Kund.ToString();
                        bool removed = customerController.RemoveCustomer(Kund);
                        if (removed)
                        {
                            MessageBoxResult respons = MessageBox.Show($"Kund {name} är borttagen ur systemet!", "Borttagen kund");
                            CloseCommand.Execute(view);
                        }
                        else
                        {
                            MessageBoxResult respons = MessageBox.Show($"Kunden {name} kunde inte tas bort ur systemet!", "Borttagen kund");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                else
                {
                    MessageBoxResult respons = MessageBox.Show($"Ingen kund är vald för att ta bort!", "Borttagen kund");
                }
            });
        /// <summary>
        /// Stänger fönster
        /// </summary>
        private ICommand closeCommand = null!;
        public ICommand CloseCommand =>
            closeCommand ??= closeCommand = new RelayCommand<ICloseable>((closeable) =>
            {
                closeable.Close();
            });

        private bool marketingManager = false;
        public bool MarketingManager
        {
            get { return marketingManager; }
            set
            {
                if (marketingManager != value)
                {
                    marketingManager = value;
                    OnPropertyChanged(nameof(MarketingManager));
                }
            }
        }
    }
}
