using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using FestivalAdministratie.Model;
using FestivalAdministratie.View;
using GalaSoft.MvvmLight.Command;

namespace FestivalAdministratie.ViewModel
{
    public class LineUpViewModel:ObservableObject,IPage
    {
        public LineUpViewModel()
        {
            #region
            //LineUps = Model.Festival.SingleFestival.LineUps;
            //Stages = Stage.Stages;
            //Bands = Band.Bands;
            //Optredens = Optreden.Optredens;
            //dummy data
            //Festival.StartDate = DateTime.Today.AddDays(-3);
            //Festival.EndDate = DateTime.Today.AddDays(3);
            #endregion
            if (LineUps.Count()>0)
            SelectedLineUp = LineUps[0];
            //Festival.SingleFestival.PropertyChanged += SingleFestival_PropertyChanged;
        }

        //void SingleFestival_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        //{
        //    if (e.PropertyName == "LineUps") LineUps = ((Festival)sender).LineUps;
        //}

        private LineUpBeheer _beheerWindow= new LineUpBeheer();

        private LineUpBeheer BeheerWindow
        {
            get { return _beheerWindow; }
            set { _beheerWindow = value; }
        }

        private OnFirstStartScreen _firstScreen = new OnFirstStartScreen();

        private OnFirstStartScreen OnFirstStartWindow
        {
            get { return _firstScreen; }
            set { _firstScreen = value; }
        }

        //private ObservableCollection<Band>  _bands;

        //public ObservableCollection<Band>  Bands
        //{
        //    get { return _bands; }
        //    set { _bands = value;
        //    OnPropertyChanged("Bands");
        //    }
        //}

        //private ObservableCollection<Optreden> _optredens;

        //public ObservableCollection<Optreden> Optredens
        //{
        //    get { return _optredens; }
        //    set
        //    {
        //        _optredens = value;
        //        OnPropertyChanged("Optredens");
        //    }
        //}

        //private ObservableCollection<Stage> _stages;

        //public ObservableCollection<Stage> Stages
        //{
        //    get { return _stages; }
        //    set
        //    {
        //        _stages = value;
        //        OnPropertyChanged("Stages");
        //    }
        //}

        //private ObservableCollection<FestivalAdministratie.Model.LineUp> _lineUps;

        public ObservableCollection<FestivalAdministratie.Model.LineUp> LineUps
        {
            get { 
                //return _lineUps;
                return Model.Festival.SingleFestival.LineUps;
            }
            set {
                Model.Festival.SingleFestival.LineUps = value;
                if (value != null && value.Count() > 0)
                    SelectedLineUp = value.First();
                else SelectedLineUp = null;
                //_lineUps = value;
            OnPropertyChanged("LineUps");
            //if (value != null) value.CollectionChanged += LineUps_CollectionChanged;
            }
        }

        //void LineUps_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        //{
        //    ObservableCollection<FestivalAdministratie.Model.LineUp> lineUps = ((ObservableCollection<FestivalAdministratie.Model.LineUp>)sender);
        //    if (lineUps.Count > 0)
        //        /*if (SelectedLineUp != null)
        //        {
        //            foreach (FestivalAdministratie.Model.LineUp lineUp in lineUps)
        //                if (lineUp.Dag == SelectedLineUp.Dag)
        //                {
        //                    SelectedLineUp = lineUp;
        //                    break;
        //                }
        //        }
        //        else */SelectedLineUp = lineUps.First();
        //}

        private FestivalAdministratie.Model.LineUp _selectedLineUp;

        public FestivalAdministratie.Model.LineUp SelectedLineUp
        {
            get { return _selectedLineUp; }
            set { _selectedLineUp = value;
            OnPropertyChanged("SelectedLineUp");
            //OnPropertyChanged("Hours");
            //OnPropertyChanged("Stages");
            }
        }

        public  void OnPropertyChangedSelectedLineUp()
        {
            OnPropertyChanged("SelectedLineUp");
        }

        //private ObservableCollection<DateTime> _hours;

        public ObservableCollection<DateTime> Hours
        {
            get { return SelectedLineUp.Hours; }
        }
        
        


        public string Name
        {
            get { return "Line-up"; }
        }

        public ICommand OpenLineUpBeheerCommand
        {
            get
            {
                return new RelayCommand(OpenLineUpBeheer, IsLineUpBeheerNotVisible);
            }
        }

        private bool IsLineUpBeheerNotVisible()
        {
            return !BeheerWindow.IsVisible;
        }

        private void OpenLineUpBeheer()
        {
            //beheerWindow.ShowDialog();
            BeheerWindow = new LineUpBeheer();
            BeheerWindow.Show();
        }

        public ICommand OpenOnFirstStartScreenCommand
        {
            get
            {
                return new RelayCommand(OpenOnFirstStartScreen, IsFirstStartScreenNotVisible);
            }
        }

        private bool IsFirstStartScreenNotVisible()
        {
            return !OnFirstStartWindow.IsVisible;
        }

        private void OpenOnFirstStartScreen()
        {
            OnFirstStartWindow.ShowDialog();
            OnFirstStartWindow = new OnFirstStartScreen();
        }

        public ICommand EditPerformanceCommand
        {
            get
            {
                return new RelayCommand<MouseButtonEventArgs>(EditPerformance);
            }
        }

        private void EditPerformance(MouseButtonEventArgs e)
        {
            e.Handled = true;
            BeheerWindow = new LineUpBeheer();
            LineUpBeheerViewModel vm = LineUpBeheerViewModel.ViewModel;
            LineUpBeheerOptredensViewModel vmOptredens = (LineUpBeheerOptredensViewModel)vm.Pages[2];
            vm.CurrentPage = vmOptredens;
            vmOptredens.SelectedItem = (e.Source as Grid).DataContext as Optreden;
            BeheerWindow.Show();
        }

        public ICommand OpenBeheerCommand
        {
            get
            {
                return new RelayCommand<string>(OpenBeheerOp);
            }
        }

        private void OpenBeheerOp(string name)
        {
            BeheerWindow = new LineUpBeheer();
            BeheerWindow.Show();
            LineUpBeheerViewModel vm = LineUpBeheerViewModel.ViewModel;
            switch(name)
            {
                case "Stages":
                    vm.CurrentPage = LineUpBeheerStagesViewModel.ViewModel;
                    break;
                case "Bands":
                    vm.CurrentPage = LineUpBeheerBandsViewModel.ViewModel;
                    break;
                case "Optredens":
                    vm.CurrentPage = LineUpBeheerOptredensViewModel.ViewModel;
                    break;
            }
        }

        public ICommand EditStageCommand
        {
            get
            {
                return new RelayCommand<MouseButtonEventArgs>(EditStage);
            }
        }

        private void EditStage(MouseButtonEventArgs e)
        {
            e.Handled = true;
            BeheerWindow = new LineUpBeheer();
            LineUpBeheerViewModel vm = LineUpBeheerViewModel.ViewModel;
            LineUpBeheerStagesViewModel stageVm = LineUpBeheerStagesViewModel.ViewModel;
            vm.CurrentPage = stageVm;
            stageVm.SelectedItem = (e.Source as Grid).DataContext as Stage;
            BeheerWindow.Show();
        }
    }
}
