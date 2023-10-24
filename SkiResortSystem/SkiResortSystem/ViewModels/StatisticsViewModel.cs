using BusinessLayer;
using SkiResortSystem.Commands;
using SkiResortSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SkiResortSystem.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private int statisticsType; //0: Beläggningsstatistik, 1: Försäljningsstatistik
        public int StatisticsType
        {  get { return statisticsType; }
            set 
            {  
                statisticsType = value; 
                OnPropertyChanged();
            } 
        }

        private IList<int> statisticYear;
        public IList<int> StatisticYear
        {
            get { return statisticYear; }
            set
            {
                statisticYear = value;
                OnPropertyChanged();
            }
        }
        
        private int selectedStatisticYear;
        public int SelectedStatisticYear
        {
            get { return selectedStatisticYear; }
            set
            {
                selectedStatisticYear = value;
                OnPropertyChanged();
            }
        }

        private int selectedPeriod;
        public int SelectedPeriod
        {
            get { return selectedPeriod; }
            set
            {
                selectedPeriod = value;
                OnPropertyChanged();
            }
        }
        private int selectedUnitType; 
        public int SelectedUnitType
        {
            get { return selectedUnitType; }
            set
            {
                ColumnChoices.Clear();
                selectedUnitType = value;
                switch (selectedUnitType)
                {
                    case 0:     // Boende
                        ColumnChoices = new List<string>() { "Alla", "Lägenheter", "Lägenhet 50m", "Lägenhet 40m", "Campingplatser" };
                        break;
                    case 1:     // Konferens
                        ColumnChoices = new List<string>() { "Alla", "Konferens stor", "Konferens Liten" };
                        break;
                    case 2:     // Aktiviteter
                        ColumnChoices = new List<string>() { "Alla", "Skidskolor", "Privatlektion", "Grupplektion" };
                        break;
                    case 3:     // Utrustning
                        ColumnChoices = new List<string>() { "Alla", "Alpinskidor", "Längdskidor", "Snowboard", "Skoter" };
                        break;
                }
                OnPropertyChanged();
            }
        }

        private IList<string> columnChoices;
        public IList<string> ColumnChoices
        {
            get { return columnChoices; }
            set
            {
                columnChoices = value;
                OnPropertyChanged();
            }
        }
        private int selectedColumnChoices;
        public int SelectedColumnChoices
        {
            get { return selectedColumnChoices; }
            set
            {
                selectedColumnChoices = value;
                OnPropertyChanged();
            }
        }

        private ICommand createStatisticsReport;
        public ICommand CreateStatisticsReport =>
            createStatisticsReport ??= createStatisticsReport = new RelayCommand(() =>
                {
                    
                });

        private int selectedReport;
        public int SelectedReport
        {
            get { return selectedReport; }
            set
            {
                selectedReport = value;
                OnPropertyChanged();
            }
        }

        private ICommand createReport;
        public ICommand CreateReport =>
            createReport ??= createReport = new RelayCommand(() =>
            {
                List<object> data = new List<object>();
                AccommodationController controller;
                string typ = string.Empty;
                switch (SelectedReport)
                {
                    case 0: // Boenderapport
                        controller = new AccommodationController();
                        typ = "Facilitet";
                        data = controller.FindFaciliteter("Boende").Cast<object>().ToList();
                        break;
                    case 1: // Konferensrapport
                        controller = new AccommodationController();
                        typ = "Facilitet";
                        data = controller.FindFaciliteter("Konferens").Cast<object>().ToList();
                        break;
                    case 2: // Aktivitetsrapport
                        ActivityController ac = new ActivityController();
                        typ = "Aktivitet";
                        data = ac.FindSkiSchool(null,null).Cast<object>().ToList();
                        break;
                    case 3: // Utrustningsrapport
                        EquipmentController ec = new EquipmentController();
                        typ = "Utrustning";
                        data = ec.FindUtrustning(null).Cast<object>().ToList();
                        break;
                    case 4: // Kundrapport
                        CustomerController cc = new CustomerController();
                        typ = "Kund";
                        data = cc.SearchCustomers("").Cast<object>().ToList();
                        break;
                }
                StatisticsOverviewViewModel statisticsOverviewViewModel = new StatisticsOverviewViewModel(data, typ);
                windowService.ShowDialog(statisticsOverviewViewModel);
            });
    }
}
