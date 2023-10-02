using BusinessLayer;
using EntityLayer;
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
    public class CustomerOverviewCompanyViewModel: ObservableObject
    {
        private CustomerController customerController;
        private Kund nyKund;
        public Kund NyKund
        {
            get { return nyKund; }
            set { nyKund = value; OnPropertyChanged(); }
        }
        private Företagskund nyFöretagsKund;
        public Företagskund NyFöretagsKund
        {
            get { return nyFöretagsKund; }
            set { nyFöretagsKund = value; OnPropertyChanged(); }
        }
        public CustomerOverviewCompanyViewModel()
        {
            customerController = new CustomerController();
            NyFöretagsKund = new Företagskund();
            Kund Kund = new Kund() { Företagskund = NyFöretagsKund };
        }
        public CustomerOverviewCompanyViewModel(Kund laddadKund)
        {
            customerController = new CustomerController();
            NyFöretagsKund = laddadKund.Företagskund;
            NyKund = laddadKund;
        }
        private ICommand saveCustomer = null!;
        public ICommand SaveCustomer =>
            saveCustomer ??= saveCustomer = new RelayCommand(() =>
            {
                try
                {
                    customerController.AddCompanyCustomer(NyKund);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            });
    }
}
