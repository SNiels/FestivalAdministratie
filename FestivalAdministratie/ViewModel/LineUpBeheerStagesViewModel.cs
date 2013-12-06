using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using FestivalLibAdmin.Model;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;

namespace FestivalAdministratie.ViewModel
{
    public class LineUpBeheerStagesViewModel:PortableClassLibrary.ObservableObject,BeheerIPage
    {

        private static LineUpBeheerStagesViewModel _viewModel;
        public static LineUpBeheerStagesViewModel ViewModel
        {
            get
            {
                if (_viewModel == null)
                    _viewModel = new LineUpBeheerStagesViewModel();
                return _viewModel;
            }
        }

        public LineUpBeheerStagesViewModel()
        {
            List = Festival.SingleFestival.Stages; ;
        }

        public string Name
        {
            get { return "Stages"; }
        }

        public string NameEnkel
        {
            get { return "Stage"; }
        }

        //private ObservableCollection<Stage> _list;

        public ObservableCollection<Stage> List
        {
            get { return Festival.SingleFestival.Stages; }
            set { Festival.SingleFestival.Stages= value;
            OnPropertyChanged("List");
            }
        }

        private Stage _selectedItem;

        public Stage SelectedItem
        {
            get { return _selectedItem; }
            set { _selectedItem = value;
            OnPropertyChanged("SelectedItem");
            OnPropertyChanged("IsAnItemSelected");
            }
        }

        public ICommand AddItemCommand
        {
            get
            {
                return new RelayCommand(AddNewStage, CanAddNewCommand);
            }
        }

        private void AddNewStage()
        {
            List.Add(new Stage());
        }

        private bool CanAddNewCommand()
        {
            return true;//voorlopig
        }

        public bool IsAnItemSelected
        {
            get
            {
                return SelectedItem != null;
            }
        }

        public ICommand ChooseStagePictureCommand
        {
            get
            {
                return new RelayCommand(ChooseStagePicture);
            }
        }

        private void ChooseStagePicture()
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
                ofd.Filter = "Image Files (*.bmp, *.jpg, *.png)|*.bmp;*.jpg;*.png";
                if (ofd.ShowDialog() == true)
                {
                    StageImage = new BitmapImage(new Uri(ofd.FileName));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

            }
        }

        private BitmapImage _stageImage;

        public BitmapImage StageImage
        {
            get { return _stageImage; }
            set
            {
                _stageImage = value;
                OnPropertyChanged("StageImage");
            }
        }
    }
}
