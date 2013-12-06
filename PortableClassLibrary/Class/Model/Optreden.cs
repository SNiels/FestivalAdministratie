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
        static Optreden()
        {
            Optredens = new ObservableCollection<Optreden>();
            #region
            //Optredens.Add(
            //    new Optreden()
            //    {
            //        Band = new Band() { Name = "ar" },
            //        From = DateTime.Now.AddHours(-1),
            //        Until = DateTime.Now.AddHours(1),
            //        Stage = Stage.Stages.First(),
            //        LineUp = LineUp.LineUps.First()
                    
            //    });
            //Optredens.Add(
            //    new Optreden()
            //    {
            //        Band = new Band() { Name = "dlfjsdlmf" },
            //        From = DateTime.Now.AddHours(1),
            //        Until = DateTime.Now.AddHours(2),
            //        Stage=Stage.Stages.First(),
            //        LineUp=LineUp.LineUps.First()
            //    });
            //Optredens.CollectionChanged += Optredens_CollectionChanged;
            #endregion
        }
        #region
        //static void Optredens_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        //{
        //    foreach (Stage stage in Stage.Stages)
        //        stage.OnOptredensChanged();
        //}

        //public static void NewOptreden()
        //{
        //    Optreden optreden = new Optreden();
        //    Optredens.Add(optreden);
        //    //if (LineUp.LineUps.Count > 0)
        //    //{
        //    //    optreden.LineUp = LineUp.LineUps.First();
        //    //    optreden.From = LineUp.LineUps.First().MinHour;
        //    //    optreden.Until = LineUp.LineUps.First().MaxHour;
        //    //}
        //    //if(Band.Bands.Count>0)
        //    //optreden.Band = Band.Bands.First();
        //    //if (Stage.Stages.Count > 0)
        //    //    optreden.Stage = Stage.Stages.First();
        //}
        #endregion
        //public Optreden()
        //{
        //    #region
        //    //if (LineUp.LineUps.Count > 0)
        //    //{
        //    //    LineUp = LineUp.LineUps.First();
        //    //    From = LineUp.LineUps.First().MinHour;
        //    //    Until = LineUp.LineUps.First().MaxHour;
        //    //}

        //    //if(Band.Bands.Count>0)
        //    //Band = Band.Bands.First();
        //    //if (Stage.Stages.Count > 0)
        //    //    Stage = Stage.Stages.First();
        //    #endregion
        //}

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
            get { return _from; }
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
            get { return _until; }
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

        public Band Band
        {
            get { return _band; }
            set { _band = value;
            OnPropertyChanged("Band");
            OnPropertyChanged("FriendlyName");
            }
        }

        //private double _positionLeft;

        public double LeftPositionPercent
        {
            get {
                //try
                //{
                    return CalculateLeftPercent();
                //}
                //catch (Exception e)
                //{
                //    Console.WriteLine(e.Message);
                //    return 0;
                //}
            }
            //set
            //{
            //    _positionLeft = value;
            //    OnPropertyChanged("LeftPositionPercent");
            //}
        }

        //private double _widthPercent;

        public double WidthPercent
        {
            get {
                //try
                //{
                    return CalculateWidthPercent();
                //}
                //catch (Exception e)
                //{
                //    Console.WriteLine(e.Message);
                //    return 0;
                //}
                
            }
            //set
            //{
            //    _widthPercent = value;
            //    OnPropertyChanged("WidthPercent");
            //}
        }

        private static ObservableCollection<Optreden> _optredens;

        public static ObservableCollection<Optreden> Optredens
        {
            get { return _optredens; }
            set { _optredens = value; }
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
