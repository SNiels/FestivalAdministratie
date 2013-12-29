using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Helper;

namespace FestivalLibAdmin.Class.Model
{
    public class UserProfile:ObservableValidationObject
    {

        #region ctors
        public UserProfile()
        {

        }

        public UserProfile(IDataRecord record)
        {
            ID = Convert.ToInt32(record["UserId"]);
            Email = Convert.IsDBNull(record["Email"]) ? null : record["Email"].ToString();
            UserName = record["UserName"].ToString();
        }

        #endregion

        #region props
        [ScaffoldColumn(false)]
        public int? ID { get; set; }
        
        [Required(ErrorMessage="Gelieve een gebruikersnaam in te geven")]
        [MinLength(2,ErrorMessage="De gebruikersnaam moet minstens 2 karakters bevatten")]
        [Display(Name = "Naam", Order = 0, Description = "Uw naam", GroupName = "Gebruiker",Prompt="bv. Barack Obama")]
        [DisplayFormat(ConvertEmptyStringToNull = true)]
        [Editable(true, AllowInitialValue = false)]
        public string UserName { get; set; }

        [DataType(DataType.EmailAddress,ErrorMessage="Gelieve een geldig email adres in te geven")]
        [RegularExpression(@"^\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,3}$",ErrorMessage="Gelieve een geldig email adres in te geven")]
        [EmailAddress(ErrorMessage = "Gelieve een geldig email adres in te geven")]
        [Display(Name = "Email", Order = 1, Description = "Uw email adres", GroupName = "Gebruiker", Prompt = "Bv. Barack.Obama@whitehouse.com")]
        [DisplayFormat(ConvertEmptyStringToNull = true)]
        [Editable(true, AllowInitialValue = false)]
        public string Email { get; set; }

        #endregion

        #region dal
        public ObservableCollection<UserProfile> GetUsers()
        {
            DbDataReader reader = null;
            try
            {
                ObservableCollection<UserProfile> users = new ObservableCollection<UserProfile>();
                reader = Database.GetData("SELECT * FROM UserProfile");
                while (reader.Read())
                    users.Add(new UserProfile(reader));
                reader.Close();
                reader = null;
                return users;
            }
            catch (Exception ex)
            {
                if (reader != null && !reader.IsClosed)
                {
                    reader.Close();
                    reader = null;
                }
                throw new Exception("Could not get users", ex);
            }
        }
        public bool Delete()
        {
            try
            {
                int i = Database.ModifyData("DELETE FROM UserProfile WHERE ID=@ID",
                    Database.CreateParameter("@ID", ID));
                if (i < 1) return false;
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception("Could not delete the damn user", ex);
            }
        }

        public bool Insert()
        {
            DbDataReader reader = null;
            try
            {
                string sql = "INSERT INTO UserProfile (UserName, Email) VALUES(@UserName, @Email); SELECT SCOPE_IDENTITY() as 'ID'";
                reader = Database.GetData(sql,
                    Database.CreateParameter("@UserName", UserName),
                    Database.CreateParameter("@Email", Email)
                    );


                if (reader.Read() && !Convert.IsDBNull(reader["ID"]))
                {
                    ID = Convert.ToInt32(reader["ID"]);
                    return true;
                }
                else
                    throw new Exception("Could not get the ID of the inserted user, it is possible the isert failed.");

            }
            catch (Exception ex)
            {
                if (reader != null) reader.Close();
                throw new Exception("Could not add user", ex);
            }
        }

        public bool Update()
        {
            try
            {
                int amountOfModifiedRows = Database.ModifyData("UPDATE UserProfile SET UserName=@UserName,Email=@Email WHERE UserId=@UserId",
                    Database.CreateParameter("@UserName", UserName),
                    Database.CreateParameter("@Email", Email),
                    Database.CreateParameter("@UserId", ID)
                    );
                if (amountOfModifiedRows == 1)
                    return true;
                else return false;
            }
            catch (Exception ex)
            {
                throw new Exception("Could not edit the user, me very sorry!", ex);
            }
        }

        public static UserProfile GetUserByID(int id)
        {
            DbDataReader reader = null;
            try
            {
                reader = Database.GetData("SELECT * FROM UserProfile WHERE UserId=@UserId",
                    Database.CreateParameter("@UserId",id));
                UserProfile user=null;
                if(reader.Read())
                     user=new UserProfile(reader);
                reader.Close();
                reader = null;
                return user;
            }
            catch (Exception ex)
            {
                if (reader != null && !reader.IsClosed)
                {
                    reader.Close();
                    reader = null;
                }
                throw new Exception("Could not get user by id="+id, ex);
            }
        }

        #endregion
    }

}
