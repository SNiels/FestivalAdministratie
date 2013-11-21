using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TimeLine
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class LineUp : UserControl
    {
        public LineUp()
        {
            Loaded += LineUp_Loaded;
            //HourHeader.Loaded += HourHeader_Loaded;
        }

        void LineUp_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        //void HourHeader_Loaded(object sender, RoutedEventArgs e)
        //{
        //    Grid grid = sender as Grid;
        //    List<Hour> hours = (List<Hour>)grid.DataContext;
        //    for (int i = 0; i < hours.Count - 1; i++)
        //    {
        //        grid.ColumnDefinitions.Add(new ColumnDefinition());
        //        TextBlock block = new TextBlock();
        //        block.Text = "" + hours[i].Uur;
        //        block.SetValue(Grid.ColumnProperty, i);
        //        grid.Children.Add(block);
        //    }
        //    grid.MinWidth = 120 * grid.Children.Count;
        //    ScrollViewer view = StagesPanelScroller;
        //}
    }
}
