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
    public class LineUp : ObservableValidationObject
    {
        private DateTime _dag;
        [Display(Name = "Dag", Order = 0, Description = "De dag van de line-up", GroupName = "Line-up")]
        [Editable(false)]
        public DateTime Dag
        {
            get { return _dag; }
            set
            {
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

        [Display(Name = "Start line-up", Order = 1, Description = "Het start tijdstip van de line up", GroupName = "Line-up")]
        [Editable(false)]
        public DateTime MinHour
        {
            get
            {
                return Stage.GetMinHourByLineUp(this);
            }
        }
        [Display(Name = "Einde line-up", Order = 2, Description = "Het eind tijdstip van de line up", GroupName = "Line-up")]
        [Editable(false)]
        public DateTime MaxHour
        {
            get
            {
                return Stage.GetMaxHourByLineUp(this);
                //if (Stages.Count <= 0) return DateTime.Now;
                //DateTime max = Stages[0].MaxHour;
                //foreach (Stage stage in Stages)
                //    if (stage.MaxHour > max) max = stage.MaxHour;
                //return new DateTime(max.Year, max.Month, max.Day, max.Hour,0,0).AddHours(1);
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
                for (DateTime i = MinHour; i <= MaxHour; i = i.AddHours(1))
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
