using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FestivalLibAdmin.Model;

namespace FestivalAdministratie.ViewModel
{
    public class TicketsViewModel:PortableClassLibrary.ObservableObject,IPage
    {
        //private ObservableCollection<Ticket> _tickets;

        public ObservableCollection<Ticket> Tickets
        {
            get { return Festival.SingleFestival.Tickets; }
            set
            {
                Festival.SingleFestival.Tickets = value;
                OnPropertyChanged("Tickets");
            }
        }

        //private ObservableCollection<TicketType> _ticketTypes;

        public ObservableCollection<TicketType> TicketTypes
        {
            get { return Festival.SingleFestival.TicketTypes; }
            //set { Festival.SingleFestival.TicketTypes = value;
            //OnPropertyChanged("TicketTypes");
            //}
        }

        public string Name
        {
            get { return "Tickets"; }
        }

        private TicketType _selectedTicketType;

        public TicketType SelectedTicketType
        {
            get { return _selectedTicketType; }
            set { _selectedTicketType = value;
            OnPropertyChanged("SelectedTicketType");
            }
        }
        
    }
}
