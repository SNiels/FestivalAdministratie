using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortableClassLibrary.Model
{
    public class Band : ObservableObject
    {
        //static Band()
        //{
        //    Bands = new ObservableCollection<Band>();
        //}

        //private static ObservableCollection<Band> _bands;

        //public static ObservableCollection<Band> Bands
        //{
        //    get { return _bands; }
        //    set { _bands = value; }
        //}

        private string _id;

        public virtual string ID
        {
            get { return _id; }
            set { _id = value; }
        }

        private string _name;

        public virtual string Name
        {
            get { return _name; }
            set { _name = value;
            OnPropertyChanged("Name");
            }
        }
        
        private string _picture;

        public virtual string Picture
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

        public virtual string Description
        {
            get { return _description; }
            set { _description = value;
            OnPropertyChanged("Description");
            }
        }

        private Uri _facebook;

        public virtual Uri Facebook
        {
            get { return _facebook; }
            set { _facebook = value; }
        }

        private Uri _twitter;

        public virtual Uri Twitter
        {
            get { return _twitter; }
            set { _twitter = value; }
        }

        private ObservableCollection<Genre> _genres;

        public virtual ObservableCollection<Genre> Genres
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
