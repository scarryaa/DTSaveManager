﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace DTSaveManager.Converters
{
    [ValueConversion(typeof(bool), typeof(Visibility))]
    class BooleanToVisibilityValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value) return Visibility.Visible;
            else return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
