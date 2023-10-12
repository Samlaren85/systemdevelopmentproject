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
        public string searchActivityBooking;
        public string SearchAktivityBooking
        {
            get { return searchActivityBooking; }
            set
            {
                if (value != null)
                {
                    searchActivityBooking = value;
                    if (TypeOfActivity != string.Empty) TypeOfActivity = string.Empty;
                    if (Ankomsttid != DateTime.Today) Ankomsttid = DateTime.Today;
                    if (Avresetid != DateTime.Today) Avresetid = DateTime.Today;
                    SearchBookings(searchActivityBooking);
                    OnPropertyChanged();
                }
            }
        }

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

        private ICommand searchActivitiesCommand;
        public ICommand SearchActivitiesCommand =>
            searchActivitiesCommand ??= searchActivitiesCommand = new RelayCommand(() =>
                {
                    try
                    {
                        Aktivitetsbokningar = new List<Aktivitetsbokning>();
                        foreach (Bokning b in SelectedCustomer.BokningsRef)
                        {
                            if (b.Ankomsttid >= Ankomsttid && b.Avresetid <= Avresetid)
                            {
                                SelectedBooking = b;
                                SearchBookedActivities();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ErrorMessage = "Hittade inga bokade aktiviteter";
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
                    SearchAktivityBooking = string.Empty;
                    SelectedBooking = null!;
                    Aktivitetsbokningar = new List<Aktivitetsbokning>();
                }
            });
    }
}
