using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FestivalLibAdmin.Model
{
    public class LineUp : ObservableValidationObject
    {
        //static LineUp()
        //{
        //    //LineUps = new ObservableCollection<Model.LineUp>{
        //    //    new Model.LineUp(){Dag=DateTime.Today},
        //    //    new Model.LineUp(){Dag=DateTime.Today.AddDays(1)}
        //    //};
        //}
        private DateTime _dag;
        
        public DateTime Dag
        {
            get { return _dag; }
            set {
                if (value > Festival.SingleFestival.EndDate || value < Festival.SingleFestival.StartDate) throw new ApplicationException("The date for a Lineup must be between the start and ending of the festival");
                _dag = value;
            }
        }

        private static ObservableCollection<LineUp> _lineUps=new ObservableCollection<LineUp>();
        public static ObservableCollection<LineUp> LineUps
        {
            get { return _lineUps; }
            set { _lineUps = value; }
        }
        #region
        //private List<Stage> _stages;

        public ObservableCollection<Stage> Stages
        {
            get
            {
                return Festival.SingleFestival.Stages;
            }
            //set { _stages = value; }
        }

        //void stage_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        //{
        //    if (e.PropertyName == "MinHour")
        //    {
        //        OnPropertyChanged("MinHour");
        //        OnPropertyChanged("Hours");
        //    }
                
        //    else if (e.PropertyName == "Maxhour")
        //    { 
        //        OnPropertyChanged("MaxHour");
        //        OnPropertyChanged("Hours");
        //    }
        //}
        #endregion
        public DateTime MinHour
        {
            get
            {
                return Stage.GetMinHourByLineUp(this);
                //if (Stages.Count <= 0) return DateTime.Now;
                //DateTime min = Stages[0].MinHour;
                //foreach (Stage stage in Stages)
                //    if (stage.MinHour < min) min = stage.MinHour;

                //return new DateTime(min.Year, min.Month, min.Day, min.Hour,0,0);
            }
        }

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
                ObservableCollection<DateTime> hours = new ObservableCollection<DateTime>();
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
