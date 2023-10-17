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
    public partial class MainViewModel:ObservableObject
    {
        private string searchEquipmentCustomer;
        public string SearchEquipmentCustomer
        {
            get { return searchEquipmentCustomer; }
            set
            {
                searchEquipmentCustomer = value;
                EquipmentCustomerError = string.Empty;
                SearchEquipmentCustomerResults = SearchCustomers(searchEquipmentCustomer);
                if (searchEquipmentCustomer == string.Empty)
                {
                    SelectedEquipmentCustomer = null;
                    SearchEquipmentCustomerResults = new List<Kund>();
                }
                if (SearchEquipmentCustomerResults.Count > 0)
                {
                    ÖppnaDropDown = true;
                }
                if (SearchEquipmentCustomerResults.Count <= 0 || SearchEquipmentCustomer == "")
                {
                    SelectedEquipmentCustomer = null;
                    ÖppnaDropDown = false;
                }
                OnPropertyChanged();
            }
        }

        private string equipmentCustomerError;
        public string EquipmentCustomerError
        {
            get { return equipmentCustomerError; }
            set
            {
                equipmentCustomerError = value;
                OnPropertyChanged();
            }
        }

        private IList<Kund> searchEquipmentCustomerResults = new List<Kund>();
        public IList<Kund> SearchEquipmentCustomerResults
        {
            get { return searchEquipmentCustomerResults; }
            set
            {
                searchEquipmentCustomerResults = value;
                OnPropertyChanged();
            }
        }

        private Kund? selectedEquipmentCustomer;
        public Kund? SelectedEquipmentCustomer
        {
            get { return selectedEquipmentCustomer; }
            set
            {
                if (selectedEquipmentCustomer == value) return;


                if (value != null)
                {
                    selectedEquipmentCustomer = value;

                    SearchEquipmentCustomer = selectedEquipmentCustomer.ToString().Split(" (")[0];
                }
                OnPropertyChanged();
            }
        }

        private IList<Utrustning> equipmentlist;
        public IList<Utrustning> Equipmentlist
        {
            get { return equipmentlist; }
            set
            {
                equipmentlist = value;
                OnPropertyChanged();
            }
        }

        private IList<Utrustning> sizeList;
        public IList<Utrustning> SizeList
        {
            get { return sizeList; }
            set
            {
                sizeList = value;
                OnPropertyChanged();
            }
        }

        private string searchEquipmentOrder;
        public string SearchEquipmentOrder
        {
            get { return searchEquipmentOrder; }
            set
            {
                searchEquipmentOrder = value;
                EquipmentOrderError = string.Empty;
                SearchEquipmentOrderResults = SearchBookings(searchEquipmentOrder,null,null);
                if (searchEquipmentOrder == string.Empty)
                {
                    SelectedEquipmentOrder = null;
                    SearchEquipmentOrderResults = new List<Bokning>();
                }
                if (SearchEquipmentOrderResults.Count > 0)
                {
                    ÖppnaDropDown = true;
                }
                if (SearchEquipmentOrderResults.Count <= 0 || SearchEquipmentOrder == "")
                {
                    SelectedEquipmentOrder = null;
                    ÖppnaDropDown = false;
                }
                OnPropertyChanged();
            }
        }

        private string equipmentOrderError;
        public string EquipmentOrderError
        {
            get { return equipmentOrderError; }
            set
            {
                equipmentOrderError = value;
                OnPropertyChanged();
            }
        }

        private IList<Bokning> searchEquipmentOrderResults = new List<Bokning>();
        public IList<Bokning> SearchEquipmentOrderResults
        {
            get { return searchEquipmentOrderResults; }
            set
            {
                searchEquipmentOrderResults = value;
                OnPropertyChanged();
            }
        }

        private Bokning? selectedEquipmentOrder;
        public Bokning? SelectedEquipmentOrder
        {
            get { return selectedEquipmentOrder; }
            set
            {
                if (selectedEquipmentOrder == value) return;


                if (value != null)
                {
                    selectedEquipmentOrder = value;

                    SearchEquipmentOrder = selectedEquipmentOrder.ToString().Split(" (")[0];
                }
                OnPropertyChanged();
            }
        }

        private IList<Utrustning> foundEquipment;
        public IList<Utrustning> FoundEquipment
        {
            get { return foundEquipment; }
            set
            {
                foundEquipment = value;
                OnPropertyChanged();
            }
        }

        private DateTime? reportDate;
        public DateTime? ReportDate
        {
            get { return reportDate; }
            set
            {
                reportDate = value;
                OnPropertyChanged();
            }
        }

        private IList<Utrustning> currentEquipment;
        public IList<Utrustning> CurrentEquipment
        {
            get { return currentEquipment; }
            set
            {
                currentEquipment = value;
                OnPropertyChanged();
            }
        }

        private ICommand bookEquipment;
        public ICommand BookEquipment =>
            bookEquipment ??= bookEquipment = new RelayCommand(() =>
            {

            });

        private ICommand searchEquipment;
        public ICommand SearchEquipment =>
            searchEquipment ??= searchEquipment = new RelayCommand(() =>
            {

            }); 
        
        private ICommand handOutEquipment;
        public ICommand HandOutEquipment =>
            handOutEquipment ??= handOutEquipment = new RelayCommand(() =>
            {

            });
        
        private ICommand recieveEquipment;
        public ICommand RecieveEquipment =>
            recieveEquipment ??= recieveEquipment = new RelayCommand(() =>
            {

            });
    }
}
