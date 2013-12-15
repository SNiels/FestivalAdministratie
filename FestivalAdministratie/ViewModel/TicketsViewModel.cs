using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using FestivalLibAdmin.Model;
using GalaSoft.MvvmLight.Command;

namespace FestivalAdministratie.ViewModel
{
    public class TicketsViewModel:PortableClassLibrary.ObservableObject,IPage
    {
        private ObservableCollection<Ticket> _tickets;

        public ObservableCollection<Ticket> Tickets
        {
            get
            {
                if (_tickets != null) return _tickets;
                try
                {
                    _tickets = Festival.SingleFestival.Tickets;
                    IsTicketsEnabled = true;
                    foreach(var ticket in _tickets)
                        ticket.PropertyChanged += ticket_PropertyChanged;
                    _tickets.CollectionChanged += tickets_CollectionChanged;
                    return _tickets;
                }catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    MessageBox.Show("Tickets konden niet uit de database gehaald worden.");
                    IsTicketsEnabled = false;
                    return null;
                }
            }
            //set
            //{
            //    Festival.SingleFestival.Tickets = value;
            //    OnPropertyChanged("Tickets");
            //}
        }

        void tickets_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
                foreach (Ticket newitem in e.NewItems)
                {
                    try
                    {
                        newitem.PropertyChanged += ticket_PropertyChanged;
                        if (newitem.ID == null && newitem.IsValid() && newitem.Insert()) newitem.Type.TicketsSold= newitem.Type.GetAmountOfSoldTickets();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        //MessageBox.Show("Kon ticket niet in de database steken");
                    }
                }
            if (e.OldItems != null)
                foreach (Ticket olditem in e.OldItems)
                {
                    if (olditem.ID == null) return;
                    try
                    {
                        if (olditem.Delete())
                        {
                            olditem.PropertyChanged -= ticket_PropertyChanged;
                            olditem.Type.TicketsSold = olditem.Type.GetAmountOfSoldTickets();
                            olditem.ID = null;
                        }
                        else throw new Exception("Could not remove ticket");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        MessageBox.Show("Kon bestelling niet verwijderen in de database");
                    }
                }
        }

        void ticket_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Ticket ticket = sender as Ticket;
            if (e.PropertyName == "TicketHolderEmail" || e.PropertyName == "TicketHolder")
            {
                InsertOrUpdateUserProfile(ticket);
                if(ticket.ID!=null)
                return;
            }
            if (ticket.IsValid())
            {
                if (ticket.ID != null)
                    try
                    {
                        if (!ticket.Update()) throw new Exception("Could not update ticket");
                        else if (e.PropertyName == "Amount") ticket.Type.TicketsSold = ticket.Type.GetAmountOfSoldTickets();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        MessageBox.Show("Kon ticket niet updaten naar de database");
                    }
                else
                    try
                    {
                        if (!ticket.Insert()) throw new Exception("Could not insert ticket");
                        ticket.Type.TicketsSold = ticket.Type.GetAmountOfSoldTickets();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        MessageBox.Show("Kon bestelling niet in de database steken");
                    }
            }
        }

        private void InsertOrUpdateUserProfile(Ticket ticket)
        {
            if (ticket.IsValid()&&ticket.TicketHolderProfile.IsValid())
            {
                if (ticket.TicketHolderProfile.ID != null)
                    try
                    {
                        if (!ticket.TicketHolderProfile.Update()) throw new Exception("Could not update ticket");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        MessageBox.Show("Kon besteller gegevens niet updaten naar de database");
                    }
                else
                    try
                    {
                        if (!ticket.TicketHolderProfile.Insert()) throw new Exception("Could not insert ticketholderprofile");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        MessageBox.Show("Kon besteller niet in de database steken");
                    }
            }
        }

        private bool _isTicketsEnabled;

        public bool IsTicketsEnabled
        {
            get { return _isTicketsEnabled; }
            set { _isTicketsEnabled = value;
            OnPropertyChanged("IsTicketsEnabled");
            }
        }


        private ObservableCollection<TicketType> _types;

        public ObservableCollection<TicketType> TicketTypes
        {
            get {
                if (_types != null) return _types;
                try
                {
                    _types = Festival.SingleFestival.TicketTypes;
                    if(_types.Count() > 0) SelectedTicketType = _types.First();
                    IsTypesEnabled = true;
                    foreach (var type in _types)
                        type.PropertyChanged += type_PropertyChanged;
                    _types.CollectionChanged += types_CollectionChanged;
                    return _types;
                }catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    MessageBox.Show("Type tickets konden niet uit de database gehaald worden.");
                    IsTypesEnabled = false;
                    return null;
                }
            }
            //set { Festival.SingleFestival.TicketTypes = value;
            //OnPropertyChanged("TicketTypes");
            //}
        }

        void types_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if(e.NewItems!=null)
                foreach(TicketType type in e.NewItems)
                {
                    try
                    {
                        type.PropertyChanged+=type_PropertyChanged;
                        if (type.ID == null && type.IsValid()) type.Insert();
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        MessageBox.Show("Kon type ticket niet in database steken");
                    }
                }
            if(e.OldItems!=null)
                foreach(TicketType type in e.OldItems)
                {
                    if (type.ID == null) return;
                    try
                    {
                        if (type.Delete())
                        {
                            type.PropertyChanged -= type_PropertyChanged;
                            type.ID = null;
                        }
                        else throw new Exception("Could not remove type");
                    }catch(Exception ex){
                        Console.WriteLine(ex.Message);
                        MessageBox.Show("Kon type ticket niet verwijderen, zorg ervoor dat er geen bestellingen meer zijn van het te verwijderen type.");
                    }
                    
                }
        }

        void type_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "AvailableTickets") return;
            TicketType type = sender as TicketType;
            if(type.IsValid())
            {
                if(type.ID!=null)
                    try
                    {
                        if (!type.Update()) throw new Exception("Could not update ticket type");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        MessageBox.Show("Kon type ticket niet aanpassen in de database");
                    }
                else
                    try
                    {
                        if (!type.Insert()) throw new Exception("Could not insert ticket type");

                    }catch(Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        MessageBox.Show("Kon type ticket niet in de database steken");
                    }
            }
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
            OnPropertyChanged("IsATypeSelected");
            }
        }

        private bool _isTypesEnabled;

        public bool IsTypesEnabled
        {
            get { return _isTypesEnabled; }
            set { _isTypesEnabled = value;
            OnPropertyChanged("IsTypesEnabled");
            }
        }


        public bool IsATypeSelected
        {
            get { return SelectedTicketType != null; }
        }

        public ICommand AddReservationCommand
        {
            get
            {
                return new RelayCommand(AddReservation);
            }
        }

        private void AddReservation()
        {
            TicketType type=null;
            if(TicketTypes.Count()>0)
            type = TicketTypes.First();
            Tickets.Add(new Ticket()
            {
                Amount = 1,
                TicketHolder = "Nieuwe koper",
                Type = type
            });
        }

        private bool _isDialogVisible;
        public bool IsDialogVisible
        {
            get
            {
                return _isDialogVisible;
            }
            set
            {
                _isDialogVisible = value;
                if (value == true) DialogVisibility = System.Windows.Visibility.Visible;
                else DialogVisibility = System.Windows.Visibility.Collapsed;
                OnPropertyChanged("IsDialogVisible");
            }
        }

        private void SubmitDialog()
        {
            IsDialogVisible = false;
        }

        public ICommand OpenDialogCommand
        {
            get { return new RelayCommand(OpenDialog); }
        }

        private void OpenDialog()
        {
            IsDialogVisible = true;
        }

        public ICommand SubmitDialogResultCommand
        {
            get { return new RelayCommand(SubmitDialog); }
        }

        private System.Windows.Visibility _dialogVisibility = System.Windows.Visibility.Collapsed;

        public System.Windows.Visibility DialogVisibility
        {
            get
            {
                return _dialogVisibility;
            }
            set
            {
                _dialogVisibility = value;
                OnPropertyChanged("DialogVisibility");
            }
        }
    }
}
