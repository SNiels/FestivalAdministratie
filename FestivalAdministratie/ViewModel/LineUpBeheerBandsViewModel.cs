using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
                    foreach (Band band in _list)
                    {
                        band.PropertyChanged += band_PropertyChanged;
                        band.Genres.CollectionChanged += BandGenres_CollectionChanged;
                    }
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

        void BandGenres_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
                foreach (Genre newitem in e.NewItems)
                {
                    try
                    {
                        try
                        {
                            if (newitem.ID == null) newitem.Insert();
                        }catch(Exception ex)
                        {
                            newitem.GetIDFromName();
                        }
                        if (SelectedItem.ID!=null && newitem.IsValid()) newitem.InsertIntoBand(SelectedItem);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        MessageBox.Show("Kon band genre niet in de database steken");
                    }
                }
            if (e.OldItems != null)
                foreach (Genre olditem in e.OldItems)
                {
                    if (olditem.ID == null||SelectedItem.ID==null) return;
                    try
                    {
                        if (olditem.DeleteFromBand(SelectedItem))
                        {
                            olditem.ID = null;
                        }
                        else throw new Exception("Could not remove genre from band");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        MessageBox.Show("Kon band genre niet verwijderen uit de database");
                    }
                }
        }

        void bands_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
                foreach (Band newitem in e.NewItems)
                {
                    try
                    {
                        newitem.PropertyChanged += band_PropertyChanged;
                        newitem.Genres.CollectionChanged += BandGenres_CollectionChanged;
                        if (newitem.ID == null && newitem.IsValid()) newitem.Insert();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        MessageBox.Show("Kon contact niet in de database steken");
                    }
                }
        }

        void band_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Genres") return;
            if (e.PropertyName == "HasID") OnPropertyChanged("IsAnItemSelectedThatHasID");
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
            OnPropertyChanged("IsAnItemSelectedThatHasID");
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
            band.PropertyChanged+=band_PropertyChanged;
        }

        public bool IsAnItemSelected
        {
            get
            {
                return SelectedItem != null;
            }
        }

        public bool IsAnItemSelectedThatHasID
        {
            get
            {
                return SelectedItem != null&&SelectedItem.HasID;
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
            if (genre != null&&!SelectedItem.Genres.Contains(genre))
                SelectedItem.Genres.Add(genre);
            cbo.SelectedItem = null;
        }


        private string _newGenreName;
        [Required(ErrorMessage = "Gelieve een naam in te vullen")]
        [Display(Name = "Naam", Order = 0, Description = "De naam van het genre", GroupName = "Genre", Prompt = "Bv. Techno")]
        [DisplayFormat(ConvertEmptyStringToNull = true)]
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

        public ICommand SubmitGenreEditCommand
        {
            get
            {
                return new RelayCommand(EditGenre, CanEditGenre);
            }
        }

        private void EditGenre()
        {
            EditedGenre.Name = EditedGenreName;
            try
            {
                if(!EditedGenre.Update())
                {
                    MessageBox.Show("Kon het genre niet updaten naar de database.");
                }
            }catch(Exception ex){
                Console.WriteLine(ex.Message);
            }
            EditedGenre = null;
            EditedGenreName = null;
            IsEditDialogVisible = false;
        }

        private bool CanEditGenre()
        {
            return !String.IsNullOrWhiteSpace(EditedGenreName) && SelectedItem.IsValid() && !Genres.Any(genre => genre.Name == EditedGenreName);
        }

        public ICommand EditGenreCommand
        {
            get
            {
                return new RelayCommand<Genre>(OpenEditDialog);
            }
        }

        private void OpenEditDialog(Genre obj)
        {
            IsEditDialogVisible = true;
            EditedGenre = obj;
            EditedGenreName =string.Empty+obj.Name;//nieuw instantie van string zodat de oude de naam van het genre niet meteen gewijzigd wordt
        }

        private Genre _editedGenre;

        public Genre EditedGenre
        {
            get { return _editedGenre; }
            set { _editedGenre = value;
            OnPropertyChanged("EditedGenre");
            }
        }

        private string _editedGenreName;

        public string EditedGenreName
        {
            get { return _editedGenreName; }
            set { _editedGenreName = value;
            OnPropertyChanged("EditedGenreName");
            }
        }

        public ICommand CancelGenreEditCommand
        {
            get
            {
                return new RelayCommand(CancelGenreEdit);
            }
        }

        private void CancelGenreEdit()
        {
            IsEditDialogVisible = false;
            EditedGenre = null;
            EditedGenreName = null;
        }

        private void OpenDialog()
        {
            IsDialogVisible = true;
        }

        private void AddGenre()
        {
            //if (!Genres.Any(genre => genre.Name == NewGenreName)) return;
            Genre genre1 = new Genre() { Name = NewGenreName };
            try
            {
                if (!genre1.Insert()) throw new Exception("Could not insert genre");
                Genres.Add(genre1);
                SelectedItem.Genres.Add(genre1);
                NewGenreName = null;
                IsDialogVisible = false;
            }catch(Exception ex)
            {
                MessageBox.Show("Kon genre niet in de database steken");
                Console.Write(ex.Message);
            }
        }

        private bool CanAddGenre()
        {
            return !String.IsNullOrWhiteSpace(NewGenreName) && SelectedItem.IsValid() && !Genres.Any(genre => genre.Name == NewGenreName);
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

        public bool IsEditDialogVisible
        {
            get
            {
                return _isEditDialogVisible;
            }
            set
            {
                _isEditDialogVisible = value;
                if (value == true) EditDialogVisibility = System.Windows.Visibility.Visible;
                else EditDialogVisibility = System.Windows.Visibility.Collapsed;
                OnPropertyChanged("IsEditDialogVisible"); 
            }
        }

        private System.Windows.Visibility _editDialogVisibility = System.Windows.Visibility.Collapsed;

        public System.Windows.Visibility EditDialogVisibility
        {
            get
            {
                return _editDialogVisibility;
            }
            set
            {
                _editDialogVisibility = value;
                OnPropertyChanged("EditDialogVisibility");
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

        public ICommand DeleteBandCommand
        {
            get
            {
                return new RelayCommand(DeleteSelectedBand,IsAnItemSelectedMethod);
            }
        }

        private bool IsAnItemSelectedMethod()
        {
            return IsAnItemSelectedThatHasID;
        }

        private void DeleteSelectedBand()
        {
            if (SelectedItem.ID == null) return;
            try
            {
                if (SelectedItem.Delete())
                {
                    SelectedItem.PropertyChanged -= band_PropertyChanged;
                    SelectedItem.Genres.CollectionChanged -= BandGenres_CollectionChanged;
                    SelectedItem.ID = null;
                    List.Remove(SelectedItem);
                    SelectedItem = null;
                }
                else throw new Exception("Could not remove band");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                MessageBox.Show("Kon band niet verwijderen uit de database, gelieve eerst de optredens van de band te verwijderen.");
            }
        }

        public ICommand DropImageCommand
        {
            get
            {
                return new RelayCommand<DragEventArgs>(AddImage);
            }
        }

        private void AddImage(DragEventArgs e)
        {
            var data = e.Data as DataObject;
            if(data.ContainsText())
            {
                if (new Regex(@"^(http\:\/\/[a-zA-Z0-9\-\.]+\.[a-zA-Z]{2,3}(?:\/\S*)?(?:[a-zA-Z0-9_])+\.(?:jpg|jpeg|gif|png))$", RegexOptions.None).IsMatch(data.GetText()))
                SelectedItem.Picture = data.GetText();
            }
            //if (data.ContainsFileDropList())
            //{
            //    var files = data.GetFileDropList();
            //    //SelectedAttraction.Picture = ImageConverter.Convert(files[0]);

            //}
        }


        public bool _isEditDialogVisible { get; set; }
    }
}
