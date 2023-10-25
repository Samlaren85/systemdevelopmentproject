using BusinessLayer;
using EntityLayer;
using SkiResortSystem.Commands;
using SkiResortSystem.Models;
using SkiResortSystem.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using SkiResortSystem.Services;

namespace SkiResortSystem.ViewModels
{
    public class CustomerOverviewPrivateViewModel : ObservableObject
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
        private bool kreditReadOnly = true;
        public bool KreditReadOnly
        {
            get { return kreditReadOnly; }
            set { kreditReadOnly = value; OnPropertyChanged(); }
        }
        private bool rabattReadOnly = true;
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
                    Rubrik = "PRIVATKUND";
                }
                else
                { 
                    KundIDReadOnly = false;
                    RemoveEnabled = false;
                    Rubrik = "LÄGG TILL NY PRIVATKUND";
                }
                OnPropertyChanged(); 
            }
        }
        private string förnamn;
        public string Förnamn 
        { 
            get { return förnamn; }
            set 
            { 
                förnamn = value;
                if (Kund != null) Kund.Privatkund.Förnamn = value;
                OnPropertyChanged(); 
            }
        }
        private string efternamn;
        public string Efternamn
        {
            get { return efternamn; }
            set 
            { 
                efternamn = value;
                if (Kund != null) Kund.Privatkund.Efternamn = value;
                OnPropertyChanged(); 
            }
        }
        private string personnummer;
        public string Personnummer
        {
            get { return personnummer; }
            set 
            { 
                personnummer = value; if (Kund != null) 
                Kund.Privatkund.PersonnummerID = value;
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
        public CustomerOverviewPrivateViewModel()
        {
            
        }
        public CustomerOverviewPrivateViewModel(bool marketingmanager) 
        {
            customerController = new CustomerController();
            KundIDReadOnly = false;
            Rubrik = "LÄGG TILL NY PRIVATKUND";
            IsCurrentUserMarketingManager = marketingmanager;
        }
        public CustomerOverviewPrivateViewModel(Kund laddadKund, bool marketingmanager)
        {
            customerController = new CustomerController();
            Kund = laddadKund;
            Förnamn = laddadKund.Privatkund.Förnamn;
            Efternamn = laddadKund.Privatkund.Efternamn;
            Personnummer = laddadKund.Privatkund.PersonnummerID;
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
        /// sparar kund
        /// </summary>
        private ICommand saveCustomer = null!;
        public ICommand SaveCustomer =>
            saveCustomer ??= saveCustomer = new RelayCommand<ICloseable>((view) =>
            {
                if (Kund == null)
                {
                    try
                    {
                        Kund = customerController.AddPrivateCustomer(Personnummer, förnamn, efternamn, Gatuadress, Postnummer, Postort, Telefonnummer, Epost);
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
        /// tar bort kund
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
        /// stänger fönstret
        /// </summary>
        private ICommand closeCommand = null;
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
