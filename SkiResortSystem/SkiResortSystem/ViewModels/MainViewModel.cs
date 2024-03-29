﻿using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using BusinessLayer;
using EntityLayer;
using SkiResortSystem.Commands;
using SkiResortSystem.Models;
using SkiResortSystem.Services;

namespace SkiResortSystem.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        WindowService windowService = new WindowService();

        private string loggedInUser = "#NA";
        public string LoggedInUser
        {
            get { return loggedInUser; }
            set { loggedInUser = value; Behörighetskontroll.Execute(true); OnPropertyChanged(); }
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
                if (value.Contains("Logga ut", StringComparison.OrdinalIgnoreCase))
                {
                    
                    LogOut.Execute(true);
                }
                else if (value.Contains("Avsluta", StringComparison.OrdinalIgnoreCase)) ExitCommand.Execute(true);
            }
        }

        /// <summary>
        /// Döljer Main View och öppnar upp löserordsrutan!
        /// </summary>
        public MainViewModel()
        {
            LogIn();
            List <int> years = new List<int>();
            for (int i = 2023; i <= DateTime.Now.Year; i++) 
            {
                years.Add(i);
            }
            StatisticYear = years;
        }

        public void LogIn()
        {
            MainVisibility = Visibility.Hidden;
            LoginViewModel logIn = new LoginViewModel();
            windowService.ShowDialog(logIn);
            if (SessionController.LoggedIn != null)
            {

                LoggedInUser = $"{SessionController.LoggedIn.AnvändarNamn}";
                MainVisibility = Visibility.Visible;

            }
            else ExitCommand.Execute(null);
        }

        /// <summary>
        /// Används för att populera datagriden/tabellen under visa beläggning. Sker genom en transponering av den inre listan.(se metoden Överförd)
        /// </summary>
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
        /// <summary>
        /// Transponerar den inre listan som används för att visa beläggning i boendemodulen/visa beläggning.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
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
                EquipmentController ec = new EquipmentController();
                VisaBeläggning = ec.VisaBeläggningen(BeläggningAnkomsttid, BeläggningDatumperiod);
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
        /// <summary>
        /// Behörighetskontroll - Hanterar vilka flikar/fönster och begränsningar som ska gälla i systemet för den inloggade användaren.
        /// </summary>
        private ICommand behörighetskontroll = null!;
        public ICommand Behörighetskontroll => behörighetskontroll ??= behörighetskontroll = new RelayCommand(() =>
        {

            foreach (Behörighet bh in SessionController.LoggedIn.RollID.BehörighetID)
            {
                switch (bh.Behörighetstyp)
                {
                    case "1 - Systemadministratör": 
                        Admin = Visibility.Visible;
                        Economy = Visibility.Collapsed;
                        MarketingManager = Visibility.Collapsed;
                        Customer = Visibility.Collapsed;
                        Governance = Visibility.Collapsed;
                        CEO = Visibility.Collapsed;
                        Reception = Visibility.Collapsed;
                        Shop = Visibility.Collapsed;
                        IsCurrentUserMarketingManager = false;
                        break;
                    case "2 - Reception":
                        Admin = Visibility.Collapsed;
                        Economy = Visibility.Collapsed;
                        MarketingManager = Visibility.Collapsed;
                        Customer = Visibility.Visible;
                        Governance = Visibility.Collapsed;
                        CEO = Visibility.Collapsed;
                        Shop = Visibility.Visible;
                        Reception = Visibility.Visible;
                        IsCurrentUserMarketingManager = false;
                        break;
                    case "3 - Skidshop":
                        Admin = Visibility.Collapsed;
                        Economy = Visibility.Collapsed;
                        MarketingManager = Visibility.Collapsed;
                        Customer = Visibility.Visible;
                        Governance = Visibility.Collapsed;
                        CEO = Visibility.Collapsed;
                        Shop = Visibility.Visible;
                        Reception = Visibility.Collapsed;
                        IsCurrentUserMarketingManager = false;
                        break;
                    case "4 - Ekonomi":
                        Admin = Visibility.Collapsed;
                        Economy = Visibility.Visible;
                        MarketingManager = Visibility.Collapsed;
                        Customer = Visibility.Collapsed;
                        Governance = Visibility.Collapsed;
                        CEO = Visibility.Collapsed;
                        Shop = Visibility.Collapsed;
                        Reception = Visibility.Collapsed;
                        IsCurrentUserMarketingManager = false;
                        break;
                    case "5 - Avdelningschef skidshop":
                        Admin = Visibility.Collapsed;
                        Economy = Visibility.Collapsed;
                        MarketingManager = Visibility.Collapsed;
                        Customer = Visibility.Visible;
                        Governance = Visibility.Visible;
                        CEO = Visibility.Collapsed;
                        Reception = Visibility.Collapsed;
                        Shop = Visibility.Visible;
                        IsCurrentUserMarketingManager = false;
                        break;
                    case "6 - Ekonomichef":
                        Admin = Visibility.Collapsed;
                        Economy = Visibility.Visible;
                        MarketingManager = Visibility.Collapsed;
                        Customer = Visibility.Collapsed;
                        Governance = Visibility.Visible;
                        CEO = Visibility.Collapsed;
                        Reception = Visibility.Collapsed;
                        Shop = Visibility.Collapsed;
                        IsCurrentUserMarketingManager = false;
                        break;
                    case "7 - Marknadschef":
                        Admin = Visibility.Collapsed;
                        //UseShopOrReception = true;
                        Economy = Visibility.Collapsed;
                        MarketingManager = Visibility.Visible;
                        Customer = Visibility.Visible;
                        Governance = Visibility.Visible;
                        CEO = Visibility.Collapsed;
                        Reception = Visibility.Visible;
                        Shop = Visibility.Visible;
                        IsCurrentUserMarketingManager = true;
                        break;
                    case "8 - Verkställande Direktör":
                        Admin = Visibility.Collapsed;
                        //UseShopOrReception = true;
                        Economy = Visibility.Collapsed;
                        MarketingManager = Visibility.Collapsed;
                        Customer = Visibility.Collapsed;
                        Governance = Visibility.Visible;
                        CEO = Visibility.Visible;
                        Reception = Visibility.Collapsed;
                        Shop = Visibility.Collapsed;
                        IsCurrentUserMarketingManager = false;
                        break;
                }
            }

        }
        );

        private Visibility admin = Visibility.Collapsed;
        public Visibility Admin
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
        private Visibility customer = Visibility.Collapsed;
        public Visibility Customer
        {
            get { return customer; }
            set
            {
                if (customer != value)
                {
                    customer = value;
                    OnPropertyChanged(nameof(Customer));
                }
            }
        }
        
        private Visibility reception = Visibility.Collapsed;
        public Visibility Reception
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

        private Visibility shop = Visibility.Collapsed;
        public Visibility Shop
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

        private Visibility economy = Visibility.Collapsed;
        public Visibility Economy
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

        
        private Visibility marketingManager = Visibility.Collapsed;
        public Visibility MarketingManager
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

        private Visibility governance = Visibility.Collapsed;
        public Visibility Governance
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


        private Visibility ceo = Visibility.Collapsed;
        public Visibility CEO
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
        /// <summary>
        /// Används för att kontrollera om inloggad användare är marknadschef med behörighet att lägga till företagskunder m.m.
        /// </summary>
        private bool _isCurrentUserMarketingManager;
        public bool IsCurrentUserMarketingManager
        {
            get { return _isCurrentUserMarketingManager; }
            set
            {
                if (_isCurrentUserMarketingManager != value)
                {
                    _isCurrentUserMarketingManager = value;
                    OnPropertyChanged(nameof(IsCurrentUserMarketingManager));
                }
            }
        }

    }
}
