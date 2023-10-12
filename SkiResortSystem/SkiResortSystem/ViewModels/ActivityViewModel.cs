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

        private List<Aktivitetsbokning> aktivitetsbokningar;
        public List<Aktivitetsbokning> Aktivitetsbokningar
        {
            get { return aktivitetsbokningar; }
            set
            {
                aktivitetsbokningar = value;
                OnPropertyChanged();
            }
        }

        public void SearchActivities()
        {
            ActivityController ac = new ActivityController();
            AktivitetsSökning = ac.FindSkiSchool(SelectedBooking.Ankomsttid, SelectedBooking.Avresetid);
            Aktivitetsbokningar = new List<Aktivitetsbokning>();
            foreach (Aktivitet a in AktivitetsSökning)
            {
                if (TypeOfActivity == string.Empty || a.Skidskola.Typ.Contains(TypeOfActivity))
                {
                    bool found = SearchBookedActivities(a);
                    if (!found) Aktivitetsbokningar.Add(new Aktivitetsbokning(SelectedBooking, a, 0));
                }

            }
        }

        public bool SearchBookedActivities()
        {
            bool found = false; 
            if (SelectedBooking.AktivitetRef != null)
            {
                foreach (Aktivitetsbokning ab in SelectedBooking.AktivitetRef)
                {
                    Aktivitetsbokningar.Add(ab);
                }
            }
            return found;
        }
        public bool SearchBookedActivities(Aktivitet aktivitet)
        {
            bool found = false;
            if (SelectedBooking.AktivitetRef != null)
            {
                foreach (Aktivitetsbokning ab in SelectedBooking.AktivitetRef)
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
                SearchCustomers();
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


        private List<Kund> searchActivityCustomerResults = new List<Kund>();
        public List<Kund> SearchActivityCustomerResults
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
                    SearchBookings(searchActivityBooking);
                    if (searchActivityBooking == string.Empty) { BookingResults = new List<Bokning>(); }
                    OnPropertyChanged(SearchActivityBooking);
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
                    SearchActivities();
                }
                OnPropertyChanged();
            }
        }

        private ICommand searchActivitiesCommand;
        public ICommand SearchActivitiesCommand =>
            searchActivitiesCommand ??= searchActivitiesCommand = new RelayCommand(() =>
                {
                    try
                    {
                        Aktivitetsbokningar = new List<Aktivitetsbokning>();
                        foreach (Bokning b in SelectedActivityCustomer.BokningsRef)
                        {
                            if ((ActivityDate == null || b.Ankomsttid >= ActivityDate) && (ActivityEndDate == null || b.Avresetid <= ActivityEndDate))
                            {
                                SelectedBooking = b;
                                SearchBookedActivities();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ActivityCustomerError = "Hittade inga bokade aktiviteter";
                    }
                    
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
                    Aktivitetsbokningar = Aktivitetsbokningar.Except(removableActivities).ToList();
                    ActivityOverviewViewModel aktivitetsöversikt = new ActivityOverviewViewModel(SelectedBooking, Aktivitetsbokningar);
                    windowService.ShowDialog(aktivitetsöversikt);
                    SearchActivityBooking = string.Empty;
                    SelectedActivityBooking = null!;
                    Aktivitetsbokningar = new List<Aktivitetsbokning>();
                }
            });
    }
}
