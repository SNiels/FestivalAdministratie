using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FestivalLibAdmin.Model;

namespace FestivalAdministratie.Model
{
    public class Festival : FestivalLibAdmin.Model.Festival
    {
        public override ObservableCollection<FestivalLibAdmin.Model.Band> Bands
        {
            get
            {
                return base.Bands;
            }
            set
            {
                base.Bands = value;
            }
        }

        public override ObservableCollection<Contactperson> ContactPersons
        {
            get
            {
                return base.ContactPersons;
            }
            set
            {
                base.ContactPersons = value;
            }
        }

        public override ObservableCollection<ContactpersonType> ContactTypes
        {
            get
            {
                return base.ContactTypes;
            }
            set
            {
                base.ContactTypes = value;
            }
        }

        public override ObservableCollection<Genre> Genres
        {
            get
            {
                return base.Genres;
            }
            set
            {
                base.Genres = value;
            }
        }

        public override ObservableCollection<LineUp> LineUps
        {
            get
            {
                return base.LineUps;
            }
            set
            {
                base.LineUps = value;
            }
        }

        public override ObservableCollection<Optreden> Optredens
        {
            get
            {
                return base.Optredens;
            }
            set
            {
                base.Optredens = value;
            }
        }

        public override ObservableCollection<Stage> Stages
        {
            get
            {
                return base.Stages;
            }
            set
            {
                base.Stages = value;
            }
        }

        public override ObservableCollection<Ticket> Tickets
        {
            get
            {
                return base.Tickets;
            }
            set
            {
                base.Tickets = value;
            }
        }

        public override ObservableCollection<TicketType> TicketTypes
        {
            get
            {
                return base.TicketTypes;
            }
            set
            {
                base.TicketTypes = value;
            }
        }

        //public ObservableCollection<LineUp> LineUps
        //{
        //    get { return LineUp.LineUps; }
        //    set
        //    {
        //        LineUp.LineUps = value;
        //        OnPropertyChanged("LineUps");
        //    }
        //}

        //public ObservableCollection<Band> Bands
        //{
        //    get { return Band.Bands; }
        //    set
        //    {
        //        Band.Bands = value;
        //        OnPropertyChanged("Bands");
        //    }
        //}

        //public ObservableCollection<Stage> Stages
        //{
        //    get { return Stage.Stages; }
        //    set
        //    {
        //        Stage.Stages = value;
        //        OnPropertyChanged("Stages");
        //    }
        //}

        //public ObservableCollection<ContactpersonType> ContactTypes
        //{
        //    get { return ContactpersonType.Types; }
        //    set
        //    {
        //        ContactpersonType.Types = value;
        //        OnPropertyChanged("ContactTypes");
        //    }
        //}

        //public ObservableCollection<Contactperson> ContactPersons
        //{
        //    get { return Contactperson.Contacten; }
        //    set
        //    {
        //        Contactperson.Contacten= value;
        //        OnPropertyChanged("ContactPersons");
        //    }
        //}

        //public ObservableCollection<Genre> Genres
        //{
        //    get { return Genre.Genres; }
        //    set
        //    {
        //        Genre.Genres= value;
        //        OnPropertyChanged("Genres");
        //    }
        //}

        //public ObservableCollection<Ticket> Tickets
        //{
        //    get { return Ticket.Tickets; }
        //    set
        //    {
        //        Ticket.Tickets = value;
        //        OnPropertyChanged("Tickets");
        //    }
        //}

        //public ObservableCollection<TicketType> TicketTypes
        //{
        //    get { return TicketType.Types; }
        //    set
        //    {
        //        TicketType.Types = value;
        //        OnPropertyChanged("TicketTypes");
        //    }
        //}

        //public ObservableCollection<Optreden> Optredens
        //{
        //    get { return Optreden.Optredens; }
        //    set
        //    {
        //        Optreden.Optredens = value;
        //        OnPropertyChanged("Optredens");
        //    }
        //}
    }
}
