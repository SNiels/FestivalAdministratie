using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FestivalLibAdmin.Model
{
    public class Genre:PortableClassLibrary.Model.Genre, IDataErrorInfo
    {
        //static Genre()
        //{
        //    Genres = new ObservableCollection<Genre>();
        //}

        //private static ObservableCollection<Genre> _genres;

        //public static ObservableCollection<Genre> Genres
        //{
        //    get { return _genres; }
        //    set { _genres = value; }
        //}
        

        //private string _id;

        //public string ID
        //{
        //    get { return _id; }
        //    set { _id = value; }
        //}

        //private string _name;

        //public string Name
        //{
        //    get { return _name; }
        //    set { _name = value;
        //    OnPropertyChanged("Name");
        //    }
        //}

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
