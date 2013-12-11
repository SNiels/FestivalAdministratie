using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FestivalLibAdmin.Model
{
    public class Festival :ObservableValidationObject
    {
        public static bool ISASP = Process.GetCurrentProcess().ProcessName == "w3wp";

        static Festival()
        {
            ////testingdata
            //StartDate = DateTime.Today.AddDays(-1);
            //EndDate = DateTime.Today.AddDays(2);
            //LineUps = new List<LineUp>();
            //for (int i = 0; i < Days.Count; i++)
            //{
            //    LineUps.Add(
            //        new LineUp()
            //        {
            //            Dag = Days[i],
            //            Stages=new List<Stage>{
            //                new Stage(){
            //                    ID=""+i,
            //                    Name="test",
            //                    StageNumber=i
            //                },
            //                new Stage(){
            //                    ID=""+i,
            //                    Name="test",
            //                    StageNumber=i
            //                }
            //            }
            //            });
            //        }

            _festival = new Festival();
            
            //_festival.StartDate = DateTime.Today.AddDays(-1);
            //_festival.EndDate = DateTime.Today.AddDays(2);
        }

        public Festival()
        {
            Stages = new ObservableCollection<Stage>();
            //Bands = new ObservableCollection<Band>();
            //Genres = new ObservableCollection<Genre>();
            //this.Tickets = new ObservableCollection<Ticket>();
            //this.TicketTypes = new ObservableCollection<TicketType>();
            this.Optredens = new ObservableCollection<Optreden>();
            //ContactPersons = new ObservableCollection<Contactperson>();
            //ContactTypes = new ObservableCollection<ContactpersonType>();
        }

        private static Festival _festival;

        public static Festival SingleFestival
        {
            get
            {
                return _festival;
            }
        }

        private string _name;
        [Required(ErrorMessage = "Gelieve de naam in te vullen")]
        [MinLength(2, ErrorMessage = "Een naam moet minimum 2 karakters zijn.")]
        [Display(Name = "Naam", Order = 0, Description = "De naam van het festival", GroupName = "Festival",Prompt="Bv. Satisfaction")]
        [DisplayFormat(ConvertEmptyStringToNull = true)]
        [Editable(true, AllowInitialValue = false)]
        public string Name
        {
            get { return _name; }
            set { _name = value;
            OnPropertyChanged("Name");
            }
        }
        

        private DateTime _startDate = DateTime.Today;
        [Required(ErrorMessage="Gelieve een startdatum in te geven")]
        [DataType(DataType.Date,ErrorMessage="Gelieve een geldige datum in te geven")]
        [Display(Name = "Start datum", Order = 1, Description ="Datum waarop het festival start", GroupName = "Festival")]
        public DateTime StartDate
        {
            get { return _startDate; }
            set
            {
                _startDate = value;
                OnPropertyChanged("StartDate");
            }
        }

        private DateTime _endDate = DateTime.Today.AddDays(1);
        [Required(ErrorMessage = "Gelieve een einddatum in te geven")]
        [DataType(DataType.Date, ErrorMessage = "Gelieve een geldige datum in te geven")]
        [Display(Name = "Eind datum", Order = 2, Description = "Datum waarop het festival eindigt", GroupName = "Festival")]
        public DateTime EndDate
        {
            get { return _endDate; }
            set
            {
                _endDate = value;
                OnPropertyChanged("EndDate");
            }
        }

        public virtual ObservableCollection<DateTime> Days
        {
            get
            {
                ObservableCollection<DateTime> days = new ObservableCollection<DateTime>();
                DateTime currentDay = new DateTime(StartDate.Ticks);
                while (currentDay < EndDate.AddDays(1))
                {
                    days.Add(new DateTime(currentDay.Ticks));
                    currentDay = currentDay.AddDays(1);
                }
                return days;
            }
        }

        private ObservableCollection<LineUp> _lineUps = new ObservableCollection<LineUp>();

        public virtual ObservableCollection<LineUp> LineUps
        {
            get
            {
                return _lineUps;
            }
            private set
            {
                _lineUps = value;
                OnPropertyChanged("LineUps");
            }
        }

        public void ComputeLineUps()
        {
            //LineUps.Clear();
            ObservableCollection<DateTime> days = Days;
            foreach (LineUp lineUp in LineUps.ToList())//to list omdat de originele LineUps gewijzigd worden in de lus
                if (days.IndexOf(lineUp.Dag) == -1) LineUps.Remove(lineUp);
            foreach (DateTime day in days)
                if (LineUps.Where(lineUp => lineUp.Dag == day).Count() < 1) LineUps.Add(new LineUp() { Dag = day });
            //LineUps.ToList().Sort();
            LineUps = new ObservableCollection<LineUp>(LineUps.OrderBy(lineUp => lineUp.Dag));
            //LineUps.Add(new LineUp() { Dag = day });
            //if (LineUps.Where(lineUp => lineUp.Dag == day).Count() == 0) LineUps.Add(new LineUp() { Dag = day });
        }

        //private ObservableCollection<LineUp> _lineUps= new ObservableCollection<LineUp>();

        public void LineUpsPropertyChanged()
        {
            OnPropertyChanged("LineUps");
        }

        private string _festivalMap;
        [RegularExpression(@"^(?=[^&])(?:(?<scheme>[^:/?#]+):)?(?://(?<authority>[^/?#]*))?(?<path>[^?#]*)(?:\?(?<query>[^#]*))?(?:#(?<fragment>.*))?", ErrorMessage = "Gelieve een geldige url te geven")]
        [DataType(DataType.ImageUrl)]
        [Url(ErrorMessage="Gelieve een geldige url in te geven")]
        [FileExtensions(ErrorMessage = "Gelieve een geldige image in te geven (.jpg, .jpeg, .gif or .png)", Extensions = "jpg,jpeg,gif,png")]
        [Display(Name = "Festivalmap", Order = 3, Description = "Een map van het festival", GroupName = "Festival")]
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
            get {
                if (_bands == null) Bands = Band.GetBands();
                return _bands; 
                }

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

        private ObservableCollection<ContactpersonType> _contactTypes;

        public ObservableCollection<ContactpersonType> ContactTypes
        {
            get {
                if (_contactTypes == null) ContactTypes = ContactpersonType.GetContactTypes();
                return _contactTypes; }
            set
            {
                _contactTypes = value;
                OnPropertyChanged("ContactTypes");
            }
        }

        private ObservableCollection<Contactperson> _contactPersons;

        public ObservableCollection<Contactperson> ContactPersons
        {
            get { 
                if(_contactPersons==null)ContactPersons = Contactperson.GetContacts();
                return _contactPersons; }
            set
            {
                _contactPersons = value;
                OnPropertyChanged("ContactPersons");
            }
        }

        private ObservableCollection<Genre> _genres;

        public ObservableCollection<Genre> Genres
        {
            get {
                if (_genres == null) Genres = Genre.GetGenres();
                return _genres; }
            set
            {
                _genres = value;
                OnPropertyChanged("Genres");
            }
        }

        private ObservableCollection<Ticket> _tickets;

        public ObservableCollection<Ticket> Tickets
        {
            get {
                if (_tickets == null) Tickets = Ticket.GetTickets();
                return _tickets; }
            set
            {
                _tickets = value;
                OnPropertyChanged("Tickets");
            }
        }

        private ObservableCollection<TicketType> _ticketTypes;

        public ObservableCollection<TicketType> TicketTypes
        {
            get {
                if (_ticketTypes == null) TicketTypes = TicketType.GetTypes();
                return _ticketTypes; }
            set
            {
                _ticketTypes = value;
                OnPropertyChanged("TicketTypes");
            }
        }

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
    }
}
