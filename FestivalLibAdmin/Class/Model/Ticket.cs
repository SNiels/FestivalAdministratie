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
using FestivalLibAdmin.Class.Model;
using Helper;

namespace FestivalLibAdmin.Model
{
    public class Ticket:ObservableValidationObject
    {

        public Ticket(IDataRecord record)
        {
            ID = record["ID"].ToString();
            Amount = Convert.ToInt32(record["Amount"]);
            Type = Festival.SingleFestival.TicketTypes.Where(type => type.ID == record["TicketType"].ToString()).First();
            TicketHolderProfile = UserProfile.GetUserByID(Convert.ToInt32(record["UserId"]));
            OnPropertyChanged("TicketHolder");
            OnPropertyChanged("TicketHolderEmail");
        }

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
        private string _id;

        public virtual string ID
        {
            get { return _id; }
            set { _id = value; }
        }

        private UserProfile _ticketHolderProfile;

        public UserProfile TicketHolderProfile
        {
            get { return _ticketHolderProfile; }
            set { _ticketHolderProfile = value;
            OnPropertyChanged("TicketHolderProfile");
            }
        }
        
        public virtual string TicketHolder
        {
            get {
                if (TicketHolderProfile != null)
                    return TicketHolderProfile.UserName;
                return null;
            }
            set
            {
                TicketHolderProfile.UserName = value;
                OnPropertyChanged("TicketHolder");
            }
        }

        public virtual string TicketHolderEmail
        {
            get
            {
                if (TicketHolderProfile != null)
                    return TicketHolderProfile.Email;
                return null;
            }
            set
            {
                TicketHolderProfile.Email = value;
                OnPropertyChanged("TicketHolderEmail");
            }
        }

        private int _amount;

        public virtual int Amount
        {
            get { return _amount; }
            set
            {
                _amount = value;
                OnPropertyChanged("Amount");
            }
        }

        private TicketType _type;

       

        public virtual TicketType Type
        {
            get { return _type; }
            set
            {
                _type = value;
                OnPropertyChanged("Type");
            }
        }

        public static ObservableCollection<Ticket> GetTickets()
        {
            DbDataReader reader = null;
            try
            {
                ObservableCollection<Ticket> tickets = new ObservableCollection<Ticket>();
                reader = Database.GetData("SELECT * FROM Tickets");
                while (reader.Read())
                    tickets.Add(new Ticket(reader));
                reader.Close();
                reader = null;
                return tickets;
            }
            catch (Exception ex)
            {
                if (reader != null && !reader.IsClosed)
                {
                    reader.Close();
                    reader = null;
                }
                throw new Exception("Could not get tickets", ex);
            }
        }
    }
}
