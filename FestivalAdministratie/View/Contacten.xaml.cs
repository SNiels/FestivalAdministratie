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

namespace FestivalAdministratie.View
{
    /// <summary>
    /// Interaction logic for Contacten.xaml
    /// </summary>
    public partial class Contacten : UserControl
    {
        public Contacten()
        {
            InitializeComponent();
        }

        private void txbZoekContact_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                CollectionViewSource.GetDefaultView(ContactDataGrid.ItemsSource).Refresh();
            }
            catch (Exception) { }
        }

        private void txbZoekContact_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.Key==Key.Enter)
            CollectionViewSource.GetDefaultView(ContactDataGrid.ItemsSource).Refresh();
        }
    }
}
