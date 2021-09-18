using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace AngelSix.SolidWorksApi.AddinInstaller
{
    [ValueConversion(typeof(string), typeof(Visibility))]
    public class PathToVisibilityConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null)
                return null;
            var path = (string)value;
            var parameterString = (string)parameter;

            return path.ToLower().Contains(parameterString.ToLower()) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}
