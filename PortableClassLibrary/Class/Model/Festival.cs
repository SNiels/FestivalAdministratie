using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PortableClassLibrary;

namespace PortableClassLibrary.Model
{
    public class Festival : ObservableObject
    {
        #region ctors
        static Festival()
        {
            _festival = new Festival();
        }

        #endregion

        #region props

        private static Festival _festival;

        public static Festival SingleFestival
        {
            get
            {
                return _festival;
            }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }


        private DateTime _startDate;
        public DateTime StartDate
        {
            get { return _startDate; }
            set { _startDate = value;
            OnPropertyChanged("StartDate");
            }
        }

        private DateTime _endDate;
        public DateTime EndDate
        {
            get { return _endDate; }
            set { _endDate = value;
            OnPropertyChanged("EndDate");
            }
        }

        private ObservableCollection<DateTime> _days;
        public ObservableCollection<DateTime> Days
        {
            get
            {
                if (_days != null) return _days;
                _days = new ObservableCollection<DateTime>();
                DateTime currentDay = new DateTime(StartDate.Ticks);
                while (currentDay < EndDate.AddDays(1))
                {
                    _days.Add(new DateTime(currentDay.Ticks));
                    currentDay=currentDay.AddDays(1);
                }
                return _days;
            }
        }

        private ObservableCollection<LineUp> _lineUps=new ObservableCollection<LineUp>();
        public ObservableCollection<LineUp> LineUps
        {
            get
            {
                if (_lineUps == null) ComputeLineUps();
                return _lineUps;
            }
            set
            {
                _lineUps = value;
                OnPropertyChanged("LineUps");
            }
        }

        public Festival ComputeLineUps()
        {
            //LineUps.Clear();
            ObservableCollection<DateTime> days = Days;
            foreach (LineUp lineUp in _lineUps.ToList())//to list omdat de originele LineUps gewijzigd worden in de lus
                if (days.IndexOf(lineUp.Dag) == -1) _lineUps.Remove(lineUp);
            foreach (DateTime day in days)
                if (_lineUps.Where(lineUp => lineUp.Dag == day).Count() < 1) _lineUps.Add(new LineUp() { Dag = day });
            //LineUps.ToList().Sort();
            LineUps = new ObservableCollection<LineUp>(_lineUps.OrderBy(lineUp => lineUp.Dag));
            //LineUps.Add(new LineUp() { Dag = day });
            //if (LineUps.Where(lineUp => lineUp.Dag == day).Count() == 0) LineUps.Add(new LineUp() { Dag = day });
            return this;
        }

        //private ObservableCollection<LineUp> _lineUps= new ObservableCollection<LineUp>();

        public void LineUpsPropertyChanged()
        {
            OnPropertyChanged("LineUps");
        }

        private string _festivalMap;
        public string FestivalMap
        {
            get
            {
                return _festivalMap;
            }
            set
            {
                _festivalMap = value;
                OnPropertyChanged("FestivalMap");
            }
        }


        private ObservableCollection<Band> _bands;

        public ObservableCollection<Band> Bands
        {
            get { return _bands; }
            set
            {
                _bands = value;
                OnPropertyChanged("Bands");
            }
        }

        private ObservableCollection<Stage> _stages;
        public ObservableCollection<Stage> Stages
        {
            get { return _stages; }
            set
            {
                _stages = value;
                OnPropertyChanged("Stages");
            }
        }

        //private ObservableCollection<ContactpersonType> _contactTypes;

        //public ObservableCollection<ContactpersonType> ContactTypes
        //{
        //    get { return _contactTypes; }
        //    set
        //    {
        //        _contactTypes = value;
        //        OnPropertyChanged("ContactTypes");
        //    }
        //}

        //private ObservableCollection<Contactperson> _contactPersons;

        //public ObservableCollection<Contactperson> ContactPersons
        //{
        //    get { return _contactPersons; }
        //    set
        //    {
        //        _contactPersons = value;
        //        OnPropertyChanged("ContactPersons");
        //    }
        //}

        private ObservableCollection<Genre> _genres;

        public ObservableCollection<Genre> Genres
        {
            get { return _genres; }
            set
            {
                _genres = value;
                OnPropertyChanged("Genres");
            }
        }

        //private ObservableCollection<Ticket> _tickets;

        //public ObservableCollection<Ticket> Tickets
        //{
        //    get { return _tickets; }
        //    set
        //    {
        //        _tickets = value;
        //        OnPropertyChanged("Tickets");
        //    }
        //}

        //private ObservableCollection<TicketType> _ticketTypes;

        //public ObservableCollection<TicketType> TicketTypes
        //{
        //    get { return _ticketTypes; }
        //    set
        //    {
        //        _ticketTypes = value;
        //        OnPropertyChanged("TicketTypes");
        //    }
        //}

        private ObservableCollection<Optreden> _optredens;

        public ObservableCollection<Optreden> Optredens
        {
            get { return _optredens; }
            set
            {
                _optredens = value;
                OnPropertyChanged("Optredens");
            }
        }

        #endregion

    }
}
