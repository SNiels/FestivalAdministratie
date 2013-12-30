using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace DemoApp.Common
{
    class DagConverter:IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return ((DateTime)value).BeDayOfWeek().ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
