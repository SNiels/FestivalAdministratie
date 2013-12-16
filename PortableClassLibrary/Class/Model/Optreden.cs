using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortableClassLibrary.Model
{
    public class Optreden:ObservableObject
    {
        public Optreden()
        {

        }

        private double CalculateLeftPercent()
        {
            TimeSpan aantalUren = LineUp.MaxHour - LineUp.MinHour;
            return (From-LineUp.MinHour).TotalMinutes /aantalUren.TotalMinutes;
        }

        private double CalculateWidthPercent()
        {
            TimeSpan aantalUren = LineUp.MaxHour - LineUp.MinHour;
            return (this.Until - From).TotalMinutes/aantalUren.TotalMinutes;
        }

        private string _id;

        public string ID
        {
            get { return _id; }
            set { _id = value; }
        }

        private DateTime _from;

        public DateTime From
        {
            get
            {
                    if (LineUp != null && LineUp.Dag!=null && _from != null) return new DateTime(LineUp.Dag.Year, LineUp.Dag.Month, LineUp.Dag.Day, _from.Hour, _from.Minute, 0);
                return _from;
            }
            set { _from = value;
            OnPropertyChanged("From");
            OnPropertyChanged("LeftPositionPercent");
            OnPropertyChanged("WidthPercent");
            if (LineUp != null)
            LineUp.OnHoursChanged();
            }
        }

        private DateTime _until;

        public DateTime Until
        {
            get
            {
                if (LineUp != null && LineUp.Dag!=null && _until != null) return new DateTime(LineUp.Dag.Year, LineUp.Dag.Month, LineUp.Dag.Day, _until.Hour, _until.Minute, 0);
                return _until;
            }
            set { _until = value; OnPropertyChanged("Until");
            OnPropertyChanged("LeftPositionPercent");
            OnPropertyChanged("WidthPercent");
                if(LineUp!=null)
            LineUp.OnHoursChanged();
            }
        }

        private LineUp _lineUp;

        public LineUp LineUp
        {
            get { return _lineUp; }
            set { _lineUp = value;
            OnPropertyChanged("LineUp");
                if(value!=null)
            LineUp.PropertyChanged += LineUp_PropertyChanged;
                if (Stage != null)
                    Stage.OnOptredensChanged();
            }
        }

        void LineUp_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != "Hours") return;
                OnPropertyChanged("LeftPositionPercent");
                OnPropertyChanged("WidthPercent");
        }

        private Stage _stage;

        public Stage Stage
        {
            get { return _stage; }
            set {
                Stage oldStage = _stage;
                _stage = value;
            OnPropertyChanged("Stage");
            OnPropertyChanged("FriendlyName");
            if (value != null)
                value.OnOptredensChanged();
            if (oldStage != null)
                oldStage.OnOptredensChanged();
            }

        }

        private Band _band;

        public virtual Band Band
        {
            get { return _band; }
            set { _band = value;
            OnPropertyChanged("Band");
            OnPropertyChanged("FriendlyName");
            }
        }

        public double LeftPositionPercent
        {
            get {
                    return CalculateLeftPercent();
            }
        }

        public double WidthPercent
        {
            get {
                    return CalculateWidthPercent();
            }
        }

        public string FriendlyName
        {
            get { return ToString(); }
        }

        public override string ToString()
        {
            if (Band == null && Stage == null) return "Nieuw optreden";
            return Band + " - " + Stage;
        }
    }
}
