using DTSaveManager.DataTypes.Enums;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace DTSaveManager.Converters
{
    [ValueConversion(typeof(bool), typeof(Color))]
    class BooleanToColorTextBoxMultiValueConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(values[0] is bool)) return null;
            bool temp = (bool)(values[0] as bool?);

            if (!temp && ((ThemeType)values[1] == ThemeType.DarkTheme || (ThemeType)values[1] == ThemeType.LightTheme)) return Application.Current.TryFindResource("DTAlertBrush");
            else return Application.Current.TryFindResource("DTBlack");
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
