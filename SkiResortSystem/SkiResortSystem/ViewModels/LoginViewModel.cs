using Businesslayer;
using SkiResortSystem.Commands;
using SkiResortSystem.Models;
using SkiResortSystem.Services;
using System;
using System.Windows;
using System.Windows.Input;

namespace SkiResortSystem.ViewModels
{
    class LoginViewModel : ObservableObject
    {
        private string username;
        public string Username
        {
            get { return username; }
            set { username = value; OnPropertyChanged(); }
        }
        private string password;
        public string Password
        {
            get { return password; }
            set { password = value; OnPropertyChanged(); }
        }

        private ICommand logInCommand = null;
        public ICommand LogInCommand
        {
            get
            {
                if (logInCommand == null)
                {
                    logInCommand = new RelayCommand<ICloseable>((view) =>
                    {
                        try
                        {
                            SessionController.Instance(Username, Password);
                        }
                        catch (Exception e)
                        {
                            MessageBox.Show(e.Message);
                            Username = string.Empty;
                            Password = string.Empty;
                        }
                        if (SessionController.LoggedIn != null) CloseCommand.Execute(view);
                    });
                }
                return logInCommand;
            }
        }

        private ICommand closeCommand = null;
        public ICommand CloseCommand 
        {
            get
            {
                if (closeCommand == null)
                { 
                    closeCommand = new RelayCommand<ICloseable>((closeable) =>
                    {
                        closeable.Close();
                    });
                }
                return closeCommand;
            }
        }
    }
}
