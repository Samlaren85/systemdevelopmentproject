using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using BusinessLayer;
using DataLayer;
using EntityLayer;
using Microsoft.EntityFrameworkCore.Diagnostics;
using SkiResortSystem.Commands;
using SkiResortSystem.Models;
using SkiResortSystem.Services;
using SkiResortSystem.Views;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SkiResortSystem.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        WindowService windowService = new WindowService();

        private string loggedInUser = "#NA";
        public string LoggedInUser
        {
            get { return loggedInUser; }
            set { loggedInUser = value; OnPropertyChanged(); }
        }

        private Visibility mainVisibility = Visibility.Visible;
        public Visibility MainVisibility
        {
            get { return mainVisibility; }
            set { mainVisibility = value; OnPropertyChanged(); }
        }

        private string menuChoise;
        public string MenuChoise
        {
            get { return menuChoise; }
            set
            {
                if (value.Contains("Logga ut")) LogOut.Execute(true);
                else if (value.Contains("Avsluta")) ExitCommand.Execute(true);
            }
        }

        /// <summary>
        /// Döljer Main View och öppnar upp löserordsrutan!
        /// </summary>
        public MainViewModel()
        {
            LogIn();
        }
       
        public void LogIn()
        {
            MainVisibility = Visibility.Hidden;
            LoginViewModel logIn = new LoginViewModel();
            windowService.ShowDialog(logIn);
            if (SessionController.LoggedIn != null)
            {
                
                LoggedInUser = $"{SessionController.LoggedIn.UserID}";
                MainVisibility = Visibility.Visible;
            }
            else ExitCommand.Execute(null);
        }

        private IList<List<string>> visaBeläggning;
        public IList<List<string>> VisaBeläggning
        {
            get { return visaBeläggning; }
            set
            {
                if (value != visaBeläggning)
                {
                    visaBeläggning = value;
                    // Överför listan för att skapa rader i tabellen av de inre listorna.
                    ÖverfördVisaBeläggning = Överförd(visaBeläggning);
                    OnPropertyChanged(nameof(VisaBeläggning));
                    OnPropertyChanged(nameof(ÖverfördVisaBeläggning));
                }
                visaBeläggning = value; OnPropertyChanged();
            }
        }

        private IList<List<string>> överfördVisaBeläggning;

        public IList<List<string>> ÖverfördVisaBeläggning
        {
            get { return överfördVisaBeläggning; }
            set
            {
                if (value != överfördVisaBeläggning)
                {
                    överfördVisaBeläggning = value;
                    OnPropertyChanged(nameof(ÖverfördVisaBeläggning));
                }
            }
        }
        private IList<List<string>> Överförd(IList<List<string>> source)
        {
            if (source == null || source.Count == 0)
                return source;

            int rowCount = source.Count;
            int colCount = source[0].Count;

            IList<List<string>> result = new List<List<string>>();

            for (int col = 0; col < colCount; col++)
            {
                List<string> newRow = new List<string>();
                for (int row = 0; row < rowCount; row++)
                {
                    newRow.Add(source[row][col]);
                }
                result.Add(newRow);
            }

            return result;
        }
        private DateTime beläggningankomsttid = DateTime.Today;
        public DateTime BeläggningAnkomsttid
        {
            get { return beläggningankomsttid; }
            set
            {
                beläggningankomsttid = value;
                BeläggningDatumperiod = beläggningankomsttid.AddDays(7);
                VisaBeläggningen.Execute(true);
                OnPropertyChanged();
            }
        }

        private DateTime beläggningdatumperiod = DateTime.Today.AddDays(7);
        public DateTime BeläggningDatumperiod
        {
            get { return beläggningdatumperiod; }
            set
            {
                beläggningdatumperiod = value;
                OnPropertyChanged();
            }
        }
        
        private bool boendekonferensbeläggningradiobutton;
        public bool BoendeKonferensbeläggningradiobutton
        {
            get { return boendekonferensbeläggningradiobutton; }
            set { boendekonferensbeläggningradiobutton = value; VisaBeläggningen.Execute(true); OnPropertyChanged(); }
        }

        private bool utrustningbeläggningradiobutton;
        public bool UtrustningBeläggningradiobutton
        {
            get { return utrustningbeläggningradiobutton; }
            set { utrustningbeläggningradiobutton = value; VisaBeläggningen.Execute(true); OnPropertyChanged(); }
        }

        private bool aktivitetbeläggningradiobutton;
        public bool Aktivitetbeläggningradiobutton
        {
            get { return aktivitetbeläggningradiobutton; }
            set { aktivitetbeläggningradiobutton = value; VisaBeläggningen.Execute(true); OnPropertyChanged(); }
        }

        private ICommand visaBeläggningen = null!;
        public ICommand VisaBeläggningen => visaBeläggningen ??= visaBeläggningen = new RelayCommand(() =>
        {
            
            if (BoendeKonferensbeläggningradiobutton)
            {
                AccommodationController ac = new AccommodationController();
                VisaBeläggning = ac.VisaBeläggningen(BeläggningAnkomsttid, BeläggningDatumperiod);
            }


            else if (UtrustningBeläggningradiobutton)
            {
                //EquipmentController ec = new EquipmentController();
                //VisaBeläggning = ec.VisaBeläggningen(BeläggningAnkomsttid, BeläggningDatumperiod);
            }

            else if (Aktivitetbeläggningradiobutton)
            {
                ActivityController ac = new ActivityController();
                VisaBeläggning = ac.VisaBeläggningen(BeläggningAnkomsttid, BeläggningDatumperiod);
            }
        });

        private bool öppnaDropDown;
        public bool ÖppnaDropDown
        {
            get { return öppnaDropDown; }
            set
            {
                öppnaDropDown = value;
                OnPropertyChanged();
            }
        }

       
        /// <summary>
        /// Logga ut och stänga ner nedan 
        /// </summary>
        private ICommand logOut = null!;
        
        public ICommand LogOut =>
            logOut ??= logOut = new RelayCommand(() =>
                {
                    SessionController.Terminate();
                    LogIn();
                });   

        private ICommand exitCommand = null!;
        public ICommand ExitCommand =>
            exitCommand ??= exitCommand = new RelayCommand(() =>
                {
                    Application.Current.Shutdown();
                });

        private bool admin = false;
        public bool Admin
        {
            get { return admin; }
            set
            {
                if (admin != value)
                {
                    admin = value;
                    OnPropertyChanged(nameof(Admin));
                }
            }
        }

        private bool reception = false;
        public bool Reception
        {
            get { return reception; }
            set
            {
                if (reception != value)
                {
                    reception = value;
                    OnPropertyChanged(nameof(Reception));
                }
            }
        }

        private bool shop = false;
        public bool Shop
        {
            get { return shop; }
            set
            {
                if (shop != value)
                {
                    shop = value;
                    OnPropertyChanged(nameof(Shop));
                }
            }
        }

        private bool economy = false;
        public bool Economy
        {
            get { return economy; }
            set
            {
                if (economy != value)
                {
                    economy = value;
                    OnPropertyChanged(nameof(Economy));
                }
            }
        }

        private bool marketingManager = false;
        public bool MarketingManager
        {
            get { return marketingManager; }
            set
            {
                if (marketingManager != value)
                {
                    marketingManager = value;
                    OnPropertyChanged(nameof(MarketingManager));
                }
            }
        }

        private bool governance  = false;
        public bool Governance 
        {
            get { return governance; }
            set
            {
                if (governance != value)
                {
                    governance = value;
                    OnPropertyChanged(nameof(Governance));
                }
            }
        }


        private bool ceo = false;
        public bool CEO
        {
            get { return ceo; }
            set
            {
                if (ceo != value)
                {
                    ceo = value;
                    OnPropertyChanged(nameof(CEO));
                }
            }
        }
    }
}
