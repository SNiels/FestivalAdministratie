using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Helper;

namespace FestivalLibAdmin.Model
{
    public class Band : ObservableValidationObject
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

        public Band()
        {

        }

        public Band(IDataRecord record)
        {
            ID = record["ID"].ToString();
            Name = record["Name"].ToString();
            Picture = !Convert.IsDBNull(record["Picture"]) ? record["Picture"].ToString() : null;
            Description = !Convert.IsDBNull(record["Description"]) ? record["Discription"].ToString() : null;
            Facebook = !Convert.IsDBNull(record["Facebook"]) ? new Uri(record["Facebook"].ToString()) : null;
            Twitter = !Convert.IsDBNull(record["Twitter"]) ? new Uri(record["Twitter"].ToString()) : null;
        }

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
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        private string _picture;

        public virtual string Picture
        {
            get { return _picture; }
            set
            {
                _picture = value;
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
            set
            {
                _description = value;
                OnPropertyChanged("Description");
            }
        }

        private Uri _facebook;

        public virtual Uri Facebook
        {
            get { return _facebook; }
            set { _facebook = value;
            OnPropertyChanged("Facebook");
            }
        }

        private Uri _twitter;

        public virtual Uri Twitter
        {
            get { return _twitter; }
            set { _twitter = value;
            OnPropertyChanged("Twitter");
            }
        }

        private ObservableCollection<Genre> _genres;

        

        public virtual ObservableCollection<Genre> Genres
        {
            get {
                if (_genres == null )
                    if(ID != null) 
                        _genres = Genre.GetGenresByBandId(ID);
                    else _genres = new ObservableCollection<Genre>();
                return _genres; }
            set { _genres = value;
            OnPropertyChanged("Genres");
            }
        }

        public override string ToString()
        {
            return Name;
        }

        public static ObservableCollection<Band> GetBands()
        {
            
            DbDataReader reader = null;
            try
            {
                ObservableCollection<Band> bands= new ObservableCollection<Band>();
                reader = Database.GetData("SELECT * FROM Bands");
                while (reader.Read())
                    bands.Add(new Band(reader));
                reader.Close();
                reader = null;
                return bands;
            }
            catch (Exception ex)
            {
                if (reader != null && !reader.IsClosed)
                {
                    reader.Close();
                    reader = null;
                }
                throw new Exception("Could not get bands", ex);
            }
        }

        public bool Delete()
        {
            try
            {
                int i = Database.ModifyData("DELETE FROM Bands WHERE ID=@ID",
                    Database.CreateParameter("@ID", ID));
                if (i < 1) return false;
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception("Could not delete the damn band", ex);
            }
        }

        public bool Update()
        {
            try
            {
                int amountOfModifiedRows = Database.ModifyData("UPDATE Bands SET Name=@Name,Picture=@Picture,Facebook=@Facebook,Twitter=@Twitter WHERE ID=@ID",
                    Database.CreateParameter("@Name", Name),
                    Database.CreateParameter("@Picture", Picture),
                    Database.CreateParameter("@Description", Description),
                    Database.CreateParameter("@Facebook", Facebook),
                    Database.CreateParameter("@Twitter", Twitter),
                    Database.CreateParameter("@ID",ID)
                    );
                if (amountOfModifiedRows == 1)
                    return true;
                else return false;
            }
            catch (Exception ex)
            {
                throw new Exception("Could not edit the band, me very sorry!", ex);
            }
        }

        public bool Insert()
        {
            DbDataReader reader = null;
            try
            {
                string sql = "INSERT INTO Bands (Name, Picture, Description, Facebook, Twitter) VALUES(@Name,@Picture, @Description, @Facebook, @Twitter); SELECT SCOPE_IDENTITY() as 'ID'";
                reader = Database.GetData(sql,
                    Database.CreateParameter("@Name", Name),
                    Database.CreateParameter("@Picture", Picture),
                    Database.CreateParameter("@Description", Description),
                    Database.CreateParameter("@Facebook", Facebook),
                    Database.CreateParameter("@Twitter", Twitter)
                    );


                if (reader.Read() && !Convert.IsDBNull(reader["ID"]))
                {
                    ID = reader["ID"].ToString();
                    return true;
                }
                else
                    throw new Exception("Could not get the ID of the inserted band, it is possible the insert failed.");

            }
            catch (Exception ex)
            {
                if (reader != null) reader.Close();
                throw new Exception("Could not insert band.", ex);
            }
        }
    }
}
