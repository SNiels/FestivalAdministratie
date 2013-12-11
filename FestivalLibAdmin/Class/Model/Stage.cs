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
    public class Stage : ObservableValidationObject
    {

        public Stage()
        {
            Name = "Nieuwe stage";
            #region
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
            #endregion
        }

        //public void ComputeShit()
        //{
        //    foreach (Optreden perf in Performances)
        //    {
        //        perf.LeftPositionPercent = perf.From / (LineUp.MaxHour - LineUp.MinHour);
        //        perf.WidthPercent = (perf.Until-perf.From) / (LineUp.MaxHour - LineUp.MinHour);
        //    }
        //}
        [ScaffoldColumn(false)]
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
        [ScaffoldColumn(false)]
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

        //static Stage()
        //{
        //    Stages = new ObservableCollection<Stage>{
        //        new Stage(){Name="main"},new Stage(){Name="second"}
        //    };
        //    //Optreden.Optredens.CollectionChanged += Optredens_CollectionChanged;

        //}

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

        //private static ObservableCollection<Stage> _stages;

        //public static ObservableCollection<Stage> Stages
        //{
        //    get { return _stages; }
        //    set { _stages = value; }
        //}


        private string _id;
        [ScaffoldColumn(false)]
        public string ID
        {
            get { return _id; }
            set { _id = value; }
        }

        private string _name;
        [Required(ErrorMessage="Gelieve een naam in te geven")]
        [MinLength(2,ErrorMessage="De naam moet minstens 2 letters bevatten")]
        [Display(Name = "Naam", Order = 0, Description = "De naam van de stage", GroupName = "Stage",Prompt="Bv: Main stage")]
        [DisplayFormat(ConvertEmptyStringToNull = true)]
        [Editable(true, AllowInitialValue = false)]
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        private int _stageNumber;
        [Range(0,int.MaxValue,ErrorMessage="Het stage nummer mag niet negatief zijn")]
        [Display(Name = "Stage nummer", Order = 1, Description = "Het nummer van de stage, deze wordt gerespecteerd voor sortering", GroupName = "Stage",Prompt="Bv: 2")]
        [DisplayFormat(ConvertEmptyStringToNull = true)]
        public int StageNumber
        {
            get { return _stageNumber; }
            set
            {
                _stageNumber = value;
                OnPropertyChanged("StageNumber");
            }
        }

        private string _logo;
        [RegularExpression(@"^(?=[^&])(?:(?<scheme>[^:/?#]+):)?(?://(?<authority>[^/?#]*))?(?<path>[^?#]*)(?:\?(?<query>[^#]*))?(?:#(?<fragment>.*))?",ErrorMessage="Gelieve een geldige url te geven")]
        [DataType(DataType.ImageUrl,ErrorMessage="Gelieve een geldige link naar de foto te geven")]
        [Display(Name = "Logo", Order = 2, Description = "Het logo van de stage", GroupName = "Stage",Prompt="Geef een url naar het logo in")]
        public string Logo
        {
            get { return _logo; }
            set
            {
                _logo = value;
                OnPropertyChanged("Logo");
            }
        }


        //private ObservableCollection<Optreden> _performances;

        public ObservableCollection<Optreden> Performances
        {
            get
            {
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
        
        private decimal _xCoordinaat;
        [Range(0, 100, ErrorMessage = "Het coördinaat moet tussen de 0 en 100 liggen")]
        [Display(Name = "X coördinaat", Order = 4, Description = "X coördinaat van de stage, dit is een percentage", GroupName = "Stage", Prompt = "Bv. 61.4")]
        [DisplayFormat(ConvertEmptyStringToNull = true)]
        public decimal XCoordinaat
        {
            get { return _xCoordinaat; }
            set
            {
                _xCoordinaat = value;
                OnPropertyChanged("XCoordinaat");
            }
        }

        private decimal _yCoordinaat;
        [Range(0, 100, ErrorMessage = "Het coördinaat moet tussen de 0 en 100 liggen")]
        [Display(Name = "Y coördinaat", Order = 5, Description = "Y coördinaat van de stage, dit is een percentage", GroupName = "Stage",Prompt="Bv. 61.4")]
        [DisplayFormat(ConvertEmptyStringToNull = true)]
        public decimal YCoordinaat
        {
            get { return _yCoordinaat; }
            set
            {
                _yCoordinaat = value;
                OnPropertyChanged("YCoordinaat");
            }
        }

        public override string ToString()
        {
            return Name;
        }

        public static DateTime GetMinHourByLineUp(LineUp lineUp)
        {

            if (Festival.SingleFestival.Stages.Count <= 0) return DateTime.Now;
            DateTime min = GetFirstMinHour(lineUp);
            foreach (Stage stage in Festival.SingleFestival.Stages)
                foreach (Optreden optreden in stage.Performances)
                    if (optreden.LineUp == lineUp && optreden.From < min) min = optreden.From;
            //DateTime min = Stages[0].MinHour;
            //foreach (Stage stage in Stages)
            //    if (stage.MinHour < min) min = stage.MinHour;

            return new DateTime(min.Year, min.Month, min.Day, min.Hour, 0, 0);
        }

        private static DateTime GetFirstMinHour(LineUp lineUp)
        {
            foreach (Stage stage in Festival.SingleFestival.Stages)
                foreach (Optreden optreden in stage.Performances)
                    if (optreden.LineUp == lineUp) return optreden.From;
            return DateTime.Now;
        }

        public static DateTime GetMaxHourByLineUp(LineUp lineUp)
        {

            if (Festival.SingleFestival.Stages.Count <= 0) return DateTime.Now;
            DateTime max = GetFirstMaxHour(lineUp);
            foreach (Stage stage in Festival.SingleFestival.Stages)
                foreach (Optreden optreden in stage.Performances)
                    if (optreden.LineUp == lineUp && optreden.Until > max) max = optreden.Until;
            //DateTime min = Stages[0].MinHour;
            //foreach (Stage stage in Stages)
            //    if (stage.MinHour < min) min = stage.MinHour;

            return new DateTime(max.Year, max.Month, max.Day, max.Hour, 0, 0).AddHours(1);
        }

        private static DateTime GetFirstMaxHour(LineUp lineUp)
        {
            foreach (Stage stage in Festival.SingleFestival.Stages)
                foreach (Optreden optreden in stage.Performances)
                    if (optreden.LineUp == lineUp) return optreden.Until;
            return DateTime.Now;
        }
    }
}
