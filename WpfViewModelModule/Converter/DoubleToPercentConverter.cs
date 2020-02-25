using System;
using System.Windows.Data;
using System.Globalization;

namespace WpfViewModelModule.Converter
{
    public class DoubleToPercentConverter 
        : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (double)value * 100.0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (double)value / 100.0;
        }
    }
}
