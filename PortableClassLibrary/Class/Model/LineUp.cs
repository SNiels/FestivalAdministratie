using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortableClassLibrary.Model
{
    public class LineUp : ObservableObject
    {
        private DateTime _dag;
        
        public virtual DateTime Dag
        {
            get { return _dag; }
            set {
                if (value > Festival.SingleFestival.EndDate || value < Festival.SingleFestival.StartDate) throw new Exception("The date for a Lineup must be between the start and ending of the festival");
                _dag = value;
            }
        }

        public ObservableCollection<Stage> Stages
        {
            get
            {
                return Festival.SingleFestival.Stages;
            }
        }

        public virtual DateTime MinHour
        {
            get
            {
                return Stage.GetMinHourByLineUp(this);
            }
        }

        public virtual DateTime MaxHour
        {
            get
            {
                return Stage.GetMaxHourByLineUp(this);
            }
        }

        public ObservableCollection<DateTime> Hours
        {
            get
            {
                DateTime min = MinHour;
                DateTime max = MaxHour;
                ObservableCollection<DateTime> hours = new ObservableCollection<DateTime>();
                if ((max - min) > TimeSpan.FromDays(5)) return hours;//something is wrong with the datetimes if the span is bigger than 5 days
                for (DateTime i = MinHour; i <= MaxHour; i=i.AddHours(1))
                    hours.Add(i);
                return hours;
            }
        }

        public void OnHoursChanged()
        {
            OnPropertyChanged("Hours");
        }

        public override string ToString()
        {
            return Dag.ToString("d MMMM");
        }
    }
}
