using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace TimeCalculator.Converter
{
    public class BoolToBrushConverter : IValueConverter
    {
        public Brush FalseBrush { get; set; }

        public Brush TrueBrush { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.GetType() != typeof(bool)) return FalseBrush;

            return (bool) value ? TrueBrush : FalseBrush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (Equals(value as Brush, TrueBrush))
            {
                return true;
            }

            return false;
        }
    }
}