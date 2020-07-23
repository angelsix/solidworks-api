using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;

namespace AngelSix.SolidWorksApi.AddinInstaller
{
    public class PathToFileNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            value != null && value is string path 
                ? Path.GetFileName(path) 
                : null;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}
