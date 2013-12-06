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
    public class Contactperson:ObservableValidationObject
    {     
        
        public Contactperson()
        {

        }

        private string _id;

        public virtual string ID
        {
            get { return _id; }
            set { _id = value; }
        }

        private string _name;
        [DataType(DataType.Text)]
        [MinLength(2, ErrorMessage = "Een naam moet minimum 2 karakters zijn.")]
        public virtual string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        private string _company;

        public virtual string Company
        {
            get { return _company; }
            set
            {
                _company = value;
                OnPropertyChanged("Company");
            }
        }

        private ContactpersonType _jobRole;

        public virtual ContactpersonType JobRole
        {
            get { return _jobRole; }
            set
            {
                _jobRole = value;
                OnPropertyChanged("JobRole");
            }
        }

        private string _city;

        public virtual string City
        {
            get { return _city; }
            set
            {
                _city = value;
                OnPropertyChanged("City");
            }
        }

        private String _email;

        public virtual String Email
        {
            get { return _email; }
            set
            {
                _email = value;
                OnPropertyChanged("Email");
            }
        }

        private string _phone;

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

        public virtual string Cellphone
        {
            get { return _cellphone; }
            set
            {
                _cellphone = value;
                OnPropertyChanged("Cellphone");
            }
        } 
    }
}
