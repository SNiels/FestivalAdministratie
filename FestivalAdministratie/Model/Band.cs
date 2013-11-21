using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace FestivalAdministratie.Model
{
    public class Band : ObservableValidationObject
    {
        public Band()
        {
            Name = "Niewe band";
            Genres = new ObservableCollection<Genre>();
        }

        static Band()
        {
            Bands = new ObservableCollection<Band>();
        }

        private static ObservableCollection<Band> _bands;

        public static ObservableCollection<Band> Bands
        {
            get { return _bands; }
            set { _bands = value; }
        }
        

        private string _id;

        public string ID
        {
            get { return _id; }
            set { _id = value; }
        }

        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value;
            OnPropertyChanged("Name");
            }
        }
        

        private string _picture;

        public string Picture
        {
            get { return _picture; }
            set { _picture = value;
            OnPropertyChanged("Picture");
            }
        }

        //public BitmapImage Image
        //{
        //    get
        //    {
        //        return ImageConverter.base64image(Picture);
        //    }
        //    set
        //    {
        //        if(value!=null)
        //        Picture = ImageConverter.ImageToBase64(value);
        //    }
        //}

        private string _description;

        public string Description
        {
            get { return _description; }
            set { _description = value;
            OnPropertyChanged("Description");
            }
        }

        private Uri _facebook;

        public Uri Facebook
        {
            get { return _facebook; }
            set { _facebook = value; }
        }

        private Uri _twitter;

        public Uri Twitter
        {
            get { return _twitter; }
            set { _twitter = value; }
        }

        private ObservableCollection<Genre> _genres;

        public ObservableCollection<Genre> Genres
        {
            get { return _genres; }
            set { _genres = value; }
        }


        public override string ToString()
        {
            return Name;
        }
    }
}
