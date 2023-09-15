using SkiResortSystem.Services;
using SkiResortSystem.ViewModels;
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
            };
        } 
    }
}
