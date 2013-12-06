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
    public class TicketType:ObservableValidationObject
    {
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
    }
}
