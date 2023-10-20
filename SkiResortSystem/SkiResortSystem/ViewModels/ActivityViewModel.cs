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

        public IList<Bokning> SearchActivities(DateTime? from, DateTime? to, string type, Bokning booking)
        {
            IList<Bokning> searchActivities = new List<Bokning>();
            ActivityController ac = new ActivityController();
            AktivitetsSökning = ac.FindSkiSchool(from, to);
            Aktivitetsbokningar = new ObservableCollection<Aktivitetsbokning>(SearchBookedActivities(booking));
            foreach (Aktivitet activity in AktivitetsSökning)
            {
                if (type == string.Empty || type == null || activity.Skidskola.Typ.Contains(type))
                {
                    
                    if (!SearchBookedActivities(activity, booking)) Aktivitetsbokningar.Add(new Aktivitetsbokning(booking, activity, 0));
                }
            }
            return searchActivities;
        }

        public IList<Aktivitetsbokning> SearchBookedActivities(Bokning booking)
        {
            IList<Aktivitetsbokning> searchActivities = new List<Aktivitetsbokning>();
            if (booking.AktivitetRef != null)
            {
                foreach (Aktivitetsbokning ab in booking.AktivitetRef)
                {
                    if (TypeOfActivity == null || ab.Aktivitetsref.Typ.Contains(TypeOfActivity))
                        searchActivities.Add(ab);
                }
            }
            return searchActivities;
        }
        public bool SearchBookedActivities(Aktivitet aktivitet, Bokning booking)
        {
            bool found = false;
            if (booking.AktivitetRef != null)
            {
                foreach (Aktivitetsbokning ab in booking.AktivitetRef)
                {
                    if (ab.Aktivitetsref.Equals(aktivitet))
                    {
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
                    SelectedActivityCustomer = null;
                    SearchActivityCustomerResults = new List<Kund>();
                }
                if (SearchActivityCustomerResults.Count > 0)
                {
                    ÖppnaDropDown = true;
                }
                if (SearchActivityCustomerResults.Count <= 0 || SearchActivityCustomer == "")
                {
                    SelectedActivityCustomer = null;
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

        private Kund? selectedActivityCustomer;
        public Kund? SelectedActivityCustomer
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
                    ActivityBookingResults = SearchBookings(searchActivityBooking, null, null);
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
                    SearchActivities(selectedActivityBooking.Ankomsttid, selectedActivityBooking.Avresetid, TypeOfActivity, SelectedActivityBooking);
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
                                if ((ActivityDate == null || b.Ankomsttid <= ActivityDate) && (ActivityEndDate == null || b.Avresetid >= ActivityEndDate))
                                {
                                    Aktivitetsbokningar = new ObservableCollection<Aktivitetsbokning>(SearchBookedActivities(b));
                                }
                            }
                        }
                        else
                        {
                            if (ActivityDate != null ||  ActivityEndDate != null )
                            {
                                ActivityController ac = new ActivityController();
                                Aktivitetsbokningar = new ObservableCollection<Aktivitetsbokning>(ac.FindBookedActivities(ActivityDate, ActivityEndDate, TypeOfActivity));
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
                ActivityOverviewViewModel aktivitetsöversikt = new ActivityOverviewViewModel(SelectedBookingActivity.Bokningsref, SelectedBookingActivity.Bokningsref.AktivitetRef.ToList());
                windowService.ShowDialog(aktivitetsöversikt);
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
