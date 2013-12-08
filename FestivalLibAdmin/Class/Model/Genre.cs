using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Common;
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

        public string ID
        {
            get { return _id; }
            set { _id = value; }
        }

        private string _name;

        
        [Required]
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
            return GetGenresByQuery("SELECT Genres.ID as ID, Genres.Name as Name FROM Band_Genre INNER JOIN Genres ON Genres.ID=GenreID WHERE BandID=@BandID",
                Database.CreateParameter("@BandID", id));
        }

        public static ObservableCollection<Genre> GetGenres()
        {
            return GetGenresByQuery("SELECT * FROM Genres");
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
    }
}
