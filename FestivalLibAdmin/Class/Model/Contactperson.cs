﻿using System;
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
        static Contactperson()
        {
            Contacten = new ObservableCollection<Contactperson>();
        }
        public Contactperson()
        {
            
        }

        private static ObservableCollection<Contactperson> _contacten;

        public static ObservableCollection<Contactperson> Contacten
        {
            get { return _contacten; }
            set { _contacten = value;}
        }
        

        private string _id;

        public string ID
        {
            get { return _id; }
            set { _id = value; }
        }

        private string _name;
        [DataType(DataType.Text)]
        [MinLength(2,ErrorMessage="Een naam moet minimum 2 karakters zijn.")]
        public string Name
        {
            get { return _name; }
            set { _name = value;
            OnPropertyChanged("Name");
            }
        }

        private string _company;

        public string Company
        {
            get { return _company; }
            set { _company = value;
            OnPropertyChanged("Company");
            }
        }

        private ContactpersonType _jobRole;

        public ContactpersonType JobRole
        {
            get { return _jobRole; }
            set { _jobRole = value;
            OnPropertyChanged("JobRole");
            }
        }

        private string _city;

        public string City
        {
            get { return _city; }
            set { _city = value;
            OnPropertyChanged("City");
            }
        }

        private String _email;

        public String Email
        {
            get { return _email; }
            set { _email = value;
            OnPropertyChanged("Email");
            }
        }

        private string _phone;

        public string Phone
        {
            get { return _phone; }
            set { _phone = value;
            OnPropertyChanged("Phone");
            }
        }

        private string _cellphone;

        public string Cellphone
        {
            get { return _cellphone; }
            set { _cellphone = value;
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
