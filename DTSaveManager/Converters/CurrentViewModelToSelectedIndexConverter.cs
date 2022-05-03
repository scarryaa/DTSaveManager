using DTSaveManager.DataTypes.Enums;
using DTSaveManager.Models;
using DTSaveManager.Services;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace DTSaveManager.Converters
{
    [ValueConversion(typeof(ViewModelType), typeof(int))]
    class CurrentViewModelToSelectedIndexConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (int)((ViewModelType)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (ViewModelType)((int)value);
        }
    }
}
