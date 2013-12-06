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
    public class Ticket:PortableClassLibrary.Model.Ticket,IDataErrorInfo
    {
        //static Ticket()
        //{
        //    Tickets = new ObservableCollection<Ticket>();
        //}

        //private string _id;

        //public string ID
        //{
        //    get { return _id; }
        //    set { _id = value; }
        //}

        //private int _userId;

        //public int UserId
        //{
        //    get { return _userId; }
        //    set { _userId = value; }
        //}
        

        //private string _ticketHolder;

        //public string TicketHolder
        //{
        //    get { return _ticketHolder; }
        //    set { _ticketHolder = value; }
        //}

        //private string _ticketHolderEmail;

        //public string TicketHolderEmail
        //{
        //    get { return _ticketHolderEmail; }
        //    set { _ticketHolderEmail = value; }
        //}

        //private int _amount;

        //public int Amount
        //{
        //    get { return _amount; }
        //    set { _amount = value; }
        //}


        //private TicketType _type;

        //public TicketType Type
        //{
        //    get { return _type; }
        //    set { _type = value; }
        //}

        //private static ObservableCollection<Ticket> _tickets;
        //public static ObservableCollection<Ticket> Tickets
        //{
        //    get { return _tickets; }
        //    set { _tickets = value; }
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
