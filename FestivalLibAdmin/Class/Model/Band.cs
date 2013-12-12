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

        public Band()
        {
        }

        public Band(IDataRecord record)
        {
            ID = record["ID"].ToString();
            Name = record["Name"].ToString();
            Picture = !Convert.IsDBNull(record["Picture"]) ? record["Picture"].ToString() : null;
            Description = !Convert.IsDBNull(record["Description"]) ? record["Description"].ToString() : null;
            Facebook = !Convert.IsDBNull(record["Facebook"]) ? record["Facebook"].ToString() : null;
            Twitter = !Convert.IsDBNull(record["Twitter"]) ? record["Twitter"].ToString() : null;
        }

        private string _id;
        [ScaffoldColumn(false)]
        public string ID
        {
            get { return _id; }
            set { _id = value;
            OnPropertyChanged("HasID");
            }
        }

        private string _name;
        [Required(ErrorMessage="Gelieve een naam in te geven")]
        [MinLength(2,ErrorMessage="Een naam moet minimum 2 letters bevatten")]
        [Display(Name="Naam",Order=0,Description="De naam van de band",GroupName="Band",Prompt="Bv: Barack Obama")]
        [DisplayFormat(ConvertEmptyStringToNull=true)]
        [Editable(true,AllowInitialValue=false)]
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        private string _picture;
        [DataType(DataType.ImageUrl)]
        [Display(Name = "Afbeelding", Order = 1, Description = "De afbeelding van de band", GroupName = "Band")]
        [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "Geef een url naar een image in")]
        [FileExtensions(ErrorMessage = "Gelieve een geldige image in te geven (.jpg, .jpeg, .gif or .png)",Extensions="jpg,jpeg,gif,png")]
        public string Picture
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
        [Display(Name = "Beschrijving", Order = 2, Description = "De beschrijving van de band", GroupName = "Band")]
        [DataType(DataType.MultilineText)]
        [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "Geef een beschrijving in")]
        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                OnPropertyChanged("Description");
            }
        }

        private string _facebook;
        [RegularExpression(@"^(http|https)\://[a-zA-Z0-9\-\.]+\.[a-zA-Z]{2,3}(:[a-zA-Z0-9]*)?/?([a-zA-Z0-9\-\._\?\,\'/\\\+&amp;%\$#\=~])*$", ErrorMessage = "Gelieve een geldige url te geven")]
        [DataType(DataType.Url,ErrorMessage="Gelieve een geldige url mee te geven")]
        [Display(Name = "Facebook", Order = 3, Description = "Het Facebook profiel van de band", GroupName = "Band")]
        [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "Geef een url naar het Facebook profiel in")]
        public string Facebook
        {
            get { return _facebook; }
            set { _facebook = value;
            OnPropertyChanged("Facebook");
            }
        }

        private string _twitter;
        [Display(Name = "Twitter", Order = 4, Description = "Het Twitter profiel van de band", GroupName = "Band")]
        [RegularExpression(@"^(http|https)\://[a-zA-Z0-9\-\.]+\.[a-zA-Z]{2,3}(:[a-zA-Z0-9]*)?/?([a-zA-Z0-9\-\._\?\,\'/\\\+&amp;%\$#\=~])*$", ErrorMessage = "Gelieve een geldige url te geven")]
        [DataType(DataType.Url, ErrorMessage = "Gelieve een geldige url mee te geven")]
        [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "Geef en url naar het Twitter profiel in")]
        public string Twitter
        {
            get { return _twitter; }
            set { _twitter = value;
            OnPropertyChanged("Twitter");
            }
        }

        public bool HasID
        {
            get
            {
                return ID!=null;
            }
        }

        private ObservableCollection<Genre> _genres;
        [Display(Name = "Genres", Order = 5, Description = "De genres van de band", GroupName = "Band")]
        public ObservableCollection<Genre> Genres
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
                int amountOfModifiedRows = Database.ModifyData("UPDATE Bands SET Name=@Name,Picture=@Picture,Description=@Description,Facebook=@Facebook,Twitter=@Twitter WHERE ID=@ID",
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
