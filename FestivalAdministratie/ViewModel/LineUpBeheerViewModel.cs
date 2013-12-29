using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using FestivalLibAdmin.Model;
using GalaSoft.MvvmLight.Command;

namespace FestivalAdministratie.ViewModel
{
    public class LineUpBeheerViewModel:PortableClassLibrary.ObservableObject//this is the viewmodel, equivalent to the application viewmodel, for the beheer window
    {

        static LineUpBeheerViewModel()
        {
            ViewModel = new LineUpBeheerViewModel();
        }

        public static LineUpBeheerViewModel ViewModel { get; set; }


        public LineUpBeheerViewModel()//add pages
        {
            Pages = new List<BeheerIPage>();
            //singleton om dezelfde viewmodel te gebruiken buiten en binnen de usercontrol
            Pages.Add(LineUpBeheerBandsViewModel.ViewModel);
            Pages.Add(LineUpBeheerStagesViewModel.ViewModel);
            Pages.Add(LineUpBeheerOptredensViewModel.ViewModel);
            _currentPage = Pages[0];
            //UnEditedLineUps = new ObservableCollection<LineUp>(LineUp.LineUps);
        }

        private List<BeheerIPage> _pages;

        public List<BeheerIPage> Pages//pages are binded to the combobox for navigation
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

        private BeheerIPage _currentPage;

        public BeheerIPage CurrentPage//current page is binden to the combobox selected item and binded to the usercontrol in the beheer window
        {
            get { return _currentPage; }
            set { _currentPage = value;
            OnPropertyChanged("CurrentPage");
            }
        }

        #region unused
        //public ICommand ChangePageCommand//command to change pages on the selectedchangedevent of the cbo, not needed anymore
        //{
        //    get { return new RelayCommand<BeheerIPage>(ChangePage); }
        //}

        //public void ChangePage(BeheerIPage page)
        //{
        //    CurrentPage = page;
        //}

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
        #endregion

        public ICommand CloseLineUpBeheerCommand
        {
            get
            {
                return new RelayCommand(CloseLineUpBeheer);
            }
        }//command to close the window when clicked on the close button

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
        
    }
}
