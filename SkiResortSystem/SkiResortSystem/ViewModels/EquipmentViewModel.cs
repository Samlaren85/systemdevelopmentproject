using BusinessLayer;
using BusinessLayer.PrintController;
using EntityLayer;
using SkiResortSystem.Commands;
using SkiResortSystem.Components;
using SkiResortSystem.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace SkiResortSystem.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {

        private string searchEquipmentOrder;
        public string SearchEquipmentOrder
        {
            get { return searchEquipmentOrder; }
            set
            {
                searchEquipmentOrder = value;
                EquipmentOrderError = string.Empty;

                SearchEquipmentOrderResults = SearchBookings(searchEquipmentOrder, null, null);
                if (searchEquipmentOrder == string.Empty)
                {
                    Utrustningsbokningsrader = new ObservableCollection<Utrustningsbokningsrad>();
                    OnPropertyChanged(nameof(Utrustningsbokningsrader));
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
                    EquipmentController equipmentController = new EquipmentController();
                    selectedEquipmentOrder = value;
                    Equipmentlist = equipmentController.FindTillgängligUtrustning(selectedEquipmentOrder.Ankomsttid, selectedEquipmentOrder.Avresetid, null);
                    List<string> EquipmentType = new List<string>();
                    foreach (IGrouping<string, Utrustning> group in Equipmentlist.GroupBy(e => e.UtrustningsBenämning))
                    {
                        EquipmentType.Add(group.Key);
                    }
                    Utrustningsbokningsrader = new ObservableCollection<Utrustningsbokningsrad>() { new Utrustningsbokningsrad() { TypAvUtrustning = EquipmentType[0], TypAvUtrustningar = EquipmentType, FrånDatum = selectedEquipmentOrder.Ankomsttid, TillDatum = selectedEquipmentOrder.Avresetid } };
                    OnPropertyChanged(nameof(Utrustningsbokningsrader));
                    SearchEquipmentOrder = selectedEquipmentOrder.ToString().Split(" (")[0];

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

        public ObservableCollection<Utrustningsbokningsrad> Utrustningsbokningsrader { get; set; }

        public void PopulateSizeList()
        {
            SizeList.Clear();
            if (selectedEquipment.TypAvUtrustning != string.Empty)
            {
                foreach (IGrouping<string, string> group in Equipmentlist.GroupBy(e => e.UtrustningsBenämning, e => e.Storlek))
                {
                    if (group.Key.Equals(selectedEquipment.TypAvUtrustning))
                    {
                        foreach (string stl in group)
                        {
                            bool found = false;
                            foreach (Size size in SizeList)
                            {
                                if (size.Storlek.Equals(stl))
                                {
                                    found = true;
                                    break;
                                }
                            }
                            if (!found || !SizeList.Any()) SizeList.Add(new Size(stl));
                        }
                    }
                }
                OnPropertyChanged(nameof(SizeList));
            }
        }

        private Utrustningsbokningsrad selectedEquipment;
        public Utrustningsbokningsrad SelectedEquipment
        {
            get { return selectedEquipment; }
            set
            {
                selectedEquipment = value;
                SelectionError = string.Empty;
                if (selectedEquipment != null)
                {
                    PopulateSizeList();
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<Size> SizeList { get; set; } = new ObservableCollection<Size>();

        private DateTime fromDate;
        public DateTime FromDate
        {
            get { return fromDate; }
            set
            {
                if (value >= SelectedEquipmentOrder.Avresetid || value < SelectedEquipmentOrder.Ankomsttid)
                {
                    ErrorDate = "Fråndatumet måste ligga inom tidsspannet för bokningen";
                    fromDate = SelectedEquipmentOrder.Ankomsttid;
                }
                else
                {
                    fromDate = value;
                }
                OnPropertyChanged();
            }
        }

        private DateTime toDate;
        public DateTime ToDate
        {
            get { return toDate; }
            set
            {
                if (value >= SelectedEquipmentOrder.Avresetid || value < SelectedEquipmentOrder.Ankomsttid)
                {
                    ErrorDate = "Tilldatumet måste ligga inom tidsspannet för bokningen";
                    toDate = SelectedEquipmentOrder.Avresetid;
                }
                else
                {
                    toDate = value;
                }
                OnPropertyChanged();
            }
        }

        private string errorDate;
        public string ErrorDate
        {
            get { return errorDate; }
            set
            {
                errorDate = value;
                OnPropertyChanged();
            }
        }

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

        private DateTime? fetchDate;
        public DateTime? FetchDate
        {
            get { return fetchDate; }
            set
            {
                fetchDate = value;
                OnPropertyChanged();
            }
        }
        private DateTime? returnDate;
        public DateTime? ReturnDate
        {
            get { return returnDate; }
            set
            {
                returnDate = value;
                OnPropertyChanged();
            }
        }

        private string typeOfEquipment;
        public string TypeOfEquipment
        {
            get { return typeOfEquipment; }
            set
            {
                typeOfEquipment = value;
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
                CurrentEquipment = SearchPickupReturn(reportDate);
                OnPropertyChanged();
            }
        }

        private int selectedFetchOrReturn;
        public int SelectedFetchOrReturn
        {
            get { return selectedFetchOrReturn; }
            set
            {
                selectedFetchOrReturn = value;
                CurrentEquipment = SearchPickupReturn(reportDate);
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// Söker utrustningsbokningar
        /// </summary>
        /// <param name="reportDate"></param>
        /// <returns></returns>
        public IList<Utrustningsbokning> SearchPickupReturn(DateTime? reportDate)
        {
            EquipmentController equipmentController = new EquipmentController();
            IList<Utrustningsbokning> utr;
            if (SelectedFetchOrReturn == 0) utr = equipmentController.FindUtrustningsbokningar(reportDate, null);
            else utr = equipmentController.FindUtrustningsbokningar(null, reportDate);
            
            return utr;
        }

        private IList<Utrustningsbokning> currentEquipment;
        public IList<Utrustningsbokning> CurrentEquipment
        {
            get { return currentEquipment; }
            set
            {
                currentEquipment = value;
                OnPropertyChanged();
            }
        }
        
        



        private Utrustningsbokning selectedPickupReturn;
        public Utrustningsbokning SelectedPickupReturn
        {
            get { return selectedPickupReturn; }
            set
            {
                selectedPickupReturn = value;
                OnPropertyChanged();
            }
        }

        private string selectionError;
        public string SelectionError
        {
            get { return selectionError; }
            set
            {
                selectionError = value;
                OnPropertyChanged();
            }
        }

        private Size sizeItem;
        public Size SizeItem
        {
            get { return sizeItem; }
            set
            {
                sizeItem = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Utrustningsbokning> Utrustningsbokningar { get; set; } = new ObservableCollection<Utrustningsbokning>();

        private Utrustningsbokning selectedEquipmentbooking;
        public Utrustningsbokning SelectedEquipmentbooking 
        { 
            get { return selectedEquipmentbooking; }
            set
            {
                selectedEquipmentbooking = value;
                OnPropertyChanged();
            }
        }

        private bool pickupReturn;
        public bool PickupReturn
        {
            get { return pickupReturn; }
            set
            {
                pickupReturn = value;
                ReportDate = DateTime.Today;
                OnPropertyChanged();
            }
        }

        private ICommand addEquipmentRow;
        /// <summary>
        /// Lägger till en ny rad av utrustningar i bokningen
        /// </summary>
        public ICommand AddEquipmentRow =>
            addEquipmentRow ??= addEquipmentRow = new RelayCommand(() =>
            {
                List<string> EquipmentType = new List<string>();
                foreach (IGrouping<string, Utrustning> group in Equipmentlist.GroupBy(e => e.UtrustningsBenämning))
                {
                    EquipmentType.Add(group.Key);
                }
                if (SelectedEquipment != null)
                {
                    Utrustningsbokningsrader.Insert(Utrustningsbokningsrader.IndexOf(SelectedEquipment) + 1, new Utrustningsbokningsrad() { TypAvUtrustning = EquipmentType[0], TypAvUtrustningar = EquipmentType, FrånDatum = selectedEquipmentOrder.Ankomsttid, TillDatum = selectedEquipmentOrder.Avresetid });
                }
                else
                {
                    Utrustningsbokningsrader.Add(new Utrustningsbokningsrad() { TypAvUtrustning = EquipmentType[0], TypAvUtrustningar = EquipmentType, FrånDatum = selectedEquipmentOrder.Ankomsttid, TillDatum = selectedEquipmentOrder.Avresetid });
                }
                OnPropertyChanged(nameof(Utrustningsbokningsrader));
            });

        private ICommand removeEquipmentRow;
        /// <summary>
        /// Tar bort en utrustningsrad i listan
        /// </summary>
        public ICommand RemoveEquipmentRow =>
            removeEquipmentRow ??= removeEquipmentRow = new RelayCommand(() =>
            {
                if (SelectedEquipment != null)
                    Utrustningsbokningsrader.Remove(SelectedEquipment);
                else SelectionError = "Du måste markera en rad i listan först";
                OnPropertyChanged(nameof(Utrustningsbokningsrader));
            });

        private ICommand addSizeSelection;
        /// <summary>
        /// Adderar en utrustning av vald storlek i bokningen
        /// </summary>
        public ICommand AddSizeSelection =>
            addSizeSelection ??= addSizeSelection = new RelayCommand(() =>
            {
                if (SizeItem != null && SelectedEquipment != null)
                {
                    SizeItem.Antal += 1;
                    if (SelectedEquipment.Storlekar == null) SelectedEquipment.Storlekar = new List<string>();
                    SelectedEquipment.Storlekar.Add(SizeItem.Storlek);
                    SelectedEquipment.Antal += 1;
                    SelectedEquipment.Pris += Equipmentlist.FirstOrDefault(e => e.UtrustningsBenämning.Equals(SelectedEquipment.TypAvUtrustning)).Pris;
                    OnPropertyChanged(nameof(SelectedEquipment));
                    OnPropertyChanged(nameof(Utrustningsbokningsrader));
                }
            });

        private ICommand removeSizeSelection;
        /// <summary>
        /// Tar bort en utrustning av vald storlek i bokningen.
        /// </summary>
        public ICommand RemoveSizeSelection =>
            removeSizeSelection ??= removeSizeSelection = new RelayCommand(() =>
            {
                if (SizeItem != null && SelectedEquipment != null && SelectedEquipment.Storlekar.Contains(SizeItem.Storlek))
                {
                    SizeItem.Antal -= 1;
                    SelectedEquipment.Storlekar.Remove(SizeItem.Storlek);
                    SelectedEquipment.Antal -= 1;
                    SelectedEquipment.Pris -= Equipmentlist.FirstOrDefault(e => e.UtrustningsBenämning.Equals(SelectedEquipment.TypAvUtrustning)).Pris;
                    OnPropertyChanged(nameof(SelectedEquipment));
                    OnPropertyChanged(nameof(Utrustningsbokningsrader));
                }
            });

        private ICommand bookEquipment;
        /// <summary>
        /// Skapar en bokning för varje utrustning i listan och öppnar upp en översikt för detta
        /// </summary>
        public ICommand BookEquipment =>
            bookEquipment ??= bookEquipment = new RelayCommand(() =>
            {
                EquipmentController controller = new EquipmentController();
                List<Utrustningsbokning> bokningar = new List<Utrustningsbokning>();
                foreach (Utrustningsbokningsrad ur in Utrustningsbokningsrader)
                {
                    foreach (string stl in ur.Storlekar)
                    {
                        Utrustning utrustning = controller.FindUtrustning(ur.TypAvUtrustning, stl);
                        bokningar.Add(controller.CreateUtrustningsbokning(utrustning, ur.FrånDatum, ur.TillDatum, SelectedEquipmentOrder));
                    }
                }
                EquipmentOverviewViewModel equipmentOverviewViewModel = new EquipmentOverviewViewModel(bokningar, SelectedEquipmentOrder);
                windowService.Show(equipmentOverviewViewModel);
                SelectedEquipmentOrder = null;
                SearchEquipmentOrder = string.Empty;
            });

        private ICommand searchEquipment;
        /// <summary>
        /// Letar efter utrustning i databasen
        /// </summary>
        public ICommand SearchEquipment =>
            searchEquipment ??= searchEquipment = new RelayCommand(() =>
            {
                try
                {
                    EquipmentController equipmentController = new EquipmentController();
                    EquipmentCustomerError = string.Empty;
                    Utrustningsbokningar = new ObservableCollection<Utrustningsbokning>();
                    if (SelectedEquipmentCustomer != null && SelectedEquipmentCustomer.BokningsRef != null)
                    {
                        foreach (Bokning b in SelectedEquipmentCustomer.BokningsRef)
                        {
                            if ((FetchDate == null || b.Ankomsttid >= FetchDate) && (ReturnDate == null || b.Avresetid <= ReturnDate) && b.UtrustningRef != null)
                            {
                                foreach (Utrustningsbokning utr in b.UtrustningRef)
                                {
                                    if ((FetchDate == null || utr.Hämtasut == FetchDate) && (ReturnDate == null || utr.Lämnasin == ReturnDate) && (TypeOfEquipment == null || utr.Utrustning.UtrustningsBenämning.Equals(TypeOfEquipment)))
                                    {
                                        Utrustningsbokningar.Add(utr);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (FetchDate != null || ReturnDate != null)
                        {
                            Utrustningsbokningar = new ObservableCollection<Utrustningsbokning>(equipmentController.FindUtrustningsbokningar(FetchDate, ReturnDate, TypeOfActivity));
                        }
                        else
                        {
                            EquipmentCustomerError = "Någon parameter måste vara angiven innan sökning";
                        }
                    }
                    OnPropertyChanged(nameof(Utrustningsbokningar));
                }
                catch
                {
                    EquipmentCustomerError = "Hittade ingen bokad utrustning";
                }
            });

        /// <summary>
        /// När man dubbelklickar på Utrustningstabellen öppnas översikten så man kan redigera sin order
        /// </summary>
        private ICommand doubleClickEquipmentCommand;
        public ICommand DoubleClickEquipmentCommand =>
            doubleClickEquipmentCommand ??= doubleClickEquipmentCommand = new RelayCommand(() =>
                {
                    EquipmentOverviewViewModel equipmentOverview = new EquipmentOverviewViewModel(SelectedEquipmentbooking.Bokning.UtrustningRef.Where(u => u.Bokning.Equals(SelectedEquipmentbooking.Bokning)).ToList(), SelectedEquipmentbooking.Bokning);
                    windowService.Show(equipmentOverview);
                });

        private ICommand handOutEquipment;
        /// <summary>
        /// Tilldelar status utlämnad till bokningen och skriver ut en uthyrningsblankett
        /// </summary>
        public ICommand HandOutEquipment =>
            handOutEquipment ??= handOutEquipment = new RelayCommand(() =>
            {
                if (SelectedPickupReturn != null && SelectedPickupReturn.Utrustningsstatus == Status.Kommande && SelectedPickupReturn.Hämtasut == DateTime.Today)
                {
                    SelectedPickupReturn.Utrustningsstatus = Status.Utlämnad;
                    PrintController.Run(SelectedPickupReturn);
                    OnPropertyChanged(nameof(CurrentEquipment));
                    CurrentEquipment = SearchPickupReturn(reportDate);
                }
            });
        
        private ICommand recieveEquipment;
        /// <summary>
        /// Tilldelar status inrapporterad till bokningen
        /// </summary>
        public ICommand RecieveEquipment =>
            recieveEquipment ??= recieveEquipment = new RelayCommand(() =>
            {
                if (SelectedPickupReturn != null && SelectedPickupReturn.Utrustningsstatus == Status.Utlämnad && SelectedPickupReturn.Lämnasin == DateTime.Today)
                {
                    SelectedPickupReturn.Utrustningsstatus = Status.Inrapporterad;
                    OnPropertyChanged(nameof(CurrentEquipment));
                    CurrentEquipment = SearchPickupReturn(reportDate);
                }
            });
    }
}
