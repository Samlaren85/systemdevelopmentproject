using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using BusinessLayer;
using SkiResortSystem.Commands;
using SkiResortSystem.Models;
using SkiResortSystem.Services;

namespace SkiResortSystem.ViewModels
{
    public class MainViewModel : ObservableObject
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

        public MainViewModel()
        {
            MainVisibility = Visibility.Hidden;
            LogIn();
        }

        public void LogIn()
        {
            LoginViewModel logIn = new LoginViewModel();
            windowService.ShowDialog(logIn);
            if (SessionController.LoggedIn != null)
            {
                LoggedInUser = $"{SessionController.LoggedIn.UserID}";
                MainVisibility = Visibility.Visible;
            }
            else ExitCommand.Execute(null);
        }

        private ICommand exitCommand = null;
        public ICommand ExitCommand =>
            exitCommand ??= exitCommand = new RelayCommand<ICloseable>((closeable) =>
                    {
                        Application.Current.Shutdown();
                    });
    }
}
