using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace DTSaveManager.Converters
{
    [ValueConversion(typeof(bool), typeof(FontWeights))]
    class BooleanToFontWeightValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value) return FontWeights.SemiBold;
            else return FontWeights.Normal;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
