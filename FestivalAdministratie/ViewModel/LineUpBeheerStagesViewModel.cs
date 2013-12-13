using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
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
            //List = Festival.SingleFestival.Stages; ;
        }

        public string Name
        {
            get { return "Stages"; }
        }

        public string NameEnkel
        {
            get { return "Stage"; }
        }

        private ObservableCollection<Stage> _list;

        public ObservableCollection<Stage> List
        {
            get {
                if (_list != null) return _list;
                try
                {
                    _list = Festival.SingleFestival.Stages;
                    if (_list.Count > 0) SelectedItem = _list.First();
                    IsStagesEnabled = true;
                    foreach (Stage stage in _list)
                        stage.PropertyChanged += Stage_PropertyChanged;
                    _list.CollectionChanged += stage_CollectionChanged;
                    return _list;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    MessageBox.Show("Stages konden niet uit de database gehaald worden.");
                    IsStagesEnabled = false;
                    return null;
                }
                }
                //set { Festival.SingleFestival.Stages= value;
                //OnPropertyChanged("List");
                //}
        }

        private void stage_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (Stage newitem in e.NewItems)
                {
                    try
                    {
                        newitem.PropertyChanged += Stage_PropertyChanged;
                        if (newitem.ID == null && newitem.IsValid()) newitem.Insert();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        MessageBox.Show("Kon stage niet in de database steken");
                    }
                }
                OnPropertyChanged("StageNumbers");
            }
            /*if (e.OldItems != null)
            {
                foreach (Stage olditem in e.OldItems)
                {
                    if (olditem.ID == null) return;
                    try
                    {
                        if (olditem.Delete())
                        {
                            olditem.PropertyChanged -= Stage_PropertyChanged;
                            olditem.ID = null;
                        }
                        else throw new Exception("Could not remove stage");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        MessageBox.Show("Kon stage niet verwijderen uit de database, gelieve eerst de optredens van de stage te verwijderen.");
                    }
                }
                OnPropertyChanged("StageNumbers");
            }*/
        }

        private void Stage_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Performances") return;
            Stage stage = sender as Stage;
            if (stage.IsValid())
            {
                if (stage.ID != null)
                    try
                    {
                        if (!stage.Update()) throw new Exception("Could not update stage");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        MessageBox.Show("Kon stage niet updaten naar de database");
                    }
                else
                    try
                    {
                        if (!stage.Insert()) throw new Exception("Could not insert stage");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        MessageBox.Show("Kon stage niet in de database steken");
                    }
            }
        }

        private Stage _selectedItem;

        public Stage SelectedItem
        {
            get { return _selectedItem; }
            set { _selectedItem = value;
            OnPropertyChanged("SelectedItem");
            OnPropertyChanged("IsAnItemSelected");
            OnPropertyChanged("Color");
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
            Stage stage = new Stage();
            List.Add(stage);
            SelectedItem = stage;
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
        private bool _isStagesEnabled;
        public bool IsStagesEnabled {
            get
            {
                return _isStagesEnabled;
            }
            set
            {
                _isStagesEnabled = value;
                OnPropertyChanged("IsStageEnabled");
            }
        }
    }
}
