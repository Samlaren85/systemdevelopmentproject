using BusinessLayer;
using EntityLayer;
using SkiResortSystem.Commands;
using SkiResortSystem.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SkiResortSystem.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private ObservableCollection<Bokning> hämtadeFakturor {  get; set; }
        public ObservableCollection<Bokning> HämtadeFakturor
        {
            get { return hämtadeFakturor; }
            set
            {
                hämtadeFakturor = value;
            }
        }

        private Bokning selectFaktura;
        public Bokning SelectFaktura
        {
            get { return selectFaktura; }
            set
            {
                selectFaktura = value;
            }
        }

        private Faktura selectedFaktura;
        public Faktura SelectedFaktura
        {
            get { return selectedFaktura; }
            set
            {
                if (value == selectedFaktura) return;
                if (value != null)
                {
                    selectedFaktura = value;
                    SearchBills();
                }
                OnPropertyChanged();
            }
        }

        private string searchFaktura;
        public string SearchFaktura
        {
            get { return searchFaktura; }
            set
            {
                if (value != null)
                {
                    searchFaktura = value;
                    SearchBills(); if (searchFaktura == string.Empty) { FakturaResults = new List<Faktura>(); }
                    OnPropertyChanged(SearchFaktura);
                }
            }
        }

        private IList<Faktura> fakturaResults;
        public IList<Faktura> FakturaResults
        {
            get { return fakturaResults; }
            set
            {
                fakturaResults = value;
                OnPropertyChanged();
            }
        }
        public void SearchBills()
        {
            EconomyController ec = new EconomyController();
            FakturaSökning = ec.FindFaktura(SelectFaktura);
        }

        private Faktura fakturaSökning;
        public Faktura FakturaSökning
        {
            get { return fakturaSökning; }
            set
            {
                fakturaSökning = value;
                OnPropertyChanged();
            }
        }
        private ICommand createFaktura = null!;
        public ICommand CreateFaktura =>
            createFaktura ??= createFaktura = new RelayCommand(() =>
            {
                EconomyController ec = new EconomyController();
                ec.CreateFaktura(SelectFaktura);
            }
            );
        private ICommand fetchFaktura = null!;
        public ICommand FetchFaktura => fetchFaktura ??= fetchFaktura = new RelayCommand(() =>
        {
            EconomyController ec = new EconomyController();
            BookingController bc = new BookingController();
            HämtadeFakturor = (ObservableCollection<Bokning>)bc.FindMasterBooking();
        }
        );

       /* private ICommand doubleClickBillingCommand = null!;
        public ICommand DoubleClickBillingCommand =>
            doubleClickBillingCommand ??= doubleClickBillingCommand = new RelayCommand(() =>
            {
                if (SelectedFaktura != null)
                {
                    BookingOverviewViewModel bokningsöversikt = new BookingOverviewViewModel(SelectedCustomer, SelectedFacility, Avresetid, Ankomsttid, antalpersoner, Facilitetspriset);
                    windowService.ShowDialog(bokningsöversikt);
                }
            });*/
    }
}
