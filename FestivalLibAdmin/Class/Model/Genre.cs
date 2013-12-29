using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Helper;

namespace FestivalLibAdmin.Model
{
    public class Genre:ObservableValidationObject
    {

        public Genre()
        {
                
        }

        public Genre(IDataRecord record)
        {
            ID = record["ID"].ToString();
            Name = record["Name"].ToString();
        }

        private string _id;
        [ScaffoldColumn(false)]
        public string ID
        {
            get { return _id; }
            set { _id = value; }
        }

        private string _name;

        [Required(ErrorMessage="Gelieve een naam in te vullen")]
        [Display(Name = "Naam", Order = 0, Description = "De naam van het genre", GroupName = "Genre", Prompt = "Bv. Techno")]
        [DisplayFormat(ConvertEmptyStringToNull = true)]
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        public static ObservableCollection<Genre> GetGenresByBandId(string id)
        {
            //wpf
            ObservableCollection<Genre> genres = GetGenresByQuery("SELECT Genres.ID as ID, Genres.Name as Name FROM Band_Genre INNER JOIN Genres ON Genres.ID=GenreID WHERE BandID=@BandID",
                Database.CreateParameter("@BandID", id));
            if (Festival.ISASP) return genres;
            ObservableCollection<Genre> genresResult = new ObservableCollection<Genre>();
            foreach(Genre genre in Festival.SingleFestival.Genres)
            {
                if (genres.Where(genre2=>genre2.ID==genre.ID).Count()>0)
                    genresResult.Add(genre);
            }
            return genresResult;
            
            
           
        }

        public static ObservableCollection<Genre> GetGenres()
        {
            return GetGenresByQuery("SELECT * FROM Genres ORDER BY Name");
        }

        public static ObservableCollection<Genre> GetGenresByQuery(string sql, params DbParameter[] parameters)
        {
            DbDataReader reader = null;
            try
            {
                ObservableCollection<Genre> genres = new ObservableCollection<Genre>();
                reader = Database.GetData(sql,parameters);
                while (reader.Read())
                    genres.Add(new Genre(reader));
                reader.Close();
                reader = null;
                return genres;
            }
            catch (Exception ex)
            {
                if (reader != null && !reader.IsClosed)
                {
                    reader.Close();
                    reader = null;
                }
                throw new Exception("Could not get genres by sql query", ex);
            }
        }

        public bool InsertIntoBand(Band SelectedItem)
        {
            //if (ID == null) Genre.Insert();
            try
            {
                string sql = "INSERT INTO Band_Genre (BandID, GenreID) VALUES(@BandID,@GenreID);";
                int i = Database.ModifyData(sql,
                    Database.CreateParameter("@GenreID", ID),
                    Database.CreateParameter("@BandID",SelectedItem.ID)
                    );


                if (i > 0)
                    return true;
                else
                    throw new Exception("Could not insert band_genre.");

            }
            catch (Exception ex)
            {
                throw new Exception("Could not insert band_genre.", ex);
            }
        }

        public bool Insert()
        {
            DbDataReader reader = null;
            try
            {
                string sql = "INSERT INTO Genres (Name) VALUES(@Name); SELECT SCOPE_IDENTITY() as 'ID'";
                reader = Database.GetData(sql,
                    Database.CreateParameter("@Name", Name)
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

        public bool DeleteFromBand(Band SelectedItem)
        {
            try
            {
                string sql = "DELETE FROM Band_Genre WHERE BandID=@BandID AND GenreID=@GenreID;";
                int i = Database.ModifyData(sql,
                    Database.CreateParameter("@GenreID", ID),
                    Database.CreateParameter("@BandID", SelectedItem.ID)
                    );


                if (i > 0)
                    return true;
                else
                    throw new Exception("Could not delete from band_genre.");

            }
            catch (Exception ex)
            {
                throw new Exception("Could not delete from band_genre.", ex);
            }
        }

        public void GetIDFromName()
        {
            DbDataReader reader = null;
            try
            {
                reader = Database.GetData("SELECT ID WHERE Name=@Name", Database.CreateParameter("@Name", Name));
                if (reader.Read())
                    ID = reader["ID"].ToString();
                else throw new Exception("Could not get ID by name");
            }
            catch (Exception ex)
            {
                throw new Exception("Could not insert band.", ex);
            }
        }

        public bool Update()
        {
            try{
                int amountOfModifiedRows = Database.ModifyData("UPDATE Genres SET Name=@Name WHERE ID=@ID",
                       Database.CreateParameter("@Name", this.Name),
                       Database.CreateParameter("@ID", ID));
                if (amountOfModifiedRows == 1)
                    return true;
                else return false;
            }
            catch (Exception ex)
            {
                throw new Exception("Could not edit the genre, me very sorry!", ex);
            }
        }
    }
}
