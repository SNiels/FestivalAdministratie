using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FestivalLibAdmin.Model
{
    public class Band : PortableClassLibrary.Model.Band, IDataErrorInfo
    {
        public Band()
        {
            Name = "Niewe band";
            //Genres = new ObservableCollection<Genre>();
        }

        //static Band()
        //{
        //    //Bands = new ObservableCollection<Band>();
        //}

        //public override string Name
        //{
        //    get
        //    {
        //        return base.Name;
        //    }
        //    set
        //    {
        //        base.Name = value;
        //        OnPropertyChanged("Name");
        //    }
        //}

        //public override string Description
        //{
        //    get
        //    {
        //        return base.Description;
        //    }
        //    set
        //    {
        //        base.Description = value;
        //        OnPropertyChanged("Description");
        //    }
        //}

        //public override Uri Facebook
        //{
        //    get
        //    {
        //        return base.Facebook;
        //    }
        //    set
        //    {
        //        base.Facebook = value;
        //        OnPropertyChanged("Facebook");
        //    }
        //}
        //private ObservableCollection<Genre> _genres;

        //public override ObservableCollection<Genre> Genres
        //{
        //    get
        //    {

        //        return _genres;
        //    }
        //    set
        //    {
        //        _genres = value;
        //        OnPropertyChanged("Genres");
        //    }
        //}

        //public override string Picture
        //{
        //    get
        //    {
        //        return base.Picture;
        //    }
        //    set
        //    {
        //        base.Picture = value;
        //        OnPropertyChanged("Picture");
        //    }
        //}

        //public override Uri Twitter
        //{
        //    get
        //    {
        //        return base.Twitter;
        //    }
        //    set
        //    {
        //        base.Twitter = value;
        //        OnPropertyChanged("Twitter");
        //    }
        //}



        //private static ObservableCollection<Band> _bands;

        //public static ObservableCollection<Band> Bands
        //{
        //    get { return _bands; }
        //    set { _bands = value; }
        //}
       

       

        //private string _name;

        //public string Name
        //{
        //    get { return _name; }
        //    set { _name = value;
        //    OnPropertyChanged("Name");
        //    }
        //}
        
        //private string _picture;

        //public string Picture
        //{
        //    get { return _picture; }
        //    set { _picture = value;
        //    OnPropertyChanged("Picture");
        //    }
        //}

        ////public BitmapImage Image
        ////{
        ////    get
        ////    {
        ////        return ImageConverter.base64image(Picture);
        ////    }
        ////    set
        ////    {
        ////        if(value!=null)
        ////        Picture = ImageConverter.ImageToBase64(value);
        ////    }
        ////}

        //private string _description;

        //public string Description
        //{
        //    get { return _description; }
        //    set { _description = value;
        //    OnPropertyChanged("Description");
        //    }
        //}

        //private Uri _facebook;

        //public Uri Facebook
        //{
        //    get { return _facebook; }
        //    set { _facebook = value; }
        //}

        //private Uri _twitter;

        //public Uri Twitter
        //{
        //    get { return _twitter; }
        //    set { _twitter = value; }
        //}

        //private ObservableCollection<Genre> _genres;

        //public ObservableCollection<Genre> Genres
        //{
        //    get { return _genres; }
        //    set { _genres = value; }
        //}

        public override string ToString()
        {
            return Name;
        }

        public string Error
        {
            get { return "Er is een fout gebeurt."; }
        }

        public string this[string propertyName]
        {
            get
            {
                try
                {
                    object value = this.GetType().GetProperty(propertyName).GetValue(this);
                    Validator.ValidateProperty(value, new ValidationContext(this) { MemberName = propertyName });
                }
                catch (Exception ex)//moet nog validation exception worden
                {
                    return ex.Message;
                }
                return string.Empty;
            }
        }

        public bool IsValid()
        {
            return Validator.TryValidateObject(this, new ValidationContext(this), null);
        }
    }
}
