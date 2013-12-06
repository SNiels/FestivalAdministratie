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

        public virtual string ID
        {
            get { return _id; }
            set { _id = value; }
        }

        private string _name;
        public ContactpersonType()
        {

        }
        public ContactpersonType(IDataRecord record)
        {
                ID = record["ID"].ToString();
                Name = record["Name"].ToString();
        }

        public virtual string Name
        {
            get { return _name; }
            set { _name = value; }
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
    }
}
