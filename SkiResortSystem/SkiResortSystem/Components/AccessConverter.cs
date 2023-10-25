using System;
using System.Globalization;
using System.Windows.Data;

namespace SkiResortSystem.Components
{
    public class AccessConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue && boolValue)
            {
                return false;
            }
            else
            {
                return true;
            }

        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        
    }
}
