using System.Windows;
using System.Windows.Controls;

namespace SkiResortSystem.Components
{
    /// <summary>
    /// Interaction logic for BoendeModulen.xaml
    /// </summary>
    public partial class BoendeModulen : UserControl
    {
        public BoendeModulen()
        {
            InitializeComponent();
        }

        private void BoendeRadio_Checked(object sender, RoutedEventArgs e)
        {
            Column1.Header = "LGH1";
            Column1.Visibility = Visibility.Visible;
            Column2.Header = "LGH2";
            Column2.Visibility = Visibility.Visible;
            Column3.Header = "CAMP";
            Column3.Visibility = Visibility.Visible;
            Column4.Header = "KONF stor";
            Column4.Visibility = Visibility.Visible;
            Column5.Header = "KONF liten";
            Column5.Visibility = Visibility.Visible;
        }

        private void UtrustningRadio_Checked(object sender, RoutedEventArgs e)
        {
            Column1.Header = "ALPINSKIDOR";
            Column1.Visibility = Visibility.Visible;
            Column2.Header = "LÄNGDSKIDOR";
            Column2.Visibility = Visibility.Visible;
            Column3.Header = "SNOWBOARD";
            Column3.Visibility = Visibility.Visible;
            Column4.Header = "SKOTER";
            Column4.Visibility = Visibility.Visible;
            Column5.Header = "";
            Column5.Visibility = Visibility.Collapsed;
        }

        private void AktivitetRadio_Checked(object sender, RoutedEventArgs e)
        {
            Column1.Header = "PRIV";
            Column1.Visibility = Visibility.Visible;
            Column2.Header = "GRUPP GRÖN";
            Column2.Visibility = Visibility.Visible;
            Column3.Header = "GRUPP BLÅ";
            Column3.Visibility = Visibility.Visible;
            Column4.Header = "GRUPP RÖD";
            Column4.Visibility = Visibility.Visible;
            Column5.Header = "GRUPP SVART";
            Column5.Visibility = Visibility.Visible;
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
    }
