using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Data;

namespace Hackathon.WP7.MultiLib.Converters
{
    public class IsEmptyOrNullConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (targetType == typeof(Visibility))
            {
                if (string.IsNullOrEmpty(value.ToString()))
                {
                    return (parameter == null || parameter.ToString() != "i" ? Visibility.Visible : Visibility.Collapsed);
                }
                else
                    return (parameter == null || parameter.ToString() != "i" ? Visibility.Collapsed : Visibility.Visible);
            }
            else
            {
                if (parameter == null || parameter.ToString() != "i")
                {
                    return string.IsNullOrEmpty(value.ToString());
                }
                else
                {
                    return !string.IsNullOrEmpty(value.ToString());
                }
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }
}
