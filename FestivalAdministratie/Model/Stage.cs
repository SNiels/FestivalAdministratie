using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FestivalAdministratie.Model
{
    public class Stage : ObservableValidationObject
    {

        public Stage()
        {
            Name = "Nieuwe stage";
            //Performances = new List<Optreden>();
            //Optreden perf = new Optreden()
            //{
            //    Band = new Band() { Name = "Arctic Monkeys" },
            //    From = 2,
            //    Until = 3,
            //    Stage = this,
            //    ID = "" + 1
            //};
            //Performances.Add(perf);

            //perf = new Optreden()
            //{
            //    Band = new Band() { Name = "Arctic Monkeys" },
            //    From = 4,
            //    Until = 6,
            //    Stage = this,
            //    ID = "" + 1
            //};
            //Performances.Add(perf);
        }

        //public void ComputeShit()
        //{
        //    foreach (Optreden perf in Performances)
        //    {
        //        perf.LeftPositionPercent = perf.From / (LineUp.MaxHour - LineUp.MinHour);
        //        perf.WidthPercent = (perf.Until-perf.From) / (LineUp.MaxHour - LineUp.MinHour);
        //    }
        //}

        public DateTime MinHour
        {
            get
            {
                if (Performances.Count <= 0) return DateTime.Now;
                DateTime min = Performances[0].From;
                foreach (Optreden perf in Performances)
                    if (perf.From < min) min = perf.From;
                return min;
            }
        }

        public DateTime MaxHour
        {
            get
            {
                if (Performances.Count <= 0) return DateTime.Now;
                DateTime max = Performances[0].Until;
                foreach (Optreden perf in Performances)
                    if (perf.Until > max) max = perf.Until;
                return max;
            }
        }

        static Stage()
        {
            Stages = new ObservableCollection<Stage>{
                new Stage(){Name="main"},new Stage(){Name="second"}
            };
            //Optreden.Optredens.CollectionChanged += Optredens_CollectionChanged;
            
        }

        //static void Optredens_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        //{
        //    foreach (Optreden optreden in e.NewItems)
        //        optreden.PropertyChanged += optreden_PropertyChanged;
        //}

        //static void optreden_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        //{
        //    if(e.PropertyName=="Stage")
        //    {
        //        foreach (Stage stage in Stage.Stages)
        //            stage.Performances = stage.ExplicitPerformances();
        //    }
        //}

        private static ObservableCollection<Stage> _stages;

        public static ObservableCollection<Stage> Stages
        {
            get { return _stages; }
            set { _stages = value; }
        }
        

        private string _id;

        public string ID
        {
            get { return _id; }
            set { _id = value; }
        }

        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value;
            OnPropertyChanged("Name");
            }
        }

        private int _stageNumber;

        public int StageNumber
        {
            get { return _stageNumber; }
            set { _stageNumber = value; }
        }

        private string _logo;

        public string Logo
        {
            get { return _logo; }
            set { _logo = value; }
        }


        //private ObservableCollection<Optreden> _performances;

        public ObservableCollection<Optreden> Performances
        {
            get {
                //if (_performances == null)
                //{
                    return ExplicitPerformances();
                    //OnPropertyChanged("Performances");
            //    }
            //    return _performances;
            //}
            //set { _performances = value;
            //OnPropertyChanged("Performances");
            }
        }

        private ObservableCollection<Optreden> ExplicitPerformances()
        {
                ObservableCollection<Optreden> optredens = new ObservableCollection<Optreden>(Festival.SingleFestival.Optredens.Where(optreden => optreden.Stage == this));
                //foreach(Optreden optreden in optredens)
                //    optreden.PropertyChanged += optreden_PropertyChanged;
                return optredens;
        } 

        public void OnOptredensChanged()
        {
            OnPropertyChanged("Performances");
        }

        //void optreden_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        //{
        //    if (e.PropertyName == "From")
        //        OnPropertyChanged("MinHour");
        //    else if (e.PropertyName == "Until")
        //        OnPropertyChanged("MaxHour");
        //    //else if (e.PropertyName == "Stage")
        //    //    OnPropertyChanged("Performances");
        //}

        //private LineUp _lineUp;

        //public LineUp LineUp
        //{
        //    get { return _lineUp; }
        //    set { _lineUp = value; }
        //}

        private int _xCoordinaat;

        public int XCoordinaat
        {
            get { return _xCoordinaat; }
            set { _xCoordinaat = value; }
        }

        private int _yCoordinaat;

        public int YCoordinaat
        {
            get { return _yCoordinaat; }
            set { _yCoordinaat = value; }
        }

        public override string ToString()
        {
            return Name;
        }

        public static DateTime GetMinHourByLineUp(LineUp lineUp)
        {

            if (Stages.Count <= 0) return DateTime.Now;
            DateTime min=GetFirstMinHour(lineUp);
            foreach (Stage stage in Stages)
                foreach (Optreden optreden in stage.Performances)
                    if (optreden.LineUp==lineUp&&optreden.From < min) min = optreden.From;
            //DateTime min = Stages[0].MinHour;
            //foreach (Stage stage in Stages)
            //    if (stage.MinHour < min) min = stage.MinHour;

            return new DateTime(min.Year, min.Month, min.Day, min.Hour, 0, 0);
        }

        private static DateTime GetFirstMinHour(LineUp lineUp)
        {
            foreach (Stage stage in Stages)
                foreach (Optreden optreden in stage.Performances)
                    if (optreden.LineUp == lineUp) return optreden.From;
            return DateTime.Now;
        }

        public static DateTime GetMaxHourByLineUp(LineUp lineUp)
        {

            if (Stages.Count <= 0) return DateTime.Now;
            DateTime max = GetFirstMaxHour(lineUp);
            foreach (Stage stage in Stages)
                foreach (Optreden optreden in stage.Performances)
                    if (optreden.LineUp == lineUp && optreden.Until > max) max = optreden.Until;
            //DateTime min = Stages[0].MinHour;
            //foreach (Stage stage in Stages)
            //    if (stage.MinHour < min) min = stage.MinHour;

            return new DateTime(max.Year, max.Month, max.Day, max.Hour, 0, 0).AddHours(1);
        }

        private static DateTime GetFirstMaxHour(LineUp lineUp)
        {
            foreach (Stage stage in Stages)
                foreach (Optreden optreden in stage.Performances)
                    if (optreden.LineUp == lineUp) return optreden.Until;
            return DateTime.Now;
        }
    }
}
