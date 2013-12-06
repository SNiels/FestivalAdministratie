using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FestivalLibAdmin.Model
{
    public class TicketType:ObservableValidationObject
    {
        static TicketType()
        {
            Types = new ObservableCollection<TicketType>();
        }

        private string _id;

        public string ID
        {
            get { return _id; }
            set { _id = value; }
        }

        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private double _price;

        public double Price
        {
            get { return _price; }
            set { _price = value; }
        }

        private int _availableTickets;

        public int AvailableTickets
        {
            get { return _availableTickets; }
            set { _availableTickets = value; }
        }

        private int _amountOfTickets;

        public int AmountOfTickets
        {
            get { return _amountOfTickets; }
            set { _amountOfTickets = value; }
        }

        private static ObservableCollection<TicketType> _types;

        public static ObservableCollection<TicketType> Types
        {
            get { return _types; }
            set { _types = value; }
        }
    }
}
