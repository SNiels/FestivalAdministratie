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
    public class Contactperson:ObservableValidationObject
    {
        #region ctors
        public Contactperson()
        {
        }

        public Contactperson(IDataRecord record)
        {
            ID = record["ID"].ToString();
            Name = !Convert.IsDBNull(record["Name"]) ? record["Name"].ToString() : null;
            Company = !Convert.IsDBNull(record["Company"]) ? record["Company"].ToString() : null;
            if (!Convert.IsDBNull(record["JobRole"]))
                JobRole = Festival.SingleFestival.ContactTypes.Where(type => type.ID == record["JobRole"].ToString()).First();
            else JobRole = null;
            City = !Convert.IsDBNull(record["City"]) ? record["City"].ToString() : null;
            Email = !Convert.IsDBNull(record["Email"]) ? record["Email"].ToString() : null;
            Phone = !Convert.IsDBNull(record["Phone"]) ? record["Phone"].ToString() : null;
            Cellphone = !Convert.IsDBNull(record["Cellphone"]) ? record["Cellphone"].ToString() : null;
        }

        public Contactperson(bool p)
        {
            if (p == true)
                _name = "Nieuw contact";
        }

        #endregion

        #region props

        private string _id;
        [ScaffoldColumn(false)]
        public string ID
        {
            get { return _id; }
            set { _id = value; }
        }

        private string _name;
        [Required(ErrorMessage="Gelieve de naam in te vullen")]
        [MinLength(2, ErrorMessage = "Een naam moet minimum 2 karakters zijn.")]
        [Display(Name = "Naam", Order = 0, Description = "De naam van de contactpersoon", GroupName = "Contactpersoon",Prompt="Bv: Barack Obama")]
        [DisplayFormat(ConvertEmptyStringToNull = true)]
        [Editable(true, AllowInitialValue = false)]
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        private string _company;
        [Display(Name = "Bedrijf", Order = 1, Description = "De naam van het bedrijf", GroupName = "Contactpersoon")]
        [DisplayFormat(ConvertEmptyStringToNull = true)]
        public string Company
        {
            get { return _company; }
            set
            {
                _company = value;
                OnPropertyChanged("Company");
            }
        }

        private ContactpersonType _jobRole;
        [Display(Name = "Functie", Order = 2, Description = "Het beroep van de contactpersoon", GroupName = "Contactpersoon", Prompt="Bv: Student")]
        [DisplayFormat(ConvertEmptyStringToNull = true)]
        public ContactpersonType JobRole
        {
            get { return _jobRole; }
            set
            {
                _jobRole = value;
                OnPropertyChanged("JobRole");
            }
        }

        private string _city;
        [Display(Name = "Stad", Order = 3, Description = "De stad van de contactpersoon", GroupName = "Contactpersoon")]
        [DisplayFormat(ConvertEmptyStringToNull = true)]
        public string City
        {
            get { return _city; }
            set
            {
                _city = value;
                OnPropertyChanged("City");
            }
        }

        private String _email;
        [DataType(DataType.EmailAddress,ErrorMessage="Gelieve een geldig email adres in te geven")]
        [Display(Name = "Email", Order = 4, Description = "Het emailadres van de contactpersoon", GroupName = "Contactpersoon")]
        [DisplayFormat(ConvertEmptyStringToNull = true)]
        [EmailAddress(ErrorMessage = "Gelieve een geldig email adres in te geven")]
        public String Email
        {
            get { return _email; }
            set
            {
                _email = value;
                OnPropertyChanged("Email");
            }
        }

        private string _phone;
        [Display(Name = "Telefoon", Order = 5, Description = "Het telefoon nummer van de contactpersoon", GroupName = "Contactpersoon")]
        [DisplayFormat(ConvertEmptyStringToNull = true)]
        [Phone(ErrorMessage="Gelieve een geldige telefoon nummer in te geven")]
        public virtual string Phone
        {
            get { return _phone; }
            set
            {
                _phone = value;
                OnPropertyChanged("Phone");
            }
        }

        private string _cellphone;
        [Display(Name = "Gsm nummer", Order = 6, Description = "Het gsm nummer van de contactpersoon", GroupName = "Contactpersoon")]
        [DisplayFormat(ConvertEmptyStringToNull = true)]
        [Phone(ErrorMessage = "Gelieve een geldige gsm nummer in te geven")]
        [RegularExpression(@"(^\+[0-9]{2}|^\+[0-9]{2}\(0\)|^\(\+[0-9]{2}\)\(0\)|^00[0-9]{2}|^0)([0-9]{9}$|[0-9\-\s]{10}$)",ErrorMessage="Gelieve een geldige gsm nummer in te geven (+31235256677 | +31(0)235256677 | 023-5256677)")]
        public string Cellphone
        {
            get { return _cellphone; }
            set
            {
                _cellphone = value;
                OnPropertyChanged("Cellphone");
            }
        }

        #endregion

        #region dal

        public static ObservableCollection<Contactperson> GetContacts()
        {
            DbDataReader reader = null;
            try
            {
                ObservableCollection<Contactperson> contacts = new ObservableCollection<Contactperson>();
                reader = Database.GetData("SELECT * FROM Contactpersons");
                while (reader.Read())
                    contacts.Add(new Contactperson(reader));
                reader.Close();
                reader = null;
                return contacts;
            }
            catch (Exception ex)
            {
                if (reader != null && !reader.IsClosed)
                {
                    reader.Close();
                    reader = null;
                }
                throw new Exception("Could not get contacts", ex);
            }
        }


        public bool Update()
        {
            try
            {
                int amountOfModifiedRows = Database.ModifyData("UPDATE Contactpersons SET Name=@Name,Company=@Company,JobRole=@JobRole,City=@City,Email=@Email,Phone=@Phone,Cellphone=@Cellphone WHERE ID=@ID",
                    Database.CreateParameter("@Name", Name),
                    Database.CreateParameter("@Company", Company),
                    Database.CreateParameter("@JobRole", JobRole == null ? null : JobRole.ID),
                    Database.CreateParameter("@City", City),
                    Database.CreateParameter("@Email", Email),
                    Database.CreateParameter("@Phone", Phone),
                    Database.CreateParameter("@Cellphone", Cellphone),
                    Database.CreateParameter("@ID", ID)
                    );
                if (amountOfModifiedRows == 1)
                    return true;
                else return false;
            }
            catch (Exception ex)
            {
                throw new Exception("Could not edit the contact, me very sorry!", ex);
            }
        }

        public bool Insert()
        {
            DbDataReader reader = null;
            try
            {
                string jobid = null;
                if (JobRole != null)
                    jobid = JobRole.ID;
                string sql = "INSERT INTO Contactpersons (Name, Company, JobRole, City, Email, Phone, Cellphone) VALUES(@Name, @Company, @JobRole, @City, @Email, @Phone, @Cellphone); SELECT SCOPE_IDENTITY() as 'ID'";
                reader = Database.GetData(sql,
                    Database.CreateParameter("@Name", Name),
                    Database.CreateParameter("@Company", Company),
                    Database.CreateParameter("@JobRole", jobid),
                    Database.CreateParameter("@City", City),
                    Database.CreateParameter("@Email", Email),
                    Database.CreateParameter("@Phone", Phone),
                    Database.CreateParameter("@Cellphone", Cellphone)
                    );


                if (reader.Read() && !Convert.IsDBNull(reader["ID"]))
                {
                    ID = reader["ID"].ToString();
                    return true;
                }
                else
                    throw new Exception("Could not get the ID of the inserted contact, it is possible the isert failed.");

            }
            catch (Exception ex)
            {
                if (reader != null) reader.Close();
                throw new Exception("Could not add contact.", ex);
            }
        }


        public bool Delete()
        {
            try
            {
                int i = Database.ModifyData("DELETE FROM Contactpersons WHERE ID=@ID",
                    Database.CreateParameter("@ID",ID));
                if (i < 1) return false;
                return true;
            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception("Could not delete the damn contact", ex);
            }
        }

        #endregion

        public override string ToString()
        {
            return Name;
        }
    }
}
