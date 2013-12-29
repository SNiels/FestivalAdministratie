using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using FestivalLibAdmin.Model;
using FestivalAdministratie.View;
using GalaSoft.MvvmLight.Command;
using System.Windows;

namespace FestivalAdministratie.ViewModel
{
    public class LineUpViewModel:PortableClassLibrary.ObservableObject,IPage
    {
        public LineUpViewModel()
        {
            #region testdata
            //LineUps = Model.Festival.SingleFestival.LineUps;
            //Stages = Stage.Stages;
            //Bands = Band.Bands;
            //Optredens = Optreden.Optredens;
            //dummy data
            //Festival.StartDate = DateTime.Today.AddDays(-3);
            //Festival.EndDate = DateTime.Today.AddDays(3);
            #endregion
            if (LineUps.Count()>0)
            SelectedLineUp = LineUps[0];//select the first lineup if there is a lineup, a lineup is equivalent to a day of the festival
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

        #region unused

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
        #endregion

        public ObservableCollection<FestivalLibAdmin.Model.LineUp> LineUps
        {
            get { 
                //return _lineUps;
                return Festival.SingleFestival.LineUps;
            }
            #region unused
            //set {
            //    Festival.SingleFestival.LineUps = value;
            //    if (value != null && value.Count() > 0)
            //        SelectedLineUp = value.First();
            //    else SelectedLineUp = null;
            //    //_lineUps = value;
            //OnPropertyChanged("LineUps");
            ////if (value != null) value.CollectionChanged += LineUps_CollectionChanged;
            //}
            #endregion
        } //get all days
        #region unused
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
        #endregion

        private FestivalLibAdmin.Model.LineUp _selectedLineUp;

        public FestivalLibAdmin.Model.LineUp SelectedLineUp
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
            get { 
                OnPropertyChanged("IsHintVisible");//this hint is to suggest to add items to the festival if there is nothing to do yet
                return SelectedLineUp.Hours;
            }
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
        }///command to open the beheer window

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
        } //open the settings window

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
        } //binded to a mouseup event of a performance in the lineup control

        private void EditPerformance(MouseButtonEventArgs e)
        {
            e.Handled = true;
            LineUpBeheerViewModel vm = null;
            LineUpBeheerOptredensViewModel vmOptredens= null;
            if (LineUpBeheerViewModel.ViewModel.Window.IsVisible)
            {
                vm = LineUpBeheerViewModel.ViewModel;
                vmOptredens = LineUpBeheerOptredensViewModel.ViewModel;
                vm.CurrentPage = vmOptredens;
                try
                {
                    vmOptredens.SelectedItem = (e.Source as Grid).DataContext as Optreden;
                }
                catch (Exception) { }
                LineUpBeheerViewModel.ViewModel.Window.Activate();
                return;
            }
            BeheerWindow = new LineUpBeheer();
             vm = LineUpBeheerViewModel.ViewModel;
            vmOptredens = LineUpBeheerOptredensViewModel.ViewModel;
            vm.CurrentPage = vmOptredens;
            try
            {
                vmOptredens.SelectedItem = (e.Source as Grid).DataContext as Optreden;
            }catch(Exception){}
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
        }//opens the beheer window on a specified page, this is used for the mouseup event on the stages,bands and performances textblocks in the uihint

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
            LineUpBeheerViewModel vm = null;
            LineUpBeheerStagesViewModel stageVm = null;
            if (LineUpBeheerViewModel.ViewModel.Window.IsVisible)
            {
                vm = LineUpBeheerViewModel.ViewModel;
                stageVm = LineUpBeheerStagesViewModel.ViewModel;
                vm.CurrentPage = stageVm;
                try
                {
                    stageVm.SelectedItem = (e.Source as FrameworkElement).DataContext as Stage;
                }
                catch (Exception) { }
                LineUpBeheerViewModel.ViewModel.Window.Activate();
                return;
            }
            BeheerWindow = new LineUpBeheer();
            vm = LineUpBeheerViewModel.ViewModel;
            stageVm = LineUpBeheerStagesViewModel.ViewModel;
            vm.CurrentPage = stageVm;
            try
            {
                stageVm.SelectedItem = (e.Source as FrameworkElement).DataContext as Stage;
            }
            catch (Exception) { }
            BeheerWindow.Show();
        }//on mousedown of a stage, navigate to the beheer of that stage

        public Visibility IsHintVisible
        {
            get
            {
                if (Festival.SingleFestival.Stages.Count() > 0 && Festival.SingleFestival.Optredens.Count() > 0) return Visibility.Hidden;
                else return Visibility.Visible;
            }
        }
    }
}
