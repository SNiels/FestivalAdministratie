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
    public class TicketType:PortableClassLibrary.Model.TicketType,IDataErrorInfo
    {
        //static TicketType()
        //{
        //    Types = new ObservableCollection<TicketType>();
        //}

        //private string _id;

        //public string ID
        //{
        //    get { return _id; }
        //    set { _id = value; }
        //}

        //private string _name;

        //public string Name
        //{
        //    get { return _name; }
        //    set { _name = value; }
        //}

        //private double _price;

        //public double Price
        //{
        //    get { return _price; }
        //    set { _price = value; }
        //}

        //private int _availableTickets;

        //public int AvailableTickets
        //{
        //    get { return _availableTickets; }
        //    set { _availableTickets = value; }
        //}

        //private int _amountOfTickets;

        //public int AmountOfTickets
        //{
        //    get { return _amountOfTickets; }
        //    set { _amountOfTickets = value; }
        //}

        //private static ObservableCollection<TicketType> _types;

        //public static ObservableCollection<TicketType> Types
        //{
        //    get { return _types; }
        //    set { _types = value; }
        //}

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
