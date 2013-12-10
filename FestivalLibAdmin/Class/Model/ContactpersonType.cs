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
    public class ContactpersonType:ObservableValidationObject
    {
        private string _id;
        [ScaffoldColumn(false)]
        public string ID
        {
            get { return _id; }
            set { _id = value; }
        }

        public ContactpersonType()
        {

        }
        public ContactpersonType(IDataRecord record)
        {
                ID = record["ID"].ToString();
                Name = record["Name"].ToString();
        }

        private string _name;

        [Required(ErrorMessage="Gelieve een naam in te vullen")]
        public string Name
        {
            get { return _name; }
            set { _name = value;
            OnPropertyChanged("Name");
            }
        }

        public static ObservableCollection<ContactpersonType> GetContactTypes()
        {
            DbDataReader reader = null;
            try
            {
                ObservableCollection<ContactpersonType> contactTypes = new ObservableCollection<ContactpersonType>();
                reader = Database.GetData("SELECT * FROM ContactpersonTypes");
                while (reader.Read())
                    contactTypes.Add(new ContactpersonType(reader));
                reader.Close();
                reader = null;
                return contactTypes;
            }
            catch (Exception ex)
            {
                if (reader != null && !reader.IsClosed)
                {
                    reader.Close();
                    reader = null;
                }
                throw new Exception("Could not get contactTypes", ex);
            }
        }

        public bool Update()
        {
            try
            {
                int amountOfModifiedRows = Database.ModifyData("UPDATE ContactpersonTypes SET Name=@Name WHERE ID=@ID",
                    Database.CreateParameter("@Name", Name),
                    Database.CreateParameter("@ID", ID)
                    );
                if (amountOfModifiedRows == 1)
                    return true;
                else return false;
            }
            catch (Exception ex)
            {
                throw new Exception("Could not edit the contactType, me very sorry!", ex);
            }
        }

        public bool Insert()
        {
            DbDataReader reader = null;
            try
            {
                string sql = "INSERT INTO ContactpersonTypes (Name) VALUES(@Name); SELECT SCOPE_IDENTITY() as 'ID'";
                reader = Database.GetData(sql,
                    Database.CreateParameter("@Name", Name)
                    );


                if (reader.Read() && !Convert.IsDBNull(reader["ID"]))
                {
                    ID = reader["ID"].ToString();
                    return true;
                }
                else
                    throw new Exception("Could not get the ID of the inserted contacttype, it is possible the isert failed.");

            }
            catch (Exception ex)
            {
                if (reader != null) reader.Close();
                throw new Exception("Could not instert contactperson", ex);
            }
        }

        public bool Delete()
        {
            try
            {
                int i = Database.ModifyData("DELETE FROM ContactpersonTypes WHERE ID=@ID",
                    Database.CreateParameter("@ID", ID));
                if (i < 1) return false;
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception("Could not delete the damn contact", ex);
            }
        }
    }
}
