using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace FestivalAdministratie.Converter
{
    public class TextToColorConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string color = value as string;
            if (color==null||color.Length != 9) return null;
            return Color.FromArgb(
            System.Convert.ToByte(color.Substring(1, 2), 16),
            System.Convert.ToByte(color.Substring(3, 2), 16),
            System.Convert.ToByte(color.Substring(5, 2), 16),
            System.Convert.ToByte(color.Substring(7, 2), 16));
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null) return null;
            return ((Color)value).ToString();
        }
    }

    public class ColorToBrushConverter:IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string color = value as string;
            if (color == null || color.Length != 9) return null;
            return new SolidColorBrush( Color.FromArgb(
            System.Convert.ToByte(color.Substring(1, 2), 16),
            System.Convert.ToByte(color.Substring(3, 2), 16),
            System.Convert.ToByte(color.Substring(5, 2), 16),
            System.Convert.ToByte(color.Substring(7, 2), 16)));
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
