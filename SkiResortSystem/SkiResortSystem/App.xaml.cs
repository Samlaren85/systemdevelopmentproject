﻿using SkiResortSystem.Services;
using SkiResortSystem.ViewModels;
using SkiResortSystem.Views;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace SkiResortSystem
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            Startup += (s, e) => //ta bort denna sen
            {
                WindowService.RegisterWindow<LoginViewModel, LoginWindow>();
                WindowService.RegisterWindow<CustomerOverviewCompanyViewModel, CustomerOverviewCompany> ();
                WindowService.RegisterWindow<CustomerOverviewPrivateViewModel, CustomerOverviewPrivate>();
                WindowService.RegisterWindow<BookingOverviewViewModel, BookingOverview>();
                WindowService.RegisterWindow<ActivityOverviewViewModel, ActivityOverview>();
                WindowService.RegisterWindow<EquipmentOverviewViewModel, EquipmentOverview>();
            };
        }
    }
}
