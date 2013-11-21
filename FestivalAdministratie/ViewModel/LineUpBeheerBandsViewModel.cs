using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using FestivalAdministratie.Model;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;

namespace FestivalAdministratie.ViewModel
{
    public class LineUpBeheerBandsViewModel:ObservableObject,BeheerIPage
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

        public ObservableCollection<Band> List
        {
            get { return Festival.SingleFestival.Bands; }
            set
            {
                Festival.SingleFestival.Bands = value;
            OnPropertyChanged("List");
            OnPropertyChanged("SelectedItem");
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
            Band.Bands.Add(band);
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
                return new RelayCommand<Genre>(AddGenre);
            }
        }

        private void AddGenre(Genre genre)
        {
            if (genre != null) SelectedItem.Genres.Add(genre);
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
