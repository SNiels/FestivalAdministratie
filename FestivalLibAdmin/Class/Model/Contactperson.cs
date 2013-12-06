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
    public class Contactperson:PortableClassLibrary.Model.Contactperson,IDataErrorInfo
    {     
        [DataType(DataType.Text)]
        [MinLength(2,ErrorMessage="Een naam moet minimum 2 karakters zijn.")]
        public override string Name
        {
            get
            {
                return base.Name;
            }
            set
            {
                base.Name = value;
                OnPropertyChanged("Name");
            }
        }

        public override string Company
        {
            get
            {
                return base.Company;
            }
            set
            {
                base.Company = value;
                OnPropertyChanged("Company");
            }
        }

        public override PortableClassLibrary.Model.ContactpersonType JobRole
        {
            get
            {
                return base.JobRole;
            }
            set
            {
                base.JobRole = value;
                OnPropertyChanged("JobRole");
            }
        }

        public override string City
        {
            get
            {
                return base.City;
            }
            set
            {
                base.City = value;
                OnPropertyChanged("City");
            }
        }

        public override string Email
        {
            get
            {
                return base.Email;
            }
            set
            {
                base.Email = value;
                OnPropertyChanged("Email");
            }
        }

        public override string Phone
        {
            get
            {
                return base.Phone;
            }
            set
            {
                base.Phone = value;
                OnPropertyChanged("Phone");
            }
        }

        public override string Cellphone
        {
            get
            {
                return base.Cellphone;
            }
            set
            {
                base.Cellphone = value;
                OnPropertyChanged("Cellphone");
            }
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
