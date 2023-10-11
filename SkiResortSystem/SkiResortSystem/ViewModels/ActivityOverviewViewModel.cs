using BusinessLayer;
using EntityLayer;
using SkiResortSystem.Commands;
using SkiResortSystem.Models;
using SkiResortSystem.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SkiResortSystem.ViewModels
{
    public class ActivityOverviewViewModel:ObservableObject
    {
        public Bokning Bokningsref { get; set; }
        public ObservableCollection<Aktivitetsbokning> Activities { get; set; }
        public ActivityOverviewViewModel()
        {
            
        }
        public ActivityOverviewViewModel(Bokning booking, IList<Aktivitetsbokning> activities)
        {
            Bokningsref = booking;
            Activities = new ObservableCollection<Aktivitetsbokning>(activities);
        }

        private Aktivitetsbokning aktivitetsbokning;
        public Aktivitetsbokning Aktivitetsbokning
        {
            get { return aktivitetsbokning; }
            set
            {
                aktivitetsbokning = value;
                OnPropertyChanged();
            }
        }

        private ICommand saveActivityBooking;
        public ICommand SaveActivityBookingCommand =>
            saveActivityBooking ??= saveActivityBooking = new RelayCommand<ICloseable>((view) =>
            {
                ActivityController ac = new ActivityController();
                foreach (Aktivitetsbokning ab in Activities)
                {
                    try { ac.SaveAktivityBooking(ab); }
                    catch (Exception ex) { MessageBox.Show(ex.Message); }
                }
                closeActivityBooking.Execute(view);
            });
        private ICommand removeActivityBooking;
        public ICommand RemoveActivityBookingCommand =>
            removeActivityBooking ??= removeActivityBooking = new RelayCommand(() =>
            {
                ActivityController ac = new ActivityController();
                ac.RemoveAktivityBooking(aktivitetsbokning); 
                Activities.Remove(aktivitetsbokning);
            });
        private ICommand closeActivityBooking;
        public ICommand CloseActivityBookingCommand =>
            closeActivityBooking ??= closeActivityBooking = new RelayCommand<ICloseable>((view) =>
            {
                view.Close();
            });
    }
}
