using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using FestivalAdministratie.Model;
using GalaSoft.MvvmLight.Command;

namespace FestivalAdministratie.ViewModel
{
    public class LineUpBeheerViewModel:ObservableObject
    {

        static LineUpBeheerViewModel()
        {
            ViewModel = new LineUpBeheerViewModel();
        }

        public static LineUpBeheerViewModel ViewModel { get; set; }


        public LineUpBeheerViewModel()
        {
            Pages = new List<BeheerIPage>();
            //singleton om dezelfde viewmodel te gebruiken buiten en binnen de usercontrol
            Pages.Add(LineUpBeheerBandsViewModel.ViewModel);
            Pages.Add(LineUpBeheerStagesViewModel.ViewModel);
            Pages.Add(LineUpBeheerOptredensViewModel.ViewModel);
            currentPage = Pages[0];
            //UnEditedLineUps = new ObservableCollection<LineUp>(LineUp.LineUps);
        }

        private List<BeheerIPage> _pages;

        public List<BeheerIPage> Pages
        {
            get
            {
                if (_pages == null)
                    _pages = new List<BeheerIPage>();
                return _pages;
            }
            set { _pages = value;
            OnPropertyChanged("Pages");
            }
        }

        private BeheerIPage currentPage;

        public BeheerIPage CurrentPage
        {
            get { return currentPage; }
            set { currentPage = value;
            OnPropertyChanged("CurrentPage");
            }
        }
        

        //public ICommand ChangePageCommand
        //{
        //    get { return new RelayCommand<BeheerIPage>(ChangePage); }
        //}

        //public void ChangePage(BeheerIPage page)
        //{
        //    CurrentPage = page;
        //}

        public ICommand CloseLineUpBeheerCommand
        {
            get
            {
                return new RelayCommand(CloseLineUpBeheer);
            }
        }

        private void CloseLineUpBeheer()
        {
            Window.Close();
        }

        private Window _window;

        public Window Window
        {
            get { return _window; }
            set { _window = value; }
        }
        

        //private ObservableCollection<T> _list;

        //public ObservableCollection<T> List
        //{
        //    get { return _list; }
        //    set { _list = value; }
        //}

        //private LineUpViewModel _lineUpViewModel;

        //public LineUpViewModel LineUpViewModel
        //{
        //    get { return _lineUpViewModel; }
        //    set { _lineUpViewModel = value; }
        //}

        //public ObservableCollection<LineUp> UnEditedLineUps { get; set; }

        //public ICommand CancelEditsCommand
        //{
        //    get
        //    {
        //        return new RelayCommand(CancelEdits, CanCancelEdits);
        //    }
        //}

        //private void CancelEdits()
        //{
        //    LineUp.LineUps = UnEditedLineUps;
        //    LineUpViewModel.LineUps = UnEditedLineUps;
        //}

        //private bool CanCancelEdits()
        //{
        //    return true;
        //}

        //public ICommand SubmitDataCommand
        //{
        //    get {
        //        return new RelayCommand(SubmitData, CanSubmitData);
        //    }
        //}

        //private void SubmitData()
        //{
        //    UnEditedLineUps = new ObservableCollection<LineUp>(LineUp.LineUps);
        //}

        //private bool CanSubmitData()
        //{
        //    return true;
        //}
        
    }
}
