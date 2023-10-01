using SkiResortSystem.Commands;
using SkiResortSystem.Models;
using SkiResortSystem.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SkiResortSystem.ViewModels
{
    public class CustomerOverviewCompanyViewModel: ObservableObject
    {
        public class KundöversiktPrivatViewModel : ObservableObject
        {
            private ICommand closeCommand = null!;
            public ICommand CloseCommand =>
                closeCommand ??= closeCommand = new RelayCommand<ICloseable>((closeable) =>
                {
                    closeable.Close();
                });
        }
    }
}
