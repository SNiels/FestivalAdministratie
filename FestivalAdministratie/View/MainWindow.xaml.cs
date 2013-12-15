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
using FestivalAdministratie.View;

namespace FestivalAdministratie.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        static MainWindow()
        {
            Window = new MainWindow();
        }

        private static Window _getWindow;

        public static Window Window
        {
            get{return _getWindow;}
            set { _getWindow = value; }
        }

        

        //private void LineUpControl_Loaded(object sender, RoutedEventArgs e)
        //{

        //}
    }
}
