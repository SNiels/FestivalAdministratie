using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortableClassLibrary.Model
{
    public class Ticket:ObservableObject
    {

        #region props

        private string _id;

        public virtual string ID
        {
            get { return _id; }
            set { _id = value; }
        }

        private string _ticketHolder;

        public virtual string TicketHolder
        {
            get { return _ticketHolder; }
            set { _ticketHolder = value;
            OnPropertyChanged("TicketHolder");
            }
        }

        private string _ticketHolderEmail;

        public virtual string TicketHolderEmail
        {
            get { return _ticketHolderEmail; }
            set { _ticketHolderEmail = value;
            OnPropertyChanged("TicketHolderEmail");
            }
        }

        private int _amount;

        public virtual int Amount
        {
            get { return _amount; }
            set { _amount = value;
            OnPropertyChanged("Amount");
            }
        }

        private TicketType _type;

        public virtual TicketType Type
        {
            get { return _type; }
            set { _type = value;
            OnPropertyChanged("Type");
            }
        }

        //private static ObservableCollection<Ticket> _tickets;
        //public static ObservableCollection<Ticket> Tickets
        //{
        //    get { return _tickets; }
        //    set { _tickets = value; }
        //}

        #endregion

    }
}
