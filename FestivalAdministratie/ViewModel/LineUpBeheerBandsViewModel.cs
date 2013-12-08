using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using FestivalLibAdmin.Model;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;

namespace FestivalAdministratie.ViewModel
{
    public class LineUpBeheerBandsViewModel:PortableClassLibrary.ObservableObject,BeheerIPage
    {

        private static LineUpBeheerBandsViewModel _viewModel;
        public static LineUpBeheerBandsViewModel ViewModel
        {
            get
            {
                if (_viewModel == null)
                    _viewModel = new LineUpBeheerBandsViewModel();
                return _viewModel;
            }
        }

        public string Name
        {
            get { return "Bands"; }
        }

        public string NameEnkel
        {
            get { return "Band"; }
        }

        private bool _isBandsEnabled;

        public bool IsBandsEnabled
        {
            get { return _isBandsEnabled; }
            set { _isBandsEnabled = value;
            OnPropertyChanged("IsBandsEnabled");
            }
        }

        private ObservableCollection<Band> _list;
        public ObservableCollection<Band> List
        {
            get {
                if (_list != null) return _list;
                try{
                    _list=Festival.SingleFestival.Bands;
                    if (_list.Count > 0) SelectedItem =_list.First();
                    IsBandsEnabled = true;
                    foreach(Band band in _list)
                        band.PropertyChanged += band_PropertyChanged;
                    _list.CollectionChanged += bands_CollectionChanged;
                    return _list;
                }catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    MessageBox.Show("Bands konden niet uit de database gehaald worden.");
                    IsBandsEnabled = false;
                    return null;
                }
                
            }
            //set
            //{
            //    Festival.SingleFestival.Bands = value;
            //OnPropertyChanged("List");
            //OnPropertyChanged("SelectedItem");
            //}
        }

        void bands_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
                foreach (Band newitem in e.NewItems)
                {
                    try
                    {
                        newitem.PropertyChanged += band_PropertyChanged;
                        if (newitem.ID == null && newitem.IsValid()) newitem.Insert();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        MessageBox.Show("Kon contact niet in de database steken");
                    }
                }
            if (e.OldItems != null)
                foreach (Band olditem in e.OldItems)
                {
                    if (olditem.ID == null) return;
                    try
                    {
                        if (olditem.Delete())
                        {
                            olditem.PropertyChanged -= band_PropertyChanged;
                            olditem.ID = null;
                        }
                        else throw new Exception("Could not remove band");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        MessageBox.Show("Kon band niet verwijderen in de database");
                    }
                }
        }

        void band_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Genres") return;
            Band band = sender as Band;
            if (band.IsValid())
            {
                if (band.ID != null)
                    try
                    {
                        if (!band.Update()) throw new Exception("Could not update band");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        MessageBox.Show("Kon band niet updaten naar de database");
                    }
                else
                    try
                    {
                        if (!band.Insert()) throw new Exception("Could not insert band");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        MessageBox.Show("Kon band niet in de database steken");
                    }
            }
        }

        private Band _selectedItem;

        public Band SelectedItem
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
                return new RelayCommand(AddNewBand, CanAddNewCommand);
            }
        }

        private bool CanAddNewCommand()
        {
            return true;//voorlopig
        }

        private void AddNewBand()
        {
            Band band = new Band();
            band.Name = "Nieuwe band";
            List.Add(band);
            SelectedItem = band;
        }

        public bool IsAnItemSelected
        {
            get
            {
                return SelectedItem != null;
            }
        }


        public ObservableCollection<Genre> Genres
        {
            get { return Festival.SingleFestival.Genres; }
            set
            {
                Festival.SingleFestival.Genres = value;
                OnPropertyChanged("Genres");
            }
        }

        public ICommand AddGenreCommand
        {
            get
            {
                return new RelayCommand<SelectionChangedEventArgs>(AddGenre);
            }
        }

        private void AddGenre(SelectionChangedEventArgs e)
        {
            ComboBox cbo = e.Source as ComboBox;
            Genre genre = cbo.SelectedItem as Genre;
            if (genre != null&&!SelectedItem.Genres.Contains(genre)) SelectedItem.Genres.Add(genre);
            cbo.SelectedItem = null;
        }

        private string _newGenreName;

        public string NewGenreName
        {
            get { return _newGenreName; }
            set
            {
                _newGenreName = value;
                OnPropertyChanged("NewGenreName");
            }
        }

        public ICommand SubmitGenreResultCommand
        {
            get
            {
                return new RelayCommand(AddGenre, CanAddGenre);
            }
        }

        public ICommand OpenDialogCommand
        {
            get { return new RelayCommand(OpenDialog); }
        }

        private void OpenDialog()
        {
            IsDialogVisible = true;
        }

        private void AddGenre()
        {
            foreach (Genre genre in Genres)
                if (genre.Name.Equals(NewGenreName)) return;
            Genre genre1 = new Genre() { Name = NewGenreName };
            Genres.Add(genre1);
            SelectedItem.Genres.Add(genre1);
            NewGenreName = null;
            IsDialogVisible = false;
        }

        private bool CanAddGenre()
        {
            return !String.IsNullOrWhiteSpace(NewGenreName);
        }

        public ICommand CancelGenreCommand
        {
            get
            {
                return new RelayCommand(CancelGenre);
            }
        }

        private void CancelGenre()
        {
            NewGenreName = null;
            IsDialogVisible = false;
        }

        private System.Windows.Visibility _dialogVisibility = System.Windows.Visibility.Collapsed;

        public System.Windows.Visibility DialogVisibility
        {
            get
            {
                return _dialogVisibility;
            }
            set
            {
                _dialogVisibility = value;
                OnPropertyChanged("DialogVisibility");
            }
        }

        private bool _isDialogVisible;

        public bool IsDialogVisible
        {
            get
            {
                return _isDialogVisible;
            }
            set
            {
                _isDialogVisible = value;
                if (value == true) DialogVisibility = System.Windows.Visibility.Visible;
                else DialogVisibility = System.Windows.Visibility.Collapsed;
                OnPropertyChanged("IsDialogVisible");
            }
        }

        public ICommand DeleteGenreCommand
        {
            get
            {
                return new RelayCommand<Genre>(DeleteGenre);
            }
        }

        private void DeleteGenre(Genre genre)
        {
            SelectedItem.Genres.Remove(genre);
        }

        public ICommand ChooseBandPictureCommand
        {
            get
            {
                return new RelayCommand(ChooseBandPicture);
            }
        }

        private void ChooseBandPicture()
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
                ofd.Filter = "Image Files (*.bmp, *.jpg, *.png)|*.bmp;*.jpg;*.png";
                if (ofd.ShowDialog() == true)
                {
                    BandImage = new BitmapImage(new Uri(ofd.FileName));
                    //SelectedItem.Image = BandImage;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

            }
        }

        private BitmapImage _bandImage;

        public BitmapImage BandImage
        {
            get { return _bandImage; }
            set
            {
                _bandImage = value;
                OnPropertyChanged("BandImage");
            }
        }
        
    }
}
