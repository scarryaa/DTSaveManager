using DTSaveManager.Models;
using DTSaveManager.Services;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace DTSaveManager.Converters
{
    [ValueConversion(typeof(bool), typeof(ObservableCollection<SaveMetadata>))]
    class BooleanToItemsSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return SaveMetadataService.Instance.GetSaveMetadata((bool)parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
