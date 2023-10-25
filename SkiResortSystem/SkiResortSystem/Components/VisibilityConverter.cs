using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows;

namespace SkiResortSystem.Components
{
   public class BooleanToVisibilityConverter : IValueConverter
   {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue && boolValue)
            {
                return Visibility.Visible;
            }
            else
            {
                return Visibility.Collapsed;
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }
   }
}
