using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using FestivalLibAdmin.Model;

namespace FestivalAdministratie.Converter
{
    class LineUpOptredensConverter:IMultiValueConverter
    {
        //public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        //{
            
        //}

        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            ObservableCollection<Optreden> optredens = (ObservableCollection<Optreden>)values[0];
            LineUp up = (LineUp)values[1];
            return optredens.Where(optreden => optreden.LineUp == up);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
