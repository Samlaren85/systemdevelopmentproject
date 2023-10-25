using BusinessLayer;
using EntityLayer;
using SkiResortSystem.Commands;
using SkiResortSystem.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SkiResortSystem.ViewModels
{
    /// <summary>
    /// Partiell-klass. Vy-modell som innehåller metoder som berör fakturering/ekonomi.
    /// </summary>
    public partial class MainViewModel : ObservableObject
    {
        /// <summary>
        /// Lista innehållandes bokningar som ännu inte fakturerats.
        /// </summary>
        private IList<Bokning> hämtadeBokningarAttFakturera { get; set; }
        public IList<Bokning> HämtadeBokningarAttFakturera 
        {
            get { return hämtadeBokningarAttFakturera; }
            set
            {
                hämtadeBokningarAttFakturera = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// Property som hanterar den selekterade fakturan i datagriden vid skapande av faktura.
        /// </summary>
        private bool createBillSelected;
        public bool CreateBillSelected
        {
            get { return createBillSelected; }
            set
            {
                createBillSelected = value;
                if (createBillSelected) FetchFaktura.Execute(true);
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// Property som binds till datagriden i fliken skapa fakturor.
        /// </summary>
        private Bokning skapandeAvFakturor { get; set; }
        public Bokning SkapandeAvFakturor 
        {
            get { return skapandeAvFakturor; }
            set
            {
                skapandeAvFakturor = value;
                OnPropertyChanged();
            }
        }
        
        /// <summary>
        /// Property för markerat val i datagriden för fliken hantera fakturor.
        /// </summary>
        private Faktura selectFaktura;
        public Faktura SelectFaktura
        {
            get { return selectFaktura; }
            set
            {
                selectFaktura = value;
            }
        }

        private List<Faktura> faktureradeFakturor {  get; set; }
        public List<Faktura> FaktureradeFakturor 
        {
            get { return faktureradeFakturor; }
            set
            {
                faktureradeFakturor = value;
                OnPropertyChanged();
            }
        }

        private string sökFaktureradeFakturor { get; set; }
        public string SökFaktureradeFakturor
        {
            get { return sökFaktureradeFakturor; }
            set
            {
                sökFaktureradeFakturor= value;
                HämtaFaktureradeFakturor.Execute(true);
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// Visar felmeddelande om faktura fliken inte hanteras korrekt av användaren.
        /// </summary>
        private string fakturaerrormsg { get; set; }
        public string Fakturaerrormsg
        {
            get { return fakturaerrormsg; }
            set 
            {
                fakturaerrormsg = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// Skapar fakturor utifrån val i listan i fliken skapafaktura.
        /// </summary>
        private ICommand createFaktura = null!;
        public ICommand CreateFaktura =>
            createFaktura ??= createFaktura = new RelayCommand(() =>
            {
                Fakturaerrormsg = string.Empty;
                EconomyController ec = new EconomyController();
                if (SkapandeAvFakturor == null)
                {
                    Fakturaerrormsg= "Du behöver välja en bokning i listan";
                }
                else
                {
                    ec.CreateFaktura(SkapandeAvFakturor);
                    HämtadeBokningarAttFakturera = new List<Bokning>();
                    FetchFaktura.Execute(true);
                }
            }
            );
        /// <summary>
        /// Hämtar fakturor som ännu inte fakturerats.
        /// </summary>
        private ICommand fetchFaktura = null!;
        public ICommand FetchFaktura => fetchFaktura ??= fetchFaktura = new RelayCommand(() =>
        {
            BookingController bc = new BookingController();
            HämtadeBokningarAttFakturera = bc.FindMasterBooking();
        }
        );
        /// <summary>
        /// Hämtar fakturor som har fakturerats mot kund.
        /// </summary>
        private ICommand hämtaFaktureradeFakturor = null!;
        public ICommand HämtaFaktureradeFakturor => hämtaFaktureradeFakturor ??= hämtaFaktureradeFakturor = new RelayCommand(() =>
        {
            EconomyController ec = new EconomyController();
            BookingController bc = new BookingController();
            List<Bokning> Lista = bc.FindMasterBooking(SökFaktureradeFakturor).ToList();
            FaktureradeFakturor = ec.HämtaFaktureradeFakturor(Lista);
        }
        );

        private ICommand doubleClickBillingCommand = null!;
        public ICommand DoubleClickBillingCommand =>
            doubleClickBillingCommand ??= doubleClickBillingCommand = new RelayCommand(() =>
            {
                if (SelectFaktura != null)
                {
                    BillOverviewViewModel fakturaöversikt = new BillOverviewViewModel(SelectFaktura);
                    windowService.ShowDialog(fakturaöversikt);
                }
            });
    }
}
