using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortableClassLibrary.Model
{
    public class TicketType:ObservableObject
    {
        //static TicketType()
        //{
        //    Types = new ObservableCollection<TicketType>();
        //}

        private string _id;

        public virtual string ID
        {
            get { return _id; }
            set { _id = value; }
        }

        private string _name;

        public virtual string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private double _price;

        public virtual double Price
        {
            get { return _price; }
            set { _price = value; }
        }

        private int _availableTickets;

        public virtual int AvailableTickets
        {
            get { return _availableTickets; }
            set { _availableTickets = value; }
        }

        private int _amountOfTickets;

        public virtual int AmountOfTickets
        {
            get { return _amountOfTickets; }
            set { _amountOfTickets = value; }
        }

        //private static ObservableCollection<TicketType> _types;

        //public static ObservableCollection<TicketType> Types
        //{
        //    get { return _types; }
        //    set { _types = value; }
        //}
    }
}
