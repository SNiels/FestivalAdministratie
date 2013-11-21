using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using FestivalAdministratie.Model;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;

namespace FestivalAdministratie.ViewModel
{
    public class MapViewModel:ObservableObject,IPage
    {
        //public MapViewModel()
        //{
        //    //Stages = Stage.Stages;
        //}

        private ObservableCollection<Stage> _stages;

        public ObservableCollection<Stage> Stages
        {
            get { return Festival.SingleFestival.Stages; }
            set
            {
                Festival.SingleFestival.Stages= value;
                OnPropertyChanged("Stages");
            }
        }

        private BitmapImage _image;

        public BitmapImage Image
        {
            get { return _image; }
            set { _image = value;
            OnPropertyChanged("Image");
            }
        }

        public string Name
        {
            get { return "Map"; }
        }

        public ICommand ChooseMapCommand
        {
            get
            {
                return new RelayCommand(ChooseMap);
            }
        }

        private void ChooseMap()
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
                ofd.Filter = "Image Files (*.bmp, *.jpg, *.png)|*.bmp;*.jpg;*.png";
                if(ofd.ShowDialog()==true)
                {
                    Image = new BitmapImage(new Uri(ofd.FileName));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                
            }
        }
        
    }
}
