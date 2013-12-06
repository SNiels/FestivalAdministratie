using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FestivalLibAdmin.Model
{
    public class Festival : ObservableValidationObject
    {
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

        private static Festival _festival;

        public static Festival SingleFestival
        {
            get
            {
                return _festival;
            }
        }

        private DateTime _startDate=DateTime.Today;

        public DateTime StartDate
        {
            get { return _startDate; }
            set { _startDate = value;
            OnPropertyChanged("StartDate");
            }
        }

        private DateTime _endDate=DateTime.Today.AddDays(1);

        public DateTime EndDate
        {
            get { return _endDate; }
            set { _endDate = value;
            OnPropertyChanged("EndDate");
            }
        }

        public ObservableCollection<DateTime> Days
        {
            get
            {
                ObservableCollection<DateTime> days = new ObservableCollection<DateTime>();
                DateTime currentDay = new DateTime(StartDate.Ticks);
                while (currentDay < EndDate.AddDays(1))
                {
                    days.Add(new DateTime(currentDay.Ticks));
                    currentDay=currentDay.AddDays(1);
                }
                return days;
            }
        }

        public void ComputeLineUps()
        {
            //LineUps.Clear();
            ObservableCollection<DateTime> days= Days;
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

        //binding to the singleton festival propperties but still getting them static from the right class
        //so that I can call OnProppertyChanged and not have to think about calling onproppertychanged from the viewmodels
        //getting and setting from here

        public ObservableCollection<LineUp> LineUps
        {
            get { return LineUp.LineUps; }
            set
            {
                LineUp.LineUps = value;
                OnPropertyChanged("LineUps");
            }
        }

        public ObservableCollection<Band> Bands
        {
            get { return Band.Bands; }
            set
            {
                Band.Bands = value;
                OnPropertyChanged("Bands");
            }
        }

        public ObservableCollection<Stage> Stages
        {
            get { return Stage.Stages; }
            set
            {
                Stage.Stages = value;
                OnPropertyChanged("Stages");
            }
        }

        public ObservableCollection<ContactpersonType> ContactTypes
        {
            get { return ContactpersonType.Types; }
            set
            {
                ContactpersonType.Types = value;
                OnPropertyChanged("ContactTypes");
            }
        }

        public ObservableCollection<Contactperson> ContactPersons
        {
            get { return Contactperson.Contacten; }
            set
            {
                Contactperson.Contacten= value;
                OnPropertyChanged("ContactPersons");
            }
        }

        public ObservableCollection<Genre> Genres
        {
            get { return Genre.Genres; }
            set
            {
                Genre.Genres= value;
                OnPropertyChanged("Genres");
            }
        }

        public ObservableCollection<Ticket> Tickets
        {
            get { return Ticket.Tickets; }
            set
            {
                Ticket.Tickets = value;
                OnPropertyChanged("Tickets");
            }
        }

        public ObservableCollection<TicketType> TicketTypes
        {
            get { return TicketType.Types; }
            set
            {
                TicketType.Types = value;
                OnPropertyChanged("TicketTypes");
            }
        }

        public ObservableCollection<Optreden> Optredens
        {
            get { return Optreden.Optredens; }
            set
            {
                Optreden.Optredens = value;
                OnPropertyChanged("Optredens");
            }
        }
    }
}
