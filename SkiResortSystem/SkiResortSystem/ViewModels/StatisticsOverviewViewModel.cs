using Accessibility;
using SkiResortSystem.Commands;
using SkiResortSystem.Models;
using SkiResortSystem.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SkiResortSystem.ViewModels
{
    public class StatisticsOverviewViewModel : ObservableObject
    {
        public IList<object> _data {  get; set; }
        
        public StatisticsOverviewViewModel(List<object> data, string typ)
        {
            _data = data;
            switch (typ)
            {
                case "Facilitet": Facilitetsrapport = Visibility.Visible; break;
                case "Aktivitet": Aktivitetsrapport = Visibility.Visible; break;
                case "Utrustning": Utrustningsrapport = Visibility.Visible; break;
                case "Kund": Kundrapport = Visibility.Visible; break;
            }
        }
        private Visibility facilitetsrapport = Visibility.Collapsed;
        public Visibility Facilitetsrapport
        {
            get { return facilitetsrapport; }
            set
            {
                facilitetsrapport = value;
                OnPropertyChanged();
            }
        }
        private Visibility aktivitetsrapport = Visibility.Collapsed;
        public Visibility Aktivitetsrapport
        {
            get { return aktivitetsrapport; }
            set
            {
                aktivitetsrapport = value;
                OnPropertyChanged();
            }
        }
        private Visibility utrustningsrapport = Visibility.Collapsed;
        public Visibility Utrustningsrapport
        {
            get { return utrustningsrapport; }
            set
            {
                utrustningsrapport = value;
                OnPropertyChanged();
            }
        }
        private Visibility kundrapport = Visibility.Collapsed;
        public Visibility Kundrapport
        {
            get { return kundrapport; }
            set
            {
                kundrapport = value;
                OnPropertyChanged();
            }
        }
        public StatisticsOverviewViewModel()
        {
            _data = new List<object>();
        }


        private ICommand closeCommand;
        public ICommand CloseCommand =>
            closeCommand ??= closeCommand = new RelayCommand<ICloseable>((view) =>
            {
                view.Close();
            });
    }
}
