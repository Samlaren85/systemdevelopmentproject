using BusinessLayer;
using EntityLayer;
using SkiResortSystem.Commands;
using SkiResortSystem.Models;
using SkiResortSystem.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        /// <summary>
        /// Sparar bokad aktivitet
        /// </summary>
        private ICommand saveActivityBooking;
        public ICommand SaveActivityBookingCommand =>
            saveActivityBooking ??= saveActivityBooking = new RelayCommand<ICloseable>((view) =>
            {
                ActivityController ac = new ActivityController();
                foreach (Aktivitetsbokning ab in Activities)
                {
                    try 
                    {
                        ac.SaveAktivityBooking(ab);
                    }
                    catch (Exception ex) { MessageBox.Show(ex.Message); }
                }
                closeActivityBooking.Execute(view);
            });
        /// <summary>
        /// tar bort bokad aktivitet
        /// </summary>
        private ICommand removeActivityBooking;
        public ICommand RemoveActivityBookingCommand =>
            removeActivityBooking ??= removeActivityBooking = new RelayCommand(() =>
            {
                ActivityController ac = new ActivityController();
                ac.RemoveAktivityBooking(aktivitetsbokning); 
                Activities.Remove(aktivitetsbokning);
            });

        /// <summary>
        /// stänger fönstret
        /// </summary>
        private ICommand closeActivityBooking;
        public ICommand CloseActivityBookingCommand =>
            closeActivityBooking ??= closeActivityBooking = new RelayCommand<ICloseable>((view) =>
            {
                view.Close();
            });
    }
}
