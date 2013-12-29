using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using FestivalLibAdmin.Model;

namespace FestivalAdministratie.Converter
{
    class GroupNameConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            switch (parameter as string)
            {
                case "TicketType":
                    return ((TicketType)value).Name;
                case "ContactpersonType":
                    return ((ContactpersonType)value).Name;
                default:
                    return string.Empty;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
