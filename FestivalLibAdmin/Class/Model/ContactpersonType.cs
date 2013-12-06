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
    public class ContactpersonType:PortableClassLibrary.Model.ContactpersonType, IDataErrorInfo
    {
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
