using System;
using System.Globalization;
using System.Windows.Data;

namespace TimeCalculator.Converter
{
    public sealed class DoubleMillisecoundToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return TimeSpan.FromMilliseconds(System.Convert.ToDouble(value)).ToString("hh\\:mm");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}