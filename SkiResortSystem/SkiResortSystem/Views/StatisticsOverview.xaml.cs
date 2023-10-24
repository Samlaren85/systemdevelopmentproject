using SkiResortSystem.Services;
using SkiResortSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SkiResortSystem.Views
{
    /// <summary>
    /// Interaction logic for StatisticsOverview.xaml
    /// </summary>
    public partial class StatisticsOverview : Window, ICloseable
    {
        public StatisticsOverview()
        {
            InitializeComponent();
        }
    }
}
