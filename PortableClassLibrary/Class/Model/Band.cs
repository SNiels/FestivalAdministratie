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
        #region ctors
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
        public Band()
        {

        }

        #endregion

        #region props

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

        public virtual string Description
        {
            get { return _description; }
            set { _description = value;
            OnPropertyChanged("Description");
            }
        }

        private string _facebook;

        public string Facebook
        {
            get { return _facebook; }
            set { _facebook = value;
            OnPropertyChanged("Facebook");
            }
        }

        private string _twitter;

        public string Twitter
        {
            get { return _twitter; }
            set { _twitter = value;
            OnPropertyChanged("Twitter");
            }
        }

        private ObservableCollection<Genre> _genres;

        public ObservableCollection<Genre> Genres
        {
            get { return _genres; }
            set { _genres = value;
            OnPropertyChanged("Genres");
            }
        }

        public ObservableCollection<Optreden> Optredens
        {
            get
            {
                return new ObservableCollection<Optreden>(Festival.SingleFestival.Optredens.Where(optreden => optreden.Band.ID == ID));

            }
        }

        #endregion

        public override string ToString()
        {
            return Name;
        }
    }
}
