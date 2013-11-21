using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using FestivalAdministratie.Model;
using FestivalAdministratie.ViewModel;

namespace FestivalAdministratie.View
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class LineUpControl : UserControl
    {
        public LineUpControl()
        {
            InitializeComponent();
            DataContextChanged += LineUpControl_DataContextChanged;
            Festival.SingleFestival.LineUps.CollectionChanged += LineUps_CollectionChanged;
            foreach(Model.LineUp lineUp in Festival.SingleFestival.LineUps )
                lineUp.PropertyChanged += lineup_PropertyChanged;
        }

        void LineUps_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            ObservableCollection<Model.LineUp> lineups = (ObservableCollection<Model.LineUp>)sender;
            if (lineups == null) return;
            if(e.Action==System.Collections.Specialized.NotifyCollectionChangedAction.Reset)
                foreach(Model.LineUp lineup in lineups)
                    lineup.PropertyChanged += lineup_PropertyChanged;
            else if(e.Action==System.Collections.Specialized.NotifyCollectionChangedAction.Add)
                foreach(Model.LineUp lineup in e.NewItems)
                    lineup.PropertyChanged += lineup_PropertyChanged;
        }

        void lineup_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Hours") CreateTimeGrid();
        }

        void LineUpControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this.DataContext == null) return;
            CreateTimeGrid();
            //foreach(Stage stage in ((FestivalAdministratie.Model.LineUp)this.DataContext).Stages)
                //foreach(Optreden optreden in stage.Performances)
                //    optreden.PropertyChanged += optreden_PropertyChanged;
            //((FestivalAdministratie.Model.LineUp)this.DataContext).PropertyChanged += LineUpControl_PropertyChanged;
        }

        //void LineUpControl_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        //{
        //    if (e.PropertyName == "Hours") CreateTimeGrid();
        //}

        //void optreden_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        //{
        //    CreateTimeGrid();
        //}

        void HourHeader_Loaded(object sender, RoutedEventArgs e)
        {
            CreateTimeGrid();
        }

        private void CreateTimeGrid()
        {
            if (this.DataContext == null) return;
            Grid grid = HourHeader;
            grid.ColumnDefinitions.Clear();
            grid.RowDefinitions.Clear();
            grid.Children.Clear();
            
            ObservableCollection<DateTime> hours = ((FestivalAdministratie.Model.LineUp)this.DataContext).Hours;
            for (int i = 0; i < hours.Count - 1; i++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition());
                TextBlock block = new TextBlock();
                block.Text = hours[i].ToString("H:mm");
                block.SetValue(Grid.ColumnProperty, i);
                grid.Children.Add(block);
            }
            grid.MinWidth = 120 * grid.Children.Count;
        }   
    }
}
