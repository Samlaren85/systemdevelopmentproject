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

namespace SkiResortSystem.ViewModels
{
    public class CustomerOverviewCompanyViewModel: ObservableObject
    {
        private CustomerController customerController;
        private Kund kund;
        public Kund Kund
        {
            get { return kund; }
            set { kund = value; OnPropertyChanged(); }
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
                if (Kund != null) Kund.Företagskund.OrganisationsnummerID = value;
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
            customerController = new CustomerController();
        }
        public CustomerOverviewCompanyViewModel(Kund laddadKund)
        {
            customerController = new CustomerController();
            Kund = laddadKund;
            Företagsnamn = laddadKund.Företagskund.Företagsnamn;
            Kontakt = laddadKund.Företagskund.Kontaktperson;
            Organisationsnummer = laddadKund.Företagskund.OrganisationsnummerID;
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
        }
        private ICommand saveCustomer = null!;
        public ICommand SaveCustomer =>
            saveCustomer ??= saveCustomer = new RelayCommand<ICloseable>((view) =>
            {
                if (Kund == null)
                {
                    try
                    {
                        Kund = customerController.AddCompanyCustomer(Organisationsnummer, Företagsnamn, Kontakt, Besöksadress, Besökspostnummer, Besöksort, Gatuadress, Postnummer, Postort, Telefonnummer, Epost);
                        MessageBoxResult respons = MessageBox.Show($"Kund {Kund} är sparad i systemet!\nVill du gå tillbaks till kundmodulen", "Sparad kund", MessageBoxButton.YesNo);
                        if (respons == MessageBoxResult.Yes) CloseCommand.Execute(view);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                else
                {
                    customerController.ChangeCustomer(Kund);
                    MessageBoxResult respons = MessageBox.Show($"Kund {Kund} är uppdaterad i systemet!", "Uppdaterad kund");
                }
            });
        private ICommand closeCommand = null!;
        public ICommand CloseCommand =>
            closeCommand ??= closeCommand = new RelayCommand<ICloseable>((closeable) =>
            {
                closeable.Close();
            });
    }
}
