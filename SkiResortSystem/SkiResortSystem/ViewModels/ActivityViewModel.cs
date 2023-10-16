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

        private string typeOfActivity;
        public string TypeOfActivity
        {
            get { return typeOfActivity; }
            set
            {
                typeOfActivity = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Aktivitetsbokning> aktivitetsbokningar;
        public ObservableCollection<Aktivitetsbokning> Aktivitetsbokningar
        {
            get { return aktivitetsbokningar; }
            set
            {
                aktivitetsbokningar = value;
                OnPropertyChanged();
            }
        }

        public void SearchActivities(DateTime? from, DateTime? to)
        {
            ActivityController ac = new ActivityController();
            AktivitetsSökning = ac.FindSkiSchool(from, to);
            Aktivitetsbokningar = new ObservableCollection<Aktivitetsbokning>();
            foreach (Aktivitet a in AktivitetsSökning)
            {
                if (TypeOfActivity == string.Empty || TypeOfActivity == null || a.Skidskola.Typ.Contains(TypeOfActivity))
                {
                    bool found = SearchBookedActivities(a);
                    if (!found) Aktivitetsbokningar.Add(new Aktivitetsbokning(SelectedActivityBooking, a, 0));
                }

            }
        }

        public bool SearchBookedActivities()
        {
            bool found = false; 
            if (SelectedActivityBooking.AktivitetRef != null)
            {
                foreach (Aktivitetsbokning ab in SelectedActivityBooking.AktivitetRef)
                {
                    Aktivitetsbokningar.Add(ab);
                }
            }
            return found;
        }
        public bool SearchBookedActivities(Aktivitet aktivitet)
        {
            bool found = false;
            if (SelectedActivityBooking.AktivitetRef != null)
            {
                foreach (Aktivitetsbokning ab in SelectedActivityBooking.AktivitetRef)
                {
                    if (ab.Aktivitetsref.Equals(aktivitet))
                    {
                        Aktivitetsbokningar.Add(ab);
                        found = true;
                    }
                }
            }
            return found;
        }

        private IList<Aktivitet> aktivitetsSökning;
        public IList<Aktivitet> AktivitetsSökning
        {
            get { return aktivitetsSökning; }
            set
            {
                aktivitetsSökning = value;
                OnPropertyChanged();
            }
        }

        private string searchActivityCustomer;
        public string SearchActivityCustomer
        {
            get { return searchActivityCustomer; }
            set
            {
                searchActivityCustomer = value;
                ActivityCustomerError = string.Empty;
                SearchActivityCustomerResults = SearchCustomers(searchActivityCustomer);
                if (searchActivityCustomer == string.Empty)
                {
                    SearchActivityCustomerResults = new List<Kund>();
                }
                if (SearchActivityCustomerResults.Count > 0)
                {
                    ÖppnaDropDown = true;
                }
                if (SearchActivityCustomerResults.Count <= 0 || SearchActivityCustomer == "")
                {
                    ÖppnaDropDown = false;
                }
                OnPropertyChanged();
            }
        }


        private IList<Kund> searchActivityCustomerResults = new List<Kund>();
        public IList<Kund> SearchActivityCustomerResults
        {
            get { return searchActivityCustomerResults; }
            set
            {
                searchActivityCustomerResults = value;
                OnPropertyChanged();
            }
        }

        private Kund selectedActivityCustomer;
        public Kund SelectedActivityCustomer
        {
            get { return selectedActivityCustomer; }
            set
            {
                if (selectedActivityCustomer == value) return;


                if (value != null)
                {
                    selectedActivityCustomer = value;

                    SearchActivityCustomer = selectedActivityCustomer.ToString().Split(" (")[0];
                }
                OnPropertyChanged();
            }
        }

        private DateTime? activityDate;
        public DateTime? ActivityDate
        {
            get { return activityDate; }
            set
            {
                activityDate = value;
                ActivityCustomerError = string.Empty;
                OnPropertyChanged();
            }
        }
        private DateTime? activityEndDate;
        public DateTime? ActivityEndDate
        {
            get { return activityEndDate; }
            set
            {
                activityEndDate = value;
                ActivityCustomerError = string.Empty;
                OnPropertyChanged();
            }
        }

        private string searchActivityBooking;
        public string SearchActivityBooking
        {
            get { return searchActivityBooking; }
            set
            {
                if (value != null)
                {
                    searchActivityBooking = value;
                    ActivityBookingResults = SearchBookings(searchActivityBooking, ActivityDate, ActivityEndDate);
                    if (searchActivityBooking == string.Empty)
                    {
                        ActivityBookingResults = new List<Bokning>();
                    }
                    if (ActivityBookingResults.Count > 0)
                    {
                        ÖppnaDropDown = true;
                    }
                    if (ActivityBookingResults.Count <= 0 || SearchActivityBooking == "")
                    {
                        ÖppnaDropDown = false;
                    }
                    OnPropertyChanged();
                }
            }
        }

        private IList<Bokning> activityBookingResults;
        public IList<Bokning> ActivityBookingResults
        {
            get { return activityBookingResults; }
            set
            {
                activityBookingResults = value;
                OnPropertyChanged();
            }
        }

        private string activityCustomerError;
        public string ActivityCustomerError
        {
            get { return activityCustomerError; }
            set
            {
                activityCustomerError = value;
                OnPropertyChanged();
            }
        }

        private Bokning selectedActivityBooking;
        public Bokning SelectedActivityBooking
        {
            get { return selectedActivityBooking; }
            set
            {
                if (value == selectedActivityBooking) return;
                if (value != null)
                {
                    selectedActivityBooking = value;
                    SearchActivityBooking = SelectedActivityBooking.ToString().Split(" (")[0];
                    SearchActivities(selectedActivityBooking.Ankomsttid, selectedActivityBooking.Avresetid);
                }
                OnPropertyChanged();
            }
        }

        private Aktivitetsbokning selectedBookingActivity;
        public Aktivitetsbokning SelectedBookingActivity
        {
            get { return selectedBookingActivity; }
            set
            {
                selectedBookingActivity = value;
                OnPropertyChanged();
            }
        }

        private ICommand searchActivitiesCommand;
        public ICommand SearchActivitiesCommand =>
            searchActivitiesCommand ??= searchActivitiesCommand = new RelayCommand(() =>
                {
                    try
                    {
                        ActivityCustomerError = string.Empty;
                        Aktivitetsbokningar = new ObservableCollection<Aktivitetsbokning>();
                        if (SelectedActivityCustomer != null)
                        {
                            foreach (Bokning b in SelectedActivityCustomer.BokningsRef)
                            {
                                if ((ActivityDate == null || b.Ankomsttid >= ActivityDate) && (ActivityEndDate == null || b.Avresetid <= ActivityEndDate))
                                {
                                    SelectedActivityBooking = b;
                                    SearchBookedActivities();
                                }
                            }
                        }
                        else
                        {
                            if (ActivityDate != null ||  ActivityEndDate != null )
                            {
                                SearchActivities(ActivityDate, ActivityEndDate);
                            }
                            else
                            {
                                ActivityCustomerError = "Någon parameter måste vara angiven innan sökning";
                            }
                        }
                    }
                    catch
                    {
                        ActivityCustomerError = "Hittade inga bokade aktiviteter";
                    }
                    
                });

        private ICommand doubleClickActivityCommand;
        public ICommand DoubleClickActivityCommand =>
            doubleClickActivityCommand ??= doubleClickActivityCommand = new RelayCommand(() =>
            {
                ActivityOverviewViewModel aktivitetsöversikt = new ActivityOverviewViewModel(SelectedBookingActivity.Bokningsref, new ObservableCollection<Aktivitetsbokning>(SelectedBookingActivity.Bokningsref.AktivitetRef));
            });

        private ICommand bookActivity;
        public ICommand BookActivity =>
            bookActivity ??= bookActivity = new RelayCommand(() =>
            {
                if (AktivitetsSökning != null)
                {
                    List<Aktivitetsbokning> removableActivities = new List<Aktivitetsbokning>();
                    foreach (Aktivitetsbokning ab in Aktivitetsbokningar)
                    {
                        if (ab.Antal == 0) removableActivities.Add(ab);
                    }
                    Aktivitetsbokningar = new ObservableCollection<Aktivitetsbokning>(Aktivitetsbokningar.Except(removableActivities).ToList());
                    ActivityOverviewViewModel aktivitetsöversikt = new ActivityOverviewViewModel(SelectedActivityBooking, Aktivitetsbokningar);
                    windowService.ShowDialog(aktivitetsöversikt);
                    SearchActivityBooking = string.Empty;
                    SelectedActivityBooking = null!;
                    Aktivitetsbokningar = new ObservableCollection<Aktivitetsbokning>();
                }
            });
    }
}
