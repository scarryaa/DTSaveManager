using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace DTSaveManager.Converters
{
    [ValueConversion(typeof(object[]), typeof(object[]))]
    public class IsActiveToIsEnabledMultiValueConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(values[0] is bool)) return null;
            if (!(values[1] is bool)) return null;
            bool temp = (bool)(values[0] as bool?);
            bool temp2 = (bool)(values[1] as bool?);

            if (temp == true || temp2 == true) return false;
            else return true;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
