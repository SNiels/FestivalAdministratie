using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace FestivalAdministratie.Converter
{
    class SimpleXPositionConverter:IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values == null)
                throw new ArgumentNullException("values");
            if (values.Length < 2)
                throw new ArgumentException("values should have at least 2 objects in it.");

            try
            {
                var percent = (double)values[0];
                var width = (double)values[1];

                return percent * width;
            }
            catch (InvalidCastException)
            {
                return DependencyProperty.UnsetValue;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
