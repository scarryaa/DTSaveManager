using DTSaveManager.DataTypes.Enums;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace DTSaveManager.Converters
{
    [ValueConversion(typeof(MessageType), typeof(Color))]
    public class MessageTypeToColorValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((MessageType)value == MessageType.DeletionMessage) return Application.Current.TryFindResource("DTAlertBrush");
            else if ((MessageType)value == MessageType.DeletionMessage) return Application.Current.TryFindResource("DTInteractiveBrush");
            return Application.Current.TryFindResource("DTAlertBrush");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
